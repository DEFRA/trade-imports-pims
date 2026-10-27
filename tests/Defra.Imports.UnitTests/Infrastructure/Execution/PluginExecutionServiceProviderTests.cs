namespace Defra.Imports.UnitTests.Infrastructure.Execution
{
    using System;
    using Defra.Imports.Infrastructure.Execution;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class PluginExecutionServiceProviderTests
    {
        private readonly Guid executingUserId = Guid.NewGuid();
        private readonly Guid initiatingUserId = Guid.NewGuid();
        private readonly Mock<IPluginExecutionContext> context = new Mock<IPluginExecutionContext>();
        private readonly Mock<IOrganizationServiceFactory> factory = new Mock<IOrganizationServiceFactory>(MockBehavior.Strict);
        private readonly PluginExecutionServiceProvider provider;

        public PluginExecutionServiceProviderTests()
        {
            this.context.SetupGet(value => value.UserId).Returns(this.executingUserId);
            this.context.SetupGet(value => value.InitiatingUserId).Returns(this.initiatingUserId);
            this.provider = new PluginExecutionServiceProvider(this.factory.Object, this.context.Object);
        }

        [Fact]
        public void Constructor_NullFactory_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PluginExecutionServiceProvider(null, this.context.Object));
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PluginExecutionServiceProvider(this.factory.Object, null));
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
        public void GetExecutingUserService_ContextHasEmptyUserId_PassesEmptyGuidToFactory()
        {
            var expected = Mock.Of<IOrganizationService>();
            this.context.SetupGet(value => value.UserId).Returns(Guid.Empty);
            this.factory.Setup(value => value.CreateOrganizationService(Guid.Empty)).Returns(expected);

            var actual = this.provider.GetExecutingUserService();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GetInitiatingUserService_ContextHasInitiatingUserId_CreatesServiceForThatUser()
        {
            var expected = Mock.Of<IOrganizationService>();
            this.factory.Setup(value => value.CreateOrganizationService(this.initiatingUserId)).Returns(expected);

            var actual = this.provider.GetInitiatingUserService();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GetSystemService_Always_CreatesServiceWithoutUserId()
        {
            var expected = Mock.Of<IOrganizationService>();
            this.factory.Setup(value => value.CreateOrganizationService((Guid?)null)).Returns(expected);

            var actual = this.provider.GetSystemService();

            Assert.Same(expected, actual);
        }

        [Fact]
        public void GetExecutingUserService_FactoryReturnsNull_ThrowsInvalidOperationException()
        {
            this.factory.Setup(value => value.CreateOrganizationService(this.executingUserId)).Returns((IOrganizationService)null);

            Assert.Throws<InvalidOperationException>(() => this.provider.GetExecutingUserService());
        }

        [Fact]
        public void GetInitiatingUserService_FactoryReturnsNull_ThrowsInvalidOperationException()
        {
            this.factory.Setup(value => value.CreateOrganizationService(this.initiatingUserId)).Returns((IOrganizationService)null);

            Assert.Throws<InvalidOperationException>(() => this.provider.GetInitiatingUserService());
        }

        [Fact]
        public void GetSystemService_FactoryReturnsNull_ThrowsInvalidOperationException()
        {
            this.factory.Setup(value => value.CreateOrganizationService((Guid?)null)).Returns((IOrganizationService)null);

            Assert.Throws<InvalidOperationException>(() => this.provider.GetSystemService());
        }
    }
}
