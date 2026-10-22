namespace Defra.Imports.Plugins.WatchLists
{
    using Defra.Imports.Infrastructure.Plugins;

    /// <summary>
    /// ILLUSTRATIVE EXAMPLE of the pattern in architecture/adr/0001-plugin-workflow-di-scaffold.md: a utility-named
    /// plug-in adapter. It is not a real business process. It must not be registered with Dataverse or included in
    /// a solution, and must not be extended or copied as a template for entity or business choices.
    /// </summary>
    public sealed class WatchListInitialisationPlugin : PluginBase<WatchListInitialisationHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="WatchListInitialisationPlugin"/> class.</summary>
        public WatchListInitialisationPlugin()
            : base(PluginCompositionRoot.Root)
        {
        }
    }
}
