namespace Defra.Imports.Plugins.ImportApplication
{
    using System;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Extensions;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Plugins;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Adapts the post-operation, synchronous Create and Update steps of defraimp_importapplication to the
    /// <see cref="IWatchFlagAssociator"/>. A step that does not match the registration contract fails. A Create that
    /// does not supply a primary ITAHC does nothing, and any other failure is not caught, as the legacy plug-in did
    /// not catch it.
    /// </summary>
    public sealed class AssociateWatchFlagsFromPrimaryItahcHandler : IPluginHandler
    {
        private readonly IWatchFlagAssociator watchFlagAssociator;
        private readonly IPluginExecution execution;

        /// <summary>Initializes a new instance of the <see cref="AssociateWatchFlagsFromPrimaryItahcHandler"/> class.</summary>
        /// <param name="watchFlagAssociator">The shared watch flag associator.</param>
        /// <param name="execution">The current invocation's services.</param>
        public AssociateWatchFlagsFromPrimaryItahcHandler(IWatchFlagAssociator watchFlagAssociator, IPluginExecution execution)
        {
            this.watchFlagAssociator = watchFlagAssociator ?? throw new ArgumentNullException(nameof(watchFlagAssociator));
            this.execution = execution ?? throw new ArgumentNullException(nameof(execution));
        }

        /// <inheritdoc/>
        public void Execute()
        {
            var context = this.execution.Context;

            // A mismatch with the registered step is a configuration error, so it fails loudly.
            var isCreate = string.Equals(context.MessageName, PluginMessages.Create, StringComparison.OrdinalIgnoreCase);
            if (context.PrimaryEntityName != defraimp_importapplication.EntityLogicalName
                || context.Stage != PluginStages.PostOperation
                || context.Mode != PluginModes.Synchronous
                || !(isCreate || string.Equals(context.MessageName, PluginMessages.Update, StringComparison.OrdinalIgnoreCase))
                || !context.InputParameters.TryGetValue(PluginInputParameters.Target, out var value)
                || !(value is Entity target))
            {
                throw new InvalidOperationException(
                    "This plug-in requires a synchronous post-operation import application Create or Update with a Target.");
            }

            // ToEntity copies the Target, and the projection then takes only the values that the application needs.
            var associateContext = target.ToEntity<defraimp_importapplication>().ToAssociateItahcWatchFlagsContext();

            // A Create only associates watch flags when it supplies a primary ITAHC, but an Update always does, even
            // when it clears it.
            if (isCreate && associateContext.PrimaryItahcId == null)
            {
                return;
            }

            this.watchFlagAssociator.AssociateWatchFlags(associateContext);
        }
    }
}
