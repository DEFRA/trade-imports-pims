namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class CounterTransactionDetailTests
    {
        private readonly Guid importApplicationId = Guid.NewGuid();
        private readonly defraimp_autonumber autonumber = InspectionTestData.Autonumber("counter_key", 1);
        private readonly defraimp_placeoforigin placeOfOrigin = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Bronze);

        [Fact]
        public void ForAutonumber_NullAutonumber_ReturnsNull()
        {
            Assert.Null(CounterTransactionDetail.ForAutonumber(this.importApplicationId, null, defraimp_counterhistory_defraimp_operation.IncrementCounter, defraimp_counterhistory_defraimp_reason.GlobalCounter));
        }

        [Fact]
        public void ForPlaceOfOrigin_NullPlaceOfOrigin_ReturnsNull()
        {
            Assert.Null(CounterTransactionDetail.ForPlaceOfOrigin(this.importApplicationId, null, defraimp_counterhistory_defraimp_operation.IncrementCounter, defraimp_counterhistory_defraimp_reason.GlobalCounter));
        }

        [Fact]
        public void ForAutonumber_Created_HasTheAutoNumberTypeAndTheOperationAndReason()
        {
            var detail = CounterTransactionDetail.ForAutonumber(this.importApplicationId, this.autonumber, defraimp_counterhistory_defraimp_operation.SetCounter, defraimp_counterhistory_defraimp_reason.ValidP2);

            Assert.Equal(defraimp_counterhistory_defraimp_counterhistorytype.AutoNumber, detail.CounterHistoryType);
            Assert.Equal(defraimp_counterhistory_defraimp_operation.SetCounter, detail.Operation);
            Assert.Equal(defraimp_counterhistory_defraimp_reason.ValidP2, detail.Reason);
        }

        [Fact]
        public void PreviousAndCurrentValues_DefaultToZeroAndAreSettable()
        {
            var detail = CounterTransactionDetail.ForAutonumber(this.importApplicationId, this.autonumber, defraimp_counterhistory_defraimp_operation.SetCounter, defraimp_counterhistory_defraimp_reason.ValidP2);

            Assert.Equal(0, detail.PreviousValue);
            Assert.Equal(0, detail.CurrentValue);

            detail.PreviousValue = 3;
            detail.CurrentValue = 4;

            Assert.Equal(3, detail.PreviousValue);
            Assert.Equal(4, detail.CurrentValue);
        }

        [Fact]
        public void GetImportApplicationReference_ReturnsAReferenceToTheImportApplication()
        {
            var detail = CounterTransactionDetail.ForAutonumber(this.importApplicationId, this.autonumber, defraimp_counterhistory_defraimp_operation.SetCounter, defraimp_counterhistory_defraimp_reason.ValidP2);

            var first = detail.GetImportApplicationReference();
            var second = detail.GetImportApplicationReference();

            Assert.Equal(defraimp_importapplication.EntityLogicalName, first.LogicalName);
            Assert.Equal(this.importApplicationId, first.Id);
            Assert.NotSame(first, second);
        }

        [Fact]
        public void GetRelatedRecordReference_AutonumberDetail_ReturnsAReferenceToTheAutonumber()
        {
            var detail = CounterTransactionDetail.ForAutonumber(this.importApplicationId, this.autonumber, defraimp_counterhistory_defraimp_operation.SetCounter, defraimp_counterhistory_defraimp_reason.ValidP2);

            var first = detail.GetRelatedRecordReference();
            var second = detail.GetRelatedRecordReference();

            Assert.Equal(defraimp_autonumber.EntityLogicalName, first.LogicalName);
            Assert.Equal(this.autonumber.Id, first.Id);
            Assert.NotSame(first, second);
        }

        [Fact]
        public void GetRelatedRecordReference_PlaceOfOriginDetail_ReturnsAReferenceToThePlaceOfOrigin()
        {
            var detail = CounterTransactionDetail.ForPlaceOfOrigin(this.importApplicationId, this.placeOfOrigin, defraimp_counterhistory_defraimp_operation.SetCounter, defraimp_counterhistory_defraimp_reason.ValidP2);

            var first = detail.GetRelatedRecordReference();
            var second = detail.GetRelatedRecordReference();

            Assert.Equal(defraimp_placeoforigin.EntityLogicalName, first.LogicalName);
            Assert.Equal(this.placeOfOrigin.Id, first.Id);
            Assert.NotSame(first, second);
        }
    }
}
