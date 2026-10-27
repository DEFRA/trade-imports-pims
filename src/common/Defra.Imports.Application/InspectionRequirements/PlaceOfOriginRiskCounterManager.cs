namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.Logging;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Counts import applications that the gold/bronze rules apply to against the application counter and quota
    /// counter of their place of origin, as well as the global counter. Nothing is counted against the place of
    /// origin if the import application has none or it is not found.
    /// </summary>
    public sealed class PlaceOfOriginRiskCounterManager : RiskCounterManager
    {
        private readonly PlaceOfOriginCounters counters;
        private readonly InspectionQueries queries;
        private readonly defraimp_placeoforigin placeOfOrigin;

        /// <summary>Initializes a new instance of the <see cref="PlaceOfOriginRiskCounterManager"/> class.</summary>
        /// <param name="autonumbers">The autonumber counters, which include the global counter.</param>
        /// <param name="auditor">The auditor that writes the counter history.</param>
        /// <param name="service">The current invocation's organisation service.</param>
        /// <param name="logger">The log writer.</param>
        /// <param name="counters">The place of origin counters.</param>
        /// <param name="queries">The queries used to find the gold/bronze coverage rule.</param>
        /// <param name="record">The import application that the manager is created for.</param>
        public PlaceOfOriginRiskCounterManager(
            AutonumberCounters autonumbers,
            ImportRiskCounterAuditor auditor,
            IOrganizationService service,
            ILogWriter logger,
            PlaceOfOriginCounters counters,
            InspectionQueries queries,
            InspectionRecord record)
            : base(autonumbers, auditor, service, logger)
        {
            this.counters = counters ?? throw new ArgumentNullException(nameof(counters));
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));

            if (record.Context.PlaceOfOrigin != null)
            {
                this.placeOfOrigin = counters.Find(record.Context.PlaceOfOrigin.Id);
            }
        }

        /// <inheritdoc/>
        public override void IncrementNumber(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            if (this.placeOfOrigin != null)
            {
                if (record.ImportRecordCounted != true)
                {
                    var detail = CounterTransactionDetail.ForPlaceOfOrigin(record.Id, this.placeOfOrigin, defraimp_counterhistory_defraimp_operation.IncrementCounter, reason);
                    detail.PreviousValue = this.counters.GetApplicationCounterValue(this.placeOfOrigin.Id);

                    this.counters.IncrementApplicationCounter(this.placeOfOrigin.Id);

                    detail.CurrentValue = this.counters.GetApplicationCounterValue(this.placeOfOrigin.Id);

                    this.Auditor.Audit(detail);

                    this.IncrementGlobalCounter(record);

                    this.SetRecordCounted(record, true);
                }
            }

            base.IncrementNumber(record, reason);
        }

        /// <inheritdoc/>
        public override void DecrementNumber(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            if (this.placeOfOrigin != null)
            {
                if (record.ImportRecordCounted == true)
                {
                    if (record.Context.InspectionRequiredReason == defraimp_importapplication_defraimp_inspectionrequiredreason.GoldPlaceofOriginInspectionCoverage)
                    {
                        this.IncrementQuota(record, reason);
                    }
                    else
                    {
                        var detail = CounterTransactionDetail.ForPlaceOfOrigin(record.Id, this.placeOfOrigin, defraimp_counterhistory_defraimp_operation.DecrementCounter, reason);
                        detail.PreviousValue = this.counters.GetApplicationCounterValue(this.placeOfOrigin.Id);

                        this.counters.DecrementApplicationCounter(this.placeOfOrigin.Id);

                        detail.CurrentValue = this.counters.GetApplicationCounterValue(this.placeOfOrigin.Id);

                        this.Auditor.Audit(detail);
                    }

                    this.DecrementGlobalCounter(record);

                    this.SetRecordCounted(record, false);

                    this.BalanceInspectionToNonInspectionAspectRatio(record);
                }
            }

            base.DecrementNumber(record, reason);
        }

        /// <inheritdoc/>
        public override void SetNumberValue(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason, int value)
        {
            if (this.placeOfOrigin != null)
            {
                var detail = CounterTransactionDetail.ForPlaceOfOrigin(record.Id, this.placeOfOrigin, defraimp_counterhistory_defraimp_operation.SetCounter, reason);
                detail.PreviousValue = this.counters.GetApplicationCounterValue(this.placeOfOrigin.Id);

                this.counters.SetApplicationCounter(this.placeOfOrigin.Id, value);

                detail.CurrentValue = this.counters.GetApplicationCounterValue(this.placeOfOrigin.Id);

                this.Auditor.Audit(detail);
            }

            base.SetNumberValue(record, reason, value);
        }

        /// <inheritdoc/>
        public override void IncrementQuota(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            if (this.placeOfOrigin != null)
            {
                var detail = CounterTransactionDetail.ForPlaceOfOrigin(record.Id, this.placeOfOrigin, defraimp_counterhistory_defraimp_operation.IncrementQuota, reason);
                detail.PreviousValue = this.counters.GetQuotaCounterValue(this.placeOfOrigin.Id);

                this.counters.IncrementQuotaCounter(this.placeOfOrigin.Id);

                detail.CurrentValue = this.counters.GetQuotaCounterValue(this.placeOfOrigin.Id);

                this.Auditor.Audit(detail);
            }

            base.IncrementQuota(record, reason);
        }

        /// <inheritdoc/>
        public override void DecrementQuota(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            if (this.placeOfOrigin != null)
            {
                var detail = CounterTransactionDetail.ForPlaceOfOrigin(record.Id, this.placeOfOrigin, defraimp_counterhistory_defraimp_operation.DecrementQuota, reason);
                detail.PreviousValue = this.counters.GetQuotaCounterValue(this.placeOfOrigin.Id);

                this.counters.DecrementQuotaCounter(this.placeOfOrigin.Id);

                detail.CurrentValue = this.counters.GetQuotaCounterValue(this.placeOfOrigin.Id);

                this.Auditor.Audit(detail);

                // TODO: taking a quota flags the record as counted. This should be investigated, but is preserved by the refactoring.
                this.SetRecordCounted(record, true);
            }

            base.DecrementQuota(record, reason);
        }

        /// <inheritdoc/>
        public override void SetQuotaValue(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason, int value)
        {
            if (this.placeOfOrigin != null)
            {
                var detail = CounterTransactionDetail.ForPlaceOfOrigin(record.Id, this.placeOfOrigin, defraimp_counterhistory_defraimp_operation.SetQuota, reason);
                detail.PreviousValue = this.counters.GetQuotaCounterValue(this.placeOfOrigin.Id);

                // TODO: setting the quota sets the application counter, not the quota counter. This should be investigated, but is preserved by the refactoring.
                this.counters.SetApplicationCounter(this.placeOfOrigin.Id, value);

                detail.CurrentValue = this.counters.GetQuotaCounterValue(this.placeOfOrigin.Id);

                this.Auditor.Audit(detail);
            }

            base.SetQuotaValue(record, reason, value);
        }

        private void BalanceInspectionToNonInspectionAspectRatio(InspectionRecord record)
        {
            var quotaCounterValue = this.counters.GetQuotaCounterValue(this.placeOfOrigin.Id);
            var counterValue = this.counters.GetApplicationCounterValue(this.placeOfOrigin.Id);

            var coverageRule = this.queries.FindCoverageRuleByKey(InspectionConstants.GoldBronzeCoverageRuleKey);

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
