namespace Defra.Imports.IntegrationTests.Dataverse.PostImportCheck.TestCases
{
    using System;
    using Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.ImportApplication.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Scenarios;
    using Defra.Imports.IntegrationTests.Dataverse.PostImportCheck.Assertions;
    using Defra.Imports.IntegrationTests.Dataverse.PostImportCheck.SampleRecords;
    using Defra.Imports.IntegrationTests.Dataverse.PostImportCheck.Scenarios;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.Xrm.Sdk;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PostImportCheck_OnUpdate_Should_UpdateOutcome : IntegrationTests
    {
        [TestMethod]
        public void Post_Import_Check_Should_Update_Related_Import_Application_Inspection_Outcome_When_Outcome_Is_Set()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplication = new BasicImportApplication(recordService.AggregateId).ImportApplication;
            var importApplicationReference = new EntityReference(sampleImportApplication.LogicalName, sampleImportApplication.Id);
            var samplePostImportCheck= new BasicPostImportCheck(Guid.NewGuid(), importApplicationReference).PostImportCheck;
            var expectedPostImportCheckOutcome = defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory;
            var expectedApplicationOutcome = defraimp_importapplication_defraimp_inspectionoutcome.Satisfactory;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImportApplication(context, sampleImportApplication))
                .Delay(5000)
                .ExecuteAction(new SetApplicationInspectionOutcomeValue(context, defraimp_importapplication_defraimp_inspectionoutcome.AwaitingResultofChecks))
                .CreateRecord(new CreatePostImportCheck(context, samplePostImportCheck))
                .Delay(2000)
                .ExecuteAction(new SetPostImportCheckOutcome(context, samplePostImportCheck, expectedPostImportCheckOutcome))
                .Delay(2000)
                .AssertAgainst(new PostImportCheckValidateOutcome(context, samplePostImportCheck.Id, expectedPostImportCheckOutcome))
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(context, expectedApplicationOutcome));
        }

        [TestMethod]
        public void Post_Import_Check_Should_Set_Import_Application_Satisfactory_When_All_Checks_Are_Marked_Satisfactory()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplication = new BasicImportApplication(recordService.AggregateId).ImportApplication;
            var importApplicationReference = new EntityReference(sampleImportApplication.LogicalName, sampleImportApplication.Id);

            var initialExpectedApplicationOutcome = defraimp_importapplication_defraimp_inspectionoutcome.AwaitingResultofChecks;
            var finalExpectedApplicationOutcome = defraimp_importapplication_defraimp_inspectionoutcome.Satisfactory;

            var samplePostImportCheckOne = new BasicPostImportCheck(Guid.NewGuid(), importApplicationReference).PostImportCheck;
            var samplePostImportCheckTwo = new BasicPostImportCheck(Guid.NewGuid(), importApplicationReference).PostImportCheck;
            var samplePostImportCheckThree = new BasicPostImportCheck(Guid.NewGuid(), importApplicationReference).PostImportCheck;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImportApplication(context, sampleImportApplication))
                .Delay(5000)
                .ExecuteAction(new SetApplicationInspectionOutcomeValue(context, defraimp_importapplication_defraimp_inspectionoutcome.AwaitingResultofChecks))
                .CreateRecord(new CreatePostImportCheck(context, samplePostImportCheckOne))
                .CreateRecord(new CreatePostImportCheck(context, samplePostImportCheckTwo))
                .CreateRecord(new CreatePostImportCheck(context, samplePostImportCheckThree))
                .Delay(20000)
                .ExecuteAction(new SetPostImportCheckOutcome(context, samplePostImportCheckOne, defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(context, initialExpectedApplicationOutcome))
                .ExecuteAction(new SetPostImportCheckOutcome(context, samplePostImportCheckTwo, defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(context, initialExpectedApplicationOutcome))
                .ExecuteAction(new SetPostImportCheckOutcome(context, samplePostImportCheckThree, defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(context, finalExpectedApplicationOutcome));
        }

        [TestMethod]
        public void Post_Import_Check_Should_Set_Import_Application_Unsatisfactory_If_Any_Check_Is_Marked_Unsatisfactory()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplication = new BasicImportApplication(recordService.AggregateId).ImportApplication;
            var importApplicationReference = new EntityReference(sampleImportApplication.LogicalName, sampleImportApplication.Id);

            var initialExpectedApplicationOutcome = defraimp_importapplication_defraimp_inspectionoutcome.AwaitingResultofChecks;
            var finalExpectedApplicationOutcome = defraimp_importapplication_defraimp_inspectionoutcome.Unsatisfactory;

            var samplePostImportCheckOne = new BasicPostImportCheck(Guid.NewGuid(), importApplicationReference).PostImportCheck;
            var samplePostImportCheckTwo = new BasicPostImportCheck(Guid.NewGuid(), importApplicationReference).PostImportCheck;
            var samplePostImportCheckThree = new BasicPostImportCheck(Guid.NewGuid(), importApplicationReference).PostImportCheck;
            var context = this.GetAppUserContext();

            recordService
                .CreateRecord(new CreateImportApplication(context, sampleImportApplication))
                .Delay(5000)
                .ExecuteAction(new SetApplicationInspectionOutcomeValue(context, defraimp_importapplication_defraimp_inspectionoutcome.AwaitingResultofChecks))
                .CreateRecord(new CreatePostImportCheck(context, samplePostImportCheckOne))
                .CreateRecord(new CreatePostImportCheck(context, samplePostImportCheckTwo))
                .CreateRecord(new CreatePostImportCheck(context, samplePostImportCheckThree))
                .Delay(2000)
                .ExecuteAction(new SetPostImportCheckOutcome(context, samplePostImportCheckOne, defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(context, initialExpectedApplicationOutcome))
                .ExecuteAction(new SetPostImportCheckOutcome(context, samplePostImportCheckTwo, defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(context, initialExpectedApplicationOutcome))
                .ExecuteAction(new SetPostImportCheckOutcome(context, samplePostImportCheckThree, defraimp_importinspection_defraimp_inspectionoutcome.Unsatisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(context, finalExpectedApplicationOutcome));
        }
    }
}
