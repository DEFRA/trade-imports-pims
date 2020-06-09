namespace Defra.Imports.Workflows.MatchRecord
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;
    using Defra.Imports.BusinessLogic;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.Extensions;
    using Defra.Imports.BusinessLogic.Utils;
    using Defra.Imports.Model;

    [CrmPluginRegistration(
      nameof(PotentialRelatedImportRecordSearch),
      "Find potentially related Import Records for Match Record",
      "Finds potentially related Import Records for the given Match Record",
      "Defra.Imports.Workflows.MatchRecord",
      IsolationModeEnum.Sandbox)]
    public class PotentialRelatedImportRecordSearch : WorkflowActivity
    {
        [Input("ITAHC")]
        [RequiredArgument]
        [ReferenceTarget("defraimp_itahc")]
        public InArgument<EntityReference> ITAHC { get; set; }

        [Input("Importer Notification")]
        [RequiredArgument]
        [ReferenceTarget("defraimp_importernotification")]
        public InArgument<EntityReference> ImporterNotification { get; set; }

        protected override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            string targetEntitySchemaName = defraimp_importapplication.EntityLogicalName;
            EntityReference itahcRef = ITAHC.GetRequired(context, nameof(this.ITAHC));
            EntityReference importerNotificationRef = ImporterNotification.GetRequired(context, nameof(this.ImporterNotification));

            var fetchXml =
                $"<fetch version='1.0' output-format='xml - platform' mapping='logical' distinct='false'>" +
                $"<entity name='{defraimp_importapplication.EntityLogicalName}'>" +
                $"<filter type='or'>" +
                $"<condition attribute='defraimp_primaryitahcid' operator='eq' uiname='{itahcRef.Name}' uitype='defraimp_itahc' value='{{{itahcRef.Id}}}' />" +
                $"<condition attribute='defraimp_primaryimporternotificationid' operator='eq' uiname='{importerNotificationRef.Name}' uitype='defraimp_importernotification' value='{{{importerNotificationRef.Id}}}'/>" +
                $"</filter></entity></fetch>";

            logWriter.Log(
                Severity.Info,
                nameof(PotentialRelatedImportRecordSearch),
                $"Called to make request: {fetchXml}");

            EntityReference primaryEntity = new EntityReference(workflowContext.PrimaryEntityName, workflowContext.PrimaryEntityId);

            var fetchService = context.GetExtension<IFetchService>() ??
                new FetchService(orgSvc, logWriter, new FetchTemplateParser(orgSvc, logWriter));

            var fetchResult = fetchService.FetchByTemplate(fetchXml, primaryEntity);
            List<Entity> entities = new List<Entity>(fetchResult);

            InnRelationshipAssociator relationshipAssociator = new NNRelationshipAssociator(primaryEntity, entities, orgSvc);
            relationshipAssociator.RunLogic();

        }
    }
}
