namespace Defra.Imports.Infrastructure.Plugins
{
    using System;
    using System.Runtime.ExceptionServices;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Execution;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.PluginTelemetry;

    /// <summary>A stateless adapter; the supplied root must contain registrations only.</summary>
    /// <typeparam name="THandler">The handler registered in the supplied composition root.</typeparam>
    public abstract class PluginBase<THandler> : IPlugin
        where THandler : IPluginHandler
    {
        private readonly PluginContainer container;

        /// <summary>Initializes a new instance of the <see cref="PluginBase{THandler}"/> class.</summary>
        /// <param name="container">The reusable registration-only root; never an execution scope.</param>
        protected PluginBase(PluginContainer container)
        {
            this.container = container ?? throw new ArgumentNullException(nameof(container));
        }

        /// <summary>Resolves and runs a fresh handler graph for the Dataverse invocation.</summary>
        /// <param name="serviceProvider">The runtime services supplied by Dataverse.</param>
        public void Execute(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            ITracingService tracing = null;
            try
            {
                tracing = GetRequiredService<ITracingService>(serviceProvider);
                tracing.Trace("Starting handler {0}.", typeof(THandler).FullName);
                var scope = this.container.CreateExecutionScope();
                Exception executionFailure = null;
                try
                {
                    var context = GetRequiredService<IPluginExecutionContext>(serviceProvider);
                    var factory = GetRequiredService<IOrganizationServiceFactory>(serviceProvider);
                    var service = factory.CreateOrganizationService(context.UserId);
                    if (service == null)
                    {
                        throw new InvalidOperationException("The organisation service factory returned no service.");
                    }

                    scope.Register<IPluginExecutionContext>(context);
                    scope.Register<IOrganizationService>(service);
                    scope.Register<ITracingService>(tracing);
                    scope.Register<IOrganizationServiceFactory>(factory);
                    scope.Register<IPluginExecution>(new PluginExecution(context, service, tracing));
                    scope.Register<IExecutionServiceProvider>(new PluginExecutionServiceProvider(factory, context));

                    // Telemetry is optional in Dataverse; a handler requiring it fails resolution when unavailable.
                    var logger = serviceProvider.GetService(typeof(ILogger)) as ILogger;
                    if (logger != null)
                    {
                        scope.Register<ILogger>(logger);
                    }

                    scope.Resolve<THandler>().Execute();
                }
                catch (Exception exception)
                {
                    executionFailure = exception;
                }

                try
                {
                    scope.Dispose();
                }
                catch (Exception cleanupFailure)
                {
                    if (executionFailure != null)
                    {
                        TraceWithoutMasking(tracing, "Cleanup after handler {0} failed: {1}", typeof(THandler).FullName, cleanupFailure);
                    }
                    else
                    {
                        ExceptionDispatchInfo.Capture(cleanupFailure).Throw();
                    }
                }

                if (executionFailure != null)
                {
                    ExceptionDispatchInfo.Capture(executionFailure).Throw();
                }

                tracing.Trace("Completed handler {0}.", typeof(THandler).FullName);
            }
            catch (InvalidPluginExecutionException exception)
            {
                TraceWithoutMasking(tracing, "Handler {0} failed: {1}", typeof(THandler).FullName, exception);
                throw;
            }
            catch (Exception exception)
            {
                TraceWithoutMasking(tracing, "Handler {0} failed: {1}", typeof(THandler).FullName, exception);
                throw new InvalidPluginExecutionException("The plug-in could not complete. See the plug-in trace log for details.", exception);
            }
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

        private static TService GetRequiredService<TService>(IServiceProvider serviceProvider)
            where TService : class
        {
            var service = serviceProvider.GetService(typeof(TService)) as TService;
            if (service == null)
            {
                throw new InvalidOperationException("Dataverse did not provide " + typeof(TService).FullName + ".");
            }

            return service;
        }
    }
}
