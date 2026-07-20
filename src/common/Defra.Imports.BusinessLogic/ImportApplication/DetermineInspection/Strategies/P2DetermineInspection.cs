namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies
{
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
    using Defra.Imports.Model;

    public class P2DetermineInspection : AbstractDetermineInspection
    {
        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            this.importApplication = determineInspectionContext.ImportApplication;
            this.importApplicationRepo = determineInspectionContext.ImportApplicationRepo;
            this.autoNumberRepo = determineInspectionContext.AutoNumberRepo;
            this.coverageRulesRepo = determineInspectionContext.CoverageRulesRepo;
            this.inspectionRequirement = new InspectionRequirement(importApplication, importApplicationRepo);
            this.riskLevelCounterManager = determineInspectionContext.RiskLevelCounterManager;
            this.configurationParameterRepo = determineInspectionContext.ConfigurationParameterRepo;

            //Does the import application have a primary ITAHC?
            if (ValidImportApplicationTypeForInspection(importApplication))
            {
                // Has the record not been counted yet?
                if (importApplication.defraimp_ImportRecordCounted != true)
                {
                    int quotaCount = autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME);

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
            else
            {
                MissingCertificateError();
            }
        }

        private void DealWithP2QuotaInspection()
        {
            // Increment the global counter first
            riskLevelCounterManager.IncrementGlobalCounter(importApplication);

            // Decrement the quota counter list
            riskLevelCounterManager.DecrementQuota(ref importApplication, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection);

            // Flag the application for inspection
            inspectionRequirement.P2Inspection();
        }

        private void DealWithNormalP2Inspection()
        {
            // Get the threashold
            defraimp_inspectioncoveragerule coverageRule = GetCoverageRule(coverageRulesRepo, ImportApplicationConstants.P2_COVERAGE_RULE_KEY);

            // Increment the counter and get the value
            riskLevelCounterManager.IncrementNumber(ref importApplication, defraimp_counterhistory_defraimp_reason.ValidP2);

            int currentCount = autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P2_COUNTER_NAME);
            if (currentCount >= coverageRule.defraimp_NumberOfRecordsUntilInspection)
            {
                // Reset the counter
                riskLevelCounterManager.SetNumberValue(ref importApplication, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection, 0);

                inspectionRequirement.P2Inspection();
            }
            else
            {
                inspectionRequirement.NoInspectionRequired();
            }
        }
    }
}
