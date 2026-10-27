namespace Defra.Imports.Infrastructure.Logging
{
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Writes messages to the tracing service without a timestamp, as the legacy workflow activity base class did.
    /// The <see cref="DependencyInjection.PluginContainer"/> requires exactly one public constructor, so this type
    /// exposes the single-argument form that the container can resolve.
    /// </summary>
    public sealed class UntimestampedTracingLogWriter : TracingServiceLogWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UntimestampedTracingLogWriter"/> class.
        /// </summary>
        /// <param name="tracingService">The current invocation's tracing service.</param>
        public UntimestampedTracingLogWriter(ITracingService tracingService)
            : base(tracingService)
        {
        }
    }
}
