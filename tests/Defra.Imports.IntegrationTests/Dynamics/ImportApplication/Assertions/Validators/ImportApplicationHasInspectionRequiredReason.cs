namespace Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class ImportApplicationHasPostInspectionOutcome : ISpecificationValidator<defraimp_importapplication>
    { 
        defraimp_importapplication_defraimp_inspectionoutcome inspectionOutcomeValue;

        public ImportApplicationHasPostInspectionOutcome(defraimp_importapplication_defraimp_inspectionoutcome inspectionOutcomeValue)
        {
            this.inspectionOutcomeValue = inspectionOutcomeValue;
        }

        public void Validate(defraimp_importapplication item)
        {
            item.defraimp_InspectionOutcome.Should().Be(this.inspectionOutcomeValue);
        }
    }
}
