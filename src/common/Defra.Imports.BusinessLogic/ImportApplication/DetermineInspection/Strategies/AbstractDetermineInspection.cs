namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies
{
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using System.Linq;

    public abstract class AbstractDetermineInspection
    {
        protected IRepositoryFactory repositoryFactory;
        protected defraimp_importapplication importApplication;
        protected InspectionRequirement inspectionRequirement;
        protected AbstractRiskCounterManager riskLevelCounterManager;
        protected IAutonumberRepository autoNumberRepo;
        protected ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo;
        protected ICrmRepository<defraimp_importapplication> importApplicationRepo;
        protected IConfigurationParameterRepository configurationParameterRepo;

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

        protected bool ValidImportApplicationTypeForInspection(defraimp_importapplication importApplication)
        {
            if ((importApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC || importApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHCLandbridge) && importApplication.defraimp_PrimaryITAHCId != null)
            {
                bool tracesEnabled = bool.Parse(this.configurationParameterRepo.GetConfigurationParameterValueByKey("defraimp_traces_enabled"));

                if (tracesEnabled)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (importApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ImportNotification && importApplication.defraimp_PrimaryImporterNotificationId != null)
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

        protected void MissingCertificateError()
        {
            if (importApplication?.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC || importApplication?.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHCLandbridge)
            {
                bool tracesEnabled = bool.Parse(this.configurationParameterRepo.GetConfigurationParameterValueByKey("defraimp_traces_enabled"));

                if (tracesEnabled)
                {
                    // No Primary ITAHC
                    inspectionRequirement?.PrimaryITAHCMissing();
                }
                else
                {
                    // TRACES is disabled
                    inspectionRequirement?.TracesDisabled();

                }

            }
            else if (importApplication?.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ImportNotification)
            {
                // No Primary Importer Notification
                inspectionRequirement?.PrimaryImporterNotificationMissing();
            }
        }
    }
}
