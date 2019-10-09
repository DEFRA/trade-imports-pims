using System;
using System.Linq;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk.Query;
using Defra.Imports.Repositories;
using Defra.Imports.Model;

namespace Defra.Imports.Xrm.Workflows
{
  public class AutomatedRiskLevelAssignment : WorkFlowActivityBase
  {
    #region Input/Output

    [Input("Import Application")]
    [ReferenceTarget("defraimp_importapplication")]
    public InArgument<EntityReference> ImportApplication { get; set; }

    #endregion Input/Output

    public override void ExecuteCRMWorkFlowActivity(CodeActivityContext executionContext, LocalWorkflowContext crmWorkflowContext)
    {
      if (executionContext == null)
      {
        throw new ArgumentNullException("crmWorkflowContext");
      }

      var context = executionContext.GetExtension<IWorkflowContext>();
      var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
      var service = serviceFactory.CreateOrganizationService(context.UserId);
      var tracingService = crmWorkflowContext.TracingService;
      tracingService.Trace(string.Format("{0} - CreateCommunications execution started", DateTime.Now));

      try
      {
        //Implement logic
        var crmContext = new ImportsContext(service);
        EntityReference importApplicationReference = ImportApplication.Get(executionContext); //Get the import application we're working with
        defraimp_importapplication importApplication = GetImportApplication(importApplicationReference.Id, service, tracingService);

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
          EntityCollection countryCommodityRiskLevelCollection = service.RetrieveMultiple(riskLevelQuery);

          //Assign the risk level to the target application
          if (countryCommodityRiskLevelCollection.Entities.Count > 0)
          {
            //Take the first or default entity
            Entity countryCommodityRiskLevel = countryCommodityRiskLevelCollection.Entities.FirstOrDefault();
            EntityReference riskLevel = (EntityReference)countryCommodityRiskLevel["defraimp_importrisklevelid"];

            //If we get a null value it will clear the field
            importApplication.defraimp_importrisklevelid = riskLevel;
            importApplication.defraimp_ImportRiskLevelStatus = defraimp_importapplication_defraimp_importrisklevelstatus.AutomaticallyRiskAssessed;

          }
          else //Ensure the value is empty if we do not find an appropriate risk level
          {
            importApplication.defraimp_importrisklevelid = null;
            importApplication.defraimp_ImportRiskLevelStatus = defraimp_importapplication_defraimp_importrisklevelstatus.UnabletoAutomaticallyRiskAssessNoCorrespondingRiskLevel;
          }
        }
        else //If we don't recieve both Country of Origin and Commodity type then one is empty and we must unassign any current risk levels
        {
          importApplication.defraimp_importrisklevelid = null;
          importApplication.defraimp_ImportRiskLevelStatus = defraimp_importapplication_defraimp_importrisklevelstatus.UnabletoAutomaticallyRiskAssessMissingData;
        }
        //Update the import notification
        service.Update(importApplication);
      }
      catch (Exception e)
      {
        tracingService.Trace(string.Format("{0} - Unhandled exception raised: {1}", DateTime.Now, e));
      }
    }

    defraimp_importapplication GetImportApplication(Guid id, IOrganizationService service, ITracingService tracingService)
    {
      ImportApplicationRepository importApplicationRepo = new ImportApplicationRepository(service, tracingService);
      ColumnSet columnSet = new ColumnSet(new string[] { "defraimp_importrisklevelid", "defraimp_countryoforiginid", "defraimp_commoditytypeid" });
      return importApplicationRepo.GetImportApplicationWithID(id, columnSet);
    }
  }
}
