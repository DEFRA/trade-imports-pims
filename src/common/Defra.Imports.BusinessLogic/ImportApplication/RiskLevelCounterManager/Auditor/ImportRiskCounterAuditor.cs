using Defra.Imports.Model;
using Defra.Imports.Repositories;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    class ImportRiskCounterAuditor : IImportRiskCounterAuditor
    {
        AbstractRiskCounterManager _riskLevelCounterManager;
        ICrmRepository<defraimp_counterhistory> _counterHistoryRepo;

        public ImportRiskCounterAuditor(AbstractRiskCounterManager riskLevelCounterManager, ICrmRepository<defraimp_counterhistory> counterHistoryRepo)
        {
            _riskLevelCounterManager = riskLevelCounterManager;
            _counterHistoryRepo = counterHistoryRepo;

            //Subscribe to the events we care about
            _riskLevelCounterManager.CounterTransactionEvent += AuditCounterEvent;
        }

        public void AuditCounterEvent(CounterTransactionDetail counterTransactionDetail)
        {
            defraimp_counterhistory counterHistoryRecord = new defraimp_counterhistory();

            counterHistoryRecord.defraimp_ImportApplicationId = counterTransactionDetail.ImportApplicationEntityReference;
            counterHistoryRecord.defraimp_CounterHistoryType = counterTransactionDetail.CounterHistoryType;
            counterHistoryRecord.defraimp_Operation = counterTransactionDetail.CounterOperation;
            counterHistoryRecord.defraimp_Reason = counterTransactionDetail.Reason;
            counterHistoryRecord.defraimp_PreviousValue = counterTransactionDetail.PreviousValue;
            counterHistoryRecord.defraimp_CurrentValue = counterTransactionDetail.CurrentValue;

            if (counterTransactionDetail.CounterHistoryType == defraimp_counterhistory_defraimp_counterhistorytype.AutoNumber)
            {
                counterHistoryRecord.defraimp_AutoNumberId = counterTransactionDetail.GetRelatedRecordEntityReference();
            }
            else if (counterTransactionDetail.CounterHistoryType == defraimp_counterhistory_defraimp_counterhistorytype.PlaceOfOrigin)
            {
                counterHistoryRecord.defraimp_PlaceOfOriginId = counterTransactionDetail.GetRelatedRecordEntityReference();
            }

            _counterHistoryRepo.Create(counterHistoryRecord);
        }
    }
}
