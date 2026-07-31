namespace Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class ImportApplicationHasInspectionRequiredReason : ISpecificationValidator<defraimp_importapplication>
    {
        defraimp_importapplication_defraimp_inspectionrequiredreason inspectionRequiredReasonValue;

        public ImportApplicationHasInspectionRequiredReason(defraimp_importapplication_defraimp_inspectionrequiredreason inspectionRequiredReasonValue)
        {
            this.inspectionRequiredReasonValue = inspectionRequiredReasonValue;
        }

        /// <inheritdoc/>
        public void Validate(defraimp_importapplication item)
        {
            item.defraimp_InspectionRequiredReason.Should().Be(this.inspectionRequiredReasonValue);
        }
    }
}
