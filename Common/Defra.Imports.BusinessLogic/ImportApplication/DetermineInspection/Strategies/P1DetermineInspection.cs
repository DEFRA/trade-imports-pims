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
        private IRepositoryFactory repositoryFactory;
        private defraimp_importapplication importApplication;
        private ICrmRepository<defraimp_importapplication> importApplicationRepo;
        private IAutonumberRepository autoNumberRepo;
        private IPlaceOfOriginRepository placeOfOriginRepo;
        private ICrmRepository <defraimp_goldbronzecommodity>goldBronzeCommodityRepo;
        private ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo;
        private InspectionRequirement inspectionRequirement;

        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            repositoryFactory = determineInspectionContext.RepositoryFactory;
            importApplication = determineInspectionContext.ImportApplication;
            importApplicationRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_importapplication>();
            placeOfOriginRepo = determineInspectionContext.PlaceOfOriginRepo;
            coverageRulesRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_inspectioncoveragerule>();
            autoNumberRepo = determineInspectionContext.AutoNumberRepo;
            inspectionRequirement = new InspectionRequirement(importApplication, importApplicationRepo);

            // Get the gold/bronze quota rule
            defraimp_inspectioncoveragerule coverageRule = coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_RiskLevelId.Id.Equals(importApplication.defraimp_importrisklevelid.Id),
                e => new defraimp_inspectioncoveragerule()
                {
                    defraimp_name = e.defraimp_name,
                    defraimp_inspectioncoverageruleId = e.defraimp_inspectioncoverageruleId,
                    defraimp_NumberOfRecordsUntilInspection = e.defraimp_NumberOfRecordsUntilInspection,
                }
            ).FirstOrDefault();

            // Does the import application have a commodity? If not, inspection can't be determined
            if (importApplication.defraimp_CommodityTypeId != null)
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
                        // Increment the gold/bronze application counter
                        placeOfOriginRepo.IncrementApplicationCounter(placeOfOrigin.Id);

                        // Ensure our local copy also increments the counter
                        placeOfOrigin.defraimp_ApplicationCounter += 1;

                        if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                        {
                            GoldInspection(placeOfOrigin, (int)coverageRule.defraimp_NumberOfRecordsUntilInspection);
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
                    inspectionRequirement.P1Inspection();
                }
            }
            else
            {
                // Can't determine risk level
                inspectionRequirement.RiskLevelUnknown();
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

        private void GoldInspection(defraimp_placeoforigin placeOfOrigin, int coverageRuleValue)
        {
            // We want a local copy of the quota so we can adjust it and not have to keep retrieving the record.
            int inspectionQuotaCounter = placeOfOrigin.defraimp_InspectionQuotaCounter ?? 0;
            // Check if the number of applications counter is over the coverage rule value
            if (placeOfOrigin.defraimp_ApplicationCounter >= coverageRuleValue)
            {
                // +1 to quota
                placeOfOriginRepo.IncrementQuotaCounter(placeOfOrigin.Id);
                inspectionQuotaCounter += 1;

                // Reset the application counter
                placeOfOriginRepo.SetApplicationCounter(placeOfOrigin.Id, 0);
            }

            // Check the outstanding inspection counter, see if we need to apply an inspection
            if (inspectionQuotaCounter > 0)
            {
                // -1 from quota
                placeOfOriginRepo.DecrementQuotaCounter(placeOfOrigin.Id);

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
