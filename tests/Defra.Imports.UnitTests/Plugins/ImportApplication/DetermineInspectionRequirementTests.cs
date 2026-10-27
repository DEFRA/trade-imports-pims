namespace Defra.Imports.UnitTests.Plugins.ImportApplication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.ImportApplication;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class DetermineInspectionRequirementTests : PluginTestBase<DetermineInspectionRequirement>
    {
        private const string PreImageName = "PreImage";
        private const string PostImageName = "PostImage";
        private const string OtherEntity = "account";
        private const string UnsupportedMessage = "Create";
        private const int AsynchronousMode = 1;
        private const int PreValidationStage = 10;

        private readonly Guid importApplicationId = Guid.NewGuid();
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly defraimp_importrisklevel p1 = InspectionTestData.RiskLevel("P1");

        public DetermineInspectionRequirementTests()
        {
            this.dataverse.Seed(this.p1);
            this.Runtime.Factory
                .Setup(factory => factory.CreateOrganizationService(this.Runtime.UserId))
                .Returns(this.dataverse.Service.Object);
        }

        [Fact]
        public void Plugin_Instance_IsAHandlerFirstPluginBase()
        {
            Assert.IsAssignableFrom<PluginBase<DetermineInspectionRequirementHandler>>(this.Plugin);
        }

        [Fact]
        public void Execute_UpdateWithAnUnchangedP1RiskLevel_RecordsTheRiskLevelAsUnknownAndReadsTheRiskLevel()
        {
            var riskLevel = InspectionTestData.RiskLevelReference(this.p1);
            this.ForUpdate();
            this.Runtime.WithPreImage(PreImageName, this.StoredApplication(riskLevel, counted: false));
            this.Runtime.WithPostImage(PostImageName, this.StoredApplication(riskLevel, counted: false));

            this.Execute();

            Assert.Empty(this.dataverse.Creates);
            var update = Assert.Single(this.dataverse.Updates).ToEntity<defraimp_importapplication>();
            Assert.Equal(defraimp_importapplication.EntityLogicalName, update.LogicalName);
            Assert.Equal(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, update.defraimp_InspectionRequired);
            Assert.Equal(defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown, update.defraimp_InspectionRequiredReason);
            Assert.Contains("Query:" + defraimp_importrisklevel.EntityLogicalName, this.dataverse.Events);
        }

        [Fact]
        public void Execute_SetStateReactivatingACancelledApplication_DeterminesTheInspectionAgain()
        {
            var riskLevel = InspectionTestData.RiskLevelReference(this.p1);
            this.ForSetState();
            this.Runtime.WithPreImage(
                PreImageName,
                this.StoredApplication(riskLevel, counted: false, state: defraimp_importapplication_statecode.Inactive, status: defraimp_importapplication_statuscode.Cancelled));
            this.Runtime.WithPostImage(PostImageName, this.StoredApplication(riskLevel, counted: false));

            this.Execute();

            Assert.Empty(this.dataverse.Creates);
            var update = Assert.Single(this.dataverse.Updates).ToEntity<defraimp_importapplication>();
            Assert.Equal(defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown, update.defraimp_InspectionRequiredReason);
        }

        [Fact]
        public void Execute_DeleteOfACountedP1Application_DecrementsTheCountersAndRecordsTheHistory()
        {
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 1);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);
            this.ForDelete();
            this.Runtime.WithPreImage(
                PreImageName,
                this.StoredApplication(InspectionTestData.RiskLevelReference(this.p1), counted: true));

            this.Execute();

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.Equal(2, this.CreatedHistoryRecords().Count);
            var update = this.dataverse.UpdatesOf(defraimp_importapplication.EntityLogicalName)[0].ToEntity<defraimp_importapplication>();
            Assert.False(update.defraimp_ImportRecordCounted);
        }

        [Fact]
        public void Execute_DeleteOfAnApplicationWithoutARiskLevel_FailsAsAnUnexpectedFailure()
        {
            this.ForDelete();
            this.Runtime.WithPreImage(PreImageName, this.StoredApplication(riskLevel: null, counted: false));

            var exception = Assert.Throws<InvalidPluginExecutionException>(this.Execute);

            Assert.IsType<NullReferenceException>(exception.InnerException);
        }

        [Fact]
        public void Execute_WrongMessage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForUpdate();
            this.Runtime.ForStep(UnsupportedMessage, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_WrongEntity_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForUpdate();
            this.Runtime.ForStep(PluginMessages.Update, OtherEntity, PluginStages.PostOperation, PluginModes.Synchronous);

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_UpdateAtTheWrongStage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForUpdate();
            this.Runtime.ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PreValidationStage, PluginModes.Synchronous);

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_AsynchronousMode_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForUpdate();
            this.Runtime.ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, AsynchronousMode);

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_UpdateWithoutATarget_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForUpdate();
            this.Runtime.WithoutTarget();

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_UpdateWithoutAPreImage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
            this.Runtime.WithTarget(this.Application());
            this.Runtime.WithPostImage(PostImageName, this.StoredApplication(riskLevel: null, counted: false));

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_UpdateWithoutAPostImage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
            this.Runtime.WithTarget(this.Application());
            this.Runtime.WithPreImage(PreImageName, this.StoredApplication(riskLevel: null, counted: false));

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_SetStateWithoutAnEntityMoniker_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForSetState();
            this.Runtime.InputParameters.Remove(PluginInputParameters.EntityMoniker);

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_SetStateWithoutAPostImage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForStep(PluginMessages.SetState, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
            this.Runtime.InputParameters[PluginInputParameters.EntityMoniker] = this.Reference();
            this.Runtime.WithPreImage(PreImageName, this.StoredApplication(riskLevel: null, counted: false));

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_DeleteAtTheWrongStage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForDelete();
            this.Runtime.ForStep(PluginMessages.Delete, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_DeleteWithoutATarget_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.ForDelete();
            this.Runtime.WithoutTarget();

            this.AssertExecuteFailsWithoutCallingService();
        }

        [Fact]
        public void Execute_DeleteWithoutAPreImage_ThrowsInvalidPluginExecutionWithoutCallingTheService()
        {
            this.Runtime.ForStep(PluginMessages.Delete, defraimp_importapplication.EntityLogicalName, PluginStages.PreOperation, PluginModes.Synchronous);
            this.Runtime.InputParameters[PluginInputParameters.Target] = this.Reference();

            this.AssertExecuteFailsWithoutCallingService();
        }

        private void ForUpdate()
        {
            this.Runtime
                .ForStep(PluginMessages.Update, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous)
                .WithTarget(this.Application());
        }

        private void ForSetState()
        {
            this.Runtime.ForStep(PluginMessages.SetState, defraimp_importapplication.EntityLogicalName, PluginStages.PostOperation, PluginModes.Synchronous);
            this.Runtime.InputParameters[PluginInputParameters.EntityMoniker] = this.Reference();
        }

        private void ForDelete()
        {
            this.Runtime.ForStep(PluginMessages.Delete, defraimp_importapplication.EntityLogicalName, PluginStages.PreOperation, PluginModes.Synchronous);
            this.Runtime.InputParameters[PluginInputParameters.Target] = this.Reference();
        }

        private IReadOnlyList<defraimp_counterhistory> CreatedHistoryRecords()
        {
            return this.dataverse.Creates
                .Where(entity => entity.LogicalName == defraimp_counterhistory.EntityLogicalName)
                .Select(entity => entity.ToEntity<defraimp_counterhistory>())
                .ToList();
        }

        private defraimp_importapplication Application()
        {
            return new defraimp_importapplication { Id = this.importApplicationId };
        }

        private EntityReference Reference()
        {
            return new EntityReference(defraimp_importapplication.EntityLogicalName, this.importApplicationId);
        }

        // A stored import application as the plug-in receives it in an image: an ITAHC with a primary ITAHC and no commodity or country.
        private defraimp_importapplication StoredApplication(
            EntityReference riskLevel,
            bool counted,
            defraimp_importapplication_statecode state = defraimp_importapplication_statecode.Active,
            defraimp_importapplication_statuscode? status = null)
        {
            return new defraimp_importapplication
            {
                Id = this.importApplicationId,
                defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
                defraimp_importrisklevelid = riskLevel,
                defraimp_PrimaryITAHCId = new EntityReference(defraimp_itahc.EntityLogicalName, Guid.NewGuid()),
                defraimp_ImportRecordCounted = counted,
                defraimp_ManualPostImportCheckDecision = defraimp_importapplication_defraimp_manualpostimportcheckdecision.UseSystemDecision,
                statecode = state,
                statuscode = status,
            };
        }
    }
}
