namespace Defra.Imports.UnitTests.Infrastructure
{
    using System;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.Plugins;
    using Defra.Imports.Infrastructure.Workflows;
    using Xunit;

    public sealed class SharedSourceAssemblyTests
    {
        [Fact]
        public void UnitTestConfiguration_SharedSourceTypes_ComeFromOneAssembly()
        {
            var expected = typeof(IPluginHandler).Assembly;
            var sharedTypes = new[]
            {
                typeof(IWorkflowActivityHandler),
                typeof(WatchListQueries),
                typeof(IWatchListInitialiser),
                typeof(EntityExtensions),
                typeof(PluginBase<>),
                typeof(WorkflowActivityBase<>),
            };

            Assert.All(sharedTypes, type => Assert.Same(expected, type.Assembly));
        }
    }
}
