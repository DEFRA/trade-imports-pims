namespace Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.TestCases
{
    using System;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Scenarios;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ImporterNotification_OnAction_CreateImportRecordFromNotification_ShouldCreateImportApplication : IntegrationTests
    {
        [TestMethod]
        public void ImporterNotification_CreateImportRecordFromNotification_Should_Create_Import_Application()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new ImporterNotificationWithData(recordService.AggregateId);
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .ExecuteAction(new ActionCreateImportRecordFromNotification(context))
                .AssertAgainst(new ImporterNotificationValidateLinkedImportApplication(context));
        }
    }
}
