using System;
using System.Activities;
using System.Globalization;
using System.Linq;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.Extensions;
using Defra.Imports.BusinessLogic.Utils;

namespace Defra.Imports.Workflows.Utils
{

    [CrmPluginRegistration(
      nameof(CountRecordsByFetchXml),
      "Counts records by FetchXML",
      "Counts records using a FetchXML query",
      "Defra.Imports.Utils",
      IsolationModeEnum.Sandbox)]
    public class CountRecordsByFetchXml : WorkflowActivity
    {
        /// <summary>
        /// Gets or sets the fetch XML used to retrieve the records.
        /// </summary>
        [Input("Lookup Entity (Schema) Name")]
        [RequiredArgument]
        public InArgument<string> EntitySchemaName { get; set; }

        /// <summary>
        /// Gets or sets the fetch XML filters used to retrieve the records.
        /// </summary>
        [Input("Fetch XML Conditions")]
        [RequiredArgument]
        public InArgument<string> FetchXmlConditions { get; set; }

        /// <summary>
        /// Gets or sets the fetch XML link entities used to retrieve the records.
        /// </summary>
        [Input("Fetch XML Link Entities")]
        public InArgument<string> FetchXmlLinkEntities { get; set; }

        /// <summary>
        /// Gets or sets optional parameter used to change the primary entity from which to populate the FetchXML.
        /// </summary>
        [Output("Count")]
        public OutArgument<int> Count { get; set; }

        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            string entitySchemaName = this.EntitySchemaName.GetRequired(context, nameof(this.EntitySchemaName));
            string fetchXmlConditions = this.FetchXmlConditions.GetRequired(context, nameof(this.FetchXmlConditions));
            string fetchXmlLinkEntities = this.FetchXmlLinkEntities.Get(context);

            var fetchXml =
                $"<fetch aggregate='true'><entity name='{entitySchemaName}' >" +
                $"<attribute name='statecode' alias='count' aggregate='count' />" +
                $"<filter>{fetchXmlConditions}</filter>{fetchXmlLinkEntities}" +
                $"</entity></fetch>";

            logWriter.Log(
                    Severity.Info,
                    nameof(CountRecordsByFetchXml),
                    $"Called to make request: {fetchXml}");

            var primaryEntity = new EntityReference(workflowContext.PrimaryEntityName, workflowContext.PrimaryEntityId);

            var fetchService = context.GetExtension<IFetchService>() ??
                new FetchService(orgSvc, logWriter, new FetchTemplateParser(orgSvc, logWriter));

            var fetchResult = fetchService.FetchByTemplate(fetchXml, primaryEntity);
            var aliasedValue = ((AliasedValue)fetchResult.First().Attributes["count"]).Value;
            var result = Convert.ToInt32(aliasedValue, CultureInfo.InvariantCulture);

            this.Count.Set(context, result);
        }
    }
}
