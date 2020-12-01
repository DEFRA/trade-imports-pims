using Defra.Imports.Model;
using Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Assertions.Validators;
using Marktek.Fluent.Testing.Engine;
using MarkTek.Fluent.Testing.RecordGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Tests.Integration.Dynamics.ImporterNotification.Assertions
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
