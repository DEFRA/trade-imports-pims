namespace Defra.Imports.Specs.Hooks
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Specs;
    using Defra.Imports.Specs.Services;
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
        /// Initialises a static client factory.
        /// </summary>
        /// <param name="testThreadContainer">The test thread container.</param>
        /// <param name="testConfiguration">The test configuration.</param>
        [BeforeTestRun(Order = -19999)]
        public static void RegisterClientFactory(ObjectContainer testThreadContainer, TestConfiguration testConfiguration)
        {
            var clientFactory = new ServiceClientFactory(
                testConfiguration.Url,
                testConfiguration.ClientId,
                testConfiguration.ClientSecret,
                testConfiguration.Personas.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Users != null && kvp.Value.Users.Any() ? kvp.Value.Users : new string[] { kvp.Value.AppId.ToString() }));

            testThreadContainer.RegisterInstanceAs(clientFactory);
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
        /// Registers the <see cref="UserPoolService"/> object.
        /// </summary>
        /// <param name="testThreadContainer">The container.</param>
        [BeforeTestRun]
        public static void RegisterUserPoolService(ObjectContainer testThreadContainer)
        {
            var testConfiguration = testThreadContainer.Resolve<TestConfiguration>();
            var credentials = testConfiguration.Credentials.ToList();
            var personas = testConfiguration.Personas.ToList();

            testThreadContainer.RegisterInstanceAs(new UserPoolService(credentials.Select(c =>
            {
                var matchingPersonas = personas.Where(p => p.Value.Users != null && p.Value.Users.Contains(c.Username)).Select(p => p.Key).ToList();
                var matchingAliases = matchingPersonas.SelectMany(p => testConfiguration.Personas[p].Aliases).ToList();

                return (c, matchingPersonas.AsEnumerable(), matchingAliases.AsEnumerable());
            })));
        }

        /// <summary>
        /// Disposes the assembly hook client.
        /// </summary>
        /// <param name="testThreadContainer">The test thread container.</param>
        [AfterTestRun(Order = 1000000)]
        public static void DisposeAssemblyHookClient(ObjectContainer testThreadContainer)
        {
            testThreadContainer.Resolve<ServiceClient>().Dispose();
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
        /// Registers the <see cref="UserPoolClient"/> object.
        /// </summary>
        [BeforeScenario]
        public void RegisterUserPoolClient()
        {
            this.objectContainer.RegisterInstanceAs(new UserPoolClient(
                this.objectContainer.Resolve<UserPoolService>(),
                this.objectContainer.Resolve<IReqnrollOutputHelper>(),
                this.objectContainer.Resolve<ScenarioContext>()));
        }

        /// <summary>
        /// Registers the static client factory for the scenario.
        /// </summary>
        [BeforeScenario(Order = 0)]
        public void RegisterLogger()
        {
            this.objectContainer.RegisterInstanceAs<ILogger>(new MsTestLogger(this.objectContainer.Resolve<TestContext>()));
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