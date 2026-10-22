namespace Defra.Imports.UnitTests.Dataverse.Contexts
{
    using System;
    using Defra.Imports.Dataverse.Contexts;
    using Defra.Imports.Infrastructure.Execution;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class ImportsContextFactoryTests
    {
        private readonly Mock<IExecutionServiceProvider> provider = new Mock<IExecutionServiceProvider>(MockBehavior.Strict);
        private readonly ImportsContextFactory factory;

        public ImportsContextFactoryTests()
        {
            this.factory = new ImportsContextFactory(this.provider.Object);
        }

        [Fact]
        public void Constructor_NullServiceProvider_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ImportsContextFactory(null));
        }

        [Fact]
        public void CreateForExecutingUser_Always_UsesExecutingUserService()
        {
            this.provider.Setup(value => value.GetExecutingUserService()).Returns(Mock.Of<IOrganizationService>());

            using (var context = this.factory.CreateForExecutingUser())
            {
                Assert.NotNull(context);
            }

            this.provider.Verify(value => value.GetExecutingUserService(), Times.Once);
            this.provider.VerifyNoOtherCalls();
        }

        [Fact]
        public void CreateForInitiatingUser_Always_UsesInitiatingUserService()
        {
            this.provider.Setup(value => value.GetInitiatingUserService()).Returns(Mock.Of<IOrganizationService>());

            using (var context = this.factory.CreateForInitiatingUser())
            {
                Assert.NotNull(context);
            }

            this.provider.Verify(value => value.GetInitiatingUserService(), Times.Once);
            this.provider.VerifyNoOtherCalls();
        }

        [Fact]
        public void CreateForSystem_Always_UsesSystemService()
        {
            this.provider.Setup(value => value.GetSystemService()).Returns(Mock.Of<IOrganizationService>());

            using (var context = this.factory.CreateForSystem())
            {
                Assert.NotNull(context);
            }

            this.provider.Verify(value => value.GetSystemService(), Times.Once);
            this.provider.VerifyNoOtherCalls();
        }
    }
}
