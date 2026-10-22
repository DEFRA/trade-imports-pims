namespace Defra.Imports.Plugins.Itahc
{
    using Defra.Imports.Infrastructure.Plugins;

    /// <summary>
    /// Flags an ITAHC on each active watch list that refers to one of its economic operators. The class name is
    /// retained for the registered step; <see cref="FlagRecordOnWatchListHandler"/> adapts the step and the shared
    /// application service does the work. Registered as a post-operation, synchronous Create step of defraimp_itahc
    /// (filtering on defraimp_itahcid, no images).
    /// </summary>
    public sealed class FlagRecordOnWatchList : PluginBase<FlagRecordOnWatchListHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="FlagRecordOnWatchList"/> class.</summary>
        public FlagRecordOnWatchList()
            : base(PluginCompositionRoot.Root)
        {
        }
    }
}
