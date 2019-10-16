using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
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
        private IPlaceOfOriginRepository _placeOfOriginRepo;
        private IRepositoryFactory _repositoryFactory;
        private ILogWriter _logWriter;
        private DetermineInspectionContext _determineInspectionContext;

        public DetermineInspectionRequirementBusinessLogic(defraimp_importapplication importApplication, ICrmRepository<defraimp_importapplication> importAppRepo, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, IAutonumberRepository autonumberRepo, IPlaceOfOriginRepository placeoforiginRepo, IRepositoryFactory repositoryFactory, ILogWriter logWriter)
        {
            _importApplication = importApplication;
            _importApplicationRepo = importAppRepo;
            _coverageRulesRepo = coverageRulesRepo;
            _autoNumberRepo = autonumberRepo;
            _placeOfOriginRepo = placeoforiginRepo;
            _repositoryFactory = repositoryFactory;
            _logWriter = logWriter;

            _determineInspectionContext = new DetermineInspectionContext()
            {
                ImportApplication = _importApplication,
                ImportApplicationRepo = _importApplicationRepo,
                CoverageRulesRepo = _coverageRulesRepo,
                AutoNumberRepo = _autoNumberRepo,
                PlaceOfOriginRepo = _placeOfOriginRepo,
                RepositoryFactory = _repositoryFactory
            };
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
                var currentRiskLevel = "";
                if(_importApplication.defraimp_importrisklevelid != null)
                    currentRiskLevel =_importApplication.defraimp_importrisklevelid.Name;

                var previousRiskLevel = "";
                if(_importApplication.defraimp_PreviousImportRiskLevelId != null)
                    previousRiskLevel =_importApplication.defraimp_PreviousImportRiskLevelId.Name;

                var inspectionRequired = _importApplication.defraimp_InspectionRequired;

                if (currentRiskLevel.ToLower() != ImportApplicationConstants.P2_RISK_LEVEL_NAME && previousRiskLevel.ToLower() == ImportApplicationConstants.P2_RISK_LEVEL_NAME)
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
            if(_importApplication.defraimp_importrisklevelid != null)
            {
                // Get the risk level from the Import Risk Level and then retrieve the correct determine inspection for the risk level
                DetermineInspectionAbstractFatory determineInspectionFactory = new DetermineInspectionFactory();
                string riskLevel = _importApplication.defraimp_importrisklevelid.Name;
                AbstractDetermineInspection determineInspection = determineInspectionFactory.GetDetermineInspection(riskLevel);

                if (determineInspection != null)
                {
                    // Execute the determine inspection logic for the specific risk level
                    determineInspection.ExecuteInspection(_determineInspectionContext);
                }
            }
        }

    }
}
