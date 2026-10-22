namespace Defra.Imports.UnitTests.Plugins
{
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Contexts;
    using Defra.Imports.Infrastructure.Execution;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins;
    using Defra.Imports.Plugins.WatchLists;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class PluginCompositionRootTests
    {
        [Fact]
        public void Root_AccessedRepeatedly_ReturnsTheSameInstance()
        {
            Assert.Same(PluginCompositionRoot.Root, PluginCompositionRoot.Root);
        }

        [Fact]
        public void Root_ExecutionScopeWithRuntimeServices_ResolvesWatchListInitialisationHandler()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IPluginExecution>(runtime.CreateExecution());
                scope.Register<IOrganizationService>(runtime.Service.Object);

                var handler = scope.Resolve<WatchListInitialisationHandler>();

                Assert.NotNull(handler);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithOrganizationService_ResolvesTheSharedWatchListInitialiser()
        {
            var service = new Mock<IOrganizationService>(MockBehavior.Strict);
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(service.Object);

                var initialiser = scope.Resolve<IWatchListInitialiser>();

                Assert.IsType<WatchListInitialiser>(initialiser);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithServiceProvider_ResolvesTheImportsContextFactory()
        {
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IExecutionServiceProvider>(Mock.Of<IExecutionServiceProvider>());

                var factory = scope.Resolve<IImportsContextFactory>();

                Assert.IsType<ImportsContextFactory>(factory);
            }
        }
    }
}
