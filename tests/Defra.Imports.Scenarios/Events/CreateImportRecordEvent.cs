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
        private string importRecordName;
        private IEnumerable<defraimp_noncompliancetype> nonComplianceTypes;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(ScenarioContext context)
        {
            if (this.importerNotificationId == Guid.Empty)
            {
                throw new InvalidOperationException($"{nameof(importerNotificationId)} must be configured before executing {nameof(CreateImportRecordEvent)}.");
            }

            this.logger.LogInformation("Creating Import Record from Importer Notification action for {ImporterNotificationId}.", this.importerNotificationId);

            var client = await this.clientFactory.GetClientAsync(Persona.LogicApp);
            await client.ExecuteAsync(new defraimp_CreateImportRecordFromNotificationRequest
            {
                Target = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, this.importerNotificationId),
            }).ConfigureAwait(false);

            var importRecord = await this.GetLatestImportRecordAsync(client).ConfigureAwait(false)
                ?? throw new InvalidOperationException($"No Import Record was created for Importer Notification '{this.importerNotificationId}' by defraimp_CreateImportRecordFromNotification.");

            if (!string.IsNullOrWhiteSpace(this.importRecordName) || this.nonComplianceTypes != null)
            {
                var update = new defraimp_importapplication
                {
                    Id = importRecord.Id,
                };

                if (!string.IsNullOrWhiteSpace(this.importRecordName))
                {
                    update.defraimp_name = this.importRecordName;
                    importRecord.defraimp_name = this.importRecordName;
                }

                if (this.nonComplianceTypes != null)
                {
                    update.defraimp_TypesofNonCompliance = this.nonComplianceTypes;
                    importRecord.defraimp_TypesofNonCompliance = this.nonComplianceTypes;
                }

                await client.UpdateAsync(update).ConfigureAwait(false);
            }

            var importRecordId = importRecord.Id;

            this.logger.LogInformation("Created Import Record {ImportRecordId}.", importRecordId);

            context.Set(nameof(CreateImportRecordEvent), new Info
            {
                ImportRecordId = importRecordId,
                ImportRecord = importRecord,
            });
        }

        private async Task<defraimp_importapplication> GetLatestImportRecordAsync(Microsoft.PowerPlatform.Dataverse.Client.ServiceClient client)
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

            for (var attempt = 0; attempt < 6; attempt++)
            {
                var result = await client.RetrieveMultipleAsync(query).ConfigureAwait(false);
                var entity = result.Entities.FirstOrDefault();

                if (entity != null)
                {
                    return entity.ToEntity<defraimp_importapplication>();
                }

                await Task.Delay(1000).ConfigureAwait(false);
            }

            return null;
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
            private string importRecordName;
            private IEnumerable<defraimp_noncompliancetype> nonComplianceTypes;

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

            /// <summary>
            /// Sets the Import Record name.
            /// </summary>
            /// <param name="name">The import record name.</param>
            /// <returns>The builder.</returns>
            public Builder WithName(string name)
            {
                this.importRecordName = name;

                return this;
            }

            /// <summary>
            /// Sets Import Record non-compliance types.
            /// </summary>
            /// <param name="types">The non-compliance types.</param>
            /// <returns>The builder.</returns>
            public Builder WithNonComplianceTypes(params defraimp_noncompliancetype[] types)
            {
                this.nonComplianceTypes = types;

                return this;
            }
        }
    }
}
