namespace Defra.Imports.IntegrationTests
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;
    using Azure.Extensions.AspNetCore.Configuration.Secrets;
    using Azure.Identity;
    using Azure.Security.KeyVault.Secrets;
    using Defra.Imports.IntegrationTests.Dataverse;
    using Defra.Imports.IntegrationTests.ServiceBus;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Scenarios.Logging;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;
    using TestConfiguration = Defra.Imports.IntegrationTests.Config.TestConfiguration;

    /// <summary>
    /// Base integration test class providing shared configuration, logging, and on-demand access to Dataverse and Service Bus fixtures.
    /// </summary>
    public abstract class IntegrationTests
    {
        private const string ConfigFile = "environment.json";
        private const string EnvironmentVariablePrefix = "IMPORTS:TEST:";

        /// <summary>
        /// Provides access to the integration test configuration.
        /// </summary>
        internal static readonly TestConfiguration TestConfig;

        private ILogger logger;

        /// <summary>
        /// Initializes static members of the <see cref="IntegrationTests"/> class.
        /// </summary>
        static IntegrationTests()
        {
            TestConfig = GetTestConfiguration();
        }

        /// <summary>
        /// Gets or sets the MSTest test context.
        /// </summary>
        public TestContext TestContext { get; set; }

        /// <summary>
        /// Gets a logger.
        /// </summary>
        public ILogger Logger
        {
            get
            {
                if (this.logger == null)
                {
                    this.logger = new MsTestLogger(this.TestContext, this.GetType().FullName);
                }

                return this.logger;
            }
        }

        /// <summary>
        /// Releases any persona client acquired via <see cref="GetClientAsync(Persona[])"/> during the test, returning its leased user to the pool.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [TestCleanup]
        public async Task ReleasePersonaClientAsync()
        {
            await DataverseFixture.ReleaseClientAsync();
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated with the configured application user.
        /// </summary>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as the configured application user.</returns>
        protected ServiceClient GetAppUserClient()
        {
            return DataverseFixture.GetAppUserClient();
        }

        /// <summary>
        /// Gets an <see cref="ImportsContext"/> instance authenticated with the configured application user.
        /// </summary>
        /// <returns>An <see cref="ImportsContext"/> instance authenticated as the configured application user.</returns>
        protected ImportsContext GetAppUserContext()
        {
            return DataverseFixture.GetAppUserContext();
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated as a user with exactly the given personas.
        /// </summary>
        /// <param name="personas">The personas the leased user must have.</param>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as a user with the given personas.</returns>
        protected Task<ServiceClient> GetClientAsync(params Persona[] personas)
        {
            return DataverseFixture.GetClientAsync(personas);
        }

        /// <summary>
        /// Creates a <see cref="ServiceBusFixture"/> for sending messages to the given queue.
        /// </summary>
        /// <param name="queueName">The name of the queue to send messages to.</param>
        /// <returns>A <see cref="ServiceBusFixture"/> connected to the given queue.</returns>
        protected ServiceBusFixture GetServiceBusFixture(string queueName)
        {
            return new ServiceBusFixture(TestConfig.ServiceBus.ConnectionString, queueName);
        }

        /// <summary>
        /// Reads test data from the TestData folder.
        /// </summary>
        /// <param name="fileName">The name of the file to read.</param>
        /// <returns>The contents of the file.</returns>
        protected string ReadTestData(string fileName)
        {
            return File.ReadAllText($"{Directory.GetCurrentDirectory()}\\TestData\\{fileName}");
        }

        private static TestConfiguration GetTestConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile(ConfigFile)
                .AddEnvironmentVariables(EnvironmentVariablePrefix)
                .AddUserSecrets<IntegrationTests>(true)
                .Build()
                .Get<TestConfiguration>();

            if (config == null)
            {
                throw new ConfigurationErrorsException("Please ensure that you have configured your environment variables or user secrets file.");
            }

            if (config.KeyVault != null)
            {
                var tenantId = config.KeyVault.TenantId;
                var clientId = config.KeyVault.ClientId;
                var clientSecret = config.KeyVault.ClientSecret;
                var keyVaultName = config.KeyVault.Name;

                var secretClient = new SecretClient(
                    new Uri($"https://{keyVaultName}.vault.azure.net"),
                    new ClientSecretCredential(tenantId.ToString(), clientId.ToString(), clientSecret, new ClientSecretCredentialOptions
                    {
                        AdditionallyAllowedTenants = { "*" },
                    }));

                config.ClientSecret = new ConfigurationBuilder()
                    .AddAzureKeyVault(secretClient, new AzureKeyVaultConfigurationOptions())
                    .Build()
                    .GetValue<string>(config.ClientId.ToString()) ?? config.ClientSecret;
            }

            return config;
        }
    }
}
