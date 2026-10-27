namespace Defra.Imports.UnitTests.Support
{
    using Microsoft.Xrm.Sdk;
    using Xunit;

    // Base class for tests of IPlugin entry points. It supplies a simulated Dataverse invocation and the
    // assertions most plug-in tests need, so derived tests describe only the step they are exercising.
    public abstract class PluginTestBase<TPlugin>
        where TPlugin : class, IPlugin, new()
    {
        protected PluginRuntime Runtime { get; } = new PluginRuntime();

        protected TPlugin Plugin { get; } = new TPlugin();

        protected void Execute()
        {
            this.Plugin.Execute(this.Runtime.Provider.Object);
        }

        // The strict service mock proves nothing was read or written when the step contract was not met.
        protected void AssertExecuteFailsWithoutCallingService()
        {
            Assert.Throws<InvalidPluginExecutionException>(this.Execute);
            this.Runtime.Service.VerifyNoOtherCalls();
        }
    }
}
