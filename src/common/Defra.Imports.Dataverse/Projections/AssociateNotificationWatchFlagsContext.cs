namespace Defra.Imports.Dataverse.Projections
{
    using System;

    /// <summary>
    /// The values of an import application that are needed to associate it with the watch flags of its primary
    /// importer notification. Use
    /// <see cref="Defra.Imports.Dataverse.Extensions.ImportApplicationExtensions.ToAssociateNotificationWatchFlagsContext(Defra.Imports.Dataverse.Model.defraimp_importapplication)"/>
    /// to project an import application onto it.
    /// </summary>
    public sealed class AssociateNotificationWatchFlagsContext
    {
        /// <summary>Initializes a new instance of the <see cref="AssociateNotificationWatchFlagsContext"/> class.</summary>
        /// <param name="importApplicationId">The identifier of the import application.</param>
        /// <param name="primaryImporterNotificationId">The identifier of the primary importer notification, if any.</param>
        public AssociateNotificationWatchFlagsContext(Guid importApplicationId, Guid? primaryImporterNotificationId)
        {
            this.ImportApplicationId = importApplicationId;
            this.PrimaryImporterNotificationId = primaryImporterNotificationId;
        }

        /// <summary>Gets the identifier of the import application.</summary>
        public Guid ImportApplicationId { get; }

        /// <summary>Gets the identifier of the primary importer notification, or null if the import application has none.</summary>
        public Guid? PrimaryImporterNotificationId { get; }
    }
}
