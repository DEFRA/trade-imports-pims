namespace Defra.Imports.Plugins.ImporterNotification
{
    using Defra.Imports.BusinessLogic;
    using Defra.Imports.BusinessLogic.ImporterNotification;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [CrmPluginRegistration(
       MessageNameEnum.Create,
       "defraimp_importernotification",
       StageEnum.PostOperation,
       ExecutionModeEnum.Asynchronous,
       "defraimp_importernotificationid",
       "Create Step",
       0,
       IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
      MessageNameEnum.Update,
      "defraimp_importernotification",
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      "defraimp_routetransitingstates",
      "Update Step",
      0,
      IsolationModeEnum.Sandbox)]
    public class PopulateCountriesOfTransit : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            defraimp_ImporterNotification target = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_ImporterNotification>();

            PopulateCountriesOfTransitBusinessLogic populateCountriesOfTransitBusinessLogic = new PopulateCountriesOfTransitBusinessLogic(target, orgSvc, logWriter);
            populateCountriesOfTransitBusinessLogic.RunLogic();
        }


    }
}
