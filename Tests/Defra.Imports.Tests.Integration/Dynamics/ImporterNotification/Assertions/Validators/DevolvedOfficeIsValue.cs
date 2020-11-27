namespace Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Assertions.Validators
{
    using System;
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

        public void Validate(defraimp_ImporterNotification item)
        {       
            item.defraimp_DevolvedOffice.Id.Should().Be(this.expectedOffice.Id);
        }
    }
}
