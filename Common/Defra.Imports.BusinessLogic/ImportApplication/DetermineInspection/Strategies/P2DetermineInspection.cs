namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;

    public class P2DetermineInspection : AbstractDetermineInspection
    {
        private defraimp_importapplication _importApplication;
        private IAutonumberRepository _autoNumberRepo;
        private ICrmRepository<defraimp_inspectioncoveragerule> _coverageRulesRepo;
        private ICrmRepository<defraimp_importapplication> _importApplicationRepo;
        private InspectionRequirement _inspectionRequirement;
        private AbstractRiskCounterManager _riskLevelCounterManager;

        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            _importApplication = determineInspectionContext.ImportApplication;
            _importApplicationRepo = determineInspectionContext.ImportApplicationRepo;
            _autoNumberRepo = determineInspectionContext.AutoNumberRepo;
            _coverageRulesRepo = determineInspectionContext.CoverageRulesRepo;
            _inspectionRequirement = new InspectionRequirement(_importApplication, _importApplicationRepo);
            _riskLevelCounterManager = determineInspectionContext.RiskLevelCounterManager;

            //Does the import application have a primary ITAHC?
            if (_importApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC && _importApplication.defraimp_PrimaryITAHCId != null)
            {
                // Has the record not been counted yet?
                if (_importApplication.defraimp_ImportRecordCounted != true)
                {
                    int quotaCount = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME);

                    if (quotaCount > 0)
                    {
                        DealWithP2QuotaInspection();
                    }
                    else
                    {
                        DealWithNormalP2Inspection();
                    }
                }
            }
            else if (_importApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
            {
                // Flag the application as missing an ITAHC
                _inspectionRequirement.PrimaryITAHCMissing();
            }
        }

        private void DealWithP2QuotaInspection()
        {
            // Decrement the quota counter list
            _riskLevelCounterManager.DecrementQuota(ref _importApplication, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection);
            _riskLevelCounterManager.IncrementGlobalCounter(_importApplication);

            // Flag the application for inspection
            _inspectionRequirement.P2Inspection();
        }

        private void DealWithNormalP2Inspection()
        {
            // Get the threashold
            defraimp_inspectioncoveragerule coverageRule = GetCoverageRule(_coverageRulesRepo, ImportApplicationConstants.P2_COVERAGE_RULE_KEY);

            // Increment the counter and get the value
            _riskLevelCounterManager.IncrementNumber(ref _importApplication, defraimp_counterhistory_defraimp_reason.ValidP2);

            int currentCount = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P2_COUNTER_NAME);
            if (currentCount >= coverageRule.defraimp_NumberOfRecordsUntilInspection)
            {
                // Reset the counter
                _riskLevelCounterManager.SetNumberValue(ref _importApplication, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection, 0);

                _inspectionRequirement.P2Inspection();
            }
            else
            {
                _inspectionRequirement.NoInspectionRequired();
            }
        }
    }
}
