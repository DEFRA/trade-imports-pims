

namespace Defra.Imports.BusinessLogic
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Defra.Imports.Model;

    class defraimp_AutonumberService
    {
        private readonly IOrganizationService orgSvc;
        private readonly ITracingService tracingService;

        public defraimp_AutonumberService(IOrganizationService svc, ITracingService service)
        {
            this.orgSvc = svc;
            this.tracingService = service;
        }

        public defraimp_autonumber GetAutonumberWithKey(string key)
        {
                QueryExpression query = new QueryExpression(defraimp_autonumber.EntityLogicalName);
                query.ColumnSet = new ColumnSet("defraimp_key", "defraimp_currentnumber");
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

        public void IterateAutonumber(string key)
        {
            defraimp_autonumber autonumberRecord = GetAutonumberWithKey(key);
            int newNumber = (int)autonumberRecord.defraimp_CurrentNumber + 1;
            orgSvc.Update(autonumberRecord);
        }
    }
}
