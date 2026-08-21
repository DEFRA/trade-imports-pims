namespace Defra.Imports.IntegrationTests.Config
{
    using System;

    /// <summary>
    /// Configuration for connecting to Azure Key Vault.
    /// </summary>
    public class KeyVaultConfiguration
    {
        /// <summary>
        /// Gets or sets the tenant ID used to authenticate.
        /// </summary>
        public Guid TenantId { get; set; }

        /// <summary>
        /// Gets or sets the client ID used to authenticate.
        /// </summary>
        public Guid ClientId { get; set; }

        /// <summary>
        /// Gets or sets the client secret used to authenticate.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the name of the Key Vault.
        /// </summary>
        public string Name { get; set; }
    }
}