
namespace Defra.Imports.IntegrationTests.Dataverse.ImporterNotification
{
    using System;
    using Defra.Imports.IntegrationTests.Dataverse;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Scenarios;
    using Defra.Imports.Model.ReferenceData;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ImporterNotification_OnCreate_ShouldHaveDevolvedOffice : IntegrationTests
    {
        [TestMethod]
        public void ImporterNotification_Should_Set_Devolved_Office_To_IRMSCIT_If_Postcode_Is_English()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new EnglandImporterNotification(recordService.AggregateId);
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationValidateDevolvedOffice(context,Teams.EnglandTeam));
        }

        [TestMethod]
        public void ImporterNotification_Should_Set_Devolved_Office_To_Scotland_If_Postcode_Is_Scottish()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new ScotlandImporterNotification(recordService.AggregateId);
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationValidateDevolvedOffice(context, Teams.ScotlandTeam));
        }

        [TestMethod]
        public void ImporterNotification_Should_Set_Devolved_Office_To_Wales_If_Postcode_Is_Welsh()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new WalesImporterNotification(recordService.AggregateId);
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationValidateDevolvedOffice(context, Teams.WalesTeam));
        }

        [TestMethod]
        public void ImporterNotification_Should_Set_Devolved_Office_To_NONGB_If_Postcode_Is_NonGB()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new NonGBImporterNotification(recordService.AggregateId);
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationValidateDevolvedOffice(context, Teams.NONGBTeam));
        }

        [TestMethod]
        public void ImporterNotification_Should_Set_Devolved_Office_To_Unknown_If_Postcode_Is_Unknown()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new UnknownImporterNotification(recordService.AggregateId);
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationValidateDevolvedOffice(context, Teams.UnknownTeam));
        }
    }
}
