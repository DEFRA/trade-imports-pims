namespace Defra.Imports.Specs.Transformations
{
    using System;
    using System.Text.RegularExpressions;
    using PowerPlaywright.Framework.Model;
    using Reqnroll;

    /// <summary>
    /// Transformations for field requirement levels.
    /// </summary>
    [Binding]
    public class FieldRequirementLevelTransformations
    {
        /// <summary>
        /// A transformation to convert a string to a requirement level.
        /// </summary>
        /// <param name="requirementLevel">The string.</param>
        /// <returns>The requirement level.</returns>
        [StepArgumentTransformation("(required|recommended|optional)")]
        public FieldRequirementLevel ApplicationTypeTransform(string requirementLevel)
        {
            if (requirementLevel == "optional")
            {
                return FieldRequirementLevel.None;
            }

            return (FieldRequirementLevel)Enum.Parse(typeof(FieldRequirementLevel), Regex.Replace(requirementLevel ?? string.Empty, @"[^A-Za-z0-9]", string.Empty), true);
        }
    }
}