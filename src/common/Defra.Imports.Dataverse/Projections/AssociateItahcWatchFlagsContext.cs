namespace Defra.Imports.Dataverse.Projections
{
    using System;

    /// <summary>
    /// The values of an import application that are needed to associate it with the watch flags of its primary ITAHC.
    /// Use <see cref="Defra.Imports.Dataverse.Extensions.ImportApplicationExtensions.ToAssociateItahcWatchFlagsContext(Defra.Imports.Dataverse.Model.defraimp_importapplication)"/>
    /// to project an import application onto it.
    /// </summary>
    public sealed class AssociateItahcWatchFlagsContext
    {
        /// <summary>Initializes a new instance of the <see cref="AssociateItahcWatchFlagsContext"/> class.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        /// <param name="primaryItahcId">The identifier of the primary ITAHC, if any.</param>
        public AssociateItahcWatchFlagsContext(Guid importApplicationId, Guid? primaryItahcId)
        {
            this.ImportApplicationId = importApplicationId;
            this.PrimaryItahcId = primaryItahcId;
        }

        /// <summary>Gets the identifier of the import application.</summary>
        public Guid ImportApplicationId { get; }

        /// <summary>Gets the identifier of the primary ITAHC, or null if the import application has none.</summary>
        public Guid? PrimaryItahcId { get; }
    }
}
