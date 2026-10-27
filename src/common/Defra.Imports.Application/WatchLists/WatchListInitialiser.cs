namespace Defra.Imports.Application.WatchLists
{
    using System;
    using Defra.Imports.Dataverse;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// ILLUSTRATIVE EXAMPLE of the pattern in architecture/adr/0001-plugin-workflow-di-scaffold.md: a targeted
    /// application service that never mutates the entity it is given. It is not a real business process. It must
    /// not be registered with Dataverse or included in a solution, and must not be extended or copied as a
    /// template for entity or business choices.
    /// </summary>
    public sealed class WatchListInitialiser : IWatchListInitialiser
    {
        private readonly WatchListQueries queries;
        private readonly IOrganizationService service;

        /// <summary>Initializes a new instance of the <see cref="WatchListInitialiser"/> class.</summary>
        /// <param name="queries">The Dataverse queries used to read the initialisation context.</param>
        /// <param name="service">The current invocation's organisation service, used for the delta update.</param>
        public WatchListInitialiser(WatchListQueries queries, IOrganizationService service)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <inheritdoc/>
        public void InitialiseWatchList(Guid watchListId)
        {
            var watchList = this.queries.GetWatchListForInitialisation(watchListId);
            var changes = this.GetInitialisedWatchList(watchList).GetChanges(watchList);

            // Nothing is sent if the watch list was already initialised.
            if (changes.Attributes.Count > 0)
            {
                this.service.Update(changes);
            }
        }

        /// <inheritdoc/>
        public defraimp_WatchList GetInitialisedWatchList(defraimp_WatchList watchList)
        {
            if (watchList == null)
            {
                throw new ArgumentNullException(nameof(watchList));
            }

            var owner = watchList.OwnerId
                ?? throw new InvalidOperationException("The watch list must have an owner.");
            var context = this.queries.GetWatchListInitialisationContext(owner.Id, watchList.defraimp_TransporterId?.Id);

            var initialised = watchList.Clone();
            if (string.IsNullOrWhiteSpace(initialised.defraimp_Name))
            {
                initialised.defraimp_Name =
                    $"{context.TransporterName ?? "Unassigned transporter"} watch list ({context.OwnerName})";
            }

            if (string.IsNullOrWhiteSpace(initialised.defraimp_Comments))
            {
                initialised.defraimp_Comments = $"Owned by {context.OwnerName} ({context.OwnerEmail}).";
            }

            return initialised;
        }
    }
}
