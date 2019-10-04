namespace Defra.Imports.Repositories
{
    using System.Linq;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;

    class AutonumberRepository : IAutonumberRepository
    {
        private readonly IOrganizationService orgSvc;
        private readonly ITracingService tracingService;

        public AutonumberRepository(IOrganizationService svc)
        {
            this.orgSvc = svc;
        }

        public defraimp_autonumber GetAutonumberWithKey(string key)
        {
                QueryExpression query = new QueryExpression(defraimp_autonumber.EntityLogicalName);
                query.ColumnSet = new ColumnSet(new string[] { "defraimp_key", "defraimp_currentnumber" });
                ConditionExpression keyCondition = new ConditionExpression("defraimp_key", ConditionOperator.Equal, key);
                query.Criteria.AddCondition(keyCondition);

                defraimp_autonumber autonumber = orgSvc.RetrieveMultiple(query).Entities.FirstOrDefault() as defraimp_autonumber;

                return autonumber;
        }

        public int GetAutonumberValue(string key)
        {
            defraimp_autonumber autonumberRecord = GetAutonumberWithKey(key);
            return (int)autonumberRecord.defraimp_CurrentNumber;
        }

        public void IncrementAutonumber(string key)
        {
            defraimp_autonumber autonumberRecord = GetAutonumberWithKey(key);
            autonumberRecord.defraimp_CurrentNumber++;
            orgSvc.Update(autonumberRecord);
        }

        public void DecrementAutonumber(string key)
        {
            defraimp_autonumber autonumberRecord = GetAutonumberWithKey(key);
            autonumberRecord.defraimp_CurrentNumber--;
            orgSvc.Update(autonumberRecord);
        }

        public void SetAutonumberValue(string key, int value)
        {
            defraimp_autonumber autonumberRecord = GetAutonumberWithKey(key);
            autonumberRecord.defraimp_CurrentNumber = value;
            orgSvc.Update(autonumberRecord);
        }
    }
}
