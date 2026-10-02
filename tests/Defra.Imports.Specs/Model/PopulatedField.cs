namespace Defra.Imports.Specs.Model
{
    /// <summary>
    /// A field value that was successfully entered on a form during a scenario.
    /// </summary>
    /// <remarks>
    /// Captured so that the values can be read back after the record is saved and reloaded, proving
    /// that the field persists what was entered rather than only accepting it.
    /// </remarks>
    public class PopulatedField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopulatedField"/> class.
        /// </summary>
        /// <param name="tabName">The tab the field appears on, which may be empty.</param>
        /// <param name="fieldName">The field display name.</param>
        /// <param name="value">The value that was entered.</param>
        public PopulatedField(string tabName, string fieldName, string value)
        {
            this.TabName = tabName;
            this.FieldName = fieldName;
            this.Value = value;
        }

        /// <summary>
        /// Gets the tab the field appears on.
        /// </summary>
        public string TabName { get; }

        /// <summary>
        /// Gets the field display name.
        /// </summary>
        public string FieldName { get; }

        /// <summary>
        /// Gets the value that was entered.
        /// </summary>
        public string Value { get; }
    }
}
