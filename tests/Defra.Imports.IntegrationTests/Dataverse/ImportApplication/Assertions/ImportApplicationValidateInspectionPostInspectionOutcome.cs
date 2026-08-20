namespace Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.IntegrationTests.Dataverse.ImportApplication.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class ImportApplicationValidateInspectionPostInspectionOutcome : BaseValidator<Guid, defraimp_importapplication>
    {
        private readonly ImportsContext context;
        private readonly defraimp_importapplication_defraimp_inspectionoutcome expectedOutcome;

        public ImportApplicationValidateInspectionPostInspectionOutcome(ImportsContext context, defraimp_importapplication_defraimp_inspectionoutcome expectedOutcome)
        {
            this.context = context;
            this.expectedOutcome = expectedOutcome;
        }

        /// <inheritdoc/>
        public override defraimp_importapplication GetRecord(Guid id)
        {
            return this.context.defraimp_importapplicationSet.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        /// <inheritdoc/>
        public override List<ISpecificationValidator<defraimp_importapplication>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_importapplication>>
            {
               new ImportApplicationHasInspectionRequiredReason(this.expectedOutcome),
            };
        }
    }
}
