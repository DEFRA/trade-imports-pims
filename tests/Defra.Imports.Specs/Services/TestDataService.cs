namespace Defra.Imports.Specs.Services
{
    using System.Collections.Generic;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// A service for managing test data.
    /// </summary>
    public class TestDataService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestDataService"/> class.
        /// </summary>
        public TestDataService()
        {
            this.History = new Stack<EntityReference>();
            this.AliasMap = new Dictionary<string, EntityReference>();
        }

        /// <summary>
        /// Gets the created record history.
        /// </summary>
        public Stack<EntityReference> History { get; private set; }

        private IDictionary<string, EntityReference> AliasMap { get; set; }

        /// <summary>
        /// Adds a record to the history with optional aliases.
        /// </summary>
        /// <param name="reference">The record.</param>
        /// <param name="aliases">The aliases.</param>
        public void AddRecord(EntityReference reference, params string[] aliases)
        {
            this.History.Push(reference);

            foreach (var alias in aliases)
            {
                if (this.AliasMap.ContainsKey(alias))
                {
                    this.AliasMap[alias] = reference;
                }
                else
                {
                    this.AliasMap.Add(alias, reference);
                }
            }
        }

        /// <summary>
        /// Gets a record by alias.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <returns>The record.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no record is found for the alias.</exception>
        public EntityReference GetRecordByAlias(string alias)
        {
            if (!this.AliasMap.ContainsKey(alias))
            {
                throw new KeyNotFoundException($"No entity reference found in the history for alias '{alias}'.");
            }

            return this.AliasMap[alias];
        }
    }
}
