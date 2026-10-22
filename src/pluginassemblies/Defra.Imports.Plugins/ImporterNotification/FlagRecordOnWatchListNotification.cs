namespace Defra.Imports.Plugins.ImporterNotification
{
    using Defra.Imports.Infrastructure.Plugins;

    /// <summary>
    /// Flags an importer notification on each active watch list that refers to one of its economic operators. The
    /// class name is retained for the registered step; <see cref="FlagRecordOnWatchListNotificationHandler"/> adapts
    /// the step and the shared application service does the work. Registered as a post-operation, synchronous Create
    /// step of defraimp_importernotification (filtering on defraimp_importernotificationid, no images).
    /// </summary>
    public sealed class FlagRecordOnWatchListNotification : PluginBase<FlagRecordOnWatchListNotificationHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="FlagRecordOnWatchListNotification"/> class.</summary>
        public FlagRecordOnWatchListNotification()
            : base(PluginCompositionRoot.Root)
        {
        }
    }
}
