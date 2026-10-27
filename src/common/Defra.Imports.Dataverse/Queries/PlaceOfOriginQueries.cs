namespace Defra.Imports.Dataverse.Queries
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk.Client;

    /// <summary>
    /// Read-only queries over places of origin.
    /// </summary>
    public class PlaceOfOriginQueries
    {
        private readonly ImportsContext context;

        /// <summary>Initializes a new instance of the <see cref="PlaceOfOriginQueries"/> class.</summary>
        /// <param name="context">The invocation-local context, which these read-only queries stop tracking.</param>
        public PlaceOfOriginQueries(ImportsContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            // These queries are read-only, so avoid change tracking on the invocation-local context.
            this.context.MergeOption = MergeOption.NoTracking;
        }

        /// <summary>Finds a place of origin, with all of its columns.</summary>
        /// <param name="placeOfOriginId">The identifier of the place of origin.</param>
        /// <returns>The place of origin, or null if there is none.</returns>
        public defraimp_placeoforigin Find(Guid placeOfOriginId)
        {
            var query =
                from placeOfOrigin in this.context.defraimp_placeoforiginSet
                where placeOfOrigin.defraimp_placeoforiginId == placeOfOriginId
                select placeOfOrigin;

            return query.FirstOrDefault();
        }
    }
}
