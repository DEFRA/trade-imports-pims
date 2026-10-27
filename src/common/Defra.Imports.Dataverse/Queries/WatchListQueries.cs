namespace Defra.Imports.Dataverse.Queries
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk.Client;

    /// <summary>
    /// ILLUSTRATIVE EXAMPLE of the pattern in architecture/adr/0001-plugin-workflow-di-scaffold.md: a shared
    /// Dataverse query class using early-bound LINQ. It is not a real business process. It must not be registered
    /// with Dataverse or included in a solution, and must not be extended or copied as a template for entity or
    /// business choices.
    /// </summary>
    public class WatchListQueries
    {
        private readonly ImportsContext context;

        /// <summary>Initializes a new instance of the <see cref="WatchListQueries"/> class.</summary>
        /// <param name="context">The invocation-local context, which these read-only queries stop tracking.</param>
        public WatchListQueries(ImportsContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            // These queries are read-only, so avoid change tracking on the invocation-local context.
            this.context.MergeOption = MergeOption.NoTracking;
        }

        /// <summary>Retrieves the values the initialiser reads, for callers that hold only an identifier.</summary>
        /// <param name="watchListId">The identifier of the watch list.</param>
        /// <returns>A watch list containing only the values the initialiser reads.</returns>
        public defraimp_WatchList GetWatchListForInitialisation(Guid watchListId)
        {
            var query =
                from watchList in this.context.defraimp_WatchListSet
                where watchList.defraimp_WatchListId == watchListId
                select new defraimp_WatchList
                {
                    defraimp_WatchListId = watchList.defraimp_WatchListId,
                    OwnerId = watchList.OwnerId,
                    defraimp_TransporterId = watchList.defraimp_TransporterId,
                    defraimp_Name = watchList.defraimp_Name,
                    defraimp_Comments = watchList.defraimp_Comments,
                };

            return query.FirstOrDefault()
                ?? throw new InvalidOperationException("The requested watch list was not found.");
        }

        /// <summary>
        /// Retrieves a projection spanning the owning user and the optional transporter. The tables are unrelated
        /// and the transporter is optional, so these are two independent queries behind one method.
        /// </summary>
        /// <param name="ownerId">The identifier of the owning user.</param>
        /// <param name="transporterId">The identifier of the transporter, if the watch list references one.</param>
        /// <returns>The initialisation context.</returns>
        public WatchListInitialisationContext GetWatchListInitialisationContext(Guid ownerId, Guid? transporterId)
        {
            var ownerQuery =
                from user in this.context.SystemUserSet
                where user.SystemUserId == ownerId
                select new { user.FullName, user.InternalEMailAddress };

            var owner = ownerQuery.FirstOrDefault()
                ?? throw new InvalidOperationException("The watch list owner was not found or is not a user.");

            string transporterName = null;
            if (transporterId.HasValue)
            {
                var id = transporterId.Value;
                var transporterQuery =
                    from transporter in this.context.defraimp_TransporterSet
                    where transporter.defraimp_TransporterId == id
                    select new { transporter.defraimp_Name };

                transporterName = transporterQuery.FirstOrDefault()?.defraimp_Name;
            }

            return new WatchListInitialisationContext(
                ownerId, owner.FullName, owner.InternalEMailAddress, transporterName);
        }
    }
}
