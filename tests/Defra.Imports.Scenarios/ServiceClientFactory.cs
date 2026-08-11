namespace Defra.Imports.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using Defra.Imports.Model;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>
    /// Base integration test class.
    /// </summary>
    public class ServiceClientFactory : IDisposable
    {
        private readonly ServiceClient baseClient;
        private readonly ILogger logger;
        private readonly IDictionary<Persona, IEnumerator<Guid>> personaUserEnumerators;
        private readonly IDictionary<Persona, IEnumerable<string>> personaMappings;

        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientFactory"/> class.
        /// </summary>
        /// <param name="url">The environment URL.</param>
        /// <param name="clientId">The client ID of the application user.</param>
        /// <param name="clientSecret">The client secret of the application user.</param>
        /// <param name="personaMappings">Mappings from personas to user emails or application IDs.</param>
        /// <param name="logger">The logger.</param>
        public ServiceClientFactory(Uri url, Guid clientId, string clientSecret, IDictionary<Persona, IEnumerable<string>> personaMappings, ILogger logger = null)
        {
            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentException($"'{nameof(clientSecret)}' cannot be null or empty.", nameof(clientSecret));
            }

            // Taken from https://github.com/microsoft/PowerApps-Samples/tree/master/dataverse/Xrm%20Tooling/TPLCrmServiceClient.
            OptimiseThreads();
            OptimiseConnections();

            this.logger = logger;
            this.baseClient = new ServiceClient(url, clientId.ToString(), clientSecret, true, logger);
            this.personaMappings = personaMappings;
            this.personaUserEnumerators = this.GetPersonaUserEnumerators(this.personaMappings);
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated with the configured application user.
        /// </summary>
        /// <returns>A <see cref="ServiceClient"/> instance.</returns>
        public ServiceClient GetAppUserClient()
        {
            return this.baseClient.Clone();
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated as the given persona.
        /// </summary>
        /// <param name="persona">The user persona to authenticate.</param>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as the given persona.</returns>
        public ServiceClient GetClient(Persona persona)
        {
            this.logger?.LogInformation($"Getting client for persona: {persona}.");

            if (!this.personaUserEnumerators.TryGetValue(persona, out var enumerator))
            {
                throw new ConfigurationErrorsException($"No users have been configured for the {persona} persona.");
            }

            Guid nextCallerId;
            lock (enumerator)
            {
                if (!enumerator.MoveNext())
                {
                    enumerator.Reset();
                    enumerator.MoveNext();
                }

                nextCallerId = enumerator.Current;
            }

            var impersonatedClient = this.baseClient.Clone();
            impersonatedClient.CallerId = nextCallerId;

            this.logger?.LogInformation($"Authenticated as caller ID: {nextCallerId}.");

            return impersonatedClient;
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance impersonating the given system user.
        /// </summary>
        /// <param name="systemUserId">The ID of the user to impersonate.</param>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as the given persona.</returns>
        public ServiceClient GetClient(Guid systemUserId)
        {
            this.logger?.LogInformation($"Getting client for system user ID: {systemUserId}.");

            var impersonatedClient = this.baseClient.Clone();
            impersonatedClient.CallerId = systemUserId;

            this.logger?.LogInformation($"Authenticated as caller ID: {systemUserId}.");

            return impersonatedClient;
        }

        /// <summary>
        /// Disposes the test class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs test clean-up.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.baseClient.Dispose();
                }

                this.disposedValue = true;
            }
        }

        private static void OptimiseThreads()
        {
            ThreadPool.SetMinThreads(100, 100);
        }

        private static void OptimiseConnections()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 65000;
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.UseNagleAlgorithm = false;
        }

        private IDictionary<Persona, IEnumerator<Guid>> GetPersonaUserEnumerators(IDictionary<Persona, IEnumerable<string>> personaIdentifiers)
        {
            this.logger?.LogInformation("Retrieving persona users.");

            if (personaIdentifiers == null)
            {
                return new Dictionary<Persona, IEnumerator<Guid>>();
            }

            var applicationIds = personaIdentifiers
                .SelectMany(p => p.Value)
                .Where(p => Guid.TryParse(p, out _))
                .Distinct()
                .ToArray<object>();

            var usernames = personaIdentifiers
                .SelectMany(p => p.Value)
                .Except(applicationIds)
                .Distinct()
                .ToArray();

            var users = this.baseClient
                .RetrieveMultiple(
                    new QueryExpression(SystemUser.EntityLogicalName)
                    {
                        ColumnSet = new ColumnSet(SystemUser.Fields.DomainName, SystemUser.Fields.ApplicationId),
                        Criteria = new FilterExpression(LogicalOperator.Or)
                        {
                            Conditions =
                            {
                                new ConditionExpression(
                                    SystemUser.Fields.DomainName,
                                    ConditionOperator.In,
                                    usernames),
                                new ConditionExpression(
                                    SystemUser.Fields.ApplicationId,
                                    ConditionOperator.In,
                                    applicationIds),
                            },
                        },
                    })
                    .Entities;

            var userIdsByIdentifier = users.ToDictionary(
                 e => e.Contains(SystemUser.Fields.ApplicationId) ? e[SystemUser.Fields.ApplicationId].ToString() : e[SystemUser.Fields.DomainName].ToString(),
                 e => e.GetAttributeValue<Guid>(SystemUser.Fields.SystemUserId));

            return personaIdentifiers.ToDictionary(
                kvp => kvp.Key,
                kvp => kvp.Value.Select(
                    identifier => userIdsByIdentifier[identifier]).ToList().AsEnumerable().GetEnumerator());
        }
    }
}