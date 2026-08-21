namespace Defra.Imports.IntegrationTests.Dataverse.ImportApplication.TestCases
{
    using System;
    using Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.ImportApplication.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Scenarios;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Scenarios;
    using Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.SampleData;
    using Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.Scenarios;
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ImportApplication_OnUpdate_OfP1GBRiskRating_DetermineInspection : IntegrationTests
    {
        [TestMethod]
        public void ImportApplication_Should_Not_Risk_Assess_If_Risk_Rating_Is_P1_And_Commodity_Is_GoldBronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Discretionary;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.VerifiedPlaceofOriginMissing;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason));
        }

        [TestMethod]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Place_Of_Origin_Is_Bronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin bronzePlaceOfOrigin = new BronzePlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, bronzePlaceOfOrigin))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(context, bronzePlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, bronzePlaceOfOrigin));
        }

        [TestMethod]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Place_Of_Origin_Is_Locked_To_Bronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin bronzePlaceOfOrigin = new LockedToBronzePlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.PlaceofOriginLockedtoBronze;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, bronzePlaceOfOrigin))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(context, bronzePlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, bronzePlaceOfOrigin));
        }

        [TestMethod]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Place_Of_Origin_Is_Bronze_And_Should_Not_Increase_Counter()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin bronzePlaceOfOrigin = new BronzePlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, bronzePlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(context, bronzePlaceOfOrigin.Id, 0))
                .WaitFor(new SetQuotaCounter(context, bronzePlaceOfOrigin.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(context, bronzePlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(context, bronzePlaceOfOrigin.Id, 0, 0))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, bronzePlaceOfOrigin));
        }

        [TestMethod]
        public void ImportApplication_Should_Not_Count_Bronze_Records()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin bronzePlaceOfOrigin = new BronzePlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, bronzePlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(context, bronzePlaceOfOrigin.Id, 0))
                .WaitFor(new SetQuotaCounter(context, bronzePlaceOfOrigin.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(context, bronzePlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(context, bronzePlaceOfOrigin.Id, 0, 0))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, bronzePlaceOfOrigin));
        }

        [TestMethod]
        public void ImportApplication_Should_Not_Require_Risk_Assessment_If_Place_Of_Origin_Is_Gold_And_Application_Count_Is_Less_Than_10()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin goldPlaceOfOrigin = new GoldPlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.No;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequiredGoldPlaceofOrigin;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, goldPlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(context, goldPlaceOfOrigin.Id, 0))
                .WaitFor(new SetQuotaCounter(context, goldPlaceOfOrigin.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(context, goldPlaceOfOrigin))
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(context, goldPlaceOfOrigin.Id, 1, 0))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, goldPlaceOfOrigin));
        }

        [TestMethod]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Place_Of_Origin_Is_Gold_And_Application_Count_Is_10()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin goldPlaceOfOrigin = new GoldPlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, goldPlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(context, goldPlaceOfOrigin.Id, 9))
                .WaitFor(new SetQuotaCounter(context, goldPlaceOfOrigin.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(context, goldPlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(context, goldPlaceOfOrigin.Id, 0, 0))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, goldPlaceOfOrigin));
        }

        [TestMethod]
        public void ImportApplication_Should_Require_Inspection_If_Place_Of_Origin_Quota_Value_Is_Above_0()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin goldPlaceOfOrigin = new GoldPlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, goldPlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(context, goldPlaceOfOrigin.Id, 0))
                .WaitFor(new SetQuotaCounter(context, goldPlaceOfOrigin.Id, 1))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(context, goldPlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(context, goldPlaceOfOrigin.Id, 0, 0))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, goldPlaceOfOrigin));
        }

        [TestMethod]
        public void ImportApplication_Should_Increase_Place_Of_Origin_Inspection_Quota_If_Trust_Level_Is_Gold_And_Inspection_Is_Skipped()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin goldPlaceOfOrigin = new GoldPlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(context, goldPlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(context, goldPlaceOfOrigin.Id, 9))
                .WaitFor(new SetQuotaCounter(context, goldPlaceOfOrigin.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(context, goldPlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .ExecuteAction(new SetManualPostImportCheckDecision(context, defraimp_importapplication_defraimp_manualpostimportcheckdecision.DoNotPostImportCheck))
                .Delay(5000)
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(context, goldPlaceOfOrigin.Id, 0, 1))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, goldPlaceOfOrigin));
        }
    }
}
