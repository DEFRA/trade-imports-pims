namespace Defra.Imports.Plugins.ImportApplication
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Defra.Imports.BusinessLogic;
    using Defra.Imports.BusinessLogic.Constants;
    using Defra.Imports.BusinessLogic.ImportApplication;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using Microsoft.Xrm.Sdk;

    [CrmPluginRegistration(
       MessageNameEnum.Update,
       nameof(defraimp_importapplication),
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "defraimp_primaryitahcid,defraimp_importrisklevelid,defraimp_placeoforiginid,defraimp_primaryimporternotificationid",
       "Update Step",
       0,
       IsolationModeEnum.Sandbox,
       Image1Attributes = "defraimp_importapplicationtype,defraimp_importrisklevelid,defraimp_previousimportrisklevelid,defraimp_inspectionrequired,defraimp_inspectionrequiredreason,defraimp_placeoforiginid,defraimp_commoditytypeid,defraimp_countryoforiginid,defraimp_primaryitahcid,statuscode,statecode,defraimp_importrecordcounted,defraimp_manualpostimportcheckdecision,defraimp_primaryimporternotificationid",
       Image1Name = "PreImage",
       Image1Type = ImageTypeEnum.PreImage,
       Image2Attributes = "defraimp_importapplicationtype,defraimp_importrisklevelid,defraimp_previousimportrisklevelid,defraimp_inspectionrequired,defraimp_inspectionrequiredreason,defraimp_placeoforiginid,defraimp_commoditytypeid,defraimp_countryoforiginid,defraimp_primaryitahcid,statuscode,statecode,defraimp_importrecordcounted,defraimp_manualpostimportcheckdecision,defraimp_primaryimporternotificationid",
       Image2Name = "PostImage",
       Image2Type = ImageTypeEnum.PostImage)]

    [CrmPluginRegistration(
       MessageNameEnum.SetState,
       nameof(defraimp_importapplication),
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "",
       "Set State Step",
       0,
       IsolationModeEnum.Sandbox,
       Image1Attributes = "defraimp_importapplicationtype,defraimp_importrisklevelid,defraimp_previousimportrisklevelid,defraimp_inspectionrequired,defraimp_inspectionrequiredreason,defraimp_placeoforiginid,defraimp_commoditytypeid,defraimp_countryoforiginid,defraimp_primaryitahcid,statuscode,statecode,defraimp_importrecordcounted,defraimp_manualpostimportcheckdecision,defraimp_primaryimporternotificationid",
       Image1Name = "PreImage",
       Image1Type = ImageTypeEnum.PreImage,
       Image2Attributes = "defraimp_importapplicationtype,defraimp_importrisklevelid,defraimp_previousimportrisklevelid,defraimp_inspectionrequired,defraimp_inspectionrequiredreason,defraimp_placeoforiginid,defraimp_commoditytypeid,defraimp_countryoforiginid,defraimp_primaryitahcid,statuscode,statecode,defraimp_importrecordcounted,defraimp_manualpostimportcheckdecision,defraimp_primaryimporternotificationid",
       Image2Name = "PostImage",
       Image2Type = ImageTypeEnum.PostImage)]

    [CrmPluginRegistration(
       MessageNameEnum.Delete,
       nameof(defraimp_importapplication),
       StageEnum.PreOperation,
       ExecutionModeEnum.Synchronous,
       "",
       "Delete Step",
       0,
       IsolationModeEnum.Sandbox,
       Image1Attributes = "defraimp_importapplicationtype,defraimp_importrisklevelid,defraimp_previousimportrisklevelid,defraimp_inspectionrequired,defraimp_inspectionrequiredreason,defraimp_placeoforiginid,defraimp_commoditytypeid,defraimp_countryoforiginid,defraimp_primaryitahcid,statuscode,statecode,defraimp_importrecordcounted,defraimp_manualpostimportcheckdecision,defraimp_primaryimporternotificationid",
       Image1Name = "PreImage",
       Image1Type = ImageTypeEnum.PreImage)]

    public class DetermineInspectionRequirement : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Fired plugin with message: " + context.MessageName + ". Depth = " + context.Depth);
            logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Plugin executing");

            // Try to retrieve a pre-image. This won't work if it's the correct step, so we will pass in a null object which the business logic will handle.
            defraimp_importapplication preImageApplication = context.PreEntityImages.Contains("PreImage") ? context.PreEntityImages["PreImage"].ToEntity<defraimp_importapplication>() : null;

            // Get the post image
            defraimp_importapplication postImageApplication = context.PostEntityImages.Contains("PostImage") ? context.PostEntityImages["PostImage"].ToEntity<defraimp_importapplication>() : null;

            DetermineInspectionRequirementBusinessLogic determineInspectionRequirementBusinessLogic = new DetermineInspectionRequirementBusinessLogic(preImageApplication, postImageApplication, repositoryFactory, logWriter);
            determineInspectionRequirementBusinessLogic.RunLogic();
        }

        private bool IsITAHCUpdateMessage(IPluginExecutionContext context, TracingServiceLogWriter logWriter)
        {
            string[] customMessageNames = this.GetCustomMessageNames();
            bool isRootContextCustomMessage = this.IsRootContextCustomMessage(context, customMessageNames);

            if (context.MessageName == "Update" && isRootContextCustomMessage)
            {
                logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Plugin Message is ITAHC Update");
                return true;
            }
            else
            {
                logWriter.Log(Severity.Info, "DetermineInspectionLogic", "Plugin Message is not ITAHC Update. Context message: " + context.MessageName + ". IsRootContextCustomMessage = " + isRootContextCustomMessage);
                return false;
            }
        }

        private string[] GetCustomMessageNames()
        {
            return new string[] { CustomActionNames.CreateImportRecordFromItahc };
        }
    }
}
