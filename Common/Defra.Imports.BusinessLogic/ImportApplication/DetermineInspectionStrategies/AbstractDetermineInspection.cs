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

            importApplicationRepo.Update(importApplicationUpdate);
        }
    }
}
