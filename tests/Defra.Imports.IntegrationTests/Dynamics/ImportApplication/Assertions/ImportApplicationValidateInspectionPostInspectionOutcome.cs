namespace Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Assertions
{
    using Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class ImportApplicationValidateInspectionPostInspectionOutcome : BaseValidator<Guid, defraimp_importapplication>
    {
        private readonly ImportsContext context;
        private readonly defraimp_importapplication_defraimp_inspectionoutcome expectedOutcome;

        public ImportApplicationValidateInspectionPostInspectionOutcome(ImportsContext context, defraimp_importapplication_defraimp_inspectionoutcome expectedOutcome)
        {
            this.context = context;
            this.expectedOutcome = expectedOutcome;
        }

        public override defraimp_importapplication GetRecord(Guid id)
        {
            return this.context.defraimp_importapplicationSet.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        public override List<ISpecificationValidator<defraimp_importapplication>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_importapplication>>
            {
               new ImportApplicationHasPostInspectionOutcome(this.expectedOutcome),
            };
        }
    }
}
