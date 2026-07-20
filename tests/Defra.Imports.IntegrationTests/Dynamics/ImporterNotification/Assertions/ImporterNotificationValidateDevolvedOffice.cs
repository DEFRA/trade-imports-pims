namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions
{
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ImporterNotificationValidateDevolvedOffice : BaseValidator<Guid, defraimp_ImporterNotification>
    {
        private readonly ImportsContext context;
        private readonly EntityReference devolvedOffice;

        public ImporterNotificationValidateDevolvedOffice(ImportsContext context, EntityReference devolvedOffice)
        {
            this.context = context;
            this.devolvedOffice = devolvedOffice;
        }

        public override defraimp_ImporterNotification GetRecord(Guid id)
        {
            return context.defraimp_ImporterNotificationSet.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        public override List<ISpecificationValidator<defraimp_ImporterNotification>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_ImporterNotification>>
            {
                new DevolvedOfficeIsValue(this.devolvedOffice),
            };
        }
    }
}
