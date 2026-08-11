namespace Defra.Imports.Repositories
{
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using System.Linq;

    class ImportRiskLevelRepository : IImportRiskLevelRepository
    {
        private readonly IOrganizationService orgSvc;
        private readonly ITracingService tracingService;

        public ImportRiskLevelRepository(IOrganizationService svc, ITracingService service)
        {
            this.orgSvc = svc;
            this.tracingService = service;
        }

        public defraimp_importrisklevel GetRiskLevelByName(string name)
        {
            QueryExpression query = new QueryExpression(defraimp_importrisklevel.EntityLogicalName);
            query.ColumnSet = new ColumnSet(false);
            ConditionExpression nameCondition = new ConditionExpression("defraimp_name", ConditionOperator.Equal, name);
            query.Criteria.AddCondition(nameCondition);

            defraimp_importrisklevel importRiskLevel = orgSvc.RetrieveMultiple(query).Entities.FirstOrDefault() as defraimp_importrisklevel;

            return importRiskLevel;
        }
    }
}
