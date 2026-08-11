
namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification
{
    using System;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.SampleRecords;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Scenarios;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Xunit;

    public class ImporterNotification_AfterCreate_ShouldFlagHealthCheckDocument : TestCasesBase
    {
        [Fact]
        public void ImporterNotification_Should_Set_HealthCheck_Flag_if_HealthCheck_Document_Created()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportNotificationData = new EnglandImporterNotification(recordService.AggregateId);

            recordService
                .CreateRecord(new CreateImporterNotification(this.context, sampleImportNotificationData.ImporterNotification))
                .Delay(10000)
                .AssertAgainst(new ImporNotificationValidateHealthCheckFlag(this.context, false))
                .ExecuteAction(new CreateIPAFFSHealthCheckDocument(this.context))
                .AssertAgainst(new ImporNotificationValidateHealthCheckFlag(this.context, true));
        }
    }
}
