namespace Defra.Imports.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios.Config;
    using Defra.Imports.Scenarios.Extensions;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Messages;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>
    /// Applies, removes and verifies persona configuration on Dataverse users: their business unit, and associated/disassociated security roles, teams and column security profiles.
    /// </summary>
    public sealed class PersonaConfigurationService : IPersonaConfigurationService
    {
        private readonly ServiceClient serviceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonaConfigurationService"/> class.
        /// </summary>
        /// <param name="serviceClient">The service client used to apply, remove and verify persona configuration.</param>
        public PersonaConfigurationService(ServiceClient serviceClient)
        {
            this.serviceClient = serviceClient ?? throw new ArgumentNullException(nameof(serviceClient));
        }

        /// <inheritdoc/>
        public async Task ApplyAsync(string username, IEnumerable<PersonaConfiguration> personas)
        {
            var personaList = personas.ToList();
            var businessUnitNames = personaList.Select(p => p.BusinessUnit).Where(bu => !string.IsNullOrEmpty(bu)).Distinct().ToList();

            if (businessUnitNames.Count > 1)
            {
                throw new InvalidOperationException($"The requested personas describe conflicting business units for user '{username}': {string.Join(", ", businessUnitNames)}.");
            }

            var roleNames = personaList.SelectMany(p => p.Roles ?? Enumerable.Empty<string>()).Distinct().ToList();
            var teamNames = personaList.SelectMany(p => p.Teams ?? Enumerable.Empty<string>()).Distinct().ToList();
            var columnSecurityProfileNames = personaList.SelectMany(p => p.ColumnSecurityProfiles ?? Enumerable.Empty<string>()).Distinct().ToList();

            var userId = await this.serviceClient.RetrieveUserIdAsync(username).ConfigureAwait(false);

            var businessUnitName = businessUnitNames.SingleOrDefault();
            if (!string.IsNullOrEmpty(businessUnitName))
            {
                var businessUnitReference = await RetrieveBusinessUnitReferenceAsync(this.serviceClient, businessUnitName).ConfigureAwait(false);

                await this.serviceClient.UpdateAsync(new SystemUser
                {
                    Id = userId,
                    BusinessUnitId = businessUnitReference,
                }).ConfigureAwait(false);
            }

            await AssociateAsync(this.serviceClient, userId, "role", "roleid", "name", roleNames, "systemuserroles_association").ConfigureAwait(false);
            await AssociateAsync(this.serviceClient, userId, "team", "teamid", "name", teamNames, "teammembership_association").ConfigureAwait(false);
            await AssociateAsync(this.serviceClient, userId, "fieldsecurityprofile", "fieldsecurityprofileid", "name", columnSecurityProfileNames, "systemuserprofiles_association").ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string username)
        {
            var userId = await this.serviceClient.RetrieveUserIdAsync(username).ConfigureAwait(false);

            await DisassociateAllAsync(this.serviceClient, userId, "systemuserroles", "role", "roleid", "systemuserroles_association").ConfigureAwait(false);
            await DisassociateAllAsync(this.serviceClient, userId, "teammembership", "team", "teamid", "teammembership_association").ConfigureAwait(false);
            await DisassociateAllAsync(this.serviceClient, userId, "systemuserprofiles", "fieldsecurityprofile", "fieldsecurityprofileid", "systemuserprofiles_association").ConfigureAwait(false);

            var defaultBusinessUnit = await RetrieveDefaultBusinessUnitAsync(this.serviceClient).ConfigureAwait(false);

            await this.serviceClient.UpdateAsync(new SystemUser
            {
                Id = userId,
                BusinessUnitId = defaultBusinessUnit.ToEntityReference(),
            }).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task VerifyStaticAssignmentsAsync(IDictionary<Persona, PersonaConfiguration> personaConfigurations)
        {
            if (personaConfigurations is null)
            {
                throw new ArgumentNullException(nameof(personaConfigurations));
            }

            // A user explicitly listed under more than one persona's `Users` is expected to carry the combined
            // configuration of every one of those personas, matching how `UserPoolService` resolves such a user
            // only when every one of its assigned personas is requested together.
            var assignments = personaConfigurations
                .Where(p => p.Value?.Users != null)
                .SelectMany(p => p.Value.Users.Select(username => (Username: username, Persona: p.Key, Configuration: p.Value)))
                .GroupBy(a => a.Username, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (assignments.Count == 0)
            {
                return;
            }

            string defaultBusinessUnitName = null;
            var failures = new List<string>();

            foreach (var assignment in assignments)
            {
                var username = assignment.Key;
                var personas = assignment.Select(a => a.Persona).ToList();
                var configurations = assignment.Select(a => a.Configuration).ToList();

                var expectedRoles = configurations.SelectMany(c => c.Roles ?? Enumerable.Empty<string>()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                var expectedTeams = configurations.SelectMany(c => c.Teams ?? Enumerable.Empty<string>()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                var expectedColumnSecurityProfiles = configurations.SelectMany(c => c.ColumnSecurityProfiles ?? Enumerable.Empty<string>()).Distinct(StringComparer.OrdinalIgnoreCase).ToList();
                var expectedBusinessUnit = configurations.Select(c => c.BusinessUnit).FirstOrDefault(bu => !string.IsNullOrEmpty(bu));

                Guid userId;

                try
                {
                    userId = await this.serviceClient.RetrieveUserIdAsync(username).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    failures.Add($"user '{username}' (persona(s): {string.Join(", ", personas)}): {ex.Message}");
                    continue;
                }

                var actualRoles = await RetrieveAssignedNamesAsync(this.serviceClient, userId, "systemuserroles", "role", "roleid", "name").ConfigureAwait(false);
                var actualTeams = await RetrieveAssignedNamesAsync(this.serviceClient, userId, "teammembership", "team", "teamid", "name").ConfigureAwait(false);
                var actualColumnSecurityProfiles = await RetrieveAssignedNamesAsync(this.serviceClient, userId, "systemuserprofiles", "fieldsecurityprofile", "fieldsecurityprofileid", "name").ConfigureAwait(false);
                var actualBusinessUnit = await RetrieveUserBusinessUnitNameAsync(this.serviceClient, userId).ConfigureAwait(false);

                var missingRoles = expectedRoles.Where(r => !actualRoles.Contains(r)).ToList();
                var missingTeams = expectedTeams.Where(t => !actualTeams.Contains(t)).ToList();
                var missingColumnSecurityProfiles = expectedColumnSecurityProfiles.Where(p => !actualColumnSecurityProfiles.Contains(p)).ToList();

                var expectedBusinessUnitName = expectedBusinessUnit;
                if (string.IsNullOrEmpty(expectedBusinessUnitName))
                {
                    // No persona for this user names a business unit explicitly, so the default (root) business
                    // unit is expected instead, matching RemoveAsync's behaviour.
                    defaultBusinessUnitName ??= (string)(await RetrieveDefaultBusinessUnitAsync(this.serviceClient).ConfigureAwait(false))["name"];
                    expectedBusinessUnitName = defaultBusinessUnitName;
                }

                var businessUnitMismatch = !string.Equals(actualBusinessUnit, expectedBusinessUnitName, StringComparison.OrdinalIgnoreCase);

                if (missingRoles.Count == 0 && missingTeams.Count == 0 && missingColumnSecurityProfiles.Count == 0 && !businessUnitMismatch)
                {
                    continue;
                }

                var issues = new List<string>();

                if (businessUnitMismatch)
                {
                    issues.Add($"business unit (expected '{expectedBusinessUnitName}', actual '{actualBusinessUnit ?? "<none>"}')");
                }

                if (missingRoles.Count > 0)
                {
                    issues.Add($"roles ({string.Join(", ", missingRoles)})");
                }

                if (missingTeams.Count > 0)
                {
                    issues.Add($"teams ({string.Join(", ", missingTeams)})");
                }

                if (missingColumnSecurityProfiles.Count > 0)
                {
                    issues.Add($"column security profiles ({string.Join(", ", missingColumnSecurityProfiles)})");
                }

                failures.Add($"user '{username}' (persona(s): {string.Join(", ", personas)}) is missing: {string.Join("; ", issues)}");
            }

            if (failures.Count > 0)
            {
                throw new InvalidOperationException(
                    $"The following statically assigned users do not have the full Dataverse security configuration described by their assigned persona(s) in environment.json: {string.Join(" | ", failures)}.");
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.serviceClient.Dispose();
        }

        private static async Task<EntityReference> RetrieveBusinessUnitReferenceAsync(ServiceClient serviceClient, string businessUnitName)
        {
            var query = new QueryExpression("businessunit")
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression("name", ConditionOperator.Equal, businessUnitName) },
                },
            };

            var result = await serviceClient.RetrieveMultipleAsync(query).ConfigureAwait(false);
            var businessUnit = result.Entities.FirstOrDefault()
                ?? throw new InvalidOperationException($"No business unit exists in Dataverse called '{businessUnitName}'.");

            return businessUnit.ToEntityReference();
        }

        private static async Task<Entity> RetrieveDefaultBusinessUnitAsync(ServiceClient serviceClient)
        {
            // The default business unit for the environment is the root of the business unit hierarchy, i.e. the one with no parent.
            var query = new QueryExpression("businessunit")
            {
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression("parentbusinessunitid", ConditionOperator.Null) },
                },
            };

            var result = await serviceClient.RetrieveMultipleAsync(query).ConfigureAwait(false);

            return result.Entities.FirstOrDefault()
                ?? throw new InvalidOperationException("No default business unit exists in Dataverse.");
        }

        private static async Task AssociateAsync(ServiceClient serviceClient, Guid userId, string entityLogicalName, string idAttribute, string nameAttribute, IReadOnlyCollection<string> names, string relationshipName)
        {
            if (names.Count == 0)
            {
                return;
            }

            var query = new QueryExpression(entityLogicalName)
            {
                ColumnSet = new ColumnSet(idAttribute, nameAttribute),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression(nameAttribute, ConditionOperator.In, names.ToArray<object>()) },
                },
            };

            var result = await serviceClient.RetrieveMultipleAsync(query).ConfigureAwait(false);
            var foundNames = new HashSet<string>(
                result.Entities
                    .Where(e => e.Contains(nameAttribute))
                    .Select(e => (string)e[nameAttribute]),
                StringComparer.OrdinalIgnoreCase);
            var missingNames = names.Where(name => !foundNames.Contains(name)).ToList();

            if (missingNames.Count > 0)
            {
                throw new InvalidOperationException($"The following requested '{entityLogicalName}' records could not be found: {string.Join(", ", missingNames)}.");
            }

            var ids = result.Entities
                .Where(e => e.Contains(nameAttribute) && names.Contains((string)e[nameAttribute], StringComparer.OrdinalIgnoreCase))
                .Select(e => e.Id)
                .ToList();

            await serviceClient.ExecuteAsync(new AssociateRequest
            {
                Target = new EntityReference(SystemUser.EntityLogicalName, userId),
                Relationship = new Relationship(relationshipName),
                RelatedEntities = new EntityReferenceCollection(ids.Select(id => new EntityReference(entityLogicalName, id)).ToList()),
            }).ConfigureAwait(false);
        }

        private static async Task DisassociateAllAsync(ServiceClient serviceClient, Guid userId, string intersectEntityLogicalName, string relatedEntityLogicalName, string relatedIdAttribute, string relationshipName)
        {
            var query = new QueryExpression(SystemUser.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression(SystemUser.Fields.SystemUserId, ConditionOperator.Equal, userId) },
                },
            };

            var intersectLink = query.AddLink(intersectEntityLogicalName, SystemUser.Fields.SystemUserId, SystemUser.Fields.SystemUserId);
            var relatedLink = intersectLink.AddLink(relatedEntityLogicalName, relatedIdAttribute, relatedIdAttribute);
            relatedLink.EntityAlias = "related";
            relatedLink.Columns = new ColumnSet(relatedIdAttribute);

            if (relatedEntityLogicalName == "team")
            {
                // A user cannot be removed from the default team of their business unit - Dataverse manages this membership automatically.
                relatedLink.LinkCriteria.Conditions.Add(new ConditionExpression("isdefault", ConditionOperator.Equal, false));
            }

            var result = await serviceClient.RetrieveMultipleAsync(query).ConfigureAwait(false);

            var relatedIdColumn = $"related.{relatedIdAttribute}";
            var ids = result.Entities
                .Where(e => e.Contains(relatedIdColumn))
                .Select(e => ((AliasedValue)e[relatedIdColumn]).Value)
                .Cast<Guid>()
                .Distinct()
                .ToList();

            if (ids.Count == 0)
            {
                return;
            }

            await serviceClient.ExecuteAsync(new DisassociateRequest
            {
                Target = new EntityReference(SystemUser.EntityLogicalName, userId),
                Relationship = new Relationship(relationshipName),
                RelatedEntities = new EntityReferenceCollection(ids.Select(id => new EntityReference(relatedEntityLogicalName, id)).ToList()),
            }).ConfigureAwait(false);
        }

        private static async Task<HashSet<string>> RetrieveAssignedNamesAsync(ServiceClient serviceClient, Guid userId, string intersectEntityLogicalName, string relatedEntityLogicalName, string relatedIdAttribute, string nameAttribute)
        {
            var query = new QueryExpression(SystemUser.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression(SystemUser.Fields.SystemUserId, ConditionOperator.Equal, userId) },
                },
            };

            var intersectLink = query.AddLink(intersectEntityLogicalName, SystemUser.Fields.SystemUserId, SystemUser.Fields.SystemUserId);
            var relatedLink = intersectLink.AddLink(relatedEntityLogicalName, relatedIdAttribute, relatedIdAttribute);
            relatedLink.EntityAlias = "related";
            relatedLink.Columns = new ColumnSet(nameAttribute);

            var result = await serviceClient.RetrieveMultipleAsync(query).ConfigureAwait(false);

            var nameColumn = $"related.{nameAttribute}";

            return new HashSet<string>(
                result.Entities
                    .Where(e => e.Contains(nameColumn))
                    .Select(e => (string)((AliasedValue)e[nameColumn]).Value),
                StringComparer.OrdinalIgnoreCase);
        }

        private static async Task<string> RetrieveUserBusinessUnitNameAsync(ServiceClient serviceClient, Guid userId)
        {
            var query = new QueryExpression(SystemUser.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression(SystemUser.Fields.SystemUserId, ConditionOperator.Equal, userId) },
                },
            };

            var businessUnitLink = query.AddLink("businessunit", SystemUser.Fields.BusinessUnitId, "businessunitid");
            businessUnitLink.EntityAlias = "businessunit";
            businessUnitLink.Columns = new ColumnSet("name");

            var result = await serviceClient.RetrieveMultipleAsync(query).ConfigureAwait(false);
            var user = result.Entities.FirstOrDefault()
                ?? throw new InvalidOperationException($"No user exists in Dataverse with ID '{userId}'.");

            const string nameColumn = "businessunit.name";

            return user.Contains(nameColumn) ? (string)((AliasedValue)user[nameColumn]).Value : null;
        }
    }
}
