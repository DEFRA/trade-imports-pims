namespace Defra.Imports.Application.InspectionRequirements
{
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Logging;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Counts import applications of a risk level that has no counters of its own (TB) against the global counter
    /// only. The global counter is also the P3 counter, so a P3 import application is not counted twice.
    /// </summary>
    public sealed class OnlyGlobalCounterRiskCounterManager : RiskCounterManager
    {
        private readonly string riskLevel;

        /// <summary>Initializes a new instance of the <see cref="OnlyGlobalCounterRiskCounterManager"/> class.</summary>
        /// <param name="autonumbers">The autonumber counters.</param>
        /// <param name="auditor">The auditor that writes the counter history.</param>
        /// <param name="service">The current invocation's organisation service.</param>
        /// <param name="logger">The log writer.</param>
        /// <param name="riskLevel">The name of the risk level that is counted against.</param>
        public OnlyGlobalCounterRiskCounterManager(
            AutonumberCounters autonumbers,
            ImportRiskCounterAuditor auditor,
            IOrganizationService service,
            ILogWriter logger,
            string riskLevel)
            : base(autonumbers, auditor, service, logger)
        {
            this.riskLevel = riskLevel;
        }

        /// <inheritdoc/>
        public override void IncrementNumber(InspectionRecord record, defraimp_counterhistory_defraimp_reason reason)
        {
            if (!string.IsNullOrEmpty(this.riskLevel))
            {
                if (record.ImportRecordCounted != true)
                {
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
                    if (this.riskLevel.ToLowerInvariant() != InspectionConstants.P3RiskLevelName)
                    {
                        this.DecrementGlobalCounter(record);
                    }

                    this.SetRecordCounted(record, false);
                }
            }

            base.DecrementNumber(record, reason);
        }
    }
}
