namespace Defra.Imports.Infrastructure.Plugins
{
    /// <summary>The Dataverse event pipeline stages, as reported by <c>IExecutionContext.Stage</c>.</summary>
    public static class PluginStages
    {
        /// <summary>The pre-validation stage.</summary>
        public const int PreValidation = 10;

        /// <summary>The pre-operation stage, which runs inside the transaction before the database operation.</summary>
        public const int PreOperation = 20;

        /// <summary>The post-operation stage, which runs inside the transaction after the database operation.</summary>
        public const int PostOperation = 40;
    }
}
