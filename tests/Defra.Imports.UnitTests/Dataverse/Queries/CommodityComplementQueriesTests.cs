namespace Defra.Imports.UnitTests.Dataverse.Queries
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    public sealed class CommodityComplementQueriesTests
    {
        private readonly Guid parameterSetId = Guid.NewGuid();
        private readonly Guid importerNotificationId = Guid.NewGuid();
        private readonly Guid commodityComplementId = Guid.NewGuid();
        private readonly Mock<IOrganizationService> service = new Mock<IOrganizationService>(MockBehavior.Strict);
        private readonly ImportsContext context;
        private readonly CommodityComplementQueries queries;

        public CommodityComplementQueriesTests()
        {
            this.context = new ImportsContext(this.service.Object);
            this.queries = new CommodityComplementQueries(this.context);
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CommodityComplementQueries(null));
        }

        [Fact]
        public void Constructor_Context_StopsTrackingBecauseTheQueriesAreReadOnly()
        {
            Assert.Equal(MergeOption.NoTracking, this.context.MergeOption);
        }

        [Fact]
        public void GetParameterSetName_ExistingParameterSet_ReturnsItsName()
        {
            this.service.ReturnsRecords(
                defraimp_commoditycomplementparameterset.EntityLogicalName,
                new defraimp_commoditycomplementparameterset { Id = this.parameterSetId, defraimp_name = "42" });

            var name = this.queries.GetParameterSetName(this.parameterSetId);

            Assert.Equal("42", name);
        }

        [Fact]
        public void GetParameterSetName_ExistingParameterSet_QueriesByIdentifierForOnlyTheName()
        {
            var recorded = this.service.ReturnsRecords(
                defraimp_commoditycomplementparameterset.EntityLogicalName,
                new defraimp_commoditycomplementparameterset { Id = this.parameterSetId, defraimp_name = "42" });

            this.queries.GetParameterSetName(this.parameterSetId);

            var query = Assert.Single(recorded);
            var condition = Assert.Single(query.Criteria.Conditions);
            Assert.Equal(defraimp_commoditycomplementparameterset.Fields.defraimp_commoditycomplementparametersetId, condition.AttributeName);
            Assert.Equal(ConditionOperator.Equal, condition.Operator);
            Assert.Equal(this.parameterSetId, Assert.Single(condition.Values));
            Assert.False(query.ColumnSet.AllColumns);
            Assert.Equal(new[] { defraimp_commoditycomplementparameterset.Fields.defraimp_name }, query.ColumnSet.Columns);
        }

        [Fact]
        public void GetParameterSetName_ParameterSetWithoutAName_ReturnsNull()
        {
            this.service.ReturnsRecords(
                defraimp_commoditycomplementparameterset.EntityLogicalName,
                new defraimp_commoditycomplementparameterset { Id = this.parameterSetId });

            Assert.Null(this.queries.GetParameterSetName(this.parameterSetId));
        }

        [Fact]
        public void GetParameterSetName_NoMatchingParameterSet_ThrowsInvalidOperationException()
        {
            this.service.ReturnsRecords(defraimp_commoditycomplementparameterset.EntityLogicalName);

            Assert.Throws<InvalidOperationException>(() => this.queries.GetParameterSetName(this.parameterSetId));
        }

        [Fact]
        public void FindCommodityComplementId_MatchingCommodityComplement_ReturnsItsIdentifier()
        {
            this.service.ReturnsRecords(
                defraimp_commoditycomplement.EntityLogicalName,
                new defraimp_commoditycomplement { Id = this.commodityComplementId });

            var id = this.queries.FindCommodityComplementId(this.importerNotificationId, 42);

            Assert.Equal(this.commodityComplementId, id);
        }

        [Fact]
        public void FindCommodityComplementId_MatchingCommodityComplement_FiltersByNotificationAndComplementIdentifier()
        {
            var recorded = this.service.ReturnsRecords(
                defraimp_commoditycomplement.EntityLogicalName,
                new defraimp_commoditycomplement { Id = this.commodityComplementId });

            this.queries.FindCommodityComplementId(this.importerNotificationId, 42);

            var query = Assert.Single(recorded);
            Assert.True(query.UsesOnlyAndFilters());
            var conditions = query.GetAllConditions();
            Assert.Equal(2, conditions.Count);
            var notification = Assert.Single(conditions, condition => condition.AttributeName == defraimp_commoditycomplement.Fields.defraimp_ImporterNotificationId);
            Assert.Equal(ConditionOperator.Equal, notification.Operator);
            Assert.Equal(this.importerNotificationId, Assert.Single(notification.Values));
            var complement = Assert.Single(conditions, condition => condition.AttributeName == defraimp_commoditycomplement.Fields.defraimp_complementid);
            Assert.Equal(ConditionOperator.Equal, complement.Operator);
            Assert.Equal(42, Assert.Single(complement.Values));
        }

        [Fact]
        public void FindCommodityComplementId_MatchingCommodityComplement_RetrievesOnlyTheIdentifier()
        {
            var recorded = this.service.ReturnsRecords(
                defraimp_commoditycomplement.EntityLogicalName,
                new defraimp_commoditycomplement { Id = this.commodityComplementId });

            this.queries.FindCommodityComplementId(this.importerNotificationId, 42);

            var columns = Assert.Single(recorded).ColumnSet;
            Assert.False(columns.AllColumns);
            Assert.Equal(new[] { defraimp_commoditycomplement.Fields.defraimp_commoditycomplementId }, columns.Columns.ToArray());
        }

        [Fact]
        public void FindCommodityComplementId_SeveralMatches_ReturnsTheFirst()
        {
            var second = Guid.NewGuid();
            this.service.ReturnsRecords(
                defraimp_commoditycomplement.EntityLogicalName,
                new defraimp_commoditycomplement { Id = this.commodityComplementId },
                new defraimp_commoditycomplement { Id = second });

            Assert.Equal(this.commodityComplementId, this.queries.FindCommodityComplementId(this.importerNotificationId, 42));
        }

        [Fact]
        public void FindCommodityComplementId_NoMatch_ReturnsNull()
        {
            this.service.ReturnsRecords(defraimp_commoditycomplement.EntityLogicalName);

            Assert.Null(this.queries.FindCommodityComplementId(this.importerNotificationId, 42));
        }
    }
}
