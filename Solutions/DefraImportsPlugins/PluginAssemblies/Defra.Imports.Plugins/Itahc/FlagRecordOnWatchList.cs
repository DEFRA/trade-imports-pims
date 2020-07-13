using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Itahc;
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

namespace Defra.Imports.Plugins.Itahc
{
    [CrmPluginRegistration(
       MessageNameEnum.Create,
       nameof(defraimp_itahc),
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "defraimp_itahcid",
       "Create Step - ITAHC Flag record on Watch List",
       0,
       IsolationModeEnum.Sandbox)]
    public class FlagRecordOnWatchList : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            var itahcFromContext = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_itahc>();

            var watchListRepo = new CrmRepository<ImportsContext, defraimp_WatchList>(orgSvc);
            var watchFlagRepo = new CrmRepository<ImportsContext, defraimp_WatchFlag>(orgSvc);

            FlagRecordOnWatchListBusinessLogic flagRecordLogic = new FlagRecordOnWatchListBusinessLogic(orgSvc, itahcFromContext, watchListRepo, watchFlagRepo);
            flagRecordLogic.FlagRecordIfOnWatchList();
        }
    }
}
