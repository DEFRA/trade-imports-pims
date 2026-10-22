namespace Defra.Imports.UnitTests.Infrastructure.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.PluginTelemetry;
    using Moq;
    using Xunit;

    public sealed class PluginBaseTests : IDisposable
    {
        private const string FailKey = "fail";
        private const string CompletedKey = "completed";
        private const string DisposedKey = "disposed";
        private const string ExecutionContextKey = "execution.Context";
        private const string ExecutionServiceKey = "execution.Service";
        private const string ExecutionTracingKey = "execution.Tracing";
        private const string ServiceKey = "service";
        private const string TracingKey = "tracing";
        private const string LoggerKey = "logger";

        // Dependencies are constructed before the handler, so they are disposed after it, in reverse order.
        private static readonly string[] CleanupOrder =
        {
            CleanupFailure.HandlerName,
            SecondCleanupService.Name,
            FirstCleanupService.Name,
        };

        private readonly PluginContainer root = new PluginContainer();
        private readonly PluginRuntime runtime = new PluginRuntime();

        public void Dispose()
        {
            this.root.Dispose();
        }

        [Fact]
        public void Constructor_NullContainer_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new Adapter<RecordingHandler>(null));
        }

        [Fact]
        public void Execute_NullServiceProvider_ThrowsArgumentNullException()
        {
            var plugin = new Adapter<RecordingHandler>(this.root);

            Assert.Throws<ArgumentNullException>(() => plugin.Execute(null));
        }

        [Fact]
        public void Execute_ValidInvocation_ExecutesHandler()
        {
            this.root.Register<RecordingHandler>();
            var plugin = new Adapter<RecordingHandler>(this.root);

            plugin.Execute(this.runtime.Provider.Object);

            Assert.True((bool)this.runtime.OutputParameters[CompletedKey]);
        }

        [Fact]
        public void Execute_ValidInvocation_CreatesServiceForExecutingUser()
        {
            this.root.Register<RecordingHandler>();
            var plugin = new Adapter<RecordingHandler>(this.root);

            plugin.Execute(this.runtime.Provider.Object);

            this.runtime.Factory.Verify(factory => factory.CreateOrganizationService(this.runtime.UserId), Times.Once);
        }

        [Fact]
        public void Execute_ValidInvocation_InjectsInvocationServicesIntoHandler()
        {
            this.root.Register<RecordingHandler>();
            var plugin = new Adapter<RecordingHandler>(this.root);

            plugin.Execute(this.runtime.Provider.Object);

            Assert.Same(this.runtime.Service.Object, this.runtime.OutputParameters[ServiceKey]);
            Assert.Same(this.runtime.Tracing, this.runtime.OutputParameters[TracingKey]);
        }

        [Fact]
        public void Execute_ValidInvocation_ProvidesSameServicesThroughPluginExecution()
        {
            this.root.Register<RecordingHandler>();
            var plugin = new Adapter<RecordingHandler>(this.root);

            plugin.Execute(this.runtime.Provider.Object);

            Assert.Same(this.runtime.Context.Object, this.runtime.OutputParameters[ExecutionContextKey]);
            Assert.Same(this.runtime.Service.Object, this.runtime.OutputParameters[ExecutionServiceKey]);
            Assert.Same(this.runtime.Tracing, this.runtime.OutputParameters[ExecutionTracingKey]);
        }

        [Fact]
        public void Execute_ValidInvocation_TracesStartAndCompletion()
        {
            this.root.Register<RecordingHandler>();
            var plugin = new Adapter<RecordingHandler>(this.root);

            plugin.Execute(this.runtime.Provider.Object);

            var handlerName = typeof(RecordingHandler).FullName;
            Assert.Equal(
                new[] { "Starting handler " + handlerName + ".", "Completed handler " + handlerName + "." },
                this.runtime.Tracing.Messages);
        }

        [Fact]
        public void Execute_PluginReusedAcrossInvocations_GivesEachInvocationItsOwnServices()
        {
            this.root.Register<RecordingHandler>();
            var plugin = new Adapter<RecordingHandler>(this.root);
            var secondRuntime = new PluginRuntime();

            plugin.Execute(this.runtime.Provider.Object);
            plugin.Execute(secondRuntime.Provider.Object);

            Assert.Same(this.runtime.Service.Object, this.runtime.OutputParameters[ServiceKey]);
            Assert.Same(secondRuntime.Service.Object, secondRuntime.OutputParameters[ServiceKey]);
        }

        [Fact]
        public void Execute_ConcurrentInvocations_IsolateExecutionState()
        {
            const int invocationCount = 10;
            this.root.Register<RecordingHandler>();
            var plugin = new Adapter<RecordingHandler>(this.root);
            var runtimes = Enumerable.Range(0, invocationCount).Select(index => new PluginRuntime()).ToArray();

            Parallel.ForEach(runtimes, invocation => plugin.Execute(invocation.Provider.Object));

            Assert.All(runtimes, invocation =>
            {
                Assert.Same(invocation.Service.Object, invocation.OutputParameters[ServiceKey]);
                Assert.True((bool)invocation.OutputParameters[CompletedKey]);
            });
        }

        [Fact]
        public void Execute_LoggerAvailable_InjectsLoggerIntoHandler()
        {
            var logger = Mock.Of<ILogger>();
            this.runtime.WithLogger(logger);
            this.root.Register<LoggingHandler>();
            var plugin = new Adapter<LoggingHandler>(this.root);

            plugin.Execute(this.runtime.Provider.Object);

            Assert.Same(logger, this.runtime.OutputParameters[LoggerKey]);
        }

        [Fact]
        public void Execute_LoggerUnavailable_FailsHandlerThatRequiresIt()
        {
            this.root.Register<LoggingHandler>();
            var plugin = new Adapter<LoggingHandler>(this.root);

            var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            Assert.Contains(typeof(ILogger).Name, exception.InnerException.Message);
        }

        [Theory]
        [InlineData(typeof(ITracingService))]
        [InlineData(typeof(IPluginExecutionContext))]
        [InlineData(typeof(IOrganizationServiceFactory))]
        public void Execute_MissingRuntimeService_ThrowsInvalidPluginExecutionNamingTheService(Type missingService)
        {
            this.runtime.WithoutService(missingService);
            this.root.Register<RecordingHandler>();
            var plugin = new Adapter<RecordingHandler>(this.root);

            var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            Assert.IsType<InvalidOperationException>(exception.InnerException);
            Assert.Contains(missingService.FullName, exception.InnerException.Message);
        }

        [Fact]
        public void Execute_FactoryReturnsNoService_ThrowsInvalidPluginExecutionException()
        {
            this.runtime.WithoutOrganizationService();
            this.root.Register<RecordingHandler>();
            var plugin = new Adapter<RecordingHandler>(this.root);

            var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            Assert.IsType<InvalidOperationException>(exception.InnerException);
        }

        [Fact]
        public void Execute_HandlerThrows_WrapsFailureInInvalidPluginExecutionException()
        {
            this.root.Register<FailingHandler>();
            var plugin = new Adapter<FailingHandler>(this.root);

            var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            var cause = Assert.IsType<SimulatedFailureException>(exception.InnerException);
            Assert.Equal("handler failed", cause.Message);
        }

        [Fact]
        public void Execute_HandlerThrows_TracesFailure()
        {
            this.root.Register<FailingHandler>();
            var plugin = new Adapter<FailingHandler>(this.root);

            Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            Assert.Contains(this.runtime.Tracing.Messages, message => message.StartsWith("Handler ", StringComparison.Ordinal) && message.Contains("handler failed"));
        }

        [Fact]
        public void Execute_HandlerThrowsPlatformException_RethrowsSameInstance()
        {
            this.root.Register<PlatformFailureHandler>();
            var plugin = new Adapter<PlatformFailureHandler>(this.root);

            var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            Assert.Same(CleanupFailure.BusinessFailure, exception);
        }

        [Fact]
        public void Execute_DisposableHandlerSucceeds_DisposesHandler()
        {
            this.root.Register<DisposableHandler>();
            var plugin = new Adapter<DisposableHandler>(this.root);

            plugin.Execute(this.runtime.Provider.Object);

            Assert.True((bool)this.runtime.OutputParameters[DisposedKey]);
        }

        [Fact]
        public void Execute_DisposableHandlerFails_DisposesHandler()
        {
            this.runtime.OutputParameters[FailKey] = true;
            this.root.Register<DisposableHandler>();
            var plugin = new Adapter<DisposableHandler>(this.root);

            Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            Assert.True((bool)this.runtime.OutputParameters[DisposedKey]);
        }

        [Fact]
        public void Execute_ExecutionAndCleanupBothFail_RethrowsExecutionFailure()
        {
            this.RegisterCleanupGraph();
            this.runtime.OutputParameters[FailKey] = true;
            var plugin = new Adapter<CleanupFailureHandler>(this.root);

            var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            Assert.Same(CleanupFailure.BusinessFailure, exception);
        }

        [Fact]
        public void Execute_ExecutionAndCleanupBothFail_StillAttemptsEveryCleanup()
        {
            this.RegisterCleanupGraph();
            this.runtime.OutputParameters[FailKey] = true;
            var plugin = new Adapter<CleanupFailureHandler>(this.root);

            Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            Assert.Equal(CleanupOrder.Select(CleanupFailure.DisposedMessage), this.DisposalMessages());
        }

        [Fact]
        public void Execute_ExecutionAndCleanupBothFailAndTracingFails_StillRethrowsExecutionFailure()
        {
            this.RegisterCleanupGraph();
            this.runtime.OutputParameters[FailKey] = true;
            this.runtime.Tracing.ThrowWhenMessageStartsWith("Cleanup after handler");
            var plugin = new Adapter<CleanupFailureHandler>(this.root);

            var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            Assert.Same(CleanupFailure.BusinessFailure, exception);
        }

        [Fact]
        public void Execute_ExecutionAndCleanupBothFail_TracesCleanupFailure()
        {
            this.RegisterCleanupGraph();
            this.runtime.OutputParameters[FailKey] = true;
            var plugin = new Adapter<CleanupFailureHandler>(this.root);

            Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            Assert.Contains(this.runtime.Tracing.Messages, message => message.StartsWith("Cleanup after handler", StringComparison.Ordinal));
        }

        [Fact]
        public void Execute_OnlyCleanupFails_ThrowsAggregateOfEveryCleanupFailure()
        {
            this.RegisterCleanupGraph();
            var plugin = new Adapter<CleanupFailureHandler>(this.root);

            var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(this.runtime.Provider.Object));

            var aggregate = Assert.IsType<AggregateException>(exception.InnerException);
            Assert.Equal(
                CleanupOrder.Select(CleanupFailure.FailureMessage),
                aggregate.InnerExceptions.Select(inner => inner.Message));
        }

        private void RegisterCleanupGraph()
        {
            this.root.Register<CleanupFailureHandler>();
            this.root.Register<FirstCleanupService>();
            this.root.Register<SecondCleanupService>();
        }

        private IEnumerable<string> DisposalMessages()
        {
            return this.runtime.Tracing.Messages
                .Where(message => message.StartsWith("Disposed ", StringComparison.Ordinal));
        }

        private sealed class Adapter<THandler> : PluginBase<THandler>
            where THandler : IPluginHandler
        {
            public Adapter(PluginContainer container)
                : base(container)
            {
            }
        }

        private sealed class RecordingHandler : IPluginHandler
        {
            private readonly IPluginExecution execution;
            private readonly IPluginExecutionContext context;
            private readonly IOrganizationService service;
            private readonly ITracingService tracing;

            public RecordingHandler(IPluginExecution execution, IPluginExecutionContext context, IOrganizationService service, ITracingService tracing)
            {
                this.execution = execution;
                this.context = context;
                this.service = service;
                this.tracing = tracing;
            }

            public void Execute()
            {
                var output = this.context.OutputParameters;
                output[CompletedKey] = true;
                output[ServiceKey] = this.service;
                output[TracingKey] = this.tracing;
                output[ExecutionContextKey] = this.execution.Context;
                output[ExecutionServiceKey] = this.execution.Service;
                output[ExecutionTracingKey] = this.execution.Tracing;
            }
        }

        private sealed class LoggingHandler : IPluginHandler
        {
            private readonly IPluginExecutionContext context;
            private readonly ILogger logger;

            public LoggingHandler(IPluginExecutionContext context, ILogger logger)
            {
                this.context = context;
                this.logger = logger;
            }

            public void Execute()
            {
                this.context.OutputParameters[LoggerKey] = this.logger;
            }
        }

        private sealed class FailingHandler : IPluginHandler
        {
            public void Execute()
            {
                throw new SimulatedFailureException("handler failed");
            }
        }

        private sealed class PlatformFailureHandler : IPluginHandler
        {
            public void Execute()
            {
                throw CleanupFailure.BusinessFailure;
            }
        }

        private sealed class DisposableHandler : IPluginHandler, IDisposable
        {
            private readonly IPluginExecutionContext context;

            public DisposableHandler(IPluginExecutionContext context)
            {
                this.context = context;
            }

            public void Execute()
            {
                if (this.context.OutputParameters.Contains(FailKey))
                {
                    throw new SimulatedFailureException("handler failed");
                }
            }

            public void Dispose()
            {
                this.context.OutputParameters[DisposedKey] = true;
            }
        }

        private sealed class CleanupFailureHandler : IPluginHandler, IDisposable
        {
            private readonly IPluginExecutionContext context;
            private readonly ITracingService tracing;

            public CleanupFailureHandler(
                IPluginExecutionContext context,
                ITracingService tracing,
                FirstCleanupService first,
                SecondCleanupService second)
            {
                this.context = context;
                this.tracing = tracing;
                this.First = first;
                this.Second = second;
            }

            public FirstCleanupService First { get; }

            public SecondCleanupService Second { get; }

            public void Execute()
            {
                if (this.context.OutputParameters.Contains(FailKey))
                {
                    throw CleanupFailure.BusinessFailure;
                }
            }

            public void Dispose()
            {
                CleanupFailure.DisposeAndFail(this.tracing, CleanupFailure.HandlerName);
            }
        }
    }
}
