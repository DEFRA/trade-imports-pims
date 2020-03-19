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
        IAutonumberRepository _autoNumberRepo;
        string _riskLevel;

        public AutonumberRiskCounterManager(ICrmRepository<defraimp_importapplication> importApplicationRepo, IAutonumberRepository autoNumberRepo, string riskLevel, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, ILogWriter logWriter)
        {
            _importApplicationRepo = importApplicationRepo;
            _autoNumberRepo = autoNumberRepo;
            _coverageRulesRepo = coverageRulesRepo;
            _riskLevel = riskLevel;
            _logWriter = logWriter;
        }

        public override void IncrementNumber(ref defraimp_importapplication importApplication, string reason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Make sure we've counted this record before we decrement
                if (importApplication.defraimp_ImportRecordCounted != true)
                {
                    _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetCounterName(_riskLevel));
                    SetRecordCounted(ref importApplication, true);
                }
            }

            base.IncrementNumber(ref importApplication, reason);
        }

        public override void DecrementNumber(ref defraimp_importapplication importApplication, string reason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Make sure we've counted this record before we decrement
                if (importApplication.defraimp_ImportRecordCounted == true)
                {
                    // If we previously had flagged this record for a post import check
                    if (importApplication.defraimp_InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.Yes)
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Increment " + _riskLevel + " quota counter");
                        _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));
                    }
                    else
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Decrement " + _riskLevel + " counter");
                        _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.GetCounterName(_riskLevel));
                    }

                    SetRecordCounted(ref importApplication, false);

                    BalanceInspectionToNonInspectionAspectRatio(importApplication);
                }
            }

            base.DecrementNumber(ref importApplication, reason);
        }

        public override void SetNumberValue(ref defraimp_importapplication importApplication, string reason, int value)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                _autoNumberRepo.SetAutonumberValue(ImportApplicationConstants.GetCounterName(_riskLevel), value);
            }
               
            base.SetNumberValue(ref importApplication, reason, value);
        }

        public override void IncrementQuota(ref defraimp_importapplication importApplication, string reason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));
            }

            base.IncrementQuota(ref importApplication, reason);
        }

        public override void DecrementQuota(ref defraimp_importapplication importApplication, string reason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));
            }

            base.DecrementQuota(ref importApplication, reason);
        }

        public override void SetQuotaValue(ref defraimp_importapplication importApplication, string reason, int value)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                _autoNumberRepo.SetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel), value);
            }

            base.SetQuotaValue(ref importApplication, reason, value);
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
                    _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));
                    _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetCounterName(_riskLevel), threshold + 1);
                }
            }
        }
    }
}
