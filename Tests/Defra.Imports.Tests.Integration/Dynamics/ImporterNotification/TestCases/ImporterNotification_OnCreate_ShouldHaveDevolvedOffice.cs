
namespace Defra.Imports.Tests.Integration.Dynamics.ImporterNotification
{
    using System;
    using Defra.Imports.Model.Constants;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Assertions;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.SampleRecords;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Scenarios;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Xunit;

    public class ImporterNotification_OnCreate_ShouldHaveDevolvedOffice : TestCasesBase
    {
        [Fact]
        public void ImporterNotification_Should_Set_Devolved_Office_To_IRMSCIT_If_Postcode_Is_English()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new EnglandImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportApplicationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationHasCorrectDevolvedOffice(context,TeamConstants.EnglandTeam));
        }

        [Fact]
        public void ImporterNotification_Should_Set_Devolved_Office_To_Scotland_If_Postcode_Is_Scottish()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new ScotlandImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportApplicationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationHasCorrectDevolvedOffice(context, TeamConstants.ScotlandTeam));
        }

        [Fact]
        public void ImporterNotification_Should_Set_Devolved_Office_To_Wales_If_Postcode_Is_Welsh()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new WalesImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportApplicationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationHasCorrectDevolvedOffice(context, TeamConstants.WalesTeam));
        }

        [Fact]
        public void ImporterNotification_Should_Set_Devolved_Office_To_NONGB_If_Postcode_Is_NonGB()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new NonGBImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportApplicationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationHasCorrectDevolvedOffice(context, TeamConstants.NONGBTeam));
        }

        [Fact]
        public void ImporterNotification_Should_Set_Devolved_Office_To_Unknown_If_Postcode_Is_Unknown()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new UnknownImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportApplicationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporterNotificationHasCorrectDevolvedOffice(context, TeamConstants.UnknownTeam));
        }
    }
}
