namespace Defra.Imports.Specs.Config
{
    using System;
    using Azure.Security.KeyVault.Secrets;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Configuration source for configuration stored in Key Vault that need to be mapped to <see cref="TestConfiguration"/>.
    /// </summary>
    public class AzureKeyVaultAdapterConfigurationSource : IConfigurationSource
    {
        private readonly SecretClient secretClient;
        private readonly IConfigurationRoot configurationRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureKeyVaultAdapterConfigurationSource"/> class.
        /// </summary>
        /// <param name="secretClient">The secret client.</param>
        /// <param name="configurationRoot">The configuration root.</param>
        public AzureKeyVaultAdapterConfigurationSource(SecretClient secretClient, IConfigurationRoot configurationRoot)
        {
            this.secretClient = secretClient ?? throw new ArgumentNullException(nameof(secretClient));
            this.configurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
        }

        /// <inheritdoc/>
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new AzureKeyVaultAdapterConfigurationProvider(this.secretClient, this.configurationRoot);
        }
    }
}
