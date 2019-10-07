using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies;
using Defra.Imports.BusinessLogic.ImportApplication.Factories;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public class DetermineInspectionRequirementBusinessLogic
    {
        private defraimp_importapplication _importApplication;
        private ICrmRepository<defraimp_importapplication> _importApplicationRepo;
        private ICrmRepository<defraimp_inspectioncoveragerule> _coverageRulesRepo;
        private IAutonumberRepository _autoNumberRepo;

        public DetermineInspectionRequirementBusinessLogic(defraimp_importapplication importApplication, ICrmRepository<defraimp_importapplication> importAppRepo, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, IAutonumberRepository autonumberRepo, ILogWriter logWriter)
        {
            _importApplication = importApplication;
            _importApplicationRepo = importAppRepo;
            _coverageRulesRepo = coverageRulesRepo;
            _autoNumberRepo = autonumberRepo;
        }

        public void RunLogic()
        {

            if (IsInitialUpdate())
            {
                // This logic occurs when the initial update has happened (~ when the record has been "created")
                // Increment the "2% All-Case-Random" counter
                _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);

            }
            else
            {
                var currentRiskLevel = _importApplication.defraimp_importrisklevelid.Name;
                var previousRiskLevel = _importApplication.defraimp_PreviousImportRiskLevelId.Name;
                var inspectionRequired = _importApplication.defraimp_InspectionRequired;

                if (currentRiskLevel.ToLower() != "p2" && previousRiskLevel.ToLower() == "p2")
                {

                    if (inspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.Yes)
                    {
                        _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME);
                    }
                    else
                    {
                        _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME);
                    }
                }
            }

            DealWithDeterminingInspection();
        }

        private bool IsInitialUpdate()
        {
            // Check if the previous risk level is populated
            return _importApplication.defraimp_PreviousImportRiskLevelId == null;
        }

        private void DealWithDeterminingInspection()
        {
            // Get the risk level from the Import Risk Level and then retrieve the correct determine inspection for the risk level
            DetermineInspectionAbstractFatory determineInspectionFactory = new DetermineInspectionFactory();
            string riskLevel = _importApplication.defraimp_importrisklevelid.Name;
            AbstractDetermineInspection determineInspection = determineInspectionFactory.GetDetermineInspection(riskLevel);

            if (determineInspection != null)
            {
                // Execute the determine inspection logic for the specific risk level
                determineInspection.ExecuteInspection(_importApplication, _importApplicationRepo, _coverageRulesRepo, _autoNumberRepo);
            }
        }

    }
}
