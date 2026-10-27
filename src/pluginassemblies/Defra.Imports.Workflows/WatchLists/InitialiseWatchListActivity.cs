namespace Defra.Imports.Workflows.WatchLists
{
    using System.Activities;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Workflows;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>
    /// ILLUSTRATIVE EXAMPLE of the pattern in architecture/adr/0001-plugin-workflow-di-scaffold.md: a declarative
    /// custom workflow activity entry point with one required input and no outputs. It is not a real business
    /// process. It must not be registered with Dataverse or included in a solution, and must not be extended or
    /// copied as a template for entity or business choices.
    /// </summary>
    public sealed class InitialiseWatchListActivity : WorkflowActivityBase<InitialiseWatchListHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="InitialiseWatchListActivity"/> class.</summary>
        public InitialiseWatchListActivity()
            : base(WorkflowCompositionRoot.Root)
        {
        }

        /// <summary>Gets or sets the watch list input.</summary>
        [Input("Watch list")]
        [ReferenceTarget("defraimp_watchlist")]
        [RequiredArgument]
        public InArgument<EntityReference> WatchList { get; set; }

        /// <inheritdoc/>
        protected override void RegisterActivity(PluginContainer scope)
        {
            scope.Register<InitialiseWatchListActivity>(this);
        }
    }
}
