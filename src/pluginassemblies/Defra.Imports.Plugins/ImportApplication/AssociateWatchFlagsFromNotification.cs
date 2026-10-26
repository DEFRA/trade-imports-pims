namespace Defra.Imports.Plugins.ImportApplication
{
    using Defra.Imports.Infrastructure.Plugins;

    /// <summary>
    /// Associates an import application with the watch flags of its primary importer notification. The class name is
    /// retained for the registered steps; <see cref="AssociateWatchFlagsFromNotificationHandler"/> adapts the steps
    /// and the shared application service does the work. Registered as post-operation, synchronous Create (filtering
    /// on defraimp_importapplicationid) and Update (filtering on defraimp_primaryimporternotificationid) steps of
    /// defraimp_importapplication, with no images.
    /// </summary>
    public sealed class AssociateWatchFlagsFromNotification : PluginBase<AssociateWatchFlagsFromNotificationHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="AssociateWatchFlagsFromNotification"/> class.</summary>
        public AssociateWatchFlagsFromNotification()
            : base(PluginCompositionRoot.Root)
        {
        }
    }
}
