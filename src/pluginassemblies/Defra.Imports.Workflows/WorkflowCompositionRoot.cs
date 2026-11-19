namespace Defra.Imports.Workflows
{
    using System;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Contexts;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.Workflows.ImportApplication;

    /// <summary>Owns registration metadata shared by new workflow activity handlers.</summary>
    internal static class WorkflowCompositionRoot
    {
        private static readonly Lazy<PluginContainer> RootValue = new Lazy<PluginContainer>(BuildRoot);

        /// <summary>Gets the immutable registration root for this deployment head.</summary>
        internal static PluginContainer Root => RootValue.Value;

        private static PluginContainer BuildRoot()
        {
            var root = new PluginContainer();

            // Activities are host-created, so only their handlers are registered here.
            root.AddInspectionRequirements();
            root.Register<DetermineInspectionRequirementHandler>();
            root.Register<InspectionPlaceOfOriginChangeHandler>();

            // The legacy workflow activities traced without timestamps.
            root.Register<ILogWriter, UntimestampedTracingLogWriter>();

            root.Register<IImportsContextFactory, ImportsContextFactory>();
            root.Register<ImportsContext>();
            return root;
        }
    }
}
