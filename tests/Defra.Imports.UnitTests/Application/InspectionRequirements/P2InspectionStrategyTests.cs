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

    public sealed class P2InspectionStrategyTests
    {
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly defraimp_importrisklevel riskLevel = InspectionTestData.RiskLevel("P2");
        private readonly InspectionServices services;

        public P2InspectionStrategyTests()
        {
            this.dataverse.Seed(this.riskLevel);
            this.services = new InspectionServices(this.dataverse);
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new P2InspectionStrategy(null, this.services.Updater, this.services.AutonumberCounters));
        }

        [Fact]
        public void Constructor_NullUpdater_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new P2InspectionStrategy(this.services.Queries, null, this.services.AutonumberCounters));
        }

        [Fact]
        public void Constructor_NullAutonumbers_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new P2InspectionStrategy(this.services.Queries, this.services.Updater, null));
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
            this.Execute(this.Application(type: defraimp_importapplication_defraimp_importapplicationtype.ImportNotification));

            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryImporterNotificationMissing);
        }

        [Fact]
        public void Execute_AlreadyCountedRecord_WritesNothing()
        {
            this.Execute(this.Application(counted: true));

            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
        }

        [Fact]
        public void Execute_BelowTheThreshold_CountsTheRecordAndRequiresNoInspection()
        {
            this.Execute(this.Application());

            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P2CounterName));
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired);
            Assert.Equal(
                new[]
                {
                    "Update:defraimp_autonumber",
                    "Create:defraimp_counterhistory",
                    "Update:defraimp_autonumber",
                    "Create:defraimp_counterhistory",
                    "Update:defraimp_importapplication",
                    "Update:defraimp_importapplication",
                },
                this.dataverse.Events.Where(entry => !entry.StartsWith("Query:", StringComparison.Ordinal)).ToArray());
            Assert.True(this.dataverse.UpdatesOf(defraimp_importapplication.EntityLogicalName)[0].ToEntity<defraimp_importapplication>().defraimp_ImportRecordCounted);
        }

        [Fact]
        public void Execute_AtTheThreshold_ResetsTheCounterAndRequiresAnInspection()
        {
            this.dataverse.SetCounter(InspectionConstants.P2CounterName, 1);

            this.Execute(this.Application());

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P2CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP2Inspection);
        }

        [Fact]
        public void Execute_WithAnOutstandingQuota_TakesTheQuotaAndRequiresAnInspection()
        {
            this.dataverse.SetCounter(InspectionConstants.P2QuotaCounterName, 1);

            this.Execute(this.Application());

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P2QuotaCounterName));
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P2CounterName));
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP2Inspection);
        }

        private InspectionImportApplicationContext Application(
            defraimp_importapplication_defraimp_importapplicationtype? type = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
            bool hasPrimaryItahc = true,
            bool? counted = false)
        {
            return InspectionTestData.Application(
                type: type,
                riskLevel: InspectionTestData.RiskLevelReference(this.riskLevel),
                hasPrimaryItahc: hasPrimaryItahc,
                counted: counted);
        }

        private void Execute(InspectionImportApplicationContext application)
        {
            this.Execute(this.services, application);
        }

        private void Execute(InspectionServices strategyServices, InspectionImportApplicationContext application)
        {
            var record = new InspectionRecord(application);
            var counters = strategyServices.CounterManagerFactory.Create(record);
            strategyServices.StrategyFactory.GetStrategy(InspectionConstants.P2RiskLevelName).Execute(record, counters);
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
