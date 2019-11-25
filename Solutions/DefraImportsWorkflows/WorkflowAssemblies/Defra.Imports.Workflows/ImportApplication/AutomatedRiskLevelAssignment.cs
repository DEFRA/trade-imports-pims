using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.ImportApplication;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace Defra.Imports.Workflows.ImportApplication
{
    [CrmPluginRegistration(
nameof(AutomatedRiskLevelAssignment),
"Automated Risk Level Assessment",
"Automatically assesses the risk level of an import application based on a given commodity and country",
"Defra.Imports.Workflows.ImportApplication",
IsolationModeEnum.Sandbox)] 

    public class AutomatedRiskLevelAssignment : WorkflowActivity
    {
        #region Input/Output

        [Input("Import Application")]
        [ReferenceTarget("defraimp_importapplication")]
        [RequiredArgument]
        public InArgument<EntityReference> ImportApplication { get; set; }

        #endregion Input/Output

        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            Guid importApplicationId = ImportApplication.Get(context).Id; //Get the import application we're working with
            ICrmRepository<defraimp_importapplication> importApplicationRepo = new CrmRepository<ImportsContext, defraimp_importapplication>(orgSvc);
            defraimp_importapplication importApplication = importApplicationRepo.Retrieve(importApplicationId, new string[] { "defraimp_countryoforiginid", "defraimp_commoditytypeid" });

            if (importApplication != null)
            {
                defraimp_importapplication updatedImportApplication = new defraimp_importapplication
                {
                    Id = importApplication.Id,
                };
                //Check we have a country and commodity
                if (importApplication.defraimp_CountryofOriginId != null && importApplication.defraimp_CommodityTypeId != null)
                {
                    //Retrieve the origin country and commodity type
                    EntityReference countryOfOrigin = importApplication.defraimp_CountryofOriginId;
                    EntityReference commodityType = importApplication.defraimp_CommodityTypeId;

                    //Construct a query - using Query expression for now as we do not have early bound code generated due a technical issue with the trial instance
                    ColumnSet columnsToReturn = new ColumnSet(new string[] { "defraimp_importrisklevelid" });
                    QueryExpression riskLevelQuery = new QueryExpression
                    {
                        EntityName = "defraimp_importcountrycommodityrisklevel",
                        ColumnSet = columnsToReturn
                    };

                    ConditionExpression countryMatchCondition = new ConditionExpression("defraimp_countryid", ConditionOperator.Equal, countryOfOrigin.Id);
                    ConditionExpression commodityMatchCondition = new ConditionExpression("defraimp_commoditytypeid", ConditionOperator.Equal, commodityType.Id);

                    riskLevelQuery.Criteria.FilterOperator = LogicalOperator.And;
                    riskLevelQuery.Criteria.Conditions.Add(countryMatchCondition);
                    riskLevelQuery.Criteria.Conditions.Add(commodityMatchCondition);

                    //Retreive risk level from Import Country Commodity
                    EntityCollection countryCommodityRiskLevelCollection = orgSvc.RetrieveMultiple(riskLevelQuery);

                    //Assign the risk level to the target application
                    if (countryCommodityRiskLevelCollection.Entities.Count > 0)
                    {
                        //Take the first or default entity
                        Entity countryCommodityRiskLevel = countryCommodityRiskLevelCollection.Entities.FirstOrDefault();

                        if (countryCommodityRiskLevel.Contains("defraimp_importrisklevelid"))
                        {
                            EntityReference riskLevel = (EntityReference)countryCommodityRiskLevel["defraimp_importrisklevelid"];

                            //If we get a null value it will clear the field
                            updatedImportApplication.defraimp_importrisklevelid = riskLevel;
                            updatedImportApplication.defraimp_ImportRiskLevelStatus = defraimp_importapplication_defraimp_importrisklevelstatus.AutomaticallyRiskAssessed;
                        }
                        else //If the country commodity risk level record does not have a risk level (this can happen due to a bad data import)
                        {
                            updatedImportApplication.defraimp_importrisklevelid = null;
                            updatedImportApplication.defraimp_ImportRiskLevelStatus = defraimp_importapplication_defraimp_importrisklevelstatus.UnabletoAutomaticallyRiskAssessNoCorrespondingRiskLevel;
                        }
                    }
                    else //Ensure the value is empty if we do not find an appropriate risk level
                    {
                        updatedImportApplication.defraimp_importrisklevelid = null;
                        updatedImportApplication.defraimp_ImportRiskLevelStatus = defraimp_importapplication_defraimp_importrisklevelstatus.UnabletoAutomaticallyRiskAssessNoCorrespondingRiskLevel;
                    }
                }
                else //If we don't recieve both Country of Origin and Commodity type then one is empty and we must unassign any current risk levels
                {
                    updatedImportApplication.defraimp_importrisklevelid = null;
                    updatedImportApplication.defraimp_ImportRiskLevelStatus = defraimp_importapplication_defraimp_importrisklevelstatus.UnabletoAutomaticallyRiskAssessMissingData;
                }
                //Update the import notification
                orgSvc.Update(updatedImportApplication);
            }
        }
    }
}
