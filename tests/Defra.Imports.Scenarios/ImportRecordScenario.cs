namespace Defra.Imports.Scenarios
{
    using System;
    using Defra.Imports.Scenarios.Events;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ScenarioBuilder;

    /// <summary>
    /// A scenario for an Import Record created from an Importer Notification.
    /// </summary>
    [ComposeUsing(0, EventIds.ImporterNotificationSubmission, typeof(LogicAppSubmitsImporterNotificationEvent))]
    [ComposeUsing(1, EventIds.ImportRecordCreation, typeof(CaseworkerCreatesImportRecordFromNotificationEvent))]
    [ComposeUsing(2, EventIds.ItahcCreation, typeof(CaseworkerCreatesItahcEvent))]
    public class ImportRecordScenario : Scenario
    {
        /// <summary>
        /// Gets the outputs of the Logic App submitting the Importer Notification.
        /// </summary>
        /// <remarks>
        /// Named to match <see cref="LogicAppSubmitsImporterNotificationEvent"/> so it is populated automatically from <see cref="ScenarioContext"/>.
        /// </remarks>
        public LogicAppSubmitsImporterNotificationEvent.Info LogicAppSubmitsImporterNotificationEvent { get; internal set; }

        /// <summary>
        /// Gets the outputs of the caseworker creating the Import Record.
        /// </summary>
        /// <remarks>
        /// Named to match <see cref="CaseworkerCreatesImportRecordFromNotificationEvent"/> so it is populated automatically from <see cref="ScenarioContext"/>.
        /// </remarks>
        public CaseworkerCreatesImportRecordFromNotificationEvent.Info CaseworkerCreatesImportRecordFromNotificationEvent { get; internal set; }

        /// <summary>
        /// Gets the outputs of the caseworker creating an ITAHC.
        /// </summary>
        /// <remarks>
        /// Named to match <see cref="CaseworkerCreatesItahcEvent"/> so it is populated automatically from <see cref="ScenarioContext"/>.
        /// </remarks>
        public CaseworkerCreatesItahcEvent.Info CaseworkerCreatesItahcEvent { get; internal set; }

        /// <summary>
        /// The event IDs for the events within the <see cref="ImportRecordScenario"/>.
        /// </summary>
        public static class EventIds
        {
            /// <summary>
            /// The event ID of the Importer Notification submission event.
            /// </summary>
            public const string ImporterNotificationSubmission = nameof(ImporterNotificationSubmission);

            /// <summary>
            /// The event ID of the Import Record creation event.
            /// </summary>
            public const string ImportRecordCreation = nameof(ImportRecordCreation);

            /// <summary>
            /// The event ID of the ITAHC creation event.
            /// </summary>
            public const string ItahcCreation = nameof(ItahcCreation);
        }

        /// <summary>
        /// A builder for the <see cref="ImportRecordScenario"/>.
        /// </summary>
        /// <param name="clientFactory">A client factory.</param>
        /// <param name="loggerProvider">
        /// An <see cref="ILoggerProvider"/> used to route the typed loggers injected into events (e.g. <see cref="ILogger{TCategoryName}"/>)
        /// to a test framework's output.
        /// </param>
        public class Builder(ServiceClientFactory clientFactory, ILoggerProvider loggerProvider) : Builder<ImportRecordScenario>
        {
            private readonly ServiceClientFactory clientFactory = clientFactory;
            private readonly ILoggerProvider loggerProvider = loggerProvider;

            /// <summary>
            /// Configures the Logic App submitting the Importer Notification event.
            /// </summary>
            /// <param name="configurator">The configurator.</param>
            /// <returns>The builder.</returns>
            public Builder SubmittedByLogicApp(Action<LogicAppSubmitsImporterNotificationEvent.Builder> configurator = null)
            {
                this.ConfigureEvent<LogicAppSubmitsImporterNotificationEvent, LogicAppSubmitsImporterNotificationEvent.Builder>(EventIds.ImporterNotificationSubmission, configurator);

                return this;
            }

            /// <summary>
            /// Configures the caseworker creating the Import Record event.
            /// </summary>
            /// <param name="configurator">The configurator.</param>
            /// <returns>The builder.</returns>
            public Builder CreatedByCaseworker(Action<CaseworkerCreatesImportRecordFromNotificationEvent.Builder> configurator = null)
            {
                this.ConfigureEvent<CaseworkerCreatesImportRecordFromNotificationEvent, CaseworkerCreatesImportRecordFromNotificationEvent.Builder>(EventIds.ImportRecordCreation, configurator);

                return this;
            }

            /// <summary>
            /// Configures the caseworker creating an ITAHC event.
            /// </summary>
            /// <param name="configurator">The configurator.</param>
            /// <returns>The builder.</returns>
            public Builder WithItahcCreatedByCaseworker(Action<CaseworkerCreatesItahcEvent.Builder> configurator = null)
            {
                this.ConfigureEvent<CaseworkerCreatesItahcEvent, CaseworkerCreatesItahcEvent.Builder>(EventIds.ItahcCreation, configurator);

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
