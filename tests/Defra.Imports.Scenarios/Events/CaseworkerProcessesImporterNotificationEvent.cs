namespace Defra.Imports.Scenarios.Events
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Microsoft.Extensions.Logging;
    using ScenarioBuilder;

    /// <summary>
    /// An event for a caseworker processing an Importer Notification.
    /// </summary>
    /// <param name="eventId">The event ID.</param>
    /// <param name="clientFactory">The service client factory.</param>
    /// <param name="logger">The logger.</param>
    public class CaseworkerProcessesImporterNotificationEvent(string eventId, ServiceClientFactory clientFactory, ILogger<CaseworkerProcessesImporterNotificationEvent> logger)
        : Event(eventId)
    {
        private readonly ServiceClientFactory clientFactory = clientFactory;
        private readonly ILogger<CaseworkerProcessesImporterNotificationEvent> logger = logger;

        private Guid importerNotificationId;
        private bool? contactedDueToNonCompliance;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(ScenarioContext context)
        {
            if (this.importerNotificationId == Guid.Empty)
            {
                var notificationInfo = context.Get<LogicAppSubmitsImporterNotificationEvent.Info>(nameof(LogicAppSubmitsImporterNotificationEvent));
                this.importerNotificationId = notificationInfo?.ImporterNotificationId ?? Guid.Empty;
            }

            if (this.importerNotificationId == Guid.Empty)
            {
                throw new InvalidOperationException($"{nameof(importerNotificationId)} must be configured before executing {nameof(CaseworkerProcessesImporterNotificationEvent)}.");
            }

            if (this.contactedDueToNonCompliance.HasValue)
            {
                this.logger.LogInformation("Caseworker processing Importer Notification {ImporterNotificationId}.", this.importerNotificationId);

                var update = new defraimp_ImporterNotification
                {
                    Id = this.importerNotificationId,
                    defraimp_ContactedDuetoNonCompliance = this.contactedDueToNonCompliance.Value,
                };

                // Not disposed here: GetClientAsync returns a client owned and cached by the factory for the
                // scenario's lifetime, released when the factory itself is disposed at the end of the scenario.
                var client = await this.clientFactory.GetClientAsync(Persona.Caseworker);
                await client.UpdateAsync(update).ConfigureAwait(false);
            }

            context.Set(nameof(CaseworkerProcessesImporterNotificationEvent), new Info
            {
                ImporterNotificationId = this.importerNotificationId,
            });
        }

        /// <summary>
        /// Outputs captured by <see cref="CaseworkerProcessesImporterNotificationEvent"/>.
        /// </summary>
        public class Info
        {
            /// <summary>
            /// Gets or sets the Importer Notification ID.
            /// </summary>
            public Guid ImporterNotificationId { get; set; }
        }

        /// <summary>
        /// A builder for <see cref="CaseworkerProcessesImporterNotificationEvent"/>.
        /// </summary>
        /// <param name="eventFactory">The event factory.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="constructorArgs">The constructor args.</param>
        public class Builder(EventFactory eventFactory, string eventId, object[] constructorArgs = null)
            : Builder<CaseworkerProcessesImporterNotificationEvent>(eventFactory, eventId, constructorArgs)
        {
            private Guid importerNotificationId;
            private bool? contactedDueToNonCompliance;

            /// <summary>
            /// Sets the Importer Notification to process.
            /// </summary>
            /// <param name="importerNotificationId">The Importer Notification ID.</param>
            /// <returns>The builder.</returns>
            public Builder ForImporterNotification(Guid importerNotificationId)
            {
                this.importerNotificationId = importerNotificationId;

                return this;
            }

            /// <summary>
            /// Sets whether the Importer Notification has been contacted due to non-compliance.
            /// </summary>
            /// <param name="contacted">Whether contacted due to non-compliance.</param>
            /// <returns>The builder.</returns>
            public Builder WithContactedDueToNonCompliance(bool contacted)
            {
                this.contactedDueToNonCompliance = contacted;

                return this;
            }
        }
    }
}
