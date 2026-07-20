using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Itahc;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Defra.Imports.Repositories.PostcodeRegion;
using Microsoft.Xrm.Sdk;

namespace Defra.Imports.Plugins.Itahc
{
    [CrmPluginRegistration(
        MessageNameEnum.Create,
        nameof(defraimp_itahc),
        StageEnum.PreOperation,
        ExecutionModeEnum.Synchronous,
        "defraimp_itahcid",
        "Create Step - ITAHC Populate Devolved Office",
        0,
        IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
        MessageNameEnum.Update,
        nameof(defraimp_itahc),
        StageEnum.PreOperation,
        ExecutionModeEnum.Synchronous,
        "defraimp_placeofdestinationaddresspostcode",
        "Update Step - ITAHC Populate Devolved Office",
        0,
        IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
        MessageNameEnum.Create,
        nameof(defraimp_docom),
        StageEnum.PreOperation,
        ExecutionModeEnum.Synchronous,
        "defraimp_docomid",
        "Create Step - DOCOM Populate Devolved Office",
        0,
        IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
        MessageNameEnum.Update,
        nameof(defraimp_docom),
        StageEnum.PreOperation,
        ExecutionModeEnum.Synchronous,
        "defraimp_placeofdestinationaddresspostcode",
        "Update Step - DOCOM Populate Devolved Office",
        0,
        IsolationModeEnum.Sandbox)]

    public class PopulateDevolvedOffice : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            Entity target = (Entity)context.InputParameters["Target"];

            IPostcodeRegionRepository postcodeRegionRepo = new PostcodeRegionRepository(orgSvc);

            ImportsContext crmContext = new ImportsContext(orgSvc);
            IConfigurationParameterRepository configParameterRepo = new ConfigurationParameterRepository(crmContext);

            PopulateDevolvedOfficeBusinessLogic businessLogic = new PopulateDevolvedOfficeBusinessLogic(target, postcodeRegionRepo, configParameterRepo);

            if(target.LogicalName == defraimp_itahc.EntityLogicalName || target.LogicalName == defraimp_docom.EntityLogicalName)
            {
                businessLogic.UpdateDevolvedOfficeForTarget("defraimp_placeofdestinationaddresspostcode", "defraimp_devolvedoffice");
            }
            else
            {
                throw new InvalidPluginExecutionException($"{nameof(PopulateDevolvedOffice)}: This plugin has been registered on an unsupported entity");
            }

        }
    }
}
