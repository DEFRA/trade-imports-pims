namespace Defra.Imports.Specs.Extensions
{
    using Azure.Security.KeyVault.Secrets;
    using Defra.Imports.Specs.Config;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Extensions for the <see cref="IConfigurationBuilder"/> interface.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// Adds an Azure Key Vault configuration provider that maps Azure Key Vault secrets that are named by username and app ID (without invalid characters) and maps them to the <see cref="TestConfiguration"/>.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="secretClient">The secret client.</param>
        /// <param name="configurationRoot">The configuration root.</param>
        /// <returns>The updated builder.</returns>
        public static IConfigurationBuilder AddAzureKeyVaultTestConfigurationAdapter(this IConfigurationBuilder builder, SecretClient secretClient, IConfigurationRoot configurationRoot)
        {
            return builder.Add(new AzureKeyVaultAdapterConfigurationSource(secretClient, configurationRoot));
        }
    }
}
