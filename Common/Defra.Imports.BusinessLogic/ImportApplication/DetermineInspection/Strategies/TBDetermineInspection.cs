namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using Microsoft.Xrm.Sdk;

    public class TBDetermineInspection : AbstractDetermineInspection
    {
        private defraimp_importapplication _importApplication;
        private IAutonumberRepository _autoNumberRepo;
        private ICrmRepository<defraimp_inspectioncoveragerule> _coverageRulesRepo;
        private ICrmRepository<defraimp_importapplication> _importApplicationRepo;
        private InspectionRequirement _inspectionRequirement;
        private AbstractRiskCounterManager _riskLevelCounterManager;

        public override void ExecuteInspection(DetermineInspectionContext determineInspectionContext)
        {
            _importApplication = determineInspectionContext.ImportApplication;
            _importApplicationRepo = determineInspectionContext.ImportApplicationRepo;
            _autoNumberRepo = determineInspectionContext.AutoNumberRepo;
            _coverageRulesRepo = determineInspectionContext.CoverageRulesRepo;
            _inspectionRequirement = new InspectionRequirement(_importApplication, _importApplicationRepo);
            _riskLevelCounterManager = determineInspectionContext.RiskLevelCounterManager;

            //Does the import application have a primary ITAHC?
            if (ValidImportApplicationTypeForInspection(_importApplication))
            {
                // Has the record not been counted yet?
                if (_importApplication.defraimp_ImportRecordCounted != true)
                {
                    // Increment the global counter first
                    _riskLevelCounterManager.IncrementGlobalCounter(_importApplication);
                }
            }
            else if (_importApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
            {
                // Flag the application as missing an ITAHC
                _inspectionRequirement.PrimaryITAHCMissing();
            }
        }
    }
}
