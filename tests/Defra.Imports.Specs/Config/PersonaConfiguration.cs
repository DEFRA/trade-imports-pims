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
        /// Gets or sets the interactive users to be used for this persona.
        /// </summary>
        public IEnumerable<string> Users { get; set; }

        /// <summary>
        /// Gets or sets the security roles that describe the persona.
        /// </summary>
        public IEnumerable<string> Roles { get; set; }

        /// <summary>
        /// Gets or sets the business unit that describes the perosna.
        /// </summary>
        public string BusinessUnit { get; set; }

        /// <summary>
        /// Gets or sets aliases that describe the persona.
        /// </summary>
        public IEnumerable<string> Aliases { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to apply the persona configuration to users at test runtime. Defaults to false.
        /// </summary>
        public bool ApplyToUsers { get; set; }

        /// <summary>
        /// Validates the persona configuration.
        /// </summary>
        /// <exception cref="Exception">Thrown if the business unit, roles, and app ID or users are not set.</exception>
        public void Validate()
        {
            if (string.IsNullOrEmpty(this.BusinessUnit))
            {
                throw new Exception("The business unit has not been set for the persona configuration.");
            }

            if (this.Roles is null || !this.Roles.Any())
            {
                throw new Exception("The roles have not been set for the persona configuration.");
            }

            if (!this.AppId.HasValue && (this.Users is null || !this.Users.Any()))
            {
                throw new Exception("Either an app ID or users must be configured for the persona.");
            }
        }
    }
}
