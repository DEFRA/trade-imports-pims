using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;

namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class P3DetermineInspection : IDetermineInspection
    {
        public void ExecuteInspection(defraimp_importapplication importApplication, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, IAutonumberRepository autoNumberRepo)
        {
            // Counter is incremented for all so has already been increased

            // Retrieve the count of the current value
            int currentCount = autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME);

            // Retrieve the threshold value
            defraimp_inspectioncoveragerule coverageRule = coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_RiskLevelId.Id.Equals(importApplication.defraimp_importrisklevelid.Id),
                e => new defraimp_inspectioncoveragerule()
                {
                    Id = e.Id,
                    defraimp_name = e.defraimp_name,
                    defraimp_inspectioncoverageruleId = e.defraimp_inspectioncoverageruleId,
                    defraimp_NumberOfRecordsUntilInspection = e.defraimp_NumberOfRecordsUntilInspection
                }
            ).ToList().First();

            // Check whether the counter has reached the threshold

        }
    }
}
