namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class InspectionRequirementDeterminerTests
    {
        private readonly InspectionDataverse dataverse = new InspectionDataverse().WithReferenceData();
        private readonly defraimp_importrisklevel p1 = InspectionTestData.RiskLevel("P1");
        private readonly defraimp_importrisklevel p2 = InspectionTestData.RiskLevel("P2");
        private readonly EntityReference commodityType = InspectionTestData.CommodityTypeReference();
        private readonly EntityReference country = InspectionTestData.CountryReference();
        private readonly InspectionServices services;

        public InspectionRequirementDeterminerTests()
        {
            this.dataverse.Seed(this.p1, this.p2);
            this.services = new InspectionServices(this.dataverse);
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionRequirementDeterminer(null, this.services.PlaceOfOriginCounters, this.services.CounterManagerFactory, this.services.StrategyFactory, this.dataverse.Service.Object, this.services.Logger));
        }

        [Fact]
        public void Constructor_NullPlaceOfOriginCounters_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionRequirementDeterminer(this.services.Queries, null, this.services.CounterManagerFactory, this.services.StrategyFactory, this.dataverse.Service.Object, this.services.Logger));
        }

        [Fact]
        public void Constructor_NullCounterManagerFactory_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionRequirementDeterminer(this.services.Queries, this.services.PlaceOfOriginCounters, null, this.services.StrategyFactory, this.dataverse.Service.Object, this.services.Logger));
        }

        [Fact]
        public void Constructor_NullStrategyFactory_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionRequirementDeterminer(this.services.Queries, this.services.PlaceOfOriginCounters, this.services.CounterManagerFactory, null, this.dataverse.Service.Object, this.services.Logger));
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionRequirementDeterminer(this.services.Queries, this.services.PlaceOfOriginCounters, this.services.CounterManagerFactory, this.services.StrategyFactory, null, this.services.Logger));
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionRequirementDeterminer(this.services.Queries, this.services.PlaceOfOriginCounters, this.services.CounterManagerFactory, this.services.StrategyFactory, this.dataverse.Service.Object, null));
        }

        [Fact]
        public void Determine_NeitherBeforeNorAfter_DoesNothing()
        {
            this.services.Determiner.Determine(null, null);

            Assert.Empty(this.dataverse.Events);
        }

        [Fact]
        public void Determine_AfterOnly_CountsTheRecordAndRecordsTheOutcome()
        {
            this.services.Determiner.Determine(null, this.Application(this.p1, commodity: true));

            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired);
        }

        [Fact]
        public void Determine_AfterOnlyWithoutARiskLevel_RecordsTheRiskLevelAsUnknown()
        {
            this.services.Determiner.Determine(null, InspectionTestData.Application());

            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown);
            Assert.Empty(this.dataverse.Creates);
        }

        [Fact]
        public void Determine_AfterOnlyWithAnUnrecognisedRiskLevel_RecordsTheRiskLevelAsUnknown()
        {
            var other = InspectionTestData.RiskLevel("N/A");
            this.dataverse.Seed(other);

            this.services.Determiner.Determine(null, this.Application(other));

            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown);
        }

        [Fact]
        public void Determine_ActiveUpdateWithAManualPostImportCheckDecision_WritesNothing()
        {
            var before = this.Application(this.p1, counted: true);
            var after = this.Application(this.p2, counted: true, decision: defraimp_importapplication_defraimp_manualpostimportcheckdecision.ManualCheckOther);

            this.services.Determiner.Determine(before, after);

            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
        }

        [Fact]
        public void Determine_RiskLevelChange_RemovesTheRecordFromTheOldCountsAndCountsItAgainstTheNewRiskLevel()
        {
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 1);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);

            this.services.Determiner.Determine(this.Application(this.p1, counted: true), this.Application(this.p2, counted: true));

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P2CounterName));
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.Equal(defraimp_counterhistory_defraimp_reason.RiskLevelChangedRemoved, this.Histories()[0].defraimp_Reason);
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired);
        }

        [Fact]
        public void Determine_RiskLevelChangeFromNoRiskLevel_TracesAndDeterminesTheInspection()
        {
            this.services.Determiner.Determine(InspectionTestData.Application(counted: false), this.Application(this.p1, commodity: true));

            Assert.Contains("DetermineInspectionLogic: Info: Null string", this.services.Logger.Messages);
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
        }

        [Fact]
        public void Determine_UnchangedRiskLevelOnACountedRecord_ReadsTheRiskLevelByIdentifierAndWritesNothing()
        {
            this.services.Determiner.Determine(this.Application(this.p1, counted: true), this.Application(this.p1, counted: true));

            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);

            // The risk level of the before values is read to find its name, and again to create the counter manager.
            Assert.True(this.dataverse.Events.Count(entry => entry == "Query:defraimp_importrisklevel") >= 3);
        }

        [Fact]
        public void Determine_ItahcRemoved_RemovesTheRecordFromTheCountsAndRecordsThePrimaryItahcAsMissing()
        {
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 1);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);

            this.services.Determiner.Determine(this.Application(this.p1, counted: true), this.Application(this.p1, counted: true, hasPrimaryItahc: false));

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.Equal(defraimp_counterhistory_defraimp_reason.ITAHCRemoved, this.Histories()[0].defraimp_Reason);
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryITAHCMissing);
        }

        [Fact]
        public void Determine_ImporterNotificationRemoved_RemovesTheRecordFromTheCountsAndRecordsTheNotificationAsMissing()
        {
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 1);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);
            var before = this.Application(this.p1, counted: true, type: defraimp_importapplication_defraimp_importapplicationtype.ImportNotification, hasPrimaryItahc: false, hasPrimaryNotification: true);
            var after = this.Application(this.p1, counted: true, type: defraimp_importapplication_defraimp_importapplicationtype.ImportNotification, hasPrimaryItahc: false, hasPrimaryNotification: false);

            this.services.Determiner.Determine(before, after);

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(defraimp_counterhistory_defraimp_reason.ImporterNotificationRemoved, this.Histories()[0].defraimp_Reason);
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryImporterNotificationMissing);
        }

        [Fact]
        public void Determine_PrimaryCertificateRemovedFromACHEDARecord_DoesNotRemoveItFromTheCounts()
        {
            var before = this.Application(this.p1, counted: true, type: defraimp_importapplication_defraimp_importapplicationtype.CHEDA, hasPrimaryItahc: true);
            var after = this.Application(this.p1, counted: true, type: defraimp_importapplication_defraimp_importapplicationtype.CHEDA, hasPrimaryItahc: false);

            this.services.Determiner.Determine(before, after);

            Assert.Empty(this.dataverse.Updates);
        }

        [Fact]
        public void Determine_PlaceOfOriginChangedOnAGoldBronzeP1Record_RemovesTheRecordFromThePreviousPlaceAndDeterminesTheInspection()
        {
            var gold = this.GoldBronzePlace(defraimp_trustlevel.Gold, applicationCounter: 1);
            var bronze = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Bronze);
            this.dataverse.Seed(bronze);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);

            this.services.Determiner.Determine(
                this.Application(this.p1, counted: true, place: gold, commodity: true),
                this.Application(this.p1, counted: true, place: bronze, commodity: true));

            Assert.Equal(0, this.dataverse.Stored<defraimp_placeoforigin>(gold.Id).defraimp_ApplicationCounter);
            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.Equal(defraimp_counterhistory_defraimp_reason.GBPlaceOfOriginRemoved, this.Histories()[0].defraimp_Reason);
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin);
        }

        [Fact]
        public void Determine_PreviousBronzePlaceOfOrigin_DecrementsTheGlobalCounterOnly()
        {
            var bronze = this.GoldBronzePlace(defraimp_trustlevel.Bronze, applicationCounter: 4);
            var gold = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold);
            this.dataverse.Seed(gold);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);

            this.services.Determiner.Determine(
                this.Application(this.p1, counted: true, place: bronze, commodity: true),
                this.Application(this.p1, counted: true, place: gold, commodity: true));

            Assert.Equal(4, this.dataverse.Stored<defraimp_placeoforigin>(bronze.Id).defraimp_ApplicationCounter);
            Assert.Contains(this.Histories(), history => history.defraimp_Reason == defraimp_counterhistory_defraimp_reason.GlobalCounter && history.defraimp_Operation == defraimp_counterhistory_defraimp_operation.DecrementCounter);
        }

        [Fact]
        public void Determine_PlaceOfOriginReferencedByTheSameObject_IsNotTreatedAsChanged()
        {
            var gold = this.GoldBronzePlace(defraimp_trustlevel.Gold, applicationCounter: 1);
            var shared = InspectionTestData.PlaceOfOriginReference(gold);
            var before = InspectionTestData.Application(riskLevel: InspectionTestData.RiskLevelReference(this.p1), placeOfOrigin: shared, commodityType: this.commodityType, country: this.country, counted: true);
            var after = InspectionTestData.Application(riskLevel: InspectionTestData.RiskLevelReference(this.p1), placeOfOrigin: shared, commodityType: this.commodityType, country: this.country, counted: true);

            this.services.Determiner.Determine(before, after);

            Assert.Empty(this.dataverse.Updates);
        }

        [Fact]
        public void Determine_PlaceOfOriginWithTheSameIdentifierInAnotherReference_IsTreatedAsChanged()
        {
            // TODO: the references are compared by object, so an unchanged place of origin counts as changed. This is preserved behaviour.
            var gold = this.GoldBronzePlace(defraimp_trustlevel.Gold, applicationCounter: 1);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);

            this.services.Determiner.Determine(
                this.Application(this.p1, counted: true, place: gold, commodity: true),
                this.Application(this.p1, counted: true, place: gold, commodity: true));

            Assert.Contains(this.Histories(), history => history.defraimp_Reason == defraimp_counterhistory_defraimp_reason.GBPlaceOfOriginRemoved);
        }

        [Fact]
        public void Determine_PlaceOfOriginChangedOnANonGoldBronzeRecord_RemovesNothing()
        {
            this.dataverse.Seed(InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold));

            this.services.Determiner.Determine(
                this.Application(this.p1, counted: true, place: InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold)),
                this.Application(this.p1, counted: true, place: InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Gold)));

            Assert.Empty(this.dataverse.Updates);
        }

        [Theory]
        [InlineData(defraimp_importapplication_statuscode.Cancelled)]
        [InlineData(defraimp_importapplication_statuscode.NoITAHCReceived)]
        public void Determine_ActiveRecordDeactivated_RemovesItFromTheCounts(defraimp_importapplication_statuscode status)
        {
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 1);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);

            this.services.Determiner.Determine(
                this.Application(this.p1, counted: true),
                this.Application(this.p1, counted: true, stateCode: defraimp_importapplication_statecode.Inactive, statusCode: status));

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.Equal(defraimp_counterhistory_defraimp_reason.RecordDeactivated, this.Histories()[0].defraimp_Reason);
        }

        [Theory]
        [InlineData(defraimp_importapplication_statuscode.Cancelled)]
        [InlineData(defraimp_importapplication_statuscode.NoITAHCReceived)]
        public void Determine_RecordReactivated_DeterminesTheInspectionAgain(defraimp_importapplication_statuscode status)
        {
            this.services.Determiner.Determine(
                this.Application(this.p1, stateCode: defraimp_importapplication_statecode.Inactive, statusCode: status),
                this.Application(this.p1, commodity: true));

            Assert.Equal(1, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired);
        }

        [Fact]
        public void Determine_InactiveRecordThatStaysInactive_WritesNothing()
        {
            this.services.Determiner.Determine(
                this.Application(this.p1, counted: true, stateCode: defraimp_importapplication_statecode.Inactive, statusCode: defraimp_importapplication_statuscode.Cancelled),
                this.Application(this.p1, counted: true, stateCode: defraimp_importapplication_statecode.Inactive, statusCode: defraimp_importapplication_statuscode.Cancelled));

            Assert.Empty(this.dataverse.Updates);
            Assert.Empty(this.dataverse.Creates);
        }

        [Fact]
        public void Determine_BeforeOnly_RemovesTheRecordFromTheCounts()
        {
            this.dataverse.SetCounter(InspectionConstants.P1CounterName, 1);
            this.dataverse.SetCounter(InspectionConstants.P3CounterName, 1);

            this.services.Determiner.Determine(this.Application(this.p1, counted: true), null);

            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P1CounterName));
            Assert.Equal(0, this.dataverse.CounterValue(InspectionConstants.P3CounterName));
            Assert.Equal(defraimp_counterhistory_defraimp_reason.RecordDeactivated, this.Histories()[0].defraimp_Reason);
        }

        [Fact]
        public void Determine_BeforeOnlyWithoutARiskLevel_FailsWithANullReference()
        {
            // TODO: a record with no risk level has no counter manager, which fails. This is preserved behaviour.
            Assert.Throws<NullReferenceException>(() => this.services.Determiner.Determine(InspectionTestData.Application(counted: true), null));
        }

        [Fact]
        public void Determine_DeactivationWithoutARiskLevel_FailsWithANullReference()
        {
            var before = InspectionTestData.Application(counted: true);
            var after = InspectionTestData.Application(stateCode: defraimp_importapplication_statecode.Inactive, statusCode: defraimp_importapplication_statuscode.Cancelled);

            Assert.Throws<NullReferenceException>(() => this.services.Determiner.Determine(before, after));
        }

        [Fact]
        public void Determine_DoesNotModifyTheValuesItIsGiven()
        {
            var before = this.Application(this.p1, counted: true);
            var after = this.Application(this.p2, counted: true);

            this.services.Determiner.Determine(before, after);

            Assert.True(before.ImportRecordCounted);
            Assert.True(after.ImportRecordCounted);
        }

        [Fact]
        public void DetermineForImportApplication_StoredApplication_RetrievesItAndDeterminesItAsNew()
        {
            var stored = new defraimp_importapplication { Id = Guid.NewGuid() };
            this.dataverse.Seed(stored);

            this.services.Determiner.DetermineForImportApplication(stored.Id);

            Assert.Equal("Retrieve:defraimp_importapplication", this.dataverse.Events[0]);
            this.AssertOutcome(defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown);
        }

        private InspectionImportApplicationContext Application(
            defraimp_importrisklevel riskLevel,
            bool counted = false,
            bool commodity = false,
            defraimp_placeoforigin place = null,
            defraimp_importapplication_defraimp_importapplicationtype? type = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
            bool hasPrimaryItahc = true,
            bool hasPrimaryNotification = false,
            defraimp_importapplication_statecode? stateCode = defraimp_importapplication_statecode.Active,
            defraimp_importapplication_statuscode? statusCode = null,
            defraimp_importapplication_defraimp_manualpostimportcheckdecision? decision = defraimp_importapplication_defraimp_manualpostimportcheckdecision.UseSystemDecision)
        {
            return InspectionTestData.Application(
                type: type,
                riskLevel: InspectionTestData.RiskLevelReference(riskLevel),
                placeOfOrigin: place != null ? InspectionTestData.PlaceOfOriginReference(place) : null,
                commodityType: commodity || place != null ? this.commodityType : null,
                country: commodity || place != null ? this.country : null,
                hasPrimaryItahc: hasPrimaryItahc,
                hasPrimaryNotification: hasPrimaryNotification,
                counted: counted,
                stateCode: stateCode,
                statusCode: statusCode,
                decision: decision);
        }

        private defraimp_placeoforigin GoldBronzePlace(defraimp_trustlevel trustLevel, int applicationCounter)
        {
            var place = InspectionTestData.PlaceOfOrigin(trustLevel, applicationCounter);
            this.dataverse.Seed(place);
            this.dataverse.Seed(InspectionTestData.GoldBronzeRules(this.commodityType, this.country));
            return place;
        }

        private IReadOnlyList<defraimp_counterhistory> Histories()
        {
            return this.dataverse.Creates.Select(entity => entity.ToEntity<defraimp_counterhistory>()).ToList();
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
        }
    }
}
