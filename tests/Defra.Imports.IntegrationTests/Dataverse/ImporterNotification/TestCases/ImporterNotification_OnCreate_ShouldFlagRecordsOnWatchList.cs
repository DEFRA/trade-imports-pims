namespace Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.TestCases
{
    using System;
    using Defra.Imports.IntegrationTests.Dataverse;
    using Defra.Imports.IntegrationTests.Dataverse.Consignee.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.Consignee.Scenarios;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Scenarios;
    using Defra.Imports.IntegrationTests.Dataverse.PlaceOfDestination.SampleData;
    using Defra.Imports.IntegrationTests.Dataverse.PlaceOfDestination.Scenarios;
    using Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.SampleData;
    using Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.Scenarios;
    using Defra.Imports.IntegrationTests.Dataverse.Transporter.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.Transporter.Scenarios;
    using Defra.Imports.IntegrationTests.Dataverse.WatchList.SampleData;
    using Defra.Imports.IntegrationTests.Dataverse.WatchList.Scenarios;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ImporterNotification_OnCreate_ShouldFlagRecordsOnWatchList : IntegrationTests
    {

        [TestMethod]
        public void ImporterNotification_Should_Create_WatchFlag_If_PlaceOfOrigin_Is_On_WatchList()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var importNotification = new ImporterNotificationWithData(recordService.AggregateId);
            var placeOfOrigin = new BronzePlaceOfOrigin();
            var watchList = new WatchListWithData(placeOfOrigin.PlaceOfOrigin.Id, defraimp_watchtype.PlaceofOrigin);

            // Set the place of origin name to be the same so it should be flagged. (for notifications the place of origin is the consignor)
            importNotification.ImporterNotification.defraimp_consignorcompanyname = placeOfOrigin.PlaceOfOrigin.defraimp_name;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, placeOfOrigin.PlaceOfOrigin))
                .CreateRecord(new CreateWatchList(context, watchList.WatchList))
                .Delay(3000)
                .CreateRecord(new CreateImporterNotification(context, importNotification.ImporterNotification))
                .Delay(3000)
                .AssertAgainst(new ImporterNotificationValidateLinkedWatchFlagRecord(context, watchList.WatchList.Id));
        }

        [TestMethod]
        public void ImporterNotification_Should_Create_WatchFlag_If_PlaceOfDestination_Is_On_WatchList()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var importNotification = new ImporterNotificationWithData(recordService.AggregateId);
            var placeOfDestination = new PlaceOfDestinationWithData();
            var watchList = new WatchListWithData(placeOfDestination.PlaceOfDestination.Id, defraimp_watchtype.PlaceofDestination);

            // Set the place of destination name to be the same so it should get flagged.
            importNotification.ImporterNotification.defraimp_placeofdestinationcompanyname = placeOfDestination.PlaceOfDestination.defraimp_Name;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfDestination(context, placeOfDestination.PlaceOfDestination))
                .CreateRecord(new CreateWatchList(context, watchList.WatchList))
                .Delay(3000)
                .CreateRecord(new CreateImporterNotification(context, importNotification.ImporterNotification))
                .Delay(3000)
                .AssertAgainst(new ImporterNotificationValidateLinkedWatchFlagRecord(context, watchList.WatchList.Id));
        }

        [TestMethod]
        public void ImporterNotification_Should_Create_WatchFlag_If_Consignee_Is_On_WatchList()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var importNotification = new ImporterNotificationWithData(recordService.AggregateId);
            var consignee = new ConsigneeWithData();
            var watchList = new WatchListWithData(consignee.Consignee.Id, defraimp_watchtype.Consignee);

            // Set the importer name to be the same as the consignee so it should get flagged.
            importNotification.ImporterNotification.defraimp_importercompanyname = consignee.Consignee.defraimp_Name;
            var context = this.GetAppUserContext();

            recordService
            .CreateRecord(new CreateConsignee(context, consignee.Consignee))
            .CreateRecord(new CreateWatchList(context, watchList.WatchList))
            .Delay(3000)
            .CreateRecord(new CreateImporterNotification(context, importNotification.ImporterNotification))
            .Delay(3000)
            .AssertAgainst(new ImporterNotificationValidateLinkedWatchFlagRecord(context, watchList.WatchList.Id));
        }

        [TestMethod]
        public void ImporterNotification_Should_Create_WatchFlag_If_Transporter_Is_On_WatchList()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var importNotification = new ImporterNotificationWithData(recordService.AggregateId);
            var transporter = new TransporterWithData();
            var watchList = new WatchListWithData(transporter.Transporter.Id, defraimp_watchtype.Transporter);

            // Set the importer name to be the same as the consignee so it should get flagged.
            importNotification.ImporterNotification.defraimp_transportercompanyname = transporter.Transporter.defraimp_Name;
            var context = this.GetAppUserContext();

            recordService
            .CreateRecord(new CreateTransporter(context, transporter.Transporter))
            .CreateRecord(new CreateWatchList(context, watchList.WatchList))
            .Delay(3000)
            .CreateRecord(new CreateImporterNotification(context, importNotification.ImporterNotification))
            .Delay(3000)
            .AssertAgainst(new ImporterNotificationValidateLinkedWatchFlagRecord(context, watchList.WatchList.Id));
        }

    }
}
