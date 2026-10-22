namespace Defra.Imports.Application.WatchLists
{
    using System;
    using Defra.Imports.Dataverse.Model;

    /// <summary>
    /// ILLUSTRATIVE EXAMPLE of the pattern in architecture/adr/0001-plugin-workflow-di-scaffold.md: a targeted
    /// application service shared by a plug-in host and a custom workflow activity host. It is not a real
    /// business process. It must not be registered with Dataverse or included in a solution, and must not be
    /// extended or copied as a template for entity or business choices.
    /// </summary>
    public interface IWatchListInitialiser
    {
        /// <summary>
        /// Returns a new watch list with initialisation values populated where they are blank. The input is not
        /// modified and nothing is persisted.
        /// </summary>
        /// <param name="watchList">The watch list to initialise; it is never modified.</param>
        /// <returns>A new watch list instance containing the initialised values.</returns>
        defraimp_WatchList GetInitialisedWatchList(defraimp_WatchList watchList);

        /// <summary>Retrieves the watch list, initialises it and updates only the values that changed.</summary>
        /// <param name="watchListId">The identifier of the watch list to initialise.</param>
        void InitialiseWatchList(Guid watchListId);
    }
}
