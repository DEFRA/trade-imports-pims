namespace Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.TestCases
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Assertions;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.SampleRecords;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Scenarios;
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
