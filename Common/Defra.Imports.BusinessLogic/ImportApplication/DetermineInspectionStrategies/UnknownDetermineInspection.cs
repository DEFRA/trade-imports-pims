using System;
using System.Collections.Generic;
using System.Text;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;

namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class UnknownDetermineInspection : AbstractDetermineInspection
    {
        public override void ExecuteInspection(defraimp_importapplication importApplication, ICrmRepository<defraimp_importapplication> importApplicationRepo, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, IAutonumberRepository autoNumberRepo)
        {
            PerformInspectionRequiredUpdate(
                importApplication,
                importApplicationRepo,
                defraimp_importapplication_defraimp_inspectionrequired.Undetermined,
                defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown
            );
        }
    }
}
