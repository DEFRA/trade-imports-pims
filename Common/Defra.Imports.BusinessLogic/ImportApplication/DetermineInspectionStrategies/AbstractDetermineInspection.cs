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

        protected void PerformInspectionRequiredUpdate(defraimp_importapplication importApplication, ICrmRepository<defraimp_importapplication> importApplicationRepo, defraimp_importapplication_defraimp_inspectionrequired required, defraimp_importapplication_defraimp_inspectionrequiredreason reason)
        {
            defraimp_importapplication importApplicationUpdate = new defraimp_importapplication()
            {
                Id = importApplication.Id,
                defraimp_InspectionRequired = required,
                defraimp_InspectionRequiredReason = reason,
            };

            if (required == defraimp_importapplication_defraimp_inspectionrequired.No)
            {
                importApplicationUpdate.defraimp_InspectionDeclinedReason = "The system has determined that an inspection is not required";
            }

            importApplicationRepo.Update(importApplicationUpdate);
        }

        protected void InspectionPlaceOfOriginMissing(defraimp_importapplication importApplication, ICrmRepository<defraimp_importapplication> importApplicationRepo)
        {
            PerformInspectionRequiredUpdate(
                importApplication,
                importApplicationRepo,
                defraimp_importapplication_defraimp_inspectionrequired.Discretionary,
                defraimp_importapplication_defraimp_inspectionrequiredreason.VerifiedPlaceofOriginMissing
                );
        }

        protected void CantDetermineInspectionNoRiskLevel(defraimp_importapplication importApplication, ICrmRepository<defraimp_importapplication> importApplicationRepo)
        {
            PerformInspectionRequiredUpdate(
                importApplication,
                importApplicationRepo,
                defraimp_importapplication_defraimp_inspectionrequired.Undetermined,
                defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown
                );
        }
    }
}
