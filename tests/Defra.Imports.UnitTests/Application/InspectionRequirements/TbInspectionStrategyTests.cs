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

    public sealed class TbInspectionStrategyTests
    {
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly defraimp_importrisklevel riskLevel = InspectionTestData.RiskLevel("TB");
        private readonly InspectionServices services;

        public TbInspectionStrategyTests()
        {
            this.dataverse.Seed(this.riskLevel);
            this.services = new InspectionServices(this.dataverse);
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TbInspectionStrategy(null, this.services.Updater));
        }

        [Fact]
        public void Constructor_NullUpdater_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TbInspectionStrategy(this.services.Queries, null));
        }

        [Fact]
        public void Execute_NotCounted_CountsTheGlobalCounterAndRequiresAnInspection()
        {
            this.Execute(this.Application());

            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.Contains("defraimp_counterhistory", this.dataverse.Creates.Select(entity => entity.LogicalName));
            Assert.Contains(true, this.dataverse.UpdatesOf(defraimp_importapplication.EntityLogicalName).Select(update => update.ToEntity<defraimp_importapplication>().defraimp_ImportRecordCounted));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.TB);
        }

        [Fact]
        public void Execute_AlreadyCountedRecord_WritesNothing()
        {
            this.Execute(this.Application(counted: true));

            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
        }

        [Fact]
        public void Execute_ItahcWithoutPrimaryItahc_RecordsThePrimaryItahcAsMissing()
        {
            this.Execute(this.Application(hasPrimaryItahc: false));

            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryITAHCMissing);
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
            this.services.StrategyFactory.GetStrategy(InspectionConstants.TbRiskLevelName).Execute(record, counters);
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
        }
    }
}
