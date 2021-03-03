using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.ImporterNotification;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
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
        StageEnum.PreOperation,
        ExecutionModeEnum.Synchronous,
        "defraimp_importernotificationid",
        "Create Step - Importer Notification Populate Imp Type",
        0,
        IsolationModeEnum.Sandbox)]

    [CrmPluginRegistration(
          MessageNameEnum.Update,
          "defraimp_importernotification",
          StageEnum.PreOperation,
          ExecutionModeEnum.Synchronous,
          "defraimp_imptype",
          "Update Step - Importer Notification Populate Imp Type",
          0,
          IsolationModeEnum.Sandbox)]
    public class PopulateImpType : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            var target = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_ImporterNotification>();
            var impTypeRepo = repositoryFactory.GetRepository<ImportsContext, defraimp_imptype>();

            PopulateImpTypeBusinessLogic logic = new PopulateImpTypeBusinessLogic(target, impTypeRepo);
            logic.RunLogic();
        }
    }
}