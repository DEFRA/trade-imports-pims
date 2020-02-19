namespace Defra.Imports.Plugins.ImportApplication
{
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
    using Microsoft.Xrm.Sdk.Workflow;

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
                defraimp_importapplication preImageApplication = null;
                if (context.PreEntityImages.Contains("PreImage"))
                {
                    Entity preImage = (Entity)context.PreEntityImages["PreImage"];
                    preImageApplication = preImage.ToEntity<defraimp_importapplication>();
                }

                // Get the post image
                defraimp_importapplication postImageApplication = null;
                if (context.PostEntityImages.Contains("PostImage"))
                {
                    Entity postImage = (Entity)context.PostEntityImages["PostImage"];
                    postImageApplication = postImage.ToEntity<defraimp_importapplication>();
                }

                // Create an import application and place of origin repository
                ICrmRepository<defraimp_importapplication> importApplicationRepo = new CrmRepository<ImportsContext, defraimp_importapplication>(orgSvc);
                IPlaceOfOriginRepository placeOfOriginRepo = new PlaceOfOriginRepository(orgSvc);

                // Start the business logic
                PrimaryITAHCCountManagerBusinessLogic primaryITAHCCountManagerBusinessLogic = new PrimaryITAHCCountManagerBusinessLogic(preImageApplication, postImageApplication, placeOfOriginRepo, logWriter);
                primaryITAHCCountManagerBusinessLogic.RunLogic();
            }
        }
    }
}
