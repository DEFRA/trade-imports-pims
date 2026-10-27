namespace Defra.Imports.UnitTests.Dataverse.Queries
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    public sealed class InspectionQueriesTests
    {
        private const string CommodityTypeLogicalName = "defraexp_commoditytype";
        private readonly Guid riskLevelId = Guid.NewGuid();
        private readonly Mock<IOrganizationService> service = new Mock<IOrganizationService>(MockBehavior.Strict);
        private readonly ImportsContext context;
        private readonly InspectionQueries queries;

        public InspectionQueriesTests()
        {
            this.context = new ImportsContext(this.service.Object);
            this.queries = new InspectionQueries(this.context);
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionQueries(null));
        }

        [Fact]
        public void Constructor_Context_StopsTrackingBecauseTheQueriesAreReadOnly()
        {
            Assert.Equal(MergeOption.NoTracking, this.context.MergeOption);
        }

        [Fact]
        public void GetImportRiskLevelName_ExistingRiskLevel_ReturnsItsName()
        {
            this.service.ReturnsRecords(
                defraimp_importrisklevel.EntityLogicalName,
                new defraimp_importrisklevel { Id = this.riskLevelId, defraimp_name = "High" });

            Assert.Equal("High", this.queries.GetImportRiskLevelName(this.riskLevelId));
        }

        [Fact]
        public void GetImportRiskLevelName_ExistingRiskLevel_FiltersByIdentifierAndRetrievesOnlyTheName()
        {
            var recorded = this.service.ReturnsRecords(
                defraimp_importrisklevel.EntityLogicalName,
                new defraimp_importrisklevel { Id = this.riskLevelId, defraimp_name = "High" });

            this.queries.GetImportRiskLevelName(this.riskLevelId);

            var query = Assert.Single(recorded);
            Assert.True(query.UsesOnlyAndFilters());
            var condition = Assert.Single(query.GetAllConditions());
            Assert.Equal(defraimp_importrisklevel.Fields.defraimp_importrisklevelId, condition.AttributeName);
            Assert.Equal(ConditionOperator.Equal, condition.Operator);
            Assert.Equal(this.riskLevelId, Assert.Single(condition.Values));
            Assert.False(query.ColumnSet.AllColumns);
            Assert.Equal(new[] { defraimp_importrisklevel.Fields.defraimp_name }, query.ColumnSet.Columns.ToArray());
        }

        [Fact]
        public void GetImportRiskLevelName_RiskLevelWithoutAName_ReturnsNull()
        {
            this.service.ReturnsRecords(
                defraimp_importrisklevel.EntityLogicalName,
                new defraimp_importrisklevel { Id = this.riskLevelId });

            Assert.Null(this.queries.GetImportRiskLevelName(this.riskLevelId));
        }

        [Fact]
        public void GetImportRiskLevelName_NoMatchingRiskLevel_ReturnsAnEmptyString()
        {
            this.service.ReturnsRecords(defraimp_importrisklevel.EntityLogicalName);

            Assert.Equal(string.Empty, this.queries.GetImportRiskLevelName(this.riskLevelId));
        }

        [Fact]
        public void FindCoverageRuleByKey_MatchingRule_ReturnsItsNameIdentifierAndThreshold()
        {
            var ruleId = Guid.NewGuid();
            this.service.ReturnsRecords(
                defraimp_inspectioncoveragerule.EntityLogicalName,
                new defraimp_inspectioncoveragerule
                {
                    Id = ruleId,
                    defraimp_name = "Random",
                    defraimp_NumberOfRecordsUntilInspection = 10,
                });

            var rule = this.queries.FindCoverageRuleByKey("RANDOM");

            Assert.Equal("Random", rule.defraimp_name);
            Assert.Equal(ruleId, rule.Id);
            Assert.Equal(10, rule.defraimp_NumberOfRecordsUntilInspection);
        }

        [Fact]
        public void FindCoverageRuleByKey_MatchingRule_FiltersByKeyAndRetrievesTheRuleColumns()
        {
            var recorded = this.service.ReturnsRecords(defraimp_inspectioncoveragerule.EntityLogicalName);

            this.queries.FindCoverageRuleByKey("RANDOM");

            var query = Assert.Single(recorded);
            Assert.True(query.UsesOnlyAndFilters());
            var condition = Assert.Single(query.GetAllConditions());
            Assert.Equal(defraimp_inspectioncoveragerule.Fields.defraimp_Key, condition.AttributeName);
            Assert.Equal(ConditionOperator.Equal, condition.Operator);
            Assert.Equal("RANDOM", Assert.Single(condition.Values));
            Assert.False(query.ColumnSet.AllColumns);
            Assert.Equal(
                new[]
                {
                    defraimp_inspectioncoveragerule.Fields.defraimp_name,
                    defraimp_inspectioncoveragerule.Fields.defraimp_inspectioncoverageruleId,
                    defraimp_inspectioncoveragerule.Fields.defraimp_NumberOfRecordsUntilInspection,
                },
                query.ColumnSet.Columns.ToArray());
        }

        [Fact]
        public void FindCoverageRuleByKey_SeveralMatches_ReturnsTheFirst()
        {
            var firstId = Guid.NewGuid();
            this.service.ReturnsRecords(
                defraimp_inspectioncoveragerule.EntityLogicalName,
                new defraimp_inspectioncoveragerule { Id = firstId },
                new defraimp_inspectioncoveragerule { Id = Guid.NewGuid() });

            Assert.Equal(firstId, this.queries.FindCoverageRuleByKey("RANDOM").Id);
        }

        [Fact]
        public void FindCoverageRuleByKey_NoMatch_ReturnsNull()
        {
            this.service.ReturnsRecords(defraimp_inspectioncoveragerule.EntityLogicalName);

            Assert.Null(this.queries.FindCoverageRuleByKey("RANDOM"));
        }

        [Fact]
        public void FindCoverageRuleByRiskLevel_MatchingRule_ReturnsItsNameIdentifierAndThreshold()
        {
            var ruleId = Guid.NewGuid();
            this.service.ReturnsRecords(
                defraimp_inspectioncoveragerule.EntityLogicalName,
                new defraimp_inspectioncoveragerule
                {
                    Id = ruleId,
                    defraimp_name = "Random",
                    defraimp_NumberOfRecordsUntilInspection = 10,
                });

            var rule = this.queries.FindCoverageRuleByRiskLevel(this.riskLevelId);

            Assert.Equal("Random", rule.defraimp_name);
            Assert.Equal(ruleId, rule.Id);
            Assert.Equal(10, rule.defraimp_NumberOfRecordsUntilInspection);
        }

        [Fact]
        public void FindCoverageRuleByRiskLevel_MatchingRule_FiltersByRiskLevelAndRetrievesTheRuleColumns()
        {
            var recorded = this.service.ReturnsRecords(defraimp_inspectioncoveragerule.EntityLogicalName);

            this.queries.FindCoverageRuleByRiskLevel(this.riskLevelId);

            var query = Assert.Single(recorded);
            Assert.True(query.UsesOnlyAndFilters());
            var condition = Assert.Single(query.GetAllConditions());
            Assert.Equal(defraimp_inspectioncoveragerule.Fields.defraimp_RiskLevelId, condition.AttributeName);
            Assert.Equal(ConditionOperator.Equal, condition.Operator);
            Assert.Equal(this.riskLevelId, Assert.Single(condition.Values));
            Assert.False(query.ColumnSet.AllColumns);
            Assert.Equal(
                new[]
                {
                    defraimp_inspectioncoveragerule.Fields.defraimp_name,
                    defraimp_inspectioncoveragerule.Fields.defraimp_inspectioncoverageruleId,
                    defraimp_inspectioncoveragerule.Fields.defraimp_NumberOfRecordsUntilInspection,
                },
                query.ColumnSet.Columns.ToArray());
        }

        [Fact]
        public void FindCoverageRuleByRiskLevel_SeveralMatches_ReturnsTheFirst()
        {
            var firstId = Guid.NewGuid();
            this.service.ReturnsRecords(
                defraimp_inspectioncoveragerule.EntityLogicalName,
                new defraimp_inspectioncoveragerule { Id = firstId },
                new defraimp_inspectioncoveragerule { Id = Guid.NewGuid() });

            Assert.Equal(firstId, this.queries.FindCoverageRuleByRiskLevel(this.riskLevelId).Id);
        }

        [Fact]
        public void FindCoverageRuleByRiskLevel_NoMatch_ReturnsNull()
        {
            this.service.ReturnsRecords(defraimp_inspectioncoveragerule.EntityLogicalName);

            Assert.Null(this.queries.FindCoverageRuleByRiskLevel(this.riskLevelId));
        }

        [Fact]
        public void GetConfigurationParameterValue_ExistingParameter_ReturnsItsValue()
        {
            this.service.ReturnsRecords(
                defraexp_configurationparameter.EntityLogicalName,
                new defraexp_configurationparameter { defraexp_Value = "42" });

            Assert.Equal("42", this.queries.GetConfigurationParameterValue("KEY"));
        }

        [Fact]
        public void GetConfigurationParameterValue_ExistingParameter_FiltersByKeyAndRetrievesOnlyTheValue()
        {
            var recorded = this.service.ReturnsRecords(
                defraexp_configurationparameter.EntityLogicalName,
                new defraexp_configurationparameter { defraexp_Value = "42" });

            this.queries.GetConfigurationParameterValue("KEY");

            var query = Assert.Single(recorded);
            Assert.True(query.UsesOnlyAndFilters());
            var condition = Assert.Single(query.GetAllConditions());
            Assert.Equal(defraexp_configurationparameter.Fields.defraexp_Key, condition.AttributeName);
            Assert.Equal(ConditionOperator.Equal, condition.Operator);
            Assert.Equal("KEY", Assert.Single(condition.Values));
            Assert.False(query.ColumnSet.AllColumns);
            Assert.Equal(new[] { defraexp_configurationparameter.Fields.defraexp_Value }, query.ColumnSet.Columns.ToArray());
        }

        [Fact]
        public void GetConfigurationParameterValue_ParameterWithoutAValue_ReturnsNull()
        {
            this.service.ReturnsRecords(defraexp_configurationparameter.EntityLogicalName, new defraexp_configurationparameter());

            Assert.Null(this.queries.GetConfigurationParameterValue("KEY"));
        }

        [Fact]
        public void GetConfigurationParameterValue_NoMatchingParameter_ReturnsNull()
        {
            this.service.ReturnsRecords(defraexp_configurationparameter.EntityLogicalName);

            Assert.Null(this.queries.GetConfigurationParameterValue("KEY"));
        }

        [Fact]
        public void IsCoveredByGoldBronze_NullApplication_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.queries.IsCoveredByGoldBronze(null));
        }

        [Fact]
        public void IsCoveredByGoldBronze_NoCommodityType_ReturnsFalseWithoutQuerying()
        {
            var application = Application(commodityType: null, countryOfOrigin: this.CountryReference());

            Assert.False(this.queries.IsCoveredByGoldBronze(application));

            this.service.VerifyQueriedAny(Times.Never());
        }

        [Fact]
        public void IsCoveredByGoldBronze_NoCountryOfOrigin_ReturnsFalseWithoutQuerying()
        {
            var application = Application(commodityType: this.CommodityTypeReference(), countryOfOrigin: null);

            Assert.False(this.queries.IsCoveredByGoldBronze(application));

            this.service.VerifyQueriedAny(Times.Never());
        }

        [Fact]
        public void IsCoveredByGoldBronze_CommodityWithoutTheCountry_ReturnsFalse()
        {
            this.ReturnsGoldBronzeCommodity(Guid.NewGuid());
            this.service.ReturnsRecords(defraimp_goldbronzecountriesnn.EntityLogicalName);

            Assert.False(this.queries.IsCoveredByGoldBronze(this.ApplicationWithCommodityAndCountry()));
        }

        [Fact]
        public void IsCoveredByGoldBronze_CommodityWithTheCountry_ReturnsTrue()
        {
            this.ReturnsGoldBronzeCommodity(Guid.NewGuid());
            this.service.ReturnsRecords(
                defraimp_goldbronzecountriesnn.EntityLogicalName,
                new defraimp_goldbronzecountriesnn { Id = Guid.NewGuid() });

            Assert.True(this.queries.IsCoveredByGoldBronze(this.ApplicationWithCommodityAndCountry()));
        }

        [Fact]
        public void IsCommodityCoveredByGoldBronze_NoActiveCommodityRule_ReturnsFalseWithoutCheckingTheCountry()
        {
            this.service.ReturnsRecords(defraimp_goldbronzecommodity.EntityLogicalName);

            Assert.False(this.queries.IsCommodityCoveredByGoldBronze(this.CommodityTypeReference(), this.CountryReference()));

            this.service.VerifyQueried(defraimp_goldbronzecountriesnn.EntityLogicalName, Times.Never());
        }

        [Fact]
        public void IsCommodityCoveredByGoldBronze_CommodityQuery_FiltersByCommodityTypeAndActiveState()
        {
            var commodityTypeId = Guid.NewGuid();
            var recorded = this.service.ReturnsRecords(defraimp_goldbronzecommodity.EntityLogicalName);

            this.queries.IsCommodityCoveredByGoldBronze(new EntityReference(CommodityTypeLogicalName, commodityTypeId), this.CountryReference());

            var query = Assert.Single(recorded);
            Assert.True(query.UsesOnlyAndFilters());
            var conditions = query.GetAllConditions();
            Assert.Equal(2, conditions.Count);
            var commodityType = Assert.Single(conditions, condition => condition.AttributeName == defraimp_goldbronzecommodity.Fields.defraimp_CommodityTypeid);
            Assert.Equal(ConditionOperator.Equal, commodityType.Operator);
            Assert.Equal(commodityTypeId, Assert.Single(commodityType.Values));
            var state = Assert.Single(conditions, condition => condition.AttributeName == defraimp_goldbronzecommodity.Fields.statecode);
            Assert.Equal(ConditionOperator.Equal, state.Operator);
            Assert.Equal((int)defraimp_goldbronzecommodity_statecode.Active, Assert.Single(state.Values));
        }

        [Fact]
        public void IsCommodityCoveredByGoldBronze_CountryQuery_FiltersByCountryAndTheFoundCommodity()
        {
            var commodityId = Guid.NewGuid();
            var countryId = Guid.NewGuid();
            this.ReturnsGoldBronzeCommodity(commodityId);
            var recorded = this.service.ReturnsRecords(defraimp_goldbronzecountriesnn.EntityLogicalName);

            this.queries.IsCommodityCoveredByGoldBronze(this.CommodityTypeReference(), new EntityReference(defra_country.EntityLogicalName, countryId));

            var query = Assert.Single(recorded);
            Assert.True(query.UsesOnlyAndFilters());
            var conditions = query.GetAllConditions();
            Assert.Equal(2, conditions.Count);
            var country = Assert.Single(conditions, condition => condition.AttributeName == defraimp_goldbronzecountriesnn.Fields.defra_countryid);
            Assert.Equal(ConditionOperator.Equal, country.Operator);
            Assert.Equal(countryId, Assert.Single(country.Values));
            var commodity = Assert.Single(conditions, condition => condition.AttributeName == defraimp_goldbronzecountriesnn.Fields.defraimp_goldbronzecommodityid);
            Assert.Equal(ConditionOperator.Equal, commodity.Operator);
            Assert.Equal(commodityId, Assert.Single(commodity.Values));
        }

        [Fact]
        public void IsCommodityCoveredByGoldBronze_CommodityAndCountryFound_ReturnsTrue()
        {
            this.ReturnsGoldBronzeCommodity(Guid.NewGuid());
            this.service.ReturnsRecords(
                defraimp_goldbronzecountriesnn.EntityLogicalName,
                new defraimp_goldbronzecountriesnn { Id = Guid.NewGuid() });

            Assert.True(this.queries.IsCommodityCoveredByGoldBronze(this.CommodityTypeReference(), this.CountryReference()));
        }

        [Fact]
        public void IsCommodityCoveredByGoldBronze_NullCommodityType_FailsWithoutQuerying()
        {
            // The catch of NullReferenceException cannot catch the TargetInvocationException that the provider throws.
            Assert.Throws<TargetInvocationException>(() => this.queries.IsCommodityCoveredByGoldBronze(null, this.CountryReference()));

            this.service.VerifyQueriedAny(Times.Never());
        }

        [Fact]
        public void IsCommodityCoveredByGoldBronze_NullCountryOfOriginAndCommodityFound_FailsBeforeTheCountryIsQueried()
        {
            this.ReturnsGoldBronzeCommodity(Guid.NewGuid());

            Assert.Throws<TargetInvocationException>(() => this.queries.IsCommodityCoveredByGoldBronze(this.CommodityTypeReference(), null));

            this.service.VerifyQueried(defraimp_goldbronzecountriesnn.EntityLogicalName, Times.Never());
        }

        private static InspectionImportApplicationContext Application(EntityReference commodityType, EntityReference countryOfOrigin)
        {
            return new InspectionImportApplicationContext(
                Guid.NewGuid(),
                null,
                null,
                null,
                null,
                null,
                null,
                commodityType,
                countryOfOrigin,
                null,
                null,
                null,
                null,
                null,
                null);
        }

        private InspectionImportApplicationContext ApplicationWithCommodityAndCountry()
        {
            return Application(this.CommodityTypeReference(), this.CountryReference());
        }

        private EntityReference CommodityTypeReference()
        {
            return new EntityReference(CommodityTypeLogicalName, Guid.NewGuid());
        }

        private EntityReference CountryReference()
        {
            return new EntityReference(defra_country.EntityLogicalName, Guid.NewGuid());
        }

        private void ReturnsGoldBronzeCommodity(Guid commodityId)
        {
            this.service.ReturnsRecords(
                defraimp_goldbronzecommodity.EntityLogicalName,
                new defraimp_goldbronzecommodity { Id = commodityId });
        }
    }
}
