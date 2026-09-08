namespace Defra.Imports.Specs.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Config;

    /// <summary>
    /// Applies and removes persona configuration (business unit, security roles, teams and column security profiles) on Dataverse users.
    /// </summary>
    internal interface IPersonaConfigurationApplier
    {
        /// <summary>
        /// Applies the combined configuration of the given personas to the specified user.
        /// </summary>
        /// <param name="username">The domain name of the user to configure.</param>
        /// <param name="personas">The persona configurations to apply.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task ApplyAsync(string username, IEnumerable<PersonaConfiguration> personas);

        /// <summary>
        /// Resets the specified user to a clean state: the environment's default business unit, with no security roles, team memberships or column security profiles.
        /// </summary>
        /// <param name="username">The domain name of the user to reset.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task RemoveAsync(string username);
    }
}
