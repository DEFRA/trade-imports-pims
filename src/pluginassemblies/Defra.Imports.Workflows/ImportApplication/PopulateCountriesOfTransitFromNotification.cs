using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Extensions;
using Defra.Imports.BusinessLogic.ImportApplication;
using Defra.Imports.BusinessLogic.Logging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using System.Activities;

namespace Defra.Imports.Workflows.ImportApplication
{
    [CrmPluginRegistration(
      nameof(PopulateCountriesOfTransitFromNotification),
      "Populate the countries of transit on the import record from the importer notification",
      "Populate the countries of transit on the import record from the importer notification",
      "Defra.Imports.Workflows.ImportApplication",
      IsolationModeEnum.Sandbox)]
    public class PopulateCountriesOfTransitFromNotification : WorkflowActivity
    {
        [Input("Importer Notification to populate transit countries from")]
        [RequiredArgument]
        [ReferenceTarget("defraimp_importernotification")]
        public InArgument<EntityReference> Notification { get; set; }

        [Input("Importer Record to populate transit countries to")]
        [RequiredArgument]
        [ReferenceTarget("defraimp_importapplication")]
        public InArgument<EntityReference> ImportRecord { get; set; }

        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            EntityReference notificationRef = Notification.GetRequired(context, nameof(this.Notification));
            EntityReference importRecordRef = ImportRecord.GetRequired(context, nameof(this.ImportRecord));
            PopulateCountriesOfTransitFromNotificationBusinessLogic businessLogic = new PopulateCountriesOfTransitFromNotificationBusinessLogic(orgSvc);
            businessLogic.PopulateCountriesOfTransit(notificationRef, importRecordRef);
        }
    }
}
