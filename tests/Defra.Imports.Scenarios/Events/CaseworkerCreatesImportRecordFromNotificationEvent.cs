namespace Defra.Imports.Scenarios.Events
{
    using System;
    using System.Threading.Tasks;
    using Bogus;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;
    using ScenarioBuilder;

    /// <summary>
    /// An event for a caseworker creating an Import Record from an Importer Notification.
    /// </summary>
    /// <param name="eventId">The event ID.</param>
    /// <param name="clientFactory">The service client factory.</param>
    /// <param name="logger">The logger.</param>
    public class CaseworkerCreatesImportRecordFromNotificationEvent(string eventId, ServiceClientFactory clientFactory, ILogger<CaseworkerCreatesImportRecordFromNotificationEvent> logger)
        : Event(eventId)
    {
        private readonly ServiceClientFactory clientFactory = clientFactory;
        private readonly ILogger<CaseworkerCreatesImportRecordFromNotificationEvent> logger = logger;
        private defraimp_importapplication importRecordUpdate;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(ScenarioContext context)
        {
            var importerNotificationId = context.Get<LogicAppSubmitsImporterNotificationEvent.Info>(nameof(LogicAppSubmitsImporterNotificationEvent)).ImporterNotificationId;

            this.logger.LogInformation("Creating an Import Record from Importer Notification {ImporterNotificationId}.", importerNotificationId);

            var client = await this.clientFactory.GetClientAsync(Persona.Caseworker);

            await client.ExecuteAsync(new defraimp_CreateImportRecordFromNotificationRequest
            {
                Target = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, importerNotificationId),
            });

            var importRecordId = await client.RetrieveImportRecordCreatedFromNotificationAsync(importerNotificationId, this.logger);

            this.logger.LogInformation("Created Import Record {ImportRecordId}.", importRecordId);

            if (this.importRecordUpdate is not null)
            {
                this.importRecordUpdate.Id = importRecordId;
                await client.UpdateAsync(this.importRecordUpdate);
            }

            context.Set(nameof(CaseworkerCreatesImportRecordFromNotificationEvent), new Info { ImportRecordId = importRecordId });
        }

        /// <summary>
        /// The outputs captured by the <see cref="CaseworkerCreatesImportRecordFromNotificationEvent"/> event.
        /// </summary>
        public class Info
        {
            /// <summary>
            /// Gets or sets the ID of the created Import Record.
            /// </summary>
            public Guid ImportRecordId { get; set; }
        }

        /// <summary>
        /// A builder for the <see cref="CaseworkerCreatesImportRecordFromNotificationEvent"/> event.
        /// </summary>
        /// <param name="eventFactory">The event factory.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="constructorArgs">The constructor args.</param>
        public class Builder(EventFactory eventFactory, string eventId, object[] constructorArgs = null)
            : Builder<CaseworkerCreatesImportRecordFromNotificationEvent>(eventFactory, eventId, constructorArgs)
        {
            // Field name/type must match CaseworkerCreatesImportRecordFromNotificationEvent.importRecordUpdate exactly - the base builder copies it across by name.
            private defraimp_importapplication importRecordUpdate;

            /// <summary>
            /// Configures the Import Record to have the given Import Record Type once created.
            /// </summary>
            /// <param name="importRecordType">The Import Record Type.</param>
            /// <returns>The builder.</returns>
            public Builder WithImportRecordType(defraimp_importapplication_defraimp_importapplicationtype importRecordType)
            {
                this.ImportRecordUpdate.defraimp_ImportApplicationType = importRecordType;

                return this;
            }

            /// <summary>
            /// Configures the Import Record to be at the Triage stage with a Commodity Code once created.
            /// </summary>
            /// <param name="commodityCode">The Commodity Code.</param>
            /// <returns>The builder.</returns>
            public Builder AtTriageStageWithCommodityCode(string commodityCode)
            {
                this.ImportRecordUpdate.statuscode = defraimp_importapplication_statuscode.Triage;
                this.ImportRecordUpdate.defraimp_CommodityCode = commodityCode;

                return this;
            }

            /// <summary>
            /// Configures the Import Record to have unique values in each of its searchable fields once created.
            /// </summary>
            /// <returns>The builder.</returns>
            public Builder WithSearchableFields()
            {
                var token = Guid.NewGuid().ToString("N").Substring(0, 12);

                this.ImportRecordUpdate.defraimp_ImporterContactName = $"Importer {token}";

                // A random historic date, so that few other Import Records share it.
                this.ImportRecordUpdate.defraimp_DateofImport = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddDays(new Faker().Random.Int(0, 3650));
                this.ImportRecordUpdate.defraimp_PlaceofOriginOrganisation = $"Origin {token}";
                this.ImportRecordUpdate.defraimp_ITAHCReference = $"ITAHC-{token}";
                this.ImportRecordUpdate.defraimp_LocalReferenceNumber = $"LREF-{token}";

                return this;
            }

            private defraimp_importapplication ImportRecordUpdate => this.importRecordUpdate ??= new defraimp_importapplication();
        }
    }
}
