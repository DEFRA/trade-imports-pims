namespace Defra.Imports.Specs.Hooks
{
    using System;
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;
    using Defra.Imports.Specs.Config;
    using Defra.Imports.Specs.Extensions;
    using Microsoft.Extensions.Configuration;
    using Reqnroll;
    using Reqnroll.BoDi;

    /// <summary>
    /// Hooks relating to the object containers.
    /// </summary>
    [Binding]
    public class ConfigurationHooks
    {
        private const string ConfigPrefixEnvironmentVariables = "IMPORTS:TEST:";
        private const string ConfigFile = "environment.json";
        private const string ConfigSectionKeyVault = "keyVault";

        /// <summary>
        /// Gets the configuration for the test run.
        /// </summary>
        /// <param name="testThreadContainer">The Object container injected.</param>
        [BeforeTestRun(Order = -20000)]
        public static void GetEnvironmentConfiguration(ObjectContainer testThreadContainer)
        {
            KeyVaultConfiguration keyVaultConfiguration = null;

            try
            {
                var configBuilder = new ConfigurationBuilder()
                    .AddJsonFile(ConfigFile)
                    .AddEnvironmentVariables(ConfigPrefixEnvironmentVariables)
                    .AddUserSecrets<ConfigurationHooks>(true);

                var configRoot = configBuilder.Build();

                keyVaultConfiguration = configRoot.GetSection(ConfigSectionKeyVault).Get<KeyVaultConfiguration>();
                if (keyVaultConfiguration != null)
                {
                    var secretClient = new SecretClient(
                        new Uri($"https://{keyVaultConfiguration.Name}.vault.azure.net"),
                        new ClientSecretCredential(
                            keyVaultConfiguration.TenantId.ToString(),
                            keyVaultConfiguration.ClientId.ToString(),
                            keyVaultConfiguration.ClientSecret,
                            new ClientSecretCredentialOptions
                            {
                                AdditionallyAllowedTenants = { "*" },
                            }));

                    configBuilder.AddAzureKeyVaultTestConfigurationAdapter(secretClient, configRoot);
                }

                var configuration = configBuilder.Build().Get<TestConfiguration>() ??
                    throw new Exception("The test suite has missing configuration values.");

                configuration.Validate();

                testThreadContainer.RegisterInstanceAs(configuration);
            }
            catch (Exception ex)
            {
                // Context is limited to non-secret identifiers (Key Vault name/tenant/client ID) to help diagnose
                // which Key Vault was targeted without logging any credentials.
                var context = keyVaultConfiguration is null
                    ? null
                    : $"keyVaultName={keyVaultConfiguration.Name}, tenantId={keyVaultConfiguration.TenantId}, clientId={keyVaultConfiguration.ClientId}";

                HookDiagnostics.LogBeforeTestRunFailure(nameof(GetEnvironmentConfiguration), ex, context);
                throw;
            }
        }
    }
}
