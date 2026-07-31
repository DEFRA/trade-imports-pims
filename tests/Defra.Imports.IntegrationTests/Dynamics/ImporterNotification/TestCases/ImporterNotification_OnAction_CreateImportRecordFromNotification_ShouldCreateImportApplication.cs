namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.TestCases
{
    using System;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.SampleRecords;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Scenarios;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Xunit;

    public class ImporterNotification_OnAction_CreateImportRecordFromNotification_ShouldCreateImportApplication : TestCasesBase
    {
        [Fact]
        public void ImporterNotification_CreateImportRecordFromNotification_Should_Create_Import_Application()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new ImporterNotificationWithData(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(this.context, sampleImportNotificationData.ImporterNotification))
                .ExecuteAction(new ActionCreateImportRecordFromNotification(this.context))
                .AssertAgainst(new ImporterNotificationValidateLinkedImportApplication(this.context));
        }
    }
}
