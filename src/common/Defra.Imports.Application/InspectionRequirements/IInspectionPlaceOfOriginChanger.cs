namespace Defra.Imports.Application.InspectionRequirements
{
    using System;

    /// <summary>
    /// Re-determines the inspection of a P1 import application whose place of origin has changed.
    /// </summary>
    public interface IInspectionPlaceOfOriginChanger
    {
        /// <summary>
        /// Retrieves an import application whose place of origin has changed. If it is a P1 import application, the
        /// gold/bronze quota of its previous place of origin is restored when it was inspected because of that
        /// place's coverage, and its P1 inspection is determined again. Counters and the inspection requirement
        /// are written explicitly, in the order in which they are worked out.
        /// </summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        void ChangePlaceOfOrigin(Guid importApplicationId);
    }
}
