using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Itahc;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
      IsolationModeEnum.Sandbox)]

    public class PopulateIntegrationFormattedValues : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            var itahcFromContext = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_itahc>();

            var populateFormattedJSONTextFields = new PopulateFormattedJSONTextFields(itahcFromContext);
            populateFormattedJSONTextFields.FormatIntegrationData();
        }
    }
}
