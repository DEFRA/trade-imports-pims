namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Defra.Imports.Specs.Config;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Messages;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>
    /// Applies and removes persona configuration on Dataverse users by updating their business unit and associating/disassociating security roles, teams and column security profiles.
    /// </summary>
    internal sealed class PersonaConfigurationApplier : IPersonaConfigurationApplier
    {
        private readonly ServiceClient serviceClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonaConfigurationApplier"/> class.
        /// </summary>
        /// <param name="serviceClient">The service client used to apply and remove persona configuration.</param>
        public PersonaConfigurationApplier(ServiceClient serviceClient)
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

            var userId = await RetrieveUserIdAsync(this.serviceClient, username).ConfigureAwait(false);

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
            var userId = await RetrieveUserIdAsync(this.serviceClient, username).ConfigureAwait(false);

            await DisassociateAllAsync(this.serviceClient, userId, "systemuserroles", "role", "roleid", "systemuserroles_association").ConfigureAwait(false);
            await DisassociateAllAsync(this.serviceClient, userId, "teammembership", "team", "teamid", "teammembership_association").ConfigureAwait(false);
            await DisassociateAllAsync(this.serviceClient, userId, "systemuserprofiles", "fieldsecurityprofile", "fieldsecurityprofileid", "systemuserprofiles_association").ConfigureAwait(false);

            var defaultBusinessUnit = await RetrieveDefaultBusinessUnitReferenceAsync(this.serviceClient).ConfigureAwait(false);

            await this.serviceClient.UpdateAsync(new SystemUser
            {
                Id = userId,
                BusinessUnitId = defaultBusinessUnit,
            }).ConfigureAwait(false);
        }

        private static async Task<Guid> RetrieveUserIdAsync(ServiceClient serviceClient, string username)
        {
            var query = new QueryExpression(SystemUser.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression(SystemUser.Fields.DomainName, ConditionOperator.Equal, username) },
                },
            };

            var result = await serviceClient.RetrieveMultipleAsync(query).ConfigureAwait(false);
            var user = result.Entities.FirstOrDefault()
                ?? throw new InvalidOperationException($"No user exists in Dataverse with username '{username}'.");

            return user.Id;
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

        private static async Task<EntityReference> RetrieveDefaultBusinessUnitReferenceAsync(ServiceClient serviceClient)
        {
            // The default business unit for the environment is the root of the business unit hierarchy, i.e. the one with no parent.
            var query = new QueryExpression("businessunit")
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression("parentbusinessunitid", ConditionOperator.Null) },
                },
            };

            var result = await serviceClient.RetrieveMultipleAsync(query).ConfigureAwait(false);
            var businessUnit = result.Entities.FirstOrDefault()
                ?? throw new InvalidOperationException("No default business unit exists in Dataverse.");

            return businessUnit.ToEntityReference();
        }

        private static async Task AssociateAsync(ServiceClient serviceClient, Guid userId, string entityLogicalName, string idAttribute, string nameAttribute, IReadOnlyCollection<string> names, string relationshipName)
        {
            if (names.Count == 0)
            {
                return;
            }

            var query = new QueryExpression(entityLogicalName)
            {
                ColumnSet = new ColumnSet(idAttribute),
                Criteria = new FilterExpression
                {
                    Conditions = { new ConditionExpression(nameAttribute, ConditionOperator.In, names.ToArray<object>()) },
                },
            };

            var result = await serviceClient.RetrieveMultipleAsync(query).ConfigureAwait(false);
            var ids = result.Entities.Select(e => e.Id).ToList();

            var missing = names.Count - ids.Count;
            if (missing > 0)
            {
                throw new InvalidOperationException($"{missing} of the requested '{entityLogicalName}' records could not be found: {string.Join(", ", names)}.");
            }

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
    }
}
