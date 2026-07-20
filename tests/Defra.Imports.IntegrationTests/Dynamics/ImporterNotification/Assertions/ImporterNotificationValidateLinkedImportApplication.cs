using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions.Validators;
using Defra.Imports.Model;
using Marktek.Fluent.Testing.Engine;
using MarkTek.Fluent.Testing.RecordGeneration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions
{
    public class ImporterNotificationValidateLinkedImportApplication : BaseValidator<Guid, defraimp_ImporterNotification>
    {
        private ImportsContext context;

        public ImporterNotificationValidateLinkedImportApplication(ImportsContext context)
        {
            this.context = context;
        }

        public override defraimp_ImporterNotification GetRecord(Guid id)
        {
            return this.context.defraimp_ImporterNotificationSet.FirstOrDefault(x => x.Id == id);
        }

        public override List<ISpecificationValidator<defraimp_ImporterNotification>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_ImporterNotification>>
            {
                new LinkedImportApplicationValidator(this.context),
            };
        }
    }
}
