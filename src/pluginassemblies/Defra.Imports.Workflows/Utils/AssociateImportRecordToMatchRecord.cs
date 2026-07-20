using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Extensions;
using Defra.Imports.BusinessLogic.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace Defra.Imports.Workflows.Utils
{
    [CrmPluginRegistration(
      nameof(AssociateImportRecordToMatchRecord),
      "Associate Import Record to Match Record",
      "Associates an Import Record to a Match Record",
      "Defra.Imports.Utils",
      IsolationModeEnum.Sandbox)]
    public class AssociateImportRecordToMatchRecord : WorkflowActivity
    {
        /// <summary>
        /// Gets or sets the fetch XML filters used to retrieve the records.
        /// </summary>
        [Input("Relationship Logical Name")]
        [RequiredArgument]
        public InArgument<string> RelationshipName { get; set; }

        [Input("The Match Record")]
        [RequiredArgument]
        [ReferenceTarget("defraimp_matchrecord")]
        public InArgument<EntityReference> MatchRecord { get; set; }

        [Input("Import Record")]
        [RequiredArgument]
        [ReferenceTarget("defraimp_importapplication")]
        public InArgument<EntityReference> ImportRecord { get; set; }


        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            string relationshipName = this.RelationshipName.GetRequired(context, nameof(this.RelationshipName));
            EntityReference matchRecord = MatchRecord.GetRequired(context, nameof(this.MatchRecord));
            EntityReference importRecord = ImportRecord.GetRequired(context, nameof(this.ImportRecord));

            EntityReferenceCollection associateCollection = new EntityReferenceCollection();
            associateCollection.Add(matchRecord);

            Relationship relationship = new Relationship(relationshipName);
            
            orgSvc.Associate(importRecord.LogicalName, importRecord.Id, relationship, associateCollection);
        }
    }
}
