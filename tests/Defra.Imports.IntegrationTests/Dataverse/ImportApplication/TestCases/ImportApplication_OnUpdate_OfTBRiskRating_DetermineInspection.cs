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
    public class ImportApplication_OnUpdate_OfTBRiskRating_DetermineInspection : IntegrationTests
    {
        [TestMethod]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Risk_Rating_Is_TB()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.TB;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.TB;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.RepublicOfIreland, CommodityTypes.Cattle))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason));
        }

        [TestMethod]
        public void ImportApplication_Should_Increase_Global_Counter_If_TB()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var context = this.GetAppUserContext();

            recordService
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p3RecordCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.RepublicOfIreland, CommodityTypes.Cattle))
                .Delay(5000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p3RecordCount.Id, 1));
        }
    }
}
