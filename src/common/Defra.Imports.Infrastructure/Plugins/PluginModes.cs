namespace Defra.Imports.Infrastructure.Plugins
{
    /// <summary>The step execution modes, as reported by <c>IExecutionContext.Mode</c>.</summary>
    public static class PluginModes
    {
        /// <summary>The synchronous mode.</summary>
        public const int Synchronous = 0;

        /// <summary>The asynchronous mode.</summary>
        public const int Asynchronous = 1;
    }
}
