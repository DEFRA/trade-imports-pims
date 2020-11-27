
namespace Defra.Imports.Tests.Integration.Dynamics.ImportApplication
{
    using System;
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;
    using Defra.Imports.Tests.Integration.Dynamics.Autonumber.Assertions;
    using Defra.Imports.Tests.Integration.Dynamics.Autonumber.Scenarios;
    using Defra.Imports.Tests.Integration.Dynamics.ImportApplication.Assertions;
    using Defra.Imports.Tests.Integration.Dynamics.ImportApplication.SampleRecords;
    using Defra.Imports.Tests.Integration.Dynamics.ImportApplication.Scenarios;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.SampleRecords;
    using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Scenarios;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.Xrm.Sdk;
    using Xunit;

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
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p1RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p1QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Cattle))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p1RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p1QuotaCount.Id, 0));
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
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p1RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p1QuotaCount.Id, 0))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Cattle))
                .Delay(5000)
                .ExecuteAction(new SetManualPostImportCheckDecision(context, defraimp_importapplication_defraimp_manualpostimportcheckdecision.DoNotPostImportCheck))
                .Delay(2000)
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p1RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p1QuotaCount.Id, 1));
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
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p1RecordCount.Id, 0))
                .WaitFor(new SetAutonumberValue(context, Autonumbers.p1QuotaCount.Id, 1))
                .CreateRecord(new CreateImporterNotification(context, sampleImporterNotificationData.ImporterNotification))
                .CreateRecord(new CreateImportApplication(context, sampleImportApplicationData.ImportApplication))
                .Delay(2000)
                .ExecuteAction(new AssignImporterNotificationToImportApplication(context, sampleImporterNotificationData.ImporterNotification))
                .Delay(2000)
                .ExecuteAction(new AssignCommodityAndCountryOfOriginToImportApplication(context, Countries.Romania, CommodityTypes.Cattle))
                .Delay(5000)
                .AssertAgainst(new ImportApplicationValidateInspectionRequired(context, expectedRiskLevel, expectedInspectionRequiredValue, expectedInspectionRequiredReason))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p1RecordCount.Id, 0))
                .AssertAgainst(new AutonumberRecordValidateCurrentNumber(context, Autonumbers.p1QuotaCount.Id, 0));
        }
    }
}
