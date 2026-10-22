namespace Defra.Imports.Plugins
{
    using System;
    using Defra.Imports.Application.Handlers;
    using Defra.Imports.Application.WorkOrders;
    using Defra.Imports.Dataverse.Contexts;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Dataverse.Model;

    /// <summary>Owns registration metadata shared by new plug-in entry points.</summary>
    internal static class PluginCompositionRoot
    {
        private static readonly Lazy<PluginContainer> RootValue = new Lazy<PluginContainer>(BuildRoot);

        /// <summary>Gets the immutable registration root for this deployment head.</summary>
        internal static PluginContainer Root => RootValue.Value;

        private static PluginContainer BuildRoot()
        {
            var root = new PluginContainer();
            root.Register<IWorkOrderApplicationService, WorkOrderApplicationService>();
            root.Register<WorkOrderQueries>();
            root.Register<WorkOrderInitialisationHandler>();
            root.Register<IImportsContextFactory, ImportsContextFactory>();
            root.Register<ImportsContext>();
            return root;
        }
    }
}
