namespace Defra.Imports.Specs.Services
{
    using System.Collections.Generic;

    /// <summary>
    /// The acceptance criteria gaps that are already known to exist in the solution.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Tolerating every gap would mean a requirement that regresses from working to missing is
    /// silently absorbed into the known defect report. Only the gaps listed here are tolerated.
    /// Anything else is an unexpected gap and fails the scenario.
    /// </para>
    /// <para>
    /// Each entry is the acceptance criterion and the requirement exactly as the step bindings
    /// record them. When a gap is fixed in the solution, its entry must be removed from this list
    /// so that it cannot silently regress.
    /// </para>
    /// </remarks>
    public static class ExpectedDefects
    {
        /// <summary>
        /// Gets the known gaps, keyed by acceptance criterion and requirement.
        /// </summary>
        public static IReadOnlyCollection<string> Keys { get; } = new HashSet<string>
        {
            // AC-1: fields named by the acceptance criterion that the form does not provide.
            Key("AC-1", "Transporter > Date of Import"),
            Key("AC-1", "Commodity > Species / Product (Common Name)"),
            Key("AC-1", "Commodity > Quantity"),
            Key("AC-1", "Commodity > Units"),
            Key("AC-1", "Commodity > Intended Use of Commodity"),
            Key("AC-1", "Transporter > Port / Airport of Entry"),
            Key("AC-1", "Importer Notification Details > Animal / Product IDs"),

            // AC-1: the caseworker role has no create privilege on the notification table.
            Key("AC-1", "Create a new Import Notification"),

            // AC-2: columns named by the acceptance criterion that the view does not show.
            Key("AC-2", "Active Importer Notifications > Date of Import"),
            Key("AC-2", "Active Importer Notifications > Premises of Origin Country"),
            Key("AC-2", "Active Importer Notifications > Species / Product (Common Name)"),
            Key("AC-2", "Active Importer Notifications > Importer Name"),
            Key("AC-2", "Active Importer Notifications > Importer Telephone"),
            Key("AC-2", "Active Importer Notifications > Importer Email"),
            Key("AC-2", "Active Importer Notifications > Port / Airport of Entry"),

            // AC-3: the quick find view searches the record name and state only.
            Key("AC-3", "Free text search by Importer Name"),
            Key("AC-3", "Free text search by Charity Name"),
            Key("AC-3", "Free text search by Premises of Origin Name"),
            Key("AC-3", "Free text search by Permanent Destination Name"),
            Key("AC-3", "Free text search by Animal / Product ID"),
        };

        /// <summary>
        /// Builds the key identifying a gap.
        /// </summary>
        /// <param name="acceptanceCriterion">The acceptance criterion.</param>
        /// <param name="requirement">The requirement.</param>
        /// <returns>The key.</returns>
        public static string Key(string acceptanceCriterion, string requirement)
        {
            return $"{acceptanceCriterion}|{requirement}";
        }
    }
}
