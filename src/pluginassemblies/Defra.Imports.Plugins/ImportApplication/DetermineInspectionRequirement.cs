namespace Defra.Imports.Plugins.ImportApplication
{
    using Defra.Imports.Infrastructure.Plugins;

    /// <summary>
    /// Determines the inspection requirement of an import application, and keeps the counters that its inspection
    /// is balanced against up to date, when the import application changes. The class name is retained for the
    /// registered steps; <see cref="DetermineInspectionRequirementHandler"/> adapts them and the shared application
    /// service does the work. Registered as three synchronous steps of defraimp_importapplication:
    /// <list type="bullet">
    /// <item><description>Update, post-operation, filtering on defraimp_primaryitahcid, defraimp_importrisklevelid, defraimp_placeoforiginid and defraimp_primaryimporternotificationid, with a PreImage and a PostImage on the Target.</description></item>
    /// <item><description>SetState, post-operation, with a PreImage and a PostImage on the EntityMoniker.</description></item>
    /// <item><description>Delete, pre-operation, with a PreImage on the Target.</description></item>
    /// </list>
    /// </summary>
    public sealed class DetermineInspectionRequirement : PluginBase<DetermineInspectionRequirementHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="DetermineInspectionRequirement"/> class.</summary>
        public DetermineInspectionRequirement()
            : base(PluginCompositionRoot.Root)
        {
        }
    }
}
