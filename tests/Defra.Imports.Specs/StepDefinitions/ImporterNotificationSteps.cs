namespace Defra.Imports.Specs.StepDefinitions
{
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Specs.Services;
    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;
    using Reqnroll;

    /// <summary>
    /// Steps relating to Importer Notifications.
    /// </summary>
    [Binding]
    public class ImporterNotificationSteps
    {
        private readonly ServiceClientFactory clientFactory;
        private readonly ILoggerProvider loggerProvider;
        private readonly PowerPlaywrightContext powerPlaywrightCtx;
        private readonly RecordNavigatorService recordNavigator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImporterNotificationSteps"/> class.
        /// </summary>
        /// <param name="clientFactory">The client factory.</param>
        /// <param name="loggerProvider">The logger provider.</param>
        /// <param name="powerPlaywrightCtx">The PowerPlaywright context.</param>
        /// <param name="recordNavigator">The record navigator service.</param>
        public ImporterNotificationSteps(ServiceClientFactory clientFactory, ILoggerProvider loggerProvider, PowerPlaywrightContext powerPlaywrightCtx, RecordNavigatorService recordNavigator)
        {
            this.clientFactory = clientFactory;
            this.loggerProvider = loggerProvider;
            this.powerPlaywrightCtx = powerPlaywrightCtx;
            this.recordNavigator = recordNavigator;
        }

        /// <summary>
        /// Submits an Importer Notification, as the EU Imports Notifications Logic App would from an IPAFFS message, with no specific configuration, and navigates to it.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("I have opened an Importer Notification")]
        public async Task GivenIHaveOpenedAnImporterNotification()
        {
            this.powerPlaywrightCtx.Validate();

            var scenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp()
                .BuildAsync();

            var importerNotificationId = scenario.LogicAppSubmitsImporterNotificationEvent.ImporterNotificationId;

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(
                new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId));
        }

        /// <summary>
        /// Submits an Importer Notification, as the EU Imports Notifications Logic App would from an IPAFFS message, with the given type, and navigates to it.
        /// </summary>
        /// <param name="type">The type of Importer Notification.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("^I have opened an Importer Notification of type '(CHEDA|CVEDA|CVEDP|GBNAG|IMP)'")]
        public async Task GivenIHaveOpenedAnImporterNotificationOfType(defraimp_importernotificationtype type)
        {
            this.powerPlaywrightCtx.Validate();

            var scenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp(a => a
                    .WithType(type))
                .BuildAsync();

            var importerNotificationId = scenario.LogicAppSubmitsImporterNotificationEvent.ImporterNotificationId;

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(
                new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId));
        }

        /// <summary>
        /// Submits an Importer Notification, as the EU Imports Notifications Logic App would from an IPAFFS message, importing from a charity, and navigates to it.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("I have opened an Importer Notification which is importing from a charity")]
        public async Task GivenIHaveOpenedAnImporterNotificationWhichIsImportingFromACharity()
        {
            this.powerPlaywrightCtx.Validate();

            var scenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp(a => a
                    .WithImportingFromCharity())
                .BuildAsync();

            var importerNotificationId = scenario.LogicAppSubmitsImporterNotificationEvent.ImporterNotificationId;

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(
                new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId));
        }
    }
}
