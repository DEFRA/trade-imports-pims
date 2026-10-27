namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.Logging;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Counts import applications of a P1, P2 or P3 risk level (or another risk level that has no counters of its
    /// own) against the autonumber counter and quota counter for the risk level, as well as the global counter.
    /// </summary>
    public sealed class AutonumberRiskCounterManager : RiskCounterManager
    {
        private readonly InspectionQueries queries;
        private readonly string riskLevel;
        private readonly defraimp_autonumber counterAutonumber;
        private readonly defraimp_autonumber quotaAutonumber;

        /// <summary>Initializes a new instance of the <see cref="AutonumberRiskCounterManager"/> class.</summary>
        /// <param name="autonumbers">The autonumber counters.</param>
        /// <param name="auditor">The auditor that writes the counter history.</param>
        /// <param name="service">The current invocation's organisation service.</param>
        /// <param name="logger">The log writer.</param>
        /// <param name="queries">The queries used to find the coverage rule.</param>
        /// <param name="riskLevel">The name of the risk level that is counted against.</param>
        public AutonumberRiskCounterManager(
            AutonumberCounters autonumbers,
            ImportRiskCounterAuditor auditor,
            IOrganizationService service,
            ILogWriter logger,
            InspectionQueries queries,
            string riskLevel)
            : base(autonumbers, auditor, service, logger)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.riskLevel = riskLevel;

            // Both records are read when the manager is created, whether or not they are used.
            this.counterAutonumber = autonumbers.GetRecord(InspectionConstants.GetCounterName(riskLevel));
            this.quotaAutonumber = autonumbers.GetRecord(InspectionConstants.GetQuotaCounterName(riskLevel));
        }

        /// <inheritdoc/>
        public override void IncrementNumber(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            if (!string.IsNullOrEmpty(this.riskLevel))
            {
                if (record.ImportRecordCounted != true)
                {
                    var detail = CounterTransactionDetail.ForAutonumber(record.Id, this.counterAutonumber, defraimp_counterhistory_defraimp_operation.IncrementCounter, reason);
                    detail.PreviousValue = this.Autonumbers.GetValue(InspectionConstants.GetCounterName(this.riskLevel));

                    this.Autonumbers.Increment(InspectionConstants.GetCounterName(this.riskLevel));

                    detail.CurrentValue = this.Autonumbers.GetValue(InspectionConstants.GetCounterName(this.riskLevel));

                    this.Auditor.Audit(detail);

                    if (this.riskLevel.ToLowerInvariant() != InspectionConstants.P3RiskLevelName)
                    {
                        this.IncrementGlobalCounter(record);
                    }

                    this.SetRecordCounted(record, true);
                }
            }

            base.IncrementNumber(record, reason);
        }

        /// <inheritdoc/>
        public override void DecrementNumber(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            if (!string.IsNullOrEmpty(this.riskLevel))
            {
                if (record.ImportRecordCounted == true)
                {
                    if (record.Context.InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.Yes)
                    {
                        this.IncrementQuota(record, reason);
                    }
                    else
                    {
                        var detail = CounterTransactionDetail.ForAutonumber(record.Id, this.counterAutonumber, defraimp_counterhistory_defraimp_operation.DecrementCounter, reason);

                        // TODO: the previous and current values are read from the quota counter, not the counter that is decremented. This should be investigated, but is preserved by the refactoring.
                        detail.PreviousValue = this.Autonumbers.GetValue(InspectionConstants.GetQuotaCounterName(this.riskLevel));

                        this.Autonumbers.Decrement(InspectionConstants.GetCounterName(this.riskLevel));

                        detail.CurrentValue = this.Autonumbers.GetValue(InspectionConstants.GetQuotaCounterName(this.riskLevel));

                        this.Auditor.Audit(detail);
                    }

                    if (this.riskLevel.ToLowerInvariant() != InspectionConstants.P3RiskLevelName)
                    {
                        this.DecrementGlobalCounter(record);
                    }

                    this.SetRecordCounted(record, false);

                    this.BalanceInspectionToNonInspectionAspectRatio(record);
                }
            }

            base.DecrementNumber(record, reason);
        }

        /// <inheritdoc/>
        public override void SetNumberValue(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason, int value)
        {
            if (!string.IsNullOrEmpty(this.riskLevel))
            {
                var detail = CounterTransactionDetail.ForAutonumber(record.Id, this.counterAutonumber, defraimp_counterhistory_defraimp_operation.SetCounter, reason);
                detail.PreviousValue = this.Autonumbers.GetValue(InspectionConstants.GetCounterName(this.riskLevel));

                this.Autonumbers.Set(InspectionConstants.GetCounterName(this.riskLevel), value);

                detail.CurrentValue = this.Autonumbers.GetValue(InspectionConstants.GetCounterName(this.riskLevel));

                this.Auditor.Audit(detail);
            }

            base.SetNumberValue(record, reason, value);
        }

        /// <inheritdoc/>
        public override void IncrementQuota(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            if (!string.IsNullOrEmpty(this.riskLevel))
            {
                var detail = CounterTransactionDetail.ForAutonumber(record.Id, this.quotaAutonumber, defraimp_counterhistory_defraimp_operation.IncrementQuota, reason);
                detail.PreviousValue = this.Autonumbers.GetValue(InspectionConstants.GetQuotaCounterName(this.riskLevel));

                this.Autonumbers.Increment(InspectionConstants.GetQuotaCounterName(this.riskLevel));

                detail.CurrentValue = this.Autonumbers.GetValue(InspectionConstants.GetQuotaCounterName(this.riskLevel));

                this.Auditor.Audit(detail);
            }

            base.IncrementQuota(record, reason);
        }

        /// <inheritdoc/>
        public override void DecrementQuota(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            if (!string.IsNullOrEmpty(this.riskLevel))
            {
                var detail = CounterTransactionDetail.ForAutonumber(record.Id, this.quotaAutonumber, defraimp_counterhistory_defraimp_operation.DecrementQuota, reason);
                detail.PreviousValue = this.Autonumbers.GetValue(InspectionConstants.GetQuotaCounterName(this.riskLevel));

                this.Autonumbers.Decrement(InspectionConstants.GetQuotaCounterName(this.riskLevel));

                detail.CurrentValue = this.Autonumbers.GetValue(InspectionConstants.GetQuotaCounterName(this.riskLevel));

                // TODO: taking a quota flags the record as counted, before its history is written. This should be investigated, but is preserved by the refactoring.
                this.SetRecordCounted(record, true);

                this.Auditor.Audit(detail);
            }

            base.DecrementQuota(record, reason);
        }

        /// <inheritdoc/>
        public override void SetQuotaValue(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason, int value)
        {
            if (!string.IsNullOrEmpty(this.riskLevel))
            {
                var detail = CounterTransactionDetail.ForAutonumber(record.Id, this.quotaAutonumber, defraimp_counterhistory_defraimp_operation.SetQuota, reason);
                detail.PreviousValue = this.Autonumbers.GetValue(InspectionConstants.GetQuotaCounterName(this.riskLevel));

                this.Autonumbers.Set(InspectionConstants.GetQuotaCounterName(this.riskLevel), value);

                detail.CurrentValue = this.Autonumbers.GetValue(InspectionConstants.GetQuotaCounterName(this.riskLevel));

                this.Auditor.Audit(detail);
            }

            base.SetQuotaValue(record, reason, value);
        }

        private void BalanceInspectionToNonInspectionAspectRatio(InspectionRecord record)
        {
            var quotaCounterValue = this.Autonumbers.GetValue(InspectionConstants.GetQuotaCounterName(this.riskLevel));
            var counterValue = this.Autonumbers.GetValue(InspectionConstants.GetCounterName(this.riskLevel));

            var coverageRule = this.queries.FindCoverageRuleByRiskLevel(record.Context.ImportRiskLevel.Id);

            if (coverageRule != null)
            {
                var threshold = coverageRule.defraimp_NumberOfRecordsUntilInspection.Value;
                var negativeThreshold = -threshold;

                if (quotaCounterValue > 0 && counterValue <= negativeThreshold)
                {
                    this.DecrementQuota(record, defraimp_counterhistory_defraimp_reason.NegativeThresholdReachedBalanceQuota);
                    this.SetNumberValue(record, defraimp_counterhistory_defraimp_reason.NegativeThresholdReachedBalanceQuota, 0);
                }
            }
        }
    }
}
