using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Itahc;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using System;

namespace Defra.Imports.Plugins.Itahc
{
    [CrmPluginRegistration(
     MessageNameEnum.Create,
     nameof(defraimp_itahc),
     StageEnum.PreOperation,
     ExecutionModeEnum.Synchronous,
     "defraimp_itahcid",
     "Create Step - ITAHC Format JSON",
     0,
     IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
      MessageNameEnum.Update,
      nameof(defraimp_itahc),
      StageEnum.PreOperation,
      ExecutionModeEnum.Synchronous,
      "defraimp_commoditycomplementstext,defraimp_identificationofanimalstext",
      "Update Step - ITAHC Format JSON",
      0,
      IsolationModeEnum.Sandbox,
      Image1Attributes = "defraimp_commoditycomplementstext,defraimp_identificationofanimalstext,defraimp_speciesnomination",
      Image1Name = "PreImage",
      Image1Type = ImageTypeEnum.PreImage)]

    public class PopulateIntegrationFormattedValues : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {

            var itahcFromContext = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_itahc>();
            var itachPreImage = (context.MessageName.ToLower() == "update") ? context.PreEntityImages["PreImage"].ToEntity<defraimp_itahc>() : null;

            try
            {
                var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahcFromContext, itachPreImage);
                populateFormattedJSONTextFields.FormatIntegrationData();
            }
            catch(Exception e)
            {
                itahcFromContext.defraimp_CommodityIdTypes = "Error extracting and formatting id types. View the tracing service for more details";
                logWriter.Log(Severity.Error, nameof(PopulateIntegrationFormattedValues), $"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }

        }
    }
}
