namespace Defra.Imports.UnitTests.Plugins.WatchLists
{
    using System;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Plugins.WatchLists;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    /// <summary>
    /// Tests the ILLUSTRATIVE EXAMPLE <see cref="WatchListInitialisationHandler"/> from
    /// architecture/adr/0001-plugin-workflow-di-scaffold.md. This is not a real business process and must not be
    /// copied as a template for entity or business choices.
    /// </summary>
    public sealed class WatchListInitialisationHandlerTests
    {
        private const string InitialisedComments = "Initialised comments";

        private readonly Guid ownerId = Guid.NewGuid();
        private readonly PluginRuntime runtime = new PluginRuntime();
        private readonly Mock<IWatchListInitialiser> initialiser = new Mock<IWatchListInitialiser>(MockBehavior.Strict);
        private readonly Entity target;
        private readonly WatchListInitialisationHandler handler;

        public WatchListInitialisationHandlerTests()
        {
            this.target = new Entity(defraimp_WatchList.EntityLogicalName)
            {
                ["ownerid"] = new EntityReference(SystemUser.EntityLogicalName, this.ownerId),
                ["defraimp_name"] = "Supplied name",
            };
            this.runtime.ForPreOperationCreate(this.target);
            this.handler = new WatchListInitialisationHandler(this.initialiser.Object, this.runtime.CreateExecution());
        }

        [Fact]
        public void Constructor_NullInitialiser_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WatchListInitialisationHandler(null, this.runtime.CreateExecution()));
        }

        [Fact]
        public void Constructor_NullExecution_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WatchListInitialisationHandler(this.initialiser.Object, null));
        }

        [Fact]
        public void Execute_PreOperationCreate_InitialisesTheInMemoryTargetWithoutReReadingIt()
        {
            defraimp_WatchList received = null;
            this.SetupInitialiser(watchList => received = watchList);

            this.handler.Execute();

            this.initialiser.Verify(value => value.GetInitialisedWatchList(It.IsAny<defraimp_WatchList>()), Times.Once);
            Assert.Equal("Supplied name", received.defraimp_Name);
            Assert.Equal(this.ownerId, received.OwnerId.Id);
            this.runtime.Service.VerifyNoOtherCalls();
        }

        [Fact]
        public void Execute_PreOperationCreate_NeverCallsThePersistingMethod()
        {
            this.SetupInitialiser();

            this.handler.Execute();

            this.initialiser.Verify(value => value.InitialiseWatchList(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public void Execute_InitialiserChangesValues_CopiesOnlyTheChangedValuesOntoTheTarget()
        {
            this.SetupInitialiser();

            this.handler.Execute();

            Assert.Equal(InitialisedComments, this.target["defraimp_comments"]);
            Assert.Equal("Supplied name", this.target["defraimp_name"]);
            Assert.Equal(3, this.target.Attributes.Count);
        }

        [Fact]
        public void Execute_InitialiserChangesNothing_LeavesTheTargetUnchanged()
        {
            this.initialiser
                .Setup(value => value.GetInitialisedWatchList(It.IsAny<defraimp_WatchList>()))
                .Returns<defraimp_WatchList>(watchList => watchList.Clone());

            this.handler.Execute();

            Assert.Equal(2, this.target.Attributes.Count);
            Assert.Equal("Supplied name", this.target["defraimp_name"]);
        }

        [Fact]
        public void Execute_InitialiserFails_LeavesTheTargetUnchangedAndPropagatesTheFailure()
        {
            this.initialiser
                .Setup(value => value.GetInitialisedWatchList(It.IsAny<defraimp_WatchList>()))
                .Throws<InvalidOperationException>();

            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            Assert.Equal(2, this.target.Attributes.Count);
        }

        [Fact]
        public void Execute_PreOperationCreate_TracesHowManyValuesWereApplied()
        {
            this.SetupInitialiser();

            this.handler.Execute();

            Assert.Contains("Watch list initialised: 1 value(s) applied.", this.runtime.Tracing.Messages);
        }

        [Theory]
        [InlineData("Update", defraimp_WatchList.EntityLogicalName, 20, 0)]
        [InlineData("Create", "account", 20, 0)]
        [InlineData("Create", defraimp_WatchList.EntityLogicalName, 10, 0)]
        [InlineData("Create", defraimp_WatchList.EntityLogicalName, 40, 0)]
        [InlineData("Create", defraimp_WatchList.EntityLogicalName, 20, 1)]
        public void Execute_UnsupportedStep_ThrowsInvalidOperationWithoutCallingTheInitialiser(
            string messageName, string entityName, int stage, int mode)
        {
            this.runtime.ForStep(messageName, entityName, stage, mode);

            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            this.initialiser.VerifyNoOtherCalls();
            Assert.Equal(2, this.target.Attributes.Count);
        }

        [Fact]
        public void Execute_NoTarget_ThrowsInvalidOperationWithoutCallingTheInitialiser()
        {
            this.runtime.WithoutTarget();

            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            this.initialiser.VerifyNoOtherCalls();
        }

        [Fact]
        public void Execute_TargetIsNotAnEntity_ThrowsInvalidOperationWithoutCallingTheInitialiser()
        {
            this.runtime.InputParameters["Target"] = new EntityReference(defraimp_WatchList.EntityLogicalName, Guid.NewGuid());

            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            this.initialiser.VerifyNoOtherCalls();
        }

        // Returns a clone of the watch list with comments added, as the real initialiser would.
        private void SetupInitialiser(Action<defraimp_WatchList> observe = null)
        {
            this.initialiser
                .Setup(value => value.GetInitialisedWatchList(It.IsAny<defraimp_WatchList>()))
                .Returns<defraimp_WatchList>(watchList =>
                {
                    observe?.Invoke(watchList);
                    var initialised = watchList.Clone();
                    initialised.defraimp_Comments = InitialisedComments;
                    return initialised;
                });
        }
    }
}
