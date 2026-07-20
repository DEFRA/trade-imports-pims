namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies
{
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;

    public class TBDetermineInspection : AbstractDetermineInspection
    {
        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            importApplication = determineInspectionContext.ImportApplication;
            importApplicationRepo = determineInspectionContext.ImportApplicationRepo;
            autoNumberRepo = determineInspectionContext.AutoNumberRepo;
            coverageRulesRepo = determineInspectionContext.CoverageRulesRepo;
            inspectionRequirement = new InspectionRequirement(importApplication, importApplicationRepo);
            riskLevelCounterManager = determineInspectionContext.RiskLevelCounterManager;
            configurationParameterRepo = determineInspectionContext.ConfigurationParameterRepo;

            //Does the import application have a primary ITAHC?
            if (ValidImportApplicationTypeForInspection(importApplication))
            {
                // Has the record not been counted yet?
                if (importApplication.defraimp_ImportRecordCounted != true)
                {
                    // Increment the global counter first
                    riskLevelCounterManager.IncrementGlobalCounter(importApplication);

                    inspectionRequirement.TBInpsection();
                }
            }
            else
            {
                MissingCertificateError();
            }
        }
    }
}
