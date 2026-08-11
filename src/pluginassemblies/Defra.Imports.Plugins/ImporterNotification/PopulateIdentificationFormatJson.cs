using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.ImporterNotification;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using System;

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
      "defraimp_identificationofanimalstext,defraimp_commoditycomplementstext",
      "Update Step - Importer Notification Format JSON",
      0,
      IsolationModeEnum.Sandbox,
      Image1Attributes = "defraimp_commoditycomplementstext,defraimp_identificationofanimalstext",
      Image1Name = "PreImage",
      Image1Type = ImageTypeEnum.PreImage)]

    public class PopulateIdentificationFormatJson : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            var notificationFromContext = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_ImporterNotification>();
            var notificationPreImage = (context.MessageName.ToLower() == "update") ? context.PreEntityImages["PreImage"].ToEntity<defraimp_ImporterNotification>() : null;

            try
            {
                var populateFormattedJSONTextFields = new PopulateJSONTextFields(notificationFromContext, notificationPreImage);
                populateFormattedJSONTextFields.FormatIntegrationData();
            }
            catch(Exception e)
            {
                notificationFromContext.defraimp_CommodityIDTypes = "Error extracting and formatting id types. View the tracing service for more details";
                logWriter.Log(Severity.Error, nameof(PopulateIdentificationFormatJson), $"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }
        }
    }
}
