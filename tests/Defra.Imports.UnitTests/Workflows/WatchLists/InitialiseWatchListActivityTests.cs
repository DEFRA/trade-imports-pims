namespace Defra.Imports.UnitTests.Workflows.WatchLists
{
    using System;
    using System.Activities;
    using System.Linq;
    using System.Reflection;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.UnitTests.Support;
    using Defra.Imports.Workflows.WatchLists;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;
    using Moq;
    using Xunit;

    /// <summary>
    /// Tests the ILLUSTRATIVE EXAMPLE <see cref="InitialiseWatchListActivity"/> and
    /// <see cref="InitialiseWatchListHandler"/> from architecture/adr/0001-plugin-workflow-di-scaffold.md through a
    /// real workflow invoker. This is not a real business process and must not be copied as a template for entity
    /// or business choices.
    /// </summary>
    public sealed class InitialiseWatchListActivityTests : WorkflowActivityTestBase<InitialiseWatchListActivity>
    {
        private const string WatchListInput = "WatchList";

        private readonly Guid watchListId = Guid.NewGuid();
        private readonly Guid ownerId = Guid.NewGuid();

        [Fact]
        public void Activity_WatchListInput_IsARequiredWatchListReference()
        {
            var property = typeof(InitialiseWatchListActivity).GetProperty(WatchListInput);

            Assert.Equal(typeof(InArgument<EntityReference>), property.PropertyType);
            Assert.NotNull(property.GetCustomAttribute<RequiredArgumentAttribute>());
            Assert.Equal(defraimp_WatchList.EntityLogicalName, property.GetCustomAttribute<ReferenceTargetAttribute>().EntityName);
            Assert.NotNull(property.GetCustomAttribute<InputAttribute>());
        }

        [Fact]
        public void Activity_Type_DeclaresNoOutputs()
        {
            var outputs = typeof(InitialiseWatchListActivity)
                .GetProperties()
                .Where(property => property.GetCustomAttribute<OutputAttribute>() != null);

            Assert.Empty(outputs);
        }

        [Fact]
        public void Invoke_BlankWatchList_RetrievesItOnceAndSendsOneDeltaUpdate()
        {
            this.SetupStoredWatchList(WatchListTestData.CreateWatchList(this.watchListId, this.ownerId));
            Entity sent = null;
            this.Runtime.Service.Setup(service => service.Update(It.IsAny<Entity>())).Callback<Entity>(entity => sent = entity);

            this.Invoke(WatchListInput, this.WatchListReference(this.watchListId));

            this.Runtime.Service.Verify(service => service.Update(It.IsAny<Entity>()), Times.Once);
            this.Runtime.Service.VerifyQueried(defraimp_WatchList.EntityLogicalName, Times.Once());
            Assert.Equal(this.watchListId, sent.Id);
            Assert.Equal(defraimp_WatchList.EntityLogicalName, sent.LogicalName);
            Assert.Equal(
                new[] { "defraimp_comments", "defraimp_name" },
                sent.Attributes.Keys.OrderBy(key => key, StringComparer.Ordinal));
        }

        [Fact]
        public void Invoke_ValidReference_ReturnsNoOutputs()
        {
            this.SetupStoredWatchList(WatchListTestData.CreateWatchList(this.watchListId, this.ownerId));
            this.Runtime.Service.Setup(service => service.Update(It.IsAny<Entity>()));

            var output = this.Invoke(WatchListInput, this.WatchListReference(this.watchListId));

            Assert.Empty(output);
        }

        [Fact]
        public void Invoke_AlreadyInitialisedWatchList_SendsNoUpdate()
        {
            this.SetupStoredWatchList(
                WatchListTestData.CreateWatchList(this.watchListId, this.ownerId, name: "Supplied name", comments: "Supplied comments"));

            this.Invoke(WatchListInput, this.WatchListReference(this.watchListId));

            this.Runtime.Service.Verify(service => service.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void Invoke_EmptyReference_FailsWithoutCallingTheService()
        {
            this.AssertInvokeFailsWithoutCallingService<ArgumentException>(WatchListInput, this.WatchListReference(Guid.Empty));
        }

        [Fact]
        public void Invoke_ReferenceToAnotherTable_FailsWithoutCallingTheService()
        {
            this.AssertInvokeFailsWithoutCallingService<ArgumentException>(WatchListInput, new EntityReference("account", this.watchListId));
        }

        [Fact]
        public void Invoke_NoReference_FailsWithoutCallingTheService()
        {
            this.AssertInvokeFailsWithoutCallingService<ArgumentException>(WatchListInput, null);
        }

        [Fact]
        public void Invoke_WatchListNotFound_FailsWithoutUpdating()
        {
            this.Runtime.Service.ReturnsRecords(defraimp_WatchList.EntityLogicalName);

            var exception = Assert.ThrowsAny<Exception>(() => this.Invoke(WatchListInput, this.WatchListReference(this.watchListId)));

            Assert.Contains(exception.Flatten(), inner => inner is InvalidOperationException);
            this.Runtime.Service.Verify(service => service.Update(It.IsAny<Entity>()), Times.Never);
        }

        private EntityReference WatchListReference(Guid id)
        {
            return new EntityReference(defraimp_WatchList.EntityLogicalName, id);
        }

        private void SetupStoredWatchList(defraimp_WatchList stored)
        {
            this.Runtime.Service.ReturnsRecords(defraimp_WatchList.EntityLogicalName, stored);
            this.Runtime.Service.ReturnsRecords(SystemUser.EntityLogicalName, WatchListTestData.CreateOwner(this.ownerId));
        }
    }
}
