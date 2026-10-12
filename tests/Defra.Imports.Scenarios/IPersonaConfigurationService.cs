namespace Defra.Imports.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios.Config;

    /// <summary>
    /// Applies, removes and verifies persona configuration (business unit, security roles, teams and column security profiles) on Dataverse users.
    /// </summary>
    public interface IPersonaConfigurationService : IDisposable
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

        /// <summary>
        /// Verifies that every statically assigned user described by <paramref name="personaConfigurations"/> (those explicitly listed under a persona's <see cref="PersonaConfiguration.Users"/>) actually has the full Dataverse security configuration described by every persona they are assigned to, collecting every discrepancy before reporting them together.
        /// </summary>
        /// <param name="personaConfigurations">The configuration for every known persona.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if one or more statically assigned users do not have the full configuration described by their assigned persona(s).</exception>
        Task VerifyStaticAssignmentsAsync(IDictionary<Persona, PersonaConfiguration> personaConfigurations);
    }
}
