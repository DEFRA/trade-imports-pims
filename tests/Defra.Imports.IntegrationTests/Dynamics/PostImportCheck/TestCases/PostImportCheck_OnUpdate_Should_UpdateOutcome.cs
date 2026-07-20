namespace Defra.Imports.IntegrationTests.Dynamics.PostImportCheck.TestCases
{
    using Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Assertions;
    using Defra.Imports.IntegrationTests.Dynamics.ImportApplication.SampleRecords;
    using Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Scenarios;
    using Defra.Imports.IntegrationTests.Dynamics.PostImportCheck.Assertions;
    using Defra.Imports.IntegrationTests.Dynamics.PostImportCheck.SampleRecords;
    using Defra.Imports.IntegrationTests.Dynamics.PostImportCheck.Scenarios;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.Xrm.Sdk;
    using System;
    using Xunit;

    public class PostImportCheck_OnUpdate_Should_UpdateOutcome : TestCasesBase
    {
        [Fact]
        public void Post_Import_Check_Should_Update_Related_Import_Application_Inspection_Outcome_When_Outcome_Is_Set()
        {
            var recordService = new RecordService<Guid>(Guid.NewGuid());
            var sampleImportApplication = new BasicImportApplication(recordService.AggregateId).ImportApplication;
            var importApplicationReference = new EntityReference(sampleImportApplication.LogicalName, sampleImportApplication.Id);
            var samplePostImportCheck= new BasicPostImportCheck(Guid.NewGuid(), importApplicationReference).PostImportCheck;
            var expectedPostImportCheckOutcome = defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory;
            var expectedApplicationOutcome = defraimp_importapplication_defraimp_inspectionoutcome.Satisfactory;

            recordService
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplication))
                .Delay(5000)
                .ExecuteAction(new SetApplicationInspectionOutcomeValue(this.context, defraimp_importapplication_defraimp_inspectionoutcome.AwaitingResultofChecks))
                .CreateRecord(new CreatePostImportCheck(this.context, samplePostImportCheck))
                .Delay(2000)
                .ExecuteAction(new SetPostImportCheckOutcome(this.context, samplePostImportCheck, expectedPostImportCheckOutcome))
                .Delay(2000)
                .AssertAgainst(new PostImportCheckValidateOutcome(this.context, samplePostImportCheck.Id, expectedPostImportCheckOutcome))
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(this.context, expectedApplicationOutcome));
        }

        [Fact]
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

            recordService
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplication))
                .Delay(5000)
                .ExecuteAction(new SetApplicationInspectionOutcomeValue(this.context, defraimp_importapplication_defraimp_inspectionoutcome.AwaitingResultofChecks))
                .CreateRecord(new CreatePostImportCheck(this.context, samplePostImportCheckOne))
                .CreateRecord(new CreatePostImportCheck(this.context, samplePostImportCheckTwo))
                .CreateRecord(new CreatePostImportCheck(this.context, samplePostImportCheckThree))
                .Delay(20000)
                .ExecuteAction(new SetPostImportCheckOutcome(this.context, samplePostImportCheckOne, defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(this.context, initialExpectedApplicationOutcome))
                .ExecuteAction(new SetPostImportCheckOutcome(this.context, samplePostImportCheckTwo, defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(this.context, initialExpectedApplicationOutcome))
                .ExecuteAction(new SetPostImportCheckOutcome(this.context, samplePostImportCheckThree, defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(this.context, finalExpectedApplicationOutcome));
        }

        [Fact]
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

            recordService
                .CreateRecord(new CreateImportApplication(this.context, sampleImportApplication))
                .Delay(5000)
                .ExecuteAction(new SetApplicationInspectionOutcomeValue(this.context, defraimp_importapplication_defraimp_inspectionoutcome.AwaitingResultofChecks))
                .CreateRecord(new CreatePostImportCheck(this.context, samplePostImportCheckOne))
                .CreateRecord(new CreatePostImportCheck(this.context, samplePostImportCheckTwo))
                .CreateRecord(new CreatePostImportCheck(this.context, samplePostImportCheckThree))
                .Delay(2000)
                .ExecuteAction(new SetPostImportCheckOutcome(this.context, samplePostImportCheckOne, defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(this.context, initialExpectedApplicationOutcome))
                .ExecuteAction(new SetPostImportCheckOutcome(this.context, samplePostImportCheckTwo, defraimp_importinspection_defraimp_inspectionoutcome.Satisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(this.context, initialExpectedApplicationOutcome))
                .ExecuteAction(new SetPostImportCheckOutcome(this.context, samplePostImportCheckThree, defraimp_importinspection_defraimp_inspectionoutcome.Unsatisfactory))
                .Delay(2000)
                .AssertAgainst(new ImportApplicationValidateInspectionPostInspectionOutcome(this.context, finalExpectedApplicationOutcome));
        }
    }
}
