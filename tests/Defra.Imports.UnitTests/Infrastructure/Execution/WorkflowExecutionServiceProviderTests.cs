namespace Defra.Imports.UnitTests.Infrastructure.Execution
{
    using System;
    using Defra.Imports.Infrastructure.Execution;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;
    using Moq;
    using Xunit;

    public sealed class WorkflowExecutionServiceProviderTests
    {
        private readonly Guid executingUserId = Guid.NewGuid();
        private readonly Mock<IWorkflowContext> context = new Mock<IWorkflowContext>();
        private readonly Mock<IOrganizationServiceFactory> factory = new Mock<IOrganizationServiceFactory>(MockBehavior.Strict);
        private readonly WorkflowExecutionServiceProvider provider;

        public WorkflowExecutionServiceProviderTests()
        {
            this.context.SetupGet(value => value.UserId).Returns(this.executingUserId);
            this.context.SetupGet(value => value.InitiatingUserId).Returns(this.executingUserId);
            this.provider = new WorkflowExecutionServiceProvider(this.factory.Object, this.context.Object);
        }

        [Fact]
        public void Constructor_NullFactory_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WorkflowExecutionServiceProvider(null, this.context.Object));
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WorkflowExecutionServiceProvider(this.factory.Object, null));
        }

        [Fact]
        public void GetExecutingUserService_ContextHasUserId_CreatesServiceForThatUser()
        {
            var expected = Mock.Of<IOrganizationService>();
            this.factory.Setup(value => value.CreateOrganizationService(this.executingUserId)).Returns(expected);

            var actual = this.provider.GetExecutingUserService();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GetExecutingUserService_FactoryReturnsNull_ThrowsInvalidOperationException()
        {
            this.factory.Setup(value => value.CreateOrganizationService(this.executingUserId)).Returns((IOrganizationService)null);

            Assert.Throws<InvalidOperationException>(() => this.provider.GetExecutingUserService());
        }

        [Fact]
        public void GetInitiatingUserService_InitiatingUserIsExecutingUser_ReturnsExecutingUserService()
        {
            var expected = Mock.Of<IOrganizationService>();
            this.factory.Setup(value => value.CreateOrganizationService(this.executingUserId)).Returns(expected);

            var actual = this.provider.GetInitiatingUserService();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GetInitiatingUserService_InitiatingUserDiffersFromExecutingUser_ThrowsNotSupportedWithoutCreatingService()
        {
            this.context.SetupGet(value => value.InitiatingUserId).Returns(Guid.NewGuid());

            Assert.Throws<NotSupportedException>(() => this.provider.GetInitiatingUserService());
            this.factory.VerifyNoOtherCalls();
        }

        [Fact]
        public void GetSystemService_Always_ThrowsNotSupportedWithoutCreatingService()
        {
            Assert.Throws<NotSupportedException>(() => this.provider.GetSystemService());
            this.factory.VerifyNoOtherCalls();
        }
    }
}
