namespace Defra.Imports.IntegrationTests.Dataverse
{
    using System;
    using System.Configuration;
    using System.Linq;
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

            ClientFactory = new ServiceClientFactory(
                config.Url,
                config.ClientId,
                config.ClientSecret,
                config.Personas?.ToDictionary(kvp => (Persona)Enum.Parse(typeof(Persona), kvp.Key, true), kvp => kvp.Value.AppId.HasValue ? new[] { kvp.Value.AppId.Value.ToString() } : kvp.Value.Users));
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
        /// </summary>
        /// <param name="persona">The user persona to authenticate.</param>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as the given persona.</returns>
        public static ServiceClient GetClient(Persona persona)
        {
            return ClientFactory.GetClient(persona);
        }
    }
}
