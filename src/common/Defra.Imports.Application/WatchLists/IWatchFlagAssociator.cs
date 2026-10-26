namespace Defra.Imports.Application.WatchLists
{
    using Defra.Imports.Dataverse.Projections;

    /// <summary>
    /// Associates an import application with the watch flags of its primary ITAHC or importer notification.
    /// </summary>
    public interface IWatchFlagAssociator
    {
        /// <summary>
        /// Clears the link to the import application on each of its watch flags that has an ITAHC, issuing one
        /// explicit update each. Then, if the import application has a primary ITAHC, links each watch flag of that
        /// ITAHC that is not already linked to the import application, issuing one explicit update each.
        /// </summary>
        /// <param name="context">The values of the import application that are needed to associate its watch flags.</param>
        void AssociateWatchFlags(AssociateItahcWatchFlagsContext context);

        /// <summary>
        /// Clears the link to the import application on each of its watch flags that has an importer notification,
        /// issuing one explicit update each. Then, if the import application has a primary importer notification,
        /// links each watch flag of that notification that is not already linked to the import application, issuing
        /// one explicit update each.
        /// </summary>
        /// <param name="context">The values of the import application that are needed to associate its watch flags.</param>
        void AssociateWatchFlags(AssociateNotificationWatchFlagsContext context);
    }
}
