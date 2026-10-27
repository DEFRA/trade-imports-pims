namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.Logging;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Chooses and creates the counter manager for an import application: by place of origin if the gold/bronze
    /// rules apply, by the global counter only for TB, and by the risk level's own autonumbers otherwise.
    /// </summary>
    public sealed class RiskCounterManagerFactory
    {
        private readonly InspectionQueries queries;
        private readonly AutonumberCounters autonumbers;
        private readonly PlaceOfOriginCounters placeOfOriginCounters;
        private readonly ImportRiskCounterAuditor auditor;
        private readonly IOrganizationService service;
        private readonly ILogWriter logger;

        /// <summary>Initializes a new instance of the <see cref="RiskCounterManagerFactory"/> class.</summary>
        /// <param name="queries">The queries used to read the risk level and the gold/bronze rules.</param>
        /// <param name="autonumbers">The autonumber counters.</param>
        /// <param name="placeOfOriginCounters">The place of origin counters.</param>
        /// <param name="auditor">The auditor that writes the counter history.</param>
        /// <param name="service">The current invocation's organisation service.</param>
        /// <param name="logger">The log writer.</param>
        public RiskCounterManagerFactory(
            InspectionQueries queries,
            AutonumberCounters autonumbers,
            PlaceOfOriginCounters placeOfOriginCounters,
            ImportRiskCounterAuditor auditor,
            IOrganizationService service,
            ILogWriter logger)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.autonumbers = autonumbers ?? throw new ArgumentNullException(nameof(autonumbers));
            this.placeOfOriginCounters = placeOfOriginCounters ?? throw new ArgumentNullException(nameof(placeOfOriginCounters));
            this.auditor = auditor ?? throw new ArgumentNullException(nameof(auditor));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Creates the counter manager for an import application. The risk level is read by identifier, because
        /// the name of a lookup on a pre-image is not reliable. The reads happen whether or not the manager is
        /// used.
        /// </summary>
        /// <param name="record">The import application.</param>
        /// <returns>The manager, or null if the import application is null or has no risk level.</returns>
        public RiskCounterManager Create(InspectionRecord record)
        {
            if (record == null)
            {
                return null;
            }

            var riskLevel = record.Context.ImportRiskLevel != null
                ? this.queries.GetImportRiskLevelName(record.Context.ImportRiskLevel.Id)
                : string.Empty;

            if (string.IsNullOrEmpty(riskLevel))
            {
                return null;
            }

            if (this.queries.IsCoveredByGoldBronze(record.Context))
            {
                return new PlaceOfOriginRiskCounterManager(this.autonumbers, this.auditor, this.service, this.logger, this.placeOfOriginCounters, this.queries, record);
            }

            if (riskLevel.ToLowerInvariant() == InspectionConstants.TbRiskLevelName)
            {
                return new OnlyGlobalCounterRiskCounterManager(this.autonumbers, this.auditor, this.service, this.logger, riskLevel);
            }

            return new AutonumberRiskCounterManager(this.autonumbers, this.auditor, this.service, this.logger, this.queries, riskLevel);
        }
    }
}
