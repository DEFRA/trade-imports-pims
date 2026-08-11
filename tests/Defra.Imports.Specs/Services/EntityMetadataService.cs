namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.PowerPlatform.Dataverse.Client.Extensions;
    using Microsoft.Xrm.Sdk.Metadata;
    using NuGet.Packaging;

    /// <summary>
    /// Provides access to entity metadata for the application.
    /// </summary>
    public class EntityMetadataService
    {
        private readonly ServiceClient appUserClient;

        private readonly ConcurrentDictionary<string, EntityMetadata> entityMetadata;
        private readonly ConcurrentDictionary<string, List<AttributeMetadata>> attributesMetadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityMetadataService"/> class.
        /// </summary>
        /// <param name="appUserClient">The app user client.</param>
        public EntityMetadataService(ServiceClient appUserClient)
        {
            this.appUserClient = appUserClient;
            this.entityMetadata = new ConcurrentDictionary<string, EntityMetadata>();
            this.attributesMetadata = new ConcurrentDictionary<string, List<AttributeMetadata>>();
        }

        /// <summary>
        /// Gets all entity metadata for the application.
        /// </summary>
        private IDictionary<string, EntityMetadata> EntityMetadata
        {
            get
            {
                if (!this.entityMetadata.Any())
                {
                    this.entityMetadata.AddRange(this.appUserClient.GetAllEntityMetadata().ToDictionary(e => e.LogicalName, e => e));
                }

                return this.entityMetadata;
            }
        }

        /// <summary>
        /// Gets the logical name for a table using its display name.
        /// </summary>
        /// <param name="displayName">The display name of the table.</param>
        /// <returns>The logical name.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the display name does not match any table.</exception>
        public string GetTableLogicalName(string displayName)
        {
            var logicalName = this.EntityMetadata.Values
                .FirstOrDefault(e => e?.DisplayName?.UserLocalizedLabel?.Label.Equals(displayName, StringComparison.OrdinalIgnoreCase) ?? false)
                ?.LogicalName;

            return logicalName is null
                ? throw new KeyNotFoundException($"A table with a display name of {displayName} could not be found.")
                : logicalName;
        }

        /// <summary>
        /// Gets the metadata for a table.
        /// </summary>
        /// <param name="tableName">The logical name of the table.</param>
        /// <returns>The table metadata.</returns>
        public EntityMetadata GetTableMetadata(string tableName)
        {
            if (this.EntityMetadata.TryGetValue(tableName, out var metadata))
            {
                return metadata;
            }

            throw new KeyNotFoundException($"A table with a logical name of {tableName} could not be found.");
        }

        /// <summary>
        /// Gets the attributes metadata for a table.
        /// </summary>
        /// <param name="tableName">The logical name of the table.</param>
        /// <returns>The table metadata.</returns>
        public List<AttributeMetadata> GetTableAttributesMetadata(string tableName)
        {
            if (!this.attributesMetadata.ContainsKey(tableName))
            {
                this.attributesMetadata.TryAdd(tableName, this.appUserClient.GetAllAttributesForEntity(tableName));
            }

            return this.attributesMetadata[tableName];
        }

        /// <summary>
        /// Gets the optionset label for a given optionset value.
        /// </summary>
        /// <param name="tableName">The logical name of the table.</param>
        /// <param name="columnName">The logical name of the optionset field.</param>
        /// <param name="optionValue">The optionset value (int).</param>
        /// <returns>The option set label.</returns>
        public string GetOptionSetLabel(string tableName, string columnName, int optionValue)
        {
            var attributeMetadata = (EnumAttributeMetadata)this.GetTableAttributesMetadata(tableName).Find(a => a.LogicalName == columnName) ?? throw new KeyNotFoundException($"A column with a logical name of {columnName} for the table with a logical name of {tableName} could not be found.");
            var optionLabel = (from o in attributeMetadata.OptionSet.Options
                               where o.Value == optionValue
                               select o.Label.UserLocalizedLabel.Label)?.FirstOrDefault();
            if (string.IsNullOrEmpty(optionLabel))
            {
                throw new KeyNotFoundException($"A label for the optionset {columnName} with a value of {optionValue} could not be found.");
            }

            return optionLabel;
        }

        /// <summary>
        /// Gets the optionset labels for a given optionset field.
        /// </summary>
        /// <param name="tableName">The logical name of the table.</param>
        /// <param name="columnName">The logical name of the optionset field.</param>
        /// <returns>Collection of option labels.</returns>
        public IEnumerable<string> GetColumnOptions(string tableName, string columnName)
        {
            var attributeMetadata = (EnumAttributeMetadata)this.GetTableAttributesMetadata(tableName).Find(a => a.LogicalName == columnName) ?? throw new KeyNotFoundException($"A column with a logical name of {columnName} for the table with a logical name of {tableName} could not be found.");

            var options = (from o in attributeMetadata.OptionSet.Options
                           select o.Label.UserLocalizedLabel.Label).ToList();

            return options;
        }
    }
}
