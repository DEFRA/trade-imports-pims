namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Bogus;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Scenarios.Events;
    using Defra.Imports.Scenarios.Extensions;
    using Defra.Imports.Specs.Extensions;
    using Defra.Imports.Specs.Services;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using PowerPlaywright.Framework.Controls.Pcf.Classes;
    using PowerPlaywright.Framework.Extensions;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;

    /// <summary>
    /// Steps relating to Import Records.
    /// </summary>
    [Binding]
    public class ImportRecordSteps
    {
        // Maps the free text search fields in AC-3 (US-001) to the Import Record property backing each one.
        private static readonly IReadOnlyDictionary<string, Func<defraimp_importapplication, string>> SearchFieldValueSelectors = new Dictionary<string, Func<defraimp_importapplication, string>>
        {
            ["Importer Name"] = r => r.defraimp_ImporterContactName,
            ["Date of Import"] = r => r.defraimp_DateofImport?.ToString("dd/MM/yyyy"),
            ["Premises of Origin Name"] = r => r.defraimp_PlaceofOriginOrganisation,
            ["ITAHC Certificate Reference Number"] = r => r.defraimp_ITAHCReference,
            ["Importer Notification Local Reference Number"] = r => r.defraimp_LocalReferenceNumber,
        };

        private readonly ServiceClientFactory clientFactory;
        private readonly ILoggerProvider loggerProvider;
        private readonly PowerPlaywrightContext powerPlaywrightCtx;
        private readonly RecordNavigatorService recordNavigator;
        private readonly ScenarioContext ctx;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportRecordSteps"/> class.
        /// </summary>
        /// <param name="clientFactory">The client factory.</param>
        /// <param name="loggerProvider">The logger provider.</param>
        /// <param name="powerPlaywrightCtx">The PowerPlaywright context.</param>
        /// <param name="recordNavigator">The record navigator service.</param>
        /// <param name="ctx">The scenario context.</param>
        public ImportRecordSteps(ServiceClientFactory clientFactory, ILoggerProvider loggerProvider, PowerPlaywrightContext powerPlaywrightCtx, RecordNavigatorService recordNavigator, ScenarioContext ctx)
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
        /// Creates an Import Record from a newly submitted Importer Notification, and navigates to it.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("I have opened an Import Record")]
        public async Task GivenIHaveOpenedAnImportRecord()
        {
            var importRecordId = await this.CreateImportRecordAsync();

            await this.NavigateToImportRecordAsync(importRecordId);
        }

        /// <summary>
        /// Creates an Import Record at Triage with a populated Commodity Code value, and navigates to it.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("I have opened an Import Record at the Triage stage with a Commodity Code value")]
        public async Task GivenIHaveOpenedAnImportRecordAtTheTriageStageWithACommodityCodeValue()
        {
            var scenario = await new ImportRecordScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp()
                .CreatedByCaseworker(c => c.AtTriageStageWithCommodityCode("0102210000"))
                .WithItahcCreatedByCaseworker()
                .BuildAsync();

            var importRecordId = scenario.CaseworkerCreatesImportRecordFromNotificationEvent.ImportRecordId;

            this.ctx.AddOrUpdate(ScenarioContextKeys.ItahcName, scenario.CaseworkerCreatesItahcEvent.Name);

            await this.NavigateToImportRecordAsync(importRecordId);
        }

        /// <summary>
        /// Creates an Import Record from a CHEDA or CVEDA Importer Notification, and navigates to it.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("I have opened an Import Record for a CHED journey")]
        public async Task GivenIHaveOpenedAnImportRecordForAChedJourney()
        {
            await this.NavigateToImportRecordAsync(await this.CreateImportRecordAsync(a => a
                .WithType(new Faker().PickRandom(defraimp_importernotificationtype.CVEDA, defraimp_importernotificationtype.CHEDA))));
        }

        /// <summary>
        /// Creates an Import Record from a CHEDA or CVEDA Importer Notification with the given purpose of consignment, and navigates to it.
        /// </summary>
        /// <param name="purposeOfConsignment">The purpose of consignment.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("^I have opened an Import Record for a CHED journey with a Purpose of Consignment of '(.*)'$")]
        public async Task GivenIHaveOpenedAnImportRecordForAChedJourneyWithAPurposeOfConsignmentOf(defraimp_purposeofconsignment purposeOfConsignment)
        {
            await this.NavigateToImportRecordAsync(await this.CreateImportRecordAsync(a => a
                .WithType(new Faker().PickRandom(defraimp_importernotificationtype.CVEDA, defraimp_importernotificationtype.CHEDA))
                .WithPurposeOfConsignment(purposeOfConsignment)));
        }

        /// <summary>
        /// Creates an Import Record, sets its Import Record Type, and navigates to it.
        /// </summary>
        /// <param name="importRecordType">The Import Record Type.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("I have opened an Import Record with an Import Record Type of {string}")]
        public async Task GivenIHaveOpenedAnImportRecordWithAnImportRecordTypeOf(defraimp_importapplication_defraimp_importapplicationtype importRecordType)
        {
            var importRecordId = await this.CreateImportRecordAsync(importRecord: c => c
                .WithImportRecordType(importRecordType));

            await this.NavigateToImportRecordAsync(importRecordId);
        }

        /// <summary>
        /// Selects the ITAHC created for the scenario in the given lookup fields of the current business process stage.
        /// </summary>
        /// <param name="table">A table with header 'Field'.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I select the ITAHC created for the scenario in the following fields of the current business process stage")]
        public async Task WhenISelectTheItahcInTheFollowingFieldsOfTheCurrentBusinessProcessStage(DataTable table)
        {
            var itahcName = (string)this.ctx[ScenarioContextKeys.ItahcName];
            var recordPage = (IEntityRecordPage)this.powerPlaywrightCtx.ActivePage;

            await recordPage.Form.BusinessProcess.ExecuteStageActionAsync(async fields =>
            {
                foreach (var row in table.Rows)
                {
                    var label = row["Field"];
                    var found = false;

                    foreach (var field in fields)
                    {
                        if (await field.GetLabelAsync() == label)
                        {
                            await field.GetControl<ILookup>().SetValueAsync(itahcName);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        throw new InvalidOperationException($"The current business process stage has no {label} field.");
                    }
                }
            });
        }

        /// <summary>
        /// Opens the Import Record that was created from the Importer Notification opened earlier in the scenario.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I open the Import Record created from the Importer Notification")]
        public async Task WhenIOpenTheImportRecordCreatedFromTheImporterNotification()
        {
            var importerNotificationId = (Guid)this.ctx[ScenarioContextKeys.ImporterNotificationId];
            var client = await this.clientFactory.GetClientAsync(Persona.Caseworker);

            var importRecordId = await client.RetrieveImportRecordCreatedFromNotificationAsync(
                importerNotificationId,
                this.loggerProvider.CreateLogger(nameof(ImportRecordSteps)));

            await this.NavigateToImportRecordAsync(importRecordId);
        }

        /// <summary>
        /// Creates an Import Record with searchable fields populated.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("an Import Record has been created with searchable fields populated")]
        public async Task GivenAnImportRecordHasBeenCreatedWithSearchableFieldsPopulated()
        {
            var importRecordId = await this.CreateImportRecordAsync(importRecord: c => c.WithSearchableFields());
            var client = await this.clientFactory.GetClientAsync(Persona.Caseworker);

            var prepared = (defraimp_importapplication)await client.RetrieveAsync(
                defraimp_importapplication.EntityLogicalName,
                importRecordId,
                new ColumnSet(
                    defraimp_importapplication.Fields.defraimp_name,
                    defraimp_importapplication.Fields.defraimp_ImporterContactName,
                    defraimp_importapplication.Fields.defraimp_DateofImport,
                    defraimp_importapplication.Fields.defraimp_PlaceofOriginOrganisation,
                    defraimp_importapplication.Fields.defraimp_ITAHCReference,
                    defraimp_importapplication.Fields.defraimp_LocalReferenceNumber));

            this.ctx.AddOrUpdate(ScenarioContextKeys.SearchableImportRecord, prepared);
        }

        /// <summary>
        /// Searches for the prepared Import Record using each of the given fields in turn, recording any field whose results do not include it.
        /// </summary>
        /// <param name="fields">The search field labels.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I search for the Import Record using each of the following fields")]
        public async Task WhenISearchForTheImportRecordUsingEachOfTheFollowingFields(DataTable fields)
        {
            var importRecord = (defraimp_importapplication)this.ctx[ScenarioContextKeys.SearchableImportRecord];
            var fieldsWithoutResults = new List<string>();

            foreach (var field in fields.Rows.Select(r => r["Field"]))
            {
                await this.EntityListPage.DataSet.SearchAsync(SearchFieldValueSelectors[field](importRecord));

                try
                {
                    // The grid refreshes after the app reports idle, so re-read it until the record appears.
                    await RetryExtensions.RetryUntilSucceedsAsync(
                        async () =>
                        {
                            await this.EntityListPage.Page.WaitForAppIdleAsync();
                            var rows = await this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>().GetRowDataAsync();

                            if (!rows.Any(r => r.Values.Contains(importRecord.defraimp_name)))
                            {
                                throw new InvalidOperationException($"Searching by {field} did not return {importRecord.defraimp_name}.");
                            }
                        },
                        this.loggerProvider.CreateLogger(nameof(ImportRecordSteps)),
                        retryCount: 5,
                        delay: TimeSpan.FromSeconds(3));
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is TimeoutException)
                {
                    fieldsWithoutResults.Add(field);
                }
            }

            this.ctx.AddOrUpdate(ScenarioContextKeys.FieldsWithoutSearchResults, fieldsWithoutResults);
        }

        /// <summary>
        /// Verifies that searching by every field returned the prepared Import Record.
        /// </summary>
        [Then("I see the matching Import Record in the search results")]
        public void ThenISeeTheMatchingImportRecordInTheSearchResults()
        {
            var fieldsWithoutResults = (List<string>)this.ctx[ScenarioContextKeys.FieldsWithoutSearchResults];

            fieldsWithoutResults.Should().BeEmpty(because: "searching by each of these fields should return the Import Record");
        }

        private async Task<Guid> CreateImportRecordAsync(Action<LogicAppSubmitsImporterNotificationEvent.Builder> notification = null, Action<CaseworkerCreatesImportRecordFromNotificationEvent.Builder> importRecord = null)
        {
            this.powerPlaywrightCtx.Validate();

            var scenario = await new ImportRecordScenario.Builder(this.clientFactory, this.loggerProvider)
                .SubmittedByLogicApp(notification)
                .CreatedByCaseworker(importRecord)
                .BuildAsync();

            return scenario.CaseworkerCreatesImportRecordFromNotificationEvent.ImportRecordId;
        }

        private async Task NavigateToImportRecordAsync(Guid importRecordId)
        {
            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(
                new EntityReference(defraimp_importapplication.EntityLogicalName, importRecordId));
        }
    }
}
