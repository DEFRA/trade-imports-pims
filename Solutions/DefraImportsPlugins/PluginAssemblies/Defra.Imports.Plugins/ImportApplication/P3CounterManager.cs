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

    [CrmPluginRegistration(
       MessageNameEnum.Create,
       nameof(defraimp_importapplication),
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "defraimp_primaryitahcid",
       "Create Step",
       0,
       IsolationModeEnum.Sandbox,
       Image1Attributes = "defraimp_importapplicationtype,defraimp_primaryitahcid,defraimp_risklevelid",
       Image1Name = "PostImage",
       Image1Type = ImageTypeEnum.PostImage)]

    [CrmPluginRegistration(
       MessageNameEnum.Update,
       nameof(defraimp_importapplication),
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "defraimp_primaryitahcid",
       "Update Step",
       0,
       IsolationModeEnum.Sandbox,
       Image1Attributes = "defraimp_importapplicationtype,defraimp_primaryitahcid,defraimp_risklevelid",
       Image1Name = "PreImage",
       Image1Type = ImageTypeEnum.PreImage,
       Image2Attributes = "defraimp_importapplicationtype,defraimp_primaryitahcid,defraimp_risklevelid",
       Image2Name = "PostImage",
       Image2Type = ImageTypeEnum.PostImage)]

    [CrmPluginRegistration(
       MessageNameEnum.Delete,
       nameof(defraimp_importapplication),
       StageEnum.PostOperation,
       ExecutionModeEnum.Synchronous,
       "defraimp_primaryitahcid",
       "Delete Step",
       0,
       IsolationModeEnum.Sandbox,
       Image1Attributes = "defraimp_importapplicationtype,defraimp_primaryitahcid,defraimp_risklevelid",
       Image1Name = "PreImage",
       Image1Type = ImageTypeEnum.PreImage)]

    public class P3CounterManager : Plugin
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

                // Generate an autonumber repo
                IAutonumberRepository autonumberRepository = new AutonumberRepository(orgSvc);

                // Start the business logic
                P3CounterManagerBusinessLogic p3CounterManagerBusinessLogic = new P3CounterManagerBusinessLogic(preImageApplication, postImageApplication, autonumberRepository, logWriter);
                p3CounterManagerBusinessLogic.RunLogic();
            }
        }
    }
}
