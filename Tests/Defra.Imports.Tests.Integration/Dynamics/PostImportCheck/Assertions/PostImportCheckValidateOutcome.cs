namespace Defra.Imports.Tests.Integration.Dynamics.PostImportCheck.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Model;
    using Defra.Imports.Tests.Integration.Dynamics.PostImportCheck.Assertions.Validators;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class PostImportCheckValidateOutcome : BaseValidator<Guid, defraimp_importinspection>
    {
        private readonly ImportsContext context;
        private readonly defraimp_importinspection postImportCheck;
        private readonly defraimp_importinspection_defraimp_inspectionoutcome expectedOutcome;

        public PostImportCheckValidateOutcome(ImportsContext context, Guid postImportCheckId, defraimp_importinspection_defraimp_inspectionoutcome expectedOutcome)
        {
            this.context = context;
            this.postImportCheck = GetRecord(postImportCheckId);
            this.expectedOutcome = expectedOutcome;
        }

        public override defraimp_importinspection GetRecord(Guid id)
        {
            return this.context.defraimp_importinspectionSet.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        public override List<ISpecificationValidator<defraimp_importinspection>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_importinspection>>
            {
               new PostImportCheckOutcomeHasValue(postImportCheck, this.expectedOutcome),
            };
        }
    }
}
