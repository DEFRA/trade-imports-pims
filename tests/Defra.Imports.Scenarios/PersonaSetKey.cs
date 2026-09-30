namespace Defra.Imports.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Builds a stable, order-independent key identifying a set of personas, for use by caches keyed on persona combinations.
    /// </summary>
    internal static class PersonaSetKey
    {
        /// <summary>
        /// Creates a key for the given set of personas.
        /// </summary>
        /// <param name="personas">The personas.</param>
        /// <returns>The key.</returns>
        internal static string Create(IEnumerable<Persona> personas)
        {
            return string.Join(",", personas.Select(p => p.ToString()).OrderBy(p => p, StringComparer.Ordinal));
        }
    }
}
