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
        ILogWriter _logWriter;

        public AutonumberRiskCounterManager(ICrmRepository<defraimp_importapplication> importApplicationRepo, ref defraimp_importapplication importApplication, IAutonumberRepository autoNumberRepo, string riskLevel, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, ILogWriter logWriter)
        {
            _importApplicationRepo = importApplicationRepo;
            _importApplication = importApplication;
            _autoNumberRepo = autoNumberRepo;
            _coverageRulesRepo = coverageRulesRepo;
            _riskLevel = riskLevel;
            _logWriter = logWriter;
        }

        public override void IncrementNumber(string reason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Make sure we've counted this record before we decrement
                if (_importApplication.defraimp_ImportRecordCounted != true)
                {
                    _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetCounterName(_riskLevel));
                    SetRecordCounted(true);
                }
            }

            base.IncrementNumber(reason);
        }

        public override void DecrementNumber(string reason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Make sure we've counted this record before we decrement
                if (_importApplication.defraimp_ImportRecordCounted == true)
                {
                    // If we previously had flagged this record for a post import check
                    if (_importApplication.defraimp_InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.Yes)
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Increment " + _riskLevel + " quota counter");
                        _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));
                    }
                    else
                    {
                        _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Decrement " + _riskLevel + " counter");
                        _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.GetCounterName(_riskLevel));
                    }

                    SetRecordCounted(false);

                    BalanceInspectionToNonInspectionAspectRatio();
                }
            }

            base.DecrementNumber(reason);
        }

        public override void SetNumberValue(string reason, int value)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                _autoNumberRepo.SetAutonumberValue(ImportApplicationConstants.GetCounterName(_riskLevel), value);
            }
               
            base.SetNumberValue(reason, value);
        }

        public override void IncrementQuota(string reason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));
            }

            base.IncrementQuota(reason);
        }

        public override void DecrementQuota(string reason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));
            }

            base.DecrementQuota(reason);
        }

        public override void SetQuotaValue(string reason, int value)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                _autoNumberRepo.SetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel), value);
            }

            base.SetQuotaValue(reason, value);
        }

        void BalanceInspectionToNonInspectionAspectRatio()
        {
            int quotaCounterValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetQuotaCounterName(_riskLevel));
            int counterValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.GetCounterName(_riskLevel));

            defraimp_inspectioncoveragerule coverageRule = _coverageRulesRepo.Find<defraimp_inspectioncoveragerule>(
                rule => rule.defraimp_RiskLevelId.Id.Equals(_importApplication.defraimp_importrisklevelid.Id),
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
