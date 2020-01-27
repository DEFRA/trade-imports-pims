using System;
using System.Collections.Generic;
using System.Text;
using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;

namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class UnknownDetermineInspection : AbstractDetermineInspection
    {
        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            var importApplication = determineInspectionContext.ImportApplication;
            var importApplicationRepo = determineInspectionContext.ImportApplicationRepo;
            var inspectionRequirement = new InspectionRequirement(importApplication, importApplicationRepo);

            inspectionRequirement.RiskLevelUnknown();
            PerformInspectionRequiredUpdate(inspectionRequirement);
        }
    }
}
