namespace Defra.Imports.Plugins.Utils
{
    using Defra.Imports.Application.Handlers;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins;

    /// <summary>Illustrates a utility-named plug-in adapter; it is not registered with Dataverse.</summary>
    public sealed class WorkOrderInitialisationPlugin : PluginBase<WorkOrderInitialisationHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="WorkOrderInitialisationPlugin"/> class.</summary>
        public WorkOrderInitialisationPlugin()
            : base(PluginCompositionRoot.Root)
        {
        }
    }
}
