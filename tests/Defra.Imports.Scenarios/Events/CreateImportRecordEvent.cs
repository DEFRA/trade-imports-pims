namespace Defra.Imports.Scenarios.Events
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Microsoft.Extensions.Logging;
    using Microsoft.Xrm.Sdk;
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

            var importRecord = new defraimp_importapplication
            {
                Id = Guid.NewGuid(),
                defraimp_name = this.importRecordName ?? $"IR-{Guid.NewGuid():N}".Substring(0, 12),
                defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ImportNotification,
                defraimp_PrimaryImporterNotificationId = new EntityReference(defraimp_ImporterNotification.EntityLogicalName, this.importerNotificationId),
            };

            if (this.nonComplianceTypes != null)
            {
                importRecord.defraimp_TypesofNonCompliance = this.nonComplianceTypes;
                importRecord.defraimp_ContactedDuetoNonCompliance = false;
                importRecord.defraimp_NonComplianceStatus = defraimp_noncompliancestatus.InProgress;
            }

            this.logger.LogInformation("Creating Import Record linked to Importer Notification {ImporterNotificationId}.", this.importerNotificationId);

            var client = await this.clientFactory.GetClientAsync(Persona.LogicApp);
            var importRecordId = await client.CreateAsync(importRecord);
            importRecord.Id = importRecordId;

            this.logger.LogInformation("Created Import Record {ImportRecordId}.", importRecordId);

            context.Set(nameof(CreateImportRecordEvent), new Info
            {
                ImportRecordId = importRecordId,
                ImportRecord = importRecord,
            });
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
