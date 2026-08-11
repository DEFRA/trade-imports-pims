namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies
{
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;

    public class UnknownDetermineInspection : AbstractDetermineInspection
    {
        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            var importApplication = determineInspectionContext.ImportApplication;
            var importApplicationRepo = determineInspectionContext.ImportApplicationRepo;
            var inspectionRequirement = new InspectionRequirement(importApplication, importApplicationRepo);

            inspectionRequirement.RiskLevelUnknown();
        }
    }
}
