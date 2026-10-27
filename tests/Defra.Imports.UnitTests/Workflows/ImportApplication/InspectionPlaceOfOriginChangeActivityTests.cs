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

    public sealed class InspectionPlaceOfOriginChangeActivityTests : WorkflowActivityTestBase<InspectionPlaceOfOriginChange>
    {
        private const string ImportApplicationInput = "ImportApplication";

        private static readonly string[] ChangeColumns =
        {
            "defraimp_importrisklevelid",
            "defraimp_previousimportrisklevelid",
            "defraimp_inspectionrequired",
            "defraimp_placeoforiginid",
            "defraimp_previousplaceoforiginid",
            "defraimp_commoditytypeid",
            "defraimp_countryoforiginid",
            "defraimp_inspectionrequiredreason",
        };

        private readonly Guid importApplicationId = Guid.NewGuid();
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly defraimp_importrisklevel p1 = InspectionTestData.RiskLevel("P1");
        private readonly EntityReference commodityType = InspectionTestData.CommodityTypeReference();
        private readonly EntityReference country = InspectionTestData.CountryReference();

        public InspectionPlaceOfOriginChangeActivityTests()
        {
            this.dataverse.Seed(this.p1);
            this.Runtime.Factory
                .Setup(factory => factory.CreateOrganizationService(this.Runtime.UserId))
                .Returns(this.dataverse.Service.Object);
        }

        [Fact]
        public void Activity_ImportApplicationInput_IsARequiredImportApplicationReference()
        {
            var property = typeof(InspectionPlaceOfOriginChange).GetProperty(ImportApplicationInput);

            Assert.Equal(typeof(InArgument<EntityReference>), property.PropertyType);
            Assert.NotNull(property.GetCustomAttribute<RequiredArgumentAttribute>());
            Assert.Equal(defraimp_importapplication.EntityLogicalName, property.GetCustomAttribute<ReferenceTargetAttribute>().EntityName);
            Assert.NotNull(property.GetCustomAttribute<InputAttribute>());
        }

        [Fact]
        public void Activity_Type_DeclaresNoOutputs()
        {
            var outputs = typeof(InspectionPlaceOfOriginChange)
                .GetProperties()
                .Where(property => property.GetCustomAttribute<OutputAttribute>() != null);

            Assert.Empty(outputs);
        }

        [Theory]
        [InlineData("P1")]
        [InlineData("p1")]
        public void Invoke_P1GoldApplicationWithAGoldPreviousPlace_IncrementsOnlyThePreviousPlacesQuotaCounter(string riskLevelName)
        {
            var previousPlace = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold);
            this.SeedGoldBronzeCoverage();
            this.dataverse.Seed(previousPlace);
            this.dataverse.Seed(this.StoredApplication(
                this.RiskLevelReference(riskLevelName),
                defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage,
                InspectionTestData.PlaceOfOriginReference(previousPlace)));

            this.Invoke(ImportApplicationInput, this.Reference());

            var update = Assert.Single(this.dataverse.Updates);
            Assert.Equal(defraimp_placeoforigin.EntityLogicalName, update.LogicalName);
            Assert.Equal(previousPlace.Id, update.Id);
            Assert.Equal(
                new[] { defraimp_placeoforigin.Fields.defraimp_InspectionQuotaCounter, defraimp_placeoforigin.EntityLogicalName + "id" },
                update.Attributes.Keys.OrderBy(key => key, StringComparer.Ordinal));
            Assert.Equal(1, update.ToEntity<defraimp_placeoforigin>().defraimp_InspectionQuotaCounter);
            Assert.Empty(this.dataverse.Creates);
        }

        [Fact]
        public void Invoke_P1ApplicationWithABronzePreviousPlace_WritesNothing()
        {
            var previousPlace = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Bronze);
            this.SeedGoldBronzeCoverage();
            this.dataverse.Seed(previousPlace);
            this.dataverse.Seed(this.StoredApplication(
                this.RiskLevelReference("P1"),
                defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage,
                InspectionTestData.PlaceOfOriginReference(previousPlace)));

            this.Invoke(ImportApplicationInput, this.Reference());

            this.AssertNothingWritten();
        }

        [Fact]
        public void Invoke_P1ApplicationWithAnotherReason_WritesNothing()
        {
            var previousPlace = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold);
            this.SeedGoldBronzeCoverage();
            this.dataverse.Seed(previousPlace);
            this.dataverse.Seed(this.StoredApplication(
                this.RiskLevelReference("P1"),
                defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection,
                InspectionTestData.PlaceOfOriginReference(previousPlace)));

            this.Invoke(ImportApplicationInput, this.Reference());

            this.AssertNothingWritten();
        }

        [Fact]
        public void Invoke_P1ApplicationWithoutAPreviousPlace_WritesNothing()
        {
            this.SeedGoldBronzeCoverage();
            this.dataverse.Seed(this.StoredApplication(
                this.RiskLevelReference("P1"),
                defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage,
                previousPlaceOfOrigin: null));

            this.Invoke(ImportApplicationInput, this.Reference());

            this.AssertNothingWritten();
        }

        [Fact]
        public void Invoke_P1ApplicationWithAnUncoveredCommodity_WritesNothing()
        {
            var previousPlace = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold);
            this.dataverse.Seed(previousPlace);
            this.dataverse.Seed(this.StoredApplication(
                this.RiskLevelReference("P1"),
                defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage,
                InspectionTestData.PlaceOfOriginReference(previousPlace)));

            this.Invoke(ImportApplicationInput, this.Reference());

            this.AssertNothingWritten();
        }

        [Fact]
        public void Invoke_NotP1Application_WritesNothing()
        {
            var previousPlace = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold);
            this.SeedGoldBronzeCoverage();
            this.dataverse.Seed(previousPlace);
            this.dataverse.Seed(this.StoredApplication(
                this.RiskLevelReference("P2"),
                defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage,
                InspectionTestData.PlaceOfOriginReference(previousPlace)));

            this.Invoke(ImportApplicationInput, this.Reference());

            this.AssertNothingWritten();
        }

        [Fact]
        public void Invoke_RetrievesItOnceWithTheChangeColumns()
        {
            this.dataverse.Seed(this.StoredApplication(this.RiskLevelReference("P2"), null, previousPlaceOfOrigin: null));

            this.Invoke(ImportApplicationInput, this.Reference());

            this.dataverse.Service.Verify(
                service => service.Retrieve(
                    defraimp_importapplication.EntityLogicalName,
                    this.importApplicationId,
                    It.Is<ColumnSet>(columns => HasExactly(columns, ChangeColumns))),
                Times.Once());
        }

        [Fact]
        public void Invoke_UnknownApplication_FailsAndWritesNothing()
        {
            var exception = Assert.ThrowsAny<Exception>(() => this.Invoke(ImportApplicationInput, this.Reference()));

            Assert.Contains(exception.Flatten(), inner => inner is InvalidOperationException);
            this.AssertNothingWritten();
        }

        [Fact]
        public void Invoke_ValidReference_ReturnsNoOutputs()
        {
            this.dataverse.Seed(this.StoredApplication(this.RiskLevelReference("P2"), null, previousPlaceOfOrigin: null));

            var output = this.Invoke(ImportApplicationInput, this.Reference());

            Assert.Empty(output);
        }

        [Fact]
        public void Handler_NullActivity_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionPlaceOfOriginChangeHandler(
                null,
                UninitialisedContext(),
                new Mock<IInspectionPlaceOfOriginChanger>(MockBehavior.Strict).Object));
        }

        [Fact]
        public void Handler_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionPlaceOfOriginChangeHandler(
                new InspectionPlaceOfOriginChange(),
                null,
                new Mock<IInspectionPlaceOfOriginChanger>(MockBehavior.Strict).Object));
        }

        [Fact]
        public void Handler_NullChanger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionPlaceOfOriginChangeHandler(
                new InspectionPlaceOfOriginChange(),
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

        private void SeedGoldBronzeCoverage()
        {
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, this.country));
        }

        private void AssertNothingWritten()
        {
            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
        }

        private EntityReference RiskLevelReference(string name)
        {
            return InspectionTestData.RiskLevelReference(InspectionTestData.RiskLevel(name));
        }

        private EntityReference Reference()
        {
            return new EntityReference(defraimp_importapplication.EntityLogicalName, this.importApplicationId);
        }

        // A stored import application. The commodity and country are those of the gold/bronze rules, so the rules apply.
        private defraimp_importapplication StoredApplication(
            EntityReference riskLevel,
            defraimp_importapplication_defraimp_inspectionrequiredreason? reason,
            EntityReference previousPlaceOfOrigin = null)
        {
            return new defraimp_importapplication
            {
                Id = this.importApplicationId,
                defraimp_importrisklevelid = riskLevel,
                defraimp_InspectionRequiredReason = reason,
                defraimp_previousplaceoforiginid = previousPlaceOfOrigin,
                defraimp_CommodityTypeId = this.commodityType,
                defraimp_CountryofOriginId = this.country,
            };
        }
    }
}
