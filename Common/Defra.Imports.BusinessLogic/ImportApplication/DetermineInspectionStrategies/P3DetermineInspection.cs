using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;

namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class P3DetermineInspection : AbstractDetermineInspection
    {
        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            var importApplication = determineInspectionContext.ImportApplication;
            var importApplicationRepo = determineInspectionContext.ImportApplicationRepo;
            var autoNumberRepo = determineInspectionContext.AutoNumberRepo;
            var coverageRulesRepo = determineInspectionContext.CoverageRulesRepo;
            var inspectionRequirement = new InspectionRequirement(importApplication, importApplicationRepo);

            // Counter is incremented for all so has already been increased and doesn't need to be incremented

            // Retrieve the count of the current value
            int currentCount = autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME);

            // Retrieve the threshold value
            defraimp_inspectioncoveragerule coverageRule = coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_RiskLevelId.Id.Equals(importApplication.defraimp_importrisklevelid.Id),
                e => new defraimp_inspectioncoveragerule()
                {
                    defraimp_name = e.defraimp_name,
                    defraimp_inspectioncoverageruleId = e.defraimp_inspectioncoverageruleId,
                    defraimp_NumberOfRecordsUntilInspection = e.defraimp_NumberOfRecordsUntilInspection
                }
            ).FirstOrDefault();

            // Check whether the counter has reached the threshold
            if (currentCount >= coverageRule.defraimp_NumberOfRecordsUntilInspection)
            {
                // Reset the counter
                autoNumberRepo.SetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME, 0);

                inspectionRequirement.P3Inspection();
                PerformInspectionRequiredUpdate(inspectionRequirement);
            }
            else
            {
                inspectionRequirement.NoInspectionRequired();
                PerformInspectionRequiredUpdate(inspectionRequirement);
            }
        }
    }
}
