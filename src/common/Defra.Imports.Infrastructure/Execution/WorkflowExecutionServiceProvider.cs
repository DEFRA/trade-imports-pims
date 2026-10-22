namespace Defra.Imports.Infrastructure.Execution
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>Creates only identities supported by a custom workflow activity host.</summary>
    public sealed class WorkflowExecutionServiceProvider : IExecutionServiceProvider
    {
        private readonly IOrganizationServiceFactory factory;
        private readonly IWorkflowContext context;

        /// <summary>Initializes a new instance of the <see cref="WorkflowExecutionServiceProvider"/> class.</summary>
        /// <param name="factory">The current invocation's organization service factory.</param>
        /// <param name="context">The current workflow execution context.</param>
        public WorkflowExecutionServiceProvider(IOrganizationServiceFactory factory, IWorkflowContext context)
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public IOrganizationService GetExecutingUserService()
        {
            return this.CreateRequiredService(this.context.UserId);
        }

        /// <inheritdoc/>
        public IOrganizationService GetInitiatingUserService()
        {
            if (this.context.InitiatingUserId != this.context.UserId)
            {
                throw new NotSupportedException("This workflow host cannot request a service for a different initiating user.");
            }

            return this.GetExecutingUserService();
        }

        /// <inheritdoc/>
        public IOrganizationService GetSystemService()
        {
            throw new NotSupportedException("This workflow host cannot request a SYSTEM organization service.");
        }

        private IOrganizationService CreateRequiredService(Guid userId)
        {
            return this.factory.CreateOrganizationService(userId)
                ?? throw new InvalidOperationException("The organization service factory returned no service.");
        }
    }
}
