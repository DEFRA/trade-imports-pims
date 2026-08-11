namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.Xrm.Sdk;

    public class DevolvedOfficeIsValue : ISpecificationValidator<defraimp_ImporterNotification>
    {
        private EntityReference expectedOffice;

        public DevolvedOfficeIsValue(EntityReference expectedOffice)
        {
            this.expectedOffice = expectedOffice;
        }

        /// <inheritdoc/>
        public void Validate(defraimp_ImporterNotification item)
        {
            item.defraimp_DevolvedOffice.Id.Should().Be(this.expectedOffice.Id);
        }
    }
}
