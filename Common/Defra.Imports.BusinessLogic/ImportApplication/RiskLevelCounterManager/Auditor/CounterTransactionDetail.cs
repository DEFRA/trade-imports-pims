using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication
{
    public class CounterTransactionDetail
    {
        // Properties
        public defraimp_importapplication ImportApplication { get; private set; }
        public defraimp_counterhistory_defraimp_counterhistorytype CounterHistoryType { get; private set; }
        public defraimp_counterhistory_defraimp_operation CounterOperation { get; private set; }
        public defraimp_counterhistory_defraimp_reason Reason { get; private set; }
        public int PreviousValue { get; set; }
        public int CurrentValue { get; set; }

        public EntityReference ImportApplicationEntityReference {
            get { return new EntityReference(ImportApplication.LogicalName, ImportApplication.Id); }
            private set { ImportApplicationEntityReference = value; }
        }

        // Member variables
        defraimp_autonumber _autoNumberRecord;
        defraimp_placeoforigin _placeOfOriginRecord;

        public CounterTransactionDetail(defraimp_importapplication importApplication, defraimp_autonumber autoNumberRecord, defraimp_counterhistory_defraimp_operation counterOperation, defraimp_counterhistory_defraimp_reason reason, int previousValue, int currentValue)
        {
            ImportApplication = importApplication;
            CounterHistoryType = defraimp_counterhistory_defraimp_counterhistorytype.AutoNumber;
            CounterOperation = counterOperation;
            _autoNumberRecord = autoNumberRecord;
            Reason = reason;
            PreviousValue = previousValue;
            CurrentValue = currentValue;
        }

        public CounterTransactionDetail(defraimp_importapplication importApplication, defraimp_placeoforigin placeOfOriginRecord, defraimp_counterhistory_defraimp_operation counterOperation, defraimp_counterhistory_defraimp_reason reason, int previousValue, int currentValue)
        {
            ImportApplication = importApplication;
            CounterHistoryType = defraimp_counterhistory_defraimp_counterhistorytype.PlaceOfOrigin;
            CounterOperation = counterOperation;
            _placeOfOriginRecord = placeOfOriginRecord;
            Reason = reason;
            PreviousValue = previousValue;
            CurrentValue = currentValue;
        }

        public EntityReference GetRelatedRecordEntityReference()
        {
            if (CounterHistoryType == defraimp_counterhistory_defraimp_counterhistorytype.AutoNumber)
            {
                if (_autoNumberRecord != null)
                {
                    return new EntityReference(_autoNumberRecord.LogicalName, _autoNumberRecord.Id);
                }
                else
                {
                    return null;
                }
            }
            else if (CounterHistoryType == defraimp_counterhistory_defraimp_counterhistorytype.PlaceOfOrigin)
            {
                if (_placeOfOriginRecord != null)
                {
                    return new EntityReference(_placeOfOriginRecord.LogicalName, _placeOfOriginRecord.Id);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
