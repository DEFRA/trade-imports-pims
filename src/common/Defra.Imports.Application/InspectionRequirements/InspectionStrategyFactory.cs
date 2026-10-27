namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Queries;

    /// <summary>
    /// Chooses the inspection strategy for a risk level.
    /// </summary>
    public sealed class InspectionStrategyFactory
    {
        private readonly InspectionStrategy p1;
        private readonly InspectionStrategy p2;
        private readonly InspectionStrategy p3;
        private readonly InspectionStrategy tb;
        private readonly InspectionStrategy unknown;

        /// <summary>Initializes a new instance of the <see cref="InspectionStrategyFactory"/> class.</summary>
        /// <param name="queries">The queries used to read configuration and coverage rules.</param>
        /// <param name="updater">The updater that records the outcome.</param>
        /// <param name="autonumbers">The autonumber counters.</param>
        /// <param name="placeOfOriginCounters">The place of origin counters.</param>
        public InspectionStrategyFactory(
            InspectionQueries queries,
            InspectionRequirementUpdater updater,
            AutonumberCounters autonumbers,
            PlaceOfOriginCounters placeOfOriginCounters)
        {
            this.p1 = new P1InspectionStrategy(queries, updater, placeOfOriginCounters, autonumbers);
            this.p2 = new P2InspectionStrategy(queries, updater, autonumbers);
            this.p3 = new P3InspectionStrategy(queries, updater, autonumbers);
            this.tb = new TbInspectionStrategy(queries, updater);
            this.unknown = new UnknownInspectionStrategy(queries, updater);
        }

        /// <summary>Gets the strategy for a risk level.</summary>
        /// <param name="riskLevel">The name of the risk level, in any case. It must not be null.</param>
        /// <returns>The strategy, which records an unknown risk level if the name is not recognised.</returns>
        public InspectionStrategy GetStrategy(string riskLevel)
        {
            switch (riskLevel.ToLowerInvariant())
            {
                case InspectionConstants.P1RiskLevelName:
                    return this.p1;
                case InspectionConstants.P2RiskLevelName:
                    return this.p2;
                case InspectionConstants.P3RiskLevelName:
                    return this.p3;
                case InspectionConstants.TbRiskLevelName:
                    return this.tb;
                default:
                    return this.unknown;
            }
        }
    }
}
