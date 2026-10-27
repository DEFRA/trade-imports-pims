namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class InspectionPlaceOfOriginChangerTests
    {
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly InspectionServices services;

        public InspectionPlaceOfOriginChangerTests()
        {
            this.services = new InspectionServices(this.dataverse);
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionPlaceOfOriginChanger(null, this.services.PlaceOfOriginCounters, this.services.StrategyFactory, this.dataverse.Service.Object));
        }

        [Fact]
        public void Constructor_NullPlaceOfOriginCounters_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionPlaceOfOriginChanger(this.services.Queries, null, this.services.StrategyFactory, this.dataverse.Service.Object));
        }

        [Fact]
        public void Constructor_NullStrategyFactory_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionPlaceOfOriginChanger(this.services.Queries, this.services.PlaceOfOriginCounters, null, this.dataverse.Service.Object));
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionPlaceOfOriginChanger(this.services.Queries, this.services.PlaceOfOriginCounters, this.services.StrategyFactory, null));
        }

        [Fact]
        public void ChangePlaceOfOrigin_ApplicationWithoutARiskLevel_ReadsOnlyTheApplication()
        {
            var application = new defraimp_importapplication { Id = Guid.NewGuid() };
            this.dataverse.Seed(application);

            this.services.Changer.ChangePlaceOfOrigin(application.Id);

            Assert.Equal(new[] { "Retrieve:defraimp_importapplication" }, this.dataverse.Events);
        }

        [Fact]
        public void ChangePlaceOfOrigin_RiskLevelWithoutAName_FailsWithANullReference()
        {
            // TODO: a risk level reference without a name fails. This is preserved behaviour.
            var riskLevel = InspectionTestData.RiskLevel("P1");
            var application = new defraimp_importapplication
            {
                Id = Guid.NewGuid(),
                defraimp_importrisklevelid = new EntityReference(defraimp_importrisklevel.EntityLogicalName, riskLevel.Id),
            };
            this.dataverse.Seed(riskLevel, application);

            Assert.Throws<NullReferenceException>(() => this.services.Changer.ChangePlaceOfOrigin(application.Id));
        }

        [Fact]
        public void ChangePlaceOfOrigin_PreviousGoldPlaceOfOriginInspectedForCoverage_IncreasesItsQuotaThenDeterminesTheP1Inspection()
        {
            var riskLevel = InspectionTestData.RiskLevel("p1");
            var commodityType = InspectionTestData.CommodityTypeReference();
            var country = InspectionTestData.CountryReference();
            var previous = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold, 0, 1);
            var application = new defraimp_importapplication
            {
                Id = Guid.NewGuid(),
                defraimp_importrisklevelid = InspectionTestData.RiskLevelReference(riskLevel),
                defraimp_previousplaceoforiginid = InspectionTestData.PlaceOfOriginReference(previous),
                defraimp_CommodityTypeId = commodityType,
                defraimp_CountryofOriginId = country,
                defraimp_InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage,
            };
            this.dataverse.Seed(riskLevel, previous, application);
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(commodityType, country));

            this.services.Changer.ChangePlaceOfOrigin(application.Id);

            Assert.Equal(2, this.dataverse.Stored<defraimp_placeoforigin>(previous.Id).defraimp_InspectionQuotaCounter);
            Assert.Single(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
        }
    }
}
