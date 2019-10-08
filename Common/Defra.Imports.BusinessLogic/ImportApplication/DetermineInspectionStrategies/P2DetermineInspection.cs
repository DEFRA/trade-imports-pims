using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;

namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class P2DetermineInspection : AbstractDetermineInspection
    {
        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            var importApplication = determineInspectionContext.ImportApplication;
            var importApplicationRepo = determineInspectionContext.ImportApplicationRepo;
            var autoNumberRepo = determineInspectionContext.AutoNumberRepo;
            var coverageRulesRepo = determineInspectionContext.CoverageRulesRepo;

            // Increment the counter and get the value
            autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.P2_COUNTER_NAME);
            int currentCount = autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P2_COUNTER_NAME);
            int quotaCount = autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME);

            // Get the threashold
            defraimp_inspectioncoveragerule coverageRule = coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_RiskLevelId.Id.Equals(importApplication.defraimp_importrisklevelid.Id),
                e => new defraimp_inspectioncoveragerule()
                {
                    defraimp_name = e.defraimp_name,
                    defraimp_inspectioncoverageruleId = e.defraimp_inspectioncoverageruleId,
                    defraimp_NumberOfRecordsUntilInspection = e.defraimp_NumberOfRecordsUntilInspection
                }
            ).ToList().First();

            if(quotaCount > 0)
            {
                // Decrement the quota counter list
                autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME);

                // Flag the application for inspection
                PerformInspectionRequiredUpdate(
                    importApplication,
                    importApplicationRepo,
                    defraimp_importapplication_defraimp_inspectionrequired.Yes,
                    defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP2Inspection
                );

            }
            else
            {
                if (currentCount >= coverageRule.defraimp_NumberOfRecordsUntilInspection)
                {
                    // Reset the counter
                    autoNumberRepo.SetAutonumberValue(ImportApplicationConstants.P2_COUNTER_NAME, 0);

                    PerformInspectionRequiredUpdate(
                        importApplication,
                        importApplicationRepo,
                        defraimp_importapplication_defraimp_inspectionrequired.Yes,
                        defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP2Inspection
                    );
                }
                else
                {
                    PerformInspectionRequiredUpdate(
                        importApplication,
                        importApplicationRepo,
                        defraimp_importapplication_defraimp_inspectionrequired.No,
                        defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired
                    );
                }
            }
        }
    }
}
