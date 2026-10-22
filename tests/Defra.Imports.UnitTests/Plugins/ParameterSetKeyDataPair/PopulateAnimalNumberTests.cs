namespace Defra.Imports.UnitTests.Plugins.ParameterSetKeyDataPair
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Application.CommodityComplements;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.ParameterSetKeyDataPair;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    public sealed class PopulateAnimalNumberTests : PluginTestBase<PopulateAnimalNumber>
    {
        private const string PreImageName = "PreImage";
        private const string UnmappedKey = "some_other_key";
        private const string UnsupportedMessage = "Delete";

        private readonly Guid importerNotificationId = Guid.NewGuid();
        private readonly Guid parameterSetId = Guid.NewGuid();
        private readonly Guid commodityComplementId = Guid.NewGuid();
        private Entity updated;

        public PopulateAnimalNumberTests()
        {
            this.Runtime.Service
                .Setup(service => service.Update(It.IsAny<Entity>()))
                .Callback<Entity>(entity => this.updated = entity);
        }

        [Fact]
        public void Plugin_Instance_IsAHandlerFirstPluginBase()
        {
            Assert.IsAssignableFrom<PluginBase<PopulateAnimalNumberHandler>>(this.Plugin);
        }

        [Fact]
        public void Execute_OnCreateWithNumberAnimalKey_UpdatesCommodityComplement()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7", "42"));
            this.SetupCommodityComplement();

            this.Execute();

            var update = Assert.IsType<defraimp_commoditycomplement>(this.updated);
            Assert.Equal(this.commodityComplementId, update.Id);
            Assert.Equal("7", update.defraimp_NumberofAnimals);
            this.Runtime.Service.Verify(service => service.Update(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public void Execute_OnCreateWithoutAParameterSetName_ReadsTheNameBeforeUpdating()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7", null));
            this.Runtime.Service.ReturnsRecords(
                defraimp_commoditycomplementparameterset.EntityLogicalName,
                new defraimp_commoditycomplementparameterset { Id = this.parameterSetId, defraimp_name = "42" });
            this.SetupCommodityComplement();

            this.Execute();

            Assert.Equal(this.commodityComplementId, this.updated.Id);
            this.Runtime.Service.VerifyQueried(defraimp_commoditycomplementparameterset.EntityLogicalName, Times.Once());
        }

        [Fact]
        public void Execute_OnCreateWithDifferentKey_MakesNoServiceCalls()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(this.CreateKeyDataPair(UnmappedKey, "7", "42"));

            this.Execute();

            this.Runtime.Service.VerifyNoOtherCalls();
        }

        [Fact]
        public void Execute_OnUpdateWithNumberAnimalKey_UsesPreImageAndUpdatesCommodityComplement()
        {
            var target = new defraimp_parametersetkeydatapair { defraimp_data = "15" };
            this.ForPostOperation(PluginMessages.Update).WithTarget(target).WithPreImage(PreImageName, this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, null, "99"));
            this.SetupCommodityComplement();

            this.Execute();

            Assert.Equal(this.commodityComplementId, this.updated.Id);
            Assert.Equal("15", ((defraimp_commoditycomplement)this.updated).defraimp_NumberofAnimals);
        }

        [Fact]
        public void Execute_OnUpdateWithNumberAnimalKey_MergesPreImageWithTargetDataAndUpdatesCommodityComplement()
        {
            var target = new defraimp_parametersetkeydatapair { defraimp_name = KeyDataPairKeys.NumberOfAnimals, defraimp_data = "15" };

            // The pre-image data is deliberately different, to prove the Target's value takes precedence.
            this.ForPostOperation(PluginMessages.Update).WithTarget(target).WithPreImage(PreImageName, this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "8", "99"));
            var recorded = this.SetupCommodityComplement();

            this.Execute();

            Assert.Equal("15", ((defraimp_commoditycomplement)this.updated).defraimp_NumberofAnimals);
            Assert.Contains(
                Assert.Single(recorded).GetAllConditions(),
                condition => condition.AttributeName == defraimp_commoditycomplement.Fields.defraimp_complementid && Equals(condition.Values.Single(), 99));
        }

        [Fact]
        public void Execute_OnUpdateWithNoPreImage_MakesNoServiceCallsAndDoesNotThrow()
        {
            this.ForPostOperation(PluginMessages.Update).WithTarget(new defraimp_parametersetkeydatapair { defraimp_name = KeyDataPairKeys.NumberOfAnimals, defraimp_data = "15" });

            this.Execute();

            this.Runtime.Service.VerifyNoOtherCalls();
        }

        [Fact]
        public void Execute_NoMatchingCommodityComplement_TracesATimestampedWarningAndUpdatesNothing()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7", "42"));
            this.Runtime.Service.ReturnsRecords(defraimp_commoditycomplement.EntityLogicalName);

            this.Execute();

            this.Runtime.Service.Verify(service => service.Update(It.IsAny<Entity>()), Times.Never);
            var warning = Assert.Single(this.Runtime.Tracing.Messages, message => message.Contains($": {nameof(CommodityComplementKeyDataApplier)}: Warning: Cannot apply key data pair '{KeyDataPairKeys.NumberOfAnimals}' - no matching commodity complement found"));
            Assert.Matches(@"^\d{4}-\d{2}-\d{2}T", warning);
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
            this.ForPostOperation(UnsupportedMessage).WithTarget(this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7", "42"));

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_WhenTheUpdateFails_TracesTheErrorAndDoesNotThrow()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7", "42"));
            this.SetupCommodityComplement();
            this.Runtime.Service.Setup(service => service.Update(It.IsAny<Entity>())).Throws(new InvalidOperationException("The update failed."));

            this.Execute();

            Assert.Single(this.Runtime.Tracing.Messages, message => message.Contains($": {nameof(PopulateAnimalNumber)}: Error: The update failed."));
        }

        private PluginRuntime ForPostOperation(string messageName)
        {
            return this.Runtime.ForStep(messageName, defraimp_parametersetkeydatapair.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
        }

        private defraimp_parametersetkeydatapair CreateKeyDataPair(string name, string data, string complementSetName)
        {
            var keyDataPair = new defraimp_parametersetkeydatapair
            {
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, this.importerNotificationId),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, this.parameterSetId) { Name = complementSetName },
            };

            if (name != null)
            {
                keyDataPair.defraimp_name = name;
            }

            if (data != null)
            {
                keyDataPair.defraimp_data = data;
            }

            return keyDataPair;
        }

        private IList<QueryExpression> SetupCommodityComplement()
        {
            return this.Runtime.Service.ReturnsRecords(
                defraimp_commoditycomplement.EntityLogicalName,
                new defraimp_commoditycomplement { Id = this.commodityComplementId });
        }
    }
}
