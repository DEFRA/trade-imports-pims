namespace Defra.Imports.Plugins
{
    using System;
    using Defra.Imports.Application.CommodityComplements;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Contexts;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.Plugins.ImportApplication;
    using Defra.Imports.Plugins.ImporterNotification;
    using Defra.Imports.Plugins.Itahc;
    using Defra.Imports.Plugins.ParameterSetKeyDataPair;

    /// <summary>Owns registration metadata shared by new plug-in entry points.</summary>
    internal static class PluginCompositionRoot
    {
        private static readonly Lazy<PluginContainer> RootValue = new Lazy<PluginContainer>(BuildRoot);

        /// <summary>Gets the immutable registration root for this deployment head.</summary>
        internal static PluginContainer Root => RootValue.Value;

        private static PluginContainer BuildRoot()
        {
            var root = new PluginContainer();

            root.Register<ICommodityComplementKeyDataApplier, CommodityComplementKeyDataApplier>();
            root.Register<CommodityComplementQueries>();
            root.Register<PopulateAnimalNumberHandler>();

            root.Register<IWatchFlagCreator, WatchFlagCreator>();
            root.Register<WatchFlagQueries>();
            root.Register<FlagRecordOnWatchListHandler>();
            root.Register<FlagRecordOnWatchListNotificationHandler>();

            root.Register<IWatchFlagAssociator, WatchFlagAssociator>();
            root.Register<AssociateWatchFlagsFromPrimaryItahcHandler>();
            root.Register<AssociateWatchFlagsFromNotificationHandler>();

            root.AddInspectionRequirements();
            root.Register<DetermineInspectionRequirementHandler>();

            root.Register<ILogWriter, TimestampedTracingLogWriter>();
            root.Register<IImportsContextFactory, ImportsContextFactory>();
            root.Register<ImportsContext>();
            return root;
        }
    }
}
