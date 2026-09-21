# Scenario Builder Reference

How Power Platform test-data projects use the `ScenarioBuilder` NuGet
package to set up test data. See the
[upstream docs](https://github.com/ewingjm/scenario-builder) for the raw
API; this document describes conventions for using it consistently on a
project.

## Core vocabulary (as used here)

- **Event** - an atomic action performed by a persona, e.g.
  `SubmitterSubmitsRequestEvent`, `ApproverAssignsWorkItemEvent`. Lives in
  `Scenarios/Events/<Persona>/`.
- **Composite event** - an event composed of other events via
  `[ComposeUsing]`, giving a higher-level action with drill-down control,
  e.g. `ApproverProcessesWorkItemEvent` composes
  `ApproverAssignsWorkItemEvent` + `ApproverProcessesWorkItemTasksEvent`.
- **Scenario** - the top-level orchestration of a full journey, e.g.
  `RequestScenario`, `NotificationScenario`. Declares its events with
  `[ComposeUsing]` and exposes each event's captured output as a property
  (e.g. `SubmitterSubmitsRequestEvent.Info`).
- **Builder** - a nested class on every Event/CompositeEvent/Scenario that
  exposes fluent `With...`/`By...` configuration methods and is the only way
  steps configure a scenario.

## Spacing and indentation conventions

Use standard C# formatting throughout the project.

- Keep fluent builder chains aligned vertically, with each chained method on
  its own line and the leading `.` at the start of the continuation line.
- For nested builders, add one extra indentation level per nesting
  level rather than crowding multiple calls onto a single line.
- Leave one blank line between logical sections of a step or scenario setup,
  but do not add extra padding within a single method chain unless it improves
  readability.

Example:

```csharp
var scenario = await this.scenarioBuilder
    .SubmitterSubmitsRequest(a => a
        .WithOneOfTheFollowingTypes(RequestType.Standard)
        .WithOneOfTheFollowingCategories(Category.TypeA))
    .ApproverProcessesWorkItem(a => a
        .ByAssigningWorkItem(b => b
            .WithAssignee(this.powerPlaywrightCtx.ActiveUserId))
        .ByProcessingWorkItemTasks(c => c
            .ByProcessingReviewTask(d => d
                .ByScheduling()
                .AndAllPreviousSteps())
            .AndAllPreviousSteps())
        .AndAllPreviousSteps())
    .BuildAsync();
```

This keeps the builder flow easy to scan.

## Preferred builder chains

### Simple, single-event setup

```csharp
[Given("a submitter has submitted a request of one of the following types")]
public async Task GivenASubmitterHasSubmittedARequestOfOneOfTheFollowingTypes(
    IEnumerable<RequestType> requestTypes)
{
    var scenario = await this.scenarioBuilder
        .SubmitterSubmitsRequest(a => a
            .WithOneOfTheFollowingTypes(requestTypes.ToArray()))
        .BuildAsync();

    this.scenarioContext.Set(scenario);
}
```

### Deep, multi-event setup (drill into composite events)

```csharp
var scenario = await this.scenarioBuilder
    .SubmitterSubmitsRequest(a => a
        .WithOneOfTheFollowingCategories(Category.TypeA)
        .WithOneOfTheFollowingTypes(RequestType.Standard))
    .ApproverProcessesWorkItem(a => a
        .ByProcessingWorkItemTasks(b => b
            .ByProcessingReviewTask(c => c
                .ByCreatingRecord()
                .AndAllPreviousSteps())
            .AndAllPreviousSteps())
        .AndAllPreviousSteps())
    .BuildAsync();
```

`AndAllPreviousSteps()` is required on a composite event's builder when you
want its earlier (unconfigured) sibling events to still run - composite
event builders only run explicitly-configured events by default. `AndAllOtherSteps()` will execute all earlier and later unconfigured sibling events.

### Incremental / resuming an existing scenario

Use `BuildAsync(scenario)` to keep building on a scenario object captured
earlier in the test (common when a Given step in one method needs to add
more history to a scenario built by a previous step):

```csharp
if (!this.scenarioContext.TryGetValue<RequestScenario>(out var scenario))
{
    scenario = await this.scenarioBuilder
        .ApproverProcessesWorkItem(a => a
            .ByAssigningWorkItem(b => b
                .WithAssignee(this.powerPlaywrightCtx.ActiveUserId))
            .ByProcessingWorkItemTasks(c => c
                .ByProcessingReviewTask(d => d
                    .ByScheduling()
                    .AndAllPreviousSteps()))
            .AndAllPreviousSteps())
        .BuildAsync();
}
else
{
    scenario = await this.scenarioBuilder
        .ApproverProcessesWorkItem(a => a
            .ByAssigningWorkItem(b => b
                .WithAssignee(this.powerPlaywrightCtx.ActiveUserId))
            .ByProcessingWorkItemTasks(c => c
                .ByProcessingReviewTask(d => d
                    .ByScheduling()
                    .AndAllPreviousSteps())
                .AndAllPreviousSteps())
            .AndAllPreviousSteps())
        .BuildAsync(scenario);
}

this.scenarioContext.Set(scenario);
```

Always check `ScenarioContext` for an existing scenario before building a
new one from scratch, so multiple `Given` steps in the same scenario compose
rather than create duplicate/conflicting records.

### Correlating two scenarios (sharing generated values)

```csharp
var secondScenario = await this.scenarioBuilder
    .SubmitterSubmitsRequest(a => a
        .WithOneOfTheFollowingTypes(RequestType.Standard)
        .WithOneOfTheFollowingCategories(Category.TypeA)
        .WithRules(b => b
            .RuleFor(c => c.new_submitter, firstScenario.SubmitterSubmitsRequestEvent.RequestOnSubmitting.new_submitter)
            .RuleFor(c => c.new_destinationid, firstScenario.SubmitterSubmitsRequestEvent.RequestOnSubmitting.new_destinationid))
        .WithFollowUpRequest())
    .BuildAsync();
```

`WithRules` overrides/extends the underlying Bogus faker rules directly -
useful for tying two otherwise-independent scenarios to the same related
entity (e.g. the same submitter or destination).

## Fakers (test data generation)

- Built on **Bogus** (`Faker<T>`), locale matching your target audience
  (e.g. `en_GB`).
- Base class: `RecordFaker<TEntity> : Faker<TEntity>` sets `Id` and any
  entity-wide defaults.
- Fakers are organised by entity folder (`Fakers/Requests`,
  `Fakers/WorkItems`, `Fakers/LineItems`, ...) with an abstract base per
  entity (e.g. `RequestFaker`) and concrete subclasses per business variant
  (e.g. `StandardRequestFaker`, `ExpeditedReviewRequestFaker`).
- Fakers compose: `RequestFaker` takes a `WorkItemFaker` in its constructor,
  which in turn takes a `LineItemFaker`. Event builders expose
  `With...Overrides(Action<TFaker> configure)` methods so a step can reach
  into a nested faker without the builder needing a method for every
  possible field.
- `RuleSetNames` (in the test-data project root) hold constants for named
  Bogus rule sets (e.g. `RuleSetNames.Submitter.Preferred`,
  `RuleSetNames.LineItem.Draft`) used to `Generate("ruleSetName")` a variant
  instead of the default.

## Model types

`Model/` holds test-only enums/DTOs that are *not* Dataverse entities:
`RequestType`, `Category`, `Journey`, `CheckType`/`CheckResult`, `TaskType`,
`TimeEntry`, `FormType`. Use these types (not raw strings) as builder/step
parameters - they are what Gherkin table/value transformations produce.

## Persona and security

- `Persona` enum represents the actor a Dataverse operation runs as (e.g.
  `Approver`, `Submitter`, `Administrator`).
- `ServiceClientFactory.GetClient(Persona)` returns a `ServiceClient`
  impersonating a pooled user for that persona; `GetAppUserClient()` returns
  the application user's own client (no impersonation).
- Events and builders take an `IServiceClientFactory`/`ServiceClientFactory`
  dependency (registered in `InitializeServices`) rather than a single
  `ServiceClient`, so each event can act as the correct persona.

## Sharing state with Reqnroll

- Store the built scenario in `ScenarioContext` immediately after
  `BuildAsync()`: `this.scenarioContext.Set(scenario)`.
- Retrieve it later with `this.scenarioContext.Get<TScenario>()` (or
  `TryGetValue<T>` for optional retrieval).
- Read captured outputs off the scenario's event properties, e.g.
  `scenario.SubmitterSubmitsRequestEvent.RequestOnSubmission`, rather than
  re-querying Dataverse for values the builder already captured.

## Logging

- Register `Scenario`/`Event` builders with a real `ILogger`
  (`Microsoft.Extensions.Logging`) wired to the test framework's output,
  rather than `NullLogger.Instance` - this makes data-setup actions
  diagnosable from CI logs, not just local runs.
- Prefer structured log messages
  (`this.logger.LogInformation("Creating {Entity} for {Persona}", ...)`)
  over string concatenation, consistent with the rest of the codebase's
  logging conventions.

## Preferred / Acceptable / Discouraged

|                 | Pattern                                                                                                                                                                                                                                                                                                                            |
| --------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Preferred**   | Compose scenarios via `scenarioBuilder.<Event>(a => a....).BuildAsync()`; drill into composite events with `AndAllPreviousSteps()`; store/resume the built scenario via `ScenarioContext`; configure fakers via builder `With...` methods; use `RuleSetNames` for named variants; register builders with a real `ILogger`.         |
| **Acceptable**  | Direct `ServiceClient.Create/Update/Execute` calls (via `ServiceClientFactory`) for one-off reference/lookup data that has no corresponding scenario event (e.g. seeding a reference-data faker record directly) - but prefer adding a builder event if the same setup recurs across features.                                     |
| **Discouraged** | Building the same persona journey by hand with direct `ServiceClient` calls when an equivalent `Event`/`Scenario` builder already exists - creates duplicate, harder-to-maintain setup logic and bypasses captured scenario outputs other steps rely on; registering builders with `NullLogger.Instance` instead of a real logger. |
