namespace Defra.Imports.Tests.Integration.Dynamics.ImportApplication.TestCases
{
    using System;
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;
    using Defra.Imports.Repositories;
    using Defra.Imports.Tests.Integration.Dynamics.ImportApplication.Assertions;
    using Defra.Imports.Tests.Integration.Dynamics.ImportApplication.SampleRecords;
    using Defra.Imports.Tests.Integration.Dynamics.ImportApplication.Scenarios;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.SampleRecords;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Scenarios;
    using Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.Assertions;
    using Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.SampleData;
    using Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.Scenarios;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Xunit;

    [Collection("RiskRatingTests")]
    public class ImportApplication_OnUpdate_OfP1GBRiskRating_DetermineInspection : TestCasesBase
    {
        [Fact]
        public void ImportApplication_Should_Not_Risk_Assess_If_Risk_Rating_Is_P1_And_Commodity_Is_GoldBronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Discretionary;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.VerifiedPlaceofOriginMissing;

            recordService
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason));
        }

        [Fact]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Place_Of_Origin_Is_Bronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin bronzePlaceOfOrigin = new BronzePlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin;

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, bronzePlaceOfOrigin))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(this.context, bronzePlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(this.context, bronzePlaceOfOrigin));
        }

        [Fact]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Place_Of_Origin_Is_Locked_To_Bronze()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin bronzePlaceOfOrigin = new LockedToBronzePlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.PlaceofOriginLockedtoBronze;

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, bronzePlaceOfOrigin))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(this.context, bronzePlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(this.context, bronzePlaceOfOrigin));
        }

        [Fact]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Place_Of_Origin_Is_Bronze_And_Should_Not_Increase_Counter()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin bronzePlaceOfOrigin = new BronzePlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin;

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, bronzePlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(this.context, bronzePlaceOfOrigin.Id, 0))
                .WaitFor(new SetQuotaCounter(this.context, bronzePlaceOfOrigin.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(this.context, bronzePlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(this.context, bronzePlaceOfOrigin.Id, 0, 0))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(this.context, bronzePlaceOfOrigin));
        }

        [Fact]
        public void ImportApplication_Should_Not_Count_Bronze_Records()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin bronzePlaceOfOrigin = new BronzePlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, bronzePlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(this.context, bronzePlaceOfOrigin.Id, 0))
                .WaitFor(new SetQuotaCounter(this.context, bronzePlaceOfOrigin.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(this.context, bronzePlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(this.context, bronzePlaceOfOrigin.Id, 0, 0))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(this.context, bronzePlaceOfOrigin));
        }

        [Fact]
        public void ImportApplication_Should_Not_Require_Risk_Assessment_If_Place_Of_Origin_Is_Gold_And_Application_Count_Is_Less_Than_10()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin goldPlaceOfOrigin = new GoldPlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.No;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequiredGoldPlaceofOrigin;

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, goldPlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(this.context, goldPlaceOfOrigin.Id, 0))
                .WaitFor(new SetQuotaCounter(this.context, goldPlaceOfOrigin.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(this.context, goldPlaceOfOrigin))
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(this.context, goldPlaceOfOrigin.Id, 1, 0))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(context, goldPlaceOfOrigin));
        }

        [Fact]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Place_Of_Origin_Is_Gold_And_Application_Count_Is_10()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin goldPlaceOfOrigin = new GoldPlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage;

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, goldPlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(this.context, goldPlaceOfOrigin.Id, 9))
                .WaitFor(new SetQuotaCounter(this.context, goldPlaceOfOrigin.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(this.context, goldPlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(this.context, goldPlaceOfOrigin.Id, 0, 0))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(this.context, goldPlaceOfOrigin));
        }

        [Fact]
        public void ImportApplication_Should_Require_Inspection_If_Place_Of_Origin_Quota_Value_Is_Above_0()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin goldPlaceOfOrigin = new GoldPlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.P1;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage;

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, goldPlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(this.context, goldPlaceOfOrigin.Id, 0))
                .WaitFor(new SetQuotaCounter(this.context, goldPlaceOfOrigin.Id, 1))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(this.context, goldPlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(this.context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(this.context, goldPlaceOfOrigin.Id, 0, 0))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(this.context, goldPlaceOfOrigin));
        }

        [Fact]
        public void ImportApplication_Should_Increase_Place_Of_Origin_Inspection_Quota_If_Trust_Level_Is_Gold_And_Inspection_Is_Skipped()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            defraimp_placeoforigin goldPlaceOfOrigin = new GoldPlaceOfOrigin().PlaceOfOrigin;
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());

            recordService
                .CreateRecord(new CreatePlaceOfOrigin(this.context, goldPlaceOfOrigin))
                .Delay(2000)
                .WaitFor(new SetApplicationCounter(this.context, goldPlaceOfOrigin.Id, 9))
                .WaitFor(new SetQuotaCounter(this.context, goldPlaceOfOrigin.Id, 0))
                .CreateRecord(new CreateImporterNotification(this.context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(this.context, sampleImporterNotificationData.ImporterNotification))
                .ExecuteAction(new AssignPlaceOfOriginToImportApplication(this.context, goldPlaceOfOrigin))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(this.context, Countries.Romania, CommodityTypes.Dog))
                .Delay(5000)
                .ExecuteAction(new SetManualPostImportCheckDecision(this.context, defraimp_importapplication_defraimp_manualpostimportcheckdecision.DoNotPostImportCheck))
                .Delay(5000)
                .AssertAgainst(new PlaceOfOriginValidateApplicationCounter(this.context, goldPlaceOfOrigin.Id, 0, 1))
                .Delay(2000)
                .ExecuteAction(new DeletePlaceOfOrigin(this.context, goldPlaceOfOrigin));
        }
    }
}
