namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Records the outcome of an inspection determination on an import application. Each outcome sends an explicit
    /// update, which is sent even when the values are unchanged.
    /// </summary>
    public sealed class InspectionRequirementUpdater
    {
        private readonly IOrganizationService service;

        /// <summary>Initializes a new instance of the <see cref="InspectionRequirementUpdater"/> class.</summary>
        /// <param name="service">The current invocation's organisation service, used for updates.</param>
        public InspectionRequirementUpdater(IOrganizationService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>Records that the verified place of origin is missing.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void PlaceOfOriginMissing(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Discretionary, defraimp_importapplication_defraimp_inspectionrequiredreason.VerifiedPlaceofOriginMissing);
        }

        /// <summary>Records that the risk level is unknown.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void RiskLevelUnknown(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown);
        }

        /// <summary>Records that the primary ITAHC is missing.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void PrimaryItahcMissing(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryITAHCMissing);
        }

        /// <summary>Records that TRACES is disabled, so importer notifications are used instead.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void TracesDisabled(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.TRACESisdisabledUseImporterNotificationsinstead);
        }

        /// <summary>Records that the primary importer notification is missing.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void PrimaryImporterNotificationMissing(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryImporterNotificationMissing);
        }

        /// <summary>Records that no inspection is required.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void NoInspectionRequired(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired);
        }

        /// <summary>Records a random P1 inspection.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void P1Inspection(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection);
        }

        /// <summary>Records a random P2 inspection.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void P2Inspection(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP2Inspection);
        }

        /// <summary>Records a random P3 inspection.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void P3Inspection(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP3Inspection);
        }

        /// <summary>Records an inspection because the place of origin is bronze.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void BronzeInspection(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin);
        }

        /// <summary>Records an inspection because the place of origin is locked to bronze.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void LockedToBronzeInspection(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.PlaceofOriginLockedtoBronze);
        }

        /// <summary>Records an inspection because of the gold place of origin inspection coverage.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void GoldCoverageInspection(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage);
        }

        /// <summary>Records that no inspection is required because the place of origin is gold.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void NoInspectionGold(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.No, defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequiredGoldPlaceofOrigin);
        }

        /// <summary>Records a TB inspection.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        public void TbInspection(Guid importApplicationId)
        {
            this.Update(importApplicationId, defraimp_importapplication_defraimp_inspectionrequired.Yes, defraimp_importapplication_defraimp_inspectionrequiredreason.TB);
        }

        private void Update(
            Guid importApplicationId,
            defraimp_importapplication_defraimp_inspectionrequired inspectionRequired,
            defraimp_importapplication_defraimp_inspectionrequiredreason inspectionRequiredReason)
        {
            var update = new defraimp_importapplication
            {
                Id = importApplicationId,
                defraimp_InspectionRequired = inspectionRequired,
                defraimp_InspectionRequiredReason = inspectionRequiredReason,
                defraimp_InspectionRequiredOriginalValue = (int?)inspectionRequired,
                defraimp_inspectionrequiredreasonoriginalvalue = (int?)inspectionRequiredReason,
            };

            if (inspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.No)
            {
                update.defraimp_InspectionDeclinedReason = "The system has determined that an inspection is not required";
            }

            this.service.Update(update);
        }
    }
}
