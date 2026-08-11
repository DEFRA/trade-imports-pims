using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System.Linq;

namespace Defra.Imports.Repositories.PostcodeRegion
{
    public class PostcodeRegionRepository : IPostcodeRegionRepository
    {
        private IOrganizationService orgService;
        private string[] postcodeRegionColumns = new string[] {
            "defraimp_postcoderegionid", "defraimp_postcodeprefix", "defraimp_devolvedoffice",
            "defraimp_postcodedistrict","defraimp_ukcountry", "defraimp_ukregion",
        };

        public PostcodeRegionRepository(IOrganizationService orgService)
        {
            this.orgService = orgService;
        }

        public defraimp_postcoderegion FindPostcodeRegionByPostcodePrefix(string postcodePrefix)
        {
            QueryExpression queryExpression = new QueryExpression(defraimp_postcoderegion.EntityLogicalName);
            queryExpression.Criteria.AddCondition(new ConditionExpression("defraimp_postcodeprefix", ConditionOperator.Equal, postcodePrefix));
            queryExpression.ColumnSet = new ColumnSet(postcodeRegionColumns);

            EntityCollection entityCol = this.orgService.RetrieveMultiple(queryExpression);
            Entity foundEntity = entityCol.Entities.FirstOrDefault();
            if(foundEntity != null)
            {
                return foundEntity.ToEntity<defraimp_postcoderegion>();
            }
            else
            {
                return null;
            }
        }

    }
}
