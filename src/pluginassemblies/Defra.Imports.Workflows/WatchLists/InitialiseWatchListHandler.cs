namespace Defra.Imports.Workflows.WatchLists
{
    using System;
    using System.Activities;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Workflows;

    /// <summary>
    /// ILLUSTRATIVE EXAMPLE of the pattern in architecture/adr/0001-plugin-workflow-di-scaffold.md: a workflow
    /// handler that adapts the activity argument to one persisting application command. It is not a real business
    /// process. It must not be registered with Dataverse or included in a solution, and must not be extended or
    /// copied as a template for entity or business choices.
    /// </summary>
    public sealed class InitialiseWatchListHandler : IWorkflowActivityHandler
    {
        private readonly InitialiseWatchListActivity activity;
        private readonly CodeActivityContext context;
        private readonly IWatchListInitialiser initialiser;

        /// <summary>Initializes a new instance of the <see cref="InitialiseWatchListHandler"/> class.</summary>
        /// <param name="activity">The exact host-created activity instance for this invocation.</param>
        /// <param name="context">The current workflow execution context.</param>
        /// <param name="initialiser">The shared watch list initialiser.</param>
        public InitialiseWatchListHandler(
            InitialiseWatchListActivity activity,
            CodeActivityContext context,
            IWatchListInitialiser initialiser)
        {
            this.activity = activity ?? throw new ArgumentNullException(nameof(activity));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.initialiser = initialiser ?? throw new ArgumentNullException(nameof(initialiser));
        }

        /// <inheritdoc/>
        public void Execute()
        {
            var reference = this.activity.WatchList.Get(this.context);
            if (reference == null
                || reference.Id == Guid.Empty
                || reference.LogicalName != defraimp_WatchList.EntityLogicalName)
            {
                throw new ArgumentException("A valid watch list reference is required.");
            }

            // The retrieve, initialisation and delta update are one application command.
            this.initialiser.InitialiseWatchList(reference.Id);
        }
    }
}
