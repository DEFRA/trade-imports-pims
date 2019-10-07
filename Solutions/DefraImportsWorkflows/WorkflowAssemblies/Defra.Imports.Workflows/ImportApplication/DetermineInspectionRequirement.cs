using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.ImportApplication;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

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
            ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo = new CrmRepository<ImportsContext, defraimp_inspectioncoveragerule>(orgSvc);
            IAutonumberRepository autoNumberRepo = new AutonumberRepository(orgSvc);
            ICrmRepository<defraimp_importapplication> importApplicationRepo = new CrmRepository<ImportsContext, defraimp_importapplication>(orgSvc);
            defraimp_importapplication importApplication = importApplicationRepo.Retrieve(importApplicationId, new string[] { "defraimp_importrisklevelid", "defraimp_previousimportrisklevelid", "defraimp_inspectionrequired" });
            DetermineInspectionRequirementBusinessLogic determineInspectionRequirementBusinessLogic = new DetermineInspectionRequirementBusinessLogic(importApplication, importApplicationRepo, coverageRulesRepo, autoNumberRepo, logWriter);

            determineInspectionRequirementBusinessLogic.RunLogic();
        }
    }
}
