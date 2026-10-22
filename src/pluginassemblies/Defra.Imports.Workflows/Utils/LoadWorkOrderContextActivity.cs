namespace Defra.Imports.Workflows.Utils
{
    using System;
    using System.Activities;
    using Defra.Imports.Application.WorkOrders;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Workflows;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>Illustrates workflow argument mapping without adding a registered activity.</summary>
    public sealed class LoadWorkOrderContextActivity : WorkflowActivityBase<LoadWorkOrderContextHandler>
    {
        /// <summary>Initializes a new instance of the <see cref="LoadWorkOrderContextActivity"/> class.</summary>
        public LoadWorkOrderContextActivity()
            : base(WorkflowCompositionRoot.Root)
        {
        }

        /// <summary>Gets or sets the work order input.</summary>
        [Input("Work order")]
        [ReferenceTarget("msdyn_workorder")]
        [RequiredArgument]
        public InArgument<EntityReference> WorkOrder { get; set; }

        /// <summary>Gets or sets the retrieved work order output.</summary>
        [Output("Retrieved work order")]
        [ReferenceTarget("msdyn_workorder")]
        public OutArgument<EntityReference> RetrievedWorkOrder { get; set; }

        /// <inheritdoc/>
        protected override void RegisterActivity(PluginContainer scope)
        {
            scope.Register<LoadWorkOrderContextActivity>(this);
        }
    }

    /// <summary>Adapts workflow arguments to the shared read-only application service.</summary>
    public sealed class LoadWorkOrderContextHandler : IWorkflowActivityHandler
    {
        private readonly LoadWorkOrderContextActivity activity;
        private readonly CodeActivityContext context;
        private readonly IWorkOrderApplicationService application;

        /// <summary>Initializes a new instance of the <see cref="LoadWorkOrderContextHandler"/> class.</summary>
        /// <param name="activity">The exact host-created activity instance for this invocation.</param>
        /// <param name="context">The current workflow execution context.</param>
        /// <param name="application">The shared application service.</param>
        public LoadWorkOrderContextHandler(
            LoadWorkOrderContextActivity activity,
            CodeActivityContext context,
            IWorkOrderApplicationService application)
        {
            this.activity = activity ?? throw new ArgumentNullException(nameof(activity));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.application = application ?? throw new ArgumentNullException(nameof(application));
        }

        /// <inheritdoc/>
        public void Execute()
        {
            var workOrder = this.activity.WorkOrder.Get(this.context);
            if (workOrder == null || workOrder.Id == Guid.Empty)
            {
                throw new ArgumentException("A valid work order reference is required.");
            }

            var result = this.application.GetInitializationContext(workOrder.Id);
            this.activity.RetrievedWorkOrder.Set(
                this.context, new EntityReference("msdyn_workorder", result.WorkOrderId));
        }
    }
}
