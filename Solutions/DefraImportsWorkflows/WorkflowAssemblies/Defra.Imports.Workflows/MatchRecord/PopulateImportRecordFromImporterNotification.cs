using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Extensions;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.Utils;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Workflows.MatchRecord
{
    [CrmPluginRegistration(
    nameof(PopulateImportRecordFromImporterNotification),
    "Maps fields from ImporterNotification to Import Record using a mapping web resource",
    "Maps fields from ImporterNotification to Import Record using a xml mapping web resource",
    "Defra.Imports.Workflows.MatchRecord",
    IsolationModeEnum.Sandbox)]
    public class PopulateImportRecordFromImporterNotification : WorkflowActivity
    {
        [Input("The Match Record that is calling this workflow")]
        [RequiredArgument]
        [ReferenceTarget("defraimp_matchrecord")]
        public InArgument<EntityReference> MatchRecord { get; set; }

        [Input("Importer Notification Record to copy from")]
        [RequiredArgument]
        [ReferenceTarget("defraimp_importernotification")]
        public InArgument<EntityReference> ImporterNotificationToMapFrom { get; set; }

        [Input("Import Record to copy to")]
        [RequiredArgument]
        [ReferenceTarget("defraimp_importapplication")]
        public InArgument<EntityReference> ImportRecordToMapTo { get; set; }

        [Input("Field Mapping XML Web Resource")]
        [RequiredArgument]
        [ReferenceTarget("webresource")]
        public InArgument<EntityReference> FieldMappingResource { get; set; }

        [Input("Overwrite existing fields?")]
        [RequiredArgument]
        public InArgument<bool> OverwriteExistingFields { get; set; }

        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            EntityReference importerNotificationToMapFromRef = ImporterNotificationToMapFrom.GetRequired(context, nameof(this.ImporterNotificationToMapFrom));
            EntityReference importRecordToMapToRef = ImportRecordToMapTo.GetRequired(context, nameof(this.ImportRecordToMapTo));
            bool shouldOverwriteExisting = OverwriteExistingFields.GetRequired(context, nameof(this.OverwriteExistingFields));

            EntityReference matchRecordRef = MatchRecord.GetRequired(context, nameof(this.ImporterNotificationToMapFrom));
            EntityReference fieldMappingResource = FieldMappingResource.GetRequired(context, nameof(this.FieldMappingResource));

            IRepositoryFactory repoFactory = new RepositoryFactory(orgSvc);
            ICrmRepository<defraimp_matchrecord> matchRecordRepo = new CrmRepository<ImportsContext, defraimp_matchrecord>(orgSvc);
            defraimp_matchrecord matchRecord = matchRecordRepo.Retrieve(matchRecordRef.Id, new string[] {"defraimp_copyconsigneefrom", "defraimp_copyconsignorfrom", "defraimp_copyplaceofdestinationfrom", "defraimp_copyplaceoforiginfrom","defraimp_copytransporterfrom"});

            MatchXmlGenerator importerMatchXmlGenerator = new MatchXmlGenerator(repoFactory, matchRecord, fieldMappingResource);
            string importerNotificationXml = importerMatchXmlGenerator.GenerateImporterNotificationMatchXML();

            MapFieldsFromOneRecordToAnotherBusinessLogic mapFieldsBusinessLogic = new MapFieldsFromOneRecordToAnotherBusinessLogic(repoFactory, importerNotificationToMapFromRef, importRecordToMapToRef, importerNotificationXml, shouldOverwriteExisting);
            mapFieldsBusinessLogic.RunLogic();
        }
    }
}