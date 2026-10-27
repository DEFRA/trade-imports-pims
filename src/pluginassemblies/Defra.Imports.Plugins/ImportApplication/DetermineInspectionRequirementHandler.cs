namespace Defra.Imports.Plugins.ImportApplication
{
    using System;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Extensions;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.Infrastructure.Plugins;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Adapts the synchronous Update and SetState (post-operation) and Delete (pre-operation) steps of
    /// defraimp_importapplication to the <see cref="IInspectionRequirementDeterminer"/>. A step that does not match
    /// the registration contract fails. Any other failure is not caught, as the legacy plug-in did not catch it.
    /// </summary>
    public sealed class DetermineInspectionRequirementHandler : IPluginHandler
    {
        private const string PreImageName = "PreImage";
        private const string PostImageName = "PostImage";

        private readonly IInspectionRequirementDeterminer determiner;
        private readonly IPluginExecution execution;
        private readonly ILogWriter logger;

        /// <summary>Initializes a new instance of the <see cref="DetermineInspectionRequirementHandler"/> class.</summary>
        /// <param name="determiner">The shared inspection requirement determiner.</param>
        /// <param name="execution">The current invocation's services.</param>
        /// <param name="logger">The log writer.</param>
        public DetermineInspectionRequirementHandler(IInspectionRequirementDeterminer determiner, IPluginExecution execution, ILogWriter logger)
        {
            this.determiner = determiner ?? throw new ArgumentNullException(nameof(determiner));
            this.execution = execution ?? throw new ArgumentNullException(nameof(execution));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public void Execute()
        {
            var context = this.execution.Context;

            // A mismatch with the registered step is a configuration error, so it fails loudly.
            var hasPostImage = ValidateStep(context);

            this.logger.Log(Severity.Info, InspectionConstants.LogTag, "Fired plugin with message: " + context.MessageName + ". Depth = " + context.Depth);
            this.logger.Log(Severity.Info, InspectionConstants.LogTag, "Plugin executing");

            // The images are copied by ToEntity, and the projection then takes only the values that the application needs.
            var before = context.PreEntityImages[PreImageName].ToEntity<defraimp_importapplication>().ToInspectionContext();
            InspectionImportApplicationContext after = null;
            if (hasPostImage)
            {
                after = context.PostEntityImages[PostImageName].ToEntity<defraimp_importapplication>().ToInspectionContext();
            }

            this.determiner.Determine(before, after);
        }

        // Returns whether the step has a post-image, which a Delete does not.
        private static bool ValidateStep(IPluginExecutionContext context)
        {
            if (context.PrimaryEntityName != defraimp_importapplication.EntityLogicalName
                || context.Mode != PluginModes.Synchronous
                || !context.PreEntityImages.Contains(PreImageName))
            {
                throw new InvalidOperationException(
                    "This plug-in requires a synchronous import application Update, SetState or Delete with a PreImage.");
            }

            if (string.Equals(context.MessageName, PluginMessages.Update, StringComparison.OrdinalIgnoreCase))
            {
                RequireStep(context, PluginStages.PostOperation, PluginInputParameters.Target, typeof(Entity), true);
                return true;
            }

            if (string.Equals(context.MessageName, PluginMessages.SetState, StringComparison.OrdinalIgnoreCase))
            {
                RequireStep(context, PluginStages.PostOperation, PluginInputParameters.EntityMoniker, typeof(EntityReference), true);
                return true;
            }

            if (string.Equals(context.MessageName, PluginMessages.Delete, StringComparison.OrdinalIgnoreCase))
            {
                RequireStep(context, PluginStages.PreOperation, PluginInputParameters.Target, typeof(EntityReference), false);
                return false;
            }

            throw new InvalidOperationException("This plug-in supports only the Update, SetState and Delete messages.");
        }

        private static void RequireStep(IPluginExecutionContext context, int stage, string parameterName, Type parameterType, bool requirePostImage)
        {
            if (context.Stage != stage
                || !context.InputParameters.TryGetValue(parameterName, out var value)
                || value == null
                || !parameterType.IsInstanceOfType(value)
                || (requirePostImage && !context.PostEntityImages.Contains(PostImageName)))
            {
                throw new InvalidOperationException(
                    "The step does not match the registration of this plug-in: the stage, the " + parameterName + " or the images are not as registered.");
            }
        }
    }
}
