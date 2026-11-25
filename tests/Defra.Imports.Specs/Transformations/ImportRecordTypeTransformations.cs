namespace Defra.Imports.Specs.Transformations
{
    using System;
    using System.Text.RegularExpressions;
    using Defra.Imports.Model;
    using Reqnroll;

    /// <summary>
    /// Transformations for Import Record Types.
    /// </summary>
    [Binding]
    public class ImportRecordTypeTransformations
    {
        /// <summary>
        /// A transformation to convert an Import Record Type label to a <see cref="defraimp_importapplication_defraimp_importapplicationtype"/>.
        /// </summary>
        /// <param name="importRecordType">The Import Record Type label.</param>
        /// <returns>The Import Record Type.</returns>
        [StepArgumentTransformation("(ITAHC - Landbridge|ITAHC|CHED-A|CHEDP)")]
        public defraimp_importapplication_defraimp_importapplicationtype ImportRecordTypeTransform(string importRecordType)
        {
            return (defraimp_importapplication_defraimp_importapplicationtype)Enum.Parse(typeof(defraimp_importapplication_defraimp_importapplicationtype), Regex.Replace(importRecordType ?? string.Empty, @"[^A-Za-z0-9]", string.Empty), true);
        }
    }
}
