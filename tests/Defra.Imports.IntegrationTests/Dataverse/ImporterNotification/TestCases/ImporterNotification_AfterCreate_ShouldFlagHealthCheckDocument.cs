
namespace Defra.Imports.IntegrationTests.Dataverse.ImporterNotification
{
    using System;
    using Defra.Imports.IntegrationTests.Dataverse;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Scenarios;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ImporterNotification_AfterCreate_ShouldFlagHealthCheckDocument : IntegrationTests
    {
        [TestMethod]
        public void ImporterNotification_Should_Set_HealthCheck_Flag_if_HealthCheck_Document_Created()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new EnglandImporterNotification(recordService.AggregateId);
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporNotificationValidateHealthCheckFlag(context, false))
                .ExecuteAction(new CreateIPAFFSHealthCheckDocument(context))
                .AssertAgainst(new ImporNotificationValidateHealthCheckFlag(context, true));
        }
    }
}
