namespace Defra.Imports.UnitTests.Plugins.ParameterSetKeyDataPair
{
    using System;
    using Defra.Imports.Application.CommodityComplements;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.ParameterSetKeyDataPair;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class PopulateAnimalNumberHandlerTests
    {
        private const string PreImageName = "PreImage";
        private const string OtherPreImageName = "SomeOtherName";
        private const string UnmappedKey = "some_other_key";
        private const string UnsupportedMessage = "Delete";
        private const string OtherEntity = "account";

        private readonly Guid importerNotificationId = Guid.NewGuid();
        private readonly PluginRuntime runtime = new PluginRuntime();
        private readonly Mock<ICommodityComplementKeyDataApplier> applier = new Mock<ICommodityComplementKeyDataApplier>(MockBehavior.Strict);
        private readonly Mock<ILogWriter> logger = new Mock<ILogWriter>(MockBehavior.Strict);
        private readonly PopulateAnimalNumberHandler handler;
        private ApplyCommodityComplementKeyDataContext received;

        public PopulateAnimalNumberHandlerTests()
        {
            this.handler = new PopulateAnimalNumberHandler(this.applier.Object, this.runtime.CreateExecution(), this.logger.Object);
            this.applier
                .Setup(value => value.Apply(It.IsAny<ApplyCommodityComplementKeyDataContext>()))
                .Callback<ApplyCommodityComplementKeyDataContext>(context => this.received = context);
            this.logger.Setup(value => value.Log(It.IsAny<Severity>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void Constructor_NullApplier_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PopulateAnimalNumberHandler(null, this.runtime.CreateExecution(), this.logger.Object));
        }

        [Fact]
        public void Constructor_NullExecution_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PopulateAnimalNumberHandler(this.applier.Object, null, this.logger.Object));
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PopulateAnimalNumberHandler(this.applier.Object, this.runtime.CreateExecution(), null));
        }

        [Fact]
        public void Execute_Create_AppliesTheTarget()
        {
            var target = this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7");
            this.ForPostOperation(PluginMessages.Create).WithTarget(target);

            this.handler.Execute();

            Assert.Equal(KeyDataPairKeys.NumberOfAnimals, this.received.Key);
            Assert.Equal("7", this.received.Data);
            Assert.Equal(this.importerNotificationId, this.received.ImporterNotificationId);
            this.logger.VerifyNoOtherCalls();
        }

        [Fact]
        public void Execute_Create_DoesNotReadThePreImage()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7"));
            this.runtime.WithPreImage(PreImageName, this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "1"));

            this.handler.Execute();

            Assert.Equal("7", this.received.Data);
        }

        [Fact]
        public void Execute_Create_MatchesTheMessageNameIgnoringCase()
        {
            this.ForPostOperation(PluginMessages.Create.ToLowerInvariant()).WithTarget(this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7"));

            this.handler.Execute();

            Assert.Equal("7", this.received.Data);
        }

        [Fact]
        public void Execute_Create_LeavesTheDecisionOnKeysToTheApplier()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(this.CreateKeyDataPair(UnmappedKey, "7"));

            this.handler.Execute();

            Assert.Equal(UnmappedKey, this.received.Key);
        }

        [Fact]
        public void Execute_Update_UsesThePreImageWithTheTargetsDataOverlaid()
        {
            var target = new defraimp_parametersetkeydatapair { defraimp_data = "15" };
            var preImage = this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "8");
            this.ForPostOperation(PluginMessages.Update).WithTarget(target).WithPreImage(PreImageName, preImage);

            this.handler.Execute();

            Assert.Equal(KeyDataPairKeys.NumberOfAnimals, this.received.Key);
            Assert.Equal("15", this.received.Data);
            Assert.Equal(this.importerNotificationId, this.received.ImporterNotificationId);
            this.logger.VerifyNoOtherCalls();
        }

        [Fact]
        public void Execute_Update_DoesNotMutateTheTargetOrThePreImage()
        {
            var target = new defraimp_parametersetkeydatapair { defraimp_data = "15" };
            var preImage = this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "8");
            this.ForPostOperation(PluginMessages.Update).WithTarget(target).WithPreImage(PreImageName, preImage);

            this.handler.Execute();

            Assert.Equal("8", preImage.defraimp_data);
            Assert.Equal(1, target.Attributes.Count);
            Assert.Equal("15", target.defraimp_data);
        }

        [Fact]
        public void Execute_Update_UsesTheFirstPreImageWhateverItsName()
        {
            var target = new defraimp_parametersetkeydatapair { defraimp_data = "15" };
            this.ForPostOperation(PluginMessages.Update).WithTarget(target).WithPreImage(OtherPreImageName, this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "8"));

            this.handler.Execute();

            Assert.Equal(KeyDataPairKeys.NumberOfAnimals, this.received.Key);
            Assert.Equal("15", this.received.Data);
        }

        [Fact]
        public void Execute_UpdateWhoseTargetHasNoData_OverlaysNoData()
        {
            this.ForPostOperation(PluginMessages.Update)
                .WithTarget(new defraimp_parametersetkeydatapair { defraimp_name = KeyDataPairKeys.NumberOfAnimals })
                .WithPreImage(PreImageName, this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "8"));

            this.handler.Execute();

            Assert.Null(this.received.Data);
        }

        [Fact]
        public void Execute_UpdateWithoutAPreImage_SkipsSilently()
        {
            this.ForPostOperation(PluginMessages.Update).WithTarget(new defraimp_parametersetkeydatapair { defraimp_data = "15" });

            this.handler.Execute();

            this.applier.VerifyNoOtherCalls();
            this.logger.VerifyNoOtherCalls();
        }

        [Fact]
        public void Execute_ApplierFails_TracesTheErrorWithoutThrowing()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7"));
            this.applier
                .Setup(value => value.Apply(It.IsAny<ApplyCommodityComplementKeyDataContext>()))
                .Throws(new InvalidOperationException("The commodity complement could not be updated."));

            var exception = Record.Exception(() => this.handler.Execute());

            Assert.Null(exception);
            this.logger.Verify(
                value => value.Log(
                    Severity.Error,
                    nameof(PopulateAnimalNumber),
                    It.Is<string>(message => message.StartsWith("The commodity complement could not be updated." + Environment.NewLine, StringComparison.Ordinal))),
                Times.Once);
        }

        [Fact]
        public void Execute_ApplierFails_TracesTheStackTrace()
        {
            this.ForPostOperation(PluginMessages.Create).WithTarget(this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7"));
            this.applier
                .Setup(value => value.Apply(It.IsAny<ApplyCommodityComplementKeyDataContext>()))
                .Callback<ApplyCommodityComplementKeyDataContext>(context => throw new InvalidOperationException("Failed."));

            this.handler.Execute();

            this.logger.Verify(
                value => value.Log(Severity.Error, It.IsAny<string>(), It.Is<string>(message => message.Contains(nameof(PopulateAnimalNumberHandlerTests)))),
                Times.Once);
        }

        [Theory]
        [InlineData(UnsupportedMessage, defraimp_parametersetkeydatapair.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous)]
        [InlineData(PluginMessages.Create, OtherEntity, PluginStages.PostOperation, PluginModes.Synchronous)]
        [InlineData(PluginMessages.Create, defraimp_parametersetkeydatapair.EntityLogicalName, PluginStages.PreOperation, PluginModes.Synchronous)]
        [InlineData(PluginMessages.Update, defraimp_parametersetkeydatapair.EntityLogicalName, PluginStages.PreValidation, PluginModes.Synchronous)]
        [InlineData(PluginMessages.Create, defraimp_parametersetkeydatapair.EntityLogicalName, PluginStages.PostOperation, PluginModes.Asynchronous)]
        public void Execute_UnsupportedStep_ThrowsInvalidOperationWithoutCallingTheApplier(
            string messageName, string entityName, int stage, int mode)
        {
            this.runtime.ForStep(messageName, entityName, stage, mode).WithTarget(this.CreateKeyDataPair(KeyDataPairKeys.NumberOfAnimals, "7"));

            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            this.applier.VerifyNoOtherCalls();
            this.logger.VerifyNoOtherCalls();
        }

        [Fact]
        public void Execute_NoTarget_ThrowsInvalidOperationWithoutCallingTheApplier()
        {
            this.ForPostOperation(PluginMessages.Create).WithoutTarget();

            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            this.applier.VerifyNoOtherCalls();
            this.logger.VerifyNoOtherCalls();
        }

        [Fact]
        public void Execute_TargetIsNotAnEntity_ThrowsInvalidOperationWithoutCallingTheApplier()
        {
            this.ForPostOperation(PluginMessages.Create).InputParameters[PluginInputParameters.Target] = new EntityReference(defraimp_parametersetkeydatapair.EntityLogicalName, Guid.NewGuid());

            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            this.applier.VerifyNoOtherCalls();
            this.logger.VerifyNoOtherCalls();
        }

        private PluginRuntime ForPostOperation(string messageName)
        {
            return this.runtime.ForStep(messageName, defraimp_parametersetkeydatapair.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
        }

        private defraimp_parametersetkeydatapair CreateKeyDataPair(string name, string data)
        {
            return new defraimp_parametersetkeydatapair
            {
                defraimp_name = name,
                defraimp_data = data,
                defraimp_ImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, this.importerNotificationId),
                defraimp_CommodityComplementParameterSet = new EntityReference(defraimp_commoditycomplementparameterset.EntityLogicalName, Guid.NewGuid()) { Name = "42" },
            };
        }
    }
}
