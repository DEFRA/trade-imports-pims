namespace Defra.Imports.Infrastructure.Execution
{
    using Microsoft.Xrm.Sdk;

    /// <summary>Creates services for identities explicitly supported by the current host.</summary>
    public interface IExecutionServiceProvider
    {
        /// <summary>Creates a service for the current execution user.</summary>
        /// <returns>A service running as the current execution user.</returns>
        IOrganizationService GetExecutingUserService();

        /// <summary>Creates a service for the initiating user when the host supports it.</summary>
        /// <returns>A service running as the initiating user.</returns>
        IOrganizationService GetInitiatingUserService();

        /// <summary>Creates a SYSTEM service when the host supports it.</summary>
        /// <returns>A service running as SYSTEM.</returns>
        IOrganizationService GetSystemService();
    }
}
