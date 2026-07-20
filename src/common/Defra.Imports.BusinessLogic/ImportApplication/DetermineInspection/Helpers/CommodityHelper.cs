namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers
{
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using Microsoft.Xrm.Sdk;
    using System;
    using System.Linq;

    public class CommodityHelper
    {
        private ICrmRepository<defraimp_goldbronzecommodity> _goldBronzeCommodityRepo;
        private IRepositoryFactory _repositoryFactory;

        public EntityReference Commodity { get; private set; }

        public bool CoveredByGoldBronze;

        public CommodityHelper(EntityReference commodityEntityReference, IRepositoryFactory repositoryFactory)
        {
            Commodity = commodityEntityReference;
            _goldBronzeCommodityRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_goldbronzecommodity>();
            _repositoryFactory = repositoryFactory;
        }

        public bool IsCommodityCoveredByGoldBronze(EntityReference countryOfOriginId)
        {
            try
            {
                defraimp_goldbronzecommodity goldBronzeCommodity = _goldBronzeCommodityRepo.Find<defraimp_goldbronzecommodity>(
                rule => rule.defraimp_CommodityTypeid.Id.Equals(Commodity.Id) && rule.statecode.Value.Equals(defraimp_goldbronzecommodityState.Active),
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
                    ICrmRepository<defraimp_goldbronzecountriesnn> goldBronzeCommodityCountriesRepo = _repositoryFactory.GetRepository<ImportsContext, defraimp_goldbronzecountriesnn>();

                    // Find the N:N relationship record between gold/bronze commodity and the country of origin.
                    defraimp_goldbronzecountriesnn goldBronzeCommodityCountry = goldBronzeCommodityCountriesRepo.Find(
                    rule => rule.defra_countryid.Equals(countryOfOriginId.Id) && rule.defraimp_goldbronzecommodityid.Equals(goldBronzeCommodity.defraimp_goldbronzecommodityId),
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
    }
}
