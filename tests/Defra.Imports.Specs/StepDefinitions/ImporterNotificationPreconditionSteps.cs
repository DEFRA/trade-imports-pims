namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.Model;
    using Defra.Imports.Specs.Extensions;
    using Defra.Imports.Specs.Services;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;
    using Reqnroll;

    /// <summary>
    /// Step definitions for programmatic Importer Notification preconditions.
    /// </summary>
    [Binding]
    public class ImporterNotificationPreconditionSteps
    {
        private const string DefaultRecordAlias = "created-importer-notification";

        private readonly ServiceClient serviceClient;
        private readonly TestDataService testDataService;
        private readonly ScenarioContext scenarioContext;
        private readonly IReqnrollOutputHelper outputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImporterNotificationPreconditionSteps"/> class.
        /// </summary>
        /// <param name="serviceClient">The app user service client.</param>
        /// <param name="testDataService">The test data service.</param>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <param name="outputHelper">The output helper.</param>
        public ImporterNotificationPreconditionSteps(ServiceClient serviceClient, TestDataService testDataService, ScenarioContext scenarioContext, IReqnrollOutputHelper outputHelper)
        {
            this.serviceClient = serviceClient;
            this.testDataService = testDataService;
            this.scenarioContext = scenarioContext;
            this.outputHelper = outputHelper;
        }

        /// <summary>
        /// Creates an Importer Notification precondition record using API automation.
        /// </summary>
        [Given("a precondition Importer Notification exists")]
        public void GivenAPreconditionImporterNotificationExists()
        {
            var uniqueSuffix = Guid.NewGuid().ToString("N").Substring(0, 6).ToUpperInvariant();
            var referenceNumber = $"CHEDA.GB.{DateTime.UtcNow:yyyy}.{DateTime.UtcNow:HHmmssfff}{uniqueSuffix}";
            var importerName = $"Auto Importer {DateTime.UtcNow:HHmmss}";
            var commodityId = $"AUTO-COM-{Guid.NewGuid():N}";
            var charityName = $"Auto Charity {uniqueSuffix}";
            var premisesOfOriginName = $"Auto Premises of Origin {uniqueSuffix}";
            var animalProductId = $"AUTO-ANIMAL-{uniqueSuffix}";

            // US-003 AC-3 names a Permanent Destination Name as a searchable field. There is no
            // permanent destination name attribute on defraimp_importernotification, so the value
            // below cannot be seeded against the record and no search can match it.
            var permanentDestinationName = $"Auto Permanent Destination {uniqueSuffix}";

            var importerNotification = new Entity(defraimp_ImporterNotification.EntityLogicalName)
            {
                ["defraimp_name"] = referenceNumber,
                ["defraimp_importercompanyname"] = importerName,
                ["defraimp_commodityid"] = commodityId,
                ["defraimp_consignortwocompanyname"] = charityName,
                ["defraimp_consignorcompanyname"] = premisesOfOriginName,
                ["defraimp_identificationofanimalstext"] = animalProductId,
            };

            var recordId = this.serviceClient.Create(importerNotification);
            var recordRef = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, recordId);

            this.testDataService.AddRecord(recordRef, DefaultRecordAlias);
            this.scenarioContext.AddOrUpdate(ScenarioContextKeys.CreatedImporterNotificationId, recordId);
            this.scenarioContext.AddOrUpdate(ScenarioContextKeys.CreatedImporterNotificationReferenceNumber, referenceNumber);
            this.scenarioContext.AddOrUpdate(ScenarioContextKeys.CreatedImporterNotificationSearchToken, referenceNumber);
            this.scenarioContext.AddOrUpdate(
                ScenarioContextKeys.CreatedImporterNotificationSearchValues,
                new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Importer Name", importerName },
                    { "Charity Name", charityName },
                    { "Premises of Origin Name", premisesOfOriginName },
                    { "Permanent Destination Name", permanentDestinationName },
                    { "Animal / Product ID", animalProductId },
                });

            this.outputHelper.WriteLine($"Created precondition Importer Notification {recordId} with reference '{referenceNumber}' and commodity token '{commodityId}'.");
        }
    }
}
