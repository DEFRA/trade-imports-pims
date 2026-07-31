namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class ImporterNotificationValidateLinkedWatchFlagRecord : BaseValidator<Guid, defraimp_ImporterNotification>
    {
        private ImportsContext context;
        private Guid watchListId;

        public ImporterNotificationValidateLinkedWatchFlagRecord(ImportsContext context, Guid watchListId)
        {
            this.context = context;
            this.watchListId = watchListId;
        }

        /// <inheritdoc/>
        public override defraimp_ImporterNotification GetRecord(Guid id)
        {
            return this.context.defraimp_ImporterNotificationSet.FirstOrDefault(x => x.Id == id);
        }

        /// <inheritdoc/>
        public override List<ISpecificationValidator<defraimp_ImporterNotification>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_ImporterNotification>>
            {
                new LinkedWatchFlagValidator(this.context, this.watchListId),
            };
        }
    }
}
