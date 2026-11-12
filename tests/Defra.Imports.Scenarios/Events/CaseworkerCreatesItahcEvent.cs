namespace Defra.Imports.Scenarios.Events
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Microsoft.Extensions.Logging;
    using ScenarioBuilder;

    /// <summary>
    /// An event for a caseworker creating an ITAHC that can be selected as an Import Record's Primary HC.
    /// </summary>
    /// <param name="eventId">The event ID.</param>
    /// <param name="clientFactory">The service client factory.</param>
    /// <param name="logger">The logger.</param>
    public class CaseworkerCreatesItahcEvent(string eventId, ServiceClientFactory clientFactory, ILogger<CaseworkerCreatesItahcEvent> logger)
        : Event(eventId)
    {
        private readonly ServiceClientFactory clientFactory = clientFactory;
        private readonly ILogger<CaseworkerCreatesItahcEvent> logger = logger;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(ScenarioContext context)
        {
            var itahc = new defraimp_itahc
            {
                defraimp_name = $"ITAHC {Guid.NewGuid():N}".Substring(0, 18),
                defraimp_TRACESReceivedDate = DateTime.UtcNow.Date,
                defraimp_CommodityCode = "0101210000",
            };

            var client = await this.clientFactory.GetClientAsync(Persona.Caseworker);

            this.logger.LogInformation("Creating ITAHC {ItahcName} with Commodity Code {CommodityCode}.", itahc.defraimp_name, itahc.defraimp_CommodityCode);

            var itahcId = await client.CreateAsync(itahc);

            this.logger.LogInformation("Created ITAHC {ItahcId}.", itahcId);

            context.Set(nameof(CaseworkerCreatesItahcEvent), new Info { ItahcId = itahcId, Name = itahc.defraimp_name });
        }

        /// <summary>
        /// The outputs captured by the <see cref="CaseworkerCreatesItahcEvent"/> event.
        /// </summary>
        public class Info
        {
            /// <summary>
            /// Gets or sets the ID of the created ITAHC.
            /// </summary>
            public Guid ItahcId { get; set; }

            /// <summary>
            /// Gets or sets the name of the created ITAHC.
            /// </summary>
            public string Name { get; set; }
        }

        /// <summary>
        /// A builder for the <see cref="CaseworkerCreatesItahcEvent"/> event.
        /// </summary>
        /// <param name="eventFactory">The event factory.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="constructorArgs">The constructor args.</param>
        public class Builder(EventFactory eventFactory, string eventId, object[] constructorArgs = null)
            : Builder<CaseworkerCreatesItahcEvent>(eventFactory, eventId, constructorArgs)
        {
        }
    }
}
