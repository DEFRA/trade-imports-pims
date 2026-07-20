using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.ImportApplication;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace Defra.Imports.Workflows.ImportApplication
{


    [CrmPluginRegistration(
    nameof(DetermineInspectionRequirement),
    "Determine Inspection for Import Application",
    "Determines whether a particular application should be inspected",
    "Defra.Imports.Workflows.ImportApplication",
    IsolationModeEnum.Sandbox)]
    public class DetermineInspectionRequirement : WorkflowActivity
    {
        #region
        [Input("Import Application")]
        [ReferenceTarget("defraimp_importapplication")]
        [RequiredArgument]
        public InArgument<EntityReference> ImportApplication { get; set; }
        #endregion

        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            Guid importApplicationId = context.GetValue<EntityReference>(ImportApplication).Id;
            
            // Create an import application repository
            ICrmRepository<defraimp_importapplication> importApplicationRepo = new CrmRepository<ImportsContext, defraimp_importapplication>(orgSvc);
            IRepositoryFactory repositoryFactory = new RepositoryFactory(orgSvc);
            defraimp_importapplication importApplication = importApplicationRepo.Retrieve(importApplicationId, new string[] { "defraimp_importrisklevelid", "defraimp_previousimportrisklevelid", "defraimp_inspectionrequired", "defraimp_placeoforiginid","defraimp_commoditytypeid", "defraimp_countryoforiginid", "defraimp_primaryitahcid" });
            DetermineInspectionRequirementBusinessLogic determineInspectionRequirementBusinessLogic = new DetermineInspectionRequirementBusinessLogic(null,importApplication, repositoryFactory, logWriter);
            determineInspectionRequirementBusinessLogic.RunLogic();
        }
    }
}
