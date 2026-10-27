namespace Defra.Imports.UnitTests.Plugins.Itahc
{
    using System;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.Itahc;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class FlagRecordOnWatchListHandlerTests
    {
        private const string UnsupportedMessage = "Update";
        private const string OtherEntity = "account";

        private readonly PluginRuntime runtime = new PluginRuntime();
        private readonly Mock<IWatchFlagCreator> creator = new Mock<IWatchFlagCreator>(MockBehavior.Strict);
        private readonly FlagRecordOnWatchListHandler handler;
        private CreateItahcWatchFlagsContext received;

        public FlagRecordOnWatchListHandlerTests()
        {
            this.handler = new FlagRecordOnWatchListHandler(this.creator.Object, this.runtime.CreateExecution());
            this.creator
                .Setup(value => value.CreateWatchFlags(It.IsAny<CreateItahcWatchFlagsContext>()))
                .Callback<CreateItahcWatchFlagsContext>(context => this.received = context);
        }

        [Fact]
        public void Constructor_NullWatchFlagCreator_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FlagRecordOnWatchListHandler(null, this.runtime.CreateExecution()));
        }

        [Fact]
        public void Constructor_NullExecution_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FlagRecordOnWatchListHandler(this.creator.Object, null));
        }

        [Fact]
        public void Execute_Create_PassesTheTargetsValuesToTheWatchFlagCreator()
        {
            var target = WatchFlagTestData.Itahc(name: "ITAHC", consignee: "Acme");
            this.ForPostOperation(PluginMessages.Create).WithTarget(target);

            this.handler.Execute();

            Assert.Equal(target.Id, this.received.Itahc.Id);
            Assert.Equal(defraimp_itahc.EntityLogicalName, this.received.Itahc.LogicalName);
            Assert.Equal("ITAHC", this.received.Name);
            Assert.Equal("Acme", this.received.Consignee);
            this.creator.Verify(value => value.CreateWatchFlags(It.IsAny<CreateItahcWatchFlagsContext>()), Times.Once);
        }

        [Fact]
        public void Execute_Create_MatchesTheMessageNameIgnoringCase()
        {
            this.ForPostOperation(PluginMessages.Create.ToLowerInvariant()).WithTarget(WatchFlagTestData.Itahc());

            this.handler.Execute();

            Assert.NotNull(this.received);
        }

        [Fact]
        public void Execute_Create_DoesNotCatchTheWatchFlagCreatorsFailure()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.Itahc());
            this.creator
                .Setup(value => value.CreateWatchFlags(It.IsAny<CreateItahcWatchFlagsContext>()))
                .Throws(new InvalidOperationException("The watch flag could not be created."));

            var exception = Assert.Throws<InvalidOperationException>(() => this.handler.Execute());

            Assert.Equal("The watch flag could not be created.", exception.Message);
        }

        [Fact]
        public void Execute_WhenTheTargetIsMissing_Throws()
        {
            this.ForPostOperation(PluginMessages.Create).WithoutTarget();

            this.AssertFailsWithoutCreatingFlags();
        }

        [Fact]
        public void Execute_WhenTheTargetIsNotAnEntity_Throws()
        {
            this.ForPostOperation(PluginMessages.Create);
            this.runtime.InputParameters[PluginInputParameters.Target] = new EntityReference(defraimp_itahc.EntityLogicalName, Guid.NewGuid());

            this.AssertFailsWithoutCreatingFlags();
        }

        [Fact]
        public void Execute_Create_PassesAttributesTheTargetDidNotSupplyAsNull()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.Itahc(consignee: "Acme"));

            this.handler.Execute();

            Assert.Equal("Acme", this.received.Consignee);
            Assert.Null(this.received.Name);
            Assert.Null(this.received.PlaceOfOrigin);
            Assert.Null(this.received.PlaceOfDestination);
            Assert.Null(this.received.Transporter);
            Assert.Null(this.received.Veterinarian);
        }

        [Fact]
        public void Execute_UnsupportedMessage_Throws()
        {
            this.ForPostOperation(UnsupportedMessage).WithTarget(WatchFlagTestData.Itahc());

            this.AssertFailsWithoutCreatingFlags();
        }

        [Fact]
        public void Execute_OtherEntity_Throws()
        {
            this.runtime.ForStep(PluginMessages.Create, OtherEntity, PluginStages.PostOperation, PluginModes.Synchronous).WithTarget(WatchFlagTestData.Itahc());

            this.AssertFailsWithoutCreatingFlags();
        }

        [Theory]
        [InlineData(PluginStages.PreValidation)]
        [InlineData(PluginStages.PreOperation)]
        public void Execute_StageOtherThanPostOperation_Throws(int stage)
        {
            this.runtime.ForStep(PluginMessages.Create, defraimp_itahc.EntityLogicalName, stage, PluginModes.Synchronous).WithTarget(WatchFlagTestData.Itahc());

            this.AssertFailsWithoutCreatingFlags();
        }

        [Fact]
        public void Execute_AsynchronousMode_Throws()
        {
            this.runtime.ForStep(PluginMessages.Create, defraimp_itahc.EntityLogicalName, PluginStages.PostOperation, PluginModes.Asynchronous).WithTarget(WatchFlagTestData.Itahc());

            this.AssertFailsWithoutCreatingFlags();
        }

        private PluginRuntime ForPostOperation(string messageName)
        {
            return this.runtime.ForStep(messageName, defraimp_itahc.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
        }

        private void AssertFailsWithoutCreatingFlags()
        {
            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            this.creator.Verify(value => value.CreateWatchFlags(It.IsAny<CreateItahcWatchFlagsContext>()), Times.Never);
        }
    }
}
