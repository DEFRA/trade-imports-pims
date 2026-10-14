namespace Defra.Imports.Specs.Config
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Scenarios.Config;

    /// <summary>
    /// Configuration for acceptance tests.
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
        /// Gets or sets the client ID for the app user used to authenticate.
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Gets or sets the client secret for the app user used to authenticate.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the persona configuration.
        /// </summary>
        public IDictionary<Persona, PersonaConfiguration> Personas { get; set; }

        /// <summary>
        /// Gets or sets the credentials used for the tests.
        /// </summary>
        public IEnumerable<CredentialConfiguration> Credentials { get; set; }

        /// <summary>
        /// Gets or sets configuration relating to Key Vault.
        /// </summary>
        public KeyVaultConfiguration KeyVault { get; set; }

        /// <summary>
        /// Validates the configuration.
        /// </summary>
        public void Validate()
        {
            if (this.Url is null)
            {
                throw new Exception("You have not configured a url for the tests.");
            }

            if (this.ClientId == Guid.Empty)
            {
                throw new Exception("You have not configured a client ID for the admin application user.");
            }

            if (string.IsNullOrEmpty(this.ClientSecret))
            {
                throw new Exception("You have not configured a client secret for the admin application user.");
            }

            if (this.Personas == null || !this.Personas.Any())
            {
                throw new Exception("You have not configured any personas for the tests.");
            }

            if (this.Credentials == null || !this.Credentials.Any())
            {
                throw new Exception("You have not configured any users for the tests.");
            }

            foreach (var persona in this.Personas.Values)
            {
                persona.Validate();
            }

            foreach (var credential in this.Credentials)
            {
                credential.Validate();
            }

            var usersInMultipleBusinessUnits = this.Personas
                .Where(p => p.Value.Users != null && p.Value.Users.Any())
                .SelectMany(p => p.Value.Users.Select(username => new { Username = username, Persona = p.Key, p.Value.BusinessUnit }))
                .GroupBy(x => x.Username, StringComparer.OrdinalIgnoreCase)
                .Where(g => g.Select(x => x.BusinessUnit).Distinct(StringComparer.OrdinalIgnoreCase).Count() > 1)
                .ToList();

            if (usersInMultipleBusinessUnits.Any())
            {
                var details = usersInMultipleBusinessUnits.Select(g =>
                    $"'{g.Key}' ({string.Join(", ", g.Select(x => $"persona '{x.Persona}' -> business unit '{x.BusinessUnit ?? "<default>"}'"))})");

                throw new Exception($"The following users were configured for personas with different business units: {string.Join("; ", details)}.");
            }
        }
    }
}