using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication.RiskLevelCounterManager
{
    class CounterHistoryDetail
    {
        public defraimp_counterhistory_defraimp_counterhistorytype CounterHistoryType { get; private set; }
        public defraimp_counterhistory_defraimp_operation CounterOperation { get; private set; }
        public defraimp_counterhistory_defraimp_reason Reason { get; private set; }
        public int PreviousValue { get; private set; }
        public int CurrentValue { get; private set; }

        defraimp_autonumber _autoNumberRecord;
        defraimp_placeoforigin _placeOfOriginRecord;

        public CounterHistoryDetail(defraimp_autonumber autoNumberRecord, defraimp_counterhistory_defraimp_operation counterOperation, defraimp_counterhistory_defraimp_reason reason, int previousValue, int currentValue)
        {
            CounterHistoryType = defraimp_counterhistory_defraimp_counterhistorytype.AutoNumber;
            CounterOperation = counterOperation;
            _autoNumberRecord = autoNumberRecord;
            Reason = reason;
            PreviousValue = previousValue;
            CurrentValue = currentValue;
        }

        public CounterHistoryDetail(defraimp_placeoforigin placeOfOriginRecord, defraimp_counterhistory_defraimp_operation counterOperation, defraimp_counterhistory_defraimp_reason reason, int previousValue, int currentValue)
        {
            CounterHistoryType = defraimp_counterhistory_defraimp_counterhistorytype.PlaceOfOrigin;
            CounterOperation = counterOperation;
            _placeOfOriginRecord = placeOfOriginRecord;
            Reason = reason;
            PreviousValue = previousValue;
            CurrentValue = currentValue;
        }

        public EntityReference GetRelatedRecord()
        {

        }
    }
}
