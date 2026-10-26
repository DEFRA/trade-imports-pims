namespace Defra.Imports.Plugins.ImportApplication
{
    using Defra.Imports.Infrastructure.Plugins;

    /// <summary>
    /// Associates an import application with the watch flags of its primary ITAHC. The class name is retained for the
    /// registered steps; <see cref="AssociateWatchFlagsFromPrimaryItahcHandler"/> adapts the steps and the shared
    /// application service does the work. Registered as post-operation, synchronous Create (filtering on
    /// defraimp_importapplicationid) and Update (filtering on defraimp_primaryitahcid) steps of
    /// defraimp_importapplication, with no images.
    /// </summary>
    public sealed class AssociateWatchFlagsFromPrimaryItahc : PluginBase<AssociateWatchFlagsFromPrimaryItahcHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="AssociateWatchFlagsFromPrimaryItahc"/> class.</summary>
        public AssociateWatchFlagsFromPrimaryItahc()
            : base(PluginCompositionRoot.Root)
        {
        }
    }
}
