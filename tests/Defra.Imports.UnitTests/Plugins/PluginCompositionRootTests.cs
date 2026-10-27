namespace Defra.Imports.UnitTests.Plugins
{
    using Defra.Imports.Application.CommodityComplements;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Contexts;
    using Defra.Imports.Infrastructure.Execution;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Plugins;
    using Defra.Imports.Plugins.ImportApplication;
    using Defra.Imports.Plugins.ImporterNotification;
    using Defra.Imports.Plugins.Itahc;
    using Defra.Imports.Plugins.ParameterSetKeyDataPair;
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
        public void Root_ExecutionScopeWithRuntimeServices_ResolvesPopulateAnimalNumberHandler()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IPluginExecution>(runtime.CreateExecution());
                scope.Register<IOrganizationService>(runtime.Service.Object);
                scope.Register<ITracingService>(runtime.Tracing);

                var handler = scope.Resolve<PopulateAnimalNumberHandler>();

                Assert.NotNull(handler);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithOrganizationService_ResolvesTheSharedCommodityComplementKeyDataApplier()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(runtime.Service.Object);
                scope.Register<ITracingService>(runtime.Tracing);

                var applier = scope.Resolve<ICommodityComplementKeyDataApplier>();

                Assert.IsType<CommodityComplementKeyDataApplier>(applier);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithRuntimeServices_ResolvesTheItahcWatchListHandler()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IPluginExecution>(runtime.CreateExecution());
                scope.Register<IOrganizationService>(runtime.Service.Object);

                var handler = scope.Resolve<FlagRecordOnWatchListHandler>();

                Assert.NotNull(handler);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithRuntimeServices_ResolvesTheImporterNotificationWatchListHandler()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IPluginExecution>(runtime.CreateExecution());
                scope.Register<IOrganizationService>(runtime.Service.Object);

                var handler = scope.Resolve<FlagRecordOnWatchListNotificationHandler>();

                Assert.NotNull(handler);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithOrganizationService_ResolvesTheSharedWatchFlagCreator()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(runtime.Service.Object);

                var creator = scope.Resolve<IWatchFlagCreator>();

                Assert.IsType<WatchFlagCreator>(creator);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithRuntimeServices_ResolvesTheAssociateWatchFlagsFromPrimaryItahcHandler()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IPluginExecution>(runtime.CreateExecution());
                scope.Register<IOrganizationService>(runtime.Service.Object);

                var handler = scope.Resolve<AssociateWatchFlagsFromPrimaryItahcHandler>();

                Assert.NotNull(handler);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithRuntimeServices_ResolvesTheAssociateWatchFlagsFromNotificationHandler()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IPluginExecution>(runtime.CreateExecution());
                scope.Register<IOrganizationService>(runtime.Service.Object);

                var handler = scope.Resolve<AssociateWatchFlagsFromNotificationHandler>();

                Assert.NotNull(handler);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithOrganizationService_ResolvesTheSharedWatchFlagAssociator()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(runtime.Service.Object);

                var associator = scope.Resolve<IWatchFlagAssociator>();

                Assert.IsType<WatchFlagAssociator>(associator);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithRuntimeServices_ResolvesTheDetermineInspectionRequirementHandler()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IPluginExecution>(runtime.CreateExecution());
                scope.Register<IOrganizationService>(runtime.Service.Object);
                scope.Register<ITracingService>(runtime.Tracing);

                var handler = scope.Resolve<DetermineInspectionRequirementHandler>();

                Assert.NotNull(handler);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithOrganizationService_ResolvesTheSharedInspectionRequirementDeterminer()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(runtime.Service.Object);
                scope.Register<ITracingService>(runtime.Tracing);

                var determiner = scope.Resolve<IInspectionRequirementDeterminer>();

                Assert.IsType<InspectionRequirementDeterminer>(determiner);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithTracingService_ResolvesATimestampedLogWriter()
        {
            var runtime = new PluginRuntime();
            using (var scope = PluginCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<ITracingService>(runtime.Tracing);

                var logWriter = scope.Resolve<ILogWriter>();
                logWriter.Log(Severity.Info, "Tag", "Message");

                Assert.IsType<TimestampedTracingLogWriter>(logWriter);
                Assert.Matches(@"^\d{4}-\d{2}-\d{2}T.*: Tag: Info: Message$", Assert.Single(runtime.Tracing.Messages));
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
