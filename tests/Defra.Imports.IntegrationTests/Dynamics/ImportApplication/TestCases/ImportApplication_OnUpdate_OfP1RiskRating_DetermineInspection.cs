namespace Defra.Imports.IntegrationTests.Dynamics.ImportApplication.TestCases
{
    using Defra.Imports.IntegrationTests.Dynamics.Autonumber.Assertions;
    using Defra.Imports.IntegrationTests.Dynamics.Autonumber.Scenarios;
    using Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Assertions;
    using Defra.Imports.IntegrationTests.Dynamics.ImportApplication.SampleRecords;
    using Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Scenarios;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.SampleRecords;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Scenarios;
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;
    using Xunit;

    [Collection("RiskRatingTests")]
    public class ImportApplication_OnUpdate_OfP1RiskRating_DetermineInspection : TestCasesBase
    {
        [Fact]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Risk_Rating_Is_P1()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection;

            recordService
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p1RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p1QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Cattle))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p1RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p1QuotaCount.Id, 0));
        }

        /// <summary>
        /// When we test skipping inspections, we cannot assert against the Inspection Required and Inspection Required Reason values as they are driven by JS.
        /// We can however ensure that the counter and quota values are correct.
        /// </summary>
        [Fact]
        public void ImportApplication_Should_Set_Quota_When_P1_Skips_Inspection()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());

            recordService
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p1RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p1QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Cattle))
                .Delay(5000)
                .ExecuteAction(new SetManualPostImportCheckDecision(this.context, defraimp_importapplication_defraimp_manualpostimportcheckdecision.DoNotPostImportCheck))
                .Delay(2000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p1RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p1QuotaCount.Id, 1));
        }

        [Fact]
        public void ImportApplication_Should_Use_Quota_When_Above_0_And_Application_Risk_Is_P1()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection;

            recordService
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p1RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p1QuotaCount.Id, 1))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Cattle))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p1RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p1QuotaCount.Id, 0));
        }

        [Fact]
        public void ImportApplication_Should_Increase_Global_Count_If_P1()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());

            recordService
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p3RecordCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Cattle))
                .Delay(5000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p3RecordCount.Id, 1));
        }
    }
}
