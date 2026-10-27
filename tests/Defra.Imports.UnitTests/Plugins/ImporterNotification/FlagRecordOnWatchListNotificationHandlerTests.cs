namespace Defra.Imports.UnitTests.Plugins.ImporterNotification
{
    using System;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.ImporterNotification;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class FlagRecordOnWatchListNotificationHandlerTests
    {
        private const string UnsupportedMessage = "Update";
        private const string OtherEntity = "account";

        private readonly PluginRuntime runtime = new PluginRuntime();
        private readonly Mock<IWatchFlagCreator> creator = new Mock<IWatchFlagCreator>(MockBehavior.Strict);
        private readonly FlagRecordOnWatchListNotificationHandler handler;
        private CreateNotificationWatchFlagsContext received;

        public FlagRecordOnWatchListNotificationHandlerTests()
        {
            this.handler = new FlagRecordOnWatchListNotificationHandler(this.creator.Object, this.runtime.CreateExecution());
            this.creator
                .Setup(value => value.CreateWatchFlags(It.IsAny<CreateNotificationWatchFlagsContext>()))
                .Callback<CreateNotificationWatchFlagsContext>(context => this.received = context);
        }

        [Fact]
        public void Constructor_NullWatchFlagCreator_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FlagRecordOnWatchListNotificationHandler(null, this.runtime.CreateExecution()));
        }

        [Fact]
        public void Constructor_NullExecution_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new FlagRecordOnWatchListNotificationHandler(this.creator.Object, null));
        }

        [Fact]
        public void Execute_Create_PassesTheTargetsValuesToTheWatchFlagCreator()
        {
            var target = WatchFlagTestData.ImporterNotification(name: "Notification", importer: "Acme");
            this.ForPostOperation(PluginMessages.Create).WithTarget(target);

            this.handler.Execute();

            Assert.Equal(target.Id, this.received.Notification.Id);
            Assert.Equal(defraimp_ImporterNotification.EntityLogicalName, this.received.Notification.LogicalName);
            Assert.Equal("Notification", this.received.Name);
            Assert.Equal("Acme", this.received.Importer);
            this.creator.Verify(value => value.CreateWatchFlags(It.IsAny<CreateNotificationWatchFlagsContext>()), Times.Once);
        }

        [Fact]
        public void Execute_Create_MatchesTheMessageNameIgnoringCase()
        {
            this.ForPostOperation(PluginMessages.Create.ToLowerInvariant()).WithTarget(WatchFlagTestData.ImporterNotification());

            this.handler.Execute();

            Assert.NotNull(this.received);
        }

        [Fact]
        public void Execute_Create_DoesNotCatchTheWatchFlagCreatorsFailure()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.ImporterNotification());
            this.creator
                .Setup(value => value.CreateWatchFlags(It.IsAny<CreateNotificationWatchFlagsContext>()))
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
            this.runtime.InputParameters[PluginInputParameters.Target] = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, Guid.NewGuid());

            this.AssertFailsWithoutCreatingFlags();
        }

        [Fact]
        public void Execute_Create_PassesAttributesTheTargetDidNotSupplyAsNull()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.ImporterNotification(importer: "Acme"));

            this.handler.Execute();

            Assert.Equal("Acme", this.received.Importer);
            Assert.Null(this.received.Name);
            Assert.Null(this.received.Consignor);
            Assert.Null(this.received.PlaceOfDestination);
            Assert.Null(this.received.Transporter);
        }

        [Fact]
        public void Execute_UnsupportedMessage_Throws()
        {
            this.ForPostOperation(UnsupportedMessage).WithTarget(WatchFlagTestData.ImporterNotification());

            this.AssertFailsWithoutCreatingFlags();
        }

        [Fact]
        public void Execute_OtherEntity_Throws()
        {
            this.runtime.ForStep(PluginMessages.Create, OtherEntity, PluginStages.PostOperation, PluginModes.Synchronous).WithTarget(WatchFlagTestData.ImporterNotification());

            this.AssertFailsWithoutCreatingFlags();
        }

        [Theory]
        [InlineData(PluginStages.PreValidation)]
        [InlineData(PluginStages.PreOperation)]
        public void Execute_StageOtherThanPostOperation_Throws(int stage)
        {
            this.runtime.ForStep(PluginMessages.Create, defraimp_ImporterNotification.EntityLogicalName, stage, PluginModes.Synchronous).WithTarget(WatchFlagTestData.ImporterNotification());

            this.AssertFailsWithoutCreatingFlags();
        }

        [Fact]
        public void Execute_AsynchronousMode_Throws()
        {
            this.runtime.ForStep(PluginMessages.Create, defraimp_ImporterNotification.EntityLogicalName, PluginStages.PostOperation, PluginModes.Asynchronous).WithTarget(WatchFlagTestData.ImporterNotification());

            this.AssertFailsWithoutCreatingFlags();
        }

                    private PluginRuntime ForPostOperation(string messageName)
        {
            return this.runtime.ForStep(messageName, defraimp_ImporterNotification.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
        }

        private void AssertFailsWithoutCreatingFlags()
        {
            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            this.creator.Verify(value => value.CreateWatchFlags(It.IsAny<CreateNotificationWatchFlagsContext>()), Times.Never);
        }
    }
}
