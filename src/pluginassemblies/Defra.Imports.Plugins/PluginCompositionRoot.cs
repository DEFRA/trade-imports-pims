namespace Defra.Imports.Plugins
{
    using System;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Contexts;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Plugins.WatchLists;

    /// <summary>Owns registration metadata shared by new plug-in entry points.</summary>
    internal static class PluginCompositionRoot
    {
        private static readonly Lazy<PluginContainer> RootValue = new Lazy<PluginContainer>(BuildRoot);

        /// <summary>Gets the immutable registration root for this deployment head.</summary>
        internal static PluginContainer Root => RootValue.Value;

        private static PluginContainer BuildRoot()
        {
            var root = new PluginContainer();

            // ILLUSTRATIVE EXAMPLE registrations (watch list initialisation) for the pattern in
            // architecture/adr/0001-plugin-workflow-di-scaffold.md. They are not a real business process; do not
            // register the example with Dataverse, and do not copy it as a template for entity or business choices.
            root.Register<IWatchListInitialiser, WatchListInitialiser>();
            root.Register<WatchListQueries>();
            root.Register<WatchListInitialisationHandler>();

            root.Register<IImportsContextFactory, ImportsContextFactory>();
            root.Register<ImportsContext>();
            return root;
        }
    }
}
