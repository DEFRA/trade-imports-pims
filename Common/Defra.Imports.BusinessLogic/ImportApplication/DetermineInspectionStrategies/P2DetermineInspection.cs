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
        private defraimp_importapplication _importApplication;
        private IAutonumberRepository _autoNumberRepo;
        private ICrmRepository<defraimp_inspectioncoveragerule> _coverageRulesRepo;
        private ICrmRepository<defraimp_importapplication> _importApplicationRepo;
        private InspectionRequirement _inspectionRequirement;

        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            _importApplication = determineInspectionContext.ImportApplication;
            _importApplicationRepo = determineInspectionContext.ImportApplicationRepo;
            _autoNumberRepo = determineInspectionContext.AutoNumberRepo;
            _coverageRulesRepo = determineInspectionContext.CoverageRulesRepo;
            _inspectionRequirement = new InspectionRequirement(_importApplication, _importApplicationRepo);

            int quotaCount = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME);

            if(quotaCount > 0)
            {
                DealWithP2QuotaInspection();
            }
            else
            {
                DealWithNormalP2Inspection();
            }
        }

        private void DealWithP2QuotaInspection()
        {
            // Decrement the quota counter list
            _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME);

            // Flag the application for inspection
            _inspectionRequirement.P2Inspection();
            PerformInspectionRequiredUpdate(_inspectionRequirement);
        }

        private void DealWithNormalP2Inspection()
        {
            // Get the threashold
            defraimp_inspectioncoveragerule coverageRule = GetP2CoverageRule();

            // Increment the counter and get the value
            _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.P2_COUNTER_NAME);

            int currentCount = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P2_COUNTER_NAME);
            if (currentCount >= coverageRule.defraimp_NumberOfRecordsUntilInspection)
            {
                // Reset the counter
                _autoNumberRepo.SetAutonumberValue(ImportApplicationConstants.P2_COUNTER_NAME, 0);

                _inspectionRequirement.P2Inspection();
                PerformInspectionRequiredUpdate(_inspectionRequirement);
            }
            else
            {
                _inspectionRequirement.NoInspectionRequired();
                PerformInspectionRequiredUpdate(_inspectionRequirement);
            }
        }

        private defraimp_inspectioncoveragerule GetP2CoverageRule()
        {
            // Get the threashold
            defraimp_inspectioncoveragerule coverageRule = _coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_RiskLevelId.Id.Equals(_importApplication.defraimp_importrisklevelid.Id),
                e => new defraimp_inspectioncoveragerule()
                {
                    defraimp_name = e.defraimp_name,
                    defraimp_inspectioncoverageruleId = e.defraimp_inspectioncoverageruleId,
                    defraimp_NumberOfRecordsUntilInspection = e.defraimp_NumberOfRecordsUntilInspection
                }
            ).FirstOrDefault();

            return coverageRule;
        }
    }
}
