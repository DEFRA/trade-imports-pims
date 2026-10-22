namespace Defra.Imports.Plugins.ParameterSetKeyDataPair
{
    using System;
    using System.Linq;
    using Defra.Imports.Application.CommodityComplements;
    using Defra.Imports.Dataverse;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.Infrastructure.Plugins;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Adapts the post-operation, synchronous Create and Update steps of defraimp_parametersetkeydatapair to the
    /// <see cref="ICommodityComplementKeyDataApplier"/>. A step that does not match the registration contract fails.
    /// Any other failure is traced and then ignored, as the legacy plug-in did.
    /// </summary>
    public sealed class PopulateAnimalNumberHandler : IPluginHandler
    {
        private readonly ICommodityComplementKeyDataApplier applier;
        private readonly IPluginExecution execution;
        private readonly ILogWriter logger;

        /// <summary>Initializes a new instance of the <see cref="PopulateAnimalNumberHandler"/> class.</summary>
        /// <param name="applier">The shared commodity complement key data applier.</param>
        /// <param name="execution">The current invocation's services.</param>
        /// <param name="logger">The log writer used to trace failures.</param>
        public PopulateAnimalNumberHandler(ICommodityComplementKeyDataApplier applier, IPluginExecution execution, ILogWriter logger)
        {
            this.applier = applier ?? throw new ArgumentNullException(nameof(applier));
            this.execution = execution ?? throw new ArgumentNullException(nameof(execution));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public void Execute()
        {
            // A mismatch with the registered step is a configuration error, so it fails loudly rather than being
            // swallowed with the failures below.
            var context = this.execution.Context;
            var target = GetTarget(context);
            var isCreate = IsCreate(context);

            // TODO: swallowing every exception should be investigated. A failed service call inside this
            // post-operation transaction may already have doomed it, so the catch might not protect the triggering
            // operation. The behaviour is preserved as is because the refactoring must not change behaviour.
            try
            {
                var keyDataPair = isCreate
                    ? target.ToEntity<defraimp_parametersetkeydatapair>()
                    : MergePreImage(context, target);

                if (keyDataPair != null)
                {
                    this.applier.Apply(keyDataPair);
                }
            }
            catch (Exception exception)
            {
                this.logger.Log(Severity.Error, nameof(PopulateAnimalNumber), $"{exception.Message}{Environment.NewLine}{exception.StackTrace}");
            }
        }

        private static Entity GetTarget(IPluginExecutionContext context)
        {
            if (context.PrimaryEntityName != defraimp_parametersetkeydatapair.EntityLogicalName
                || context.Stage != PluginStages.PostOperation
                || context.Mode != PluginModes.Synchronous
                || !context.InputParameters.TryGetValue(PluginInputParameters.Target, out var value)
                || !(value is Entity target))
            {
                throw new InvalidOperationException(
                    "This plug-in requires a synchronous post-operation parameter set key data pair Create or Update with a Target.");
            }

            return target;
        }

        private static bool IsCreate(IPluginExecutionContext context)
        {
            if (string.Equals(context.MessageName, PluginMessages.Create, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (!string.Equals(context.MessageName, PluginMessages.Update, StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException("This plug-in supports only the Create and Update messages.");
            }

            return false;
        }

        // On Update the Target holds only the changed values, so the pre-image supplies the rest and the Target's
        // data value is overlaid on it. Neither is modified.
        private static defraimp_parametersetkeydatapair MergePreImage(IPluginExecutionContext context, Entity target)
        {
            // The first pre-image is used whatever its name; this should be investigated, as the step is only
            // expected to have one, but the behaviour is preserved by the refactoring.
            var preImage = context.PreEntityImages.Select(image => image.Value).FirstOrDefault();
            var keyDataPair = preImage?.ToEntity<defraimp_parametersetkeydatapair>().Clone();
            if (keyDataPair != null)
            {
                // The Target's data replaces the pre-image's even when the Target has none, which nulls it. This
                // should be investigated, as the step filters on defraimp_data, but it is preserved as is.
                keyDataPair.defraimp_data = target.ToEntity<defraimp_parametersetkeydatapair>().defraimp_data;
            }

            return keyDataPair;
        }
    }
}
