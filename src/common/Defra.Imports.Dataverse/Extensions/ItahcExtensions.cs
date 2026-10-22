namespace Defra.Imports.Dataverse.Extensions
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;

    /// <summary>
    /// Extensions for the ITAHC entity.
    /// </summary>
    public static class ItahcExtensions
    {
        /// <summary>
        /// Projects an ITAHC onto the values needed to flag it. The ITAHC is not modified. An attribute that the ITAHC
        /// does not hold, such as one that a Create did not supply, is projected as null.
        /// </summary>
        /// <param name="itahc">The ITAHC.</param>
        /// <returns>The context holding a reference to the ITAHC, its name and the names of its economic operators.</returns>
        public static CreateItahcWatchFlagsContext ToCreateWatchFlagsContext(this defraimp_itahc itahc)
        {
            if (itahc == null)
            {
                throw new ArgumentNullException(nameof(itahc));
            }

            return new CreateItahcWatchFlagsContext(
                itahc.ToEntityReference(),
                itahc.defraimp_name,
                itahc.defraimp_PlaceOfOriginHarvestName,
                itahc.defraimp_PlaceOfDestinationName,
                itahc.defraimp_ConsigneeName,
                itahc.defraimp_TransporterName,
                itahc.defraimp_OVName);
        }
    }
}
