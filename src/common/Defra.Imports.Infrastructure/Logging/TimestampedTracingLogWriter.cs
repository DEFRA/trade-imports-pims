namespace Defra.Imports.Infrastructure.Logging
{
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Writes timestamped messages to the tracing service, as the legacy plug-in base class did. The
    /// <see cref="DependencyInjection.PluginContainer"/> requires exactly one public constructor, so this type
    /// exposes the single-argument form that the container can resolve.
    /// </summary>
    public sealed class TimestampedTracingLogWriter : TracingServiceLogWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimestampedTracingLogWriter"/> class.
        /// </summary>
        /// <param name="tracingService">The current invocation's tracing service.</param>
        public TimestampedTracingLogWriter(ITracingService tracingService)
            : base(tracingService, true)
        {
        }
    }
}
