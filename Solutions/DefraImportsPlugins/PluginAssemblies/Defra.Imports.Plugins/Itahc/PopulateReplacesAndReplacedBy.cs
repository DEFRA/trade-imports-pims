namespace Defra.Imports.Plugins.Itahc
{
  using Defra.Imports.BusinessLogic;
  using Defra.Imports.BusinessLogic.Itahc;
  using Defra.Imports.BusinessLogic.Logging;
  using Defra.Imports.Model;
  using Defra.Imports.Repositories;
  using Microsoft.Xrm.Sdk;

  [CrmPluginRegistration(
     MessageNameEnum.Create,
     nameof(defraimp_itahc),
     StageEnum.PreOperation,
     ExecutionModeEnum.Synchronous,
     "defraimp_itahcid",
     "Create Step",
     0,
     IsolationModeEnum.Sandbox)]

  [CrmPluginRegistration(
      MessageNameEnum.Update,
      nameof(defraimp_itahc),
      StageEnum.PreOperation,
      ExecutionModeEnum.Synchronous,
      "defraimp_replacedreferencenumber,defraimp_replacingreferencenumber",
      "Update Step",
      0,
      IsolationModeEnum.Sandbox)]

  [CrmPluginRegistration(
     MessageNameEnum.Create,
     nameof(defraimp_docom),
     StageEnum.PreOperation,
     ExecutionModeEnum.Synchronous,
     "defraimp_docomid",
     "Create Step DOCOM",
     0,
     IsolationModeEnum.Sandbox)]

  [CrmPluginRegistration(
      MessageNameEnum.Update,
      nameof(defraimp_docom),
      StageEnum.PreOperation,
      ExecutionModeEnum.Synchronous,
      "defraimp_replacedreferencenumber,defraimp_replacingreferencenumber",
      "Update Step DOCOM",
      0,
      IsolationModeEnum.Sandbox)]
  public class PopulateReplacesAndReplacedBy : Plugin
  {
    protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
    {
      Entity target = (Entity)context.InputParameters["Target"];

      ICrmRepository entityRepo = repositoryFactory.GetRepository(target.LogicalName);

      PopulateReplacesAndReplacedByBusinessLogic populateReplacesAndReplacedByBusinessLogic = new PopulateReplacesAndReplacedByBusinessLogic(entityRepo, target);
      populateReplacesAndReplacedByBusinessLogic.RunLogic();
    }
  }
}
