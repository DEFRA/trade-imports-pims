namespace Defra.Imports.BusinessLogic.ImportApplication
{
    using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
    using Defra.Imports.BusinessLogic.ImportApplication.Factories;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Helpers;
    using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class RiskLevelCounterManagerBusinessLogic
    {
        private defraimp_importapplication _preImageImportApplication;
        private defraimp_importapplication _postOperationImportApplication;
        private IAutonumberRepository _autoNumberRepo;
        private ILogWriter _logWriter;

        public RiskLevelCounterManagerBusinessLogic(defraimp_importapplication preImageImportApplication, defraimp_importapplication postOperationImportApplication, IAutonumberRepository autoNumberRepo, ILogWriter logWriter)
        {
            _preImageImportApplication = preImageImportApplication;
            _postOperationImportApplication = postOperationImportApplication;
            _autoNumberRepo = autoNumberRepo;
            _logWriter = logWriter;
        }

        public void RunLogic()
        {
            // Are we managing an ITAHC Record?
            if (_postOperationImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
            {
                // Do we have a primary ITAHC?
                if (_postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                {
                    if (HasRiskLevelChanged())
                    {
                        //ManageRiskLevelChange();
                    }

                    // Are we gold/bronze and has the place of origin changed? If so manage it using logic written elsewhere.
                }
                else
                {
                    // Missing primary ITAHC
                    // Was there an ITAHC previously? If so, decrement relevant counters.
                }
            }
            else
            {
                // Not an ITAHC
            }
        }

        bool HasRiskLevelChanged()
        {
            if (_preImageImportApplication.defraimp_importrisklevelid != _postOperationImportApplication.defraimp_importrisklevelid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
        /*void ManageRiskLevelChange(string previousCounterName, string previousQuotaCounterName, string postCounterName, string postQuotaCounterName)
        {
            // If we previously had flagged this record for a post import check
            if (_preImageImportApplication.defraimp_InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.Yes)
            {
                _autoNumberRepo.IncrementAutonumber(previousQuotaCounterName);
            }
            else
            {
                _autoNumberRepo.DecrementAutonumber(previousCounterName);
            }

            BalanceInspectionToNonInspectionAspectRatio(postCounterName,postQuotaCounterName);
        }*/

        /*void BalanceInspectionToNonInspectionAspectRatio(string counterName, string quotaCounterName)
        {
            int quotaCounterValue = _autoNumberRepo.GetAutonumberValue(quotaCounterName);
            int counterValue = _autoNumberRepo.GetAutonumberValue(counterName);

            defraimp_inspectioncoveragerule coverageRule = _coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_RiskLevelId.Id.Equals(_importApplication.defraimp_PreviousImportRiskLevelId.Id),
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
                    _autoNumberRepo.DecrementAutonumber(quotaCounterName);
                    _autoNumberRepo.IncrementAutonumber(counterName, threshold + 1);
                }
            }
        }*/

        void P2Path()
        {
            // Update path
            if (_preImageImportApplication != null && _postOperationImportApplication != null)
            {
                if (_postOperationImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
                {
                    if (_preImageImportApplication.defraimp_PrimaryITAHCId == null && _postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        // IF we add an ITAHC, run the inspection logic
                    }
                    else if (_preImageImportApplication.defraimp_PrimaryITAHCId != null && _postOperationImportApplication.defraimp_PrimaryITAHCId == null)
                    {
                        // IF we remove an ITAHC, decrement the autonumber. Run the inspection logic.
                        _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.P2_COUNTER_NAME);
                    }
                }
            }
        }

        void P3Path()
        {
            // Update path
            if (_preImageImportApplication != null && _postOperationImportApplication != null)
            {
                if (_postOperationImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
                {
                    if (_preImageImportApplication.defraimp_PrimaryITAHCId == null && _postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);
                    }
                    else if (_preImageImportApplication.defraimp_PrimaryITAHCId != null && _postOperationImportApplication.defraimp_PrimaryITAHCId == null)
                    {
                        _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);
                    }
                }
            } // Create Path
            else if (_preImageImportApplication == null && _postOperationImportApplication != null)
            {
                if (_postOperationImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
                {
                    if (_postOperationImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);
                    }
                }
            } // Delete Path
            else if (_preImageImportApplication != null && _postOperationImportApplication == null)
            {
                if (_preImageImportApplication.defraimp_ImportApplicationType == defraimp_importapplication_defraimp_importapplicationtype.ITAHC)
                {
                    if (_preImageImportApplication.defraimp_PrimaryITAHCId != null)
                    {
                        _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);
                    }
                }
            }
        }
    }
}
