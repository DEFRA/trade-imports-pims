namespace Defra.Imports.Scenarios.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using ScenarioBuilder;

    /// <summary>
    /// An event for creating an Import Record linked to an Importer Notification.
    /// </summary>
    /// <param name="eventId">The event ID.</param>
    /// <param name="clientFactory">The service client factory.</param>
    /// <param name="logger">The logger.</param>
    public class CreateImportRecordEvent(string eventId, ServiceClientFactory clientFactory, ILogger<CreateImportRecordEvent> logger)
        : Event(eventId)
    {
        private readonly ServiceClientFactory clientFactory = clientFactory;
        private readonly ILogger<CreateImportRecordEvent> logger = logger;

        private Guid importerNotificationId;

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
                throw new InvalidOperationException($"{nameof(importerNotificationId)} must be configured before executing {nameof(CreateImportRecordEvent)}.");
            }

            this.logger.LogInformation("Creating Import Record from Importer Notification action for {ImporterNotificationId}.", this.importerNotificationId);

            // Not disposed here: GetClientAsync returns a client owned and cached by the factory for the
            // scenario's lifetime, released when the factory itself is disposed at the end of the scenario.
            var client = await this.clientFactory.GetClientAsync(Persona.Caseworker);
            await client.ExecuteAsync(new defraimp_CreateImportRecordFromNotificationRequest
            {
                Target = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, this.importerNotificationId),
            }).ConfigureAwait(false);

            var importRecord = await this.GetImportRecordAsync(client).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"No Import Record was created for Importer Notification '{this.importerNotificationId}' by defraimp_CreateImportRecordFromNotification.");

            var importRecordId = importRecord.Id;

            this.logger.LogInformation("Created Import Record {ImportRecordId}.", importRecordId);

            context.Set(nameof(CreateImportRecordEvent), new Info
            {
                ImportRecordId = importRecordId,
                ImportRecord = importRecord,
            });
        }

        private async Task<defraimp_importapplication> GetImportRecordAsync(Microsoft.PowerPlatform.Dataverse.Client.ServiceClient client)
        {
            var query = new QueryExpression(defraimp_importapplication.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(true),
                TopCount = 1,
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression(defraimp_importapplication.Fields.defraimp_PrimaryImporterNotificationId, ConditionOperator.Equal, this.importerNotificationId),
                    },
                },
            };

            query.Orders.Add(new OrderExpression("createdon", OrderType.Descending));

            var result = await client.RetrieveMultipleAsync(query).ConfigureAwait(false);
            var entity = result.Entities.FirstOrDefault();

            return entity?.ToEntity<defraimp_importapplication>();
        }

        /// <summary>
        /// Outputs captured by <see cref="CreateImportRecordEvent"/>.
        /// </summary>
        public class Info
        {
            /// <summary>
            /// Gets or sets the created Import Record ID.
            /// </summary>
            public Guid ImportRecordId { get; set; }

            /// <summary>
            /// Gets or sets the created Import Record.
            /// </summary>
            public defraimp_importapplication ImportRecord { get; set; }
        }

        /// <summary>
        /// A builder for <see cref="CreateImportRecordEvent"/>.
        /// </summary>
        /// <param name="eventFactory">The event factory.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="constructorArgs">The constructor args.</param>
        public class Builder(EventFactory eventFactory, string eventId, object[] constructorArgs = null)
            : Builder<CreateImportRecordEvent>(eventFactory, eventId, constructorArgs)
        {
            private Guid importerNotificationId;

            /// <summary>
            /// Sets the Importer Notification to link from the created Import Record.
            /// </summary>
            /// <param name="importerNotificationId">The Importer Notification ID.</param>
            /// <returns>The builder.</returns>
            public Builder ForImporterNotification(Guid importerNotificationId)
            {
                this.importerNotificationId = importerNotificationId;

                return this;
            }

        }
    }
}
