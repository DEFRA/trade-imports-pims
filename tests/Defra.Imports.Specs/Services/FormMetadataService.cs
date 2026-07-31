namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Linq;
    using Defra.Imports.Specs.Model;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using PowerPlaywright.Framework.Controls.Pcf;
    using PowerPlaywright.Framework.Controls.Pcf.Attributes;
    using PowerPlaywright.Framework.Model;

    /// <summary>
    /// Provides access to form metadata for the application.
    /// </summary>
    public class FormMetadataService
    {
        private const string QuickViewClassId = "{5C5600E0-1D6E-4205-A272-BE80DA87FD42}";

        private readonly ServiceClient appUserClient;
        private readonly ConcurrentDictionary<Guid, Entity> formCache;
        private readonly ConcurrentDictionary<Guid, XmlDocument> formXmlCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormMetadataService"/> class.
        /// </summary>
        /// <param name="appUserClient">The app user client.</param>
        public FormMetadataService(ServiceClient appUserClient)
        {
            this.appUserClient = appUserClient;
            this.formCache = new ConcurrentDictionary<Guid, Entity>();
            this.formXmlCache = new ConcurrentDictionary<Guid, XmlDocument>();
        }

        /// <summary>
        /// Gets the column name of a control using its display name from the form XML and any quick view form XMLs. Display name is case-sensitive.
        /// </summary>
        /// <param name="formId">The form ID.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="fieldFormId">The field's form ID.</param>
        /// <param name="tab">The tab.</param>
        /// <returns>The field's column name.</returns>
        /// <exception cref="InvalidOperationException">Thrown if field can't be found.</exception>
        public string GetControlColumnName(Guid formId, string displayName, out Guid fieldFormId, string tab = null)
        {
            fieldFormId = formId;

            try
            {
                return this.GetControlColumnName(formId, displayName, tab);
            }
            catch (InvalidOperationException)
            {
                // Swallow
            }

            string columnName = null;
            foreach (var quickView in this.GetQuickViewControlFormIds(formId))
            {
                foreach (var quickViewFormId in quickView.Value)
                {
                    try
                    {
                        columnName = this.GetControlColumnName(quickViewFormId, displayName);
                        fieldFormId = quickViewFormId;
                        break;
                    }
                    catch (InvalidOperationException)
                    {
                        // Swallow
                    }
                }
            }

            return columnName ?? throw new InvalidOperationException($"Unable to find a control on the form with the specified display name: {displayName}.");
        }

        /// <summary>
        /// Gets the logical name of a control using its display name from the form XML and any quick view form XMLs. Display name is case-sensitive.
        /// </summary>
        /// <param name="formId">The form ID.</param>
        /// <param name="displayName">The control display name.</param>
        /// <param name="fieldFormId">The form ID of the form that contains the control.</param>
        /// <param name="tab">An optional tab to scope the search by.</param>
        /// <returns>The control logical name.</returns>
        public string GetControlLogicalName(Guid formId, string displayName, out Guid fieldFormId, string tab = null)
        {
            fieldFormId = formId;

            try
            {
                return this.GetControlLogicalName(formId, displayName, tab);
            }
            catch (InvalidOperationException)
            {
                // Swallow
            }

            string logicalName = null;
            foreach (var quickView in this.GetQuickViewControlFormIds(formId))
            {
                foreach (var quickViewFormId in quickView.Value)
                {
                    try
                    {
                        logicalName = $"{quickView.Key}.{this.GetControlLogicalName(quickViewFormId, displayName)}";
                        fieldFormId = quickViewFormId;
                        break;
                    }
                    catch (InvalidOperationException)
                    {
                        // Swallow
                    }
                }

                if (logicalName != null)
                {
                    break;
                }
            }

            return logicalName ?? throw new InvalidOperationException($"Unable to find a control on the form with the specified display name: {displayName}.");
        }

        /// <summary>
        /// Gets whether the control is a header or standard field.
        /// </summary>
        /// <param name="formId">The form ID.</param>
        /// <param name="displayName">The control display name.</param>
        /// <returns>The field location.</returns>
        public FieldLocation GetControlLocation(Guid formId, string displayName)
        {
            var logicalName = this.GetControlLogicalName(formId, displayName);

            return logicalName.StartsWith("header_") ? FieldLocation.Header : FieldLocation.Body;
        }

        /// <summary>
        /// Gets the logical name of the table the form is for.
        /// </summary>
        /// <param name="formId">The ID of the form.</param>
        /// <returns>The logical name of the table.</returns>
        public string GetTableLogicalNameByFormId(Guid formId)
        {
            var form = this.GetForm(formId);

            return form.GetAttributeValue<string>("objecttypecode");
        }

        /// <summary>
        /// Gets the custom control type name.
        /// </summary>
        /// <param name="formId">The ID of the form.</param>
        /// <param name="logicalName">The logical name of the control.</param>
        /// <param name="formFactor">The form factor.</param>
        /// <returns>The custom control type name; otherwise null.</returns>
        public string GetCustomControlTypeName(Guid formId, string logicalName, FormFactor formFactor = FormFactor.Web)
        {
            var formXml = this.GetFormXml(formId);
            var document = XDocument.Parse(formXml.OuterXml);

            var fieldControl = document
                .Descendants("control")
                .FirstOrDefault(c => this.IsMatchingControl(c, logicalName));

            if (fieldControl == null)
            {
                return null;
            }

            var uniqueId = fieldControl.Attribute("uniqueid")?.Value;
            if (string.IsNullOrEmpty(uniqueId))
            {
                return null;
            }

            var controlDescription = document
                .Descendants("controlDescription")
                .FirstOrDefault(cd => cd.Attribute("forControl")?.Value == uniqueId);

            if (controlDescription == null)
            {
                var isGridControl = fieldControl.Attribute("classid")?.Value == "{E7A81278-8635-4d9e-8D4D-59480B391C5B}";
                var targetEntityType = fieldControl.Descendants("TargetEntityType").FirstOrDefault()?.Value;

                if (isGridControl && targetEntityType == "connection")
                {
                    return typeof(IGridControl).GetCustomAttribute<PcfControlAttribute>().Name;
                }

                return null;
            }

            return controlDescription
                .Descendants("customControl")
                .Where(x => x.Attribute("formFactor")?.Value == $"{(int)formFactor}")
                .Select(x => x.Attribute("name").Value)
                .FirstOrDefault(x => !string.IsNullOrWhiteSpace(x));
        }

        private static XmlNodeList GetControlNodesByDisplayName(string displayName, string tab, XmlDocument formXml)
        {
            var tabSegment = !string.IsNullOrEmpty(tab) ? $"//tab[./labels/label[@description='{tab}']]" : string.Empty;
            var controlsWithDisplayName = formXml.SelectNodes($"{tabSegment}//control[../labels/label[@description='{displayName}'] and not(@classid='{QuickViewClassId}')]");

            if (controlsWithDisplayName == null || controlsWithDisplayName.Count == 0)
            {
                controlsWithDisplayName = formXml.SelectNodes($"//header//control[../labels/label[@description='{displayName}'] and not(@classid='{QuickViewClassId}')]");
            }

            return controlsWithDisplayName != null && controlsWithDisplayName.Count > 0
                ? controlsWithDisplayName
                : throw new InvalidOperationException($"Unable to find a control on the form with the specified display name: {displayName}.");
        }

        /// <summary>
        /// Gets the column name of a control using its display name from the form XML. Display name is case-sensitive.
        /// </summary>
        /// <param name="formId">The form ID.</param>
        /// <param name="displayName">The control display name.</param>
        /// <param name="tab">An optional tab to scope the search by.</param>
        /// <returns>The control logical name.</returns>
        private string GetControlColumnName(Guid formId, string displayName, string tab = null)
        {
            return GetControlNodesByDisplayName(displayName, tab, this.GetFormXml(formId))[0]
                .Attributes?["datafieldname"].Value;
        }

        /// <summary>
        /// Gets the logical name of a control using its display name from the form XML. Display name is case-sensitive.
        /// </summary>
        /// <param name="formId">The form ID.</param>
        /// <param name="displayName">The control display name.</param>
        /// <param name="tab">An optional tab to scope the search by.</param>
        /// <returns>The control logical name.</returns>
        private string GetControlLogicalName(Guid formId, string displayName, string tab = null)
        {
            var formXml = this.GetFormXml(formId);

            var firstControlWithDisplayName = GetControlNodesByDisplayName(displayName, tab, this.GetFormXml(formId))[0];
            var logicalName = firstControlWithDisplayName.Attributes?["id"].Value;

            var allControlsWithLogicalName = formXml.SelectNodes($"//control[@id='{logicalName}']");
            if (allControlsWithLogicalName == null || allControlsWithLogicalName.Count <= 1)
            {
                return logicalName;
            }

            int position = -1;
            for (int i = 0; i < allControlsWithLogicalName.Count; i++)
            {
                if (ReferenceEquals(allControlsWithLogicalName[i], firstControlWithDisplayName) || allControlsWithLogicalName[i].OuterXml == firstControlWithDisplayName.OuterXml)
                {
                    position = i + 1;
                    break;
                }
            }

            if (position == -1 || position == 1)
            {
                return logicalName;
            }

            return $"{logicalName}{position - 1}";
        }

        private IDictionary<string, IEnumerable<Guid>> GetQuickViewControlFormIds(Guid formId)
        {
            var quickViewControls = this
                .GetFormXml(formId)
                .SelectNodes($"//control[@classid='{QuickViewClassId}']");

            var quickViewIds = new Dictionary<string, IEnumerable<Guid>>();
            foreach (XmlNode control in quickViewControls)
            {
                var quickFormsNode = control.SelectSingleNode("./parameters/QuickForms");
                if (quickFormsNode == null)
                {
                    continue;
                }

                var quickFormsDoc = new XmlDocument();
                quickFormsDoc.LoadXml(WebUtility.HtmlDecode(quickFormsNode.InnerText));
                var quickFormIdNodes = quickFormsDoc.SelectNodes("//QuickFormId");

                var ids = new List<Guid>();
                foreach (XmlNode quickFormIdNode in quickFormIdNodes)
                {
                    if (Guid.TryParse(quickFormIdNode.InnerText, out var quickFormId))
                    {
                        ids.Add(quickFormId);
                    }
                }

                quickViewIds.Add(control.Attributes["id"].Value, ids);
            }

            return quickViewIds;
        }

        private Entity GetForm(Guid formId)
        {
            if (!this.formCache.ContainsKey(formId))
            {
                var form = this.appUserClient
                    .Retrieve("systemform", formId, new ColumnSet("objecttypecode"));

                if (form is null)
                {
                    throw new InvalidOperationException($"Unable to find a form with ID: {formId}.");
                }

                this.formCache.TryAdd(formId, form);
            }

            return this.formCache[formId];
        }

        private XmlDocument GetFormXml(Guid formId)
        {
            if (!this.formXmlCache.ContainsKey(formId))
            {
                var formXml = this.appUserClient
                    .Retrieve("systemform", formId, new ColumnSet("formxml"))
                    .GetAttributeValue<string>("formxml");

                var formXmlDoc = new XmlDocument();
                formXmlDoc.LoadXml(formXml);

                this.formXmlCache.TryAdd(formId, formXmlDoc);
            }

            return this.formXmlCache[formId];
        }

        private bool IsMatchingControl(XElement control, string logicalName)
        {
            var dataFieldName = control.Attribute("datafieldname")?.Value;
            var id = control.Attribute("id")?.Value;
            return dataFieldName == logicalName || id == logicalName;
        }
    }
}