namespace Defra.Imports.Infrastructure.Plugins
{
    /// <summary>Defines a constructor-injected plug-in use case.</summary>
    public interface IPluginHandler
    {
        /// <summary>Executes the use case within the current Dataverse invocation.</summary>
        void Execute();
    }
}
