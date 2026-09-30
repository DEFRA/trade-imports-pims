namespace Defra.Imports.Specs.Hooks
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Scenarios.Logging;
    using Defra.Imports.Specs;
    using Microsoft.Extensions.Logging;
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
        /// Initialises a static client factory and the user pool it uses to resolve persona-based clients.
        /// </summary>
        /// <param name="testThreadContainer">The test thread container.</param>
        /// <param name="testConfiguration">The test configuration.</param>
        [BeforeTestRun(Order = -19999)]
        public static void RegisterClientFactory(ObjectContainer testThreadContainer, TestConfiguration testConfiguration)
        {
            // A separate connection to build the user pool - the factory's own base client can't be
            // used here as it doesn't exist yet (it's what we're about to construct below). Ownership
            // is passed to the applier, which disposes it, cascading from UserPoolService.Dispose().
            var poolServiceClient = new ServiceClient(testConfiguration.Url, testConfiguration.ClientId.ToString(), testConfiguration.ClientSecret, true);
            var applier = new PersonaConfigurationApplier(poolServiceClient);
            var userPoolService = new UserPoolService(testConfiguration.Credentials.Select(c => c.Username), testConfiguration.Personas, applier);

            var clientFactory = new ServiceClientFactory(
                testConfiguration.Url,
                testConfiguration.ClientId,
                testConfiguration.ClientSecret,
                userPoolService);

            testThreadContainer.RegisterInstanceAs(clientFactory);
            testThreadContainer.RegisterInstanceAs(userPoolService);
        }

        /// <summary>
        /// Registers an app user client for assembly level hookds.
        /// </summary>
        /// <param name="testThreadContainer">The test thread container.</param>
        [BeforeTestRun(Order = -19998)]
        public static void RegisterAssemblyHookClient(ObjectContainer testThreadContainer)
        {
            testThreadContainer.RegisterInstanceAs(
                testThreadContainer.Resolve<ServiceClientFactory>().GetAppUserClient());
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
            var playwright = await Playwright.CreateAsync();

            testThreadContainer.RegisterInstanceAs(playwright);
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
        /// Disposes the client factory, which cascades to any held persona leases and clients and the configured user pool service.
        /// </summary>
        /// <param name="testThreadContainer">The test thread container.</param>
        [AfterTestRun(Order = 1000000)]
        public static void DisposeClientFactory(ObjectContainer testThreadContainer)
        {
            if (testThreadContainer.IsRegistered<ServiceClientFactory>())
            {
                testThreadContainer.Resolve<ServiceClientFactory>().Dispose();
            }
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
        /// Registers a fresh <see cref="ScenarioUserClient"/> for the scenario, wired to this scenario's output helper and context.
        /// </summary>
        [BeforeScenario(Order = -9999)]
        public void RegisterScenarioUserClient()
        {
            var scenarioContext = this.objectContainer.Resolve<ScenarioContext>();
            var testConfiguration = this.objectContainer.Resolve<TestConfiguration>();
            var scenarioUserClient = new ScenarioUserClient(this.objectContainer.Resolve<UserPoolService>(), testConfiguration.Credentials);

            scenarioUserClient.Logged += this.outputHelper.WriteLine;
            scenarioUserClient.Revoked += ex => scenarioContext[ScenarioContextKeys.LeaseRevokedErrorKey] = ex;

            this.objectContainer.RegisterInstanceAs(scenarioUserClient);
        }

        /// <summary>
        /// Registers the static client factory for the scenario.
        /// </summary>
        [BeforeScenario(Order = 0)]
        public void RegisterLogger()
        {
            var testContext = this.objectContainer.Resolve<TestContext>();

            this.objectContainer.RegisterInstanceAs<ILogger>(new MsTestLogger(testContext));
            this.objectContainer.RegisterInstanceAs<ILoggerProvider>(new MsTestLoggerProvider(testContext));
        }

        /// <summary>
        /// Sets up the Power Playwright instance for the scenario.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [BeforeScenario(Order = -9998)]
        public async Task SetupPowerPlaywright()
        {
            var powerPlaywright = await PowerPlaywright.CreateAsync(new PowerPlaywrightConfiguration());

            this.objectContainer.RegisterInstanceAs(powerPlaywright);
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
    }
}