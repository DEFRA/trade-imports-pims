namespace Defra.Imports.BusinessLogic.Utils
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Defra.Imports.BusinessLogic.Logging;

    public class FetchService : IFetchService
    {
        private const string Tag = nameof(FetchService);

        private readonly IOrganizationService organizationService;
        private readonly ILogWriter logWriter;
        private readonly IFetchTemplateParser fetchTemplateParser;

        public FetchService(IOrganizationService organizationService, ILogWriter logWriter, IFetchTemplateParser fetchTemplateParser)
        {
            this.organizationService = organizationService;
            this.logWriter = logWriter;
            this.fetchTemplateParser = fetchTemplateParser;
        }

        public IEnumerable<Entity> FetchByTemplate(string templateFetchXml, EntityReference primaryEntity, Dictionary<string, object> additionalValues = null)
        {
            if (string.IsNullOrEmpty(templateFetchXml))
            {
                throw new System.ArgumentException("No template provided", nameof(templateFetchXml));
            }

            if (primaryEntity == null)
            {
                throw new System.ArgumentNullException(nameof(primaryEntity));
            }

            this.logWriter.Log(Severity.Info, Tag, $"Fetching records by template for primary entity {primaryEntity.LogicalName} {primaryEntity.Id}");

            var parsedFetchXml = this.fetchTemplateParser.Parse(templateFetchXml, primaryEntity, additionalValues);

            this.logWriter.Log(Severity.Info, Tag, $"Template parsed successfully: {parsedFetchXml}");

            var fetchQuery = new FetchExpression(parsedFetchXml);

            var results = this.organizationService.RetrieveMultiple(fetchQuery);

            this.logWriter.Log(Severity.Info, Tag, $"Fetched {results.Entities.Count} records.");

            return results.Entities;
        }

        public IEnumerable<TEntity> FetchByTemplate<TEntity>(string templateFetchXml, EntityReference primaryEntity, Dictionary<string, object> additionalValues = null)
            where TEntity : Entity
        {
            return this.FetchByTemplate(templateFetchXml, primaryEntity).Select(entity => entity.ToEntity<TEntity>());
        }
    }
}