namespace Defra.Imports.Dataverse.Projections
{
    using System;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// The values of an importer notification that are needed to flag it on the watch lists. Use
    /// <see cref="Defra.Imports.Dataverse.Extensions.ImporterNotificationExtensions.ToCreateWatchFlagsContext(Defra.Imports.Dataverse.Model.defraimp_ImporterNotification)"/>
    /// to project an importer notification onto it.
    /// </summary>
    public sealed class CreateNotificationWatchFlagsContext
    {
        /// <summary>Initializes a new instance of the <see cref="CreateNotificationWatchFlagsContext"/> class.</summary>
        /// <param name="notification">A reference to the importer notification that the watch flags are created for.</param>
        /// <param name="name">The name of the importer notification, which is used in the name of each watch flag.</param>
        /// <param name="consignor">The name of the consignor company, if any.</param>
        /// <param name="placeOfDestination">The name of the place of destination company, if any.</param>
        /// <param name="importer">The name of the importer company, if any.</param>
        /// <param name="transporter">The name of the transporter company, if any.</param>
        public CreateNotificationWatchFlagsContext(
            EntityReference notification,
            string name,
            string consignor,
            string placeOfDestination,
            string importer,
            string transporter)
        {
            this.Notification = notification ?? throw new ArgumentNullException(nameof(notification));
            this.Name = name;
            this.Consignor = consignor;
            this.PlaceOfDestination = placeOfDestination;
            this.Importer = importer;
            this.Transporter = transporter;
        }

        /// <summary>Gets a reference to the importer notification that the watch flags are created for.</summary>
        public EntityReference Notification { get; }

        /// <summary>Gets the name of the importer notification.</summary>
        public string Name { get; }

        /// <summary>Gets the name of the consignor company.</summary>
        public string Consignor { get; }

        /// <summary>Gets the name of the place of destination company.</summary>
        public string PlaceOfDestination { get; }

        /// <summary>Gets the name of the importer company.</summary>
        public string Importer { get; }

        /// <summary>Gets the name of the transporter company.</summary>
        public string Transporter { get; }
    }
}
