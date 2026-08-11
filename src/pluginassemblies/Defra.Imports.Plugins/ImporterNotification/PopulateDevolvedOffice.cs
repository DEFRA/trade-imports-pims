using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Itahc;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Defra.Imports.Repositories.PostcodeRegion;
using Microsoft.Xrm.Sdk;

namespace Defra.Imports.Plugins.ImporterNotification
{
    [CrmPluginRegistration(
    MessageNameEnum.Create,
    "defraimp_importernotification",
    StageEnum.PreOperation,
    ExecutionModeEnum.Synchronous,
    "defraimp_importernotificationid",
    "Create Step - Importer Notification Populate Devolved Office",
    0,
    IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
    MessageNameEnum.Update,
    "defraimp_importernotification",
    StageEnum.PreOperation,
    ExecutionModeEnum.Synchronous,
    "defraimp_placeofdestinationaddresspostalzipcode",
    "Update Step - Importer Notification Populate Devolved Office",
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

            if (target.LogicalName == defraimp_ImporterNotification.EntityLogicalName)
            {
                businessLogic.UpdateDevolvedOfficeForTarget("defraimp_placeofdestinationaddresspostalzipcode", "defraimp_devolvedoffice");
            }
            else
            {
                throw new InvalidPluginExecutionException($"{nameof(PopulateDevolvedOffice)}: This plugin has been registered on an unsupported entity");
            }
        }
    }
}
