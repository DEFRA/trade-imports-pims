namespace Defra.Imports.Application.WatchLists
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Dataverse.Queries;
    using Microsoft.Xrm.Sdk;

    /// <inheritdoc/>
    public sealed class WatchFlagCreator : IWatchFlagCreator
    {
        // For each type of watch list, the lookup that names its economic operator.
        private static readonly IReadOnlyDictionary<defraimp_watchtype, string> WatchListLookups =
            new Dictionary<defraimp_watchtype, string>
            {
                [defraimp_watchtype.PlaceofOrigin] = defraimp_WatchList.Fields.defraimp_PlaceOfOriginId,
                [defraimp_watchtype.PlaceofDestination] = defraimp_WatchList.Fields.defraimp_PlaceOfDestinationId,
                [defraimp_watchtype.Consignee] = defraimp_WatchList.Fields.defraimp_ConsigneeId,
                [defraimp_watchtype.Transporter] = defraimp_WatchList.Fields.defraimp_TransporterId,
                [defraimp_watchtype.Veterinarian] = defraimp_WatchList.Fields.defraimp_VeterinarianId,
            };

        private readonly WatchFlagQueries queries;
        private readonly IOrganizationService service;

        /// <summary>Initializes a new instance of the <see cref="WatchFlagCreator"/> class.</summary>
        /// <param name="queries">The Dataverse queries used to find watch lists and economic operators.</param>
        /// <param name="service">The current invocation's organisation service, used to create the watch flags.</param>
        public WatchFlagCreator(WatchFlagQueries queries, IOrganizationService service)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <inheritdoc/>
        public void CreateWatchFlags(CreateItahcWatchFlagsContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            // The order is the order in which the identifiers are checked and flags created.
            var identifiers = new[]
            {
                new Identifier(defraimp_watchtype.PlaceofOrigin, context.PlaceOfOrigin),
                new Identifier(defraimp_watchtype.PlaceofDestination, context.PlaceOfDestination),
                new Identifier(defraimp_watchtype.Consignee, context.Consignee),
                new Identifier(defraimp_watchtype.Transporter, context.Transporter),
                new Identifier(defraimp_watchtype.Veterinarian, context.Veterinarian),
            };

            this.CreateWatchFlags(identifiers, context.Name, flag => flag.defraimp_ItahcId = Copy(context.Itahc));
        }

        /// <inheritdoc/>
        public void CreateWatchFlags(CreateNotificationWatchFlagsContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var identifiers = new[]
            {
                new Identifier(defraimp_watchtype.PlaceofOrigin, context.Consignor),
                new Identifier(defraimp_watchtype.PlaceofDestination, context.PlaceOfDestination),
                new Identifier(defraimp_watchtype.Consignee, context.Importer),
                new Identifier(defraimp_watchtype.Transporter, context.Transporter),
            };

            this.CreateWatchFlags(identifiers, context.Name, flag => flag.defraimp_ImporterNotificationId = Copy(context.Notification));
        }

        // Each watch flag gets its own reference, so no two flags share a mutable instance.
        private static EntityReference Copy(EntityReference reference)
        {
            return new EntityReference(reference.LogicalName, reference.Id);
        }

        private void CreateWatchFlags(IEnumerable<Identifier> identifiers, string recordName, Action<defraimp_WatchFlag> linkRecord)
        {
            // TODO: DateTime.Now is the server's local time, not UTC, so the watch lists that are considered active
            // can differ around midnight. This should be investigated, but the refactoring preserves it.
            // The watch lists are read once, even when no identifier is given, as the legacy plug-in did.
            var watchLists = this.queries.GetActiveWatchLists(DateTime.Now.Date);

            foreach (var identifier in identifiers.Where(value => !string.IsNullOrEmpty(value.Value)))
            {
                var lookup = WatchListLookups[identifier.WatchType];

                var candidates = watchLists
                    .Where(candidate => candidate.defraimp_WatchType == identifier.WatchType
                        && candidate.Attributes.Contains(lookup)
                        && candidate[lookup] != null)
                    .ToList();
                if (candidates.Count == 0)
                {
                    continue;
                }

                // TODO: the economic operator lookup has no ordering, so when several operators share a name only the
                // first one returned is checked. This should be investigated, but the refactoring preserves it.
                var economicOperatorId = this.queries.FindEconomicOperatorId(identifier.WatchType, identifier.Value);
                if (economicOperatorId == null)
                {
                    continue;
                }

                // TODO: only the first matching watch list is flagged, and nothing prevents a duplicate flag when the
                // same operator is named twice or the step runs again. This should be investigated, but the
                // refactoring preserves it.
                var watchList = candidates.FirstOrDefault(candidate => ((EntityReference)candidate[lookup]).Id == economicOperatorId.Value);
                if (watchList != null)
                {
                    this.CreateWatchFlag(watchList, recordName, linkRecord);
                }
            }
        }

        private void CreateWatchFlag(defraimp_WatchList watchList, string recordName, Action<defraimp_WatchFlag> linkRecord)
        {
            // The name uses the record's name as supplied on the Target, which is empty when Create omitted it. This
            // should be investigated, but the refactoring preserves it.
            var watchFlag = new defraimp_WatchFlag
            {
                defraimp_Name = $"{watchList.defraimp_Name} - {recordName}",
                defraimp_WatchListId = watchList.ToEntityReference(),
            };
            linkRecord(watchFlag);

            this.service.Create(watchFlag);
        }

        private sealed class Identifier
        {
            public Identifier(defraimp_watchtype watchType, string value)
            {
                this.WatchType = watchType;
                this.Value = value;
            }

            public defraimp_watchtype WatchType { get; }

            public string Value { get; }
        }
    }
}
