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
        private const string PermanentAddressAlias = "created-permanent-address";
        private const string PermanentAddressLogicalName = "defraimp_notificationpermanentaddress";

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

            // US-003 AC-3 names a Permanent Destination Name as a searchable field. This is held on
            // the related defraimp_notificationpermanentaddress table rather than on the
            // notification itself, so a linked record is seeded below and the value is searched for
            // in the same way as every other criterion.
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

            // The Permanent Destination details named by AC-1 and AC-3 are held on a related record
            // rather than on the notification, so one is seeded and linked to the notification.
            var permanentAddress = new Entity(PermanentAddressLogicalName)
            {
                ["defraimp_name"] = permanentDestinationName,
                ["defraimp_individualname"] = permanentDestinationName,
                ["defraimp_addressline1"] = "6 Permanent Close",
                ["defraimp_addresscity"] = "Permanent City",
                ["defraimp_addresspostalzipcode"] = "PE1 8RM",
                ["defraimp_addresstelephone"] = "01615550505",
                ["defraimp_addressemail"] = "updated.permanent@email.com",
                ["defraimp_importernotificationid"] = recordRef,
            };

            var permanentAddressId = this.serviceClient.Create(permanentAddress);

            this.testDataService.AddRecord(new EntityReference(PermanentAddressLogicalName, permanentAddressId), PermanentAddressAlias);
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
            this.outputHelper.WriteLine($"Created linked permanent address {permanentAddressId} named '{permanentDestinationName}'.");
        }
    }
}
