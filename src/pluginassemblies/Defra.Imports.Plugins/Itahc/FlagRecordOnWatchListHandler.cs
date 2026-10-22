namespace Defra.Imports.Plugins.Itahc
{
    using System;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Extensions;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Plugins;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Adapts the post-operation, synchronous Create step of defraimp_itahc to the <see cref="IWatchFlagCreator"/>.
    /// A step that does not match the registration contract fails. Any other failure is not caught, as the legacy
    /// plug-in did not catch it.
    /// </summary>
    public sealed class FlagRecordOnWatchListHandler : IPluginHandler
    {
        private readonly IWatchFlagCreator watchFlagCreator;
        private readonly IPluginExecution execution;

        /// <summary>Initializes a new instance of the <see cref="FlagRecordOnWatchListHandler"/> class.</summary>
        /// <param name="watchFlagCreator">The shared watch flag creator.</param>
        /// <param name="execution">The current invocation's services.</param>
        public FlagRecordOnWatchListHandler(IWatchFlagCreator watchFlagCreator, IPluginExecution execution)
        {
            this.watchFlagCreator = watchFlagCreator ?? throw new ArgumentNullException(nameof(watchFlagCreator));
            this.execution = execution ?? throw new ArgumentNullException(nameof(execution));
        }

        /// <inheritdoc/>
        public void Execute()
        {
            var context = this.execution.Context;

            // A mismatch with the registered step is a configuration error, so it fails loudly.
            if (context.PrimaryEntityName != defraimp_itahc.EntityLogicalName
                || context.Stage != PluginStages.PostOperation
                || context.Mode != PluginModes.Synchronous
                || !string.Equals(context.MessageName, PluginMessages.Create, StringComparison.OrdinalIgnoreCase)
                || !context.InputParameters.TryGetValue(PluginInputParameters.Target, out var value)
                || !(value is Entity target))
            {
                throw new InvalidOperationException(
                    "This plug-in requires a synchronous post-operation ITAHC Create with a Target.");
            }

            // ToEntity copies the Target, and the projection then takes only the values that the application needs.
            this.watchFlagCreator.CreateWatchFlags(target.ToEntity<defraimp_itahc>().ToCreateWatchFlagsContext());
        }
    }
}
