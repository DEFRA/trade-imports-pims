namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Extensions;
    using Defra.Imports.Specs.Services;
    using Microsoft.Playwright;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PowerPlaywright.Api;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;
    using Reqnroll.BoDi;

    /// <summary>
    /// Login steps.
    /// </summary>
    [Binding]
    public class LoginSteps
    {
        private const string AppLogicalName = "defraimp_EUImportsApp";

        private readonly IPowerPlaywright powerPlaywright;
        private readonly IPlaywright playwright;
        private readonly IObjectContainer objectContainer;
        private readonly Config.TestConfiguration testConfig;
        private readonly UserPoolClient userPool;
        private readonly PowerPlaywrightContext powerPlaywrightCtx;
        private readonly ScenarioContext ctx;
        private readonly ServiceClient serviceClient;
        private readonly TestContext testContext;
        private readonly IReqnrollOutputHelper outputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginSteps"/> class.
        /// </summary>
        /// <param name="powerPlaywright">The Power Playwright instance.</param>
        /// <param name="playwright">The Playwright instance.</param>
        /// <param name="objectContainer">The object container.</param>
        /// <param name="testConfig">The test config.</param>
        /// <param name="userPool">The user pool.</param>
        /// <param name="powerPlaywrightCtx">The Power Playwright context.</param>
        /// <param name="ctx">The scenario context.</param>
        /// <param name="serviceClient">The service client.</param>
        /// <param name="testContext">The test context.</param>
        /// <param name="outputHelper">The output helper.</param>
        public LoginSteps(IPowerPlaywright powerPlaywright, IPlaywright playwright, IObjectContainer objectContainer, Config.TestConfiguration testConfig, UserPoolClient userPool, PowerPlaywrightContext powerPlaywrightCtx, ScenarioContext ctx, ServiceClient serviceClient, TestContext testContext, IReqnrollOutputHelper outputHelper)
        {
            this.powerPlaywright = powerPlaywright;
            this.playwright = playwright;
            this.objectContainer = objectContainer;
            this.testConfig = testConfig;
            this.userPool = userPool;
            this.powerPlaywrightCtx = powerPlaywrightCtx;
            this.ctx = ctx;
            this.serviceClient = serviceClient;
            this.testContext = testContext;
            this.outputHelper = outputHelper;
        }

        /// <summary>
        /// Logs in to the EU Imports app.
        /// </summary>
        /// <param name="userAlias">The user alias.</param>
        /// <returns>A <see cref="Task"/> representing the async task.</returns>
        [Given(@"I am logged in to the 'EU Imports' app as {string}")]
        public async Task GivenIAmLoggedInToTheEuImportsAppAs(string userAlias)
        {
            var credentials = await this.userPool.GetByAliasAsync(userAlias, allowMultiplePersonas: true);

            await this.LoginAndSetContextAsync(credentials.Username, credentials.Password);
        }

        /// <summary>
        /// Logs in to the EU Imports app as a user with no other roles.
        /// </summary>
        /// <param name="userAlias">The user alias.</param>
        /// <returns>A <see cref="Task"/> representing the async task.</returns>
        [Given(@"I am logged in to the 'EU Imports' app as {string} with no other roles")]
        public async Task GivenIAmLoggedInToTheEuImportsAppAsWithNoOtherRoles(string userAlias)
        {
            var credentials = await this.userPool.GetByAliasAsync(userAlias, allowMultiplePersonas: false);

            await this.LoginAndSetContextAsync(credentials.Username, credentials.Password);
        }

        private async Task LoginAndSetContextAsync(string username, string password)
        {
            var (browserContext, isReused) = await this.GetBrowserContextAsync(username);
            IModelDrivenAppPage homePage;

            try
            {
                homePage = await this.powerPlaywright.LaunchAppAsync(
                    browserContext,
                    this.testConfig.Url,
                    AppLogicalName,
                    username,
                    password);
            }
            catch (Exception ex) when (ex is TimeoutException || (ex is PlaywrightException pex && pex.Message.Contains("Xrm is not defined")))
            {
                await browserContext.CloseAsync();
                (browserContext, _) = await this.GetBrowserContextAsync(username);
                homePage = await this.powerPlaywright.LaunchAppAsync(
                    browserContext,
                    this.testConfig.Url,
                    AppLogicalName,
                    username,
                    password);
            }

            // TODO: Add OnPageChange event to PowerPlaywright and register an event listener to update context object
            this.powerPlaywrightCtx.ActivePage = homePage;
            this.powerPlaywrightCtx.ActiveUserId = await homePage.GetActiveUserIdAsync();

            if (!isReused)
            {
                await this.SaveBrowserContextStorageStateAsync(username);
            }
        }

        private async Task<(IBrowserContext context, bool isReused)> GetBrowserContextAsync(string username = null)
        {
            var browser = await this.playwright.Chromium.LaunchAsync(
                new BrowserTypeLaunchOptions
                {
                    Headless = bool.TryParse(Environment.GetEnvironmentVariable("HEADLESS"), out var headless) && headless,
                    Channel = "msedge",
                });

            this.objectContainer.RegisterInstanceAs(browser);

            // Attempt to reuse storage state for the provided user
            IBrowserContext context;
            var isReused = false;
            try
            {
                if (!string.IsNullOrEmpty(username))
                {
                    var storagePath = this.GetStoragePathForUser(username);
                    if (File.Exists(storagePath))
                    {
                        context = await browser.NewContextAsync(new BrowserNewContextOptions { StorageStatePath = storagePath });
                        isReused = true;
                    }
                    else
                    {
                        context = await browser.NewContextAsync();
                    }
                }
                else
                {
                    context = await browser.NewContextAsync();
                }
            }
            catch
            {
                // Fallback to a plain new context if loading storage state fails for any reason
                context = await browser.NewContextAsync();
            }

            this.objectContainer.RegisterInstanceAs(context);

            await context.Tracing.StartAsync(new TracingStartOptions()
            {
                Title = $"{this.ctx.ScenarioInfo.Title}",
                Screenshots = true,
                Snapshots = true,
                Sources = true,
            });

            return (context, isReused);
        }

        private string GetStoragePathForUser(string username)
        {
            var safeUser = string.Concat(username.Split(Path.GetInvalidFileNameChars()));
            var dir = Path.Combine(Path.GetTempPath(), "playwright_user_storage");
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            return Path.Combine(dir, safeUser + ".json");
        }

        private async Task SaveBrowserContextStorageStateAsync(string username)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                {
                    return;
                }

                var context = this.objectContainer.Resolve<IBrowserContext>();
                if (context == null)
                {
                    return;
                }

                var storagePath = this.GetStoragePathForUser(username);
                await context.StorageStateAsync(new BrowserContextStorageStateOptions { Path = storagePath });
            }
            catch (Exception ex)
            {
                this.outputHelper.WriteLine($"Failed to save browser storage state for user {username}: {ex.Message}");
            }
        }
    }
}
