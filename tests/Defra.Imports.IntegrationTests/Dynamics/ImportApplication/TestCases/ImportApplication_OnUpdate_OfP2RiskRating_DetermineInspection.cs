namespace Defra.Imports.IntegrationTests.Dynamics.ImportApplication.TestCases
{
    using System;
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
    using Xunit;

    [Collection("RiskRatingTests")]
    public class ImportApplication_OnUpdate_OfP2RiskRating_DetermineInspection : TestCasesBase
    {
        [Fact]
        public void ImportApplication_Should_Not_Require_Risk_Assessment_If_Risk_Rating_Is_P2_And_Count_Should_Be_1()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P2;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.No;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired;

            recordService
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p2RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p2QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.France, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p2RecordCount.Id, 1))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p2QuotaCount.Id, 0));
        }

        [Fact]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Risk_Rating_Is_P2_And_Count_Is_10()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P2;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP2Inspection;

            recordService
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p2RecordCount.Id, 9))
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p2QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.France, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p2RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p2QuotaCount.Id, 0));
        }

        [Fact]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Risk_Rating_Is_P2_And_Quota_Is_Over_0()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P2;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP2Inspection;

            recordService
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p2RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p2QuotaCount.Id, 2))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.France, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p2RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p2QuotaCount.Id, 1));
        }

        [Fact]
        public void ImportApplication_Should_Set_Quota_When_P2_Skips_Inspection()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());

            recordService
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p2RecordCount.Id, 9))
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p2QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.France, CommodityTypes.Dog))
                .Delay(5000)
                .ExecuteAction(new SetManualPostImportCheckDecision(this.context, defraimp_importapplication_defraimp_manualpostimportcheckdecision.DoNotPostImportCheck))
                .Delay(2000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p2RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p2QuotaCount.Id, 1));
        }

        [Fact]
        public void ImportApplication_Should_Increase_Global_Count_If_P2()
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
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.France, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p3RecordCount.Id, 1));
        }

        [Fact]
        public void ImportApplication_Should_Decrease_Global_Count_If_P2_And_Risk_Level_Is_Removed()
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
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.France, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p3RecordCount.Id, 1))
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, null, null))
                .Delay(5000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(this.context, Autonumbers.p3RecordCount.Id, 0));
        }

        [Fact]
        public void ImportApplication_Should_Not_Require_Inspection_If_Global_Count_Is_100()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P2;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.No;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired;

            recordService
                .WaitFor(new SetAutonumberValue(this.context, Autonumbers.p3RecordCount.Id, 100))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.France, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason));
        }
    }
}
