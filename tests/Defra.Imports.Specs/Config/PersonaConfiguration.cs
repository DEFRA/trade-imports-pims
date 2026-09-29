namespace Defra.Imports.Specs.Config
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Configuration of a persona used for testing.
    /// </summary>
    public class PersonaConfiguration
    {
        /// <summary>
        /// Gets or sets the application ID of the app registration that describes this persona (if the persona is an application).
        /// </summary>
        public Guid? AppId { get; set; }

        /// <summary>
        /// Gets or sets the interactive users to be used for this persona. If supplied, only these users will be used for authenticating as this persona during tests.
        /// </summary>
        public IEnumerable<string> Users { get; set; }

        /// <summary>
        /// Gets or sets the security roles that describe the persona.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets the business unit that describes the persona. If not specified, the persona is assumed to be mapped to the root business unit.
        /// </summary>
        public string BusinessUnit { get; set; }

        /// <summary>
        /// Gets or sets the teams that the persona is a member of.
        /// </summary>
        public IEnumerable<string> Teams { get; set; }

        /// <summary>
        /// Gets or sets the column security profiles that are assigned to the persona.
        /// </summary>
        public IEnumerable<string> ColumnSecurityProfiles { get; set; }

        /// <summary>
        /// Gets or sets aliases that describe the persona.
        /// </summary>
        public IEnumerable<string> Aliases { get; set; }

        /// <summary>
        /// Validates the persona configuration.
        /// </summary>
        /// <exception cref="Exception">Thrown if the roles are not set.</exception>
        public void Validate()
        {
            if (this.Roles is null || !this.Roles.Any())
            {
                throw new Exception("The roles have not been set for the persona configuration.");
            }
        }
    }
}
