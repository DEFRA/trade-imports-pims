using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies
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

        //Requirements - Missing Data
        public void PlaceOfOriginMissing()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Discretionary;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.VerifiedPlaceofOriginMissing;
        }

        public void RiskLevelUnknown()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Undetermined;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown;
        }

        //Requirements - Standard Requirements
        public void NoInspectionRequired()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.No;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.NoInspectionRequired;
        }

        public void P1Inspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP1Inspection;
        }

        public void P2Inspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP2Inspection;
        }

        public void P3Inspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.RandomP3Inspection;
        }

        //Requirements - Gold/Bronze Requirements
        public void BronzeInspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.BronzePlaceofOrigin;
        }

        public void LockedToBronzeInspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.PlaceofOriginLockedtoBronze;
        }

        public void GoldCoverageInspection()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage;
        }

        public void NoInspectionGold()
        {
            InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.No;
            InspectionRequiredReason = defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage;
        }
    }
}
