namespace Defra.Imports.Plugins.ParameterSetKeyDataPair
{
    using System;
    using System.Linq;
    using Defra.Imports.BusinessLogic;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.ParameterSetKeyDataPair;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;

    /// <inheritdoc/>
    [CrmPluginRegistration(
     MessageNameEnum.Create,
     "defraimp_parametersetkeydatapair",
     StageEnum.PostOperation,
     ExecutionModeEnum.Synchronous,
     "defraimp_importernotificationid",
     "Create Step - Populate Animal Number",
     0,
     IsolationModeEnum.Sandbox)]
    [CrmPluginRegistration(
      MessageNameEnum.Update,
      "defraimp_parametersetkeydatapair",
      StageEnum.PostOperation,
      ExecutionModeEnum.Synchronous,
      "defraimp_data",
      "Update Step - Populate Animal Number",
      0,
      IsolationModeEnum.Sandbox,
      Image1Attributes = "defraimp_commoditycomplementparameterset,defraimp_importernotificationid,defraimp_name",
      Image1Name = "PreImage",
      Image1Type = ImageTypeEnum.PreImage)]
    public class PopulateAnimalNumber : Plugin
    {
        /// <summary>
        /// Executes the plugin logic to populate animal numbers for the ParameterSetKeyDataPair entity.
        /// </summary>
        /// <param name="context">The plugin execution context.</param>
        /// <param name="orgSvc">The organization service.</param>
        /// <param name="logWriter">The tracing service log writer.</param>
        /// <param name="repositoryFactory">The repository factory.</param>
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            try
            {
                var keyDataPairFromContext = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_parametersetkeydatapair>();

                var message = (MessageNameEnum)Enum.Parse(typeof(MessageNameEnum), context.MessageName, true);

                defraimp_parametersetkeydatapair keyDataPair;
                if (message == MessageNameEnum.Create)
                {
                    keyDataPair = keyDataPairFromContext;
                }
                else
                {
                    keyDataPair = context.PreEntityImages.FirstOrDefault().Value?.ToEntity<defraimp_parametersetkeydatapair>();
                    if (keyDataPair != null)
                    {
                        keyDataPair.defraimp_data = keyDataPairFromContext.defraimp_data;
                    }
                }

                // only run this plugin if the key data pair is for the number of animals
                if (keyDataPair != null && keyDataPair.defraimp_name == "number_animal")
                {
                    var addSpeciesAnimalNumbers = new AddSpeciesAnimalNumbers(orgSvc, logWriter);
                    addSpeciesAnimalNumbers.AddAnimalNumbersToSpecies(keyDataPair);
                }
            }
            catch (Exception e)
            {
                logWriter.Log(Severity.Error, nameof(PopulateAnimalNumber), $"{e.Message}{Environment.NewLine}{e.StackTrace}");
            }
        }
    }
}
