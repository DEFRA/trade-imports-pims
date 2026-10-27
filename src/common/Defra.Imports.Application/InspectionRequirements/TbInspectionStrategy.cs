namespace Defra.Imports.Application.InspectionRequirements
{
    using Defra.Imports.Dataverse.Queries;

    /// <summary>
    /// Inspects every TB import application, counting it against the global counter.
    /// </summary>
    public sealed class TbInspectionStrategy : InspectionStrategy
    {
        /// <summary>Initializes a new instance of the <see cref="TbInspectionStrategy"/> class.</summary>
        /// <param name="queries">The queries used to read configuration.</param>
        /// <param name="updater">The updater that records the outcome.</param>
        public TbInspectionStrategy(InspectionQueries queries, InspectionRequirementUpdater updater)
            : base(queries, updater)
        {
        }

        /// <inheritdoc/>
        public override void Execute(InspectionRecord record, RiskCounterManager counters)
        {
            if (this.ValidImportApplicationTypeForInspection(record))
            {
                if (record.ImportRecordCounted != true)
                {
                    counters.IncrementGlobalCounter(record);

                    this.Updater.TbInspection(record.Id);
                }
            }
            else
            {
                this.MissingCertificateError(record);
            }
        }
    }
}
