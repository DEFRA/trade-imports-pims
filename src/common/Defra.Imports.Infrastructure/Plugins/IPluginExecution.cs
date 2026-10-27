namespace Defra.Imports.Infrastructure.Plugins
{
    using Microsoft.Xrm.Sdk;

    /// <summary>Exposes only the current invocation's SDK services, without service location.</summary>
    public interface IPluginExecution
    {
        /// <summary>Gets the current plug-in execution context.</summary>
        IPluginExecutionContext Context { get; }

        /// <summary>Gets the organisation service for the step's executing user.</summary>
        IOrganizationService Service { get; }

        /// <summary>Gets the current invocation's tracing service.</summary>
        ITracingService Tracing { get; }
    }
}
