namespace Defra.Imports.UnitTests.Workflows
{
    using System;
    using System.Activities;
    using System.Runtime.Serialization;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Contexts;
    using Defra.Imports.Infrastructure.Execution;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.UnitTests.Support;
    using Defra.Imports.Workflows;
    using Defra.Imports.Workflows.ImportApplication;
    using Defra.Imports.Workflows.WatchLists;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    public sealed class WorkflowCompositionRootTests
    {
        [Fact]
        public void Root_AccessedRepeatedly_ReturnsTheSameInstance()
        {
            Assert.Same(WorkflowCompositionRoot.Root, WorkflowCompositionRoot.Root);
        }

        [Fact]
        public void Root_ExecutionScopeWithRuntimeServices_ResolvesInitialiseWatchListHandler()
        {
            using (var scope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(Mock.Of<IOrganizationService>());

                // The host supplies a CodeActivityContext that cannot be constructed outside the workflow runtime;
                // resolution only stores it, so an uninitialised instance is enough to prove the graph resolves.
                scope.Register<CodeActivityContext>((CodeActivityContext)FormatterServices.GetUninitializedObject(typeof(CodeActivityContext)));
                scope.Register<InitialiseWatchListActivity>(new InitialiseWatchListActivity());

                var handler = scope.Resolve<InitialiseWatchListHandler>();

                Assert.NotNull(handler);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithRuntimeServices_ResolvesDetermineInspectionRequirementHandler()
        {
            using (var scope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(Mock.Of<IOrganizationService>());
                scope.Register<ITracingService>(new RecordingTracingService());

                // The host supplies a CodeActivityContext that cannot be constructed outside the workflow runtime;
                // resolution only stores it, so an uninitialised instance is enough to prove the graph resolves.
                scope.Register<CodeActivityContext>((CodeActivityContext)FormatterServices.GetUninitializedObject(typeof(CodeActivityContext)));
                scope.Register<DetermineInspectionRequirement>(new DetermineInspectionRequirement());

                var handler = scope.Resolve<DetermineInspectionRequirementHandler>();

                Assert.NotNull(handler);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithRuntimeServices_ResolvesInspectionPlaceOfOriginChangeHandler()
        {
            using (var scope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(Mock.Of<IOrganizationService>());
                scope.Register<ITracingService>(new RecordingTracingService());

                // The host supplies a CodeActivityContext that cannot be constructed outside the workflow runtime;
                // resolution only stores it, so an uninitialised instance is enough to prove the graph resolves.
                scope.Register<CodeActivityContext>((CodeActivityContext)FormatterServices.GetUninitializedObject(typeof(CodeActivityContext)));
                scope.Register<InspectionPlaceOfOriginChange>(new InspectionPlaceOfOriginChange());

                var handler = scope.Resolve<InspectionPlaceOfOriginChangeHandler>();

                Assert.NotNull(handler);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithOrganizationService_ResolvesTheSharedInspectionRequirementDeterminer()
        {
            using (var scope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(Mock.Of<IOrganizationService>());
                scope.Register<ITracingService>(new RecordingTracingService());

                var determiner = scope.Resolve<IInspectionRequirementDeterminer>();

                Assert.IsType<InspectionRequirementDeterminer>(determiner);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithOrganizationService_ResolvesTheSharedInspectionPlaceOfOriginChanger()
        {
            using (var scope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(Mock.Of<IOrganizationService>());
                scope.Register<ITracingService>(new RecordingTracingService());

                var changer = scope.Resolve<IInspectionPlaceOfOriginChanger>();

                Assert.IsType<InspectionPlaceOfOriginChanger>(changer);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithTracingService_ResolvesAnUntimestampedLogWriter()
        {
            var tracing = new RecordingTracingService();
            using (var scope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<ITracingService>(tracing);

                var logWriter = scope.Resolve<ILogWriter>();
                logWriter.Log(Severity.Info, "Tag", "Message");

                Assert.IsType<UntimestampedTracingLogWriter>(logWriter);
                Assert.Equal("Tag: Info: Message", Assert.Single(tracing.Messages));
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithOrganizationService_ResolvesTheSharedWatchListInitialiser()
        {
            using (var scope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IOrganizationService>(Mock.Of<IOrganizationService>());

                var initialiser = scope.Resolve<IWatchListInitialiser>();

                Assert.IsType<WatchListInitialiser>(initialiser);
            }
        }

        [Fact]
        public void Root_ExecutionScopeWithServiceProvider_ResolvesTheImportsContextFactory()
        {
            using (var scope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                scope.Register<IExecutionServiceProvider>(Mock.Of<IExecutionServiceProvider>());

                var factory = scope.Resolve<IImportsContextFactory>();

                Assert.IsType<ImportsContextFactory>(factory);
            }
        }

        [Fact]
        public void Root_ExecutionScope_DoesNotProvidePluginExecutionContext()
        {
            using (var scope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                Assert.Throws<InvalidOperationException>(() => scope.Resolve<IPluginExecutionContext>());
            }
        }

        [Fact]
        public void Root_ExecutionScope_DoesNotProvideTheActivityItself()
        {
            using (var scope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                Assert.Throws<InvalidOperationException>(() => scope.Resolve<InitialiseWatchListActivity>());
            }
        }
    }
}
