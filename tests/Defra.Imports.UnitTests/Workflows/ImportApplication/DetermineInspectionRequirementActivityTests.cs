namespace Defra.Imports.UnitTests.Workflows.ImportApplication
{
    using System;
    using System.Activities;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.UnitTests.Support;
    using Defra.Imports.Workflows.ImportApplication;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Microsoft.Xrm.Sdk.Workflow;
    using Moq;
    using Xunit;

    public sealed class DetermineInspectionRequirementActivityTests : WorkflowActivityTestBase<DetermineInspectionRequirement>
    {
        private const string ImportApplicationInput = "ImportApplication";

        private static readonly string[] DeterminationColumns =
        {
            "defraimp_importrisklevelid",
            "defraimp_previousimportrisklevelid",
            "defraimp_inspectionrequired",
            "defraimp_placeoforiginid",
            "defraimp_commoditytypeid",
            "defraimp_countryoforiginid",
            "defraimp_primaryitahcid",
        };

        private readonly Guid importApplicationId = Guid.NewGuid();
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly defraimp_importrisklevel p1 = InspectionTestData.RiskLevel("P1");

        public DetermineInspectionRequirementActivityTests()
        {
            this.dataverse.Seed(this.p1);
            this.Runtime.Factory
                .Setup(factory => factory.CreateOrganizationService(this.Runtime.UserId))
                .Returns(this.dataverse.Service.Object);
        }

        [Fact]
        public void Activity_ImportApplicationInput_IsARequiredImportApplicationReference()
        {
            var property = typeof(DetermineInspectionRequirement).GetProperty(ImportApplicationInput);

            Assert.Equal(typeof(InArgument<EntityReference>), property.PropertyType);
            Assert.NotNull(property.GetCustomAttribute<RequiredArgumentAttribute>());
            Assert.Equal(defraimp_importapplication.EntityLogicalName, property.GetCustomAttribute<ReferenceTargetAttribute>().EntityName);
            Assert.NotNull(property.GetCustomAttribute<InputAttribute>());
        }

        [Fact]
        public void Activity_Type_DeclaresNoOutputs()
        {
            var outputs = typeof(DetermineInspectionRequirement)
                .GetProperties()
                .Where(property => property.GetCustomAttribute<OutputAttribute>() != null);

            Assert.Empty(outputs);
        }

        [Fact]
        public void Invoke_ApplicationWithoutARiskLevel_RetrievesItOnceWithTheColumnsAndRecordsTheRiskLevelAsUnknown()
        {
            this.dataverse.Seed(this.StoredApplication(riskLevel: null));

            this.Invoke(ImportApplicationInput, this.Reference());

            this.dataverse.Service.Verify(
                service => service.Retrieve(
                    defraimp_importapplication.EntityLogicalName,
                    this.importApplicationId,
                    It.Is<ColumnSet>(columns => HasExactly(columns, DeterminationColumns))),
                Times.Once());
            Assert.Empty(this.dataverse.Creates);
            var update = Assert.Single(this.dataverse.Updates).ToEntity<defraimp_importapplication>();
            Assert.Equal(defraimp_importapplication.EntityLogicalName, update.LogicalName);
            Assert.Equal(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, update.defraimp_InspectionRequired);
            Assert.Equal(defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown, update.defraimp_InspectionRequiredReason);
        }

        [Fact]
        public void Invoke_P1Application_WritesNothing()
        {
            this.dataverse.Seed(this.StoredApplication(InspectionTestData.RiskLevelReference(this.p1)));

            this.Invoke(ImportApplicationInput, this.Reference());

            this.dataverse.Service.Verify(
                service => service.Retrieve(
                    defraimp_importapplication.EntityLogicalName,
                    this.importApplicationId,
                    It.Is<ColumnSet>(columns => HasExactly(columns, DeterminationColumns))),
                Times.Once());
            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
        }

        [Fact]
        public void Invoke_UnknownApplication_FailsAndWritesNothing()
        {
            var exception = Assert.ThrowsAny<Exception>(() => this.Invoke(ImportApplicationInput, this.Reference()));

            Assert.Contains(exception.Flatten(), inner => inner is InvalidOperationException);
            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
        }

        [Fact]
        public void Invoke_ValidReference_ReturnsNoOutputs()
        {
            this.dataverse.Seed(this.StoredApplication(riskLevel: null));

            var output = this.Invoke(ImportApplicationInput, this.Reference());

            Assert.Empty(output);
        }

        [Fact]
        public void Handler_NullActivity_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DetermineInspectionRequirementHandler(
                null,
                UninitialisedContext(),
                new Mock<IInspectionRequirementDeterminer>(MockBehavior.Strict).Object));
        }

        [Fact]
        public void Handler_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DetermineInspectionRequirementHandler(
                new DetermineInspectionRequirement(),
                null,
                new Mock<IInspectionRequirementDeterminer>(MockBehavior.Strict).Object));
        }

        [Fact]
        public void Handler_NullDeterminer_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new DetermineInspectionRequirementHandler(
                new DetermineInspectionRequirement(),
                UninitialisedContext(),
                null));
        }

        private static CodeActivityContext UninitialisedContext()
        {
            return (CodeActivityContext)FormatterServices.GetUninitializedObject(typeof(CodeActivityContext));
        }

        private static bool HasExactly(ColumnSet columns, string[] names)
        {
            return !columns.AllColumns
                && columns.Columns.Count == names.Length
                && names.All(name => columns.Columns.Contains(name));
        }

        private EntityReference Reference()
        {
            return new EntityReference(defraimp_importapplication.EntityLogicalName, this.importApplicationId);
        }

        // A stored import application with an ITAHC type. The type is not among the retrieved columns, so it is not used.
        private defraimp_importapplication StoredApplication(EntityReference riskLevel)
        {
            return new defraimp_importapplication
            {
                Id = this.importApplicationId,
                defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
                defraimp_importrisklevelid = riskLevel,
                defraimp_PrimaryITAHCId = new EntityReference(defraimp_itahc.EntityLogicalName, Guid.NewGuid()),
                defraimp_ImportRecordCounted = false,
            };
        }
    }
}
