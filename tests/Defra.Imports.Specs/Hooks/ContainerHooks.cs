namespace Defra.Imports.Specs.Hooks
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Scenarios.Logging;
    using Defra.Imports.Specs.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Microsoft.Playwright;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PowerPlaywright.Api;
    using PowerPlaywright.Config;
    using Reqnroll;
    using Reqnroll.BoDi;
    using TestConfiguration = Defra.Imports.Specs.Config.TestConfiguration;

    /// <summary>
    /// Hooks relating to dependency injection.
    /// </summary>
    [Binding]
    public sealed class ContainerHooks
    {
        // Not registered in the container: the container already holds a per-scenario ServiceClient (the
        // app user client registered by RegisterAppUserClient), so registering this shared connection under
        // the same type would clash with it.
        private static ServiceClient sharedBaseClient;

        private readonly IObjectContainer objectContainer;
        private readonly IReqnrollOutputHelper outputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerHooks"/> class.
        /// </summary>
        /// <param name="objectContainer">The <see cref="IObjectContainer"/> instance.</param>
        /// <param name="outputHelper">The output helper.</param>
        public ContainerHooks(IObjectContainer objectContainer, IReqnrollOutputHelper outputHelper)
        {
            this.objectContainer = objectContainer;
            this.outputHelper = outputHelper;
        }

        /// <summary>
        /// Initialises the shared base client and the user pool it uses to resolve persona-based clients, for the whole test run.
        /// </summary>
        /// <param name="testThreadContainer">The test thread container.</param>
        /// <param name="testConfiguration">The test configuration.</param>
        [BeforeTestRun(Order = -19999)]
        public static void RegisterUserPoolService(ObjectContainer testThreadContainer, TestConfiguration testConfiguration)
        {
            try
            {
                sharedBaseClient = ServiceClientFactory.CreateBaseClient(testConfiguration.Url, testConfiguration.ClientId, testConfiguration.ClientSecret);

                // A separate connection to build the user pool rather than the shared base client, since
                // ownership is passed to the persona configuration service, which disposes it, cascading from UserPoolService.Dispose().
                var poolServiceClient = new ServiceClient(testConfiguration.Url, testConfiguration.ClientId.ToString(), testConfiguration.ClientSecret, true);
                var personaConfigurationService = new PersonaConfigurationService(poolServiceClient);
                var usernames = testConfiguration.Credentials
                    .Select(c => c.Username)
                    .Where(u => !string.IsNullOrWhiteSpace(u))
                    .Distinct(StringComparer.OrdinalIgnoreCase);

                var userPoolService = new UserPoolService(usernames, testConfiguration.Personas, personaConfigurationService);

                testThreadContainer.RegisterInstanceAs(userPoolService);
            }
            catch (Exception ex)
            {
                // Context is limited to non-secret identifiers (URL/client/tenant ID) to help diagnose which
                // environment/application user was targeted without logging any credentials.
                var context = $"url={testConfiguration.Url}, clientId={testConfiguration.ClientId}, tenantId={testConfiguration.TenantId}";

                HookDiagnostics.LogBeforeTestRunFailure(nameof(RegisterUserPoolService), ex, context);
                throw;
            }
        }

        /// <summary>
        /// Registers an app user client for assembly level hookds.
        /// </summary>
        /// <param name="testThreadContainer">The test thread container.</param>
        [BeforeTestRun(Order = -19998)]
        public static void RegisterAssemblyHookClient(ObjectContainer testThreadContainer)
        {
            try
            {
                testThreadContainer.RegisterInstanceAs(sharedBaseClient.Clone());
            }
            catch (Exception ex)
            {
                HookDiagnostics.LogBeforeTestRunFailure(nameof(RegisterAssemblyHookClient), ex);
                throw;
            }
        }

        /// <summary>
        /// Sets up Playwright for the test run.
        /// </summary>
        /// <param name="testThreadContainer">The Object container injected.</param>
        /// <remarks>
        /// Playwright is registered as a singleton (i.e. same instance is used for all tests).
        /// </remarks>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [BeforeTestRun(Order = -19997)]
        public static async Task SetupPlaywright(ObjectContainer testThreadContainer)
        {
            try
            {
                var playwright = await Playwright.CreateAsync();

                testThreadContainer.RegisterInstanceAs(playwright);
            }
            catch (Exception ex)
            {
                HookDiagnostics.LogBeforeTestRunFailure(nameof(SetupPlaywright), ex);
                throw;
            }
        }

        /// <summary>
        /// Disposes the assembly hook client.
        /// </summary>
        /// <param name="testThreadContainer">The test thread container.</param>
        [AfterTestRun(Order = 1000000)]
        public static void DisposeAssemblyHookClient(ObjectContainer testThreadContainer)
        {
            if (testThreadContainer.IsRegistered<ServiceClient>())
            {
                testThreadContainer.Resolve<ServiceClient>().Dispose();
            }
        }

        /// <summary>
        /// Disposes the shared user pool service and base client, once every scenario on this thread has finished.
        /// </summary>
        /// <param name="testThreadContainer">The test thread container.</param>
        [AfterTestRun(Order = 1000000)]
        public static void DisposeSharedConnection(ObjectContainer testThreadContainer)
        {
            if (testThreadContainer.IsRegistered<UserPoolService>())
            {
                testThreadContainer.Resolve<UserPoolService>().Dispose();
            }

            sharedBaseClient?.Dispose();
        }

        /// <summary>
        /// Registers a fresh <see cref="ServiceClientFactory"/> for the scenario, so persona clients it obtains are tracked and released independently of other scenarios sharing the same underlying connection.
        /// </summary>
        [BeforeScenario(Order = -19999)]
        public void RegisterScenarioClientFactory()
        {
            var userPoolService = this.objectContainer.Resolve<UserPoolService>();

            this.objectContainer.RegisterInstanceAs(new ServiceClientFactory(sharedBaseClient, userPoolService));
        }

        /// <summary>
        /// Registers the app user client for the scenario.
        /// </summary>
        [BeforeScenario(Order = -10000)]
        public void RegisterAppUserClient()
        {
            var appUserClient = this.objectContainer.Resolve<ServiceClientFactory>().GetAppUserClient();

            this.objectContainer.RegisterInstanceAs(appUserClient);
        }

        /// <summary>
        /// Registers a fresh <see cref="ScenarioUserClient"/> for the scenario, wired to this scenario's output helper.
        /// </summary>
        [BeforeScenario(Order = -9999)]
        public void RegisterScenarioUserClient()
        {
            try
            {
                var testConfiguration = this.objectContainer.Resolve<TestConfiguration>();
                var scenarioUserClient = new ScenarioUserClient(this.objectContainer.Resolve<UserPoolService>(), testConfiguration.Credentials);

                scenarioUserClient.Logged += this.outputHelper.WriteLine;

                this.objectContainer.RegisterInstanceAs(scenarioUserClient);
            }
            catch (Exception ex)
            {
                this.outputHelper.WriteLine($"[BeforeScenario] {nameof(this.RegisterScenarioUserClient)} failed: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Registers the static client factory for the scenario.
        /// </summary>
        [BeforeScenario(Order = 0)]
        public void RegisterLogger()
        {
            try
            {
                if (!this.objectContainer.IsRegistered<TestContext>())
                {
                    this.objectContainer.RegisterInstanceAs<ILogger>(NullLogger.Instance);
                    this.objectContainer.RegisterInstanceAs<ILoggerProvider>(NullLoggerProvider.Instance);
                    this.outputHelper.WriteLine("[BeforeScenario] TestContext was not registered. Falling back to null logger.");
                    return;
                }

                var testContext = this.objectContainer.Resolve<TestContext>();
                if (testContext == null)
                {
                    this.objectContainer.RegisterInstanceAs<ILogger>(NullLogger.Instance);
                    this.objectContainer.RegisterInstanceAs<ILoggerProvider>(NullLoggerProvider.Instance);
                    this.outputHelper.WriteLine("[BeforeScenario] TestContext was null. Falling back to null logger.");
                    return;
                }

                this.objectContainer.RegisterInstanceAs<ILogger>(new MsTestLogger(testContext));
                this.objectContainer.RegisterInstanceAs<ILoggerProvider>(new MsTestLoggerProvider(testContext));
            }
            catch (Exception ex)
            {
                this.outputHelper.WriteLine($"[BeforeScenario] {nameof(this.RegisterLogger)} failed: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Sets up the Power Playwright instance for the scenario.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [BeforeScenario(Order = -9998)]
        public async Task SetupPowerPlaywright()
        {
            try
            {
                var powerPlaywright = await PowerPlaywright.CreateAsync(new PowerPlaywrightConfiguration());

                this.objectContainer.RegisterInstanceAs(powerPlaywright);
            }
            catch (Exception ex)
            {
                this.outputHelper.WriteLine($"[BeforeScenario] {nameof(this.SetupPowerPlaywright)} failed: {ex}");
                throw;
            }
        }

        /// <summary>
        /// Disposes the browser and browser context that were created for the scenario.
        /// </summary>
        /// <returns>An async Task.</returns>
        [AfterScenario(Order = 20000)]
        public async Task DisposeBrowser()
        {
            try
            {
                var browser = this.objectContainer.Resolve<IBrowser>();
                if (browser != null)
                {
                    await browser.DisposeAsync();
                }
            }
            catch (Exception ex)
            {
                this.outputHelper.WriteLine($"An error occurred while disposing the Playwright browser: {ex.Message}.");
            }
        }

        /// <summary>
        /// Disposes the scenario app user client.
        /// </summary>
        [AfterScenario(Order = 20000)]
        public void DisposeAppUserClient()
        {
            try
            {
                var appUserClient = this.objectContainer.Resolve<ServiceClient>();
                appUserClient.Dispose();
            }
            catch (Exception ex)
            {
                this.outputHelper.WriteLine($"An error occurred while disposing the app user client: {ex.Message}.");
            }
        }

        /// <summary>
        /// Disposes the scenario's <see cref="ServiceClientFactory"/>, releasing any persona leases/clients it obtained. The shared base client and user pool service are unaffected.
        /// </summary>
        [AfterScenario(Order = 20000)]
        public void DisposeClientFactory()
        {
            try
            {
                this.objectContainer.Resolve<ServiceClientFactory>().Dispose();
            }
            catch (Exception ex)
            {
                this.outputHelper.WriteLine($"An error occurred while disposing the client factory: {ex.Message}.");
            }
        }
    }
}