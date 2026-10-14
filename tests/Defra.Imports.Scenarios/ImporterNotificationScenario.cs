namespace Defra.Imports.Scenarios
{
    using System;
    using Defra.Imports.Scenarios.Events;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using ScenarioBuilder;

    /// <summary>
    /// A scenario for an Importer Notification.
    /// </summary>
    [ComposeUsing(0, EventIds.ImporterNotificationSubmission, typeof(LogicAppSubmitsImporterNotificationEvent))]
    public class ImporterNotificationScenario : Scenario
    {
        /// <summary>
        /// Gets the outputs of the Logic App submitting the Importer Notification.
        /// </summary>
        /// <remarks>
        /// Named to match <see cref="LogicAppSubmitsImporterNotificationEvent"/> so it is populated automatically from <see cref="ScenarioContext"/>.
        /// </remarks>
        public LogicAppSubmitsImporterNotificationEvent.Info LogicAppSubmitsImporterNotificationEvent { get; internal set; }

        /// <summary>
        /// The event IDs for the events within the <see cref="ImporterNotificationScenario"/>.
        /// </summary>
        public static class EventIds
        {
            /// <summary>
            /// The event ID of the Importer Notification submission event.
            /// </summary>
            public const string ImporterNotificationSubmission = nameof(ImporterNotificationSubmission);
        }

        /// <summary>
        /// A builder for the <see cref="ImporterNotificationScenario"/>.
        /// </summary>
        /// <param name="clientFactory">A client factory.</param>
        /// <param name="loggerProvider">
        /// An <see cref="ILoggerProvider"/> used to route the typed loggers injected into events (e.g. <see cref="ILogger{TCategoryName}"/>)
        /// to a test framework's output.
        /// </param>
        public class Builder(ServiceClientFactory clientFactory, ILoggerProvider loggerProvider) : Builder<ImporterNotificationScenario>
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
