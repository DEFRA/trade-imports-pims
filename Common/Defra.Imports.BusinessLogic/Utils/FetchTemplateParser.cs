namespace Defra.Imports.BusinessLogic.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Defra.Imports.BusinessLogic.Extensions;
    using Defra.Imports.BusinessLogic.Logging;

    /// <summary>
    /// Builds FetchXML using a template string.
    /// </summary>
    public class FetchTemplateParser : IFetchTemplateParser
    {
        private const string Tag = nameof(FetchTemplateParser);

        private readonly IOrganizationService orgSvc;
        private readonly ILogWriter logWriter;

        public FetchTemplateParser(IOrganizationService orgSvc, ILogWriter logWriter)
        {
            this.orgSvc = orgSvc ?? throw new ArgumentNullException(nameof(orgSvc));
            this.logWriter = logWriter ?? throw new ArgumentNullException(nameof(logWriter));
        }

        public string Parse(string template, EntityReference primaryEntity, IDictionary<string, object> additionalValues = null)
        {
            if (string.IsNullOrEmpty(template))
            {
                throw new ArgumentException("Template is empty.", nameof(template));
            }

            if (primaryEntity == null)
            {
                throw new ArgumentNullException(nameof(primaryEntity));
            }

            this.logWriter.Log(Severity.Info, Tag, $"Parsing FetchXML template for {primaryEntity.LogicalName} {primaryEntity.Id}.");

            var fieldPlaceholders = this.GetFieldPlaceholders(template);
            var allPlaceholders = fieldPlaceholders.Concat(this.GetAdditionalPlaceholders(template)).OrderBy(placeholder => placeholder.Index);

            var placeholderValues = this.GetFieldPlaceholderValues(primaryEntity, fieldPlaceholders);

            if (additionalValues != null)
            {
                this.logWriter.Log(Severity.Info, Tag, $"Using the following additional values for template: {string.Join(", ", additionalValues)}.");
                placeholderValues.AddRange(additionalValues);
            }

            return this.ReplaceTemplatePlaceholders(template, allPlaceholders, placeholderValues);
        }

        private static string ConvertToFetchXmlValue(object attributeValue)
        {
            object replacement;
            if (attributeValue is EntityReference)
            {
                replacement = ((EntityReference)attributeValue).Id.ToString("B", CultureInfo.CurrentCulture);
            }
            else if (attributeValue is Money)
            {
                replacement = ((Money)attributeValue).Value;
            }
            else if (attributeValue is OptionSetValue)
            {
                replacement = ((OptionSetValue)attributeValue).Value;
            }
            else if (attributeValue is Guid)
            {
                replacement = ((Guid)attributeValue).ToString("B", CultureInfo.CurrentCulture);
            }
            else
            {
                replacement = attributeValue;
            }

            return replacement?.ToString();
        }

        private string ReplaceTemplatePlaceholders(string template, IEnumerable<Match> matches, AttributeCollection values)
        {
            foreach (var placeholder in matches.Reverse())
            {
                var replacement = ConvertToFetchXmlValue(values[placeholder.Value]);
                this.logWriter.Log(Severity.Info, Tag, $"Replacing {placeholder.Value} placeholder with {replacement}.");

                template = template.ReplaceFromPosition(placeholder.Index - 2, placeholder.Length + 4, ConvertToFetchXmlValue(values[placeholder.Value]));
            }

            return template;
        }

        private IEnumerable<Match> GetFieldPlaceholders(string template)
        {
            var matches = Regex.Matches(template, @"(?<=\{\{)[^}.]*(?=\}\})").Cast<Match>();

            this.logWriter.Log(Severity.Info, Tag, $"{matches.Count()} field placeholders found in template.");

            return matches;
        }

        private IEnumerable<Match> GetAdditionalPlaceholders(string template)
        {
            var matches = Regex.Matches(template, @"(?<=\{\{)\w*\.[^}]*(?=\}\})").Cast<Match>();

            this.logWriter.Log(Severity.Info, Tag, $"{matches.Count()} additional placeholders found in template.");

            return matches;
        }

        private AttributeCollection GetFieldPlaceholderValues(EntityReference primaryEntity, IEnumerable<Match> matches)
        {
            this.logWriter.Log(Severity.Info, Tag, $"Retrieving placeholder fields from {primaryEntity.LogicalName} {primaryEntity.Id}.");

            var fields = matches.Select(match => match.Value.ToLower(CultureInfo.CurrentCulture)).ToArray();

            var result = new AttributeCollection();

            if (fields.Count() > 0)
            {
                result = this.orgSvc.Retrieve(primaryEntity.LogicalName, primaryEntity.Id, new ColumnSet(fields)).Attributes;
            }

            return result;
        }
    }
}
