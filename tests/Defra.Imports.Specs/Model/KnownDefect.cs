namespace Defra.Imports.Specs.Model
{
    /// <summary>
    /// A gap between an acceptance criterion and the behaviour currently implemented by the system.
    /// </summary>
    /// <remarks>
    /// A known defect is not a test failure. It records that the system does not yet provide
    /// something an acceptance criterion asks for, so that the remainder of the criterion can still
    /// be exercised and reported on.
    /// </remarks>
    public class KnownDefect
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KnownDefect"/> class.
        /// </summary>
        /// <param name="acceptanceCriterion">The acceptance criterion that is not met.</param>
        /// <param name="requirement">The specific field, view or search criterion that was expected.</param>
        /// <param name="expected">The behaviour required by the acceptance criterion.</param>
        /// <param name="actual">The behaviour observed in the system.</param>
        public KnownDefect(string acceptanceCriterion, string requirement, string expected, string actual)
        {
            this.AcceptanceCriterion = acceptanceCriterion;
            this.Requirement = requirement;
            this.Expected = expected;
            this.Actual = actual;
        }

        /// <summary>
        /// Gets the acceptance criterion that is not met.
        /// </summary>
        public string AcceptanceCriterion { get; }

        /// <summary>
        /// Gets the specific field, view or search criterion that was expected.
        /// </summary>
        public string Requirement { get; }

        /// <summary>
        /// Gets the behaviour required by the acceptance criterion.
        /// </summary>
        public string Expected { get; }

        /// <summary>
        /// Gets the behaviour observed in the system.
        /// </summary>
        public string Actual { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"[{this.AcceptanceCriterion}] {this.Requirement} | Expected: {this.Expected} | Actual: {this.Actual}";
        }
    }
}
