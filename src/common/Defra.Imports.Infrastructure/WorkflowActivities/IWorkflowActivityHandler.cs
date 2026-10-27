namespace Defra.Imports.Infrastructure.Workflows
{
    /// <summary>Defines a synchronous, constructor-injected workflow activity use case.</summary>
    public interface IWorkflowActivityHandler
    {
        /// <summary>Executes the use case within the current workflow invocation.</summary>
        void Execute();
    }
}
