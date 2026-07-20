namespace Defra.Imports.Plugins.ImportApplication
{
    using Defra.Imports.BusinessLogic;
    using Defra.Imports.BusinessLogic.ImportApplication;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using Microsoft.Xrm.Sdk;

    [CrmPluginRegistration(
    MessageNameEnum.Create,
    nameof(defraimp_importapplication),
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "defraimp_placeoforiginid,defraimp_primaryitahcid",
    "Create Step",
    0,
    IsolationModeEnum.Sandbox,
    Image1Attributes = "defraimp_placeoforiginid,defraimp_primaryitahcid",
    Image1Name = "PostImage",
    Image1Type = ImageTypeEnum.PostImage)]

    [CrmPluginRegistration(
        MessageNameEnum.Update,
        nameof(defraimp_importapplication),
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "defraimp_placeoforiginid,defraimp_primaryitahcid",
        "Update Step",
        0,
        IsolationModeEnum.Sandbox,
        Image1Attributes = "defraimp_placeoforiginid,defraimp_primaryitahcid",
        Image1Name = "PreImage",
        Image1Type = ImageTypeEnum.PreImage,
        Image2Attributes = "defraimp_placeoforiginid,defraimp_primaryitahcid",
        Image2Name = "PostImage",
        Image2Type = ImageTypeEnum.PostImage)]

    [CrmPluginRegistration(
    MessageNameEnum.Delete,
    nameof(defraimp_importapplication),
    StageEnum.PostOperation,
    ExecutionModeEnum.Synchronous,
    "defraimp_placeoforiginid,defraimp_primaryitahcid",
    "Delete Step",
    0,
    IsolationModeEnum.Sandbox,
    Image1Attributes = "defraimp_placeoforiginid,defraimp_primaryitahcid",
    Image1Name = "PreImage",
    Image1Type = ImageTypeEnum.PreImage)]

    public class PrimaryITAHCCountManager : Plugin
    {
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            // Ensure the depth is 1
            if (context.Depth == 1)
            {
                // Try to retrieve a pre-image. This won't work if it's the correct step, so we will pass in a null object which the business logic will handle.
                defraimp_importapplication preImageApplication = context.PreEntityImages.Contains("PreImage") ? context.PreEntityImages["PreImage"].ToEntity<defraimp_importapplication>() : null;

                // Get the post image
                defraimp_importapplication postImageApplication = context.PostEntityImages.Contains("PostImage") ? context.PostEntityImages["PostImage"].ToEntity<defraimp_importapplication>() : null;

                // Create an import application and place of origin repository
                IPlaceOfOriginRepository placeOfOriginRepo = new PlaceOfOriginRepository(orgSvc);
                ImportsContext crmContext = new ImportsContext(orgSvc);

                // Start the business logic
                PrimaryITAHCCountManagerBusinessLogic primaryITAHCCountManagerBusinessLogic = new PrimaryITAHCCountManagerBusinessLogic(preImageApplication, postImageApplication, crmContext, placeOfOriginRepo, logWriter);
                primaryITAHCCountManagerBusinessLogic.RunLogic();
            }
        }
    }
}
