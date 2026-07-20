namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions
{
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ImporterNotificationValidateLinkedWatchFlagRecord : BaseValidator<Guid, defraimp_ImporterNotification>
    {
        private ImportsContext context;
        private Guid watchListId;

        public ImporterNotificationValidateLinkedWatchFlagRecord(ImportsContext context, Guid watchListId)
        {
            this.context = context;
            this.watchListId = watchListId;
        }

        public override defraimp_ImporterNotification GetRecord(Guid id)
        {
            return this.context.defraimp_ImporterNotificationSet.FirstOrDefault(x => x.Id == id);
        }

        public override List<ISpecificationValidator<defraimp_ImporterNotification>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_ImporterNotification>>
            {
                new LinkedWatchFlagValidator(this.context, this.watchListId),
            };
        }
    }
}
