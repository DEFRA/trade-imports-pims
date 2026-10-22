namespace Defra.Imports.Dataverse.Extensions
{
    using System;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;

    /// <summary>
    /// Extensions for the importer notification entity.
    /// </summary>
    public static class ImporterNotificationExtensions
    {
        /// <summary>
        /// Projects an importer notification onto the values needed to flag it. The notification is not modified. An
        /// attribute that the notification does not hold, such as one that a Create did not supply, is projected as null.
        /// </summary>
        /// <param name="notification">The importer notification.</param>
        /// <returns>The context holding a reference to the notification, its name and the names of its economic operators.</returns>
        public static CreateNotificationWatchFlagsContext ToCreateWatchFlagsContext(this defraimp_ImporterNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            return new CreateNotificationWatchFlagsContext(
                notification.ToEntityReference(),
                notification.defraimp_Name,
                notification.defraimp_consignorcompanyname,
                notification.defraimp_placeofdestinationcompanyname,
                notification.defraimp_importercompanyname,
                notification.defraimp_transportercompanyname);
        }
    }
}
