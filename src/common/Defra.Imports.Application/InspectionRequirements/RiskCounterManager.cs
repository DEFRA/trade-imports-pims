namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Logging;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Keeps the counters that an import application is counted against, and writes a counter history record for
    /// each change. A manager is created for one import application by the <see cref="RiskCounterManagerFactory"/>
    /// and is not shared. Each operation does nothing to a counter that the manager does not own, apart from
    /// writing a trace message.
    /// </summary>
    public abstract class RiskCounterManager
    {
        private readonly IOrganizationService service;

        /// <summary>Initializes a new instance of the <see cref="RiskCounterManager"/> class.</summary>
        /// <param name="autonumbers">The autonumber counters, which include the global counter.</param>
        /// <param name="auditor">The auditor that writes the counter history.</param>
        /// <param name="service">The current invocation's organisation service, used to flag a record as counted.</param>
        /// <param name="logger">The log writer.</param>
        protected RiskCounterManager(AutonumberCounters autonumbers, ImportRiskCounterAuditor auditor, IOrganizationService service, ILogWriter logger)
        {
            this.Autonumbers = autonumbers ?? throw new ArgumentNullException(nameof(autonumbers));
            this.Auditor = auditor ?? throw new ArgumentNullException(nameof(auditor));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>Gets the autonumber counters.</summary>
        protected AutonumberCounters Autonumbers { get; }

        /// <summary>Gets the auditor that writes the counter history.</summary>
        protected ImportRiskCounterAuditor Auditor { get; }

        /// <summary>Gets the log writer.</summary>
        protected ILogWriter Logger { get; }

        /// <summary>Counts an import application against its counter.</summary>
        /// <param name="record">The import application.</param>
        /// <param name="reason">The reason for the change.</param>
        public virtual void IncrementNumber(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            this.Logger.Log(Severity.Info, InspectionConstants.LogTag, "Increment because: " + reason);
        }

        /// <summary>Removes an import application from its counter.</summary>
        /// <param name="record">The import application.</param>
        /// <param name="reason">The reason for the change.</param>
        public virtual void DecrementNumber(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            this.Logger.Log(Severity.Info, InspectionConstants.LogTag, "Decrement because: " + reason);
        }

        /// <summary>Sets the counter.</summary>
        /// <param name="record">The import application.</param>
        /// <param name="reason">The reason for the change.</param>
        /// <param name="value">The new value.</param>
        public virtual void SetNumberValue(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason, int value)
        {
            this.Logger.Log(Severity.Info, InspectionConstants.LogTag, "Set number because: " + reason);
        }

        /// <summary>Increases the inspection quota.</summary>
        /// <param name="record">The import application.</param>
        /// <param name="reason">The reason for the change.</param>
        public virtual void IncrementQuota(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            this.Logger.Log(Severity.Info, InspectionConstants.LogTag, "Increment Quota because: " + reason);
        }

        /// <summary>Decreases the inspection quota.</summary>
        /// <param name="record">The import application.</param>
        /// <param name="reason">The reason for the change.</param>
        public virtual void DecrementQuota(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            this.Logger.Log(Severity.Info, InspectionConstants.LogTag, "Decrement Quota because: " + reason);
        }

        /// <summary>Sets the inspection quota.</summary>
        /// <param name="record">The import application.</param>
        /// <param name="reason">The reason for the change.</param>
        /// <param name="value">The new value.</param>
        public virtual void SetQuotaValue(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason, int value)
        {
            this.Logger.Log(Severity.Info, InspectionConstants.LogTag, "Set Quota because: " + reason);
        }

        /// <summary>Increases the global (P3) counter, unless the import application is already counted.</summary>
        /// <param name="record">The import application.</param>
        public void IncrementGlobalCounter(InspectionRecord record)
        {
            if (record.ImportRecordCounted == false)
            {
                var autonumber = this.Autonumbers.GetRecord(InspectionConstants.P3CounterName);
                var detail = CounterTransactionDetail.ForAutonumber(record.Id, autonumber, defraimp_counterhistory_defraimp_operation.IncrementCounter, defraimp_counterhistory_defraimp_reason.GlobalCounter);

                detail.PreviousValue = this.Autonumbers.GetValue(InspectionConstants.P3CounterName);

                this.Logger.Log(Severity.Info, InspectionConstants.LogTag, "Increment Global P3 counter");
                this.Autonumbers.Increment(InspectionConstants.P3CounterName);

                detail.CurrentValue = this.Autonumbers.GetValue(InspectionConstants.P3CounterName);

                this.Auditor.Audit(detail);

                this.SetRecordCounted(record, true);
            }
        }

        /// <summary>Decreases the global (P3) counter, if the import application is counted.</summary>
        /// <param name="record">The import application.</param>
        public void DecrementGlobalCounter(InspectionRecord record)
        {
            if (record.ImportRecordCounted == true)
            {
                var autonumber = this.Autonumbers.GetRecord(InspectionConstants.P3CounterName);
                var detail = CounterTransactionDetail.ForAutonumber(record.Id, autonumber, defraimp_counterhistory_defraimp_operation.DecrementCounter, defraimp_counterhistory_defraimp_reason.GlobalCounter);

                detail.PreviousValue = this.Autonumbers.GetValue(InspectionConstants.P3CounterName);

                this.Logger.Log(Severity.Info, InspectionConstants.LogTag, "Decrement Global P3 counter");
                this.Autonumbers.Decrement(InspectionConstants.P3CounterName);

                detail.CurrentValue = this.Autonumbers.GetValue(InspectionConstants.P3CounterName);

                this.Auditor.Audit(detail);

                this.SetRecordCounted(record, false);
            }
        }

        /// <summary>
        /// Flags the import application as counted or not counted. The update is sent only if the tracked value
        /// differs, and the tracked value is then changed so that the update is not sent twice.
        /// </summary>
        /// <param name="record">The import application.</param>
        /// <param name="counted">Whether the import application is now counted.</param>
        protected void SetRecordCounted(InspectionRecord record, bool counted)
        {
            if (record.ImportRecordCounted != counted)
            {
                this.service.Update(new defraimp_importapplication
                {
                    Id = record.Id,
                    defraimp_ImportRecordCounted = counted,
                });

                record.ImportRecordCounted = counted;
            }
        }
    }
}
