namespace Defra.Imports.UnitTests.Support
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Application.ReferenceData;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Microsoft.Xrm.Sdk;

    // Builders for the records that the inspection requirement logic reads.
    internal static class InspectionTestData
    {
        public const string CommodityTypeLogicalName = "defraexp_commoditytype";
        public const string CountryLogicalName = "defra_country";

        public static defraimp_autonumber Autonumber(string key, int? value)
        {
            return new defraimp_autonumber
            {
                defraimp_autonumberId = Guid.NewGuid(),
                defraimp_Key = key,
                defraimp_CurrentNumber = value,
            };
        }

        public static defraimp_placeoforigin PlaceOfOrigin(
            defraimp_trustlevel? trustLevel,
            int? applicationCounter = 0,
            int? quotaCounter = 0,
            bool? lockToBronze = false)
        {
            return new defraimp_placeoforigin
            {
                defraimp_placeoforiginId = Guid.NewGuid(),
                defraimp_TrustLevel = trustLevel,
                defraimp_ApplicationCounter = applicationCounter,
                defraimp_InspectionQuotaCounter = quotaCounter,
                defraimp_LocktoBronze = lockToBronze,
            };
        }

        public static defraimp_inspectioncoveragerule CoverageRule(string key, int? threshold, Guid? riskLevelId = null)
        {
            var rule = new defraimp_inspectioncoveragerule
            {
                defraimp_inspectioncoverageruleId = Guid.NewGuid(),
                defraimp_Key = key,
                defraimp_NumberOfRecordsUntilInspection = threshold,
            };

            if (riskLevelId.HasValue)
            {
                rule.defraimp_RiskLevelId = new EntityReference(defraimp_importrisklevel.EntityLogicalName, riskLevelId.Value);
            }

            return rule;
        }

        public static defraimp_importrisklevel RiskLevel(string name)
        {
            return new defraimp_importrisklevel
            {
                defraimp_importrisklevelId = Guid.NewGuid(),
                defraimp_name = name,
            };
        }

        public static defraexp_configurationparameter ConfigurationParameter(string key, string value)
        {
            return new defraexp_configurationparameter
            {
                defraexp_configurationparameterId = Guid.NewGuid(),
                defraexp_Key = key,
                defraexp_Value = value,
            };
        }

        public static EntityReference RiskLevelReference(defraimp_importrisklevel riskLevel)
        {
            return new EntityReference(defraimp_importrisklevel.EntityLogicalName, riskLevel.Id) { Name = riskLevel.defraimp_name };
        }

        public static EntityReference PlaceOfOriginReference(defraimp_placeoforigin placeOfOrigin)
        {
            return new EntityReference(defraimp_placeoforigin.EntityLogicalName, placeOfOrigin.Id);
        }

        public static EntityReference CommodityTypeReference()
        {
            return new EntityReference(CommodityTypeLogicalName, Guid.NewGuid());
        }

        public static EntityReference CountryReference()
        {
            return new EntityReference(CountryLogicalName, Guid.NewGuid());
        }

        // The gold/bronze commodity and its country, so that the gold/bronze rules apply to the references.
        public static Entity[] GoldBronzeRules(EntityReference commodityType, EntityReference country)
        {
            var commodity = new defraimp_goldbronzecommodity
            {
                defraimp_goldbronzecommodityId = Guid.NewGuid(),
                defraimp_CommodityTypeid = commodityType,
                statecode = defraimp_goldbronzecommodity_statecode.Active,
            };

            var link = new defraimp_goldbronzecountriesnn { Id = Guid.NewGuid() };
            link[defraimp_goldbronzecountriesnn.Fields.defra_countryid] = country.Id;
            link[defraimp_goldbronzecountriesnn.Fields.defraimp_goldbronzecommodityid] = commodity.Id;

            return new Entity[] { commodity, link };
        }

        // The records that every determination reads: the counters, the coverage rules, the risk levels and the setting that enables TRACES.
        public static InspectionDataverse WithReferenceData(this InspectionDataverse dataverse, bool tracesEnabled = true)
        {
            return dataverse.Seed(
                Autonumber(InspectionConstants.P1CounterName, 0),
                Autonumber(InspectionConstants.P1QuotaCounterName, 0),
                Autonumber(InspectionConstants.P2CounterName, 0),
                Autonumber(InspectionConstants.P2QuotaCounterName, 0),
                Autonumber(InspectionConstants.P3CounterName, 0),
                Autonumber(InspectionConstants.P3QuotaCounterName, 0),
                CoverageRule(InspectionConstants.P1CoverageRuleKey, 2),
                CoverageRule(InspectionConstants.P2CoverageRuleKey, 2),
                CoverageRule(InspectionConstants.P3CoverageRuleKey, 2),
                CoverageRule(InspectionConstants.GoldBronzeCoverageRuleKey, 2),
                ConfigurationParameter(ConfigurationParameters.TracesEnabledKey, tracesEnabled ? "true" : "false"));
        }

        public static int CounterValue(this InspectionDataverse dataverse, string key)
        {
            foreach (var autonumber in dataverse.StoredRecords<defraimp_autonumber>())
            {
                if (autonumber.defraimp_Key == key)
                {
                    return autonumber.defraimp_CurrentNumber.Value;
                }
            }

            throw new InvalidOperationException("No autonumber has the key " + key + ".");
        }

        public static void SetCounter(this InspectionDataverse dataverse, string key, int value)
        {
            foreach (var autonumber in dataverse.StoredRecords<defraimp_autonumber>())
            {
                if (autonumber.defraimp_Key == key)
                {
                    autonumber.defraimp_CurrentNumber = value;
                    dataverse.Replace(autonumber);
                    return;
                }
            }

            throw new InvalidOperationException("No autonumber has the key " + key + ".");
        }

        // An import application with the values that the inspection requirement depends on. A record that has an
        // ITAHC type and a primary ITAHC is inspected, which suits most cases.
        public static InspectionImportApplicationContext Application(
            Guid? id = null,
            defraimp_importapplication_defraimp_importapplicationtype? type = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
            EntityReference riskLevel = null,
            EntityReference placeOfOrigin = null,
            EntityReference previousPlaceOfOrigin = null,
            EntityReference commodityType = null,
            EntityReference country = null,
            bool hasPrimaryItahc = true,
            bool hasPrimaryNotification = false,
            bool? counted = false,
            defraimp_importapplication_statecode? stateCode = defraimp_importapplication_statecode.Active,
            defraimp_importapplication_statuscode? statusCode = null,
            defraimp_importapplication_defraimp_inspectionrequired? inspectionRequired = null,
            defraimp_importapplication_defraimp_inspectionrequiredreason? inspectionRequiredReason = null,
            defraimp_importapplication_defraimp_manualpostimportcheckdecision? decision = defraimp_importapplication_defraimp_manualpostimportcheckdecision.UseSystemDecision)
        {
            return new InspectionImportApplicationContext(
                id ?? Guid.NewGuid(),
                type,
                riskLevel,
                inspectionRequired,
                inspectionRequiredReason,
                placeOfOrigin,
                previousPlaceOfOrigin,
                commodityType,
                country,
                hasPrimaryItahc ? new EntityReference(defraimp_itahc.EntityLogicalName, Guid.NewGuid()) : null,
                hasPrimaryNotification ? new EntityReference(defraimp_ImporterNotification.EntityLogicalName, Guid.NewGuid()) : null,
                stateCode,
                statusCode,
                counted,
                decision);
        }
    }
}
