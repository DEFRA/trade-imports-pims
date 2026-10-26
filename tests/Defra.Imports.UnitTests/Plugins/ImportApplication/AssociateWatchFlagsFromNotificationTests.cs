namespace Defra.Imports.UnitTests.Plugins.ImportApplication
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.ImportApplication;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class AssociateWatchFlagsFromNotificationTests : PluginTestBase<AssociateWatchFlagsFromNotification>
    {
        private const string UnsupportedMessage = "Delete";

        private readonly Guid importApplicationId = Guid.NewGuid();
        private readonly Guid primaryNotificationId = Guid.NewGuid();
        private readonly List<Entity> updated = new List<Entity>();

        public AssociateWatchFlagsFromNotificationTests()
        {
            this.Runtime.Service
                .Setup(service => service.Update(It.IsAny<Entity>()))
                .Callback<Entity>(entity => this.updated.Add(entity));
        }

        [Fact]
        public void Plugin_Instance_IsAHandlerFirstPluginBase()
        {
            Assert.IsAssignableFrom<PluginBase<AssociateWatchFlagsFromNotificationHandler>>(this.Plugin);
        }

        [Fact]
        public void Execute_OnCreateWithAPrimaryNotification_ClearsTheLinkedWatchFlagsAndLinksThoseOfTheNotification()
        {
            var linked = WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId);
            var notificationFlag = WatchFlagTestData.WatchFlag();
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.ImportApplication(this.importApplicationId, primaryImporterNotificationId: this.primaryNotificationId));
            this.Runtime.Service.ReturnsRecordsInSequence(defraimp_WatchFlag.EntityLogicalName, new Entity[] { linked }, new Entity[] { notificationFlag });

            this.Execute();

            Assert.Equal(2, this.updated.Count);
            var cleared = Assert.IsType<defraimp_WatchFlag>(this.updated[0]);
            Assert.Equal(linked.Id, cleared.Id);
            Assert.Null(cleared.defraimp_ImportApplicationId);
            var linkedUpdate = Assert.IsType<defraimp_WatchFlag>(this.updated[1]);
            Assert.Equal(notificationFlag.Id, linkedUpdate.Id);
            Assert.Equal(this.importApplicationId, linkedUpdate.defraimp_ImportApplicationId.Id);
        }

        [Fact]
        public void Execute_OnCreateWithoutAPrimaryNotification_ReadsAndWritesNothing()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.ImportApplication(this.importApplicationId));

            this.Execute();

            this.Runtime.Service.VerifyNoOtherCalls();
            Assert.Empty(this.updated);
        }

        [Fact]
        public void Execute_OnCreateWithAPrimaryItahcOnly_ReadsAndWritesNothing()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.ImportApplication(this.importApplicationId, primaryItahcId: Guid.NewGuid()));

            this.Execute();

            this.Runtime.Service.VerifyNoOtherCalls();
        }

        [Fact]
        public void Execute_OnUpdateWithAPrimaryNotification_ClearsTheLinkedWatchFlagsAndLinksThoseOfTheNotification()
        {
            var notificationFlag = WatchFlagTestData.WatchFlag(importApplicationId: Guid.NewGuid());
            this.ForPostOperation(PluginMessages.Update).WithTarget(WatchFlagTestData.ImportApplication(this.importApplicationId, primaryImporterNotificationId: this.primaryNotificationId));
            this.Runtime.Service.ReturnsRecordsInSequence(defraimp_WatchFlag.EntityLogicalName, new Entity[0], new Entity[] { notificationFlag });

            this.Execute();

            var linkedUpdate = Assert.IsType<defraimp_WatchFlag>(Assert.Single(this.updated));
            Assert.Equal(notificationFlag.Id, linkedUpdate.Id);
            Assert.Equal(this.importApplicationId, linkedUpdate.defraimp_ImportApplicationId.Id);
        }

        [Fact]
        public void Execute_OnUpdateClearingThePrimaryNotification_ClearsTheLinkedWatchFlagsOnly()
        {
            var linked = WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId);
            this.ForPostOperation(PluginMessages.Update).WithTarget(WatchFlagTestData.ImportApplication(this.importApplicationId));
            var queries = this.Runtime.Service.ReturnsRecordsInSequence(defraimp_WatchFlag.EntityLogicalName, new Entity[] { linked });

            this.Execute();

            Assert.Single(queries);
            var cleared = Assert.IsType<defraimp_WatchFlag>(Assert.Single(this.updated));
            Assert.Equal(linked.Id, cleared.Id);
            Assert.Null(cleared.defraimp_ImportApplicationId);
        }

        [Fact]
        public void Execute_WhenTheTargetIsMissing_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForPostOperation(PluginMessages.Update).WithoutTarget();

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_UnsupportedMessage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForPostOperation(UnsupportedMessage).WithTarget(WatchFlagTestData.ImportApplication(this.importApplicationId, primaryImporterNotificationId: this.primaryNotificationId));

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_OtherEntity_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForStep(PluginMessages.Update, defraimp_ImporterNotification.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous).WithTarget(WatchFlagTestData.ImportApplication(this.importApplicationId));

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_PreOperationStage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PreOperation, PluginModes.Synchronous).WithTarget(WatchFlagTestData.ImportApplication(this.importApplicationId));

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_AsynchronousMode_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Asynchronous).WithTarget(WatchFlagTestData.ImportApplication(this.importApplicationId));

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_WhenAnUpdateFails_ThrowsInvalidPluginExecutionWithTheCauseAsTheInnerException()
        {
            this.ForPostOperation(PluginMessages.Update).WithTarget(WatchFlagTestData.ImportApplication(this.importApplicationId));
            this.Runtime.Service.ReturnsRecordsInSequence(defraimp_WatchFlag.EntityLogicalName, new Entity[] { WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId) });
            this.Runtime.Service.Setup(service => service.Update(It.IsAny<Entity>())).Throws(new InvalidOperationException("The update failed."));

            var exception = Assert.Throws<InvalidPluginExecutionException>(this.Execute);

            Assert.Equal("The update failed.", exception.InnerException.Message);
        }

        private PluginRuntime ForPostOperation(string messageName)
        {
            return this.Runtime.ForStep(messageName, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
        }
    }
}
