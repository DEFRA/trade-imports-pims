namespace Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.Xrm.Sdk;

    class ImportNotificationHasHealthCertificateFlag : ISpecificationValidator<defraimp_ImporterNotification>
    {
        bool flag;

        public ImportNotificationHasHealthCertificateFlag(bool expectedFlag)
        {
            this.flag = expectedFlag;
        }

        public void Validate(defraimp_ImporterNotification item)
        {
            item.defraimp_HealthCertificateAttached.Should().Be(this.flag);
        }
    }
}
