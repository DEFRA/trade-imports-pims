namespace Defra.Imports.Application.InspectionRequirements
{
    using Defra.Imports.Dataverse.Queries;

    /// <summary>
    /// Records that the inspection requirement of an import application cannot be determined because its risk
    /// level is not one that has rules.
    /// </summary>
    public sealed class UnknownInspectionStrategy : InspectionStrategy
    {
        /// <summary>Initializes a new instance of the <see cref="UnknownInspectionStrategy"/> class.</summary>
        /// <param name="queries">The queries used to read configuration.</param>
        /// <param name="updater">The updater that records the outcome.</param>
        public UnknownInspectionStrategy(InspectionQueries queries, InspectionRequirementUpdater updater)
            : base(queries, updater)
        {
        }

        /// <inheritdoc/>
        public override void Execute(InspectionRecord record, RiskCounterManager counters)
        {
            this.Updater.RiskLevelUnknown(record.Id);
        }
    }
}
