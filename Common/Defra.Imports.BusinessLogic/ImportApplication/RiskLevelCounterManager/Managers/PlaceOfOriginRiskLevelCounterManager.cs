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
        ILogWriter _logWriter;

        public PlaceOfOriginRiskLevelCounterManager(ICrmRepository<defraimp_importapplication> importApplicationRepo, ref defraimp_importapplication importApplication, IPlaceOfOriginRepository placeOfOriginRepo, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, ILogWriter logWriter)
        {
            _importApplicationRepo = importApplicationRepo;
            _importApplication = importApplication;
            _placeOfOriginRepo = placeOfOriginRepo;
            _coverageRulesRepo = coverageRulesRepo;
            _logWriter = logWriter;

            if (importApplication.defraimp_PlaceofOriginid != null)
            {
                _placeOfOrigin = _placeOfOriginRepo.Find(importApplication.defraimp_PlaceofOriginid.Id);
            }
        }

        public override void IncrementNumber(string reason)
        {
            if (_placeOfOrigin != null)
            {
                // Make sure we've counted this record before we decrement
                if (_importApplication.defraimp_ImportRecordCounted != true)
                {
                    _placeOfOriginRepo.IncrementApplicationCounter(_placeOfOrigin.Id);
                    SetRecordCounted(true);
                }
            }

            base.IncrementNumber(reason);
        }

        public override void DecrementNumber(string reason)
        {
            if (_placeOfOrigin != null)
            {
                // Make sure we've counted this record before we decrement
                if (_importApplication.defraimp_ImportRecordCounted == true)
                {
                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Inspection reason is " + _importApplication.defraimp_InspectionRequiredReason.Value);
                    // If we needed to inspect because of Gold/Bronze inspection coverage
                    if (_importApplication.defraimp_InspectionRequiredReason == defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage)
                    {
                        //Increment the quota counter so that the next record for this place of origin is inspected
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Increment PoO '" + _placeOfOrigin.Id + "' Quota");
                        _placeOfOriginRepo.IncrementQuotaCounter(_placeOfOrigin.Id);
                    }
                    else
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Decrement PoO '" + _placeOfOrigin.Id + "' counter");
                        _placeOfOriginRepo.DecrementApplicationCounter(_placeOfOrigin.Id);
                    }

                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Set record as 'Not Counted'");
                    SetRecordCounted(false);

                    _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Balance Place of Origin ratios");
                    BalanceInspectionToNonInspectionAspectRatio();
                }
            }

            base.DecrementNumber(reason);
        }

        public override void SetNumberValue(string reason, int value)
        {
            if (_placeOfOrigin != null)
            {
                _placeOfOriginRepo.SetApplicationCounter(_placeOfOrigin.Id, value);
            }

            base.SetNumberValue(reason, value);
        }

        public override void IncrementQuota(string reason)
        {
            if (_placeOfOrigin != null)
            {
                _placeOfOriginRepo.IncrementQuotaCounter(_placeOfOrigin.Id);
            }

            base.IncrementNumber(reason);
        }

        public override void DecrementQuota(string reason)
        {
            if (_placeOfOrigin != null)
            {
                _placeOfOriginRepo.DecrementQuotaCounter(_placeOfOrigin.Id);
            }

            base.DecrementQuota(reason);
        }

        public override void SetQuotaValue(string reason, int value)
        {
            if (_placeOfOrigin != null)
            {
                _placeOfOriginRepo.SetApplicationCounter(_placeOfOrigin.Id, value);
            }

            base.SetQuotaValue(reason, value);
        }


        void BalanceInspectionToNonInspectionAspectRatio()
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
                    _placeOfOriginRepo.DecrementQuotaCounter(_placeOfOrigin.Id);
                    _placeOfOriginRepo.SetApplicationCounter(_placeOfOrigin.Id, 0);
                }
            }
        }
    }
}
