namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bogus;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Specs.Services;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;
    using PowerPlaywright.Framework.Controls.Pcf.Classes;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;

    /// <summary>
    /// Steps relating to Importer Notifications.
    /// </summary>
    [Binding]
    public class ImporterNotificationSteps
    {
        // Maps the free text search fields in AC-3 (US-003) to the Importer Notification property backing each one.
        private static readonly IReadOnlyDictionary<string, Func<defraimp_ImporterNotification, string>> SearchFieldValueSelectors = new Dictionary<string, Func<defraimp_ImporterNotification, string>>
        {
            ["Importer Name"] = n => n.defraimp_importercompanyname,
            ["Charity Name"] = n => n.defraimp_consignortwocompanyname,
            ["Premises of Origin Name"] = n => n.defraimp_consignorcompanyname,
            ["Permanent Destination Name"] = n => n.defraimp_placeofdestinationcompanyname,
            ["Animal / Product ID"] = n => n.defraimp_CommodityId,
        };

        private readonly ServiceClientFactory clientFactory;
        private readonly ILoggerProvider loggerProvider;
        private readonly PowerPlaywrightContext powerPlaywrightCtx;
        private readonly RecordNavigatorService recordNavigator;
        private readonly ScenarioContext ctx;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImporterNotificationSteps"/> class.
        /// </summary>
        /// <param name="clientFactory">The client factory.</param>
        /// <param name="loggerProvider">The logger provider.</param>
        /// <param name="powerPlaywrightCtx">The PowerPlaywright context.</param>
        /// <param name="recordNavigator">The record navigator service.</param>
        /// <param name="ctx">The scenario context.</param>
        public ImporterNotificationSteps(ServiceClientFactory clientFactory, ILoggerProvider loggerProvider, PowerPlaywrightContext powerPlaywrightCtx, RecordNavigatorService recordNavigator, ScenarioContext ctx)
        {
            this.clientFactory = clientFactory;
            this.loggerProvider = loggerProvider;
            this.powerPlaywrightCtx = powerPlaywrightCtx;
            this.recordNavigator = recordNavigator;
            this.ctx = ctx;
        }

        private IEntityListPage EntityListPage
        {
            get
            {
                this.powerPlaywrightCtx.ValidatePage<IEntityListPage>();

                return (IEntityListPage)this.powerPlaywrightCtx.ActivePage;
            }
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
        /// Submits an Importer Notification, as the EU Imports Notifications Logic App would from an IPAFFS message, with the given type, and navigates to it.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("^I have opened an Importer Notification of type CHEDA or CVEDA")]
        public async Task GivenIHaveOpenedAnImporterNotificationOfTypeChedAOrCvedA()
        {
            this.powerPlaywrightCtx.Validate();

            var scenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp(a => a
                    .WithType(new Faker().PickRandom(defraimp_importernotificationtype.CVEDA, defraimp_importernotificationtype.CHEDA)))
                .BuildAsync();

            var importerNotificationId = scenario.LogicAppSubmitsImporterNotificationEvent.ImporterNotificationId;

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(
                new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId));
        }

        /// <summary>
        /// Submits an Importer Notification, as the EU Imports Notifications Logic App would from an IPAFFS message, with a randomly selected type that is not the given type, and navigates to it.
        /// </summary>
        /// <param name="excludedType">The type of Importer Notification to exclude.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("^I have opened an Importer Notification not of type '(CHEDA|CVEDA|CVEDP|GBNAG|IMP)'")]
        public async Task GivenIHaveOpenedAnImporterNotificationNotOfType(defraimp_importernotificationtype excludedType)
        {
            this.powerPlaywrightCtx.Validate();

            var type = new Faker().PickRandomWithout(excludedType);

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

        /// <summary>
        /// Submits an Importer Notification with a given purpose of consignment and navigates to it.
        /// </summary>
        /// <param name="purposeOfConsignment">The purpose of consignment.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("^I have opened an Importer Notification with a Purpose of Consignment of '(.*)'$")]
        public async Task GivenIHaveOpenedAnImporterNotificationWithPurposeOfConsignmentOf(defraimp_purposeofconsignment purposeOfConsignment)
        {
            this.powerPlaywrightCtx.Validate();

            var scenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp(a => a
                    .WithType(new Faker().PickRandom(defraimp_importernotificationtype.CVEDA, defraimp_importernotificationtype.CHEDA))
                    .WithPurposeOfConsignment(purposeOfConsignment))
                .BuildAsync();

            var importerNotificationId = scenario.LogicAppSubmitsImporterNotificationEvent.ImporterNotificationId;

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(
                new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId));
        }

        /// <summary>
        /// Submits an Importer Notification, as the EU Imports Notifications Logic App would from an IPAFFS message, with no specific configuration, and adds it to the scenario context for later steps to reference.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("an Importer Notification has been created")]
        public async Task GivenAnImporterNotificationHasBeenCreated()
        {
            var scenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp()
                .BuildAsync();

            this.ctx.Set(scenario);
        }

        /// <summary>
        /// Submits an Importer Notification with searchable field values.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("an Importer Notification has been created with searchable fields populated")]
        public async Task GivenAnImporterNotificationHasBeenCreatedWithSearchableFieldsPopulated()
        {
            var scenario = await new ImporterNotificationScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp(a => a
                    .WithImportingFromCharity())
                .BuildAsync();

            this.ctx.Set(scenario);
        }

        /// <summary>
        /// Searches the active entity list page's data set for the Importer Notification stored in the scenario context, using a value selected at random from one of the given fields.
        /// </summary>
        /// <param name="fields">The candidate fields to search by.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I search for the Importer Notification using one the following fields")]
        public async Task WhenISearchForTheImporterNotificationUsingOneTheFollowingFields(DataTable fields)
        {
            var importerNotification = this.GetCreatedImporterNotification();

            // Not every field is necessarily populated on this Importer Notification (e.g. Charity Name, when it isn't importing from a charity) - only the populated ones are valid search terms.
            var candidateValues = fields.Rows
                .Select(row => SearchFieldValueSelectors[row["Field"]](importerNotification))
                .Where(value => !string.IsNullOrEmpty(value))
                .ToArray();

            if (candidateValues.Length == 0)
            {
                throw new InvalidOperationException("None of the specified fields have a value on the Importer Notification to search by.");
            }

            var searchTerm = candidateValues[new Random().Next(candidateValues.Length)];

            await this.EntityListPage.DataSet.SearchAsync(searchTerm);
        }

        /// <summary>
        /// Asserts that the Importer Notification stored in the scenario context is visible in the active entity list page's search results, identified by its reference number.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the matching Importer Notification in the search results")]
        public async Task ThenISeeTheMatchingImporterNotificationInTheSearchResults()
        {
            var importerNotification = this.GetCreatedImporterNotification();

            var rows = await this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>().GetRowDataAsync();

            rows.Should().Contain(
                row => row.Contains("Reference Number") && row.Get("Reference Number") == importerNotification.defraimp_Name,
                because: $"the search results should include the Importer Notification '{importerNotification.defraimp_Name}'");
        }

        private defraimp_ImporterNotification GetCreatedImporterNotification()
        {
            if (!this.ctx.TryGetValue<ImporterNotificationScenario>(out var scenario))
            {
                throw new InvalidOperationException("No Importer Notification has been created in this scenario. Add a step to create one first.");
            }

            return scenario.LogicAppSubmitsImporterNotificationEvent.ImporterNotification;
        }
    }
}
