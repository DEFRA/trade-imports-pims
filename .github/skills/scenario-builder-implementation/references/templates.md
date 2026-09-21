# Scenario Builder Templates

Worked example: an `ImportRecordScenario` covering a user submitting an import record. Adjust namespaces, entities (`Defra.Imports.Model`), and API calls to the real Dataverse process — this is structural, not business-accurate, guidance.

## 1. Leaf event

```csharp
namespace Defra.Imports.Scenarios.Events
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Microsoft.Extensions.Logging;
    using ScenarioBuilder;

    /// <summary>
    /// An event for a user submitting an import record.
    /// </summary>
    /// <param name="eventId">The event ID.</param>
    /// <param name="clientFactory">The service client factory.</param>
    /// <param name="logger">The logger.</param>
    public class UserSubmitsImportRecordEvent(string eventId, ServiceClientFactory clientFactory, ILogger<UserSubmitsImportRecordEvent> logger)
        : Event(eventId)
    {
        private readonly ServiceClientFactory clientFactory = clientFactory;
        private readonly ILogger<UserSubmitsImportRecordEvent> logger = logger;
        private ImportRecordFaker importRecordFaker = new ImportRecordFaker();
        private bool? isPriorityLane;

        /// <inheritdoc/>
        public override async Task ExecuteAsync(ScenarioContext context)
        {
            this.logger.LogInformation("Submitting an import record.");

            var importRecord = this.importRecordFaker.Generate();
            importRecord.IsPriorityLane = this.isPriorityLane ?? false;

            using var client = this.clientFactory.GetClient(Persona.Importer);
            var importRecordId = await client.CreateAsync(importRecord);

            this.logger.LogInformation("Created import record {ImportRecordId}.", importRecordId);

            context.Set(nameof(Info.ImportRecordId), importRecordId);
        }

        /// <summary>
        /// The outputs captured by the <see cref="UserSubmitsImportRecordEvent"/> event.
        /// </summary>
        public class Info
        {
            /// <summary>
            /// Gets or sets the ID of the created import record.
            /// </summary>
            public Guid ImportRecordId { get; set; }
        }

        /// <summary>
        /// A builder for the <see cref="UserSubmitsImportRecordEvent"/> event.
        /// </summary>
        /// <param name="eventFactory">The event factory.</param>
        /// <param name="eventId">The event ID.</param>
        /// <param name="constructorArgs">The constructor args.</param>
        public class Builder(EventFactory eventFactory, string eventId, object[] constructorArgs = null)
            : Builder<UserSubmitsImportRecordEvent>(eventFactory, eventId, constructorArgs)
        {
            // Field name/type must match UserSubmitsImportRecordEvent.importRecordFaker exactly - the base builder copies it across by name.
            private ImportRecordFaker importRecordFaker;

            // bool? (not bool) so the event can tell "not configured" apart from an explicit false.
            private bool? isPriorityLane;

            /// <summary>
            /// Overrides the faker used to generate the import record.
            /// </summary>
            /// <param name="importRecordFaker">The faker to use.</param>
            /// <returns>The builder.</returns>
            public Builder WithImportRecord(ImportRecordFaker importRecordFaker)
            {
                this.importRecordFaker = importRecordFaker;

                return this;
            }

            /// <summary>
            /// Overrides whether the import record is submitted in the priority lane.
            /// </summary>
            /// <param name="isPriorityLane">Whether the import record is in the priority lane.</param>
            /// <returns>The builder.</returns>
            public Builder WithIsPriorityLane(bool isPriorityLane)
            {
                this.isPriorityLane = isPriorityLane;

                return this;
            }
        }
    }
}
```

## 2. Scenario

```csharp
namespace Defra.Imports.Scenarios
{
    using System;
    using Defra.Imports.Scenarios.Events;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using ScenarioBuilder;

    /// <summary>
    /// A scenario for an import record.
    /// </summary>
    [ComposeUsing(0, EventIds.ImportRecordSubmission, typeof(UserSubmitsImportRecordEvent))]
    public class ImportRecordScenario : Scenario
    {
        /// <summary>
        /// Gets the ID of the created import record.
        /// </summary>
        /// <remarks>
        /// Named to match <see cref="UserSubmitsImportRecordEvent.Info.ImportRecordId"/> so it is populated automatically from <see cref="ScenarioContext"/>.
        /// </remarks>
        public Guid? ImportRecordId { get; internal set; }

        /// <summary>
        /// The event IDs for the events within the <see cref="ImportRecordScenario"/>.
        /// </summary>
        public static class EventIds
        {
            /// <summary>
            /// The event ID of the import record submission event.
            /// </summary>
            public const string ImportRecordSubmission = nameof(ImportRecordSubmission);
        }

        /// <summary>
        /// A builder for the <see cref="ImportRecordScenario"/>.
        /// </summary>
        /// <param name="clientFactory">A client factory.</param>
        /// <param name="loggerProvider">
        /// An <see cref="ILoggerProvider"/> used to route the typed loggers injected into events (e.g. <see cref="ILogger{TCategoryName}"/>)
        /// to a test framework's output. Defaults to <see cref="NullLoggerProvider"/> (no output) when not supplied.
        /// </param>
        public class Builder(ServiceClientFactory clientFactory, ILoggerProvider loggerProvider = null) : Builder<ImportRecordScenario>
        {
            private readonly ServiceClientFactory clientFactory = clientFactory;
            private readonly ILoggerProvider loggerProvider = loggerProvider ?? NullLoggerProvider.Instance;

            /// <summary>
            /// Configures the user submitting an import record event.
            /// </summary>
            /// <param name="configurator">The configurator.</param>
            /// <returns>The builder.</returns>
            public Builder UserSubmitsImportRecord(Action<UserSubmitsImportRecordEvent.Builder> configurator = null)
            {
                this.ConfigureEvent<UserSubmitsImportRecordEvent, UserSubmitsImportRecordEvent.Builder>(EventIds.ImportRecordSubmission, configurator);

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
```

## 3. Composite event (only when a process has multiple, independently-stoppable steps)

```csharp
namespace Defra.Imports.Scenarios.Events
{
    using System;
    using ScenarioBuilder;

    /// <summary>
    /// A composite event for processing an import record through to a decision.
    /// </summary>
    /// <param name="eventsFactory">The event factory.</param>
    /// <param name="eventId">The ID of the event.</param>
    [ComposeUsing(0, EventIds.ImportRecordSubmission, typeof(UserSubmitsImportRecordEvent))]
    [ComposeUsing(1, EventIds.ImportRecordDecision, typeof(OfficerRecordsDecisionEvent))]
    public class OfficerProcessesImportRecordEvent(EventFactory eventsFactory, string eventId)
        : CompositeEvent(eventsFactory, eventId)
    {
        /// <summary>
        /// The event IDs for the events within the <see cref="OfficerProcessesImportRecordEvent"/>.
        /// </summary>
        public static class EventIds
        {
            /// <summary>
            /// The event ID of the import record submission event.
            /// </summary>
            public const string ImportRecordSubmission = nameof(ImportRecordSubmission);

            /// <summary>
            /// The event ID of the import record decision event.
            /// </summary>
            public const string ImportRecordDecision = nameof(ImportRecordDecision);
        }

        /// <summary>
        /// A builder for the <see cref="OfficerProcessesImportRecordEvent"/>.
        /// </summary>
        /// <param name="eventBuilderFactory">The event builder factory.</param>
        /// <param name="eventFactory">The event factory.</param>
        /// <param name="eventId">The ID of the event.</param>
        public class Builder(EventBuilderFactory eventBuilderFactory, EventFactory eventFactory, string eventId)
            : Builder<OfficerProcessesImportRecordEvent>(eventBuilderFactory, eventFactory, eventId)
        {
            /// <summary>
            /// Configures the submission of the import record.
            /// </summary>
            /// <param name="configurator">The configurator.</param>
            /// <returns>The builder.</returns>
            public Builder ByUserSubmittingImportRecord(Action<UserSubmitsImportRecordEvent.Builder> configurator = null)
            {
                this.ConfigureEvent<UserSubmitsImportRecordEvent, UserSubmitsImportRecordEvent.Builder>(EventIds.ImportRecordSubmission, configurator);

                return this;
            }

            /// <summary>
            /// Configures the recording of a decision on the import record.
            /// </summary>
            /// <param name="configurator">The configurator.</param>
            /// <returns>The builder.</returns>
            public Builder ByRecordingDecision(Action<OfficerRecordsDecisionEvent.Builder> configurator = null)
            {
                this.ConfigureEvent<OfficerRecordsDecisionEvent, OfficerRecordsDecisionEvent.Builder>(EventIds.ImportRecordDecision, configurator);

                return this;
            }
        }
    }
}
```

## 4. Usage

```csharp
// Fully processed, default data.
var scenario = await this.scenarioBuilder
    .UserSubmitsImportRecord()
    .BuildAsync();

// Only as far as submission, with specific data.
var importRecordFaker = new ImportRecordFaker();
importRecordFaker.RuleFor(x => x.Commodity, "Live animals");

var scenario = await this.scenarioBuilder
    .UserSubmitsImportRecord(b => b
        .WithImportRecord(importRecordFaker))
    .BuildAsync();

// Incremental (e.g. across Reqnroll Given steps), caching the scenario in context. Note: imaginary event method for illustrative purposes only.
var scenario = await this.scenarioBuilder
    .UserUpdatesImportRecord()
    .BuildAsync(existingScenario);
```
