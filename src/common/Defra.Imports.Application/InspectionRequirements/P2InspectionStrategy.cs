namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;

    /// <summary>
    /// Inspects P2 import applications at random, once enough have been counted since the last inspection or if a
    /// quota of inspections is outstanding.
    /// </summary>
    public sealed class P2InspectionStrategy : InspectionStrategy
    {
        private readonly AutonumberCounters autonumbers;

        /// <summary>Initializes a new instance of the <see cref="P2InspectionStrategy"/> class.</summary>
        /// <param name="queries">The queries used to read configuration and coverage rules.</param>
        /// <param name="updater">The updater that records the outcome.</param>
        /// <param name="autonumbers">The autonumber counters.</param>
        public P2InspectionStrategy(InspectionQueries queries, InspectionRequirementUpdater updater, AutonumberCounters autonumbers)
            : base(queries, updater)
        {
            this.autonumbers = autonumbers ?? throw new ArgumentNullException(nameof(autonumbers));
        }

        /// <inheritdoc/>
        public override void Execute(InspectionRecord record, RiskCounterManager counters)
        {
            if (this.ValidImportApplicationTypeForInspection(record))
            {
                if (record.ImportRecordCounted != true)
                {
                    var quotaCount = this.autonumbers.GetValue(InspectionConstants.P2QuotaCounterName);

                    if (quotaCount > 0)
                    {
                        counters.IncrementGlobalCounter(record);
                        counters.DecrementQuota(record, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection);
                        this.Updater.P2Inspection(record.Id);
                    }
                    else
                    {
                        var coverageRule = this.GetCoverageRule(InspectionConstants.P2CoverageRuleKey);

                        counters.IncrementNumber(record, defraimp_counterhistory_defraimp_reason.ValidP2);

                        var currentCount = this.autonumbers.GetValue(InspectionConstants.P2CounterName);
                        if (currentCount >= coverageRule.defraimp_NumberOfRecordsUntilInspection)
                        {
                            counters.SetNumberValue(record, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection, 0);

                            this.Updater.P2Inspection(record.Id);
                        }
                        else
                        {
                            this.Updater.NoInspectionRequired(record.Id);
                        }
                    }
                }
            }
            else
            {
                this.MissingCertificateError(record);
            }
        }
    }
}
