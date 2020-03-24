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
    public class PlaceOfOriginRiskLevelCounterManager : AbstractRiskCounterManager
    {
        IPlaceOfOriginRepository _placeOfOriginRepo;
        defraimp_placeoforigin _placeOfOrigin;

        public PlaceOfOriginRiskLevelCounterManager(ICrmRepository<defraimp_importapplication> importApplicationRepo, IAutonumberRepository autoNumberRepo, defraimp_importapplication importApplication, IPlaceOfOriginRepository placeOfOriginRepo, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, ILogWriter logWriter)
        {
            _importApplicationRepo = importApplicationRepo;
            _placeOfOriginRepo = placeOfOriginRepo;
            _coverageRulesRepo = coverageRulesRepo;
            _autoNumberRepo = autoNumberRepo;
            _logWriter = logWriter;
            _abstractCounterTransactionDetailFactory = new CounterTransactionDetailFactory();

            if (importApplication.defraimp_PlaceofOriginid != null)
            {
                _placeOfOrigin = _placeOfOriginRepo.Find(importApplication.defraimp_PlaceofOriginid.Id);
            }
        }

        public override void IncrementNumber(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            if (_placeOfOrigin != null)
            {
                // Make sure we've counted this record before we decrement
                if (importApplication.defraimp_ImportRecordCounted != true)
                {   
                    // Create a new counterTransactionDetail record and populate it
                    CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _placeOfOrigin, defraimp_counterhistory_defraimp_operation.Increment, counterTransactionReason);
                    counterTransactionDetail.PreviousValue = _placeOfOriginRepo.GetApplicationCounterValue(_placeOfOrigin.Id);

                    // Carry out the increment operation
                    _placeOfOriginRepo.IncrementApplicationCounter(_placeOfOrigin.Id);

                    // Get the current value of the counter after the operation
                    counterTransactionDetail.CurrentValue = _placeOfOriginRepo.GetApplicationCounterValue(_placeOfOrigin.Id);

                    // Broadcast the message so that the auditor can pick it up
                    BroadcastCounterTransactionEvent(counterTransactionDetail);

                    // Increment the P3 global counter
                    IncrementGlobalCounter(importApplication);

                    SetRecordCounted(ref importApplication, true);
                }
            }

            base.IncrementNumber(ref importApplication, counterTransactionReason);
        }

        public override void DecrementNumber(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            if (_placeOfOrigin != null)
            {
                // Make sure we've counted this record before we decrement
                if (importApplication.defraimp_ImportRecordCounted == true)
                {
                    // If we needed to inspect because of Gold/Bronze inspection coverage
                    if (importApplication.defraimp_InspectionRequiredReason == defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage)
                    {
                        IncrementQuota(ref importApplication, counterTransactionReason);
                    }
                    else
                    {
                        // Create a new counterTransactionDetail record and populate it
                        CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _placeOfOrigin, defraimp_counterhistory_defraimp_operation.Decrement, counterTransactionReason);
                        counterTransactionDetail.PreviousValue = _placeOfOriginRepo.GetApplicationCounterValue(_placeOfOrigin.Id);

                        // Carry out the decrement operation
                        _placeOfOriginRepo.DecrementApplicationCounter(_placeOfOrigin.Id);

                        // Get the current value of the counter after the operation
                        counterTransactionDetail.CurrentValue = _placeOfOriginRepo.GetApplicationCounterValue(_placeOfOrigin.Id);

                        // Broadcast the message so that the auditor can pick it up
                        BroadcastCounterTransactionEvent(counterTransactionDetail);

                    }

                    // Decrement the P3 global counter
                    DecrementGlobalCounter(importApplication);

                    SetRecordCounted(ref importApplication, false);

                    BalanceInspectionToNonInspectionAspectRatio(importApplication);
                }
            }

            base.DecrementNumber(ref importApplication, counterTransactionReason);
        }

        public override void SetNumberValue(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason, int value)
        {
            if (_placeOfOrigin != null)
            {
                // Create a new counterTransactionDetail record and populate it
                CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _placeOfOrigin, defraimp_counterhistory_defraimp_operation.Setto0, counterTransactionReason);
                counterTransactionDetail.PreviousValue = _placeOfOriginRepo.GetApplicationCounterValue(_placeOfOrigin.Id);

                // Carry out the set number operation
                _placeOfOriginRepo.SetApplicationCounter(_placeOfOrigin.Id, value);

                // Get the current value of the counter after the operation
                counterTransactionDetail.CurrentValue = _placeOfOriginRepo.GetApplicationCounterValue(_placeOfOrigin.Id);

                // Broadcast the message so that the auditor can pick it up
                BroadcastCounterTransactionEvent(counterTransactionDetail);
            }

            base.SetNumberValue(ref importApplication, counterTransactionReason, value);
        }

        public override void IncrementQuota(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            if (_placeOfOrigin != null)
            {
                // Create a new counterTransactionDetail record and populate it
                CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _placeOfOrigin, defraimp_counterhistory_defraimp_operation.Increment, counterTransactionReason);
                counterTransactionDetail.PreviousValue = _placeOfOriginRepo.GetQuotaCounterValue(_placeOfOrigin.Id);

                _placeOfOriginRepo.IncrementQuotaCounter(_placeOfOrigin.Id);

                // Get the current value of the quota after the operation
                counterTransactionDetail.CurrentValue = _placeOfOriginRepo.GetQuotaCounterValue(_placeOfOrigin.Id);

                // Broadcast the message so that the auditor can pick it up
                BroadcastCounterTransactionEvent(counterTransactionDetail);
            }

            base.IncrementQuota(ref importApplication, counterTransactionReason);
        }

        public override void DecrementQuota(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            // This is called when there was a quota value that we're now taking to flag an import record for an inspection
            if (_placeOfOrigin != null)
            {
                // Create a new counterTransactionDetail record and populate it
                CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _placeOfOrigin, defraimp_counterhistory_defraimp_operation.Decrement, counterTransactionReason);
                counterTransactionDetail.PreviousValue = _placeOfOriginRepo.GetQuotaCounterValue(_placeOfOrigin.Id);

                _placeOfOriginRepo.DecrementQuotaCounter(_placeOfOrigin.Id);

                // Get the current value of the quota after the operation
                counterTransactionDetail.CurrentValue = _placeOfOriginRepo.GetQuotaCounterValue(_placeOfOrigin.Id);

                // Broadcast the message so that the auditor can pick it up
                BroadcastCounterTransactionEvent(counterTransactionDetail);

                SetRecordCounted(ref importApplication, true);
            }

            base.DecrementQuota(ref importApplication, counterTransactionReason);
        }

        public override void SetQuotaValue(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason, int value)
        {
            if (_placeOfOrigin != null)
            {
                // Create a new counterTransactionDetail record and populate it
                CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, _placeOfOrigin, defraimp_counterhistory_defraimp_operation.Setto0, counterTransactionReason);
                counterTransactionDetail.PreviousValue = _placeOfOriginRepo.GetQuotaCounterValue(_placeOfOrigin.Id);

                _placeOfOriginRepo.SetApplicationCounter(_placeOfOrigin.Id, value);

                // Get the current value of the quota after the operation
                counterTransactionDetail.CurrentValue = _placeOfOriginRepo.GetQuotaCounterValue(_placeOfOrigin.Id);

                // Broadcast the message so that the auditor can pick it up
                BroadcastCounterTransactionEvent(counterTransactionDetail);
            }

            base.SetQuotaValue(ref importApplication, counterTransactionReason, value);
        }


        void BalanceInspectionToNonInspectionAspectRatio(defraimp_importapplication importApplication)
        {
            int quotaCounterValue = _placeOfOriginRepo.GetQuotaCounterValue(_placeOfOrigin.Id);
            int counterValue = _placeOfOriginRepo.GetApplicationCounterValue(_placeOfOrigin.Id);

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
                    DecrementQuota(ref importApplication, defraimp_counterhistory_defraimp_reason.NegativeThresholdReachedBalanceQuota);
                    SetNumberValue(ref importApplication, defraimp_counterhistory_defraimp_reason.NegativeThresholdReachedBalanceQuota, 0);
                }
            }
        }
    }
}
