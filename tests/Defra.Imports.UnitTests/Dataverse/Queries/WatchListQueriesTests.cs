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

    public sealed class WatchListQueriesTests
    {
        private readonly Guid watchListId = Guid.NewGuid();
        private readonly Guid ownerId = Guid.NewGuid();
        private readonly Guid transporterId = Guid.NewGuid();
        private readonly Mock<IOrganizationService> service = new Mock<IOrganizationService>(MockBehavior.Strict);
        private readonly ImportsContext context;
        private readonly WatchListQueries queries;

        public WatchListQueriesTests()
        {
            this.context = new ImportsContext(this.service.Object);
            this.queries = new WatchListQueries(this.context);
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WatchListQueries(null));
        }

        [Fact]
        public void Constructor_Context_StopsTrackingBecauseTheQueriesAreReadOnly()
        {
            Assert.Equal(MergeOption.NoTracking, this.context.MergeOption);
        }

        [Fact]
        public void GetWatchListForInitialisation_ExistingWatchList_ReturnsTheValuesTheInitialiserReads()
        {
            var stored = WatchListTestData.CreateWatchList(this.watchListId, this.ownerId, this.transporterId, "Name", "Comments");
            this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName, stored);

            var watchList = this.queries.GetWatchListForInitialisation(this.watchListId);

            Assert.Equal(this.watchListId, watchList.Id);
            Assert.Equal(this.ownerId, watchList.OwnerId.Id);
            Assert.Equal(this.transporterId, watchList.defraimp_TransporterId.Id);
            Assert.Equal("Name", watchList.defraimp_Name);
            Assert.Equal("Comments", watchList.defraimp_Comments);
        }

        [Fact]
        public void GetWatchListForInitialisation_ExistingWatchList_QueriesByIdentifier()
        {
            var recorded = this.service.ReturnsRecords(
                defraimp_WatchList.EntityLogicalName,
                WatchListTestData.CreateWatchList(this.watchListId, this.ownerId));

            this.queries.GetWatchListForInitialisation(this.watchListId);

            var query = Assert.Single(recorded);
            var condition = Assert.Single(query.Criteria.Conditions);
            Assert.Equal("defraimp_watchlistid", condition.AttributeName);
            Assert.Equal(ConditionOperator.Equal, condition.Operator);
            Assert.Equal(this.watchListId, Assert.Single(condition.Values));
        }

        [Fact]
        public void GetWatchListForInitialisation_ExistingWatchList_RetrievesOnlyTheColumnsTheInitialiserReads()
        {
            var recorded = this.service.ReturnsRecords(
                defraimp_WatchList.EntityLogicalName,
                WatchListTestData.CreateWatchList(this.watchListId, this.ownerId));

            this.queries.GetWatchListForInitialisation(this.watchListId);

            var columns = Assert.Single(recorded).ColumnSet;
            Assert.False(columns.AllColumns);
            Assert.Equal(
                new[] { "defraimp_comments", "defraimp_name", "defraimp_transporterid", "defraimp_watchlistid", "ownerid" },
                columns.Columns.OrderBy(column => column, StringComparer.Ordinal));
        }

        [Fact]
        public void GetWatchListForInitialisation_NoMatchingWatchList_ThrowsInvalidOperationException()
        {
            this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName);

            Assert.Throws<InvalidOperationException>(() => this.queries.GetWatchListForInitialisation(this.watchListId));
        }

        [Fact]
        public void GetWatchListInitialisationContext_OwnerWithoutTransporter_ReturnsOwnerDetailsOnly()
        {
            this.service.ReturnsRecords(SystemUser.EntityLogicalName, WatchListTestData.CreateOwner(this.ownerId));

            var result = this.queries.GetWatchListInitialisationContext(this.ownerId, null);

            Assert.Equal(this.ownerId, result.OwnerId);
            Assert.Equal(WatchListTestData.OwnerName, result.OwnerName);
            Assert.Equal(WatchListTestData.OwnerEmail, result.OwnerEmail);
            Assert.Null(result.TransporterName);
        }

        [Fact]
        public void GetWatchListInitialisationContext_OwnerWithoutTransporter_DoesNotQueryTransporters()
        {
            this.service.ReturnsRecords(SystemUser.EntityLogicalName, WatchListTestData.CreateOwner(this.ownerId));

            this.queries.GetWatchListInitialisationContext(this.ownerId, null);

            this.service.VerifyQueriedAny(Times.Once());
        }

        [Fact]
        public void GetWatchListInitialisationContext_OwnerQuery_ReadsOnlyTheNameAndEmailOfTheUser()
        {
            var recorded = this.service.ReturnsRecords(SystemUser.EntityLogicalName, WatchListTestData.CreateOwner(this.ownerId));

            this.queries.GetWatchListInitialisationContext(this.ownerId, null);

            var query = Assert.Single(recorded);
            var condition = Assert.Single(query.Criteria.Conditions);
            Assert.Equal("systemuserid", condition.AttributeName);
            Assert.Equal(this.ownerId, Assert.Single(condition.Values));
            Assert.Equal(new[] { "fullname", "internalemailaddress" }, query.ColumnSet.Columns.OrderBy(column => column, StringComparer.Ordinal));
        }

        [Fact]
        public void GetWatchListInitialisationContext_OwnerWithTransporter_ReturnsTheTransporterName()
        {
            this.service.ReturnsRecords(SystemUser.EntityLogicalName, WatchListTestData.CreateOwner(this.ownerId));
            this.service.ReturnsRecords(defraimp_Transporter.EntityLogicalName, WatchListTestData.CreateTransporter(this.transporterId));

            var result = this.queries.GetWatchListInitialisationContext(this.ownerId, this.transporterId);

            Assert.Equal(WatchListTestData.TransporterName, result.TransporterName);
        }

        [Fact]
        public void GetWatchListInitialisationContext_TransporterQuery_ReadsOnlyTheNameOfTheTransporter()
        {
            this.service.ReturnsRecords(SystemUser.EntityLogicalName, WatchListTestData.CreateOwner(this.ownerId));
            var recorded = this.service.ReturnsRecords(
                defraimp_Transporter.EntityLogicalName, WatchListTestData.CreateTransporter(this.transporterId));

            this.queries.GetWatchListInitialisationContext(this.ownerId, this.transporterId);

            var query = Assert.Single(recorded);
            var condition = Assert.Single(query.Criteria.Conditions);
            Assert.Equal("defraimp_transporterid", condition.AttributeName);
            Assert.Equal(this.transporterId, Assert.Single(condition.Values));
            Assert.Equal(new[] { "defraimp_name" }, query.ColumnSet.Columns);
        }

        [Fact]
        public void GetWatchListInitialisationContext_TransporterNotFound_ReturnsContextWithoutATransporterName()
        {
            this.service.ReturnsRecords(SystemUser.EntityLogicalName, WatchListTestData.CreateOwner(this.ownerId));
            this.service.ReturnsRecords(defraimp_Transporter.EntityLogicalName);

            var result = this.queries.GetWatchListInitialisationContext(this.ownerId, this.transporterId);

            Assert.Null(result.TransporterName);
        }

        [Fact]
        public void GetWatchListInitialisationContext_OwnerNotAUser_ThrowsWithoutQueryingTransporters()
        {
            this.service.ReturnsRecords(SystemUser.EntityLogicalName);

            Assert.Throws<InvalidOperationException>(
                () => this.queries.GetWatchListInitialisationContext(this.ownerId, this.transporterId));
            this.service.VerifyQueriedAny(Times.Once());
        }
    }
}
