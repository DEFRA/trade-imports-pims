namespace Defra.Imports.UnitTests.Plugins.ImportApplication
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.ImportApplication;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class DetermineInspectionRequirementHandlerTests
    {
        private const string PreImageName = "PreImage";
        private const string PostImageName = "PostImage";
        private const string OtherEntity = "account";
        private const string UnsupportedMessage = "Create";
        private const string DeterminerFailure = "The determiner failed.";
        private const int AsynchronousMode = 1;
        private const int PreValidationStage = 10;

        private readonly Guid importApplicationId = Guid.NewGuid();
        private readonly PluginRuntime runtime = new PluginRuntime();
        private readonly Mock<IInspectionRequirementDeterminer> determiner = new Mock<IInspectionRequirementDeterminer>(MockBehavior.Strict);
        private readonly Mock<ILogWriter> logger = new Mock<ILogWriter>(MockBehavior.Strict);
        private readonly List<string> logged = new List<string>();
        private readonly DetermineInspectionRequirementHandler handler;
        private int determinations;
        private InspectionImportApplicationContext before;
        private InspectionImportApplicationContext after;

        public DetermineInspectionRequirementHandlerTests()
        {
            this.determiner
                .Setup(value => value.Determine(It.IsAny<InspectionImportApplicationContext>(), It.IsAny<InspectionImportApplicationContext>()))
                .Callback<InspectionImportApplicationContext, InspectionImportApplicationContext>((determinedBefore, determinedAfter) =>
                {
                    this.determinations++;
                    this.before = determinedBefore;
                    this.after = determinedAfter;
                });
            this.logger
                .Setup(value => value.Log(It.IsAny<Severity>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<Severity, string, string>((severity, tag, message) => this.logged.Add(tag + ": " + severity + ": " + message));
            this.handler = new DetermineInspectionRequirementHandler(this.determiner.Object, this.runtime.CreateExecution(), this.logger.Object);
        }

        [Fact]
        public void Constructor_NullDeterminer_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DetermineInspectionRequirementHandler(null, this.runtime.CreateExecution(), this.logger.Object));
        }

        [Fact]
        public void Constructor_NullExecution_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DetermineInspectionRequirementHandler(this.determiner.Object, null, this.logger.Object));
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DetermineInspectionRequirementHandler(this.determiner.Object, this.runtime.CreateExecution(), null));
        }

        [Fact]
        public void Execute_Update_DeterminesWithTheProjectedImagesAndLogsInOrder()
        {
            this.ForValidUpdate();

            this.handler.Execute();

            Assert.Equal(1, this.determinations);
            Assert.Equal(this.importApplicationId, this.before.Id);
            Assert.Equal(true, this.before.ImportRecordCounted);
            Assert.Equal(this.importApplicationId, this.after.Id);
            Assert.Equal(false, this.after.ImportRecordCounted);
            Assert.Equal(
                new[]
                {
                    InspectionConstants.LogTag + ": Info: Fired plugin with message: Update. Depth = 0",
                    InspectionConstants.LogTag + ": Info: Plugin executing",
                },
                this.logged);
        }

        [Fact]
        public void Execute_SetState_DeterminesWithTheProjectedImagesAndLogsInOrder()
        {
            this.ForValidSetState();

            this.handler.Execute();

            Assert.Equal(1, this.determinations);
            Assert.Equal(this.importApplicationId, this.before.Id);
            Assert.Equal(defraimp_importapplication_statecode.Inactive, this.before.StateCode);
            Assert.Equal(defraimp_importapplication_statecode.Active, this.after.StateCode);
            Assert.Equal(
                new[]
                {
                    InspectionConstants.LogTag + ": Info: Fired plugin with message: SetState. Depth = 0",
                    InspectionConstants.LogTag + ": Info: Plugin executing",
                },
                this.logged);
        }

        [Fact]
        public void Execute_Delete_DeterminesWithTheBeforeImageAndNoAfterImage()
        {
            this.ForValidDelete();

            this.handler.Execute();

            Assert.Equal(1, this.determinations);
            Assert.Equal(this.importApplicationId, this.before.Id);
            Assert.Null(this.after);
            Assert.Equal(
                new[]
                {
                    InspectionConstants.LogTag + ": Info: Fired plugin with message: Delete. Depth = 0",
                    InspectionConstants.LogTag + ": Info: Plugin executing",
                },
                this.logged);
        }

        [Theory]
        [InlineData("update")]
        [InlineData("SETSTATE")]
        [InlineData("delete")]
        public void Execute_MessageInAnyCase_IsAccepted(string messageName)
        {
            this.ForValidStep(messageName);

            this.handler.Execute();

            Assert.Equal(1, this.determinations);
            Assert.Equal(
                InspectionConstants.LogTag + ": Info: Fired plugin with message: " + messageName + ". Depth = 0",
                this.logged[0]);
        }

        [Fact]
        public void Execute_WithDepthTwo_LogsTheDepth()
        {
            this.runtime.Context.SetupGet(context => context.Depth).Returns(2);
            this.ForValidUpdate();

            this.handler.Execute();

            Assert.Equal(InspectionConstants.LogTag + ": Info: Fired plugin with message: Update. Depth = 2", this.logged[0]);
        }

        [Fact]
        public void Execute_DeterminerFails_PropagatesTheFailureUnchanged()
        {
            this.determiner
                .Setup(value => value.Determine(It.IsAny<InspectionImportApplicationContext>(), It.IsAny<InspectionImportApplicationContext>()))
                .Throws(new InvalidOperationException(DeterminerFailure));
            this.ForValidUpdate();

            var exception = Assert.Throws<InvalidOperationException>(() => this.handler.Execute());

            Assert.Equal(DeterminerFailure, exception.Message);
            Assert.Equal(2, this.logged.Count);
        }

        [Fact]
        public void Execute_WrongPrimaryEntity_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidUpdate();
            this.runtime.ForStep(PluginMessages.Update, OtherEntity, PluginStages.PostOperation, PluginModes.Synchronous);

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_AsynchronousMode_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidUpdate();
            this.runtime.ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, AsynchronousMode);

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_UnsupportedMessage_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidUpdate();
            this.runtime.ForStep(UnsupportedMessage, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_NoPreImage_ThrowsWithoutDeterminingOrLogging()
        {
            this.runtime
                .ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous)
                .WithTarget(this.Application())
                .WithPostImage(PostImageName, this.Application());

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_UpdateAtTheWrongStage_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidUpdate();
            this.runtime.ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PreValidationStage, PluginModes.Synchronous);

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_UpdateWithoutATarget_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidUpdate();
            this.runtime.WithoutTarget();

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_UpdateWhoseTargetIsNotAnEntity_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidUpdate();
            this.runtime.InputParameters[PluginInputParameters.Target] = new EntityReference(defraimp_importapplication.EntityLogicalName, this.importApplicationId);

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_UpdateWithoutAPostImage_ThrowsWithoutDeterminingOrLogging()
        {
            this.runtime
                .ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous)
                .WithTarget(this.Application())
                .WithPreImage(PreImageName, this.Application());

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_SetStateAtTheWrongStage_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidSetState();
            this.runtime.ForStep(PluginMessages.SetState, defraimp_importapplication.EntityLogicalName, PluginStages.PreOperation, PluginModes.Synchronous);

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_SetStateWithoutAnEntityMoniker_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidSetState();
            this.runtime.InputParameters.Remove(PluginInputParameters.EntityMoniker);

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_SetStateWhoseEntityMonikerIsNotAReference_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidSetState();
            this.runtime.InputParameters[PluginInputParameters.EntityMoniker] = this.Application();

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_SetStateWithoutAPostImage_ThrowsWithoutDeterminingOrLogging()
        {
            this.runtime
                .ForStep(PluginMessages.SetState, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous)
                .WithPreImage(PreImageName, this.Application());
            this.runtime.InputParameters[PluginInputParameters.EntityMoniker] = this.Reference();

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_DeleteAtTheWrongStage_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidDelete();
            this.runtime.ForStep(PluginMessages.Delete, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_DeleteWithoutATarget_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidDelete();
            this.runtime.WithoutTarget();

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        [Fact]
        public void Execute_DeleteWhoseTargetIsAnEntity_ThrowsWithoutDeterminingOrLogging()
        {
            this.ForValidDelete();
            this.runtime.WithTarget(this.Application());

            this.AssertRejectedWithoutDeterminingOrLogging();
        }

        private void AssertRejectedWithoutDeterminingOrLogging()
        {
            Assert.Throws<InvalidOperationException>(() => this.handler.Execute());
            this.determiner.VerifyNoOtherCalls();
            this.logger.VerifyNoOtherCalls();
        }

        private void ForValidStep(string messageName)
        {
            if (string.Equals(messageName, PluginMessages.Delete, StringComparison.OrdinalIgnoreCase))
            {
                this.ForValidDelete();
                this.runtime.ForStep(messageName, defraimp_importapplication.EntityLogicalName, PluginStages.PreOperation, PluginModes.Synchronous);
            }
            else if (string.Equals(messageName, PluginMessages.SetState, StringComparison.OrdinalIgnoreCase))
            {
                this.ForValidSetState();
                this.runtime.ForStep(messageName, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
            }
            else
            {
                this.ForValidUpdate();
                this.runtime.ForStep(messageName, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
            }
        }

        private void ForValidUpdate()
        {
            this.runtime
                .ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous)
                .WithTarget(this.Application())
                .WithPreImage(PreImageName, this.Image(true, defraimp_importapplication_statecode.Active))
                .WithPostImage(PostImageName, this.Image(false, defraimp_importapplication_statecode.Active));
        }

        private void ForValidSetState()
        {
            this.runtime
                .ForStep(PluginMessages.SetState, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous)
                .WithPreImage(PreImageName, this.Image(false, defraimp_importapplication_statecode.Inactive))
                .WithPostImage(PostImageName, this.Image(false, defraimp_importapplication_statecode.Active));
            this.runtime.InputParameters[PluginInputParameters.EntityMoniker] = this.Reference();
        }

        private void ForValidDelete()
        {
            this.runtime
                .ForStep(PluginMessages.Delete, defraimp_importapplication.EntityLogicalName, PluginStages.PreOperation, PluginModes.Synchronous)
                .WithPreImage(PreImageName, this.Image(false, defraimp_importapplication_statecode.Active));
            this.runtime.InputParameters[PluginInputParameters.Target] = this.Reference();
        }

        private defraimp_importapplication Application()
        {
            return new defraimp_importapplication { Id = this.importApplicationId };
        }

        private defraimp_importapplication Image(bool? counted, defraimp_importapplication_statecode state)
        {
            return new defraimp_importapplication
            {
                Id = this.importApplicationId,
                defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
                defraimp_ImportRecordCounted = counted,
                statecode = state,
            };
        }

        private EntityReference Reference()
        {
            return new EntityReference(defraimp_importapplication.EntityLogicalName, this.importApplicationId);
        }
    }
}
