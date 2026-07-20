using Defra.Imports.Model;
using FluentAssertions;
using MarkTek.Fluent.Testing.RecordGeneration;
using System.Linq;

namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions.Validators
{
    public class LinkedImportApplicationValidator : ISpecificationValidator<defraimp_ImporterNotification>
    {
        private ImportsContext context;

        public LinkedImportApplicationValidator(ImportsContext context)
        {
            this.context = context;
        }

        public void Validate(defraimp_ImporterNotification item)
        {
            defraimp_importapplication linkedImportApplication = this.context.defraimp_importapplicationSet.FirstOrDefault(x => x.defraimp_PrimaryImporterNotificationId.Id == item.Id);
            linkedImportApplication.Should().NotBeNull();
            linkedImportApplication.defraimp_DateIV66Received.Should().NotBeNull();
        }
    }
}
