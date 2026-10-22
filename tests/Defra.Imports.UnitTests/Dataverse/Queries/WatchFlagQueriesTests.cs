namespace Defra.Imports.UnitTests.Dataverse.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    public sealed class WatchFlagQueriesTests
    {
        private static readonly DateTime Today = new DateTime(2026, 10, 22);

        private readonly Mock<IOrganizationService> service = new Mock<IOrganizationService>(MockBehavior.Strict);
        private readonly ImportsContext context;
        private readonly WatchFlagQueries queries;

        public WatchFlagQueriesTests()
        {
            this.context = new ImportsContext(this.service.Object);
            this.queries = new WatchFlagQueries(this.context);
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WatchFlagQueries(null));
        }

        [Fact]
        public void Constructor_Context_StopsTrackingBecauseTheQueriesAreReadOnly()
        {
            Assert.Equal(MergeOption.NoTracking, this.context.MergeOption);
        }

        [Fact]
        public void GetActiveWatchLists_MatchingWatchLists_ReturnsThemInOrder()
        {
            var first = new defraimp_WatchList { Id = Guid.NewGuid(), defraimp_Name = "First", defraimp_WatchType = defraimp_watchtype.Consignee };
            var second = new defraimp_WatchList { Id = Guid.NewGuid(), defraimp_Name = "Second", defraimp_WatchType = defraimp_watchtype.Transporter };
            this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName, first, second);

            var watchLists = this.queries.GetActiveWatchLists(Today);

            Assert.Equal(new[] { first.Id, second.Id }, watchLists.Select(watchList => watchList.Id).ToArray());
            Assert.Equal(new[] { "First", "Second" }, watchLists.Select(watchList => watchList.defraimp_Name).ToArray());
            Assert.Equal(defraimp_watchtype.Transporter, watchLists[1].defraimp_WatchType);
        }

        [Fact]
        public void GetActiveWatchLists_Lookups_AreCopiedToTheResult()
        {
            var operatorReference = new EntityReference(defraimp_Consignee.EntityLogicalName, Guid.NewGuid());
            this.service.ReturnsRecords(
                defraimp_WatchList.EntityLogicalName,
                new defraimp_WatchList { Id = Guid.NewGuid(), defraimp_ConsigneeId = operatorReference });

            var watchList = Assert.Single(this.queries.GetActiveWatchLists(Today));

            Assert.Equal(operatorReference.Id, watchList.defraimp_ConsigneeId.Id);
        }

        [Fact]
        public void GetActiveWatchLists_NoMatches_ReturnsAnEmptyList()
        {
            this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName);

            Assert.Empty(this.queries.GetActiveWatchLists(Today));
        }

        [Fact]
        public void GetActiveWatchLists_Date_FiltersOnActiveStateAndAPeriodThatCoversTheDate()
        {
            var recorded = this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName);

            this.queries.GetActiveWatchLists(Today);

            var query = Assert.Single(recorded);
            var conditions = query.GetAllConditions();
            var state = Assert.Single(conditions, condition => condition.AttributeName == defraimp_WatchList.Fields.statecode);
            Assert.Equal(ConditionOperator.Equal, state.Operator);
            Assert.Equal((int)defraimp_watchlist_statecode.Active, Assert.Single(state.Values));
            var start = Assert.Single(conditions, condition => condition.AttributeName == defraimp_WatchList.Fields.defraimp_StartDate);
            Assert.Equal(ConditionOperator.LessEqual, start.Operator);
            Assert.Equal(Today, Assert.Single(start.Values));
            var end = Assert.Single(conditions, condition => condition.AttributeName == defraimp_WatchList.Fields.defraimp_EndDate && condition.Operator == ConditionOperator.GreaterEqual);
            Assert.Equal(Today, Assert.Single(end.Values));
        }

        [Fact]
        public void GetActiveWatchLists_Date_AcceptsAWatchListWithNoEndDate()
        {
            var recorded = this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName);

            this.queries.GetActiveWatchLists(Today);

            Assert.Contains(
                Assert.Single(recorded).GetAllConditions(),
                condition => condition.AttributeName == defraimp_WatchList.Fields.defraimp_EndDate && condition.Operator == ConditionOperator.Null);
        }

        [Fact]
        public void GetActiveWatchLists_Date_RetrievesOnlyTheColumnsTheFlaggingNeeds()
        {
            var recorded = this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName);

            this.queries.GetActiveWatchLists(Today);

            var columns = Assert.Single(recorded).ColumnSet;
            Assert.False(columns.AllColumns);
            var expected = new[]
            {
                defraimp_WatchList.Fields.defraimp_WatchListId,
                defraimp_WatchList.Fields.defraimp_Name,
                defraimp_WatchList.Fields.defraimp_StartDate,
                defraimp_WatchList.Fields.defraimp_EndDate,
                defraimp_WatchList.Fields.defraimp_WatchType,
                defraimp_WatchList.Fields.defraimp_PlaceOfOriginId,
                defraimp_WatchList.Fields.defraimp_PlaceOfDestinationId,
                defraimp_WatchList.Fields.defraimp_ConsigneeId,
                defraimp_WatchList.Fields.defraimp_TransporterId,
                defraimp_WatchList.Fields.defraimp_VeterinarianId,
            };
            Assert.Equal(expected.OrderBy(column => column), columns.Columns.OrderBy(column => column));
        }

        public static IEnumerable<object[]> WatchTypes => new[]
        {
            new object[] { defraimp_watchtype.PlaceofOrigin },
            new object[] { defraimp_watchtype.PlaceofDestination },
            new object[] { defraimp_watchtype.Consignee },
            new object[] { defraimp_watchtype.Transporter },
            new object[] { defraimp_watchtype.Veterinarian },
        };

        [Theory]
        [MemberData(nameof(WatchTypes))]
        public void FindEconomicOperatorId_MatchingOperator_ReturnsItsIdentifier(defraimp_watchtype watchType)
        {
            var id = Guid.NewGuid();
            this.service.ReturnsRecords(EconomicOperatorTestData.LogicalName(watchType), EconomicOperatorTestData.Create(watchType, id));

            Assert.Equal(id, this.queries.FindEconomicOperatorId(watchType, "Acme"));
        }

        [Theory]
        [MemberData(nameof(WatchTypes))]
        public void FindEconomicOperatorId_Name_FiltersOnlyByNameAndRetrievesOnlyTheIdentifier(defraimp_watchtype watchType)
        {
            var logicalName = EconomicOperatorTestData.LogicalName(watchType);
            var recorded = this.service.ReturnsRecords(logicalName, EconomicOperatorTestData.Create(watchType, Guid.NewGuid()));

            this.queries.FindEconomicOperatorId(watchType, "Acme");

            var query = Assert.Single(recorded);
            Assert.True(query.UsesOnlyAndFilters());
            var condition = Assert.Single(query.GetAllConditions());
            Assert.Equal(EconomicOperatorTestData.NameAttribute(watchType), condition.AttributeName);
            Assert.Equal(ConditionOperator.Equal, condition.Operator);
            Assert.Equal("Acme", Assert.Single(condition.Values));
            Assert.False(query.ColumnSet.AllColumns);
            Assert.Equal(new[] { EconomicOperatorTestData.IdAttribute(watchType) }, query.ColumnSet.Columns.ToArray());
        }

        [Fact]
        public void FindEconomicOperatorId_Name_QueriesOnlyTheTableOfTheWatchType()
        {
            this.service.ReturnsRecords(defraimp_Veterinarian.EntityLogicalName, new defraimp_Veterinarian { Id = Guid.NewGuid() });

            this.queries.FindEconomicOperatorId(defraimp_watchtype.Veterinarian, "Acme");

            this.service.VerifyQueriedAny(Times.Once());
            this.service.VerifyQueried(defraimp_Veterinarian.EntityLogicalName, Times.Once());
        }

        [Fact]
        public void FindEconomicOperatorId_SeveralMatches_ReturnsTheFirst()
        {
            var first = Guid.NewGuid();
            this.service.ReturnsRecords(
                defraimp_Consignee.EntityLogicalName,
                new defraimp_Consignee { Id = first },
                new defraimp_Consignee { Id = Guid.NewGuid() });

            Assert.Equal(first, this.queries.FindEconomicOperatorId(defraimp_watchtype.Consignee, "Acme"));
        }

        [Fact]
        public void FindEconomicOperatorId_NoMatch_ReturnsNull()
        {
            this.service.ReturnsRecords(defraimp_Consignee.EntityLogicalName);

            Assert.Null(this.queries.FindEconomicOperatorId(defraimp_watchtype.Consignee, "Acme"));
        }

        [Fact]
        public void FindEconomicOperatorId_WatchTypeWithoutAnOperatorTable_ThrowsWithoutQuerying()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => this.queries.FindEconomicOperatorId((defraimp_watchtype)0, "Acme"));
        }
    }
}
