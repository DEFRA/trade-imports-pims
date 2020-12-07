namespace Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.TestCases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Defra.Imports.Tests.Integration.Dynamics.Consignee.SampleRecords;
    using Defra.Imports.Tests.Integration.Dynamics.Consignee.Scenarios;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Assertions;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.SampleRecords;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Scenarios;
    using Defra.Imports.Tests.Integration.Dynamics.PlaceOfDestination.SampleData;
    using Defra.Imports.Tests.Integration.Dynamics.PlaceOfDestination.Scenarios;
    using Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.SampleData;
    using Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.Scenarios;
    using Defra.Imports.Tests.Integration.Dynamics.Transporter.SampleRecords;
    using Defra.Imports.Tests.Integration.Dynamics.Transporter.Scenarios;
    using Defra.Imports.Tests.Integration.Dynamics.WatchList.SampleData;
    using Defra.Imports.Tests.Integration.Dynamics.WatchList.Scenarios;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Xunit;

    public class ImporterNotification_OnCreate_ShouldFlagRecordsOnWatchList : TestCasesBase
    {

        [Fact]
        public void ImporterNotification_Should_Create_WatchFlag_If_PlaceOfOrigin_Is_On_WatchList()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var importNotification = new ImporterNotificationWithData(recordService.AggregateId);
            var placeOfOrigin = new BronzePlaceOfOrigin();
            var watchList = new WatchListWithData(placeOfOrigin.PlaceOfOrigin.Id, defraimp_watchtype.PlaceofOrigin);

            // Set the place of origin name to be the same so it should be flagged. (for notifications the place of origin is the consignor)
            importNotification.ImporterNotification.defraimp_consignorcompanyname = placeOfOrigin.PlaceOfOrigin.defraimp_name;

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, placeOfOrigin.PlaceOfOrigin))
                .CreateRecord(new CreateWatchList(this.context, watchList.WatchList))
                .Delay(3000)
                .CreateRecord(new CreateImporterNotification(this.context, importNotification.ImporterNotification))
                .Delay(3000)
                .AssertAgainst(new ImporterNotificationValidateLinkedWatchFlagRecord(this.context, watchList.WatchList.Id));
        }

        [Fact]
        public void ImporterNotification_Should_Create_WatchFlag_If_PlaceOfDestination_Is_On_WatchList()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var importNotification = new ImporterNotificationWithData(recordService.AggregateId);
            var placeOfDestination = new PlaceOfDestinationWithData();
            var watchList = new WatchListWithData(placeOfDestination.PlaceOfDestination.Id, defraimp_watchtype.PlaceofDestination);

            // Set the place of destination name to be the same so it should get flagged.
            importNotification.ImporterNotification.defraimp_placeofdestinationcompanyname = placeOfDestination.PlaceOfDestination.defraimp_Name;

            recordService
                .CreateRecord(new CreatePlaceOfDestination(this.context, placeOfDestination.PlaceOfDestination))
                .CreateRecord(new CreateWatchList(this.context, watchList.WatchList))
                .Delay(3000)
                .CreateRecord(new CreateImporterNotification(this.context, importNotification.ImporterNotification))
                .Delay(3000)
                .AssertAgainst(new ImporterNotificationValidateLinkedWatchFlagRecord(this.context, watchList.WatchList.Id));
        }

        [Fact]
        public void ImporterNotification_Should_Create_WatchFlag_If_Consignee_Is_On_WatchList()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var importNotification = new ImporterNotificationWithData(recordService.AggregateId);
            var consignee = new ConsigneeWithData();
            var watchList = new WatchListWithData(consignee.Consignee.Id, defraimp_watchtype.Consignee);

            // Set the importer name to be the same as the consignee so it should get flagged.
            importNotification.ImporterNotification.defraimp_importercompanyname = consignee.Consignee.defraimp_Name;

            recordService
            .CreateRecord(new CreateConsignee(this.context, consignee.Consignee))
            .CreateRecord(new CreateWatchList(this.context, watchList.WatchList))
            .Delay(3000)
            .CreateRecord(new CreateImporterNotification(this.context, importNotification.ImporterNotification))
            .Delay(3000)
            .AssertAgainst(new ImporterNotificationValidateLinkedWatchFlagRecord(this.context, watchList.WatchList.Id));
        }

        [Fact]
        public void ImporterNotification_Should_Create_WatchFlag_If_Transporter_Is_On_WatchList()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var importNotification = new ImporterNotificationWithData(recordService.AggregateId);
            var transporter = new TransporterWithData();
            var watchList = new WatchListWithData(transporter.Transporter.Id, defraimp_watchtype.Transporter);

            // Set the importer name to be the same as the consignee so it should get flagged.
            importNotification.ImporterNotification.defraimp_transportercompanyname = transporter.Transporter.defraimp_Name;

            recordService
            .CreateRecord(new CreateTransporter(this.context, transporter.Transporter))
            .CreateRecord(new CreateWatchList(this.context, watchList.WatchList))
            .Delay(3000)
            .CreateRecord(new CreateImporterNotification(this.context, importNotification.ImporterNotification))
            .Delay(3000)
            .AssertAgainst(new ImporterNotificationValidateLinkedWatchFlagRecord(this.context, watchList.WatchList.Id));
        }

    }
}
