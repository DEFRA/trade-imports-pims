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
                currentRiskLevel =
                     _postImageImportApplication.defraimp_importrisklevelid != null ? _postImageImportApplication.defraimp_importrisklevelid.Name : string.Empty;
            }

            if (_preImageImportApplication != null)
            {
                previousRiskLevel =
                    _preImageImportApplication.defraimp_importrisklevelid != null ? _preImageImportApplication.defraimp_importrisklevelid.Name : string.Empty;
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
                else if (_preImageImportApplication.statecode == defraimp_importapplicationState.Active && _postImageImportApplication.statuscode == defraimp_importapplication_statuscode.Inactive)
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Deactivate");
                    ManageRecordDeactivation(currentRiskLevel);
                }
                else if (_preImageImportApplication.statuscode == defraimp_importapplication_statuscode.Inactive && _postImageImportApplication.statecode == defraimp_importapplicationState.Active)
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Reactivate");
                    ManageRecordReactivation();
                }
            }
            else if (_preImageImportApplication != null && _postImageImportApplication == null)
            {
                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Delete");
                ManageRecordDeactivation(previousRiskLevel);
            }
        }

        void ManageRiskLevelChange(string previousRiskLevel, string currentRiskLevel)
        {
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Manage Risk");
            if (previousRiskLevel != currentRiskLevel)
            {
                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Risk changed from: " + previousRiskLevel);
                // Make sure the previous risk level was not empty
                if (!string.IsNullOrEmpty(previousRiskLevel))
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Call Decrement " + previousRiskLevel + " counter");
                    if (CommodityCoveredByGoldBronze())
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
                                ManagePlaceOfOriginCounterDecrement(placeOfOrigin.Id);
                            }
                        }
                    }
                    else
                    {
                        ManageCounterDecrement(previousRiskLevel);
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
                    if (CommodityCoveredByGoldBronze())
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
                                ManagePlaceOfOriginCounterDecrement(placeOfOrigin.Id);
                            }
                        }
                    }
                    else
                    {
                        ManageCounterDecrement(previousRiskLevel);
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
                if (CommodityCoveredByGoldBronze())
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
                                ManagePlaceOfOriginCounterDecrement(placeOfOrigin.Id);
                            }
                        }
                    }
                }
            }
        }

        void ManageRecordDeactivation(string riskLevel)
        {
            ManageCounterDecrement(riskLevel);
        }

        void ManageRecordReactivation()
        {
            DealWithDeterminingInspection();
        }

        void ManageCounterDecrement(string riskLevel)
        {
            if (!string.IsNullOrEmpty(riskLevel))
            {
                // Make sure we've counted this record before we decrement
                if (_preImageImportApplication.defraimp_ImportRecordCounted == true)
                {
                    // If we previously had flagged this record for a post import check
                    if (_preImageImportApplication.defraimp_InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.Yes)
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Increment " + riskLevel + " quota counter");
                        _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(riskLevel));
                    }
                    else
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Decrement " + riskLevel + " counter");
                        _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.GetCounterName(riskLevel));
                    }

                    SetRecordCounted(false);

                    BalanceInspectionToNonInspectionAspectRatio(riskLevel);
                }
            }
        }

        void ManagePlaceOfOriginCounterDecrement(Guid placeOfOriginId)
        {
            // Make sure we've counted this record before we decrement
            if (_preImageImportApplication.defraimp_ImportRecordCounted == true)
            {
                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Inspection reason is " + _preImageImportApplication.defraimp_InspectionRequiredReason.Value);
                // If we needed to inspect because of Gold/Bronze inspection coverage
                if (_preImageImportApplication.defraimp_InspectionRequiredReason == defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage)
                {
                    //Increment the quota counter so that the next record for this place of origin is inspected
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Increment PoO '" + placeOfOriginId + "' Quota");
                    _placeOfOriginRepo.IncrementQuotaCounter(placeOfOriginId);
                }
                else
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Decrement PoO '" + placeOfOriginId + "' counter");
                    _placeOfOriginRepo.DecrementApplicationCounter(placeOfOriginId);
                }

                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Set record as 'Not Counted'");
                SetRecordCounted(false);

                _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Balance Place of Origin ratios");
                BalancePlaceOfOriginInspectionToNonInspectionAspectRatio(placeOfOriginId);
            }
        }

        void SetRecordCounted(bool counted)
        {
            defraimp_importapplication updatedImportApplication = new defraimp_importapplication();
            updatedImportApplication.Id = _postImageImportApplication.Id;
            updatedImportApplication.defraimp_ImportRecordCounted = counted;
            _importApplicationRepo.Update(updatedImportApplication);

            //Update the local copy of the flag
            _preImageImportApplication.defraimp_ImportRecordCounted = counted; //Set this to ensure we don't decrement twice
            _postImageImportApplication.defraimp_ImportRecordCounted = counted; //Set this to ensure we can increment if we need to
        }
        

        void BalanceInspectionToNonInspectionAspectRatio(string currentRiskLevel)
        {
            int quotaCounterValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(currentRiskLevel));
            int counterValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetCounterName(currentRiskLevel));

            defraimp_inspectioncoveragerule coverageRule = _coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_RiskLevelId.Id.Equals(_preImageImportApplication.defraimp_importrisklevelid.Id),
                e => new defraimp_inspectioncoveragerule()
                {
                    defraimp_name = e.defraimp_name,
                    defraimp_inspectioncoverageruleId = e.defraimp_inspectioncoverageruleId,
                    defraimp_NumberOfRecordsUntilInspection = e.defraimp_NumberOfRecordsUntilInspection
                }
            ).FirstOrDefault();

            if (coverageRule != null)
            {
                int threshold = coverageRule.defraimp_NumberOfRecordsUntilInspection.Value;
                int negativeThreshold = -threshold;

                if ((quotaCounterValue > 0) && (counterValue <= negativeThreshold))
                {
                    _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(currentRiskLevel));
                    _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetCounterName(currentRiskLevel), threshold + 1);
                }
            }
        }
        
        void BalancePlaceOfOriginInspectionToNonInspectionAspectRatio(Guid placeOfOriginId)
        {
            int quotaCounterValue = _placeOfOriginRepo.GetQuotaCounterValue(placeOfOriginId);
            int counterValue = _placeOfOriginRepo.GetApplicationCounterValue(placeOfOriginId);

            defraimp_inspectioncoveragerule coverageRule = _coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_Key.Equals(ImportApplicationConstants.GB_COVERAGE_RULE_KEY),
                e => new defraimp_inspectioncoveragerule()
                {
                    defraimp_name = e.defraimp_name,
                    defraimp_inspectioncoverageruleId = e.defraimp_inspectioncoverageruleId,
                    defraimp_NumberOfRecordsUntilInspection = e.defraimp_NumberOfRecordsUntilInspection
                }
            ).FirstOrDefault();

            if (coverageRule != null)
            {
                int threshold = coverageRule.defraimp_NumberOfRecordsUntilInspection.Value;
                int negativeThreshold = -threshold;

                if ((quotaCounterValue > 0) && (counterValue <= negativeThreshold))
                {
                    _placeOfOriginRepo.DecrementQuotaCounter(placeOfOriginId);
                    _placeOfOriginRepo.SetApplicationCounter(placeOfOriginId, 0);
                }
            }
        }

        private bool CommodityCoveredByGoldBronze()
        {
            // Make sure we have an import application and country of origin
            if (_preImageImportApplication.defraimp_CommodityTypeId != null && _preImageImportApplication.defraimp_CountryofOriginId != null)
            {
                CommodityHelper commodityHelper = new CommodityHelper(_preImageImportApplication.defraimp_CommodityTypeId, _repositoryFactory);

                if (commodityHelper.IsCommodityCoveredByGoldBronze(_preImageImportApplication.defraimp_CountryofOriginId))
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
    }
}
