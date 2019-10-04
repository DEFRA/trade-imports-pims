using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace Defra.Imports.Workflows.Autonumber
{
    [CrmPluginRegistration(
    nameof(IncrementAutonumber),
    "Reset the specified autonumber attribute",
    "Reset the auto number counter for a specific record to 0",
    "Defra.Imports.Workflows.Autonumber",
    IsolationModeEnum.Sandbox)]
    public class ResetAutonumber : WorkflowActivity
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
                autoNumberRepo.SetAutonumberValue(autoNumberCounterName, 0);
            }
            catch (InvalidPluginExecutionException exception)
            {
                logWriter.Log(Severity.Error, nameof(ResetAutonumber), exception.Message);
            }
        }
    }
}
