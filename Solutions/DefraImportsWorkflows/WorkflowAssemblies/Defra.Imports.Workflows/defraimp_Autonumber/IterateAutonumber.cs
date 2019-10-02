using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Defra.Imports.Repositories;

namespace Defra.Imports.Xrm.Workflows
{
    public class IterateAutonumber : WorkFlowActivityBase
    {
        public override void ExecuteCRMWorkFlowActivity(CodeActivityContext executionContext, LocalWorkflowContext crmWorkflowContext)
        {
            if (executionContext == null)
            {
                throw new ArgumentNullException("crmWorkflowContext");
            }

            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            var tracingService = crmWorkflowContext.TracingService;
            tracingService.Trace(string.Format("{0} - CreateCommunications execution started", DateTime.Now));

            try
            {
                AutonumberRepository autonumberRepo = new AutonumberRepository(service, tracingService);
                autonumberRepo.IncrementAutonumber("defraimp_ImportApplicationRecordCount");
            }
            catch (Exception e)
            {
                tracingService.Trace(string.Format("{0} - Unhandled exception raised: {1}", DateTime.Now, e));
            }
        }
    }
}
