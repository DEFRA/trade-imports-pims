using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public abstract class AbstractDetermineInspection
    {
        public abstract void ExecuteInspection(DetermineInspectionContext determineInspectionContext);

        protected void PerformInspectionRequiredUpdate(InspectionRequirement inspectionRequirement)
        {
            defraimp_importapplication importApplicationUpdate = new defraimp_importapplication()
            {
                Id = inspectionRequirement.ImportApplication.Id,
                defraimp_InspectionRequired = inspectionRequirement.InspectionRequired,
                defraimp_InspectionRequiredReason = inspectionRequirement.InspectionRequiredReason,
            };

            if (inspectionRequirement.InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.No)
            {
                importApplicationUpdate.defraimp_InspectionDeclinedReason = "The system has determined that an inspection is not required";
            }

            inspectionRequirement.ImportApplicationRepo.Update(importApplicationUpdate);
        }
    }
}
