namespace Defra.Imports.Specs.Transformations
{
    using System;
    using System.Text.RegularExpressions;
    using Defra.Imports.Model;
    using Reqnroll;

    /// <summary>
    /// Transformations for purposes of consignment.
    /// </summary>
    [Binding]
    public class PurposeOfConsignmentTransformations
    {
        /// <summary>
        /// A transformation to convert a purpose of consignment label to a <see cref="defraimp_purposeofconsignment"/>.
        /// </summary>
        /// <param name="purposeOfConsignment">The purpose of consignment label.</param>
        /// <returns>The purpose of consignment.</returns>
        [StepArgumentTransformation("(Internal Market|Re-Entry|Temporary admission horses|Transhipment or onward travel|Transit|For Re-Import)")]
        public defraimp_purposeofconsignment PurposeOfConsignmentTransform(string purposeOfConsignment)
        {
            return (defraimp_purposeofconsignment)Enum.Parse(typeof(defraimp_purposeofconsignment), Regex.Replace(purposeOfConsignment ?? string.Empty, @"[^A-Za-z0-9]", string.Empty), true);
        }
    }
}
