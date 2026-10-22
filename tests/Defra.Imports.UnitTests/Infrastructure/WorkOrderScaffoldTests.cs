namespace Defra.Imports.UnitTests.Infrastructure
{
    using System;
    using Defra.Imports.Application.Handlers;
    using Defra.Imports.Application.WorkOrders;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins.Utils;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Microsoft.Xrm.Sdk.Workflow;
    using Moq;
    using Xunit;

    public sealed class WorkOrderScaffoldTests
    {
        [Fact]
        public void GetInitializationContext_ExistingWorkOrder_UsesBoundedIdQueryAndProjectsResult()
        {
            var id = Guid.NewGuid();
            var service = new Mock<IOrganizationService>(MockBehavior.Strict);
            service.Setup(value => value.RetrieveMultiple(It.Is<QueryExpression>(query =>
                query.EntityName == "msdyn_workorder" &&
                query.TopCount == 1 &&
                !query.ColumnSet.AllColumns &&
                query.ColumnSet.Columns.Count == 1 &&
                query.ColumnSet.Columns[0] == "msdyn_workorderid" &&
                query.Criteria.Conditions.Count == 1 &&
                query.Criteria.Conditions[0].AttributeName == "msdyn_workorderid" &&
                query.Criteria.Conditions[0].Operator == ConditionOperator.Equal &&
                (Guid)query.Criteria.Conditions[0].Values[0] == id)))
                .Returns(new EntityCollection(new[] { new Entity("msdyn_workorder", id) }));
            var application = new WorkOrderApplicationService(new WorkOrderQueries(service.Object));
            Assert.Equal(id, application.GetInitializationContext(id).WorkOrderId);
            service.VerifyAll();
        }

        [Fact]
        public void GetInitializationContext_EmptyOrMissingId_RejectsInvalidRequest()
        {
            var service = new Mock<IOrganizationService>();
            var queries = new WorkOrderQueries(service.Object);
            Assert.Throws<ArgumentException>(() => queries.GetInitializationContext(Guid.Empty));
            service.Verify(value => value.RetrieveMultiple(It.IsAny<QueryBase>()), Times.Never);
            service.Setup(value => value.RetrieveMultiple(It.IsAny<QueryBase>())).Returns(new EntityCollection());
            Assert.Throws<InvalidOperationException>(() => queries.GetInitializationContext(Guid.NewGuid()));
        }

        [Fact]
        public void Resolve_WorkflowExecutionScope_ReusesApplicationWithoutPluginContext()
        {
            var id = Guid.NewGuid();
            var service = new Mock<IOrganizationService>(MockBehavior.Strict);
            service.Setup(value => value.RetrieveMultiple(It.IsAny<QueryExpression>()))
                .Returns(new EntityCollection(new[] { new Entity("msdyn_workorder", id) }));
            using (var root = new PluginContainer())
            {
                root.Register<IWorkOrderApplicationService, WorkOrderApplicationService>();
                root.Register<WorkOrderQueries>();
                using (var scope = root.CreateExecutionScope())
                {
                    var workflowContext = Mock.Of<IWorkflowContext>();
                    scope.Register<IWorkflowContext>(workflowContext);
                    scope.Register<IOrganizationService>(service.Object);
                    scope.Register<ITracingService>(Mock.Of<ITracingService>());
                    Assert.Same(workflowContext, scope.Resolve<IWorkflowContext>());
                    Assert.Equal(id, scope.Resolve<IWorkOrderApplicationService>().GetInitializationContext(id).WorkOrderId);
                    Assert.Throws<InvalidOperationException>(() => scope.Resolve<IPluginExecutionContext>());
                }
            }
        }

        [Fact]
        public void Execute_WorkOrderCreate_ReadsApplicationContextWithoutWrites()
        {
            var id = Guid.NewGuid();
            var context = new Mock<IPluginExecutionContext>();
            context.SetupGet(value => value.MessageName).Returns("Create");
            context.SetupGet(value => value.PrimaryEntityName).Returns("msdyn_workorder");
            context.SetupGet(value => value.PrimaryEntityId).Returns(id);
            var service = new Mock<IOrganizationService>(MockBehavior.Strict);
            var tracing = new Mock<ITracingService>();
            var application = new Mock<IWorkOrderApplicationService>(MockBehavior.Strict);
            application.Setup(value => value.GetInitializationContext(id)).Returns(new WorkOrderInitializationContext(id));
            var handler = new WorkOrderInitialisationHandler(application.Object, new PluginExecution(context.Object, service.Object, tracing.Object));
            handler.Execute();
            application.Verify(value => value.GetInitializationContext(id), Times.Once);
            tracing.Verify(value => value.Trace(It.Is<string>(message => message.Contains("No business changes")), It.IsAny<object[]>()), Times.Once);
        }

        [Fact]
        public void PluginEntryPoint_UsesSameSharedApplicationQueryWithoutWrites()
        {
            var workOrderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var context = new Mock<IPluginExecutionContext>();
            context.SetupGet(value => value.MessageName).Returns("Create");
            context.SetupGet(value => value.PrimaryEntityName).Returns("msdyn_workorder");
            context.SetupGet(value => value.PrimaryEntityId).Returns(workOrderId);
            context.SetupGet(value => value.UserId).Returns(userId);
            var service = new Mock<IOrganizationService>(MockBehavior.Strict);
            service.Setup(value => value.RetrieveMultiple(It.IsAny<QueryExpression>()))
                .Returns(new EntityCollection(new[] { new Entity("msdyn_workorder", workOrderId) }));
            var factory = new Mock<IOrganizationServiceFactory>(MockBehavior.Strict);
            factory.Setup(value => value.CreateOrganizationService(userId)).Returns(service.Object);
            var tracing = new Mock<ITracingService>();
            var provider = new Mock<IServiceProvider>();
            provider.Setup(value => value.GetService(typeof(ITracingService))).Returns(tracing.Object);
            provider.Setup(value => value.GetService(typeof(IPluginExecutionContext))).Returns(context.Object);
            provider.Setup(value => value.GetService(typeof(IOrganizationServiceFactory))).Returns(factory.Object);

            new WorkOrderInitialisationPlugin().Execute(provider.Object);

            service.Verify(value => value.RetrieveMultiple(It.IsAny<QueryExpression>()), Times.Once);
            service.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData("Update", "msdyn_workorder")]
        [InlineData("Create", "account")]
        public void Execute_WrongExecution_RejectsBeforeApplicationCall(string message, string entity)
        {
            var context = new Mock<IPluginExecutionContext>();
            context.SetupGet(value => value.MessageName).Returns(message);
            context.SetupGet(value => value.PrimaryEntityName).Returns(entity);
            var application = new Mock<IWorkOrderApplicationService>(MockBehavior.Strict);
            var handler = new WorkOrderInitialisationHandler(application.Object, new PluginExecution(
                context.Object, Mock.Of<IOrganizationService>(), Mock.Of<ITracingService>()));
            Assert.Throws<InvalidOperationException>(() => handler.Execute());
        }
    }
}
