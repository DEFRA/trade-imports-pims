namespace Defra.Imports.IntegrationTests.Config
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Scenarios.Config;

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
        public IDictionary<Persona, PersonaConfiguration> Personas { get; set; }

        /// <summary>
        /// Gets or sets the usernames of the users used for dynamic persona users. Passwords are not required here since integration tests only ever impersonate these users rather than logging in as them.
        /// </summary>
        public IEnumerable<string> Credentials { get; set; }

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