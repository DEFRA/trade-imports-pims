namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class RiskCounterManagerTests
    {
        private const string Tag = "DetermineInspectionLogic";
        private const defraimp_counterhistory_defraimp_reason Reason = defraimp_counterhistory_defraimp_reason.ValidP1;

        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly defraimp_importrisklevel p1 = InspectionTestData.RiskLevel("P1");
        private readonly defraimp_importrisklevel p3 = InspectionTestData.RiskLevel("P3");
        private readonly defraimp_importrisklevel tb = InspectionTestData.RiskLevel("TB");
        private readonly EntityReference commodityType = InspectionTestData.CommodityTypeReference();
        private readonly EntityReference country = InspectionTestData.CountryReference();
        private readonly InspectionServices services;

        public RiskCounterManagerTests()
        {
            this.dataverse.Seed(this.p1, this.p3, this.tb);
            this.services = new InspectionServices(this.dataverse);
        }

        [Fact]
        public void AutonumberManager_IncrementNumber_CountsTheRecordAgainstTheRiskLevelAndTheGlobalCounter()
        {
            var record = this.Record(this.p1);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.IncrementNumber(record, Reason);

            Assert.IsType<AutonumberRiskCounterManager>(manager);
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.True(record.ImportRecordCounted);
            Assert.Equal(
                new[] { "Update:defraimp_autonumber", "Create:defraimp_counterhistory", "Update:defraimp_autonumber", "Create:defraimp_counterhistory", "Update:defraimp_importapplication" },
                this.Writes());
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.IncrementCounter, Reason, 0, 1, record);
            this.AssertHistory(1, defraimp_counterhistory_defraimp_operation.IncrementCounter, defraimp_counterhistory_defraimp_reason.GlobalCounter, 0, 1, record);
            Assert.Equal(
                new[] { Tag + ": Info: Increment Global P3 counter", Tag + ": Info: Increment because: " + Reason },
                this.services.Logger.Messages);
        }

        [Fact]
        public void AutonumberManager_IncrementNumberOnAP3Record_DoesNotCountTheGlobalCounterTwice()
        {
            var record = this.Record(this.p3);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.IncrementNumber(record, Reason);

            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.Single(this.dataverse.Creates);
            Assert.True(record.ImportRecordCounted);
        }

        [Fact]
        public void AutonumberManager_IncrementNumberOnACountedRecord_OnlyTraces()
        {
            var record = this.Record(this.p1, counted: true);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.IncrementNumber(record, Reason);

            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
            Assert.Equal(new[] { Tag + ": Info: Increment because: " + Reason }, this.services.Logger.Messages);
        }

        [Fact]
        public void AutonumberManager_DecrementNumber_RemovesTheRecordFromTheCountsAndFlagsItNotCounted()
        {
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 1);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);
            var record = this.Record(this.p1, counted: true);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.DecrementNumber(record, defraimp_counterhistory_defraimp_reason.RecordDeactivated);

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.False(record.ImportRecordCounted);
            Assert.Single(this.dataverse.UpdatesOf(defraimp_importapplication.EntityLogicalName));
            Assert.False(this.dataverse.UpdatesOf(defraimp_importapplication.EntityLogicalName)[0].ToEntity<defraimp_importapplication>().defraimp_ImportRecordCounted);
            this.AssertHistory(1, defraimp_counterhistory_defraimp_operation.DecrementCounter, defraimp_counterhistory_defraimp_reason.GlobalCounter, 1, 0, record);
        }

        [Fact]
        public void AutonumberManager_DecrementNumber_ReadsThePreviousAndCurrentValuesFromTheQuotaCounter()
        {
            // TODO: the history of the decrement holds the quota counter values, not the counter that changed. This is preserved behaviour.
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 5);
            this.dataverse.SetCounter(InspectionConstants.P1QuotaCounterName, 3);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);
            var record = this.Record(this.p1, counted: true);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.DecrementNumber(record, defraimp_counterhistory_defraimp_reason.RecordDeactivated);

            Assert.Equal(4, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.DecrementCounter, defraimp_counterhistory_defraimp_reason.RecordDeactivated, 3, 3, record);
        }

        [Fact]
        public void AutonumberManager_DecrementNumberOfAnInspectedRecord_IncreasesTheQuotaInsteadOfDecreasingTheCounter()
        {
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 2);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);
            var record = this.Record(this.p1, counted: true, inspectionRequired: defraimp_importapplication_defraimp_inspectionrequired.Yes);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.DecrementNumber(record, defraimp_counterhistory_defraimp_reason.RecordDeactivated);

            Assert.Equal(2, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P1QuotaCounterName));
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.IncrementQuota, defraimp_counterhistory_defraimp_reason.RecordDeactivated, 0, 1, record);
        }

        [Fact]
        public void AutonumberManager_DecrementNumberOnAnUncountedRecord_OnlyTraces()
        {
            var record = this.Record(this.p1);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.DecrementNumber(record, defraimp_counterhistory_defraimp_reason.RecordDeactivated);

            Assert.Empty(this.dataverse.Updates);
            Assert.Equal(new[] { Tag + ": Info: Decrement because: " + defraimp_counterhistory_defraimp_reason.RecordDeactivated }, this.services.Logger.Messages);
        }

        [Fact]
        public void AutonumberManager_DecrementNumberWithQuotaAndALowCounter_BalancesTheQuota()
        {
            this.dataverse.Seed(InspectionTestData.CoverageRule("balance", 2, this.p1.Id));
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, -1);
            this.dataverse.SetCounter(InspectionConstants.P1QuotaCounterName, 1);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);
            var record = this.Record(this.p1, counted: true);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.DecrementNumber(record, defraimp_counterhistory_defraimp_reason.RecordDeactivated);

            // The counter reaches -2, the negative threshold, so a quota is taken and the counter is reset.
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1QuotaCounterName));
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Contains(this.Histories(), history => history.defraimp_Reason == defraimp_counterhistory_defraimp_reason.NegativeThresholdReachedBalanceQuota && history.defraimp_Operation == defraimp_counterhistory_defraimp_operation.DecrementQuota);
            Assert.Contains(this.Histories(), history => history.defraimp_Reason == defraimp_counterhistory_defraimp_reason.NegativeThresholdReachedBalanceQuota && history.defraimp_Operation == defraimp_counterhistory_defraimp_operation.SetCounter);
        }

        [Fact]
        public void AutonumberManager_SetNumberValue_SetsTheCounterAndRecordsItsHistory()
        {
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 4);
            var record = this.Record(this.p1);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.SetNumberValue(record, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection, 0);

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.SetCounter, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection, 4, 0, record);
            Assert.Equal(Tag + ": Info: Set number because: " + defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection, Assert.Single(this.services.Logger.Messages));
        }

        [Fact]
        public void AutonumberManager_IncrementQuota_IncreasesTheQuotaAndRecordsItsHistory()
        {
            var record = this.Record(this.p1);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.IncrementQuota(record, Reason);

            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P1QuotaCounterName));
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.IncrementQuota, Reason, 0, 1, record);
        }

        [Fact]
        public void AutonumberManager_DecrementQuota_FlagsTheRecordCountedBeforeItsHistoryIsWritten()
        {
            this.dataverse.SetCounter(InspectionConstants.P1QuotaCounterName, 2);
            var record = this.Record(this.p1);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.DecrementQuota(record, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection);

            // TODO: taking a quota flags the record as counted. This is preserved behaviour.
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P1QuotaCounterName));
            Assert.True(record.ImportRecordCounted);
            Assert.Equal(new[] { "Update:defraimp_autonumber", "Update:defraimp_importapplication", "Create:defraimp_counterhistory" }, this.Writes());
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.DecrementQuota, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection, 2, 1, record);
        }

        [Fact]
        public void AutonumberManager_SetQuotaValue_SetsTheQuotaAndRecordsItsHistory()
        {
            this.dataverse.SetCounter(InspectionConstants.P1QuotaCounterName, 3);
            var record = this.Record(this.p1);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.SetQuotaValue(record, Reason, 0);

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1QuotaCounterName));
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.SetQuota, Reason, 3, 0, record);
        }

        [Fact]
        public void OnlyGlobalManager_IncrementNumber_CountsTheGlobalCounterAndFlagsTheRecordCounted()
        {
            var record = this.Record(this.tb);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.IncrementNumber(record, Reason);

            Assert.IsType<OnlyGlobalCounterRiskCounterManager>(manager);
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.True(record.ImportRecordCounted);
            Assert.Equal(
                new[] { "Update:defraimp_autonumber", "Create:defraimp_counterhistory", "Update:defraimp_importapplication" },
                this.Writes());
        }

        [Fact]
        public void OnlyGlobalManager_DecrementNumber_RemovesTheRecordFromTheGlobalCounter()
        {
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);
            var record = this.Record(this.tb, counted: true);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.DecrementNumber(record, defraimp_counterhistory_defraimp_reason.RecordDeactivated);

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.False(record.ImportRecordCounted);
        }

        [Fact]
        public void OnlyGlobalManager_IncrementAndDecrementOfAnUnchangedState_OnlyTrace()
        {
            var counted = this.Record(this.tb, counted: true);
            var uncounted = this.Record(this.tb);
            var manager = this.services.CounterManagerFactory.Create(counted);

            manager.IncrementNumber(counted, Reason);
            manager.DecrementNumber(uncounted, Reason);

            Assert.Empty(this.dataverse.Updates);
            Assert.Equal(2, this.services.Logger.Messages.Count);
        }

        [Fact]
        public void OnlyGlobalManager_QuotaAndSetOperations_OnlyTrace()
        {
            var record = this.Record(this.tb);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.SetNumberValue(record, Reason, 0);
            manager.IncrementQuota(record, Reason);
            manager.DecrementQuota(record, Reason);
            manager.SetQuotaValue(record, Reason, 0);

            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
            Assert.Equal(4, this.services.Logger.Messages.Count);
        }

        [Fact]
        public void PlaceOfOriginManager_IncrementNumber_CountsAgainstThePlaceOfOriginAndTheGlobalCounter()
        {
            var place = this.GoldBronzePlace(defraimp_trustlevel.Gold, 1, 0);
            var record = this.Record(this.p1, place);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.IncrementNumber(record, Reason);

            Assert.IsType<PlaceOfOriginRiskCounterManager>(manager);
            Assert.Equal(2, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_ApplicationCounter);
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.True(record.ImportRecordCounted);
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.IncrementCounter, Reason, 1, 2, record, place.Id);
            Assert.Equal(defraimp_counterhistory_defraimp_counterhistorytype.PlaceOfOrigin, this.Histories()[0].defraimp_CounterHistoryType);
        }

        [Fact]
        public void PlaceOfOriginManager_DecrementNumber_RemovesTheRecordFromThePlaceOfOriginAndTheGlobalCounter()
        {
            var place = this.GoldBronzePlace(defraimp_trustlevel.Gold, 1, 0);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);
            var record = this.Record(this.p1, place, counted: true);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.DecrementNumber(record, defraimp_counterhistory_defraimp_reason.RecordDeactivated);

            Assert.Equal(0, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_ApplicationCounter);
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.False(record.ImportRecordCounted);
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.DecrementCounter, defraimp_counterhistory_defraimp_reason.RecordDeactivated, 1, 0, record, place.Id);
        }

        [Fact]
        public void PlaceOfOriginManager_DecrementNumberOfARecordInspectedForGoldCoverage_IncreasesTheQuotaInstead()
        {
            var place = this.GoldBronzePlace(defraimp_trustlevel.Gold, 1, 0);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);
            var record = this.Record(this.p1, place, counted: true, inspectionRequiredReason: defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.DecrementNumber(record, defraimp_counterhistory_defraimp_reason.RecordDeactivated);

            Assert.Equal(1, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_ApplicationCounter);
            Assert.Equal(1, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_InspectionQuotaCounter);
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
        }

        [Fact]
        public void PlaceOfOriginManager_DecrementNumberWithQuotaAndALowCounter_BalancesTheQuota()
        {
            var place = this.GoldBronzePlace(defraimp_trustlevel.Gold, -1, 1);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);
            var record = this.Record(this.p1, place, counted: true);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.DecrementNumber(record, defraimp_counterhistory_defraimp_reason.RecordDeactivated);

            Assert.Equal(0, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_InspectionQuotaCounter);
            Assert.Equal(0, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_ApplicationCounter);
        }

        [Fact]
        public void PlaceOfOriginManager_QuotaOperations_ChangeThePlaceOfOriginAndRecordTheirHistory()
        {
            var place = this.GoldBronzePlace(defraimp_trustlevel.Gold, 0, 2);
            var record = this.Record(this.p1, place);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.IncrementQuota(record, Reason);
            manager.DecrementQuota(record, Reason);

            Assert.Equal(2, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_InspectionQuotaCounter);

            // TODO: taking a quota flags the record as counted. This is preserved behaviour.
            Assert.True(record.ImportRecordCounted);
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.IncrementQuota, Reason, 2, 3, record, place.Id);
            this.AssertHistory(1, defraimp_counterhistory_defraimp_operation.DecrementQuota, Reason, 3, 2, record, place.Id);
        }

        [Fact]
        public void PlaceOfOriginManager_SetNumberValue_SetsTheApplicationCounter()
        {
            var place = this.GoldBronzePlace(defraimp_trustlevel.Gold, 3, 0);
            var record = this.Record(this.p1, place);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.SetNumberValue(record, Reason, 0);

            Assert.Equal(0, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_ApplicationCounter);
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.SetCounter, Reason, 3, 0, record, place.Id);
        }

        [Fact]
        public void PlaceOfOriginManager_SetQuotaValue_SetsTheApplicationCounterNotTheQuota()
        {
            // TODO: setting the quota sets the application counter. This is preserved behaviour.
            var place = this.GoldBronzePlace(defraimp_trustlevel.Gold, 3, 2);
            var record = this.Record(this.p1, place);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.SetQuotaValue(record, Reason, 0);

            Assert.Equal(0, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_ApplicationCounter);
            Assert.Equal(2, this.dataverse.Stored<defraimp_placeoforigin>(place.Id).defraimp_InspectionQuotaCounter);
            this.AssertHistory(0, defraimp_counterhistory_defraimp_operation.SetQuota, Reason, 2, 2, record, place.Id);
        }

        [Fact]
        public void PlaceOfOriginManager_WithoutAPlaceOfOrigin_OnlyTraces()
        {
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, this.country));
            var record = this.Record(this.p1);
            var manager = this.services.CounterManagerFactory.Create(record);

            manager.IncrementNumber(record, Reason);
            manager.DecrementNumber(record, Reason);
            manager.SetNumberValue(record, Reason, 0);
            manager.IncrementQuota(record, Reason);
            manager.DecrementQuota(record, Reason);
            manager.SetQuotaValue(record, Reason, 0);

            Assert.IsType<PlaceOfOriginRiskCounterManager>(manager);
            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
            Assert.Equal(6, this.services.Logger.Messages.Count);
        }

        [Fact]
        public void IncrementGlobalCounter_WithoutAP3Autonumber_FailsBeforeAnythingIsWritten()
        {
            var withoutP3 = new InspectionDataverse().Seed(this.tb);
            var withoutP3Services = new InspectionServices(withoutP3);
            var record = this.Record(this.tb);
            var manager = withoutP3Services.CounterManagerFactory.Create(record);

            var exception = Assert.Throws<InvalidPluginExecutionException>(() => manager.IncrementGlobalCounter(record));

            Assert.Equal("Autonumber record key not found.", exception.Message);
            Assert.Empty(withoutP3.Updates);
        }

        private InspectionRecord Record(
            defraimp_importrisklevel riskLevel,
            defraimp_placeoforigin place = null,
            bool counted = false,
            defraimp_importapplication_defraimp_inspectionrequired? inspectionRequired = null,
            defraimp_importapplication_defraimp_inspectionrequiredreason? inspectionRequiredReason = null)
        {
            return new InspectionRecord(InspectionTestData.Application(
                riskLevel: InspectionTestData.RiskLevelReference(riskLevel),
                placeOfOrigin: place != null ? InspectionTestData.PlaceOfOriginReference(place) : null,
                commodityType: this.commodityType,
                country: this.country,
                counted: counted,
                inspectionRequired: inspectionRequired,
                inspectionRequiredReason: inspectionRequiredReason));
        }

        private defraimp_placeoforigin GoldBronzePlace(defraimp_trustlevel trustLevel, int applicationCounter, int quotaCounter)
        {
            var place = InspectionTestData.PlaceOfOrigin(trustLevel, applicationCounter, quotaCounter);
            this.dataverse.Seed(place);
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, this.country));
            return place;
        }

        private string[] Writes()
        {
            return this.dataverse.Events.Where(entry => !entry.StartsWith("Query:", StringComparison.Ordinal)).ToArray();
        }

        private IReadOnlyList<defraimp_counterhistory> Histories()
        {
            return this.dataverse.Creates.Select(entity => entity.ToEntity<defraimp_counterhistory>()).ToList();
        }

        private void AssertHistory(
            int index,
            defraimp_counterhistory_defraimp_operation operation,
            defraimp_counterhistory_defraimp_reason reason,
            int previousValue,
            int currentValue,
            InspectionRecord record,
            Guid? placeOfOriginId = null)
        {
            var history = this.Histories()[index];

            Assert.Equal(operation, history.defraimp_Operation);
            Assert.Equal(reason, history.defraimp_Reason);
            Assert.Equal(previousValue, history.defraimp_PreviousValue);
            Assert.Equal(currentValue, history.defraimp_CurrentValue);
            Assert.Equal(record.Id, history.defraimp_ImportApplicationId.Id);
            if (placeOfOriginId.HasValue)
            {
                Assert.Equal(placeOfOriginId.Value, history.defraimp_PlaceOfOriginId.Id);
                Assert.Null(history.defraimp_AutoNumberId);
            }
            else
            {
                Assert.NotNull(history.defraimp_AutoNumberId);
                Assert.Null(history.defraimp_PlaceOfOriginId);
            }
        }
    }
}
