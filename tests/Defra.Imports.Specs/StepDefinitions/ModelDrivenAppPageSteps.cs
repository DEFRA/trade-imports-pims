namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Extensions;
    using Defra.Imports.Specs.Services;
    using FluentAssertions;
    using PowerPlaywright.Framework.Controls.Platform;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;

    /// <summary>
    /// Step bindings relating to the <see cref="ModelDrivenAppPageSteps"/> page.
    /// </summary>
    [Binding]
    public class ModelDrivenAppPageSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly PowerPlaywrightContext powerPlaywrightCtx;
        private readonly EntityMetadataService entityMetadataSvc;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelDrivenAppPageSteps"/> class.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <param name="powerPlaywrightCtx">The Power Playwright context.</param>
        /// <param name="entityMetadataSvc">The entity metadata service.</param>
        public ModelDrivenAppPageSteps(ScenarioContext scenarioContext, PowerPlaywrightContext powerPlaywrightCtx, EntityMetadataService entityMetadataSvc)
        {
            this.scenarioContext = scenarioContext;
            this.powerPlaywrightCtx = powerPlaywrightCtx;
            this.entityMetadataSvc = entityMetadataSvc;
        }

        private IModelDrivenAppPage AppPage
        {
            get
            {
                this.powerPlaywrightCtx.ValidatePage<IModelDrivenAppPage>();

                return this.powerPlaywrightCtx.ActivePage;
            }
        }

        /// <summary>
        /// Opens a new record form for the specified entity.
        /// </summary>
        /// <param name="displayName">The entity display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Given("I am creating a new {string} record")]
        public async Task GivenIAmCreatingANewRecord(string displayName)
        {
            var logicalName = this.entityMetadataSvc.GetTableLogicalName(displayName);

            using (var cts = new CancellationTokenSource(TimeSpan.FromMinutes(1)))
            {
                this.powerPlaywrightCtx.ActivePage = await this.powerPlaywrightCtx.ActivePage.ClientApi.OpenFormAsync(logicalName).WithCancellation(cts.Token);
            }
        }

        /// <summary>
        /// Clicks the confirm button on a dialog.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I confirm the dialog")]
        public async Task WhenIConfirmTheDialog()
        {
            if (await this.AppPage.ConfirmDialog.IsVisibleAsync())
            {
                await this.AppPage.ConfirmDialog.ConfirmAsync();
            }
            else if (await this.AppPage.AlertDialog.IsVisibleAsync())
            {
                await this.AppPage.AlertDialog.ConfirmAsync();
            }
            else if (await this.AppPage.SetStateDialog.IsVisibleAsync())
            {
                await this.AppPage.SetStateDialog.ConfirmAsync();
            }
            else
            {
                throw new Exception("No confirm dialog is currently displayed.");
            }
        }

        /// <summary>
        /// Clicks the cancel button on a dialog.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I cancel the dialog")]
        public async Task WhenICancelTheDialog()
        {
            if (await this.AppPage.ConfirmDialog.IsVisibleAsync())
            {
                await this.AppPage.ConfirmDialog.CancelAsync();
            }
            else if (await this.AppPage.AlertDialog.IsVisibleAsync())
            {
                await this.AppPage.AlertDialog.ConfirmAsync();
            }
            else
            {
                throw new Exception("No confirm dialog is currently displayed.");
            }
        }

        /// <summary>
        /// Closes the entity record modal dialog.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I close the entity modal form")]
        public async Task WhenICloseTheEntityModelForm()
        {
            if (this.scenarioContext.TryGetValue<INavigationDialog<IEntityRecordPageContent>>(ScenarioContextKeys.EntityRecordModal, out var modelForm))
            {
                if (modelForm != null && await modelForm.IsVisibleAsync())
                {
                    await modelForm.CloseAsync();
                }
            }
            else
            {
                await this.powerPlaywrightCtx.ActivePage.GetNavigationDialog<IEntityRecordPageContent>().CloseAsync();
            }
        }

        /// <summary>
        /// Asserts the message displayed on the dialog.
        /// </summary>
        /// <param name="message">The message displayed on the dialog.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I see an alert dialog displayed with the message '(.*)'")]
        [Then(@"I can see an alert dialog displayed with the message '(.*)'")]
        [Then(@"I receive an error stating '(.*)'")]
        [Then(@"I see an alert dialog with the message {string}")]
        public async Task ThenICanSeeAnAlertDialogDisplayedWithTheMessage(string message)
        {
            string text;

            if (await this.AppPage.AlertDialog.IsVisibleAsync())
            {
                text = await this.AppPage.AlertDialog.GetTextAsync();
            }
            else if (await this.AppPage.ErrorDialog.IsVisibleAsync())
            {
                text = await this.AppPage.ErrorDialog.GetTextAsync();
            }
            else
            {
                throw new Exception("No alert or error dialog is currently displayed.");
            }

            text.Should().Be(message);
        }

        /// <summary>
        /// Asserts the title and message displayed on the dialog.
        /// </summary>
        /// <param name="expectedTitle">The title displayed on the dialog.</param>
        /// <param name="expectedMessage">The message displayed on the dialog.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I see an alert dialog titled '(.*)' with the message '(.*)'")]
        public async Task ThenISeeAnAlertDialogTitledWithTheMessage(string expectedTitle, string expectedMessage)
        {
            var actualTitle = await this.AppPage.AlertDialog.GetTitleAsync();
            var actualMessage = await this.AppPage.AlertDialog.GetTextAsync();

            actualTitle.Should().Be(expectedTitle);
            actualMessage.Should().Be(expectedMessage);
        }

        /// <summary>
        /// Asserts the message displayed on the dialog.
        /// </summary>
        /// <param name="expectedMessage">The message displayed on the dialog.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see a confirm dialog with the message {string}")]
        public async Task ThenISeeAConfirmDialogWithTheMessage(string expectedMessage)
        {
            var actualMessage = await this.AppPage.ConfirmDialog.GetTextAsync();
            actualMessage.Should().Be(expectedMessage);
        }

        /// <summary>
        /// Asserts the message displayed within an error dialog.
        /// </summary>
        /// <param name="expectedMessage">The error message displayed in the dialog.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see an error dialog with the message {string}")]
        public async Task ThenISeeAnErrorDialogWithTheMessage(string expectedMessage)
        {
            var actualMessage = await this.AppPage.ErrorDialog.GetTextAsync();
            actualMessage.Should().Be(expectedMessage);
        }
    }
}
