using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.ImporterNotification;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Plugins.ImporterNotification
{
    [CrmPluginRegistration(
        MessageNameEnum.Create,
        defraimp_ImporterNotification.EntityLogicalName,
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "defraimp_importernotificationid",
        "Create Step - Importer Notification Flag record on Watch List",
        0,
        IsolationModeEnum.Sandbox)]
    public class FlagRecordOnWatchListNotification : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            var notificationFromContext = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_ImporterNotification>();

            var watchListRepo = new CrmRepository<ImportsContext, defraimp_WatchList>(orgSvc);
            var watchFlagRepo = new CrmRepository<ImportsContext, defraimp_WatchFlag>(orgSvc);

            FlagRecordsOnWatchListNotificationBusinessLogic flagRecordLogic = new FlagRecordsOnWatchListNotificationBusinessLogic(orgSvc, notificationFromContext, watchListRepo, watchFlagRepo);
            flagRecordLogic.FlagRecordIfOnWatchList();
        }
    }
}
