
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

    [Collection("RiskRatingTests")]
    public class ImportApplication_OnUpdate_OfTBRiskRating_DetermineInspection : TestCasesBase
    {
        [Fact]
        public void ImportApplication_Should_Require_Risk_Assessment_If_Risk_Rating_Is_TB()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());
            var expectedRiskLevel = RiskLevels.TB;
            var expectedInspectionRequiredValue = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            var expectedInspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.TB;

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

        [Fact]
        public void ImportApplication_Should_Increase_Global_Counter_If_TB()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplicationData = new BasicImportApplication(recordService.AggregateId);
            var sampleImporterNotificationData = new EnglandImporterNotification(Guid.NewGuid());

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
