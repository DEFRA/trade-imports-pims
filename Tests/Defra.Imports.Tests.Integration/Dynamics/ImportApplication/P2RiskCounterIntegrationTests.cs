using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Integration.Dynamics.ImportApplication
{
    public class P2RiskCounterIntegrationTests : IntegrationTests
    {
        [Fact]
        public void CreateImportRecord_WithP2RiskLevel_IncrementsTheP2Counter()
        {
            // Get a commodity and country that relate to P2
            Guid irmsTeamId = new Guid("A8E19BEE-0106-EA11-A811-000D3AB5D511");
            Guid dogCommodityId = new Guid("195545A7-BBD3-E911-A870-000D3AB1DCDD");
            Guid franceCountryId = new Guid("BE9BB7ED-B2D3-E911-A861-000D3AB1DAD7");

            // Get the current count
            QueryExpression queryExpression = new QueryExpression(defraimp_autonumber.EntityLogicalName);
            queryExpression.ColumnSet = new ColumnSet(new string[] { "defraimp_currentnumber", "defraimp_key" });
            queryExpression.Criteria.AddCondition(new ConditionExpression("defraimp_key", ConditionOperator.Equal, "p2_record_count"));

            EntityCollection autoNumberCol = _orgSvc.RetrieveMultiple(queryExpression);
            if (autoNumberCol.Entities.Count > 0)
            {
                defraimp_autonumber p2Counter = autoNumberCol.Entities.FirstOrDefault().ToEntity<defraimp_autonumber>();
                p2Counter.defraimp_CurrentNumber = 0;
                
                // Set the current p2 counter to 0
                _orgSvc.Update(p2Counter);

                int initialCounterValue = p2Counter.defraimp_CurrentNumber.Value;

                // Create the import record
                defraimp_importapplication importRecord = new defraimp_importapplication()
                {
                    defraimp_DevolvedOfficeId = new EntityReference("team", irmsTeamId),
                    defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC,
                    defraimp_CommodityTypeId = new EntityReference("defraexp_commoditytype", dogCommodityId),
                    defraimp_CountryofOriginId = new EntityReference("defra_country", franceCountryId)
                };

                _orgSvc.Create(importRecord);

                // Get the count after the import record creation
                EntityCollection autoNumberColAfter = _orgSvc.RetrieveMultiple(queryExpression);
                if (autoNumberCol.Entities.Count > 0)
                {
                    p2Counter = autoNumberColAfter.Entities.FirstOrDefault().ToEntity<defraimp_autonumber>();
                    int AfterCounterValue = p2Counter.defraimp_CurrentNumber.Value;

                    Assert.Equal(initialCounterValue + 1, AfterCounterValue);
                }
                else
                {
                    throw new Exception("P2 counter doesn't exist");
                }
            }
            else
            {
                throw new Exception("P2 counter doesn't exist");
            }

        }

    }
}
