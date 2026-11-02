namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Specs.Controls;
    using Defra.Imports.Specs.Services;
    using Microsoft.Extensions.Logging;
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
        /// Creates and opens a non-compliant Importer Notification without explicitly setting default non-compliance values.
        /// </summary>
        /// <returns>A task.</returns>
        [Given("I have opened a non-compliant Importer Notification without default non-compliance values")]
        public async Task GivenIHaveOpenedANonCompliantImporterNotificationWithoutDefaultValues()
        {
            var scenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp(a => a.WithNotificationNonCompliance(defraimp_noncompliancetypenotification.NoHealthCertificate))
                .BuildAsync();

            this.ctx.Set(scenario);

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(
                new EntityReference(defraimp_ImporterNotification.EntityLogicalName, scenario.LogicAppSubmitsImporterNotificationEvent.ImporterNotificationId));
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
            await this.GivenIHaveOpenedAnImportRecordLinkedToANonCompliantImporterNotificationOfType("No Health Certificate");
        }

        /// <summary>
        /// Creates and opens an Import Record linked to a non-compliant Importer Notification of the specified type.
        /// </summary>
        /// <param name="nonComplianceType">The notification non-compliance type.</param>
        /// <returns>A task.</returns>
        [Given("I have opened an Import Record linked to a non-compliant Importer Notification of type {string}")]
        public async Task GivenIHaveOpenedAnImportRecordLinkedToANonCompliantImporterNotificationOfType(string nonComplianceType)
        {
            var type = this.ToNotificationType(nonComplianceType);

            var scenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp(a => a
                    .WithNotificationNonCompliance(type)
                    .WithNotificationContactedDueToNonCompliance(false))
                .CreatedImportRecord()
                .BuildAsync();

            this.ctx.Set(scenario);

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(
                new EntityReference(defraimp_importapplication.EntityLogicalName, scenario.CreateImportRecordEvent.ImportRecordId));
        }

        /// <summary>
        /// Adds a note in the timeline on the current record.
        /// </summary>
        /// <returns>A task.</returns>
        [When("I add a non-compliance timeline note")]
        [When("I add a timeline note")]
        public async Task WhenIAddANonComplianceTimelineNote()
        {
            var note = $"US-049 non-compliance note {Guid.NewGuid():N}";
            this.ctx.Set(note, ContextKeyTimelineNote);

            var timeline = new TimelineControl(this.RecordPage.Page);
            await timeline.AddNoteAsync(note);
        }

        /// <summary>
        /// Verifies the timeline note is visible on the current record.
        /// </summary>
        /// <returns>A task.</returns>
        [Then("I see the non-compliance timeline note")]
        [Then("I see the timeline note")]
        public async Task ThenISeeTheNonComplianceTimelineNote()
        {
            var expectedNote = this.ctx.Get<string>(ContextKeyTimelineNote);

            var timeline = new TimelineControl(this.RecordPage.Page);
            await timeline.WaitForNoteAsync(expectedNote);
        }

        /// <summary>
        /// Verifies timeline is visible on the current record.
        /// </summary>
        /// <returns>A task.</returns>
        [Then("I see a non-compliance timeline")]
        [Then("I see a timeline")]
        public async Task ThenISeeANonComplianceTimeline()
        {
            var timeline = new TimelineControl(this.RecordPage.Page);
            await timeline.WaitForVisibleAsync();
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
