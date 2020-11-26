using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.ImportApplication;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Plugins.ImportApplication
{
    [CrmPluginRegistration(
       MessageNameEnum.Create,
       defraimp_importapplication.EntityLogicalName,
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "defraimp_importapplicationid",
       "Create Step - Associate watch flags from primary notification",
       0,
       IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
        MessageNameEnum.Update,
        defraimp_importapplication.EntityLogicalName,
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "defraimp_primaryimporternotificationid",
        "Update Step - Associate watch flags from primary notification",
        0,
        IsolationModeEnum.Sandbox)]
    public class AssociateWatchFlagsFromNotification : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            defraimp_importapplication target = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_importapplication>();
            ICrmRepository<defraimp_WatchFlag> watchFlagRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_WatchFlag>();

            if (context.MessageName == MessageNameEnum.Update.ToString() ||
                (context.MessageName == MessageNameEnum.Create.ToString() && target.defraimp_PrimaryImporterNotificationId != null))
            {
                AssociateWatchFlagsFromPrimaryNotificationBusinessLogic associateFlagLogic = new AssociateWatchFlagsFromPrimaryNotificationBusinessLogic(target, watchFlagRepo);
                associateFlagLogic.RunLogic();
            }
        }
    }
}
