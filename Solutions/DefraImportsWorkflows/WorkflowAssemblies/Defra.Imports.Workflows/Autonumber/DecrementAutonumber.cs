using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Defra.Imports.Repositories;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic;

namespace Defra.Imports.Workflows.Autonumber
{
    [CrmPluginRegistration(
    nameof(DecrementAutonumber),
    "Decrement the specified autonumber attribute",
    "Decrements the auto number counter for a specific record",
    "Defra.Imports.Workflows.Autonumber",
    IsolationModeEnum.Sandbox)]
    public class DecrementAutonumber : WorkflowActivity
    {
        [Input("Autonumber Counter Name")]
        [RequiredArgument]
        public InArgument<string> AutonumberCounterName { get; set; }

        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            try
            {
                string autoNumberCounterName = context.GetValue<string>(AutonumberCounterName);
                AutonumberRepository autoNumberRepo = new AutonumberRepository(orgSvc);
                autoNumberRepo.DecrementAutonumber(autoNumberCounterName);
            }
            catch(InvalidPluginExecutionException exception)
            {
                logWriter.Log(Severity.Error, nameof(IncrementAutonumber), exception.Message);
            }
        }
    }
}
