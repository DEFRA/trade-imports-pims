namespace Defra.Imports.Dataverse.Contexts
{
    using Defra.Imports.Dataverse.Model;

    /// <summary>Creates caller-owned, invocation-local imports contexts.</summary>
    public interface IImportsContextFactory
    {
        /// <summary>Creates a context for the executing user.</summary>
        /// <returns>A caller-owned context bound to the executing user's service.</returns>
        ImportsContext CreateForExecutingUser();

        /// <summary>Creates a context for the initiating user when supported by the host.</summary>
        /// <returns>A caller-owned context bound to the initiating user's service.</returns>
        ImportsContext CreateForInitiatingUser();

        /// <summary>Creates a SYSTEM context when supported by the host.</summary>
        /// <returns>A caller-owned context bound to the SYSTEM service.</returns>
        ImportsContext CreateForSystem();
    }
}
