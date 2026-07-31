namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Xrm.Sdk.Metadata;
    using PowerPlaywright.Framework.Controls.Pcf.Attributes;
    using PowerPlaywright.Framework.Controls.Pcf.Classes;

    /// <summary>
    /// Provides access to form metadata for the application.
    /// </summary>
    public class PowerPlaywrightMetadataService
    {
        private readonly FormMetadataService formMetadataSvc;
        private readonly EntityMetadataService entityMetadataSvc;

        private Dictionary<string, Type> customControlTypes;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerPlaywrightMetadataService"/> class.
        /// </summary>
        /// <param name="formMetadataSvc">The form metadata service.</param>
        /// <param name="entityMetadataSvc">The entity metadata service.</param>
        public PowerPlaywrightMetadataService(FormMetadataService formMetadataSvc, EntityMetadataService entityMetadataSvc)
        {
            this.formMetadataSvc = formMetadataSvc;
            this.entityMetadataSvc = entityMetadataSvc;
        }

        private Dictionary<string, Type> CustomControlTypes
        {
            get
            {
                if (this.customControlTypes == null)
                {
                    this.customControlTypes = this.LoadControlTypes("PowerPlaywright.Framework.dll", "Defra.Trade.Plants.PageObjects.dll");
                }

                return this.customControlTypes;
            }
        }

        /// <summary>
        /// Gets the Power Playwright control class for a column.
        /// </summary>
        /// <param name="tableName">The table logical name.</param>
        /// <param name="columnName">The column logical name.</param>
        /// <returns>The control type.</returns>
        public Type GetPowerPlaywrightControlClass(string tableName, string columnName)
        {
            var columns = this.entityMetadataSvc.GetTableAttributesMetadata(tableName);

            var columnMetadata = columns.FirstOrDefault(a => a.LogicalName == columnName);

            if (columnMetadata is BooleanAttributeMetadata)
            {
                return typeof(IYesNo);
            }
            else if (columnMetadata is DateTimeAttributeMetadata dateTimeMetadata)
            {
                switch (dateTimeMetadata.Format)
                {
                    case DateTimeFormat.DateOnly:
                        return typeof(IDate);
                    case DateTimeFormat.DateAndTime:
                        return typeof(IDateTime);
                }
            }
            else if (columnMetadata is DecimalAttributeMetadata)
            {
                return typeof(IDecimalNumber);
            }
            else if (columnMetadata is DoubleAttributeMetadata)
            {
                return typeof(IFloatingPointNumber);
            }
            else if (columnMetadata is IntegerAttributeMetadata integerMetadata)
            {
                switch (integerMetadata.Format)
                {
                    case IntegerFormat.None:
                        return typeof(IWholeNumber);
                    case IntegerFormat.Duration:
                        return typeof(IDuration);
                }
            }
            else if (columnMetadata is LookupAttributeMetadata)
            {
                return typeof(ILookup);
            }
            else if (columnMetadata is StateAttributeMetadata || columnMetadata is StatusAttributeMetadata || columnMetadata is PicklistAttributeMetadata)
            {
                return typeof(IChoice);
            }
            else if (columnMetadata is StringAttributeMetadata stringMetadata)
            {
                if (stringMetadata.FormatName == StringFormatName.Text)
                {
                    return typeof(ISingleLineText);
                }
                else if (stringMetadata.FormatName == StringFormatName.Email)
                {
                    return typeof(ISingleLineEmail);
                }
                else if (stringMetadata.FormatName == StringFormatName.Phone)
                {
                    return typeof(ISingleLinePhoneNumber);
                }
                else if (stringMetadata.FormatName == StringFormatName.TextArea)
                {
                    return typeof(ISingleLineTextArea);
                }
                else if (stringMetadata.FormatName == StringFormatName.TickerSymbol)
                {
                    return typeof(ISingleLineTickerSymbol);
                }
                else if (stringMetadata.FormatName == StringFormatName.Url)
                {
                    return typeof(ISingleLineUrl);
                }
                else
                {
                    throw new NotSupportedException($"Unrecognised string format: {stringMetadata.FormatName}");
                }
            }
            else if (columnMetadata is MultiSelectPicklistAttributeMetadata)
            {
                return typeof(IChoices);
            }
            else if (columnMetadata is MoneyAttributeMetadata)
            {
                return typeof(ICurrency);
            }
            else if (columnMetadata is MemoAttributeMetadata memoMetadata)
            {
                return typeof(IMultiLineText);
            }

            throw new NotImplementedException($"Unable to determine control class for column type of {columnMetadata.AttributeTypeName?.Value}.");
        }

        /// <summary>
        /// Gets the Power Playwright control class for a control.
        /// </summary>
        /// <param name="formId">The form ID.</param>
        /// <param name="displayName">The control display name.</param>
        /// <returns>The control type.</returns>
        public Type GetPowerPlaywrightControlClass(Guid formId, string displayName)
        {
            return this.GetPowerPlaywrightControlClass(
                this.formMetadataSvc.GetTableLogicalNameByFormId(formId),
                this.formMetadataSvc.GetControlLogicalName(formId, displayName, out _));
        }

        /// <summary>
        /// Gets the Power Playwright dataset class for a control.
        /// </summary>
        /// <param name="formId">The form ID.</param>
        /// <param name="displayName">The control display name.</param>
        /// <returns>The dataset type.</returns>
        public Type GetPowerPlaywrightDataSetClass(Guid formId, string displayName)
        {
            var customControlName = this.formMetadataSvc.GetCustomControlTypeName(formId, displayName);

            if (string.IsNullOrEmpty(customControlName))
            {
                return typeof(IReadOnlyGrid);
            }

            return this.FindControlType(customControlName);
        }

        /// <summary>
        /// Gets the Power Playwright control interface for a control.
        /// </summary>
        /// <param name="formId">The form ID.</param>
        /// <param name="columnName">The column name.</param>
        /// <returns>The control type.</returns>
        public Type GetPowerPlaywrightControlInterface(Guid formId, string columnName)
        {
            var customControlName = this.formMetadataSvc.GetCustomControlTypeName(formId, columnName);

            if (string.IsNullOrEmpty(customControlName))
            {
                return this.GetPowerPlaywrightControlClass(
                    this.formMetadataSvc.GetTableLogicalNameByFormId(formId),
                    columnName);
            }

            return this.FindControlType(customControlName);
        }

        private Type FindControlType(string controlTypeName)
        {
            if (string.IsNullOrEmpty(controlTypeName))
            {
                throw new ArgumentNullException(nameof(controlTypeName), $"A control type name must be provided");
            }

            this.CustomControlTypes.TryGetValue(controlTypeName, out var type);
            return type;
        }

        private Dictionary<string, Type> LoadControlTypes(params string[] assemblyFiles)
        {
            var dir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            return assemblyFiles
                .SelectMany(assemblyFile =>
                {
                    var assembly = Assembly.LoadFrom(Path.Combine(dir, assemblyFile));
                    return assembly
                        .GetTypes()
                        .SelectMany(t => t.GetCustomAttributes<PcfControlAttribute>()
                            .Select(attr => new { attr.Name, Type = t }));
                })
                .GroupBy(x => x.Name)
                .ToDictionary(g => g.Key, g => g.First().Type);
        }
    }
}