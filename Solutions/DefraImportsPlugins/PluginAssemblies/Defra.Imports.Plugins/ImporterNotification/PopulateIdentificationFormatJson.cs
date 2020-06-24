using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using Defra.Imports.BusinessLogic.ImporterNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Plugins.ImporterNotification
{
    [CrmPluginRegistration(
     MessageNameEnum.Create,
     nameof(defraimp_ImporterNotification),
     StageEnum.PreOperation,
     ExecutionModeEnum.Synchronous,
     "defraimp_itahcid",
     "Create Step - Importer Notification Format JSON",
     0,
     IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
      MessageNameEnum.Update,
      nameof(defraimp_ImporterNotification),
      StageEnum.PreOperation,
      ExecutionModeEnum.Synchronous,
      "defraimp_identificationofanimalstext, defraimp_commoditycomplementstext",
      "Update Step - Importer Notification Format JSON",
      0,
      IsolationModeEnum.Sandbox)]

    public class PopulateIdentificationFormatJson : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            var notificationFromContext = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_ImporterNotification>();

            var populateFormattedJSONTextFields = new PopulateJSONTextFields(notificationFromContext);
            populateFormattedJSONTextFields.FormatIntegrationData();
        }
    }
}
