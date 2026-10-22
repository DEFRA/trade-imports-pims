namespace Defra.Imports.Application.Handlers
{
    using System;
    using Defra.Imports.Application.WorkOrders;
    using Defra.Imports.Infrastructure.Plugins;

    /// <summary>Unregistered, read-only example; does not initialise or modify a work order.</summary>
    public sealed class WorkOrderInitialisationHandler : IPluginHandler
    {
        private readonly IWorkOrderApplicationService application;
        private readonly IPluginExecution execution;

        /// <summary>Initializes a new instance of the <see cref="WorkOrderInitialisationHandler"/> class.</summary>
        /// <param name="application">The read-only application service.</param>
        /// <param name="execution">The current invocation's services.</param>
        public WorkOrderInitialisationHandler(IWorkOrderApplicationService application, IPluginExecution execution)
        {
            this.application = application ?? throw new ArgumentNullException(nameof(application));
            this.execution = execution ?? throw new ArgumentNullException(nameof(execution));
        }

        /// <inheritdoc/>
        public void Execute()
        {
            if (this.execution.Context.MessageName != "Create" ||
                this.execution.Context.PrimaryEntityName != "msdyn_workorder")
            {
                throw new InvalidOperationException("This example requires a work order Create execution.");
            }

            this.application.GetInitializationContext(this.execution.Context.PrimaryEntityId);
            this.execution.Tracing.Trace("Work order example context loaded. No business changes were applied.");
        }
    }
}
