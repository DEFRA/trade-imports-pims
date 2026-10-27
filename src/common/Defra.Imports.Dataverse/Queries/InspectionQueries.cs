namespace Defra.Imports.Dataverse.Queries
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;

    /// <summary>
    /// Read-only queries over inspection coverage rules, import risk levels, configuration parameters and the
    /// gold/bronze commodity rules that decide how an import application is inspected.
    /// </summary>
    public class InspectionQueries
    {
        private readonly ImportsContext context;

        /// <summary>Initializes a new instance of the <see cref="InspectionQueries"/> class.</summary>
        /// <param name="context">The invocation-local context, which these read-only queries stop tracking.</param>
        public InspectionQueries(ImportsContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            // These queries are read-only, so avoid change tracking on the invocation-local context.
            this.context.MergeOption = MergeOption.NoTracking;
        }

        /// <summary>
        /// Gets the name of an import risk level. The lookup is by identifier because the name of a lookup on a
        /// pre-image is not reliable.
        /// </summary>
        /// <param name="importRiskLevelId">The identifier of the import risk level.</param>
        /// <returns>The name, which is null if the record has none, or an empty string if the record is not found.</returns>
        public string GetImportRiskLevelName(Guid importRiskLevelId)
        {
            var query =
                from riskLevel in this.context.defraimp_importrisklevelSet
                where riskLevel.defraimp_importrisklevelId == importRiskLevelId
                select new defraimp_importrisklevel
                {
                    defraimp_name = riskLevel.defraimp_name,
                };

            var found = query.FirstOrDefault();
            return found != null ? found.defraimp_name : string.Empty;
        }

        /// <summary>
        /// Finds an inspection coverage rule by its key. The lookup is not constrained to a single record, so the
        /// first match returned is used.
        /// </summary>
        /// <param name="key">The key of the coverage rule.</param>
        /// <returns>The name, identifier and threshold of the rule, or null if there is none.</returns>
        public defraimp_inspectioncoveragerule FindCoverageRuleByKey(string key)
        {
            var query =
                from rule in this.context.defraimp_inspectioncoverageruleSet
                where rule.defraimp_Key == key
                select new defraimp_inspectioncoveragerule
                {
                    defraimp_name = rule.defraimp_name,
                    defraimp_inspectioncoverageruleId = rule.defraimp_inspectioncoverageruleId,
                    defraimp_NumberOfRecordsUntilInspection = rule.defraimp_NumberOfRecordsUntilInspection,
                };

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Finds an inspection coverage rule by its risk level. The lookup is not constrained to a single record, so
        /// the first match returned is used.
        /// </summary>
        /// <param name="importRiskLevelId">The identifier of the import risk level.</param>
        /// <returns>The name, identifier and threshold of the rule, or null if there is none.</returns>
        public defraimp_inspectioncoveragerule FindCoverageRuleByRiskLevel(Guid importRiskLevelId)
        {
            var query =
                from rule in this.context.defraimp_inspectioncoverageruleSet
                where rule.defraimp_RiskLevelId.Id == importRiskLevelId
                select new defraimp_inspectioncoveragerule
                {
                    defraimp_name = rule.defraimp_name,
                    defraimp_inspectioncoverageruleId = rule.defraimp_inspectioncoverageruleId,
                    defraimp_NumberOfRecordsUntilInspection = rule.defraimp_NumberOfRecordsUntilInspection,
                };

            return query.FirstOrDefault();
        }

        /// <summary>Gets the value of a configuration parameter.</summary>
        /// <param name="key">The key of the configuration parameter.</param>
        /// <returns>The value, or null if there is no such parameter or it has no value.</returns>
        public string GetConfigurationParameterValue(string key)
        {
            var query =
                from parameter in this.context.defraexp_configurationparameterSet
                where parameter.defraexp_Key == key
                select new defraexp_configurationparameter
                {
                    defraexp_Value = parameter.defraexp_Value,
                };

            return query.FirstOrDefault()?.defraexp_Value;
        }

        /// <summary>
        /// Determines whether the gold/bronze rules apply to an import application, which they cannot if it has no
        /// commodity type or country of origin.
        /// </summary>
        /// <param name="application">The import application.</param>
        /// <returns>True if the gold/bronze rules apply.</returns>
        public bool IsCoveredByGoldBronze(InspectionImportApplicationContext application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }

            return application.CommodityType != null
                && application.CountryOfOrigin != null
                && this.IsCommodityCoveredByGoldBronze(application.CommodityType, application.CountryOfOrigin);
        }

        /// <summary>
        /// Determines whether the gold/bronze rules apply to a commodity type and country of origin. They do if an
        /// active gold/bronze commodity exists for the commodity type and its countries include the country of
        /// origin. A missing reference is not guarded against: the query provider wraps the resulting
        /// <see cref="NullReferenceException"/> in a <see cref="System.Reflection.TargetInvocationException"/>, which escapes.
        /// </summary>
        /// <param name="commodityType">The commodity type.</param>
        /// <param name="countryOfOrigin">The country of origin.</param>
        /// <returns>True if the gold/bronze rules apply.</returns>
        public bool IsCommodityCoveredByGoldBronze(EntityReference commodityType, EntityReference countryOfOrigin)
        {
            try
            {
                var commodityQuery =
                    from commodity in this.context.defraimp_goldbronzecommoditySet
                    where commodity.defraimp_CommodityTypeid.Id == commodityType.Id
                        && commodity.statecode == defraimp_goldbronzecommodity_statecode.Active
                    select new defraimp_goldbronzecommodity
                    {
                        defraimp_goldbronzecommodityId = commodity.defraimp_goldbronzecommodityId,
                        defraimp_name = commodity.defraimp_name,
                        defraimp_CommodityTypeid = commodity.defraimp_CommodityTypeid,
                    };

                // The lookup is not constrained to a single record, so the first match returned is used.
                var goldBronzeCommodity = commodityQuery.FirstOrDefault();
                if (goldBronzeCommodity == null)
                {
                    return false;
                }

                var countryQuery =
                    from commodityCountry in this.context.defraimp_goldbronzecountriesnnSet
                    where commodityCountry.defra_countryid == countryOfOrigin.Id
                        && commodityCountry.defraimp_goldbronzecommodityid == goldBronzeCommodity.defraimp_goldbronzecommodityId
                    select new { commodityCountry.defra_countryid };

                return countryQuery.FirstOrDefault() != null;
            }
            catch (NullReferenceException)
            {
                // TODO: this catch cannot catch a missing reference, because the query provider wraps the exception in a TargetInvocationException, so it never returns false. This should be investigated, but is preserved by the refactoring.
                return false;
            }
        }
    }
}
