namespace Defra.Imports.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.DependencyInjection;

    /// <summary>
    /// Registers the inspection requirement services that the plug-in and workflow deployment heads share, so that
    /// the two composition roots cannot drift apart. Handlers and host-specific services, such as the log writer,
    /// stay in each head's own root.
    /// </summary>
    public static class InspectionRequirementRegistrations
    {
        /// <summary>Adds the host-agnostic inspection requirement services and queries to a root.</summary>
        /// <param name="root">The registration root of a deployment head.</param>
        public static void AddInspectionRequirements(this PluginContainer root)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            root.Register<IInspectionRequirementDeterminer, InspectionRequirementDeterminer>();
            root.Register<IInspectionPlaceOfOriginChanger, InspectionPlaceOfOriginChanger>();
            root.Register<InspectionStrategyFactory>();
            root.Register<RiskCounterManagerFactory>();
            root.Register<InspectionRequirementUpdater>();
            root.Register<ImportRiskCounterAuditor>();
            root.Register<AutonumberCounters>();
            root.Register<PlaceOfOriginCounters>();
            root.Register<InspectionQueries>();
            root.Register<AutonumberQueries>();
            root.Register<PlaceOfOriginQueries>();
        }
    }
}
