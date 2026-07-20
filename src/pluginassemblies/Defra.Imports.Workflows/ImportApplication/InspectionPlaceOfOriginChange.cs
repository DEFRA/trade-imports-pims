using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.ImportApplication;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;

namespace Defra.Imports.Workflows.ImportApplication
{
    [CrmPluginRegistration(
    nameof(InspectionPlaceOfOriginChange),
    "Inspection Place of Origin Change for Import Application",
    "Manages the Place of Origin changing and how it impacts the Inspection Requirements",
    "Defra.Imports.Workflows.ImportApplication",
    IsolationModeEnum.Sandbox)]
    public class InspectionPlaceOfOriginChange : WorkflowActivity
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
            IPlaceOfOriginRepository placeOfOriginRepo = new PlaceOfOriginRepository(orgSvc);
            IRepositoryFactory repositoryFactory = new RepositoryFactory(orgSvc);
            defraimp_importapplication importApplication = importApplicationRepo.Retrieve(importApplicationId, new string[] { "defraimp_importrisklevelid", "defraimp_previousimportrisklevelid", "defraimp_inspectionrequired", "defraimp_placeoforiginid", "defraimp_previousplaceoforiginid", "defraimp_commoditytypeid", "defraimp_countryoforiginid", "defraimp_inspectionrequiredreason" });
            InspectionPlaceOfOriginChangeBusinessLogic determineInspectionRequirementBusinessLogic = new InspectionPlaceOfOriginChangeBusinessLogic(importApplication, importApplicationRepo, coverageRulesRepo, autoNumberRepo, placeOfOriginRepo, repositoryFactory, logWriter);
            determineInspectionRequirementBusinessLogic.RunLogic();
        }
    }
}
