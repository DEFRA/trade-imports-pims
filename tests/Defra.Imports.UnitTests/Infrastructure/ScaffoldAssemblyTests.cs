namespace Defra.Imports.UnitTests.Infrastructure
{
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Infrastructure.Workflows;
    using Defra.Imports.Plugins;
    using Defra.Imports.Plugins.Utils;
    using Defra.Imports.Workflows;
    using Xunit;

    public sealed class ScaffoldAssemblyTests
    {
        [Fact]
        public void UnitTestRouting_UsesOneSharedAssemblyForBothHeadsAndApplicationTypes()
        {
            var sharedAssembly = typeof(IPluginHandler).Assembly;

            Assert.Same(sharedAssembly, typeof(IWorkflowActivityHandler).Assembly);
            Assert.Same(sharedAssembly, typeof(WorkOrderQueries).Assembly);
            Assert.Same(sharedAssembly, typeof(PluginBase<>).Assembly);
            Assert.Same(sharedAssembly, typeof(WorkflowActivityBase<>).Assembly);
        }

        [Fact]
        public void CompositionRoots_AreReusedPerHeadAndDoNotRegisterExampleActivities()
        {
            Assert.Same(PluginCompositionRoot.Root, PluginCompositionRoot.Root);
            Assert.Same(WorkflowCompositionRoot.Root, WorkflowCompositionRoot.Root);
            Assert.NotSame(PluginCompositionRoot.Root, WorkflowCompositionRoot.Root);

            using (var pluginScope = PluginCompositionRoot.Root.CreateExecutionScope())
            using (var workflowScope = WorkflowCompositionRoot.Root.CreateExecutionScope())
            {
                Assert.Throws<System.InvalidOperationException>(() => pluginScope.Resolve<WorkOrderInitialisationPlugin>());
                Assert.Throws<System.InvalidOperationException>(() => workflowScope.Resolve<Defra.Imports.Workflows.Utils.LoadWorkOrderContextActivity>());
            }
        }
    }
}
