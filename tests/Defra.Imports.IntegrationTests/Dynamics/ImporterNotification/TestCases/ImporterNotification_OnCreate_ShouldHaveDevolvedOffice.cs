
namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification
{
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.SampleRecords;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Scenarios;
    using Defra.Imports.Model.ReferenceData;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;
    using Xunit;

    public class ImporterNotification_OnCreate_ShouldHaveDevolvedOffice : TestCasesBase
    {
        [Fact]
        public void ImporterNotification_Should_Set_Devolved_Office_To_IRMSCIT_If_Postcode_Is_English()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new EnglandImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationValidateDevolvedOffice(context,Teams.EnglandTeam));
        }

        [Fact]
        public void ImporterNotification_Should_Set_Devolved_Office_To_Scotland_If_Postcode_Is_Scottish()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new ScotlandImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationValidateDevolvedOffice(context, Teams.ScotlandTeam));
        }

        [Fact]
        public void ImporterNotification_Should_Set_Devolved_Office_To_Wales_If_Postcode_Is_Welsh()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new WalesImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationValidateDevolvedOffice(context, Teams.WalesTeam));
        }

        [Fact]
        public void ImporterNotification_Should_Set_Devolved_Office_To_NONGB_If_Postcode_Is_NonGB()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new NonGBImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationValidateDevolvedOffice(context, Teams.NONGBTeam));
        }

        [Fact]
        public void ImporterNotification_Should_Set_Devolved_Office_To_Unknown_If_Postcode_Is_Unknown()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new UnknownImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationValidateDevolvedOffice(context, Teams.UnknownTeam));
        }
    }
}
