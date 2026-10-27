namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Dataverse.Projections;

    /// <summary>
    /// Determines the inspection requirement of an import application, and keeps the counters that its inspection
    /// is balanced against up to date as the import application changes.
    /// </summary>
    public interface IInspectionRequirementDeterminer
    {
        /// <summary>
        /// Works out what the change to an import application means for its inspection. Which values are given
        /// decides the change: only <paramref name="after"/> for a creation, both for an update or a change of
        /// state, and only <paramref name="before"/> for a deletion. Counters, counter history and the inspection
        /// requirement are written explicitly, in the order in which they are worked out, and are written even if
        /// they are unchanged.
        /// </summary>
        /// <param name="before">The import application before the change, or null.</param>
        /// <param name="after">The import application after the change, or null.</param>
        void Determine(InspectionImportApplicationContext before, InspectionImportApplicationContext after);

        /// <summary>
        /// Retrieves an import application and determines its inspection requirement as if it had just been
        /// created. Only the columns that this needs are retrieved.
        /// </summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        void DetermineForImportApplication(Guid importApplicationId);
    }
}
