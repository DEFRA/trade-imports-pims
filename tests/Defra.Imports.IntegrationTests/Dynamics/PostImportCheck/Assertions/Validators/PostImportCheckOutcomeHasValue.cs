namespace Defra.Imports.IntegrationTests.Dynamics.PostImportCheck.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class PostImportCheckOutcomeHasValue : ISpecificationValidator<defraimp_importinspection>
    {
        defraimp_importinspection postImportCheck;
        defraimp_importinspection_defraimp_inspectionoutcome expectedOutcome;

        public PostImportCheckOutcomeHasValue(defraimp_importinspection postImportCheck, defraimp_importinspection_defraimp_inspectionoutcome expectedOutcome)
        {
            this.postImportCheck = postImportCheck;
            this.expectedOutcome = expectedOutcome;
        }

        public void Validate(defraimp_importinspection item)
        {
            this.postImportCheck.defraimp_InspectionOutcome.Should().Be(this.expectedOutcome);
        }
    }
}
