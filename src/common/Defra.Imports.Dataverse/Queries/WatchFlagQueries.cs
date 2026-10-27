namespace Defra.Imports.Dataverse.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk.Client;

    /// <summary>
    /// Read-only queries that find the active watch lists, the economic operators that they may refer to, and the
    /// watch flags that are linked to an import application or to its primary ITAHC or importer notification.
    /// </summary>
    public class WatchFlagQueries
    {
        private readonly ImportsContext context;

        /// <summary>Initializes a new instance of the <see cref="WatchFlagQueries"/> class.</summary>
        /// <param name="context">The invocation-local context, which these read-only queries stop tracking.</param>
        public WatchFlagQueries(ImportsContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            // These queries are read-only, so avoid change tracking on the invocation-local context.
            this.context.MergeOption = MergeOption.NoTracking;
        }

        /// <summary>
        /// Retrieves the active watch lists whose period includes the date. A watch list with no end date has no
        /// upper limit. The result is not paged, so only the first page that Dataverse returns is used.
        /// </summary>
        /// <param name="date">The date that the watch lists must cover.</param>
        /// <returns>Watch lists holding only their identifier, name, dates, watch type and economic operator lookups.</returns>
        public IReadOnlyList<defraimp_WatchList> GetActiveWatchLists(DateTime date)
        {
            var query =
                from watchList in this.context.defraimp_WatchListSet
                where watchList.statecode == defraimp_watchlist_statecode.Active
                    && watchList.defraimp_StartDate <= date
                    && (watchList.defraimp_EndDate >= date || watchList.defraimp_EndDate == null)
                select new defraimp_WatchList
                {
                    defraimp_WatchListId = watchList.defraimp_WatchListId,
                    defraimp_Name = watchList.defraimp_Name,
                    defraimp_StartDate = watchList.defraimp_StartDate,
                    defraimp_EndDate = watchList.defraimp_EndDate,
                    defraimp_WatchType = watchList.defraimp_WatchType,
                    defraimp_PlaceOfOriginId = watchList.defraimp_PlaceOfOriginId,
                    defraimp_PlaceOfDestinationId = watchList.defraimp_PlaceOfDestinationId,
                    defraimp_ConsigneeId = watchList.defraimp_ConsigneeId,
                    defraimp_TransporterId = watchList.defraimp_TransporterId,
                    defraimp_VeterinarianId = watchList.defraimp_VeterinarianId,
                };

            return query.ToList();
        }

        /// <summary>
        /// Finds the economic operator of the type that has the given name. The lookup is not constrained to a single
        /// record, so the first match returned is used.
        /// </summary>
        /// <param name="watchType">The type of watch list, which selects the type of economic operator to search.</param>
        /// <param name="name">The name of the economic operator.</param>
        /// <returns>The identifier of the matching economic operator, or null if there is none.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The watch type has no economic operator table.</exception>
        public Guid? FindEconomicOperatorId(defraimp_watchtype watchType, string name)
        {
            switch (watchType)
            {
                case defraimp_watchtype.PlaceofOrigin:
                    return (from economicOperator in this.context.defraimp_placeoforiginSet
                            where economicOperator.defraimp_name == name
                            select new { economicOperator.defraimp_placeoforiginId }).FirstOrDefault()?.defraimp_placeoforiginId;
                case defraimp_watchtype.PlaceofDestination:
                    return (from economicOperator in this.context.defraimp_PlaceOfDestinationSet
                            where economicOperator.defraimp_Name == name
                            select new { economicOperator.defraimp_PlaceOfDestinationId }).FirstOrDefault()?.defraimp_PlaceOfDestinationId;
                case defraimp_watchtype.Consignee:
                    return (from economicOperator in this.context.defraimp_ConsigneeSet
                            where economicOperator.defraimp_Name == name
                            select new { economicOperator.defraimp_ConsigneeId }).FirstOrDefault()?.defraimp_ConsigneeId;
                case defraimp_watchtype.Transporter:
                    return (from economicOperator in this.context.defraimp_TransporterSet
                            where economicOperator.defraimp_Name == name
                            select new { economicOperator.defraimp_TransporterId }).FirstOrDefault()?.defraimp_TransporterId;
                case defraimp_watchtype.Veterinarian:
                    return (from economicOperator in this.context.defraimp_VeterinarianSet
                            where economicOperator.defraimp_Name == name
                            select new { economicOperator.defraimp_VeterinarianId }).FirstOrDefault()?.defraimp_VeterinarianId;
                default:
                    throw new ArgumentOutOfRangeException(nameof(watchType), watchType, "The watch type has no economic operator table.");
            }
        }

        /// <summary>
        /// Retrieves the watch flags that are linked to the import application and to an ITAHC. The result is not
        /// paged, so only the first page that Dataverse returns is used.
        /// </summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        /// <returns>Watch flags holding only their identifier and import application lookup.</returns>
        public IReadOnlyList<defraimp_WatchFlag> GetItahcWatchFlagsLinkedToImportApplication(Guid importApplicationId)
        {
            var query =
                from watchFlag in this.context.defraimp_WatchFlagSet
                where watchFlag.defraimp_ImportApplicationId.Id == importApplicationId
                    && watchFlag.defraimp_ItahcId != null
                select new defraimp_WatchFlag
                {
                    defraimp_WatchFlagId = watchFlag.defraimp_WatchFlagId,
                    defraimp_ImportApplicationId = watchFlag.defraimp_ImportApplicationId,
                };

            return query.ToList();
        }

        /// <summary>
        /// Retrieves the watch flags that are linked to the import application and to an importer notification. The
        /// result is not paged, so only the first page that Dataverse returns is used.
        /// </summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        /// <returns>Watch flags holding only their identifier and import application lookup.</returns>
        public IReadOnlyList<defraimp_WatchFlag> GetNotificationWatchFlagsLinkedToImportApplication(Guid importApplicationId)
        {
            var query =
                from watchFlag in this.context.defraimp_WatchFlagSet
                where watchFlag.defraimp_ImportApplicationId.Id == importApplicationId
                    && watchFlag.defraimp_ImporterNotificationId != null
                select new defraimp_WatchFlag
                {
                    defraimp_WatchFlagId = watchFlag.defraimp_WatchFlagId,
                    defraimp_ImportApplicationId = watchFlag.defraimp_ImportApplicationId,
                };

            return query.ToList();
        }

        /// <summary>
        /// Retrieves the watch flags of an ITAHC, whichever import application they are linked to. The result is not
        /// paged, so only the first page that Dataverse returns is used.
        /// </summary>
        /// <param name="itahcId">The identifier of the ITAHC.</param>
        /// <returns>Watch flags holding only their identifier and import application lookup.</returns>
        public IReadOnlyList<defraimp_WatchFlag> GetWatchFlagsForItahc(Guid itahcId)
        {
            var query =
                from watchFlag in this.context.defraimp_WatchFlagSet
                where watchFlag.defraimp_ItahcId.Id == itahcId
                select new defraimp_WatchFlag
                {
                    defraimp_WatchFlagId = watchFlag.defraimp_WatchFlagId,
                    defraimp_ImportApplicationId = watchFlag.defraimp_ImportApplicationId,
                };

            return query.ToList();
        }

        /// <summary>
        /// Retrieves the watch flags of an importer notification, whichever import application they are linked to.
        /// The result is not paged, so only the first page that Dataverse returns is used.
        /// </summary>
        /// <param name="importerNotificationId">The identifier of the importer notification.</param>
        /// <returns>Watch flags holding only their identifier and import application lookup.</returns>
        public IReadOnlyList<defraimp_WatchFlag> GetWatchFlagsForImporterNotification(Guid importerNotificationId)
        {
            var query =
                from watchFlag in this.context.defraimp_WatchFlagSet
                where watchFlag.defraimp_ImporterNotificationId.Id == importerNotificationId
                select new defraimp_WatchFlag
                {
                    defraimp_WatchFlagId = watchFlag.defraimp_WatchFlagId,
                    defraimp_ImportApplicationId = watchFlag.defraimp_ImportApplicationId,
                };

            return query.ToList();
        }
    }
}
