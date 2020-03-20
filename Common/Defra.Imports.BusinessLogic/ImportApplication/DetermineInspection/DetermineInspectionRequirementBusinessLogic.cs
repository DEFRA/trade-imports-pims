using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
using Defra.Imports.BusinessLogic.ImportApplication.Factories;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public class DetermineInspectionRequirementBusinessLogic
    {
        private defraimp_importapplication _preImageImportApplication;
        private defraimp_importapplication _postImageImportApplication;
        private ICrmRepository<defraimp_importapplication> _importApplicationRepo;
        private ICrmRepository<defraimp_inspectioncoveragerule> _coverageRulesRepo;
        private ICrmRepository<defraimp_importrisklevel> _importRiskLevelRepo;
        private IAutonumberRepository _autoNumberRepo;
        private IPlaceOfOriginRepository _placeOfOriginRepo;
        private IRepositoryFactory _repositoryFactory;
        private ILogWriter _logWriter;
        private DetermineInspectionContext _determineInspectionContext;
        private IRiskLevelCounterManager _previousRiskLevelCounterManager;

        public DetermineInspectionRequirementBusinessLogic(defraimp_importapplication preImageImportApplication, defraimp_importapplication postImageImportApplication, ICrmRepository<defraimp_importapplication> importAppRepo, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, ICrmRepository<defraimp_importrisklevel> importRiskLevelRepo, IAutonumberRepository autonumberRepo, IPlaceOfOriginRepository placeoforiginRepo, IRepositoryFactory repositoryFactory, ILogWriter logWriter)
        {
            _preImageImportApplication = preImageImportApplication;
            _postImageImportApplication = postImageImportApplication;
            _importApplicationRepo = importAppRepo;
            _coverageRulesRepo = coverageRulesRepo;
            _importRiskLevelRepo = importRiskLevelRepo;
            _autoNumberRepo = autonumberRepo;
            _placeOfOriginRepo = placeoforiginRepo;
            _repositoryFactory = repositoryFactory;
            _logWriter = logWriter;

            _determineInspectionContext = new DetermineInspectionContext()
            {
                ImportApplication = _postImageImportApplication,
                ImportApplicationRepo = _importApplicationRepo,
                CoverageRulesRepo = _coverageRulesRepo,
                AutoNumberRepo = _autoNumberRepo,
                PlaceOfOriginRepo = _placeOfOriginRepo,
                RepositoryFactory = _repositoryFactory
            };
        }

        public void RunLogic()
        {
            string currentRiskLevel = "";
            string previousRiskLevel = "";

            if (_postImageImportApplication != null)
            {
                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Try to popultate current risk as " + _postImageImportApplication.defraimp_importrisklevelid);

                currentRiskLevel =
                     _postImageImportApplication.defraimp_importrisklevelid != null ? _postImageImportApplication.defraimp_importrisklevelid.Name : string.Empty;

                //Set up counter manager to manage incrementing. We need seperate counter managers to support a change in risk levels. We put this in the determine inspection context for risk strategies to use.
                _determineInspectionContext.RiskLevelCounterManager = SetupRiskLevelCounterManager(_postImageImportApplication);
            }

            if (_preImageImportApplication != null)
            {
                //We need this method because the "Name" primary field does not work correctly on a pre-image
                if (_preImageImportApplication.defraimp_importrisklevelid != null)
                {
                    previousRiskLevel = GetRiskLevelName(_preImageImportApplication.defraimp_importrisklevelid.Id);
                }

                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Prev risk is " + previousRiskLevel);

                //Set up counter manager to manage decrementing. We need seperate counter managers to support a change in risk levels.
                _previousRiskLevelCounterManager = SetupRiskLevelCounterManager(_preImageImportApplication);
            }

            // Create (Only post)
            if (_preImageImportApplication == null && _postImageImportApplication != null)
            {
                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Create");

                DealWithDeterminingInspection();
            } //Update
            else if (_preImageImportApplication != null && _postImageImportApplication != null)
            {
                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Update");

                // Update of active record
                if (_preImageImportApplication.statecode == defraimp_importapplicationState.Active && _postImageImportApplication.statecode == defraimp_importapplicationState.Active)
                {
                    ManageRiskLevelChange(previousRiskLevel, currentRiskLevel);
                    ManageITAHCRemoval(previousRiskLevel);
                    ManagePlaceOfOriginChange(previousRiskLevel);

                    DealWithDeterminingInspection();
                } // Deactivate when the state was previously active but has been moved to a status of inactive (Note that we use state and statuscode here as we don't want this logic to run on Application Completion status reason)
                else if (_preImageImportApplication.statecode == defraimp_importapplicationState.Active && _postImageImportApplication.statuscode == defraimp_importapplication_statuscode.Cancelled)
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Deactivate");
                    ManageRecordDeactivation(currentRiskLevel);
                }
                else if (_preImageImportApplication.statecode == defraimp_importapplicationState.Active && _postImageImportApplication.statuscode == defraimp_importapplication_statuscode.NoITAHCReceived)
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Deactivate");
                    ManageRecordDeactivation(currentRiskLevel);
                }
                else if (_preImageImportApplication.statuscode == defraimp_importapplication_statuscode.Cancelled && _postImageImportApplication.statecode == defraimp_importapplicationState.Active)
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Reactivate");
                    ManageRecordReactivation();
                }
                else if (_preImageImportApplication.statuscode == defraimp_importapplication_statuscode.NoITAHCReceived && _postImageImportApplication.statecode == defraimp_importapplicationState.Active)
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Reactivate");
                    ManageRecordReactivation();
                }
            }
            else if (_preImageImportApplication != null && _postImageImportApplication == null)
            {
                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Delete");
                ManageRecordDeletion(previousRiskLevel);
            }
        }

        IRiskLevelCounterManager SetupRiskLevelCounterManager(defraimp_importapplication importApplication)
        {
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Setup Risk Management for risk level of " + importApplication.defraimp_importrisklevelid);
            if (importApplication != null)
            {
                string riskLevel = "";
                if (importApplication.defraimp_importrisklevelid != null)
                {
                    riskLevel = GetRiskLevelName(importApplication.defraimp_importrisklevelid.Id);
                }

                if (!string.IsNullOrEmpty(riskLevel))
                {
                    //Does this record work with Gold/Bronze
                    if (CommodityCoveredByGoldBronze(importApplication))
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Create Place of Origin Risk Level Counter Manager");
                        return new PlaceOfOriginRiskLevelCounterManager(_importApplicationRepo, _autoNumberRepo, importApplication, _placeOfOriginRepo, _coverageRulesRepo, _logWriter);
                    }
                    else
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Create Autonumber: " + riskLevel + " Risk Level Counter Manager");
                        return new AutonumberRiskCounterManager(_importApplicationRepo, _autoNumberRepo, riskLevel, _coverageRulesRepo, _logWriter);
                    }
                }
            }

            return null; //If we make it here, we should return null.
        }
        void ManageRiskLevelChange(string previousRiskLevel, string currentRiskLevel)
        {
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Manage Risk");
            if (previousRiskLevel != currentRiskLevel)
            {
                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Risk changed from: " + previousRiskLevel + " to " + currentRiskLevel);
                // Make sure the previous risk level was not empty
                if (!string.IsNullOrEmpty(previousRiskLevel))
                {
                    if (CommodityCoveredByGoldBronze(_preImageImportApplication))
                    {
                        // Does the pre image have a place of origin?
                        if (_preImageImportApplication.defraimp_PlaceofOriginid != null)
                        {
                            // Get the Place of Origin
                            defraimp_placeoforigin placeOfOrigin = _placeOfOriginRepo.Find(_preImageImportApplication.defraimp_PlaceofOriginid.Id);

                            //If previous Place of Origin Gold?
                            if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                            {
                                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Call Decrement PoO counter");
                                // Manage the counts for the place of origin record we're replacing
                                _previousRiskLevelCounterManager.DecrementNumber(ref _preImageImportApplication, "Risk level changed");
                            }
                        }
                    }
                    else
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Call Decrement " + previousRiskLevel + " counter");
                        _previousRiskLevelCounterManager.DecrementNumber(ref _preImageImportApplication, "Risk level Changed");
                    }
                }
                else
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Null string");
                }
            }
        }

        void ManageITAHCRemoval(string previousRiskLevel)
        {
            if (_preImageImportApplication.defraimp_PrimaryITAHCId != null && _postImageImportApplication.defraimp_PrimaryITAHCId == null)
            {
                // Make sure the previous risk level was not empty
                if (!string.IsNullOrEmpty(previousRiskLevel))
                {
                    if (CommodityCoveredByGoldBronze(_preImageImportApplication))
                    {
                        // Does the pre image have a place of origin?
                        if (_preImageImportApplication.defraimp_PlaceofOriginid != null)
                        {
                            // Get the Place of Origin
                            defraimp_placeoforigin placeOfOrigin = _placeOfOriginRepo.Find(_preImageImportApplication.defraimp_PlaceofOriginid.Id);

                            //If previous Place of Origin Gold?
                            if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                            {
                                // Manage the counts for the place of origin record we're replacing
                                _previousRiskLevelCounterManager.DecrementNumber(ref _preImageImportApplication, "Primary ITAHC Removed");
                            }
                        }
                    }
                    else
                    {
                        _previousRiskLevelCounterManager.DecrementNumber(ref _preImageImportApplication, "Primary ITAHC Removed");
                    }
                }
            }
        }

        void ManagePlaceOfOriginChange(string previousRiskLevel)
        {
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Manage PoO change, previous risk level is: " + previousRiskLevel);
            // Record is P1 and subject to Gold/Bronze rule?
            if (!string.IsNullOrEmpty(previousRiskLevel) && previousRiskLevel.ToLower() == ImportApplicationConstants.P1_RISK_LEVEL_NAME)
            {
                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Check if covered by G/B");
                // Was the commodity a Gold/Bronze commodity
                if (CommodityCoveredByGoldBronze(_preImageImportApplication))
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Is covered by G/B");
                    // Was there previously a place of origin?
                    if (_preImageImportApplication.defraimp_PlaceofOriginid != null)
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Has prev PoO");
                        // Has the place of origin changed?
                        if (_preImageImportApplication.defraimp_PlaceofOriginid != _postImageImportApplication.defraimp_PlaceofOriginid)
                        {
                            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "PoO changed - retrieve");
                            // Get the Place of Origin
                            defraimp_placeoforigin placeOfOrigin = _placeOfOriginRepo.Find(_preImageImportApplication.defraimp_PlaceofOriginid.Id);

                            //If previous Place of Origin Gold?
                            if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                            {
                                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "PoO '" + placeOfOrigin.defraimp_name + "' is gold, decrement");
                                // Manage the counts for the place of origin record we're replacing
                                _previousRiskLevelCounterManager.DecrementNumber(ref _preImageImportApplication, "Place of Origin Changed");
                            }
                        }
                    }
                }
            }
        }

        void ManageRecordDeactivation(string riskLevel)
        {
            _previousRiskLevelCounterManager.DecrementNumber(ref _preImageImportApplication, "Record Deactivated");
        }

        void ManageRecordDeletion(string riskLevel)
        {
            _previousRiskLevelCounterManager.DecrementNumber(ref _preImageImportApplication, "Record Deleted");
        }


        void ManageRecordReactivation()
        {
            DealWithDeterminingInspection();
        }
      
        private bool CommodityCoveredByGoldBronze(defraimp_importapplication importApplication)
        {
            // Make sure we have an import application and country of origin
            if (importApplication.defraimp_CommodityTypeId != null && importApplication.defraimp_CountryofOriginId != null)
            {
                CommodityHelper commodityHelper = new CommodityHelper(importApplication.defraimp_CommodityTypeId, _repositoryFactory);

                if (commodityHelper.IsCommodityCoveredByGoldBronze(importApplication.defraimp_CountryofOriginId))
                {
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }

        private void DealWithDeterminingInspection()
        {
            // Get the risk level from the Import Risk Level and then retrieve the correct determine inspection for the risk level
            DetermineInspectionAbstractFatory determineInspectionFactory = new DetermineInspectionFactory();
            AbstractDetermineInspection determineInspection;

            // Make sure we have a risk level as we need to access the name, otherwise pass in an empty string
            string riskLevel = _postImageImportApplication.defraimp_importrisklevelid != null ? _postImageImportApplication.defraimp_importrisklevelid.Name : string.Empty;
            determineInspection = determineInspectionFactory.GetDetermineInspection(riskLevel);

            if (determineInspection != null)
            {
                // Execute the determine inspection logic for the specific risk level
                determineInspection.ExecuteInspection(_determineInspectionContext);
            }
        }
        string GetRiskLevelName(Guid riskLevelId)
        {
            defraimp_importrisklevel previousRiskLevelId = _importRiskLevelRepo.Find<defraimp_importrisklevel>(
            rule => rule.Id.Equals(riskLevelId),
            e => new defraimp_importrisklevel()
            {
                defraimp_name = e.defraimp_name
            }
            ).FirstOrDefault();

            return previousRiskLevelId != null ? previousRiskLevelId.defraimp_name : string.Empty;
        }
    }
}
