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
    public class P1DetermineInspection : AbstractDetermineInspection
    {
        private IRepositoryFactory repositoryFactory;
        private defraimp_importapplication importApplication;
        private ICrmRepository<defraimp_importapplication> importApplicationRepo;
        private IAutonumberRepository autoNumberRepo;
        private IPlaceOfOriginRepository placeOfOriginRepo;
        private ICrmRepository <defraimp_goldbronzecommodity>goldBronzeCommodityRepo;
        private ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo;

        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            repositoryFactory = determineInspectionContext.RepositoryFactory;
            importApplication = determineInspectionContext.ImportApplication;
            importApplicationRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_importapplication>();
            placeOfOriginRepo = determineInspectionContext.PlaceOfOriginRepo;
            goldBronzeCommodityRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_goldbronzecommodity>();
            coverageRulesRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_inspectioncoveragerule>();
            autoNumberRepo = determineInspectionContext.AutoNumberRepo;

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
                // Is the commodity gold/bronze? 
                if (IsCommodityCoveredByGoldBronze())
                {
                    // Try to get a place of origin
                    defraimp_placeoforigin placeOfOrigin = GetPlaceOfOrigin();

                    // Do we have a place of origin?
                    if (placeOfOrigin != null)
                    {
                        //Increment the gold/bronze application counter
                        placeOfOriginRepo.IncrementApplicationCounter(placeOfOrigin.Id);

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
                        InspectionPlaceOfOriginMissing(importApplication, importApplicationRepo);
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
                CantDetermineInspectionNoRiskLevel(importApplication, importApplicationRepo);
            }
        }

        private bool IsCommodityCoveredByGoldBronze()
        {
            try
            {
                defraimp_goldbronzecommodity goldBronzeCommodity = goldBronzeCommodityRepo.Find<defraimp_goldbronzecommodity>(
                rule => rule.defraimp_CommodityTypeid.Id.Equals(importApplication.defraimp_CommodityTypeId.Id) && rule.statecode.Value.Equals(defraimp_goldbronzecommodityState.Active),
                e => new defraimp_goldbronzecommodity()
                {
                    defraimp_goldbronzecommodityId = e.defraimp_goldbronzecommodityId,
                    defraimp_name = e.defraimp_name,
                    defraimp_CommodityTypeid = e.defraimp_CommodityTypeid,
                }
                ).FirstOrDefault();

                // Check if we found a rule for the given commodity
                if (goldBronzeCommodity != null)
                {
                    // Do these rules apply to the country of origin?
                    ICrmRepository<defraimp_goldbronzecountriesnn> goldBronzeCommodityCountriesRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_goldbronzecountriesnn>();

                    // Find the N:N relationship record between gold/bronze commodity and the country of origin.
                    defraimp_goldbronzecountriesnn goldBronzeCommodityCountry = goldBronzeCommodityCountriesRepo.Find(
                    rule => rule.defra_countryid.Equals(importApplication.defraimp_CountryofOriginId.Id) && rule.defraimp_goldbronzecommodityid.Equals(goldBronzeCommodity.defraimp_goldbronzecommodityId),
                    e => new defraimp_goldbronzecountriesnn()
                    ).FirstOrDefault();

                    // Check to see if we have found a valid country/gold bronze commodity combination
                    if (goldBronzeCommodityCountry != null)
                    {
                        // The given gold bronze commodity and country of origin place this inspection requirement under the gold/bronze rule.
                        return true;
                    }
                    else
                    {
                        // We did find a rule, but the country of origin was not included on the rule and thus G/B logic should not apply.
                        return false;
                    }
                }
                else
                {
                    // We did not find a valid rule with the given commodity
                    return false;
                }
            }
            catch (NullReferenceException e)
            {
                return false;
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
            // We want a local copy of the quota     so we can adjust it and not have to keep retrieving the record.
            int inspectionQuotaCounter = placeOfOrigin.defraimp_InspectionQuotaCounter ?? 0;
            // Check if the number of applications counter is over the coverage rule value
            if (placeOfOrigin.defraimp_ApplicationCounter >= coverageRuleValue)
            {
                // +1 to quota
                inspectionQuotaCounter++;
                placeOfOriginRepo.IncrementQuotaCounter(placeOfOrigin.Id);

                // Reset the application counter
                placeOfOriginRepo.SetApplicationCounter(placeOfOrigin.Id, 0);
            }

            // Check the outstanding inspection counter, see if we need to apply an inspection
            if (inspectionQuotaCounter > 0)
            {
                // -1 from quota
                inspectionQuotaCounter--;
                placeOfOriginRepo.DecrementQuotaCounter(placeOfOrigin.Id);

                // Set to inspect
                PerformInspectionRequiredUpdate(
                importApplication,
                importApplicationRepo,
                defraimp_importapplication_defraimp_inspectionrequired.Yes,
                defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage
                );
            }
            else
            {
                // Don't inspect
                PerformInspectionRequiredUpdate(
                importApplication,
                importApplicationRepo,
                defraimp_importapplication_defraimp_inspectionrequired.No,
                defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequiredGoldPlaceofOrigin
                );
            }
        }

        void BronzeInspection(defraimp_placeoforigin placeOfOrigin)
        {
            // Check if the commodity has been locked to bronze. If so, set that as the inspection reason.
            if (placeOfOrigin.defraimp_LocktoBronze == true)
            {
                // Set to inspect
                PerformInspectionRequiredUpdate(
                importApplication,
                importApplicationRepo,
                defraimp_importapplication_defraimp_inspectionrequired.Yes,
                defraimp_importapplication_defraimp_inspectionrequiredreason.PlaceofOriginLockedtoBronze
                );
            }
            else
            {
                // Set to inspect
                PerformInspectionRequiredUpdate(
                importApplication,
                importApplicationRepo,
                defraimp_importapplication_defraimp_inspectionrequired.Yes,
                defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin
                );
            }
        }

        void P1Inspection()
        {
            PerformInspectionRequiredUpdate(
            importApplication,
            importApplicationRepo,
            defraimp_importapplication_defraimp_inspectionrequired.Yes,
            defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection
            );
        }
    }
}
