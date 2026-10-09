namespace Defra.Imports.Scenarios.Events
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Defra.Imports.Scenarios.Fakers;
    using Microsoft.Extensions.Logging;
    using ScenarioBuilder;

    /// <summary>
    /// An event for the EU Imports Notifications Logic App submitting a new Importer Notification, as it does when it
    /// processes an IPAFFS notification message received on the Service Bus queue.
    /// </summary>
    /// <param name="eventId">The event ID.</param>
    /// <param name="clientFactory">The service client factory.</param>
    /// <param name="logger">The logger.</param>
    public class LogicAppSubmitsImporterNotificationEvent(string eventId, ServiceClientFactory clientFactory, ILogger<LogicAppSubmitsImporterNotificationEvent> logger)
        : Event(eventId)
    {
        private readonly ServiceClientFactory clientFactory = clientFactory;
        private readonly ILogger<LogicAppSubmitsImporterNotificationEvent> logger = logger;
        private ImporterNotificationFaker importerNotificationFaker;

        private ImporterNotificationFaker ImporterNotificationFaker
        {
            get
            {
                this.importerNotificationFaker ??= new ImporterNotificationFaker();

                return this.importerNotificationFaker;
            }
        }

        /// <inheritdoc/>
        public override async Task ExecuteAsync(ScenarioContext context)
        {
            this.logger.LogInformation("Submitting an Importer Notification.");

            var importerNotification = this.ImporterNotificationFaker.Generate();

            // Not disposed here: GetClientAsync returns a client owned and cached by the factory for the
            // scenario's lifetime, released when the factory itself is disposed at the end of the scenario.
            var client = await this.clientFactory.GetClientAsync(Persona.LogicApp);
            var importerNotificationId = await client.CreateAsync(importerNotification);
            importerNotification.Id = importerNotificationId;

            this.logger.LogInformation("Created Importer Notification {ImporterNotificationId}.", importerNotificationId);

            context.Set(nameof(LogicAppSubmitsImporterNotificationEvent), new Info { ImporterNotificationId = importerNotificationId, ImporterNotification = importerNotification });
        }

        /// <summary>
        /// The outputs captured by the <see cref="LogicAppSubmitsImporterNotificationEvent"/> event.
        /// </summary>
        public class Info
        {
            /// <summary>
            /// Gets or sets the ID of the created Importer Notification.
            /// </summary>
            public Guid ImporterNotificationId { get; set; }

            /// <summary>
            /// Gets or sets the created Importer Notification, as generated and submitted by this event (not re-retrieved from Dataverse).
            /// </summary>
            public defraimp_ImporterNotification ImporterNotification { get; set; }
        }

        /// <summary>
        /// A builder for the <see cref="LogicAppSubmitsImporterNotificationEvent"/> event.
        /// </summary>
        /// <param name="eventFactory">The event factory.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="constructorArgs">The constructor args.</param>
        public class Builder(EventFactory eventFactory, string eventId, object[] constructorArgs = null)
            : Builder<LogicAppSubmitsImporterNotificationEvent>(eventFactory, eventId, constructorArgs)
        {
            // Statuses per notification-schema.json's EconomicOperator.status definition.
            private static readonly string[] ConsignorStatuses = { "approved", "nonapproved", "suspended" };

            // Field name/type must match LogicAppSubmitsImporterNotificationEvent.importerNotificationFaker exactly - the base builder copies it across by name.
            private ImporterNotificationFaker importerNotificationFaker;

            /// <summary>
            /// Overrides the faker used to generate the Importer Notification.
            /// </summary>
            /// <param name="importerNotificationFaker">The faker to use.</param>
            /// <returns>The builder.</returns>
            public Builder WithImporterNotification(ImporterNotificationFaker importerNotificationFaker)
            {
                this.importerNotificationFaker = importerNotificationFaker;

                return this;
            }

            /// <summary>
            /// Configures the Importer Notification to have the given type.
            /// </summary>
            /// <param name="type">The type.</param>
            /// <returns>The builder.</returns>
            public Builder WithType(defraimp_importernotificationtype type)
            {
                this.importerNotificationFaker ??= new ImporterNotificationFaker();
                this.importerNotificationFaker.RuleFor(n => n.defraimp_type, f => type);

                return this;
            }

            /// <summary>
            /// Configures the Importer Notification to have the given purpose of consignment.
            /// </summary>
            /// <param name="purposeOfConsignment">The display value of the purpose of consignment.</param>
            /// <returns>The builder.</returns>
            public Builder WithPurposeOfConsignment(defraimp_purposeofconsignment purposeOfConsignment)
            {
                this.importerNotificationFaker ??= new ImporterNotificationFaker();

                this.importerNotificationFaker.RuleFor(n => n.defraimp_PurposeofConsignment, f => purposeOfConsignment);

                return this;
            }

            /// <summary>
            /// Configures the Importer Notification as importing from a charity, populating the Charity tab's consignor two fields.
            /// </summary>
            /// <returns>The builder.</returns>
            public Builder WithImportingFromCharity()
            {
                this.importerNotificationFaker ??= new ImporterNotificationFaker();
                this.importerNotificationFaker.RuleFor(n => n.defraimp_importingfromcharity, f => true);

                // The Charity tab is bound to the "consignor two" fields (see Charity_Tab in the Importer Notification FormXml).
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwotype, f => "charity");
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwostatus, f => f.PickRandom(ConsignorStatuses));
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwoindividualname, f => f.Name.FullName());
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwocompanyname, f => f.Company.CompanyName());
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwoaddressemail, f => f.Internet.Email());
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwoaddresstelephone, f => f.Phone.PhoneNumber());
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwoaddressuktelephone, f => f.Random.ReplaceNumbers("###########"));
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwointernationalphonecode, f => f.Random.ReplaceNumbers("##"));
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwointernationalphonenumber, f => f.Random.ReplaceNumbers("##########"));
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwoaddressaddressline1, f => f.Address.StreetAddress());
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwoaddressaddressline2, f => f.Address.SecondaryAddress());
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwoaddressaddressline3, f => f.Address.County());
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwoaddresscity, f => f.Address.City());
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwoaddresspostalzipcode, f => f.Address.ZipCode());
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwoapprovalnumber, f => f.Random.Replace("APP-#####"));
                this.importerNotificationFaker.RuleFor(n => n.defraimp_consignortwootheridentifier, f => f.Random.Replace("ID-#####"));

                return this;
            }
        }
    }
}
