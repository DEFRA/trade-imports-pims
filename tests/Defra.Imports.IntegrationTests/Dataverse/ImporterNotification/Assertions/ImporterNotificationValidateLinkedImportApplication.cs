namespace Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.IntegrationTests.Dataverse.ImporterNotification.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class ImporterNotificationValidateLinkedImportApplication : BaseValidator<Guid, defraimp_ImporterNotification>
    {
        private ImportsContext context;

        public ImporterNotificationValidateLinkedImportApplication(ImportsContext context)
        {
            this.context = context;
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
                new LinkedImportApplicationValidator(this.context),
            };
        }
    }
}
