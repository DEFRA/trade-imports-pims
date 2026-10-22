namespace Defra.Imports.UnitTests.Infrastructure
{
    using System;
    using Defra.Imports.Dataverse.Contexts;
    using Defra.Imports.Infrastructure.Execution;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;
    using Moq;
    using Xunit;

    public sealed class ExecutionServiceProviderTests
    {
        [Fact]
        public void PluginProvider_UsesDocumentedFactoryArgumentsForEachIdentity()
        {
            var executingId = Guid.NewGuid();
            var initiatingId = Guid.NewGuid();
            var context = new Mock<IPluginExecutionContext>();
            context.SetupGet(value => value.UserId).Returns(executingId);
            context.SetupGet(value => value.InitiatingUserId).Returns(initiatingId);
            var executingService = Mock.Of<IOrganizationService>();
            var initiatingService = Mock.Of<IOrganizationService>();
            var systemService = Mock.Of<IOrganizationService>();
            var factory = new Mock<IOrganizationServiceFactory>(MockBehavior.Strict);
            factory.Setup(value => value.CreateOrganizationService(executingId)).Returns(executingService);
            factory.Setup(value => value.CreateOrganizationService(initiatingId)).Returns(initiatingService);
            factory.Setup(value => value.CreateOrganizationService((Guid?)null)).Returns(systemService);

            var provider = new PluginExecutionServiceProvider(factory.Object, context.Object);

            Assert.Same(executingService, provider.GetExecutingUserService());
            Assert.Same(initiatingService, provider.GetInitiatingUserService());
            Assert.Same(systemService, provider.GetSystemService());
            factory.VerifyAll();
        }

        [Fact]
        public void PluginProvider_PassesGuidEmptyForExecutingUserAsSpecifiedByContext()
        {
            var context = new Mock<IPluginExecutionContext>();
            context.SetupGet(value => value.UserId).Returns(Guid.Empty);
            var service = Mock.Of<IOrganizationService>();
            var factory = new Mock<IOrganizationServiceFactory>(MockBehavior.Strict);
            factory.Setup(value => value.CreateOrganizationService(Guid.Empty)).Returns(service);
            var provider = new PluginExecutionServiceProvider(factory.Object, context.Object);

            Assert.Same(service, provider.GetExecutingUserService());
            factory.VerifyAll();
        }

        [Fact]
        public void WorkflowProvider_RejectsUnsupportedIdentitiesWithoutFallback()
        {
            var userId = Guid.NewGuid();
            var context = new Mock<IWorkflowContext>();
            context.SetupGet(value => value.UserId).Returns(userId);
            context.SetupGet(value => value.InitiatingUserId).Returns(Guid.NewGuid());
            var executingService = Mock.Of<IOrganizationService>();
            var factory = new Mock<IOrganizationServiceFactory>(MockBehavior.Strict);
            factory.Setup(value => value.CreateOrganizationService(userId)).Returns(executingService);

            var provider = new WorkflowExecutionServiceProvider(factory.Object, context.Object);

            Assert.Same(executingService, provider.GetExecutingUserService());
            Assert.Throws<NotSupportedException>(() => provider.GetInitiatingUserService());
            Assert.Throws<NotSupportedException>(() => provider.GetSystemService());
            factory.Verify(value => value.CreateOrganizationService(userId), Times.Once);
            factory.VerifyNoOtherCalls();
        }

        [Fact]
        public void WorkflowProvider_UsesExecutingServiceWhenBothUserIdsMatch()
        {
            var userId = Guid.NewGuid();
            var context = new Mock<IWorkflowContext>();
            context.SetupGet(value => value.UserId).Returns(userId);
            context.SetupGet(value => value.InitiatingUserId).Returns(userId);
            var service = Mock.Of<IOrganizationService>();
            var factory = new Mock<IOrganizationServiceFactory>(MockBehavior.Strict);
            factory.Setup(value => value.CreateOrganizationService(userId)).Returns(service);
            var provider = new WorkflowExecutionServiceProvider(factory.Object, context.Object);

            Assert.Same(service, provider.GetInitiatingUserService());
        }

        [Fact]
        public void ImportsContextFactory_ReturnsCallerOwnedContextForRequestedIdentity()
        {
            var service = Mock.Of<IOrganizationService>();
            var provider = new Mock<IExecutionServiceProvider>(MockBehavior.Strict);
            provider.Setup(value => value.GetInitiatingUserService()).Returns(service);
            var factory = new ImportsContextFactory(provider.Object);

            using (var context = factory.CreateForInitiatingUser())
            {
                Assert.NotNull(context);
            }

            provider.Verify(value => value.GetInitiatingUserService(), Times.Once);
            provider.VerifyNoOtherCalls();
        }
    }
}
