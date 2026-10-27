namespace Defra.Imports.UnitTests.Infrastructure.Plugins
{
    using Defra.Imports.Infrastructure.Plugins;
    using Xunit;

    // The values are the platform's, so they are pinned against the documented registration values.
    public sealed class PluginConstantsTests
    {
        [Fact]
        public void PluginStages_HaveTheDataversePipelineStageNumbers()
        {
            Assert.Equal(10, PluginStages.PreValidation);
            Assert.Equal(20, PluginStages.PreOperation);
            Assert.Equal(40, PluginStages.PostOperation);
        }

        [Fact]
        public void PluginModes_HaveTheDataverseModeNumbers()
        {
            Assert.Equal(0, PluginModes.Synchronous);
            Assert.Equal(1, PluginModes.Asynchronous);
        }

        [Fact]
        public void PluginMessages_HaveTheDataverseMessageNames()
        {
            Assert.Equal("Create", PluginMessages.Create);
            Assert.Equal("Update", PluginMessages.Update);
            Assert.Equal("Delete", PluginMessages.Delete);
            Assert.Equal("SetState", PluginMessages.SetState);
        }

        [Fact]
        public void PluginInputParameters_HaveTheDataverseParameterNames()
        {
            Assert.Equal("Target", PluginInputParameters.Target);
            Assert.Equal("EntityMoniker", PluginInputParameters.EntityMoniker);
        }
    }
}
