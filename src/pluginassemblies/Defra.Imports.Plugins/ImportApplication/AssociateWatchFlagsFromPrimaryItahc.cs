using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.ImportApplication;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;

namespace Defra.Imports.Plugins.ImportApplication
{
    [CrmPluginRegistration(
       MessageNameEnum.Create,
       nameof(defraimp_importapplication),
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "defraimp_importapplicationid",
       "Create Step - Associate watch flags from primary itahc",
       0,
       IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
        MessageNameEnum.Update,
        nameof(defraimp_importapplication),
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "defraimp_primaryitahcid",
        "Update Step - Associate watch flags from primary itahc",
        0,
        IsolationModeEnum.Sandbox)]
    public class AssociateWatchFlagsFromPrimaryItahc : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            defraimp_importapplication target = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_importapplication>();
            ICrmRepository<defraimp_WatchFlag> watchFlagRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_WatchFlag>();

            if (context.MessageName == MessageNameEnum.Update.ToString() ||
                (context.MessageName == MessageNameEnum.Create.ToString() && target.defraimp_PrimaryITAHCId != null))
            {
                AssociateWatchFlagsFromPrimaryItahcBusinessLogic associateFlagLogic = new AssociateWatchFlagsFromPrimaryItahcBusinessLogic(target, watchFlagRepo);
                associateFlagLogic.RunLogic();
            }
        }
    }
}
