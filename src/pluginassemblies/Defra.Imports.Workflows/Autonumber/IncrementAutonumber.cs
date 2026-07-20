using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace Defra.Imports.Workflows
{
    [CrmPluginRegistration(
    nameof(IncrementAutonumber),
    "Increment the specified autonumber attribute",
    "Increments the auto number counter for a specific record",
    "Defra.Imports.Workflows.Autonumber",
    IsolationModeEnum.Sandbox)]
    public class IncrementAutonumber : WorkflowActivity
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
                autoNumberRepo.IncrementAutonumber(autoNumberCounterName);
            }
            catch(InvalidPluginExecutionException exception)
            {
                logWriter.Log(Severity.Error, nameof(IncrementAutonumber), exception.Message);
            }
        }
    }
}
