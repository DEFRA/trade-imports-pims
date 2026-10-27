namespace Defra.Imports.UnitTests.Plugins.ImportApplication
{
    using System;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.ImportApplication;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class AssociateWatchFlagsFromNotificationHandlerTests
    {
        private const string UnsupportedMessage = "Delete";
        private const string OtherEntity = "account";

        private readonly PluginRuntime runtime = new PluginRuntime();
        private readonly Mock<IWatchFlagAssociator> associator = new Mock<IWatchFlagAssociator>(MockBehavior.Strict);
        private readonly AssociateWatchFlagsFromNotificationHandler handler;
        private AssociateNotificationWatchFlagsContext received;

        public AssociateWatchFlagsFromNotificationHandlerTests()
        {
            this.handler = new AssociateWatchFlagsFromNotificationHandler(this.associator.Object, this.runtime.CreateExecution());
            this.associator
                .Setup(value => value.AssociateWatchFlags(It.IsAny<AssociateNotificationWatchFlagsContext>()))
                .Callback<AssociateNotificationWatchFlagsContext>(context => this.received = context);
        }

        [Fact]
        public void Constructor_NullWatchFlagAssociator_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AssociateWatchFlagsFromNotificationHandler(null, this.runtime.CreateExecution()));
        }

        [Fact]
        public void Constructor_NullExecution_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AssociateWatchFlagsFromNotificationHandler(this.associator.Object, null));
        }

        [Theory]
        [InlineData(PluginMessages.Create)]
        [InlineData(PluginMessages.Update)]
        public void Execute_WithAPrimaryNotification_PassesTheTargetsValuesToTheWatchFlagAssociator(string message)
        {
            var notificationId = Guid.NewGuid();
            var target = WatchFlagTestData.ImportApplication(primaryImporterNotificationId: notificationId);
            this.ForPostOperation(message).WithTarget(target);

            this.handler.Execute();

            Assert.Equal(target.Id, this.received.ImportApplicationId);
            Assert.Equal(notificationId, this.received.PrimaryImporterNotificationId);
            this.associator.Verify(value => value.AssociateWatchFlags(It.IsAny<AssociateNotificationWatchFlagsContext>()), Times.Once);
        }

        [Fact]
        public void Execute_UpdateWithoutAPrimaryNotification_StillPassesTheTargetToTheWatchFlagAssociator()
        {
            var target = WatchFlagTestData.ImportApplication();
            this.ForPostOperation(PluginMessages.Update).WithTarget(target);

            this.handler.Execute();

            Assert.Equal(target.Id, this.received.ImportApplicationId);
            Assert.Null(this.received.PrimaryImporterNotificationId);
        }

        [Fact]
        public void Execute_CreateWithoutAPrimaryNotification_DoesNotCallTheWatchFlagAssociator()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.ImportApplication(primaryItahcId: Guid.NewGuid()));

            this.handler.Execute();

            this.associator.Verify(value => value.AssociateWatchFlags(It.IsAny<AssociateNotificationWatchFlagsContext>()), Times.Never);
        }

        [Theory]
        [InlineData("create")]
        [InlineData("UPDATE")]
        public void Execute_MatchesTheMessageNameIgnoringCase(string message)
        {
            this.ForPostOperation(message).WithTarget(WatchFlagTestData.ImportApplication(primaryImporterNotificationId: Guid.NewGuid()));

            this.handler.Execute();

            Assert.NotNull(this.received);
        }

        [Fact]
        public void Execute_DoesNotCatchTheWatchFlagAssociatorsFailure()
        {
            this.ForPostOperation(PluginMessages.Update).WithTarget(WatchFlagTestData.ImportApplication());
            this.associator
                .Setup(value => value.AssociateWatchFlags(It.IsAny<AssociateNotificationWatchFlagsContext>()))
                .Throws(new InvalidOperationException("The watch flags could not be associated."));

            var exception = Assert.Throws<InvalidOperationException>(() => this.handler.Execute());

            Assert.Equal("The watch flags could not be associated.", exception.Message);
        }

        [Fact]
        public void Execute_WhenTheTargetIsMissing_Throws()
        {
            this.ForPostOperation(PluginMessages.Update).WithoutTarget();

            this.AssertFailsWithoutAssociating();
        }

        [Fact]
        public void Execute_WhenTheTargetIsNotAnEntity_Throws()
        {
            this.ForPostOperation(PluginMessages.Update);
            this.runtime.InputParameters[PluginInputParameters.Target] = new EntityReference(defraimp_importapplication.EntityLogicalName, Guid.NewGuid());

            this.AssertFailsWithoutAssociating();
        }

        [Fact]
        public void Execute_UnsupportedMessage_Throws()
        {
            this.ForPostOperation(UnsupportedMessage).WithTarget(WatchFlagTestData.ImportApplication());

            this.AssertFailsWithoutAssociating();
        }

        [Fact]
        public void Execute_OtherEntity_Throws()
        {
            this.runtime.ForStep(PluginMessages.Update, OtherEntity, PluginStages.PostOperation, PluginModes.Synchronous).WithTarget(WatchFlagTestData.ImportApplication());

            this.AssertFailsWithoutAssociating();
        }

        [Theory]
        [InlineData(PluginStages.PreValidation)]
        [InlineData(PluginStages.PreOperation)]
        public void Execute_StageOtherThanPostOperation_Throws(int stage)
        {
            this.runtime.ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, stage, PluginModes.Synchronous).WithTarget(WatchFlagTestData.ImportApplication());

            this.AssertFailsWithoutAssociating();
        }

        [Fact]
        public void Execute_AsynchronousMode_Throws()
        {
            this.runtime.ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Asynchronous).WithTarget(WatchFlagTestData.ImportApplication());

            this.AssertFailsWithoutAssociating();
        }

        private PluginRuntime ForPostOperation(string messageName)
        {
            return this.runtime.ForStep(messageName, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
        }

        private void AssertFailsWithoutAssociating()
        {
            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            this.associator.Verify(value => value.AssociateWatchFlags(It.IsAny<AssociateNotificationWatchFlagsContext>()), Times.Never);
        }
    }
}
