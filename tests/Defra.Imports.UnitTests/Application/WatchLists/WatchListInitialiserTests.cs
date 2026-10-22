namespace Defra.Imports.UnitTests.Application.WatchLists
{
    using System;
    using System.Linq;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    /// <summary>
    /// Tests the ILLUSTRATIVE EXAMPLE <see cref="WatchListInitialiser"/> from
    /// architecture/adr/0001-plugin-workflow-di-scaffold.md. This is not a real business process and must not be
    /// copied as a template for entity or business choices.
    /// </summary>
    public sealed class WatchListInitialiserTests
    {
        private readonly Guid watchListId = Guid.NewGuid();
        private readonly Guid ownerId = Guid.NewGuid();
        private readonly Guid transporterId = Guid.NewGuid();
        private readonly Mock<IOrganizationService> service = new Mock<IOrganizationService>(MockBehavior.Strict);
        private readonly WatchListInitialiser initialiser;

        public WatchListInitialiserTests()
        {
            this.initialiser = new WatchListInitialiser(new WatchListQueries(new ImportsContext(this.service.Object)), this.service.Object);
            this.service.ReturnsRecords(SystemUser.EntityLogicalName, WatchListTestData.CreateOwner(this.ownerId));
            this.service.ReturnsRecords(defraimp_Transporter.EntityLogicalName, WatchListTestData.CreateTransporter(this.transporterId));
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WatchListInitialiser(null, this.service.Object));
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            var queries = new WatchListQueries(new ImportsContext(this.service.Object));

            Assert.Throws<ArgumentNullException>(() => new WatchListInitialiser(queries, null));
        }

        [Fact]
        public void GetInitialisedWatchList_NullWatchList_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.initialiser.GetInitialisedWatchList(null));
        }

        [Fact]
        public void GetInitialisedWatchList_BlankValues_ReturnsADifferentInstance()
        {
            var watchList = WatchListTestData.CreateWatchList(this.watchListId, this.ownerId);

            var initialised = this.initialiser.GetInitialisedWatchList(watchList);

            Assert.NotSame(watchList, initialised);
            Assert.Equal(this.watchListId, initialised.Id);
        }

        [Fact]
        public void GetInitialisedWatchList_BlankValues_DoesNotMutateTheInput()
        {
            var watchList = WatchListTestData.CreateWatchList(this.watchListId, this.ownerId, name: " ");
            var before = watchList.Attributes.ToDictionary(attribute => attribute.Key, attribute => attribute.Value);

            this.initialiser.GetInitialisedWatchList(watchList);

            Assert.Equal(before.Count, watchList.Attributes.Count);
            Assert.All(before, attribute => Assert.Same(attribute.Value, watchList[attribute.Key]));
            Assert.Null(watchList.defraimp_Comments);
        }

        [Fact]
        public void GetInitialisedWatchList_BlankValues_DoesNotPersistAnything()
        {
            this.initialiser.GetInitialisedWatchList(WatchListTestData.CreateWatchList(this.watchListId, this.ownerId));

            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
            this.service.Verify(value => value.Create(It.IsAny<Entity>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void GetInitialisedWatchList_BlankNameAndComments_DefaultsBoth(string blank)
        {
            var watchList = WatchListTestData.CreateWatchList(this.watchListId, this.ownerId, name: blank, comments: blank);

            var initialised = this.initialiser.GetInitialisedWatchList(watchList);

            Assert.Equal(WatchListTestData.UnassignedName, initialised.defraimp_Name);
            Assert.Equal(WatchListTestData.OwnerComments, initialised.defraimp_Comments);
        }

        [Fact]
        public void GetInitialisedWatchList_SuppliedNameAndComments_PreservesThem()
        {
            var watchList = WatchListTestData.CreateWatchList(this.watchListId, this.ownerId, name: "Supplied name", comments: "Supplied comments");

            var initialised = this.initialiser.GetInitialisedWatchList(watchList);

            Assert.Equal("Supplied name", initialised.defraimp_Name);
            Assert.Equal("Supplied comments", initialised.defraimp_Comments);
        }

        [Fact]
        public void GetInitialisedWatchList_OnlyNameSupplied_DefaultsOnlyTheComments()
        {
            var watchList = WatchListTestData.CreateWatchList(this.watchListId, this.ownerId, name: "Supplied name");

            var initialised = this.initialiser.GetInitialisedWatchList(watchList);

            Assert.Equal("Supplied name", initialised.defraimp_Name);
            Assert.Equal(WatchListTestData.OwnerComments, initialised.defraimp_Comments);
        }

        [Fact]
        public void GetInitialisedWatchList_WatchListWithTransporter_UsesTheTransporterNameInTheDefaultName()
        {
            var watchList = WatchListTestData.CreateWatchList(this.watchListId, this.ownerId, this.transporterId);

            var initialised = this.initialiser.GetInitialisedWatchList(watchList);

            Assert.Equal(WatchListTestData.TransporterWatchListName, initialised.defraimp_Name);
        }

        [Fact]
        public void GetInitialisedWatchList_TargetWithoutAnIdentifier_DoesNotInventOne()
        {
            var watchList = WatchListTestData.CreateWatchList(Guid.Empty, this.ownerId);

            var initialised = this.initialiser.GetInitialisedWatchList(watchList);

            Assert.Equal(Guid.Empty, initialised.Id);
            Assert.False(initialised.Contains(defraimp_WatchList.Fields.defraimp_WatchListId));
        }

        [Fact]
        public void GetInitialisedWatchList_WatchListWithoutAnOwner_ThrowsInvalidOperationExceptionWithoutQuerying()
        {
            var watchList = WatchListTestData.CreateWatchList(this.watchListId, null);

            Assert.Throws<InvalidOperationException>(() => this.initialiser.GetInitialisedWatchList(watchList));
            this.service.VerifyQueriedAny(Times.Never());
        }

        [Fact]
        public void GetInitialisedWatchList_OwnerNotFound_PropagatesTheQueryFailure()
        {
            var watchList = WatchListTestData.CreateWatchList(this.watchListId, Guid.NewGuid());
            this.service.ReturnsRecords(SystemUser.EntityLogicalName);

            Assert.Throws<InvalidOperationException>(() => this.initialiser.GetInitialisedWatchList(watchList));
        }

        [Fact]
        public void InitialiseWatchList_BlankValues_SendsOneUpdateWithOnlyTheChangedValues()
        {
            var stored = WatchListTestData.CreateWatchList(this.watchListId, this.ownerId);
            this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName, stored);
            Entity sent = null;
            this.service.Setup(value => value.Update(It.IsAny<Entity>())).Callback<Entity>(entity => sent = entity);

            this.initialiser.InitialiseWatchList(this.watchListId);

            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Once);
            Assert.Equal(defraimp_WatchList.EntityLogicalName, sent.LogicalName);
            Assert.Equal(this.watchListId, sent.Id);
            Assert.Equal(
                new[] { "defraimp_comments", "defraimp_name" },
                sent.Attributes.Keys.OrderBy(key => key, StringComparer.Ordinal));
            Assert.Equal(WatchListTestData.UnassignedName, sent["defraimp_name"]);
            Assert.Equal(WatchListTestData.OwnerComments, sent["defraimp_comments"]);
        }

        [Fact]
        public void InitialiseWatchList_OnlyCommentsBlank_SendsOnlyTheComments()
        {
            var stored = WatchListTestData.CreateWatchList(this.watchListId, this.ownerId, name: "Supplied name");
            this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName, stored);
            Entity sent = null;
            this.service.Setup(value => value.Update(It.IsAny<Entity>())).Callback<Entity>(entity => sent = entity);

            this.initialiser.InitialiseWatchList(this.watchListId);

            var change = Assert.Single(sent.Attributes);
            Assert.Equal("defraimp_comments", change.Key);
        }

        [Fact]
        public void InitialiseWatchList_AlreadyInitialised_SendsNoUpdate()
        {
            var stored = WatchListTestData.CreateWatchList(this.watchListId, this.ownerId, name: "Supplied name", comments: "Supplied comments");
            this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName, stored);

            this.initialiser.InitialiseWatchList(this.watchListId);

            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void InitialiseWatchList_NoMatchingWatchList_ThrowsInvalidOperationExceptionWithoutUpdating()
        {
            this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName);

            Assert.Throws<InvalidOperationException>(() => this.initialiser.InitialiseWatchList(this.watchListId));
            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
        }
    }
}
