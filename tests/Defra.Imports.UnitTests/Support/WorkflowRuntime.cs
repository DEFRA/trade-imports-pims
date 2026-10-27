namespace Defra.Imports.UnitTests.Support
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;
    using Moq;

    // Simulates the extensions the Dataverse workflow host supplies to a single activity invocation.
    public sealed class WorkflowRuntime
    {
        private bool provideWorkflowContext = true;

        public WorkflowRuntime()
        {
            this.Context.SetupGet(context => context.UserId).Returns(this.UserId);
            this.Context.SetupGet(context => context.InitiatingUserId).Returns(Guid.NewGuid());
            this.Factory.Setup(factory => factory.CreateOrganizationService(this.UserId)).Returns(this.Service.Object);
        }

        public Guid UserId { get; } = Guid.NewGuid();

        public Mock<IWorkflowContext> Context { get; } = new Mock<IWorkflowContext>();

        public Mock<IOrganizationServiceFactory> Factory { get; } = new Mock<IOrganizationServiceFactory>(MockBehavior.Strict);

        public Mock<IOrganizationService> Service { get; } = new Mock<IOrganizationService>(MockBehavior.Strict);

        public RecordingTracingService Tracing { get; } = new RecordingTracingService();

        public WorkflowRuntime WithoutWorkflowContext()
        {
            this.provideWorkflowContext = false;
            return this;
        }

        public IDictionary<string, object> Invoke(CodeActivity activity, string inputName, object input)
        {
            var invoker = new WorkflowInvoker(activity);
            if (this.provideWorkflowContext)
            {
                invoker.Extensions.Add(this.Context.Object);
            }

            invoker.Extensions.Add(this.Factory.Object);
            invoker.Extensions.Add<ITracingService>(() => this.Tracing);
            return invoker.Invoke(new Dictionary<string, object> { { inputName, input } });
        }
    }
}
