namespace Defra.Imports.Repositories
{
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using System.Linq;

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

            if (autonumberRecord != null)
            {
                int value = autonumberRecord.defraimp_CurrentNumber ?? 0;
                return value;
            }
            else
            {
                throw new InvalidPluginExecutionException("Autonumber record key not found.");
            }
        }

        public void IncrementAutonumber(string key)
        {
            defraimp_autonumber autonumberRecord = GetAutonumberWithKey(key);
            autonumberRecord.defraimp_CurrentNumber++;
            orgSvc.Update(autonumberRecord);
        }

        public void IncrementAutonumber(string key, int amountToIncrementBy)
        {
            defraimp_autonumber autonumberRecord = GetAutonumberWithKey(key);
            autonumberRecord.defraimp_CurrentNumber += amountToIncrementBy;
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
