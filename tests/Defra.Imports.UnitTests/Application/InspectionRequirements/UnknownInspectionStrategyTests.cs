namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class UnknownInspectionStrategyTests
    {
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly InspectionServices services;

        public UnknownInspectionStrategyTests()
        {
            this.services = new InspectionServices(this.dataverse);
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new UnknownInspectionStrategy(null, this.services.Updater));
        }

        [Fact]
        public void Constructor_NullUpdater_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new UnknownInspectionStrategy(this.services.Queries, null));
        }

        [Fact]
        public void Execute_WithANullCounterManager_RecordsTheRiskLevelAsUnknownWithoutReadingOrCreating()
        {
            var application = InspectionTestData.Application();
            var record = new InspectionRecord(application);
            var strategy = new UnknownInspectionStrategy(this.services.Queries, this.services.Updater);

            strategy.Execute(record, null);

            Assert.Equal(new[] { "Update:defraimp_importapplication" }, this.dataverse.Events);
            Assert.Empty(this.dataverse.Creates);
            Assert.Single(this.dataverse.Updates);

            var update = this.dataverse.Updates[0].ToEntity<defraimp_importapplication>();
            Assert.Equal(application.Id, update.Id);
            Assert.Equal(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, update.defraimp_InspectionRequired);
            Assert.Equal(defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown, update.defraimp_InspectionRequiredReason);
            Assert.Equal((int)defraimp_importapplication_defraimp_inspectionrequired.Undetermined, update.defraimp_InspectionRequiredOriginalValue);
            Assert.Equal((int)defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown, update.defraimp_inspectionrequiredreasonoriginalvalue);
        }
    }
}
