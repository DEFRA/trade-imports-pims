namespace Defra.Imports.IntegrationTests.Config
{
    using System;
    using System.Collections.Generic;

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
    }
}
