namespace Defra.Imports.UnitTests.Dataverse.Queries
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    public sealed class PlaceOfOriginQueriesTests
    {
        private readonly Guid placeOfOriginId = Guid.NewGuid();
        private readonly Mock<IOrganizationService> service = new Mock<IOrganizationService>(MockBehavior.Strict);
        private readonly ImportsContext context;
        private readonly PlaceOfOriginQueries queries;

        public PlaceOfOriginQueriesTests()
        {
            this.context = new ImportsContext(this.service.Object);
            this.queries = new PlaceOfOriginQueries(this.context);
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PlaceOfOriginQueries(null));
        }

        [Fact]
        public void Constructor_Context_StopsTrackingBecauseTheQueriesAreReadOnly()
        {
            Assert.Equal(MergeOption.NoTracking, this.context.MergeOption);
        }

        [Fact]
        public void Find_ExistingPlaceOfOrigin_ReturnsIt()
        {
            this.service.ReturnsRecords(
                defraimp_placeoforigin.EntityLogicalName,
                new defraimp_placeoforigin { Id = this.placeOfOriginId, defraimp_name = "France" });

            var placeOfOrigin = this.queries.Find(this.placeOfOriginId);

            Assert.Equal(this.placeOfOriginId, placeOfOrigin.Id);
            Assert.Equal("France", placeOfOrigin.defraimp_name);
        }

        [Fact]
        public void Find_ExistingPlaceOfOrigin_FiltersByIdentifierAndRetrievesAllColumns()
        {
            var recorded = this.service.ReturnsRecords(
                defraimp_placeoforigin.EntityLogicalName,
                new defraimp_placeoforigin { Id = this.placeOfOriginId });

            this.queries.Find(this.placeOfOriginId);

            var query = Assert.Single(recorded);
            Assert.True(query.UsesOnlyAndFilters());
            var condition = Assert.Single(query.GetAllConditions());
            Assert.Equal(defraimp_placeoforigin.Fields.defraimp_placeoforiginId, condition.AttributeName);
            Assert.Equal(ConditionOperator.Equal, condition.Operator);
            Assert.Equal(this.placeOfOriginId, Assert.Single(condition.Values));
            Assert.True(query.ColumnSet.AllColumns);
        }

        [Fact]
        public void Find_NoMatch_ReturnsNull()
        {
            this.service.ReturnsRecords(defraimp_placeoforigin.EntityLogicalName);

            Assert.Null(this.queries.Find(this.placeOfOriginId));
        }
    }
}
