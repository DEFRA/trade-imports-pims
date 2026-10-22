namespace Defra.Imports.Plugins.WatchLists
{
    using System;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Plugins;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// ILLUSTRATIVE EXAMPLE of the pattern in architecture/adr/0001-plugin-workflow-di-scaffold.md: a plug-in
    /// handler for a pre-operation, synchronous Create that copies only the changed values onto the Target. It is
    /// not a real business process. It must not be registered with Dataverse or included in a solution, and must
    /// not be extended or copied as a template for entity or business choices.
    /// </summary>
    public sealed class WatchListInitialisationHandler : IPluginHandler
    {
        private const string CreateMessage = "Create";
        private const string TargetParameter = "Target";
        private const int PreOperationStage = 20;
        private const int SynchronousMode = 0;

        private readonly IWatchListInitialiser initialiser;
        private readonly IPluginExecution execution;

        /// <summary>Initializes a new instance of the <see cref="WatchListInitialisationHandler"/> class.</summary>
        /// <param name="initialiser">The shared watch list initialiser.</param>
        /// <param name="execution">The current invocation's services.</param>
        public WatchListInitialisationHandler(IWatchListInitialiser initialiser, IPluginExecution execution)
        {
            this.initialiser = initialiser ?? throw new ArgumentNullException(nameof(initialiser));
            this.execution = execution ?? throw new ArgumentNullException(nameof(execution));
        }

        /// <inheritdoc/>
        public void Execute()
        {
            var context = this.execution.Context;
            if (context.MessageName != CreateMessage
                || context.PrimaryEntityName != defraimp_WatchList.EntityLogicalName
                || context.Stage != PreOperationStage
                || context.Mode != SynchronousMode
                || !context.InputParameters.TryGetValue(TargetParameter, out var value)
                || !(value is Entity target))
            {
                throw new InvalidOperationException(
                    "This example requires a synchronous pre-operation watch list Create with a Target.");
            }

            var watchList = target.ToEntity<defraimp_WatchList>();
            var initialised = this.initialiser.GetInitialisedWatchList(watchList);

            // The pipeline persists these values; no Update is issued.
            var changes = initialised.GetChanges(watchList);
            foreach (var change in changes.Attributes)
            {
                target[change.Key] = change.Value;
            }

            this.execution.Tracing.Trace("Watch list initialised: {0} value(s) applied.", changes.Attributes.Count);
        }
    }
}
