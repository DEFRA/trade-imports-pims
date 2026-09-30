namespace Defra.Imports.IntegrationTests.Dataverse
{
    using System;
    using System.Configuration;
    using System.Linq;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios;
    using Microsoft.PowerPlatform.Dataverse.Client;

    /// <summary>
    /// Provides the shared Dataverse connection and user pool used by integration tests. A new <see cref="ServiceClientFactory"/> must be constructed per test from <see cref="BaseClient"/> and <see cref="UserPoolService"/> (see <see cref="IntegrationTests"/>) so persona leases are tracked and released independently per test.
    /// </summary>
    public static class DataverseFixture
    {
        /// <summary>
        /// The shared base <see cref="ServiceClient"/> connection, public to allow for use by feature flag initialisation class.
        /// </summary>
        public static readonly ServiceClient BaseClient;

        /// <summary>
        /// The shared user pool service, or <c>null</c> if no personas are configured.
        /// </summary>
        public static readonly UserPoolService UserPoolService;

        /// <summary>
        /// Initializes static members of the <see cref="DataverseFixture"/> class.
        /// </summary>
        static DataverseFixture()
        {
            var config = IntegrationTests.TestConfig;

            if (config.Url == null)
            {
                throw new ConfigurationErrorsException("You must configure a URL.");
            }

            if (config.ClientId == default)
            {
                throw new ConfigurationErrorsException("You must configure a client ID.");
            }

            if (string.IsNullOrEmpty(config.ClientSecret))
            {
                throw new ConfigurationErrorsException("You must configure a client secret.");
            }

            BaseClient = ServiceClientFactory.CreateBaseClient(config.Url, config.ClientId, config.ClientSecret);

            if (config.Personas != null && config.Personas.Any())
            {
                // A separate connection to build the user pool rather than the shared base client, since
                // ownership is passed to the applier, which disposes it, cascading from UserPoolService.Dispose().
                var poolServiceClient = new ServiceClient(config.Url, config.ClientId.ToString(), config.ClientSecret, true);
                var applier = new PersonaConfigurationApplier(poolServiceClient);
                UserPoolService = new UserPoolService(config.Credentials ?? Enumerable.Empty<string>(), config.Personas, applier);
            }
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated with the configured application user.
        /// </summary>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as the configured application user.</returns>
        public static ServiceClient GetAppUserClient()
        {
            return BaseClient.Clone();
        }

        /// <summary>
        /// Gets an <see cref="ImportsContext"/> instance authenticated with the configured application user.
        /// </summary>
        /// <returns>An <see cref="ImportsContext"/> instance authenticated as the configured application user.</returns>
        public static ImportsContext GetAppUserContext()
        {
            return new ImportsContext(GetAppUserClient());
        }
    }
}
