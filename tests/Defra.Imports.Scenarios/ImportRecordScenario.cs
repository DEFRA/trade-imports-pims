namespace Defra.Imports.Scenarios
{
    using System;
    using Defra.Imports.Scenarios.Events;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ScenarioBuilder;

    /// <summary>
    /// A scenario for creating an Import Record.
    /// </summary>
    [ComposeUsing(0, EventIds.ImportRecordCreation, typeof(CreateImportRecordEvent))]
    public class ImportRecordScenario : Scenario
    {
        /// <summary>
        /// Gets the outputs of the Import Record creation event.
        /// </summary>
        public CreateImportRecordEvent.Info CreateImportRecordEvent { get; internal set; }

        /// <summary>
        /// Event IDs for the scenario.
        /// </summary>
        public static class EventIds
        {
            /// <summary>
            /// The event ID for Import Record creation.
            /// </summary>
            public const string ImportRecordCreation = nameof(ImportRecordCreation);
        }

        /// <summary>
        /// A builder for <see cref="ImportRecordScenario"/>.
        /// </summary>
        /// <param name="clientFactory">A client factory.</param>
        /// <param name="loggerProvider">Logger provider for event logging.</param>
        public class Builder(ServiceClientFactory clientFactory, ILoggerProvider loggerProvider) : Builder<ImportRecordScenario>
        {
            private readonly ServiceClientFactory clientFactory = clientFactory;
            private readonly ILoggerProvider loggerProvider = loggerProvider;

            /// <summary>
            /// Configures Import Record creation.
            /// </summary>
            /// <param name="configurator">The configurator.</param>
            /// <returns>The builder.</returns>
            public Builder Created(Action<CreateImportRecordEvent.Builder> configurator)
            {
                this.ConfigureEvent<CreateImportRecordEvent, CreateImportRecordEvent.Builder>(EventIds.ImportRecordCreation, configurator);

                return this;
            }

            /// <inheritdoc/>
            protected override IServiceCollection InitializeServices(ServiceCollection serviceCollection)
            {
                return serviceCollection
                    .AddSingleton(this.clientFactory)
                    .AddLogging(builder => builder.AddProvider(this.loggerProvider));
            }
        }
    }
}
