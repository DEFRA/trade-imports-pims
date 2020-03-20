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

namespace Defra.Imports.BusinessLogic.ImportApplication.RiskLevelCounterManager.Auditor
{
    class RiskCounterAuditor
    {
        AbstractRiskCounterManager _riskLevelCounterManager;
        ICrmRepository<defraimp_counterhistory> _counterHistoryRepo;

        public RiskCounterAuditor(AbstractRiskCounterManager riskLevelCounterManager, ICrmRepository<defraimp_counterhistory> counterHistoryRepo)
        {
            _riskLevelCounterManager = riskLevelCounterManager;

            //Subscribe to the events we care about
            _riskLevelCounterManager.IncrementNumberEvent += IncrementNumber;
            _riskLevelCounterManager.DecrementNumberEvent += DecrementNumber;
            _riskLevelCounterManager.SetNumberEvent += SetNumber;
            _riskLevelCounterManager.IncrementQuotaEvent += IncrementQuota;
            _riskLevelCounterManager.DecrementQuotaEvent +=
        }

        void AuditCounterEvent(string reason)
        {

        }

        void CreateCounterHistoryRecord()
        {
            defraimp_counterhistory counterHistoryRecord = new defraimp_counterhistory();

            counterHistoryRecord.defraimp_CounterHistoryType = defraimp_counterhistory_defraimp_counterhistorytype.AutoNumber;
            counterHistoryRecord.defraimp_Operation = defraimp_counterhistory_defraimp_operation.;
            counterHistoryRecord.defraimp_AutoNumberId =;
            counterHistoryRecord.defraimp_Reason = defraimp_counterhistory_defraimp_reason.;
            counterHistoryRecord.defraimp_PreviousValue = 0;
            counterHistoryRecord.defraimp_CurrentValue = 1;

            _counterHistoryRepo.Create(counterHistoryRecord);
        }
    }
}
