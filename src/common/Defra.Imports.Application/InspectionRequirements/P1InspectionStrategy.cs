namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;

    /// <summary>
    /// Inspects P1 import applications. P1 import applications to which the gold/bronze rules apply are inspected
    /// by the trust level of their place of origin; the rest are inspected at random once a threshold is reached.
    /// </summary>
    public sealed class P1InspectionStrategy : InspectionStrategy
    {
        private readonly PlaceOfOriginCounters placeOfOriginCounters;
        private readonly AutonumberCounters autonumbers;

        /// <summary>Initializes a new instance of the <see cref="P1InspectionStrategy"/> class.</summary>
        /// <param name="queries">The queries used to read configuration and coverage rules.</param>
        /// <param name="updater">The updater that records the outcome.</param>
        /// <param name="placeOfOriginCounters">The place of origin counters.</param>
        /// <param name="autonumbers">The autonumber counters.</param>
        public P1InspectionStrategy(
            InspectionQueries queries,
            InspectionRequirementUpdater updater,
            PlaceOfOriginCounters placeOfOriginCounters,
            AutonumberCounters autonumbers)
            : base(queries, updater)
        {
            this.placeOfOriginCounters = placeOfOriginCounters ?? throw new ArgumentNullException(nameof(placeOfOriginCounters));
            this.autonumbers = autonumbers ?? throw new ArgumentNullException(nameof(autonumbers));
        }

        /// <inheritdoc/>
        public override void Execute(InspectionRecord record, RiskCounterManager counters)
        {
            if (this.ValidImportApplicationTypeForInspection(record))
            {
                if (record.ImportRecordCounted != true)
                {
                    if (record.Context.CommodityType != null && record.Context.CountryOfOrigin != null)
                    {
                        if (this.Queries.IsCommodityCoveredByGoldBronze(record.Context.CommodityType, record.Context.CountryOfOrigin))
                        {
                            var placeOfOrigin = this.GetPlaceOfOrigin(record);

                            if (placeOfOrigin != null)
                            {
                                var coverageRule = this.GetCoverageRule(InspectionConstants.GoldBronzeCoverageRuleKey);

                                if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Gold)
                                {
                                    // TODO: a missing gold/bronze coverage rule or threshold fails here. This should be investigated, but is preserved by the refactoring.
                                    this.GoldInspection(record, counters, placeOfOrigin, (int)coverageRule.defraimp_NumberOfRecordsUntilInspection);
                                }
                                else if (placeOfOrigin.defraimp_TrustLevel == defraimp_trustlevel.Bronze)
                                {
                                    this.BronzeInspection(record, counters, placeOfOrigin);
                                }
                            }
                            else
                            {
                                this.Updater.PlaceOfOriginMissing(record.Id);
                            }
                        }
                        else
                        {
                            this.P1Inspection(record, counters);
                        }
                    }
                    else
                    {
                        this.Updater.RiskLevelUnknown(record.Id);
                    }
                }
            }
            else
            {
                this.MissingCertificateError(record);
            }
        }

        private defraimp_placeoforigin GetPlaceOfOrigin(InspectionRecord record)
        {
            return record.Context.PlaceOfOrigin != null
                ? this.placeOfOriginCounters.Find(record.Context.PlaceOfOrigin.Id)
                : null;
        }

        private void P1Inspection(InspectionRecord record, RiskCounterManager counters)
        {
            var quotaCount = this.autonumbers.GetValue(InspectionConstants.P1QuotaCounterName);

            if (quotaCount > 0)
            {
                // Count the record against the global counter first, then take the quota.
                counters.IncrementGlobalCounter(record);
                counters.DecrementQuota(record, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection);
                this.Updater.P1Inspection(record.Id);
            }
            else
            {
                var coverageRule = this.GetCoverageRule(InspectionConstants.P1CoverageRuleKey);

                counters.IncrementNumber(record, defraimp_counterhistory_defraimp_reason.ValidP1);

                var currentCount = this.autonumbers.GetValue(InspectionConstants.P1CounterName);
                if (currentCount >= coverageRule.defraimp_NumberOfRecordsUntilInspection)
                {
                    counters.SetNumberValue(record, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection, 0);

                    this.Updater.P1Inspection(record.Id);
                }
                else
                {
                    this.Updater.NoInspectionRequired(record.Id);
                }
            }
        }

        private void GoldInspection(InspectionRecord record, RiskCounterManager counters, defraimp_placeoforigin placeOfOrigin, int coverageRuleValue)
        {
            counters.IncrementNumber(record, defraimp_counterhistory_defraimp_reason.ValidGB);

            // The local copy of the place of origin is increased too, so that it need not be read again.
            placeOfOrigin.defraimp_ApplicationCounter += 1;

            var quotaCount = this.placeOfOriginCounters.GetQuotaCounterValue(placeOfOrigin.Id);

            if (placeOfOrigin.defraimp_ApplicationCounter >= coverageRuleValue)
            {
                counters.IncrementQuota(record, defraimp_counterhistory_defraimp_reason.CounterTargetReachedQuotaIncrement);
                quotaCount += 1;

                counters.SetNumberValue(record, defraimp_counterhistory_defraimp_reason.CounterTargetReachedQuotaIncrement, 0);
            }

            if (quotaCount > 0)
            {
                counters.DecrementQuota(record, defraimp_counterhistory_defraimp_reason.WasFlaggedforInspection);

                this.Updater.GoldCoverageInspection(record.Id);
            }
            else
            {
                this.Updater.NoInspectionGold(record.Id);
            }
        }

        private void BronzeInspection(InspectionRecord record, RiskCounterManager counters, defraimp_placeoforigin placeOfOrigin)
        {
            counters.IncrementGlobalCounter(record);

            if (placeOfOrigin.defraimp_LocktoBronze == true)
            {
                this.Updater.LockedToBronzeInspection(record.Id);
            }
            else
            {
                this.Updater.BronzeInspection(record.Id);
            }
        }
    }
}
