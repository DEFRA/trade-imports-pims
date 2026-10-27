namespace Defra.Imports.Application.WatchLists
{
    using Defra.Imports.Dataverse.Projections;

    /// <summary>
    /// Creates a watch flag for each economic operator named on a record that is also on an active watch list.
    /// </summary>
    public interface IWatchFlagCreator
    {
        /// <summary>
        /// Creates a watch flag, issuing one explicit create each, for every watch list that is active today and refers
        /// to an economic operator named by the ITAHC's place of origin, place of destination, consignee, transporter
        /// or official veterinarian.
        /// </summary>
        /// <param name="context">The values of the ITAHC that are needed to flag it.</param>
        void CreateWatchFlags(CreateItahcWatchFlagsContext context);

        /// <summary>
        /// Creates a watch flag, issuing one explicit create each, for every watch list that is active today and refers
        /// to an economic operator named by the importer notification's consignor, place of destination, importer or
        /// transporter.
        /// </summary>
        /// <param name="context">The values of the importer notification that are needed to flag it.</param>
        void CreateWatchFlags(CreateNotificationWatchFlagsContext context);
    }
}
