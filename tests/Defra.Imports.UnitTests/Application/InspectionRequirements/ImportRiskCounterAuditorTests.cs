namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class ImportRiskCounterAuditorTests
    {
        private readonly InspectionDataverse dataverse = new InspectionDataverse();
        private readonly Guid importApplicationId = Guid.NewGuid();
        private readonly ImportRiskCounterAuditor auditor;

        public ImportRiskCounterAuditorTests()
        {
            this.auditor = new ImportRiskCounterAuditor(this.dataverse.Service.Object);
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ImportRiskCounterAuditor(null));
        }

        [Fact]
        public void Audit_NullDetail_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.auditor.Audit(null));
        }

        [Fact]
        public void Audit_AutonumberDetail_CreatesOneAutoNumberCounterHistory()
        {
            var autonumber = InspectionTestData.Autonumber("counter_key", 1);
            var detail = CounterTransactionDetail.ForAutonumber(this.importApplicationId, autonumber, defraimp_counterhistory_defraimp_operation.IncrementCounter, defraimp_counterhistory_defraimp_reason.GlobalCounter);
            detail.PreviousValue = 1;
            detail.CurrentValue = 2;

            this.auditor.Audit(detail);

            Assert.Equal(new[] { "Create:defraimp_counterhistory" }, this.dataverse.Events);
            Assert.Single(this.dataverse.Creates);

            var history = this.dataverse.Creates[0].ToEntity<defraimp_counterhistory>();
            Assert.Equal(this.importApplicationId, history.defraimp_ImportApplicationId.Id);
            Assert.Equal(defraimp_counterhistory_defraimp_counterhistorytype.AutoNumber, history.defraimp_CounterHistoryType);
            Assert.Equal(defraimp_counterhistory_defraimp_operation.IncrementCounter, history.defraimp_Operation);
            Assert.Equal(defraimp_counterhistory_defraimp_reason.GlobalCounter, history.defraimp_Reason);
            Assert.Equal(1, history.defraimp_PreviousValue);
            Assert.Equal(2, history.defraimp_CurrentValue);
            Assert.Equal(autonumber.Id, history.defraimp_AutoNumberId.Id);
            Assert.Null(history.defraimp_PlaceOfOriginId);
        }

        [Fact]
        public void Audit_PlaceOfOriginDetail_CreatesOnePlaceOfOriginCounterHistory()
        {
            var placeOfOrigin = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold);
            var detail = CounterTransactionDetail.ForPlaceOfOrigin(this.importApplicationId, placeOfOrigin, defraimp_counterhistory_defraimp_operation.SetCounter, defraimp_counterhistory_defraimp_reason.ValidP1);

            this.auditor.Audit(detail);

            Assert.Equal(new[] { "Create:defraimp_counterhistory" }, this.dataverse.Events);
            Assert.Single(this.dataverse.Creates);

            var history = this.dataverse.Creates[0].ToEntity<defraimp_counterhistory>();
            Assert.Equal(defraimp_counterhistory_defraimp_counterhistorytype.PlaceOfOrigin, history.defraimp_CounterHistoryType);
            Assert.Equal(placeOfOrigin.Id, history.defraimp_PlaceOfOriginId.Id);
            Assert.Null(history.defraimp_AutoNumberId);
        }
    }
}
