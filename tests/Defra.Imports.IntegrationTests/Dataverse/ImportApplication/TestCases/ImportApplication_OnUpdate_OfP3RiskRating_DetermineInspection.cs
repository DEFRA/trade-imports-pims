namespace Defra.Imports.IntegrationTests.Dataverse.ImportApplication.TestCases
{
    using System;
    using Defra.Imports.IntegrationTests.Dataverse.Autonumber.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.Autonumber.Scenarios;
    using Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.ImportApplication.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Scenarios;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Scenarios;
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    [DoNotParallelize]
    public class ImportApplication_OnUpdate_OfP3RiskRating_DetermineInspection : IntegrationTests
    {
        [TestMethod]
        public void ImportApplication_Should_Not_Require_Risk_Assessment_If_Risk_Rating_Is_P3_And_P3_Global_Count_Is_Less_Than_50()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P3;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.No;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired;
            var context = this.GetAppUserContext();

            recordService
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Germany, CommodityTypes.Pig))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3RecordCount.Id, 1))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3QuotaCount.Id, 0));
        }

        [TestMethod]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Risk_Rating_Is_P3_And_Count_Is_50()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P3;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP3Inspection;
            var context = this.GetAppUserContext();

            recordService
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3RecordCount.Id, 49))
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Germany, CommodityTypes.Pig))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3QuotaCount.Id, 0));
        }

        [TestMethod]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Risk_Rating_Is_P3_And_Quota_Is_Over_0()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P3;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP3Inspection;
            var context = this.GetAppUserContext();

            recordService
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3QuotaCount.Id, 2))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Germany, CommodityTypes.Pig))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3QuotaCount.Id, 1));
        }

        [TestMethod]
        public void ImportApplication_Should_Set_Quota_When_P3_Skips_Inspection()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var context = this.GetAppUserContext();

            recordService
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3RecordCount.Id, 49))
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Germany, CommodityTypes.Pig))
                .Delay(5000)
                .ExecuteAction(new SetManualPostImportCheckDecision(context, defraimp_importapplication_defraimp_manualpostimportcheckdecision.DoNotPostImportCheck))
                .Delay(2000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3QuotaCount.Id, 1));
        }

        [TestMethod]
        public void ImportApplication_Should_Decrease_P3_Count_If_Risk_Level_Is_Removed()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var context = this.GetAppUserContext();

            recordService
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Germany, CommodityTypes.Pig))
                .Delay(5000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3RecordCount.Id, 1))
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, null, null))
                .Delay(5000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3RecordCount.Id, 0));
        }
    }
}