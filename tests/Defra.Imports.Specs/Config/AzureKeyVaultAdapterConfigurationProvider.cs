namespace Defra.Imports.Specs.Config
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Azure.Extensions.AspNetCore.Configuration.Secrets;
    using Azure.Security.KeyVault.Secrets;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// A configuration provider for a Key Vault where the secret names are usernames or app IDs (without '.' or '-' characters) which need to be mapped to <see cref="TestConfiguration"/>.
    /// </summary>
    public class AzureKeyVaultAdapterConfigurationProvider : AzureKeyVaultConfigurationProvider
    {
        private const string ConfigSectionCredentials = "credentials";
        private const string ConfigKeyClientId = "clientId";
        private const string ConfigKeyClientSecret = "clientSecret";

        private readonly IConfigurationRoot configurationRoot;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureKeyVaultAdapterConfigurationProvider"/> class.
        /// </summary>
        /// <param name="secretClient">The secret client.</param>
        /// <param name="configurationRoot">The configuration root.</param>
        public AzureKeyVaultAdapterConfigurationProvider(SecretClient secretClient, IConfigurationRoot configurationRoot)
            : base(secretClient)
        {
            this.configurationRoot = configurationRoot ?? throw new ArgumentNullException(nameof(configurationRoot));
        }

        /// <inheritdoc/>
        public override void Load()
        {
            base.Load();
            this.LoadClientSecret();
            this.LoadCredentials();
        }

        private static string GetSecretName(string usernameOrAppId)
        {
            return usernameOrAppId
                .Replace(".", string.Empty)
                .Split('@')
                .First();
        }

        private void LoadCredentials()
        {
            var credentialsSection = this.configurationRoot.GetSection(ConfigSectionCredentials);
            var credentials = credentialsSection.Get<IEnumerable<CredentialConfiguration>>();

            for (int i = 0; i < credentials.Count(); i++)
            {
                var secretName = GetSecretName(credentials.ElementAt(i).Username);

                if (this.TryGet(secretName, out var secret) || this.TryGet(secretName.Replace("-", string.Empty), out secret))
                {
                    this.Set($"{credentialsSection.Path}:{i}:Password", secret);
                }
            }
        }

        private void LoadClientSecret()
        {
            if (this.TryGet(GetSecretName(this.configurationRoot[ConfigKeyClientId]), out var clientSecret))
            {
                this.Set(ConfigKeyClientSecret, clientSecret);
            }
        }
    }
}
