namespace Defra.Imports.BusinessLogic.ImportApplication
{
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

    public abstract class AbstractRiskCounterManager
    {
        protected ICrmRepository<defraimp_importapplication> _importApplicationRepo;
        protected ICrmRepository<defraimp_inspectioncoveragerule> _coverageRulesRepo;
        protected IAutonumberRepository _autoNumberRepo;
        protected ILogWriter _logWriter;
        protected AbstractCounterTransactionDetailFactory _abstractCounterTransactionDetailFactory;

        public delegate void CounterDelegate(CounterTransactionDetail counterTransactionDetail);
        public event CounterDelegate CounterTransactionEvent;

        public virtual void IncrementNumber(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Increment because: " + counterTransactionReason);
        }

        public virtual void DecrementNumber(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Decrement because: " + counterTransactionReason);
        }
        public virtual void SetNumberValue(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason, int value)
        {
            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Set number because: " + counterTransactionReason);
        }

        public virtual void IncrementQuota(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Increment Quota because: " + counterTransactionReason);
        }

        public virtual void DecrementQuota(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason)
        {
            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Decrement Quota because: " + counterTransactionReason);
        }

        public virtual void SetQuotaValue(ref defraimp_importapplication importApplication, defraimp_counterhistory_defraimp_reason counterTransactionReason, int value)
        {
            //Broadcast Stuff
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Set Quota because: " + counterTransactionReason);
        }

        protected void SetRecordCounted(ref defraimp_importapplication importApplication, bool counted)
        {
            defraimp_importapplication updatedImportApplication = new defraimp_importapplication();
            updatedImportApplication.Id = importApplication.Id;
            updatedImportApplication.defraimp_ImportRecordCounted = counted;
            _importApplicationRepo.Update(updatedImportApplication);

            //Set the local value to ensure we don't do an operation twice
            importApplication.defraimp_ImportRecordCounted = counted;
        }

        protected void BroadcastCounterTransactionEvent(CounterTransactionDetail counterTransactionDetail)
        {
            // Call the counter transaction event
            CounterTransactionEvent(counterTransactionDetail);
        }

        public void IncrementGlobalCounter(defraimp_importapplication importApplication)
        {
            defraimp_autonumber autoNumberRecord = _autoNumberRepo.GetAutonumberWithKey(ImportApplicationConstants.P3_COUNTER_NAME);
            // Create a new counterTransactionDetail record and populate it
            CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, autoNumberRecord, defraimp_counterhistory_defraimp_operation.Increment, defraimp_counterhistory_defraimp_reason.GlobalCounter);

            // Set the previous value before the operation
            counterTransactionDetail.PreviousValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME);

            // Increment the P3 global counter
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Increment Global P3 counter");
            _autoNumberRepo.IncrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);

            // Get the current value after the operation
            counterTransactionDetail.CurrentValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME);

            // Broadcast the message so that the auditor can pick it up
            BroadcastCounterTransactionEvent(counterTransactionDetail);
        }

        public void DecrementGlobalCounter(defraimp_importapplication importApplication)
        {
            defraimp_autonumber autoNumberRecord = _autoNumberRepo.GetAutonumberWithKey(ImportApplicationConstants.P3_COUNTER_NAME);
            // Create a new counterTransactionDetail record and populate it
            CounterTransactionDetail counterTransactionDetail = _abstractCounterTransactionDetailFactory.GetCounterTransactionDetail(importApplication, autoNumberRecord, defraimp_counterhistory_defraimp_operation.Decrement, defraimp_counterhistory_defraimp_reason.GlobalCounter);

            // Set the previous value before the operation
            counterTransactionDetail.PreviousValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME);

            // Increment the P3 global counter
            _logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Decrement Global P3 counter");
            _autoNumberRepo.DecrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME);

            // Get the current value after the operation
            counterTransactionDetail.CurrentValue = _autoNumberRepo.GetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME);

            // Broadcast the message so that the auditor can pick it up
            BroadcastCounterTransactionEvent(counterTransactionDetail);
        }
    }
}
