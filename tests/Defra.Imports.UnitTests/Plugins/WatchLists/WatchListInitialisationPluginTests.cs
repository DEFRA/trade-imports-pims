namespace Defra.Imports.UnitTests.Plugins.WatchLists
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.WatchLists;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    /// <summary>
    /// Tests the ILLUSTRATIVE EXAMPLE <see cref="WatchListInitialisationPlugin"/> from
    /// architecture/adr/0001-plugin-workflow-di-scaffold.md end to end through the composition root. This is not a
    /// real business process and must not be copied as a template for entity or business choices.
    /// </summary>
    public sealed class WatchListInitialisationPluginTests : PluginTestBase<WatchListInitialisationPlugin>
    {
        private readonly Guid ownerId = Guid.NewGuid();
        private readonly Guid transporterId = Guid.NewGuid();

        [Fact]
        public void Plugin_Instance_IsAHandlerFirstPluginBase()
        {
            Assert.IsAssignableFrom<PluginBase<WatchListInitialisationHandler>>(this.Plugin);
        }

        [Fact]
        public void Execute_PreOperationCreate_AppliesTheDefaultsToTheTarget()
        {
            var target = this.CreateTarget();
            this.Runtime.ForPreOperationCreate(target);
            this.SetupOwnerAndTransporter();

            this.Execute();

            Assert.Equal(WatchListTestData.TransporterWatchListName, target["defraimp_name"]);
            Assert.Equal(WatchListTestData.OwnerComments, target["defraimp_comments"]);
        }

        [Fact]
        public void Execute_PreOperationCreate_AddsNoIdentifierAttributeToTheTarget()
        {
            var target = this.CreateTarget();
            this.Runtime.ForPreOperationCreate(target);
            this.SetupOwnerAndTransporter();

            this.Execute();

            Assert.False(target.Contains(defraimp_WatchList.Fields.defraimp_WatchListId));
            Assert.Equal(Guid.Empty, target.Id);
        }

        [Fact]
        public void Execute_TargetWithSuppliedValues_PreservesThem()
        {
            var target = this.CreateTarget();
            target["defraimp_name"] = "Supplied name";
            target["defraimp_comments"] = "Supplied comments";
            this.Runtime.ForPreOperationCreate(target);
            this.SetupOwnerAndTransporter();

            this.Execute();

            Assert.Equal("Supplied name", target["defraimp_name"]);
            Assert.Equal("Supplied comments", target["defraimp_comments"]);
        }

        [Fact]
        public void Execute_PreOperationCreate_ReadsTheOwnerAndTransporterButNeverTheWatchListAndNeverWrites()
        {
            this.Runtime.ForPreOperationCreate(this.CreateTarget());
            this.SetupOwnerAndTransporter();

            this.Execute();

            this.Runtime.Service.VerifyQueriedAny(Times.Exactly(2));
            this.Runtime.Service.VerifyQueried(defraimp_WatchList.EntityLogicalName, Times.Never());
            this.Runtime.Service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
            this.Runtime.Service.Verify(value => value.Create(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void Execute_UnsupportedMessage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForPreOperationCreate(this.CreateTarget()).ForStep("Update", defraimp_WatchList.EntityLogicalName, 20, 0);

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_PostOperationStage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForPreOperationCreate(this.CreateTarget()).ForStep("Create", defraimp_WatchList.EntityLogicalName, 40, 0);

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_NoTarget_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForPreOperationCreate(this.CreateTarget()).WithoutTarget();

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_TargetWithoutAnOwner_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            var target = new Entity(defraimp_WatchList.EntityLogicalName);
            this.Runtime.ForPreOperationCreate(target);

            this.AssertExecuteFailsWithoutCallingService();
            Assert.Empty(target.Attributes);
        }

        private Entity CreateTarget()
        {
            return new Entity(defraimp_WatchList.EntityLogicalName)
            {
                ["ownerid"] = new EntityReference(SystemUser.EntityLogicalName, this.ownerId),
                ["defraimp_transporterid"] = new EntityReference(defraimp_Transporter.EntityLogicalName, this.transporterId),
            };
        }

        private void SetupOwnerAndTransporter()
        {
            this.Runtime.Service.ReturnsRecords(SystemUser.EntityLogicalName, WatchListTestData.CreateOwner(this.ownerId));
            this.Runtime.Service.ReturnsRecords(defraimp_Transporter.EntityLogicalName, WatchListTestData.CreateTransporter(this.transporterId));
        }
    }
}
