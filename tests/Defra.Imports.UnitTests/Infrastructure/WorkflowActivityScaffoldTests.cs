namespace Defra.Imports.UnitTests.Infrastructure
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Workflows;
    using Defra.Imports.Workflows.Utils;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Microsoft.Xrm.Sdk.Workflow;
    using Moq;
    using Xunit;

    public sealed class WorkflowActivityScaffoldTests
    {
        [Fact]
        public void Invoke_UsesSharedApplicationQueryAndMapsWorkflowOutput()
        {
            var workOrderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var service = new Mock<IOrganizationService>(MockBehavior.Strict);
            service.Setup(value => value.RetrieveMultiple(It.IsAny<QueryExpression>()))
                .Returns(new EntityCollection(new[] { new Entity("msdyn_workorder", workOrderId) }));

            var output = Invoke(
                new LoadWorkOrderContextActivity(),
                "WorkOrder",
                new EntityReference("msdyn_workorder", workOrderId),
                userId,
                service.Object);

            var result = Assert.IsType<EntityReference>(output["RetrievedWorkOrder"]);
            Assert.Equal(workOrderId, result.Id);
            service.Verify(value => value.RetrieveMultiple(It.IsAny<QueryExpression>()), Times.Once);
        }

        [Fact]
        public void Invoke_BindsExactActivityContextAndServices_ThenFreezesIsolatedRoot()
        {
            using (var root = CreateTestRoot())
            {
                var activity = new TestWorkflowActivity(root);
                var id = Guid.NewGuid();
                var userId = Guid.NewGuid();
                var service = CreateOrganizationService(id);
                var output = Invoke(activity, "Input", new EntityReference("account", id), userId, service);

                Assert.Equal(id, Assert.IsType<EntityReference>(output["Output"]).Id);
                Assert.Equal(userId, Assert.IsType<Guid>(output["ExecutingUser"]));
                Assert.Throws<InvalidOperationException>(() => root.Register<TestWorkflowService>());
            }
        }

        [Fact]
        public void Invoke_ReusedActivityConcurrently_KeepsInvocationValuesIsolated()
        {
            using (var root = CreateTestRoot())
            {
                var activity = new TestWorkflowActivity(root);
                var outputs = new IDictionary<string, object>[8];
                Parallel.For(0, outputs.Length, index =>
                {
                    var id = Guid.NewGuid();
                    var userId = Guid.NewGuid();
                    outputs[index] = Invoke(
                        activity,
                        "Input",
                        new EntityReference("account", id),
                        userId,
                        CreateOrganizationService(id));
                });

                for (int index = 0; index < outputs.Length; index++)
                {
                    Assert.NotEqual(Guid.Empty, Assert.IsType<Guid>(outputs[index]["ExecutingUser"]));
                    Assert.Equal(
                        Assert.IsType<EntityReference>(outputs[index]["Output"]).Id,
                        Assert.IsType<Guid>(outputs[index]["ObservedId"]));
                }
            }
        }

        [Fact]
        public void Invoke_MissingWorkflowSdkExtension_FailsExplicitly()
        {
            using (var root = CreateTestRoot())
            {
                var invoker = new WorkflowInvoker(new TestWorkflowActivity(root));
                invoker.Extensions.Add(Mock.Of<ITracingService>());
                invoker.Extensions.Add(Mock.Of<IOrganizationServiceFactory>());

                var exception = Assert.ThrowsAny<Exception>(() => invoker.Invoke(
                    new Dictionary<string, object>
                    {
                        { "Input", new EntityReference("account", Guid.NewGuid()) },
                    }));
                Assert.Contains("IWorkflowContext", exception.ToString());
            }
        }

        [Fact]
        public void Invoke_ExecutionFailureRemainsPrimaryWhileEveryCleanupIsAttempted()
        {
            using (var root = CreateCleanupRoot())
            {
                var recorder = new CleanupRecorder();
                var activity = new CleanupWorkflowActivity(root, recorder);

                var thrown = Assert.ThrowsAny<Exception>(() => InvokeCleanup(activity, true));

                Assert.Contains(EnumerateExceptions(thrown), exception => ReferenceEquals(exception, CleanupWorkflowHandler.Failure));
                Assert.Equal(3, recorder.Disposed.Count);
            }
        }

        [Fact]
        public void Invoke_CleanupOnlyFailureSurfacesAggregateAfterEveryCleanup()
        {
            using (var root = CreateCleanupRoot())
            {
                var recorder = new CleanupRecorder();
                var activity = new CleanupWorkflowActivity(root, recorder);
                var tracing = new Mock<ITracingService>();

                var thrown = Assert.ThrowsAny<Exception>(() => InvokeCleanup(activity, false, tracing));

                Assert.Contains(EnumerateExceptions(thrown), exception => exception.Message == "handler cleanup failed");
                tracing.Verify(value => value.Trace(
                    It.Is<string>(format => format.Contains("failed")),
                    It.Is<object[]>(arguments => HasThreeCleanupFailures(arguments))), Times.Once);
                Assert.Equal(3, recorder.Disposed.Count);
            }
        }

        private static PluginContainer CreateTestRoot()
        {
            var root = new PluginContainer();
            root.Register<TestWorkflowHandler>();
            root.Register<ITestWorkflowService, TestWorkflowService>();
            return root;
        }

        private static PluginContainer CreateCleanupRoot()
        {
            var root = new PluginContainer();
            root.Register<CleanupWorkflowHandler>();
            root.Register<CleanupFirstService>();
            root.Register<CleanupSecondService>();
            return root;
        }

        private static void InvokeCleanup(
            CleanupWorkflowActivity activity,
            bool failExecution,
            Mock<ITracingService> tracing = null)
        {
            var workflowContext = new Mock<IWorkflowContext>();
            workflowContext.SetupGet(value => value.UserId).Returns(Guid.NewGuid());
            var factory = new Mock<IOrganizationServiceFactory>();
            factory.Setup(value => value.CreateOrganizationService(It.IsAny<Guid?>()))
                .Returns(Mock.Of<IOrganizationService>());

            var invoker = new WorkflowInvoker(activity);
            invoker.Extensions.Add(workflowContext.Object);
            invoker.Extensions.Add(factory.Object);
            invoker.Extensions.Add(tracing?.Object ?? Mock.Of<ITracingService>());
            invoker.Invoke(new Dictionary<string, object> { { "FailExecution", failExecution } });
        }

        private static IEnumerable<Exception> EnumerateExceptions(Exception exception)
        {
            yield return exception;
            if (exception is AggregateException aggregate)
            {
                foreach (var innerException in aggregate.InnerExceptions)
                {
                    foreach (var nested in EnumerateExceptions(innerException))
                    {
                        yield return nested;
                    }
                }
            }

            if (exception.InnerException != null)
            {
                foreach (var nested in EnumerateExceptions(exception.InnerException))
                {
                    yield return nested;
                }
            }
        }

        private static bool HasThreeCleanupFailures(object[] arguments)
        {
            return arguments.Any(argument =>
            {
                var aggregate = argument as AggregateException;
                return aggregate != null && aggregate.InnerExceptions.Count == 3;
            });
        }

        private static IOrganizationService CreateOrganizationService(Guid id)
        {
            var service = new Mock<IOrganizationService>(MockBehavior.Strict);
            service.Setup(value => value.Retrieve("account", id, It.IsAny<ColumnSet>()))
                .Returns(new Entity("account", id));
            return service.Object;
        }

        private static IDictionary<string, object> Invoke(
            CodeActivity activity,
            string inputName,
            EntityReference input,
            Guid userId,
            IOrganizationService service)
        {
            var workflowContext = new Mock<IWorkflowContext>();
            workflowContext.SetupGet(value => value.UserId).Returns(userId);
            workflowContext.SetupGet(value => value.InitiatingUserId).Returns(Guid.NewGuid());
            var factory = new Mock<IOrganizationServiceFactory>(MockBehavior.Strict);
            factory.Setup(value => value.CreateOrganizationService(userId)).Returns(service);

            var invoker = new WorkflowInvoker(activity);
            invoker.Extensions.Add(workflowContext.Object);
            invoker.Extensions.Add(factory.Object);
            invoker.Extensions.Add(Mock.Of<ITracingService>());
            return invoker.Invoke(new Dictionary<string, object> { { inputName, input } });
        }

        public interface ITestWorkflowService
        {
            Guid Observe(IOrganizationService service, Guid id);
        }

        public sealed class TestWorkflowActivity : WorkflowActivityBase<TestWorkflowHandler>
        {
            public TestWorkflowActivity(PluginContainer root)
                : base(root)
            {
            }

            [Input("Input")]
            public InArgument<EntityReference> Input { get; set; }

            [Output("Output")]
            public OutArgument<EntityReference> Output { get; set; }

            [Output("Executing user")]
            public OutArgument<Guid> ExecutingUser { get; set; }

            [Output("Observed id")]
            public OutArgument<Guid> ObservedId { get; set; }

            protected override void RegisterActivity(PluginContainer scope)
            {
                scope.Register<TestWorkflowActivity>(this);
            }
        }

        public sealed class TestWorkflowService : ITestWorkflowService
        {
            public Guid Observe(IOrganizationService service, Guid id)
            {
                return service.Retrieve("account", id, new ColumnSet()).Id;
            }
        }

        public sealed class TestWorkflowHandler : IWorkflowActivityHandler
        {
            private readonly TestWorkflowActivity activity;
            private readonly CodeActivityContext context;
            private readonly IWorkflowContext workflowContext;
            private readonly ITestWorkflowService service;
            private readonly IOrganizationService organizationService;

            public TestWorkflowHandler(
                TestWorkflowActivity activity,
                CodeActivityContext context,
                IWorkflowContext workflowContext,
                ITestWorkflowService service,
                IOrganizationService organizationService)
            {
                this.activity = activity;
                this.context = context;
                this.workflowContext = workflowContext;
                this.service = service;
                this.organizationService = organizationService;
            }

            public void Execute()
            {
                var input = this.activity.Input.Get(this.context);
                var observedId = this.service.Observe(this.organizationService, input.Id);
                this.activity.Output.Set(this.context, new EntityReference(input.LogicalName, input.Id));
                this.activity.ExecutingUser.Set(this.context, this.workflowContext.UserId);
                this.activity.ObservedId.Set(this.context, observedId);
            }
        }

        public sealed class CleanupWorkflowActivity : WorkflowActivityBase<CleanupWorkflowHandler>
        {
            private readonly CleanupRecorder recorder;

            public CleanupWorkflowActivity(PluginContainer root, CleanupRecorder recorder)
                : base(root)
            {
                this.recorder = recorder;
            }

            [Input("Fail execution")]
            public InArgument<bool> FailExecution { get; set; }

            protected override void RegisterActivity(PluginContainer scope)
            {
                scope.Register<CleanupWorkflowActivity>(this);
                scope.Register(this.recorder);
            }
        }

        public sealed class CleanupWorkflowHandler : IWorkflowActivityHandler, IDisposable
        {
            public static readonly InvalidPluginExecutionException Failure =
                new InvalidPluginExecutionException("workflow business failure");

            private readonly CleanupWorkflowActivity activity;
            private readonly CodeActivityContext context;
            private readonly CleanupRecorder recorder;
            private readonly CleanupFirstService first;
            private readonly CleanupSecondService second;

            public CleanupWorkflowHandler(
                CleanupWorkflowActivity activity,
                CodeActivityContext context,
                CleanupRecorder recorder,
                CleanupFirstService first,
                CleanupSecondService second)
            {
                this.activity = activity;
                this.context = context;
                this.recorder = recorder;
                this.first = first;
                this.second = second;
            }

            public void Execute()
            {
                if (this.activity.FailExecution.Get(this.context))
                {
                    throw Failure;
                }
            }

            public void Dispose()
            {
                this.recorder.Disposed.Add("handler");
                throw new InvalidOperationException("handler cleanup failed");
            }
        }

        public sealed class CleanupRecorder
        {
            public IList<string> Disposed { get; } = new List<string>();
        }

        public sealed class CleanupFirstService : IDisposable
        {
            private readonly CleanupRecorder recorder;

            public CleanupFirstService(CleanupRecorder recorder)
            {
                this.recorder = recorder;
            }

            public void Dispose()
            {
                this.recorder.Disposed.Add("first");
                throw new InvalidOperationException("first cleanup failed");
            }
        }

        public sealed class CleanupSecondService : IDisposable
        {
            private readonly CleanupRecorder recorder;

            public CleanupSecondService(CleanupRecorder recorder)
            {
                this.recorder = recorder;
            }

            public void Dispose()
            {
                this.recorder.Disposed.Add("second");
                throw new InvalidOperationException("second cleanup failed");
            }
        }
    }
}
