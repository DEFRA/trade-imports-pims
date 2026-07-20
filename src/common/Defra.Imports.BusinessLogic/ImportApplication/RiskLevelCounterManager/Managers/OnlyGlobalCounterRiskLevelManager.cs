using Defra.Imports.BusinessLogic.ImportApplication.Factories;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public class OnlyGlobalCounterRiskLevelManager : AbstractRiskCounterManager
    {
        string _riskLevel;

        public OnlyGlobalCounterRiskLevelManager(ICrmRepository<defraimp_importapplication> importApplicationRepo, IAutonumberRepository autoNumberRepo, string riskLevel, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, ILogWriter logWriter)
        {
            _importApplicationRepo = importApplicationRepo;
            _autoNumberRepo = autoNumberRepo;
            _coverageRulesRepo = coverageRulesRepo;
            _riskLevel = riskLevel;
            _logWriter = logWriter;
            _abstractCounterTransactionDetailFactory = new CounterTransactionDetailFactory();
        }

        public override void IncrementNumber(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            if (!string.IsNullOrEmpty(_riskLevel))
            {
                // Make sure we've counted this record before we decrement
                if (importApplication.defraimp_ImportRecordCounted != true)
                {
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
                    // If this was a risk level other than P3 we need to decrement the P3 counter as well
                    if (_riskLevel.ToLower() != ImportApplicationConstants.P3_RISK_LEVEL_NAME)
                    {
                        DecrementGlobalCounter(importApplication);
                    }

                    SetRecordCounted(ref importApplication, false);
                }
            }

            base.DecrementNumber(ref importApplication, counterTransactionReason);
        }
    }
}
