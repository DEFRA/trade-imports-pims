namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Specs.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Playwright;
    using Microsoft.Xrm.Sdk;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;

    /// <summary>
    /// Steps for US-049 Non-Compliance management scenarios.
    /// </summary>
    [Binding]
    public class NonComplianceSteps
    {
        private const string ContextKeyTimelineNote = nameof(ContextKeyTimelineNote);

        private readonly ServiceClientFactory clientFactory;
        private readonly ILoggerProvider loggerProvider;
        private readonly PowerPlaywrightContext powerPlaywrightCtx;
        private readonly RecordNavigatorService recordNavigator;
        private readonly ScenarioContext ctx;

        /// <summary>
        /// Initializes a new instance of the <see cref="NonComplianceSteps"/> class.
        /// </summary>
        /// <param name="clientFactory">The client factory.</param>
        /// <param name="loggerProvider">The logger provider.</param>
        /// <param name="powerPlaywrightCtx">The Power Playwright context.</param>
        /// <param name="recordNavigator">The record navigator service.</param>
        /// <param name="ctx">The scenario context.</param>
        public NonComplianceSteps(
            ServiceClientFactory clientFactory,
            ILoggerProvider loggerProvider,
            PowerPlaywrightContext powerPlaywrightCtx,
            RecordNavigatorService recordNavigator,
            ScenarioContext ctx)
        {
            this.clientFactory = clientFactory;
            this.loggerProvider = loggerProvider;
            this.powerPlaywrightCtx = powerPlaywrightCtx;
            this.recordNavigator = recordNavigator;
            this.ctx = ctx;
        }

        private IEntityRecordPage RecordPage
        {
            get
            {
                this.powerPlaywrightCtx.ValidatePage<IEntityRecordPage>();

                return (IEntityRecordPage)this.powerPlaywrightCtx.ActivePage;
            }
        }

        /// <summary>
        /// Creates and opens a non-compliant Importer Notification with default type.
        /// </summary>
        /// <returns>A task.</returns>
        [Given("I have opened a non-compliant Importer Notification")]
        public async Task GivenIHaveOpenedANonCompliantImporterNotification()
        {
            await this.GivenIHaveOpenedANonCompliantImporterNotificationOfType("No Health Certificate");
        }

        /// <summary>
        /// Creates and opens a non-compliant Importer Notification.
        /// </summary>
        /// <param name="nonComplianceType">The notification non-compliance type.</param>
        /// <returns>A task.</returns>
        [Given("I have opened a non-compliant Importer Notification of type {string}")]
        public async Task GivenIHaveOpenedANonCompliantImporterNotificationOfType(string nonComplianceType)
        {
            var type = this.ToNotificationType(nonComplianceType);

            var scenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp(a => a
                    .WithNotificationNonCompliance(type)
                    .WithNotificationNonComplianceStatus(defraimp_noncompliancestatus.InProgress)
                    .WithNotificationContactedDueToNonCompliance(false))
                .BuildAsync();

            this.ctx.Set(scenario);

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(
                new EntityReference(defraimp_ImporterNotification.EntityLogicalName, scenario.LogicAppSubmitsImporterNotificationEvent.ImporterNotificationId));
        }

        /// <summary>
        /// Creates and opens an Import Record linked to a non-compliant Importer Notification.
        /// </summary>
        /// <returns>A task.</returns>
        [Given("I have opened an Import Record linked to a non-compliant Importer Notification")]
        public async Task GivenIHaveOpenedAnImportRecordLinkedToANonCompliantImporterNotification()
        {
            var notificationScenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp(a => a
                    .WithNotificationNonCompliance(defraimp_noncompliancetypenotification.NoHealthCertificate)
                    .WithNotificationNonComplianceStatus(defraimp_noncompliancestatus.InProgress)
                    .WithNotificationContactedDueToNonCompliance(false))
                .BuildAsync();

            var importerNotificationId = notificationScenario.LogicAppSubmitsImporterNotificationEvent.ImporterNotificationId;

            var importRecordScenario = await new ImportRecordScenario.Builder(this.clientFactory, this.loggerProvider)
                .Created(a => a
                    .ForImporterNotification(importerNotificationId))
                .BuildAsync();

            this.ctx.Set(notificationScenario);
            this.ctx.Set(importRecordScenario);

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(
                new EntityReference(defraimp_importapplication.EntityLogicalName, importRecordScenario.CreateImportRecordEvent.ImportRecordId));
        }

        /// <summary>
        /// Adds a note in the timeline on the current record.
        /// </summary>
        /// <returns>A task.</returns>
        [When("I add a non-compliance timeline note")]
        public async Task WhenIAddANonComplianceTimelineNote()
        {
            var note = $"US-049 non-compliance note {Guid.NewGuid():N}";
            this.ctx.Set(note, ContextKeyTimelineNote);

            var page = this.RecordPage.Page;
            var timeline = page.Locator("[aria-label='Timeline'], [data-id='notescontrol'], [id*='Timeline']").First;
            await timeline.WaitForAsync(new LocatorWaitForOptions { Timeout = 60000 });

            var addNoteButton = page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Add note" }).First;
            if (await addNoteButton.IsVisibleAsync())
            {
                await addNoteButton.ClickAsync();
            }

            var textbox = page
                .GetByPlaceholder("Type a note...")
                .Or(page.GetByLabel("Type a note..."))
                .Or(page.Locator("div[contenteditable='true'][role='textbox'][aria-label*='note' i]"))
                .First;

            await textbox.WaitForAsync(new LocatorWaitForOptions { Timeout = 60000 });
            await textbox.FillAsync(note);

            var postButton = page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Post|Save", Exact = false }).First;

            await postButton.WaitForAsync(new LocatorWaitForOptions { Timeout = 30000 });
            await postButton.ClickAsync();
        }

        /// <summary>
        /// Verifies the timeline note is visible on the current record.
        /// </summary>
        /// <returns>A task.</returns>
        [Then("I see the non-compliance timeline note")]
        public async Task ThenISeeTheNonComplianceTimelineNote()
        {
            var expectedNote = this.ctx.Get<string>(ContextKeyTimelineNote);
            await this.RecordPage.Page.GetByText(expectedNote, new PageGetByTextOptions { Exact = false }).First.WaitForAsync(new LocatorWaitForOptions { Timeout = 60000 });
        }

        /// <summary>
        /// Verifies timeline is visible on the current record.
        /// </summary>
        /// <returns>A task.</returns>
        [Then("I see a non-compliance timeline")]
        public async Task ThenISeeANonComplianceTimeline()
        {
            var page = this.RecordPage.Page;
            var timeline = page.Locator("[aria-label='Timeline'], [data-id='notescontrol'], [id*='Timeline']").First;
            await timeline.WaitForAsync();
        }

        private defraimp_noncompliancetypenotification ToNotificationType(string value)
        {
            return value switch
            {
                "No Health Certificate" => defraimp_noncompliancetypenotification.NoHealthCertificate,
                "Other" => defraimp_noncompliancetypenotification.Other,
                _ => throw new InvalidOperationException($"Unsupported non-compliance type '{value}'."),
            };
        }

    }
}
