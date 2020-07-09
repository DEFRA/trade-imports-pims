using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.Import_Query;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
using Defra.Imports.Repositories.Annotations;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Plugins.ImportQuery
{
    [CrmPluginRegistration(
     MessageNameEnum.Create,
     "defraimp_importquery",
     StageEnum.PostOperation,
     ExecutionModeEnum.Synchronous,
     "activityid",
     "Create Step - Import Query Clone Processes",
     0,
     IsolationModeEnum.Sandbox)]

    public class CloneProcessesImportQuery : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            var crmContext = new ImportsContext(orgSvc);
            var annotationRepos = new AnnotationsRepository(crmContext);

            var importQueryFromContext = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_importquery>();

            var addNotesToCloneImportQuery = new AddNotesToCloneImportQuery(annotationRepos, importQueryFromContext);
            addNotesToCloneImportQuery.CloneNotes();
        }
    }
}
