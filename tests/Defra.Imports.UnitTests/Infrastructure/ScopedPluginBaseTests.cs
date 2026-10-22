namespace Defra.Imports.UnitTests.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Plugins;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.PluginTelemetry;
    using Moq;
    using Xunit;

    public sealed class ScopedPluginBaseTests
    {
        [Fact]
        public void Execute_ReusedPlugin_InvokesHandlerSynchronouslyWithCurrentServices()
        {
            using (var root = new PluginContainer())
            {
                root.Register<RecordingHandler>();
                var plugin = new Adapter<RecordingHandler>(root);
                for (int index = 0; index < 2; index++)
                {
                    var runtime = new Runtime();
                    plugin.Execute(runtime.Provider.Object);
                    Assert.Equal(true, runtime.Context.Object.OutputParameters["completed"]);
                    Assert.Same(runtime.Service.Object, runtime.Context.Object.OutputParameters["service"]);
                    Assert.Same(runtime.Tracing.Object, runtime.Context.Object.OutputParameters["tracing"]);
                    runtime.Factory.Verify(factory => factory.CreateOrganizationService(runtime.UserId), Times.Once);
                }
            }
        }

        [Fact]
        public void Execute_OptionalLogger_InjectsOnlyWhenAvailable()
        {
            using (var root = new PluginContainer())
            {
                root.Register<LoggingHandler>();
                var plugin = new Adapter<LoggingHandler>(root);
                var runtime = new Runtime();
                var logger = new Mock<ILogger>();
                runtime.Provider.Setup(provider => provider.GetService(typeof(ILogger))).Returns(logger.Object);
                plugin.Execute(runtime.Provider.Object);
                Assert.Same(logger.Object, runtime.Context.Object.OutputParameters["logger"]);
                runtime.Provider.Setup(provider => provider.GetService(typeof(ILogger))).Returns(null);
                var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(runtime.Provider.Object));
                Assert.IsType<InvalidOperationException>(exception.InnerException);
                Assert.Contains("ILogger", exception.InnerException.Message);
            }
        }

        [Fact]
        public void Execute_ConcurrentCallsOnSamePlugin_IsolatesExecutionState()
        {
            using (var root = new PluginContainer())
            {
                root.Register<RecordingHandler>();
                var plugin = new Adapter<RecordingHandler>(root);
                Parallel.For(0, 10, index =>
                {
                    var runtime = new Runtime();
                    plugin.Execute(runtime.Provider.Object);
                    Assert.Same(runtime.Service.Object, runtime.Context.Object.OutputParameters["service"]);
                    Assert.Equal(true, runtime.Context.Object.OutputParameters["completed"]);
                });
            }
        }

        [Fact]
        public void Execute_DisposableHandler_DisposesAfterSuccessAndFailure()
        {
            using (var root = new PluginContainer())
            {
                root.Register<DisposableHandler>();
                var plugin = new Adapter<DisposableHandler>(root);
                var success = new Runtime();
                plugin.Execute(success.Provider.Object);
                Assert.Equal(true, success.Context.Object.OutputParameters["disposed"]);

                var failure = new Runtime();
                failure.Context.Object.OutputParameters["fail"] = true;
                Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(failure.Provider.Object));
                Assert.Equal(true, failure.Context.Object.OutputParameters["disposed"]);
            }
        }

        [Fact]
        public void Execute_ExecutionFailureRemainsPrimaryAndAllCleanupIsAttempted()
        {
            using (var root = new PluginContainer())
            {
                root.Register<CleanupFailureHandler>();
                root.Register<FirstCleanupDependency>();
                root.Register<SecondCleanupDependency>();
                var runtime = new Runtime();
                runtime.Context.Object.OutputParameters["fail"] = true;
                runtime.Tracing.Setup(tracing => tracing.Trace(It.IsAny<string>(), It.IsAny<object[]>()))
                    .Callback<string, object[]>((format, arguments) =>
                    {
                        if (format.StartsWith("Cleanup", StringComparison.Ordinal))
                        {
                            throw new InvalidOperationException("trace failed");
                        }
                    });

                var plugin = new Adapter<CleanupFailureHandler>(root);
                Assert.Same(
                    CleanupFailureHandler.Failure,
                    Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(runtime.Provider.Object)));
                Assert.True((bool)runtime.Context.Object.OutputParameters["handlerDisposed"]);
                Assert.True((bool)runtime.Context.Object.OutputParameters["firstDisposed"]);
                Assert.True((bool)runtime.Context.Object.OutputParameters["secondDisposed"]);
            }
        }

        [Fact]
        public void Execute_CleanupOnlyFailureSurfacesAllDisposalFailures()
        {
            using (var root = new PluginContainer())
            {
                root.Register<CleanupFailureHandler>();
                root.Register<FirstCleanupDependency>();
                root.Register<SecondCleanupDependency>();
                var runtime = new Runtime();
                var plugin = new Adapter<CleanupFailureHandler>(root);

                var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(runtime.Provider.Object));

                var cleanupFailure = Assert.IsType<AggregateException>(exception.InnerException);
                Assert.Equal(3, cleanupFailure.InnerExceptions.Count);
                Assert.True((bool)runtime.Context.Object.OutputParameters["handlerDisposed"]);
                Assert.True((bool)runtime.Context.Object.OutputParameters["firstDisposed"]);
                Assert.True((bool)runtime.Context.Object.OutputParameters["secondDisposed"]);
            }
        }

        [Theory]
        [InlineData("tracing")]
        [InlineData("context")]
        [InlineData("factory")]
        [InlineData("service")]
        public void Execute_MissingRuntimeService_FailsExplicitly(string missing)
        {
            using (var root = new PluginContainer())
            {
                root.Register<RecordingHandler>();
                var runtime = new Runtime();
                if (missing == "service")
                {
                    runtime.Factory.Setup(factory => factory.CreateOrganizationService(It.IsAny<Guid?>())).Returns((IOrganizationService)null);
                }
                else
                {
                    var type = missing == "tracing" ? typeof(ITracingService) :
                        missing == "context" ? typeof(IPluginExecutionContext) : typeof(IOrganizationServiceFactory);
                    runtime.Provider.Setup(provider => provider.GetService(type)).Returns(null);
                }

                var plugin = new Adapter<RecordingHandler>(root);
                var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(runtime.Provider.Object));
                Assert.IsType<InvalidOperationException>(exception.InnerException);
            }
        }

        [Fact]
        public void Execute_FailedHandler_TracesAndWrapsOriginalException()
        {
            using (var root = new PluginContainer())
            {
                root.Register<FailedHandler>();
                var runtime = new Runtime();
                var plugin = new Adapter<FailedHandler>(root);
                var exception = Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(runtime.Provider.Object));
                Assert.IsType<ApplicationException>(exception.InnerException);
                Assert.Equal("handler failed", exception.InnerException.Message);
                runtime.Tracing.Verify(tracing => tracing.Trace(
                    It.Is<string>(format => format.Contains("failed")),
                    It.IsAny<object[]>()), Times.Once);
            }
        }

        [Fact]
        public void Execute_PlatformException_PreservesInstance()
        {
            using (var root = new PluginContainer())
            {
                root.Register<PlatformFailureHandler>();
                var runtime = new Runtime();
                var plugin = new Adapter<PlatformFailureHandler>(root);
                Assert.Same(PlatformFailureHandler.Failure, Assert.Throws<InvalidPluginExecutionException>(() => plugin.Execute(runtime.Provider.Object)));
            }
        }

        [Fact]
        public void Execute_NullProvider_RejectsInvalidArgument()
        {
            using (var root = new PluginContainer())
            {
                var runtime = new Runtime();
                var plugin = new Adapter<RecordingHandler>(root);
                Assert.Throws<ArgumentNullException>(() => plugin.Execute(null));
            }
        }

        public sealed class Adapter<THandler> : PluginBase<THandler>
            where THandler : IPluginHandler
        {
            public Adapter(PluginContainer container)
                : base(container)
            {
            }
        }

        public sealed class RecordingHandler : IPluginHandler
        {
            private readonly IPluginExecution execution;
            private readonly IOrganizationService service;
            private readonly ITracingService tracing;

            public RecordingHandler(IPluginExecution execution, IPluginExecutionContext context, IOrganizationService service, ITracingService tracing)
            {
                Assert.Same(context, execution.Context);
                this.execution = execution;
                this.service = service;
                this.tracing = tracing;
            }

            public void Execute()
            {
                this.execution.Context.OutputParameters["completed"] = true;
                this.execution.Context.OutputParameters["service"] = this.service;
                this.execution.Context.OutputParameters["tracing"] = this.tracing;
                Assert.Same(this.service, this.execution.Service);
                Assert.Same(this.tracing, this.execution.Tracing);
            }
        }

        public sealed class LoggingHandler : IPluginHandler
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
                this.context.OutputParameters["logger"] = this.logger;
            }
        }

        public sealed class FailedHandler : IPluginHandler
        {
            public void Execute()
            {
                throw new ApplicationException("handler failed");
            }
        }

        public sealed class PlatformFailureHandler : IPluginHandler
        {
            public static readonly InvalidPluginExecutionException Failure = new InvalidPluginExecutionException("business validation failed");

            public void Execute()
            {
                throw Failure;
            }
        }

        public sealed class DisposableHandler : IPluginHandler, IDisposable
        {
            private readonly IPluginExecutionContext context;

            public DisposableHandler(IPluginExecutionContext context)
            {
                this.context = context;
            }

            public void Execute()
            {
                if (this.context.OutputParameters.Contains("fail"))
                {
                    throw new ApplicationException("handler failed");
                }

            }

            public void Dispose()
            {
                this.context.OutputParameters["disposed"] = true;
            }
        }

        public sealed class CleanupFailureHandler : IPluginHandler, IDisposable
        {
            public static readonly InvalidPluginExecutionException Failure =
                new InvalidPluginExecutionException("business validation failed");

            private readonly IPluginExecutionContext context;

            public CleanupFailureHandler(
                IPluginExecutionContext context,
                FirstCleanupDependency first,
                SecondCleanupDependency second)
            {
                this.context = context;
            }

            public void Execute()
            {
                if (this.context.OutputParameters.Contains("fail"))
                {
                    throw Failure;
                }
            }

            public void Dispose()
            {
                this.context.OutputParameters["handlerDisposed"] = true;
                throw new InvalidOperationException("handler cleanup failed");
            }
        }

        public sealed class FirstCleanupDependency : IDisposable
        {
            private readonly IPluginExecutionContext context;

            public FirstCleanupDependency(IPluginExecutionContext context)
            {
                this.context = context;
            }

            public void Dispose()
            {
                this.context.OutputParameters["firstDisposed"] = true;
                throw new InvalidOperationException("first cleanup failed");
            }
        }

        public sealed class SecondCleanupDependency : IDisposable
        {
            private readonly IPluginExecutionContext context;

            public SecondCleanupDependency(IPluginExecutionContext context)
            {
                this.context = context;
            }

            public void Dispose()
            {
                this.context.OutputParameters["secondDisposed"] = true;
                throw new InvalidOperationException("second cleanup failed");
            }
        }

        private sealed class Runtime
        {
            public Runtime()
            {
                this.Context.SetupGet(context => context.UserId).Returns(this.UserId);
                this.Context.SetupGet(context => context.OutputParameters).Returns(new ParameterCollection());
                this.Factory.Setup(factory => factory.CreateOrganizationService(this.UserId)).Returns(this.Service.Object);
                this.Provider.Setup(provider => provider.GetService(typeof(IPluginExecutionContext))).Returns(this.Context.Object);
                this.Provider.Setup(provider => provider.GetService(typeof(IOrganizationServiceFactory))).Returns(this.Factory.Object);
                this.Provider.Setup(provider => provider.GetService(typeof(ITracingService))).Returns(this.Tracing.Object);
            }

            public Guid UserId { get; } = Guid.NewGuid();

            public Mock<IServiceProvider> Provider { get; } = new Mock<IServiceProvider>();

            public Mock<IPluginExecutionContext> Context { get; } = new Mock<IPluginExecutionContext>();

            public Mock<IOrganizationServiceFactory> Factory { get; } = new Mock<IOrganizationServiceFactory>();

            public Mock<IOrganizationService> Service { get; } = new Mock<IOrganizationService>();

            public Mock<ITracingService> Tracing { get; } = new Mock<ITracingService>();
        }
    }
}
