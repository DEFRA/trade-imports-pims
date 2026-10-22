namespace Defra.Imports.UnitTests.Plugins.Itahc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.Itahc;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class FlagRecordOnWatchListTests : PluginTestBase<FlagRecordOnWatchList>
    {
        private const string UnsupportedMessage = "Update";

        private readonly Guid itahcId = Guid.NewGuid();
        private readonly Guid consigneeId = Guid.NewGuid();
        private readonly Guid watchListId = Guid.NewGuid();
        private readonly List<Entity> created = new List<Entity>();

        public FlagRecordOnWatchListTests()
        {
            this.Runtime.Service
                .Setup(service => service.Create(It.IsAny<Entity>()))
                .Callback<Entity>(entity => this.created.Add(entity))
                .Returns(Guid.NewGuid());
        }

        [Fact]
        public void Plugin_Instance_IsAHandlerFirstPluginBase()
        {
            Assert.IsAssignableFrom<PluginBase<FlagRecordOnWatchListHandler>>(this.Plugin);
        }

        [Fact]
        public void Execute_OnCreateNamingAConsigneeOnAWatchList_CreatesAWatchFlag()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.Itahc(this.itahcId, name: "ITAHC", consignee: "Acme"));
            this.SetupConsigneeOnWatchList();

            this.Execute();

            var flag = Assert.IsType<defraimp_WatchFlag>(Assert.Single(this.created));
            Assert.Equal(this.itahcId, flag.defraimp_ItahcId.Id);
            Assert.Equal(this.watchListId, flag.defraimp_WatchListId.Id);
            Assert.Equal("Watch list - ITAHC", flag.defraimp_Name);
        }

        [Fact]
        public void Execute_OnCreateNamingNoOperators_ReadsTheWatchListsAndCreatesNothing()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.Itahc(this.itahcId));
            this.Runtime.Service.ReturnsRecords(defraimp_WatchList.EntityLogicalName);

            this.Execute();

            this.Runtime.Service.VerifyQueriedAny(Times.Once());
            Assert.Empty(this.created);
        }

        [Fact]
        public void Execute_OnCreateNamingAnOperatorNotOnAWatchList_CreatesNothing()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.Itahc(this.itahcId, consignee: "Acme"));
            this.Runtime.Service.ReturnsRecords(defraimp_WatchList.EntityLogicalName);

            this.Execute();

            Assert.Empty(this.created);
        }

        [Fact]
        public void Execute_WhenTheTargetIsMissing_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForPostOperation(PluginMessages.Create).WithoutTarget();

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_UnsupportedMessage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForPostOperation(UnsupportedMessage).WithTarget(WatchFlagTestData.Itahc(this.itahcId));

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_PreOperationStage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForStep(PluginMessages.Create, defraimp_itahc.EntityLogicalName, PluginStages.PreOperation, PluginModes.Synchronous).WithTarget(WatchFlagTestData.Itahc(this.itahcId));

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_WhenTheCreateFails_ThrowsInvalidPluginExecutionWithTheCauseAsTheInnerException()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(WatchFlagTestData.Itahc(this.itahcId, consignee: "Acme"));
            this.SetupConsigneeOnWatchList();
            this.Runtime.Service.Setup(service => service.Create(It.IsAny<Entity>())).Throws(new InvalidOperationException("The create failed."));

            var exception = Assert.Throws<InvalidPluginExecutionException>(this.Execute);

            Assert.Equal("The create failed.", exception.InnerException.Message);
        }

        private PluginRuntime ForPostOperation(string messageName)
        {
            return this.Runtime.ForStep(messageName, defraimp_itahc.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
        }

        private void SetupConsigneeOnWatchList()
        {
            this.Runtime.Service.ReturnsRecords(
                defraimp_WatchList.EntityLogicalName,
                new defraimp_WatchList
                {
                    Id = this.watchListId,
                    defraimp_Name = "Watch list",
                    defraimp_WatchType = defraimp_watchtype.Consignee,
                    defraimp_ConsigneeId = new EntityReference(defraimp_Consignee.EntityLogicalName, this.consigneeId),
                });
            this.Runtime.Service.ReturnsRecords(defraimp_Consignee.EntityLogicalName, new defraimp_Consignee { Id = this.consigneeId });
        }
    }
}
