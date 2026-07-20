namespace Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Assertions
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class ImportApplicationRequiresInspection : ISpecificationValidator<defraimp_importapplication>
    {
        defraimp_importapplication_defraimp_inspectionrequired inspectionRequiredValue;

        public ImportApplicationRequiresInspection(defraimp_importapplication_defraimp_inspectionrequired inspectionRequiredValue)
        {
            this.inspectionRequiredValue = inspectionRequiredValue;
        }

        public void Validate(defraimp_importapplication item)
        {
            item.defraimp_InspectionRequired.Should().Be(this.inspectionRequiredValue);
        }
    }
}
