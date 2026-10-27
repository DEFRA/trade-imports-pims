namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using System.Linq;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Application.ReferenceData;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class P1InspectionStrategyTests
    {
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly defraimp_importrisklevel riskLevel = InspectionTestData.RiskLevel("P1");
        private readonly EntityReference commodityType = InspectionTestData.CommodityTypeReference();
        private readonly EntityReference country = InspectionTestData.CountryReference();
        private readonly InspectionServices services;

        public P1InspectionStrategyTests()
        {
            this.dataverse.Seed(this.riskLevel);
            this.services = new InspectionServices(this.dataverse);
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new P1InspectionStrategy(null, this.services.Updater, this.services.PlaceOfOriginCounters, this.services.AutonumberCounters));
        }

        [Fact]
        public void Constructor_NullUpdater_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new P1InspectionStrategy(this.services.Queries, null, this.services.PlaceOfOriginCounters, this.services.AutonumberCounters));
        }

        [Fact]
        public void Constructor_NullPlaceOfOriginCounters_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new P1InspectionStrategy(this.services.Queries, this.services.Updater, null, this.services.AutonumberCounters));
        }

        [Fact]
        public void Constructor_NullAutonumbers_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new P1InspectionStrategy(this.services.Queries, this.services.Updater, this.services.PlaceOfOriginCounters, null));
        }

        [Fact]
        public void Execute_ItahcWithoutPrimaryItahc_RecordsThePrimaryItahcAsMissing()
        {
            this.Execute(this.Application(hasPrimaryItahc: false));

            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryITAHCMissing);
            Assert.Empty(this.dataverse.Creates);
        }

        [Fact]
        public void Execute_ItahcWithTracesDisabled_RecordsThatTracesIsDisabled()
        {
            var disabled = new InspectionDataverse().WithReferenceData(tracesEnabled: false).Seed(this.riskLevel);

            this.Execute(new InspectionServices(disabled), this.Application());

            Assert.Equal(
                defraimp_importapplication_defraimp_inspectionrequiredreason.TRACESisdisabledUseImporterNotificationsinstead,
                disabled.Updates[disabled.Updates.Count - 1].ToEntity<defraimp_importapplication>().defraimp_InspectionRequiredReason);
        }

        [Fact]
        public void Execute_ImportNotificationWithoutPrimaryNotification_RecordsThePrimaryImporterNotificationAsMissing()
        {
            this.Execute(this.Application(type: defraimp_importapplication_defraimp_importapplicationtype.ImportNotification, hasPrimaryItahc: false));

            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryImporterNotificationMissing);
        }

        [Fact]
        public void Execute_TypeWithoutACertificateRule_RecordsNothing()
        {
            this.Execute(this.Application(type: null, hasPrimaryItahc: false));

            Assert.Empty(this.dataverse.Updates);
        }

        [Fact]
        public void Execute_AlreadyCountedRecord_WritesNothing()
        {
            this.Execute(this.Application(counted: true));

            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void Execute_MissingCommodityOrCountry_RecordsTheRiskLevelAsUnknown(bool hasCommodity, bool hasCountry)
        {
            this.Execute(this.Application(commodityType: hasCommodity ? this.commodityType : null, country: hasCountry ? this.country : null));

            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown);
        }

        [Fact]
        public void Execute_BelowTheThreshold_CountsTheRecordAndRequiresNoInspection()
        {
            this.Execute(this.Application(commodityType: this.commodityType, country: this.country));

            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired);
            Assert.Equal(
                new[] { "Update:defraimp_autonumber", "Create:defraimp_counterhistory", "Update:defraimp_autonumber", "Create:defraimp_counterhistory", "Update:defraimp_importapplication", "Update:defraimp_importapplication" },
                this.dataverse.Events.Where(entry => !entry.StartsWith("Query:", StringComparison.Ordinal)).ToArray());
            Assert.True(this.dataverse.UpdatesOf(defraimp_importapplication.EntityLogicalName)[0].ToEntity<defraimp_importapplication>().defraimp_ImportRecordCounted);
        }

        [Fact]
        public void Execute_AtTheThreshold_ResetsTheCounterAndRequiresAnInspection()
        {
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 1);

            this.Execute(this.Application(commodityType: this.commodityType, country: this.country));

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection);
        }

        [Fact]
        public void Execute_WithAnOutstandingQuota_TakesTheQuotaAndRequiresAnInspection()
        {
            this.dataverse.SetCounter(InspectionConstants.P1QuotaCounterName, 1);

            this.Execute(this.Application(commodityType: this.commodityType, country: this.country));

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1QuotaCounterName));
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection);
        }

        [Fact]
        public void Execute_GoldPlaceOfOriginBelowTheThresholdWithNoQuota_RequiresNoInspection()
        {
            var place = this.SeedGoldBronze(defraimp_trustlevel.Gold, applicationCounter: 0, quotaCounter: 0);

            this.Execute(this.Application(placeOfOrigin: InspectionTestData.PlaceOfOriginReference(place), commodityType: this.commodityType, country: this.country));

            Assert.Equal(1, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_ApplicationCounter);
            Assert.Equal(0, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_InspectionQuotaCounter);
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequiredGoldPlaceofOrigin);
        }

        [Fact]
        public void Execute_GoldPlaceOfOriginReachingTheThreshold_AddsAndTakesAQuotaAndResetsTheCounter()
        {
            var place = this.SeedGoldBronze(defraimp_trustlevel.Gold, applicationCounter: 1, quotaCounter: 0);

            this.Execute(this.Application(placeOfOrigin: InspectionTestData.PlaceOfOriginReference(place), commodityType: this.commodityType, country: this.country));

            Assert.Equal(0, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_ApplicationCounter);
            Assert.Equal(0, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_InspectionQuotaCounter);
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage);
        }

        [Fact]
        public void Execute_GoldPlaceOfOriginWithAnOutstandingQuota_TakesTheQuota()
        {
            var place = this.SeedGoldBronze(defraimp_trustlevel.Gold, applicationCounter: 0, quotaCounter: 1);

            this.Execute(this.Application(placeOfOrigin: InspectionTestData.PlaceOfOriginReference(place), commodityType: this.commodityType, country: this.country));

            Assert.Equal(1, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_ApplicationCounter);
            Assert.Equal(0, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_InspectionQuotaCounter);
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage);
        }

        [Fact]
        public void Execute_BronzePlaceOfOrigin_CountsTheGlobalCounterAndRequiresAnInspection()
        {
            var place = this.SeedGoldBronze(defraimp_trustlevel.Bronze, applicationCounter: 0, quotaCounter: 0);

            this.Execute(this.Application(placeOfOrigin: InspectionTestData.PlaceOfOriginReference(place), commodityType: this.commodityType, country: this.country));

            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin);
        }

        [Fact]
        public void Execute_BronzePlaceOfOriginLockedToBronze_RecordsTheLock()
        {
            var place = this.SeedGoldBronze(defraimp_trustlevel.Bronze, applicationCounter: 0, quotaCounter: 0, lockToBronze: true);

            this.Execute(this.Application(placeOfOrigin: InspectionTestData.PlaceOfOriginReference(place), commodityType: this.commodityType, country: this.country));

            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.PlaceofOriginLockedtoBronze);
        }

        [Fact]
        public void Execute_GoldBronzeCommodityWithoutAPlaceOfOrigin_RecordsThePlaceOfOriginAsMissing()
        {
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, this.country));

            this.Execute(this.Application(commodityType: this.commodityType, country: this.country));

            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Discretionary, defraimp_importapplication_defraimp_inspectionrequiredreason.VerifiedPlaceofOriginMissing);
        }

        [Fact]
        public void Execute_PlaceOfOriginThatIsNeitherGoldNorBronze_WritesNothing()
        {
            var place = this.SeedGoldBronze(null, applicationCounter: 0, quotaCounter: 0);

            this.Execute(this.Application(placeOfOrigin: InspectionTestData.PlaceOfOriginReference(place), commodityType: this.commodityType, country: this.country));

            Assert.Empty(this.dataverse.Updates);
        }

        [Fact]
        public void Execute_GoldPlaceOfOriginWithoutAGoldBronzeCoverageRule_FailsWithANullReference()
        {
            // The strategy has no guard for the missing rule, which is preserved behaviour.
            var withoutRule = new InspectionDataverse().Seed(
                this.riskLevel,
                InspectionTestData.Autonumber(InspectionConstants.P3CounterName, 0),
                InspectionTestData.ConfigurationParameter(ConfigurationParameters.TracesEnabledKey, "true"));
            withoutRule.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, this.country));
            var place = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold);
            withoutRule.Seed(place);
            var application = this.Application(placeOfOrigin: InspectionTestData.PlaceOfOriginReference(place), commodityType: this.commodityType, country: this.country);

            Assert.Throws<NullReferenceException>(() => this.Execute(new InspectionServices(withoutRule), application));
        }

        private InspectionImportApplicationContext Application(
            defraimp_importapplication_defraimp_importapplicationtype? type = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
            EntityReference placeOfOrigin = null,
            EntityReference commodityType = null,
            EntityReference country = null,
            bool hasPrimaryItahc = true,
            bool? counted = false)
        {
            return InspectionTestData.Application(
                type: type,
                riskLevel: InspectionTestData.RiskLevelReference(this.riskLevel),
                placeOfOrigin: placeOfOrigin,
                commodityType: commodityType,
                country: country,
                hasPrimaryItahc: hasPrimaryItahc,
                counted: counted);
        }

        private defraimp_placeoforigin SeedGoldBronze(defraimp_trustlevel? trustLevel, int applicationCounter, int quotaCounter, bool lockToBronze = false)
        {
            var place = InspectionTestData.PlaceOfOrigin(trustLevel, applicationCounter, quotaCounter, lockToBronze);
            this.dataverse.Seed(place);
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, this.country));
            return place;
        }

        private void Execute(InspectionImportApplicationContext application)
        {
            this.Execute(this.services, application);
        }

        private void Execute(InspectionServices strategyServices, InspectionImportApplicationContext application)
        {
            var record = new InspectionRecord(application);
            var counters = strategyServices.CounterManagerFactory.Create(record);
            strategyServices.StrategyFactory.GetStrategy(InspectionConstants.P1RiskLevelName).Execute(record, counters);
        }

        private void AssertOutcome(
            defraimp_importapplication_defraimp_inspectionrequired required,
            defraimp_importapplication_defraimp_inspectionrequiredreason reason)
        {
            var outcome = this.dataverse
                .UpdatesOf(defraimp_importapplication.EntityLogicalName)
                .Select(entity => entity.ToEntity<defraimp_importapplication>())
                .Last(update => update.defraimp_InspectionRequired.HasValue);

            Assert.Equal(required, outcome.defraimp_InspectionRequired);
            Assert.Equal(reason, outcome.defraimp_InspectionRequiredReason);
            Assert.Equal((int)required, outcome.defraimp_InspectionRequiredOriginalValue);
            Assert.Equal((int)reason, outcome.defraimp_inspectionrequiredreasonoriginalvalue);
        }
    }
}
