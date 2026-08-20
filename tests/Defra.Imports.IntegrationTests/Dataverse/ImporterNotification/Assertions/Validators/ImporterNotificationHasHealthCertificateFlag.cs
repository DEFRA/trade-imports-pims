namespace Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class ImportNotificationHasHealthCertificateFlag : ISpecificationValidator<defraimp_ImporterNotification>
    {
        bool flag;

        public ImportNotificationHasHealthCertificateFlag(bool expectedFlag)
        {
            this.flag = expectedFlag;
        }

        /// <inheritdoc/>
        public void Validate(defraimp_ImporterNotification item)
        {
            item.defraimp_HealthCertificateAttached.Should().Be(this.flag);
        }
    }
}
