namespace Defra.Imports.IntegrationTests.Dataverse
{
    using System;
    using System.Configuration;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios;
    using Microsoft.PowerPlatform.Dataverse.Client;

    /// <summary>
    /// Provides on-demand access to a Dataverse connection for integration tests.
    /// </summary>
    public static class DataverseFixture
    {
        /// <summary>
        /// Client factory, public to allow for use by feature flag initialisation class.
        /// </summary>
        public static readonly ServiceClientFactory ClientFactory;

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

            UserPoolService userPoolService = null;
            if (config.Personas != null && config.Personas.Any())
            {
                // A separate connection to build the user pool - the factory's own base client can't be
                // used here as it doesn't exist yet (it's what we're about to construct below). Ownership
                // is passed to the applier, which disposes it, cascading from UserPoolService.Dispose().
                var poolServiceClient = new ServiceClient(config.Url, config.ClientId.ToString(), config.ClientSecret, true);
                var applier = new PersonaConfigurationApplier(poolServiceClient);
                userPoolService = new UserPoolService(config.Credentials ?? Enumerable.Empty<string>(), config.Personas, applier);
            }

            ClientFactory = new ServiceClientFactory(config.Url, config.ClientId, config.ClientSecret, userPoolService);
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated with the configured application user.
        /// </summary>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as the configured application user.</returns>
        public static ServiceClient GetAppUserClient()
        {
            return ClientFactory.GetAppUserClient();
        }

        /// <summary>
        /// Gets an <see cref="ImportsContext"/> instance authenticated with the configured application user.
        /// </summary>
        /// <returns>An <see cref="ImportsContext"/> instance authenticated as the configured application user.</returns>
        public static ImportsContext GetAppUserContext()
        {
            return new ImportsContext(GetAppUserClient());
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated as the given persona.
        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated as a user with exactly the given personas.
        /// </summary>
        /// <param name="personas">The personas the leased user must have.</param>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as a user with the given personas.</returns>
        public static Task<ServiceClient> GetClientAsync(params Persona[] personas)
        {
            return ClientFactory.GetClientAsync(personas);
        }

        /// <summary>
        /// Releases any persona client currently held by <see cref="ClientFactory"/>, returning its leased user to the pool.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task ReleaseClientAsync()
        {
            return ClientFactory.ReleaseClientAsync();
        }
    }
}
