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
  nameof(DecrementPlaceOfOriginCounters),
  "Decrement Counters on the Import Application's Place of Origin",
  "Safely manages decrement of counter fields on Place of Origin related to the import application",
  "Defra.Imports.Workflows.ImportApplication",
  IsolationModeEnum.Sandbox)]
  public class DecrementPlaceOfOriginCounters : WorkflowActivity
  {

    [Input("Import Application")]
    [ReferenceTarget("defraimp_importapplication")]
    [RequiredArgument]
    public InArgument<EntityReference> ImportApplication { get; set; }

    internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
    {
      Guid importApplicationId = context.GetValue<EntityReference>(ImportApplication).Id;

      // Create an import application repository
      ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo = new CrmRepository<ImportsContext, defraimp_inspectioncoveragerule>(orgSvc);
      ICrmRepository<defraimp_importapplication> importApplicationRepo = new CrmRepository<ImportsContext, defraimp_importapplication>(orgSvc);
      IPlaceOfOriginRepository placeOfOriginRepo = new PlaceOfOriginRepository(orgSvc);
      defraimp_importapplication importApplication = importApplicationRepo.Retrieve(importApplicationId, new string[] { "defraimp_placeoforiginid" });
      defraimp_placeoforigin placeOfOrigin = placeOfOriginRepo.Find(importApplication.defraimp_PlaceofOriginid.Id);
    }
  }
}
