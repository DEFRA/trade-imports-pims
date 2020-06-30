namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;

    public abstract class AbstractDetermineInspection
    {
        public abstract void ExecuteInspection(DetermineInspectionContext determineInspectionContext);

        public defraimp_inspectioncoveragerule GetCoverageRule(ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, string coverageRuleKey)
        {
            // Get the threashold
            defraimp_inspectioncoveragerule coverageRule = coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_Key.Equals(coverageRuleKey),
                e => new defraimp_inspectioncoveragerule()
                {
                    defraimp_name = e.defraimp_name,
                    defraimp_inspectioncoverageruleId = e.defraimp_inspectioncoverageruleId,
                    defraimp_NumberOfRecordsUntilInspection = e.defraimp_NumberOfRecordsUntilInspection
                }
            ).FirstOrDefault();

            return coverageRule;
        }

        public bool ValidImportApplicationTypeForInspection(defraimp_importapplication importApplication)
        {
            if (importApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC && importApplication.defraimp_PrimaryITAHCId != null)
            {
                return true;
            }
            else if (importApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.CHEDA)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
