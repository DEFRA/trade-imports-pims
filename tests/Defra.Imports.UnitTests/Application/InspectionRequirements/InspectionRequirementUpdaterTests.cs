namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class InspectionRequirementUpdaterTests
    {
        private const string DeclinedReason = "The system has determined that an inspection is not required";

        public enum Outcome
        {
            PlaceOfOriginMissing,
            RiskLevelUnknown,
            PrimaryItahcMissing,
            TracesDisabled,
            PrimaryImporterNotificationMissing,
            NoInspectionRequired,
            P1Inspection,
            P2Inspection,
            P3Inspection,
            BronzeInspection,
            LockedToBronzeInspection,
            GoldCoverageInspection,
            NoInspectionGold,
            TbInspection,
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new InspectionRequirementUpdater(null));
        }

        [Theory]
        [InlineData(Outcome.PlaceOfOriginMissing, defraimp_importapplication_defraimp_inspectionrequired.Discretionary, defraimp_importapplication_defraimp_inspectionrequiredreason.VerifiedPlaceofOriginMissing)]
        [InlineData(Outcome.RiskLevelUnknown, defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown)]
        [InlineData(Outcome.PrimaryItahcMissing, defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryITAHCMissing)]
        [InlineData(Outcome.TracesDisabled, defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.TRACESisdisabledUseImporterNotificationsinstead)]
        [InlineData(Outcome.PrimaryImporterNotificationMissing, defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryImporterNotificationMissing)]
        [InlineData(Outcome.NoInspectionRequired, defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired)]
        [InlineData(Outcome.P1Inspection, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection)]
        [InlineData(Outcome.P2Inspection, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP2Inspection)]
        [InlineData(Outcome.P3Inspection, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP3Inspection)]
        [InlineData(Outcome.BronzeInspection, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin)]
        [InlineData(Outcome.LockedToBronzeInspection, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.PlaceofOriginLockedtoBronze)]
        [InlineData(Outcome.GoldCoverageInspection, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage)]
        [InlineData(Outcome.NoInspectionGold, defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequiredGoldPlaceofOrigin)]
        [InlineData(Outcome.TbInspection, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.TB)]
        public void Outcome_SendsOneUpdateWithTheRequiredAndReason(
            Outcome outcome,
            defraimp_importapplication_defraimp_inspectionrequired required,
            defraimp_importapplication_defraimp_inspectionrequiredreason reason)
        {
            var dataverse = new InspectionDataverse();
            var updater = new InspectionRequirementUpdater(dataverse.Service.Object);
            var importApplicationId = Guid.NewGuid();

            Invoke(updater, outcome, importApplicationId);

            Assert.Equal(new[] { "Update:defraimp_importapplication" }, dataverse.Events);
            Assert.Single(dataverse.Updates);

            var update = dataverse.Updates[0].ToEntity<defraimp_importapplication>();
            Assert.Equal(importApplicationId, update.Id);
            Assert.Equal(required, update.defraimp_InspectionRequired);
            Assert.Equal(reason, update.defraimp_InspectionRequiredReason);
            Assert.Equal((int)required, update.defraimp_InspectionRequiredOriginalValue);
            Assert.Equal((int)reason, update.defraimp_inspectionrequiredreasonoriginalvalue);

            if (required == defraimp_importapplication_defraimp_inspectionrequired.No)
            {
                Assert.Equal(DeclinedReason, update.defraimp_InspectionDeclinedReason);
            }
            else
            {
                Assert.Null(update.defraimp_InspectionDeclinedReason);
            }
        }

        private static void Invoke(InspectionRequirementUpdater updater, Outcome outcome, Guid importApplicationId)
        {
            switch (outcome)
            {
                case Outcome.PlaceOfOriginMissing:
                    updater.PlaceOfOriginMissing(importApplicationId);
                    break;
                case Outcome.RiskLevelUnknown:
                    updater.RiskLevelUnknown(importApplicationId);
                    break;
                case Outcome.PrimaryItahcMissing:
                    updater.PrimaryItahcMissing(importApplicationId);
                    break;
                case Outcome.TracesDisabled:
                    updater.TracesDisabled(importApplicationId);
                    break;
                case Outcome.PrimaryImporterNotificationMissing:
                    updater.PrimaryImporterNotificationMissing(importApplicationId);
                    break;
                case Outcome.NoInspectionRequired:
                    updater.NoInspectionRequired(importApplicationId);
                    break;
                case Outcome.P1Inspection:
                    updater.P1Inspection(importApplicationId);
                    break;
                case Outcome.P2Inspection:
                    updater.P2Inspection(importApplicationId);
                    break;
                case Outcome.P3Inspection:
                    updater.P3Inspection(importApplicationId);
                    break;
                case Outcome.BronzeInspection:
                    updater.BronzeInspection(importApplicationId);
                    break;
                case Outcome.LockedToBronzeInspection:
                    updater.LockedToBronzeInspection(importApplicationId);
                    break;
                case Outcome.GoldCoverageInspection:
                    updater.GoldCoverageInspection(importApplicationId);
                    break;
                case Outcome.NoInspectionGold:
                    updater.NoInspectionGold(importApplicationId);
                    break;
                case Outcome.TbInspection:
                    updater.TbInspection(importApplicationId);
                    break;
                default:
                    throw new InvalidOperationException("Unknown outcome " + outcome + ".");
            }
        }
    }
}
