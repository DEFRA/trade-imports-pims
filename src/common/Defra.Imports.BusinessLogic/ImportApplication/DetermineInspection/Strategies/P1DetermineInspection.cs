namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies
{
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;

    public class P1DetermineInspection : AbstractDetermineInspection
    {
        private IPlaceOfOriginRepository placeOfOriginRepo;
        private ICrmRepository <defraimp_goldbronzecommodity> goldBronzeCommodityRepo;

        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            this.importApplication = determineInspectionContext.ImportApplication;
            this.repositoryFactory = determineInspectionContext.RepositoryFactory;
            this.importApplicationRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_importapplication>();
            this.placeOfOriginRepo = determineInspectionContext.PlaceOfOriginRepo;
            this.coverageRulesRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_inspectioncoveragerule>();
            this.autoNumberRepo = determineInspectionContext.AutoNumberRepo;
            this.inspectionRequirement = new InspectionRequirement(importApplication, importApplicationRepo);
            this.riskLevelCounterManager = determineInspectionContext.RiskLevelCounterManager;
            this.configurationParameterRepo = determineInspectionContext.ConfigurationParameterRepo;

            //Does the import application have a primary ITAHC?
            if (ValidImportApplicationTypeForInspection(importApplication))
            {
                // Has the record not been counted yet?
                if (importApplication.defraimp_ImportRecordCounted != true)
                {
                    // Does the import application have a commodity and country? If not, inspection can't be determined
                    if (importApplication.defraimp_CommodityTypeId != null && importApplication.defraimp_CountryofOriginId != null)
                    {
                        CommodityHelper goldBronzeCommodity = new CommodityHelper(importApplication.defraimp_CommodityTypeId, repositoryFactory);

                        // Is the commodity gold/bronze? 
                        if (goldBronzeCommodity.IsCommodityCoveredByGoldBronze(importApplication.defraimp_CountryofOriginId))
                        {
                            // Try to get a place of origin
                            defraimp_placeoforigin placeOfOrigin = GetPlaceOfOrigin();

                            // Do we have a place of origin?
                            if (placeOfOrigin != null)
                            {
                                // Get the gold/bronze quota rule
                                defraimp_inspectioncoveragerule gbCoverageRule = GetCoverageRule(coverageRulesRepo, ImportApplicationConstants.GB_COVERAGE_RULE_KEY);

                                if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                                {
                                    GoldInspection(placeOfOrigin, (int)gbCoverageRule.defraimp_NumberOfRecordsUntilInspection);
                                }
                                else if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Bronze)
                                {
                                    BronzeInspection(placeOfOrigin);
                                }
                            }
                            else
                            {
                                // There is not a valid place of origin, likely because it is missing or there are errors. Set inspection as undetermined.
                                inspectionRequirement.PlaceOfOriginMissing();
                            }
                        }
                        else // If the commodity is not covered by the gold/bronze rule then it falls into the default P1 path
                        {
                            // P1
                            P1Inspection();
                        }
                    }
                    else
                    {
                        // Can't determine risk level
                        inspectionRequirement.RiskLevelUnknown();
                    }
                }
            }
            else
            {
                MissingCertificateError();
            }
        }

        private defraimp_placeoforigin GetPlaceOfOrigin()
        {
            if (importApplication.defraimp_PlaceofOriginid != null)
            {
                defraimp_placeoforigin placeOfOrigin = placeOfOriginRepo.Find(importApplication.defraimp_PlaceofOriginid.Id);

                return placeOfOrigin;
            }
            else
            {
                return null;
            }
        }

        private void P1Inspection()
        {
            int quotaCount = autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P1_QUOTA_COUNTER_NAME);

            if (quotaCount > 0)
            {
                DealWithP1QuotaInspection();
            }
            else
            {
                DealWithNormalP1Inspection();
            }
        }

        private void DealWithP1QuotaInspection()
        {
            // Increment the global counter first
            riskLevelCounterManager.IncrementGlobalCounter(importApplication);

            // Decrement the quota counter list
            riskLevelCounterManager.DecrementQuota(ref importApplication, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection);

            // Flag the application for inspection
            inspectionRequirement.P1Inspection();
        }

        private void DealWithNormalP1Inspection()
        {
            // Get the threashold
            defraimp_inspectioncoveragerule p1coverageRule = GetCoverageRule(coverageRulesRepo, ImportApplicationConstants.P1_COVERAGE_RULE_KEY);

            // Increment the counter and get the value
            riskLevelCounterManager.IncrementNumber(ref importApplication, defraimp_counterhistory_defraimp_reason.ValidP1);

            int currentCount = autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P1_COUNTER_NAME);
            if (currentCount >= p1coverageRule.defraimp_NumberOfRecordsUntilInspection)
            {
                // Reset the counter
                riskLevelCounterManager.SetNumberValue(ref importApplication, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection, 0);

                inspectionRequirement.P1Inspection();
            }
            else
            {
                inspectionRequirement.NoInspectionRequired();
            }
        }

        private void GoldInspection(defraimp_placeoforigin placeOfOrigin, int coverageRuleValue)
        {
            // Increment the gold/bronze application counter
            riskLevelCounterManager.IncrementNumber(ref importApplication, defraimp_counterhistory_defraimp_reason.ValidGB);

            // Ensure our local copy also increments the counter
            placeOfOrigin.defraimp_ApplicationCounter += 1;

            // We want a local copy of the quota so we can adjust it and not have to keep retrieving the record.
            int quotaCount = placeOfOriginRepo.GetQuotaCounterValue(placeOfOrigin.Id);
            // Check if the number of applications counter is over the coverage rule value
            if (placeOfOrigin.defraimp_ApplicationCounter >= coverageRuleValue)
            {
                // +1 to quota
                riskLevelCounterManager.IncrementQuota(ref importApplication, defraimp_counterhistory_defraimp_reason.CounterTargetReachedQuotaIncrement);
                quotaCount += 1;

                // Reset the application counter
                riskLevelCounterManager.SetNumberValue(ref importApplication, defraimp_counterhistory_defraimp_reason.CounterTargetReachedQuotaIncrement, 0);
            }

            // Check the outstanding inspection counter, see if we need to apply an inspection
            if (quotaCount > 0)
            {
                // -1 from quota
                riskLevelCounterManager.DecrementQuota(ref importApplication, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection);

                // Set to inspect
                inspectionRequirement.GoldCoverageInspection();
            }
            else
            {
                // Don't inspect
                inspectionRequirement.NoInspectionGold();
            }
        }

        void BronzeInspection(defraimp_placeoforigin placeOfOrigin)
        {
            // Increment the global counter first
            riskLevelCounterManager.IncrementGlobalCounter(importApplication);

            // Check if the commodity has been locked to bronze. If so, set that as the inspection reason.
            if (placeOfOrigin.defraimp_LocktoBronze == true)
            {
                // Set to inspect
                inspectionRequirement.LockedToBronzeInspection();
            }
            else
            {
                // Set to inspect
                inspectionRequirement.BronzeInspection();
            }
        }
    }
}
