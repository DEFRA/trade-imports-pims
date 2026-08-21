namespace Defra.Imports.IntegrationTests.Config
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Configuration for integration tests.
    /// </summary>
    public class TestConfiguration
    {
        /// <summary>
        /// Gets or sets the URL to the Dataverse environment.
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Gets or sets the client ID for the app user used to authenticate.
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret for the app user used to authenticate.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets persona mappings.
        /// </summary>
        public IDictionary<string, PersonaConfiguration> Personas { get; set; }

        /// <summary>
        /// Gets or sets configuration relating to Key Vault.
        /// </summary>
        public KeyVaultConfiguration KeyVault { get; set; }

        /// <summary>
        /// Gets or sets configuration relating to Service Bus.
        /// </summary>
        public ServiceBusConfiguration ServiceBus { get; set; }
    }
}