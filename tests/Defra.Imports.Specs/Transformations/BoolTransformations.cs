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

        /// <summary>
        /// Transorms with or without into a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The with or without string.</param>
        /// <returns>The bool value.</returns>
        [StepArgumentTransformation("(with|without)")]
        public static bool With(string value)
        {
            return value.Equals("with", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Transforms checked or unchecked into a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The checked or unchecked string.</param>
        /// <returns>The bool value.</returns>
        [StepArgumentTransformation("(checked|unchecked)")]
        public static bool Checked(string value)
        {
            return value.Equals("checked", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Transforms Yes or No into a <see cref="bool"/> value.
        /// </summary>
        /// <param name="value">The Yes or No string.</param>
        /// <returns>The bool value.</returns>
        [StepArgumentTransformation("(Yes|No)")]
        public static bool YesNo(string value)
        {
            return value.Equals("Yes", StringComparison.OrdinalIgnoreCase);
        }
    }
}
