namespace Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.Xrm.Sdk;

    class ImportApplicationHasRiskLevel : ISpecificationValidator<defraimp_importapplication>
    {
        EntityReference riskLevel;

        public ImportApplicationHasRiskLevel(EntityReference riskLevel)
        {
            this.riskLevel = riskLevel;
        }

        public void Validate(defraimp_importapplication item)
        {
            item.defraimp_importrisklevelid.Id.Should().Be(this.riskLevel.Id);
        }

    }
}
