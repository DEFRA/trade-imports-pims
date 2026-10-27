namespace Defra.Imports.Infrastructure.Plugins
{
    /// <summary>The names of the input parameters Dataverse supplies to plug-ins.</summary>
    public static class PluginInputParameters
    {
        /// <summary>The entity or entity reference that the message acts on.</summary>
        public const string Target = "Target";

        /// <summary>The reference to the record whose state the SetState message changes.</summary>
        public const string EntityMoniker = "EntityMoniker";
    }
}
