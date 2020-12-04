namespace Defra.Imports.Tests.Integration.Dynamics.ImportApplication.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Model;
    using Defra.Imports.Tests.Integration.Dynamics.ImportApplication.Assertions.Validators;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using Microsoft.Xrm.Sdk;

    class ImportApplicationValidateInspectionRequired : BaseValidator<Guid, defraimp_importapplication>
    {
        private readonly ImportsContext context;
        private readonly EntityReference riskLevel;
        private readonly defraimp_importapplication_defraimp_inspectionrequired inspectionRequired;
        private readonly defraimp_importapplication_defraimp_inspectionrequiredreason inspectionRequiredReason;

        public ImportApplicationValidateInspectionRequired(ImportsContext context, EntityReference riskLevel, defraimp_importapplication_defraimp_inspectionrequired inspectionRequired, defraimp_importapplication_defraimp_inspectionrequiredreason inspectionRequiredReason)
        {
            this.context = context;
            this.riskLevel = riskLevel;
            this.inspectionRequired = inspectionRequired;
            this.inspectionRequiredReason = inspectionRequiredReason;
        }

        public override defraimp_importapplication GetRecord(Guid id)
        {
            return this.context.defraimp_importapplicationSet.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        public override List<ISpecificationValidator<defraimp_importapplication>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_importapplication>>
            {
               new ImportApplicationHasRiskLevel(this.riskLevel),
               new ImportApplicationRequiresInspection(this.inspectionRequired),
               new ImportApplicationHasInspectionRequiredReason(this.inspectionRequiredReason),
            };
        }
    }
}
