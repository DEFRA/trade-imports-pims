namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class RiskCounterManagerFactoryTests
    {
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly defraimp_importrisklevel p1 = InspectionTestData.RiskLevel("P1");
        private readonly defraimp_importrisklevel tb = InspectionTestData.RiskLevel("TB");
        private readonly EntityReference commodityType = InspectionTestData.CommodityTypeReference();
        private readonly EntityReference country = InspectionTestData.CountryReference();
        private readonly InspectionServices services;

        public RiskCounterManagerFactoryTests()
        {
            this.dataverse.Seed(this.p1, this.tb);
            this.services = new InspectionServices(this.dataverse);
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RiskCounterManagerFactory(null, this.services.AutonumberCounters, this.services.PlaceOfOriginCounters, this.services.Auditor, this.dataverse.Service.Object, this.services.Logger));
        }

        [Fact]
        public void Constructor_NullAutonumbers_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RiskCounterManagerFactory(this.services.Queries, null, this.services.PlaceOfOriginCounters, this.services.Auditor, this.dataverse.Service.Object, this.services.Logger));
        }

        [Fact]
        public void Constructor_NullPlaceOfOriginCounters_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RiskCounterManagerFactory(this.services.Queries, this.services.AutonumberCounters, null, this.services.Auditor, this.dataverse.Service.Object, this.services.Logger));
        }

        [Fact]
        public void Constructor_NullAuditor_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RiskCounterManagerFactory(this.services.Queries, this.services.AutonumberCounters, this.services.PlaceOfOriginCounters, null, this.dataverse.Service.Object, this.services.Logger));
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RiskCounterManagerFactory(this.services.Queries, this.services.AutonumberCounters, this.services.PlaceOfOriginCounters, this.services.Auditor, null, this.services.Logger));
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RiskCounterManagerFactory(this.services.Queries, this.services.AutonumberCounters, this.services.PlaceOfOriginCounters, this.services.Auditor, this.dataverse.Service.Object, null));
        }

        [Fact]
        public void Create_NullRecord_ReturnsNullWithoutReading()
        {
            Assert.Null(this.services.CounterManagerFactory.Create(null));

            Assert.Empty(this.dataverse.Events);
        }

        [Fact]
        public void Create_RecordWithoutARiskLevel_ReturnsNullWithoutReading()
        {
            var record = new InspectionRecord(InspectionTestData.Application());

            Assert.Null(this.services.CounterManagerFactory.Create(record));

            Assert.Empty(this.dataverse.Events);
        }

        [Fact]
        public void Create_RiskLevelThatIsNotFound_ReturnsNullAfterReadingTheRiskLevel()
        {
            var missing = new EntityReference(defraimp_importrisklevel.EntityLogicalName, Guid.NewGuid()) { Name = "P1" };
            var record = new InspectionRecord(InspectionTestData.Application(riskLevel: missing));

            Assert.Null(this.services.CounterManagerFactory.Create(record));

            Assert.Equal(new[] { "Query:defraimp_importrisklevel" }, this.dataverse.Events);
        }

        [Fact]
        public void Create_RiskLevelWithoutAName_ReturnsNull()
        {
            var unnamed = InspectionTestData.RiskLevel(null);
            this.dataverse.Seed(unnamed);
            var record = new InspectionRecord(InspectionTestData.Application(riskLevel: InspectionTestData.RiskLevelReference(unnamed)));

            Assert.Null(this.services.CounterManagerFactory.Create(record));
        }

        [Fact]
        public void Create_RiskLevelWithoutGoldBronzeRules_ReturnsAnAutonumberManagerAfterReadingItsCounters()
        {
            var record = new InspectionRecord(InspectionTestData.Application(riskLevel: InspectionTestData.RiskLevelReference(this.p1)));

            var manager = this.services.CounterManagerFactory.Create(record);

            Assert.IsType<AutonumberRiskCounterManager>(manager);
            Assert.Equal(new[] { "Query:defraimp_importrisklevel", "Query:defraimp_autonumber", "Query:defraimp_autonumber" }, this.dataverse.Events);
        }

        [Fact]
        public void Create_GoldBronzeCommodityNotCoveredForTheCountry_ReturnsAnAutonumberManager()
        {
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, InspectionTestData.CountryReference()));
            var record = this.RecordWithGoldBronzeReferences();

            Assert.IsType<AutonumberRiskCounterManager>(this.services.CounterManagerFactory.Create(record));
        }

        [Fact]
        public void Create_GoldBronzeRulesThatApply_ReturnsAPlaceOfOriginManagerAfterReadingThePlaceOfOrigin()
        {
            var place = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold);
            this.dataverse.Seed(place);
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, this.country));
            var record = new InspectionRecord(InspectionTestData.Application(
                riskLevel: InspectionTestData.RiskLevelReference(this.p1),
                placeOfOrigin: InspectionTestData.PlaceOfOriginReference(place),
                commodityType: this.commodityType,
                country: this.country));

            var manager = this.services.CounterManagerFactory.Create(record);

            Assert.IsType<PlaceOfOriginRiskCounterManager>(manager);
            Assert.Equal(
                new[] { "Query:defraimp_importrisklevel", "Query:defraimp_goldbronzecommodity", "Query:defraimp_goldbronzecountriesnn", "Query:defraimp_placeoforigin" },
                this.dataverse.Events);
        }

        [Fact]
        public void Create_GoldBronzeRulesThatApplyWithoutAPlaceOfOrigin_DoesNotReadAPlaceOfOrigin()
        {
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, this.country));
            var record = this.RecordWithGoldBronzeReferences();

            var manager = this.services.CounterManagerFactory.Create(record);

            Assert.IsType<PlaceOfOriginRiskCounterManager>(manager);
            Assert.DoesNotContain("Query:defraimp_placeoforigin", this.dataverse.Events);
        }

        [Theory]
        [InlineData("TB")]
        [InlineData("tb")]
        public void Create_TbRiskLevel_ReturnsAGlobalCounterOnlyManagerWithoutReadingAutonumbers(string name)
        {
            var riskLevel = InspectionTestData.RiskLevel(name);
            this.dataverse.Seed(riskLevel);
            var record = new InspectionRecord(InspectionTestData.Application(riskLevel: InspectionTestData.RiskLevelReference(riskLevel)));

            var manager = this.services.CounterManagerFactory.Create(record);

            Assert.IsType<OnlyGlobalCounterRiskCounterManager>(manager);
            Assert.DoesNotContain("Query:defraimp_autonumber", this.dataverse.Events);
        }

        [Fact]
        public void Create_TbRiskLevelCoveredByGoldBronze_ReturnsAPlaceOfOriginManager()
        {
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, this.country));
            var record = new InspectionRecord(InspectionTestData.Application(
                riskLevel: InspectionTestData.RiskLevelReference(this.tb),
                commodityType: this.commodityType,
                country: this.country));

            Assert.IsType<PlaceOfOriginRiskCounterManager>(this.services.CounterManagerFactory.Create(record));
        }

        private InspectionRecord RecordWithGoldBronzeReferences()
        {
            return new InspectionRecord(InspectionTestData.Application(
                riskLevel: InspectionTestData.RiskLevelReference(this.p1),
                commodityType: this.commodityType,
                country: this.country));
        }
    }
}
