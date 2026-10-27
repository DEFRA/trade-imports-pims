namespace Defra.Imports.Infrastructure.Workflows
{
    using System;
    using System.Activities;
    using System.Runtime.ExceptionServices;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Execution;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>Runs a constructor-injected workflow handler in an isolated invocation scope.</summary>
    /// <typeparam name="THandler">The handler registered in the workflow composition root.</typeparam>
    public abstract class WorkflowActivityBase<THandler> : CodeActivity
        where THandler : IWorkflowActivityHandler
    {
        private readonly PluginContainer container;

        /// <summary>Initializes a new instance of the <see cref="WorkflowActivityBase{THandler}"/> class.</summary>
        /// <param name="container">The reusable registration-only root; never an execution scope.</param>
        protected WorkflowActivityBase(PluginContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>Binds this host-created activity instance into the current invocation scope.</summary>
        /// <param name="scope">The invocation-local scope.</param>
        protected abstract void RegisterActivity(PluginContainer scope);

        /// <inheritdoc/>
        protected sealed override void Execute(CodeActivityContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ITracingService tracing = null;
            PluginContainer scope = null;
            Exception executionFailure = null;
            try
            {
                var workflowContext = GetRequiredExtension<IWorkflowContext>(context);
                tracing = GetRequiredExtension<ITracingService>(context);
                var factory = GetRequiredExtension<IOrganizationServiceFactory>(context);
                var service = factory.CreateOrganizationService(workflowContext.UserId) ?? throw new InvalidOperationException("The organization service factory returned no service.");
                scope = this.container.CreateExecutionScope();
                scope.Register<CodeActivityContext>(context);
                scope.Register<IWorkflowContext>(workflowContext);
                scope.Register<IOrganizationServiceFactory>(factory);
                scope.Register<IOrganizationService>(service);
                scope.Register<ITracingService>(tracing);
                scope.Register<IExecutionServiceProvider>(new WorkflowExecutionServiceProvider(factory, workflowContext));
                this.RegisterActivity(scope);
                scope.Resolve<THandler>().Execute();
            }
            catch (Exception exception)
            {
                executionFailure = exception;
            }

            if (scope != null)
            {
                try
                {
                    scope.Dispose();
                }
                catch (Exception cleanupFailure)
                {
                    if (executionFailure == null)
                    {
                        executionFailure = cleanupFailure;
                    }
                    else
                    {
                        TraceWithoutMasking(tracing, "Workflow handler {0} cleanup failed: {1}", typeof(THandler).FullName, cleanupFailure);
                    }
                }
            }

            if (executionFailure != null)
            {
                TraceWithoutMasking(tracing, "Workflow handler {0} failed: {1}", typeof(THandler).FullName, executionFailure);
                ExceptionDispatchInfo.Capture(executionFailure).Throw();
            }

            TraceWithoutMasking(tracing, "Completed workflow handler {0}.", typeof(THandler).FullName);
        }

        private static TExtension GetRequiredExtension<TExtension>(CodeActivityContext context)
            where TExtension : class
        {
            var extension = context.GetExtension<TExtension>();
            if (extension == null)
            {
                throw new InvalidOperationException("The workflow host did not provide " + typeof(TExtension).FullName + ".");
            }

            return extension;
        }

        private static void TraceWithoutMasking(ITracingService tracing, string format, params object[] args)
        {
            if (tracing == null)
            {
                return;
            }

            try
            {
                tracing.Trace(format, args);
            }
            catch (Exception)
            {
                // Diagnostics must not replace an execution or cleanup failure.
            }
        }
    }
}
