namespace Defra.Imports.Specs.Transformations
{
    using System;
    using Reqnroll;

    /// <summary>
    /// Transformations relating to the <see cref="bool"/> type.
    /// </summary>
    [Binding]
    public class BoolTransformations
    {
        /// <summary>
        /// Transorms can or cannot into a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The can or cannot string.</param>
        /// <returns>The bool value.</returns>
        [StepArgumentTransformation("(can|cannot)")]
        public static bool Can(string value)
        {
            if (value.Equals("can", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Transorms is or is not into a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The is or is not string.</param>
        /// <returns>The bool value.</returns>
        [StepArgumentTransformation("(is|is not)")]
        public static bool Is(string value)
        {
            if (value.Equals("is", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Transorms do not into a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The do not string.</param>
        /// <returns>The bool value.</returns>
        [StepArgumentTransformation("(| do not)")]
        public static bool Do(string value)
        {
            if (value == string.Empty)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Transorms do not into a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The do not string.</param>
        /// <returns>The bool value.</returns>
        [StepArgumentTransformation("(are|are not)")]
        public static bool Are(string value)
        {
            if (value == "are")
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Transorms select or deselect into a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The select or deselect string.</param>
        /// <returns>The bool value.</returns>
        [StepArgumentTransformation("(select|deselect)")]
        public static bool Select(string value)
        {
            if (value == "select")
            {
                return true;
            }

            return false;
        }
    }
}
