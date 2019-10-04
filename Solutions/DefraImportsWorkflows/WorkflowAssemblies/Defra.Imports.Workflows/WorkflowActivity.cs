using Defra.Imports.BusinessLogic.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Workflows
{
    /// <summary>
    /// Base class for Dynamics 365 workflow activities.
    /// </summary>
    public abstract class WorkflowActivity : CodeActivity
    {
        internal abstract void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter);

        protected override void Execute(CodeActivityContext context)
        {
            var tracingSvc = context.GetExtension<ITracingService>();
            var workflowContext = context.GetExtension<IWorkflowContext>();
            var serviceFactory = context.GetExtension<IOrganizationServiceFactory>();
            var orgSvc = serviceFactory.CreateOrganizationService(workflowContext.UserId);
            var logWriter = new TracingServiceLogWriter(tracingSvc);

            this.ExecuteWorkflowActivity(context, workflowContext, orgSvc, logWriter);
        }
    }
}
