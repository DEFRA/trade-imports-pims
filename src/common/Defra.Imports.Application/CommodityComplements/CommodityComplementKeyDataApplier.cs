namespace Defra.Imports.Application.CommodityComplements
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.Logging;
    using Microsoft.Xrm.Sdk;

    /// <inheritdoc/>
    public sealed class CommodityComplementKeyDataApplier : ICommodityComplementKeyDataApplier
    {
        // The keys that are applied, each mapped to the commodity complement attribute that receives its value.
        // Keys are matched exactly, including case.
        private static readonly IReadOnlyDictionary<string, string> AllowedKeys =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                [KeyDataPairKeys.NumberOfAnimals] = defraimp_commoditycomplement.Fields.defraimp_NumberofAnimals,
            };

        private readonly CommodityComplementQueries queries;
        private readonly IOrganizationService service;
        private readonly ILogWriter logger;

        /// <summary>Initializes a new instance of the <see cref="CommodityComplementKeyDataApplier"/> class.</summary>
        /// <param name="queries">The Dataverse queries used to find the commodity complement.</param>
        /// <param name="service">The current invocation's organisation service, used for the update.</param>
        /// <param name="logger">The log writer for warnings about key data pairs that cannot be applied.</param>
        public CommodityComplementKeyDataApplier(CommodityComplementQueries queries, IOrganizationService service, ILogWriter logger)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public void Apply(ApplyCommodityComplementKeyDataContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var key = context.Key;
            if (key == null || !AllowedKeys.TryGetValue(key, out var attributeName))
            {
                return;
            }

            if (context.ImporterNotificationId == null || context.CommodityComplementParameterSetId == null)
            {
                this.logger.Log(Severity.Warning, nameof(CommodityComplementKeyDataApplier), $"Cannot apply key data pair '{key}' - it is missing the importer notification or commodity complement parameter set reference.");
                return;
            }

            var complementSetName = this.ResolveComplementParameterSetName(context);

            if (string.IsNullOrWhiteSpace(complementSetName)
                || !int.TryParse(complementSetName, NumberStyles.Integer, CultureInfo.InvariantCulture, out var complementId))
            {
                this.logger.Log(Severity.Warning, nameof(CommodityComplementKeyDataApplier), $"Cannot apply key data pair '{key}' - commodity complement parameter set name '{complementSetName}' is not a valid complement ID.");
                return;
            }

            // The lookup has no TopCount and no tie-break, so if several commodity complements match, an arbitrary
            // one is updated. This should be investigated but is preserved by the refactoring.
            var importerNotificationId = context.ImporterNotificationId.Value;
            var commodityComplementId = this.queries.FindCommodityComplementId(importerNotificationId, complementId);

            if (commodityComplementId == null)
            {
                this.logger.Log(Severity.Warning, nameof(CommodityComplementKeyDataApplier), $"Cannot apply key data pair '{key}' - no matching commodity complement found for importer notification {importerNotificationId} and complement ID {complementId}.");
                return;
            }

            // The update is sent even when the value is unchanged, which triggers downstream plug-ins, flows and
            // audit. This should be investigated but is preserved by the refactoring.
            var update = new defraimp_commoditycomplement { Id = commodityComplementId.Value };
            update[attributeName] = context.Data;
            this.service.Update(update);
        }

        // The lookup's Name is only populated when the reference was resolved from a stored record (such as an image
        // or a retrieved entity). A lookup set through an @odata.bind on Create has no Name, so the record is read.
        private string ResolveComplementParameterSetName(ApplyCommodityComplementKeyDataContext context)
        {
            return string.IsNullOrWhiteSpace(context.CommodityComplementParameterSetName)
                ? this.queries.GetParameterSetName(context.CommodityComplementParameterSetId.Value)
                : context.CommodityComplementParameterSetName;
        }
    }
}
