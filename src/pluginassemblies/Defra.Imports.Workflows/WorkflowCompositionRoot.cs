namespace Defra.Imports.Workflows
{
    using System;
    using Defra.Imports.Application.WorkOrders;
    using Defra.Imports.Dataverse.Contexts;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Workflows.Utils;

    /// <summary>Owns registration metadata shared by new workflow activity handlers.</summary>
    internal static class WorkflowCompositionRoot
    {
        private static readonly Lazy<PluginContainer> RootValue = new Lazy<PluginContainer>(BuildRoot);

        /// <summary>Gets the immutable registration root for this deployment head.</summary>
        internal static PluginContainer Root => RootValue.Value;

        private static PluginContainer BuildRoot()
        {
            var root = new PluginContainer();
            root.Register<IWorkOrderApplicationService, WorkOrderApplicationService>();
            root.Register<WorkOrderQueries>();
            root.Register<LoadWorkOrderContextHandler>();
            root.Register<IImportsContextFactory, ImportsContextFactory>();
            root.Register<ImportsContext>();
            return root;
        }
    }
}
