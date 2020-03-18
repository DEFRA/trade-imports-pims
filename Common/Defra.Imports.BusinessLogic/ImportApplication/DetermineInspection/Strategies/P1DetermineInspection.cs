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

    public class P1DetermineInspection : AbstractDetermineInspection
    {
        private IRepositoryFactory _repositoryFactory;
        private defraimp_importapplication _importApplication;
        private ICrmRepository<defraimp_importapplication> _importApplicationRepo;
        private IAutonumberRepository _autoNumberRepo;
        private IPlaceOfOriginRepository _placeOfOriginRepo;
        private ICrmRepository <defraimp_goldbronzecommodity> _goldBronzeCommodityRepo;
        private ICrmRepository<defraimp_inspectioncoveragerule> _coverageRulesRepo;
        private InspectionRequirement _inspectionRequirement;

        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            _importApplication = determineInspectionContext.ImportApplication;
            _repositoryFactory = determineInspectionContext.RepositoryFactory;
            _importApplicationRepo = _repositoryFactory.GetRepository<ImportsContext, defraimp_importapplication>();
            _placeOfOriginRepo = determineInspectionContext.PlaceOfOriginRepo;
            _coverageRulesRepo = _repositoryFactory.GetRepository<ImportsContext, defraimp_inspectioncoveragerule>();
            _autoNumberRepo = determineInspectionContext.AutoNumberRepo;
            _inspectionRequirement = new InspectionRequirement(_importApplication, _importApplicationRepo);

            //Does the import application have a primary ITAHC?
            if (_importApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC && _importApplication.defraimp_PrimaryITAHCId != null)
            {
                // Has the record not been counted yet?
                if (_importApplication.defraimp_ImportRecordCounted != true)
                {
                    // Does the import application have a commodity? If not, inspection can't be determined
                    if (_importApplication.defraimp_CommodityTypeId != null)
                    {
                        CommodityHelper goldBronzeCommodity = new CommodityHelper(_importApplication.defraimp_CommodityTypeId, _repositoryFactory);

                        // Is the commodity gold/bronze? 
                        if (goldBronzeCommodity.IsCommodityCoveredByGoldBronze(_importApplication.defraimp_CountryofOriginId))
                        {
                            // Try to get a place of origin
                            defraimp_placeoforigin placeOfOrigin = GetPlaceOfOrigin();

                            // Do we have a place of origin?
                            if (placeOfOrigin != null)
                            {
                                // Get the gold/bronze quota rule
                                defraimp_inspectioncoveragerule gbCoverageRule = GetCoverageRule(_coverageRulesRepo, ImportApplicationConstants.GB_COVERAGE_RULE_KEY);

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
                                _inspectionRequirement.PlaceOfOriginMissing();
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
                        _inspectionRequirement.RiskLevelUnknown();
                    }
                }
            }
            else
            {
                // No Primary ITAHC
                _inspectionRequirement.PrimaryITAHCMissing();
            }
        }

        private defraimp_placeoforigin GetPlaceOfOrigin()
        {
            if (_importApplication.defraimp_PlaceofOriginid != null)
            {
                defraimp_placeoforigin placeOfOrigin = _placeOfOriginRepo.Find(_importApplication.defraimp_PlaceofOriginid.Id);

                return placeOfOrigin;
            }
            else
            {
                return null;
            }
        }

        private void P1Inspection()
        {
            int quotaCount = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P1_QUOTA_COUNTER_NAME);

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
            // Decrement the quota counter list
            _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.P1_QUOTA_COUNTER_NAME);

            // Flag the application for inspection
            _inspectionRequirement.P1Inspection();

            SetRecordCounted(_importApplicationRepo, _importApplication, true);
        }

        private void DealWithNormalP1Inspection()
        {
            // Get the threashold
            defraimp_inspectioncoveragerule p1coverageRule = GetCoverageRule(_coverageRulesRepo, ImportApplicationConstants.P1_COVERAGE_RULE_KEY);

            // Increment the counter and get the value
            _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.P1_COUNTER_NAME);

            int currentCount = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P1_COUNTER_NAME);
            if (currentCount >= p1coverageRule.defraimp_NumberOfRecordsUntilInspection)
            {
                // Reset the counter
                _autoNumberRepo.SetAutonumberValue(ImportApplicationConstants.P1_COUNTER_NAME, 0);

                _inspectionRequirement.P1Inspection();
            }
            else
            {
                _inspectionRequirement.NoInspectionRequired();
            }

            SetRecordCounted(_importApplicationRepo, _importApplication, true);
        }

        private void GoldInspection(defraimp_placeoforigin placeOfOrigin, int coverageRuleValue)
        {
            // Increment the gold/bronze application counter
            _placeOfOriginRepo.IncrementApplicationCounter(placeOfOrigin.Id);

            // Ensure our local copy also increments the counter
            placeOfOrigin.defraimp_ApplicationCounter += 1;

            SetRecordCounted(_importApplicationRepo, _importApplication, true);

            // We want a local copy of the quota so we can adjust it and not have to keep retrieving the record.
            int quotaCount = _placeOfOriginRepo.GetQuotaCounterValue(placeOfOrigin.Id);
            // Check if the number of applications counter is over the coverage rule value
            if (placeOfOrigin.defraimp_ApplicationCounter >= coverageRuleValue)
            {
                // +1 to quota
                _placeOfOriginRepo.IncrementQuotaCounter(placeOfOrigin.Id);
                quotaCount += 1;

                // Reset the application counter
                _placeOfOriginRepo.SetApplicationCounter(placeOfOrigin.Id, 0);
            }

            // Check the outstanding inspection counter, see if we need to apply an inspection
            if (quotaCount > 0)
            {
                // -1 from quota
                _placeOfOriginRepo.DecrementQuotaCounter(placeOfOrigin.Id);

                // Set to inspect
                _inspectionRequirement.GoldCoverageInspection();
            }
            else
            {
                // Don't inspect
                _inspectionRequirement.NoInspectionGold();
            }
        }

        void BronzeInspection(defraimp_placeoforigin placeOfOrigin)
        {
            // Increment the gold/bronze application counter
            _placeOfOriginRepo.IncrementApplicationCounter(placeOfOrigin.Id);

            // Ensure our local copy also increments the counter
            placeOfOrigin.defraimp_ApplicationCounter += 1;

            SetRecordCounted(_importApplicationRepo, _importApplication, true);

            // Check if the commodity has been locked to bronze. If so, set that as the inspection reason.
            if (placeOfOrigin.defraimp_LocktoBronze == true)
            {
                // Set to inspect
                _inspectionRequirement.LockedToBronzeInspection();
            }
            else
            {
                // Set to inspect
                _inspectionRequirement.BronzeInspection();
            }
        }
    }
}
