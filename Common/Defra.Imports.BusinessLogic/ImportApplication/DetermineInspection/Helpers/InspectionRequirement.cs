using Defra.Imports.Model;
using Defra.Imports.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers
{
    /// <summary>
    /// This class contains all the required data to perform an inspection outcome update, and provides methods to select the one we want.
    /// </summary>
    public class InspectionRequirement
    {
        public defraimp_importapplication ImportApplication { get; private set; }

        public ICrmRepository<defraimp_importapplication> ImportApplicationRepo { get; private set; }

        public defraimp_importapplication_defraimp_inspectionrequired InspectionRequired { get; private set; }

        public defraimp_importapplication_defraimp_inspectionrequiredreason InspectionRequiredReason { get; private set; }

        public InspectionRequirement(defraimp_importapplication importApplication, ICrmRepository<defraimp_importapplication> importApplicationRepo)
        {
            this.ImportApplication = importApplication;
            this.ImportApplicationRepo = importApplicationRepo;
        }

        private void PerformInspectionRequiredUpdate()
        {
            defraimp_importapplication importApplicationUpdate = new defraimp_importapplication()
            {
                Id = ImportApplication.Id,
                defraimp_InspectionRequired = InspectionRequired,
                defraimp_InspectionRequiredReason = InspectionRequiredReason,
                defraimp_InspectionRequiredOriginalValue = (int?)InspectionRequired,
                defraimp_inspectionrequiredreasonoriginalvalue = (int?)InspectionRequiredReason
            };

            if (InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.No)
            {
                importApplicationUpdate.defraimp_InspectionDeclinedReason = "The system has determined that an inspection is not required";
            }

            ImportApplicationRepo.Update(importApplicationUpdate);
        }

        //Requirements - Missing Data
        public void PlaceOfOriginMissing()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Discretionary;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.VerifiedPlaceofOriginMissing;
            PerformInspectionRequiredUpdate();
        }

        public void RiskLevelUnknown()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Undetermined;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown;
            PerformInspectionRequiredUpdate();
        }

        public void PrimaryITAHCMissing()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Undetermined;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.PrimaryITAHCMissing;
            PerformInspectionRequiredUpdate();
        }

        //Requirements - Standard Requirements
        public void NoInspectionRequired()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.No;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired;
            PerformInspectionRequiredUpdate();
        }

        public void P1Inspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection;
            PerformInspectionRequiredUpdate();
        }

        public void P2Inspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP2Inspection;
            PerformInspectionRequiredUpdate();
        }

        public void P3Inspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP3Inspection;
            PerformInspectionRequiredUpdate();
        }

        //Requirements - Gold/Bronze Requirements
        public void BronzeInspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin;
            PerformInspectionRequiredUpdate();
        }

        public void LockedToBronzeInspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.PlaceofOriginLockedtoBronze;
            PerformInspectionRequiredUpdate();
        }

        public void GoldCoverageInspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage;
            PerformInspectionRequiredUpdate();
        }

        public void NoInspectionGold()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.No;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequiredGoldPlaceofOrigin;
            PerformInspectionRequiredUpdate();
        }
    }
}
