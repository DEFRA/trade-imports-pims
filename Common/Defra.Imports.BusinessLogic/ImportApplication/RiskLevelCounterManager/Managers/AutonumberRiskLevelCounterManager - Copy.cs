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
using System.Linq;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public class AutonumberRiskCounterManager : AbstractRiskCounterManager
    {
        string _riskLevel;
        defraimp_autonumber _autoNumberRecord;
        defraimp_autonumber _quotaAutoNumberRecord;

        public AutonumberRiskCounterManager(ICrmRepository<defraimp_importapplication> importApplicationRepo, IAutonumberRepository autoNumberRepo, string riskLevel, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, ILogWriter logWriter)
        {
            _importApplicationRepo = importApplicationRepo;
            _autoNumberRepo = autoNumberRepo;
            _coverageRulesRepo = coverageRulesRepo;
            _riskLevel = riskLevel;
            _logWriter = logWriter;
            _autoNumberRecord = _autoNumberRepo.GetAutonumberWithKey(ImportApplicationConstants.GetCounterName(_riskLevel));
            _quotaAutoNumberRecord = _autoNumberRepo.GetAutonumberWithKey(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));
            _abstractCounterTransactionDetailFactory = new CounterTransactionDetailFactory();
        }

        public override void IncrementNumber(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Make sure we've counted this record before we decrement
                if (importApplication.defraimp_ImportRecordCounted != true)
                {
                    // Create a new counterTransactionDetail record and populate it
                    CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _autoNumberRecord, defraimp_counterhistory_defraimp_operation.Increment, counterTransactionReason);               
                    counterTransactionDetail.PreviousValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetCounterName(_riskLevel));

                    // Carry out the increment operation
                    _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetCounterName(_riskLevel));

                    // Get the current value of the counter after the operation
                    counterTransactionDetail.CurrentValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetCounterName(_riskLevel));

                    // Broadcast the message so that the auditor can pick it up
                    BroadcastCounterTransactionEvent(counterTransactionDetail);

                    // If this was a risk level other than P3 we need to also increment the P3 counter
                    if (_riskLevel.ToLower() != ImportApplicationConstants.P3_RISK_LEVEL_NAME)
                    {
                        IncrementGlobalCounter(importApplication);
                    }

                    SetRecordCounted(ref importApplication, true);
                }
            }

            base.IncrementNumber(ref importApplication, counterTransactionReason);
        }

        public override void DecrementNumber(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Make sure we've counted this record before we decrement
                if (importApplication.defraimp_ImportRecordCounted == true)
                {
                    // If we previously had flagged this record for a post import check
                    if (importApplication.defraimp_InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.Yes)
                    {
                        // Carry out the increment operation for the quota
                        IncrementQuota(ref importApplication, counterTransactionReason);
                    }
                    else
                    {
                        // Create a new counterTransactionDetail record and populate it
                        CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _autoNumberRecord, defraimp_counterhistory_defraimp_operation.Decrement, counterTransactionReason);
                        counterTransactionDetail.PreviousValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));

                        // Carry out the decrement operation
                        _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.GetCounterName(_riskLevel));

                        // Get the current value of the counter after the operation
                        counterTransactionDetail.CurrentValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));

                        // Broadcast the message so that the auditor can pick it up
                        BroadcastCounterTransactionEvent(counterTransactionDetail);
                    }

                    // If this was a risk level other than P3 we need to decrement the P3 counter as well
                    if (_riskLevel.ToLower() != ImportApplicationConstants.P3_RISK_LEVEL_NAME)
                    {
                        DecrementGlobalCounter(importApplication);
                    }

                    SetRecordCounted(ref importApplication, false);

                    BalanceInspectionToNonInspectionAspectRatio(importApplication);
                }
            }

            base.DecrementNumber(ref importApplication, counterTransactionReason);
        }

        public override void SetNumberValue(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason, int value)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Create a new counterTransactionDetail record and populate it
                CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _autoNumberRecord, defraimp_counterhistory_defraimp_operation.Setto0, counterTransactionReason);
                counterTransactionDetail.PreviousValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetCounterName(_riskLevel));

                // Carry out the set number operation
                _autoNumberRepo.SetAutonumberValue(ImportApplicationConstants.GetCounterName(_riskLevel), value);

                // Get the current value of the counter after the operation
                counterTransactionDetail.CurrentValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetCounterName(_riskLevel));

                // Broadcast the message so that the auditor can pick it up
                BroadcastCounterTransactionEvent(counterTransactionDetail);
            }
               
            base.SetNumberValue(ref importApplication, counterTransactionReason, value);
        }

        public override void IncrementQuota(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Create a new counterTransactionDetail record and populate it
                CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _quotaAutoNumberRecord, defraimp_counterhistory_defraimp_operation.Increment, counterTransactionReason);
                counterTransactionDetail.PreviousValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));

                // Carry out the increment quota operation
                _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));

                // Get the current value of the quota after the operation
                counterTransactionDetail.CurrentValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));

                // Broadcast the message so that the auditor can pick it up
                BroadcastCounterTransactionEvent(counterTransactionDetail);
            }

            base.IncrementQuota(ref importApplication, counterTransactionReason);
        }

        public override void DecrementQuota(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Create a new counterTransactionDetail record and populate it
                CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _quotaAutoNumberRecord, defraimp_counterhistory_defraimp_operation.Decrement, counterTransactionReason);
                counterTransactionDetail.PreviousValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));

                // Carry out the decrement quota operation
                _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));

                // Get the current value of the quota after the operation
                counterTransactionDetail.CurrentValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));

                SetRecordCounted(ref importApplication, true);

                // Broadcast the message so that the auditor can pick it up
                BroadcastCounterTransactionEvent(counterTransactionDetail);
            }

            base.DecrementQuota(ref importApplication, counterTransactionReason);
        }

        public override void SetQuotaValue(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason, int value)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Create a new counterTransactionDetail record and populate it
                CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _quotaAutoNumberRecord, defraimp_counterhistory_defraimp_operation.Setto0, counterTransactionReason);
                counterTransactionDetail.PreviousValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));

                // Carry out the set quota value operation
                _autoNumberRepo.SetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel), value);

                // Get the current value of the quota after the operation
                counterTransactionDetail.CurrentValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));

                // Broadcast the message so that the auditor can pick it up
                BroadcastCounterTransactionEvent(counterTransactionDetail);
            }

            base.SetQuotaValue(ref importApplication, counterTransactionReason, value);
        }

        void BalanceInspectionToNonInspectionAspectRatio(defraimp_importapplication importApplication)
        {
            int quotaCounterValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));
            int counterValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetCounterName(_riskLevel));

            defraimp_inspectioncoveragerule coverageRule = _coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_RiskLevelId.Id.Equals(importApplication.defraimp_importrisklevelid.Id),
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
                    DecrementQuota(ref importApplication, defraimp_counterhistory_defraimp_reason.NegativeThresholdReachedBalanceQuota);
                    SetNumberValue(ref importApplication, defraimp_counterhistory_defraimp_reason.NegativeThresholdReachedBalanceQuota, 0);
                }
            }
        }
    }
}
