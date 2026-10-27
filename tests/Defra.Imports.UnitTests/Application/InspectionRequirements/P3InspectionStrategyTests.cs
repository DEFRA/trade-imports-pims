namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using System.Linq;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class P3InspectionStrategyTests
    {
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly defraimp_importrisklevel riskLevel = InspectionTestData.RiskLevel("P3");
        private readonly InspectionServices services;

        public P3InspectionStrategyTests()
        {
            this.dataverse.Seed(this.riskLevel);
            this.services = new InspectionServices(this.dataverse);
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new P3InspectionStrategy(null, this.services.Updater, this.services.AutonumberCounters));
        }

        [Fact]
        public void Constructor_NullUpdater_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new P3InspectionStrategy(this.services.Queries, null, this.services.AutonumberCounters));
        }

        [Fact]
        public void Constructor_NullAutonumbers_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new P3InspectionStrategy(this.services.Queries, this.services.Updater, null));
        }

        [Fact]
        public void Execute_BelowTheThreshold_CountsTheRecordOnceAndRequiresNoInspection()
        {
            this.Execute(this.Application());

            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired);
        }

        [Fact]
        public void Execute_AtTheThreshold_ResetsTheCounterAndRequiresAnInspection()
        {
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);

            this.Execute(this.Application());

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP3Inspection);
        }

        [Fact]
        public void Execute_WithAnOutstandingQuota_TakesTheQuotaAndRequiresAnInspection()
        {
            this.dataverse.SetCounter(InspectionConstants.P3QuotaCounterName, 1);

            this.Execute(this.Application());

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3QuotaCounterName));
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP3Inspection);
            Assert.Contains(true, this.dataverse.UpdatesOf(defraimp_importapplication.EntityLogicalName).Select(update => update.ToEntity<defraimp_importapplication>().defraimp_ImportRecordCounted));
        }

        [Fact]
        public void Execute_ItahcWithoutPrimaryItahc_RecordsThePrimaryItahcAsMissing()
        {
            this.Execute(this.Application(hasPrimaryItahc: false));

            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryITAHCMissing);
        }

        [Fact]
        public void Execute_ItahcLandbridgeWithPrimaryItahc_CountsTheRecordAndFlagsItCounted()
        {
            this.Execute(InspectionTestData.Application(
                type: defraimp_importapplication_defraimp_importapplicationtype.ITAHCLandbridge,
                riskLevel: InspectionTestData.RiskLevelReference(this.riskLevel)));

            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.Contains(true, this.dataverse.UpdatesOf(defraimp_importapplication.EntityLogicalName).Select(update => update.ToEntity<defraimp_importapplication>().defraimp_ImportRecordCounted));
        }

        [Fact]
        public void Execute_AlreadyCountedRecord_WritesNothing()
        {
            this.Execute(this.Application(counted: true));

            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
        }

        private InspectionImportApplicationContext Application(bool hasPrimaryItahc = true, bool? counted = false)
        {
            return InspectionTestData.Application(
                riskLevel: InspectionTestData.RiskLevelReference(this.riskLevel),
                hasPrimaryItahc: hasPrimaryItahc,
                counted: counted);
        }

        private void Execute(InspectionImportApplicationContext application)
        {
            var record = new InspectionRecord(application);
            var counters = this.services.CounterManagerFactory.Create(record);
            this.services.StrategyFactory.GetStrategy(InspectionConstants.P3RiskLevelName).Execute(record, counters);
        }

        private void AssertOutcome(
            defraimp_importapplication_defraimp_inspectionrequired required,
            defraimp_importapplication_defraimp_inspectionrequiredreason reason)
        {
            var outcomes = this.dataverse
                .UpdatesOf(defraimp_importapplication.EntityLogicalName)
                .Select(entity => entity.ToEntity<defraimp_importapplication>())
                .Where(update => update.defraimp_InspectionRequired.HasValue)
                .ToList();
            var outcome = outcomes[outcomes.Count - 1];

            Assert.Equal(required, outcome.defraimp_InspectionRequired);
            Assert.Equal(reason, outcome.defraimp_InspectionRequiredReason);
            Assert.Equal((int)required, outcome.defraimp_InspectionRequiredOriginalValue);
            Assert.Equal((int)reason, outcome.defraimp_inspectionrequiredreasonoriginalvalue);
        }
    }
}
