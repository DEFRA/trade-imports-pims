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
     "defraimp_importernotification",
     StageEnum.PreOperation,
     ExecutionModeEnum.Synchronous,
     "defraimp_importernotificationid",
     "Create Step - Importer Notification Format JSON",
     0,
     IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
      MessageNameEnum.Update,
      "defraimp_importernotification",
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

            var notificationPreImage = (context.MessageName.ToLower() == "update") ? context.PreEntityImages["PreImage"].ToEntity<defraimp_ImporterNotification>() : null;

            var populateFormattedJSONTextFields = new PopulateJSONTextFields(notificationFromContext, notificationPreImage);
            populateFormattedJSONTextFields.FormatIntegrationData();
        }
    }
}
