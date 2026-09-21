---
name: scenario-builder-implementation
description: "Implement or extend test scenario builders in Defra.Imports.Scenarios using the ScenarioBuilder NuGet package (github.com/ewingjm/scenario-builder). Use when creating a new Scenario/Event/CompositeEvent/Builder class, adding an ImportRecordScenario or similar scenario, wiring new test setup for integration or acceptance (Reqnroll) tests, or reviewing whether test data setup follows the builder pattern instead of hand-rolled record generation. Covers Event, CompositeEvent, Scenario, ComposeUsing ordering, ScenarioContext, and incremental BuildAsync."
---

# Scenario Builder Implementation

## Why this exists

Test data must be created the way production data is created — through the same API calls and business processes, never by seeding fields directly that a real action would derive. The [ScenarioBuilder](https://github.com/ewingjm/scenario-builder) package provides a builder-pattern framework for this:

- **Declarative tests** — a test configures only what's relevant to it; everything else runs with sensible defaults.
- **Single source of truth** — the sequence of steps in a business process lives in one `Scenario` class, so process changes touch one place instead of dozens of tests.
- **Low brittleness** — tests don't reference steps they don't care about, so unrelated process changes don't break unrelated tests.

All new scenario/event classes live in [Defra.Imports.Scenarios](../../../tests/Defra.Imports.Scenarios/) (`netstandard2.0`): events under the `Defra.Imports.Scenarios.Events` namespace, scenarios under `Defra.Imports.Scenarios`. See also the [integration-tests instruction file](../../instructions/integration-tests.instructions.md), `CONTRIBUTING.md` → "Scenario builder", and the canonical project guidance in [docs/testing/scenario-builder-reference.md](../../../docs/testing/scenario-builder-reference.md) for the repo-level policy this skill implements.

## Reference documentation

- [docs/testing/scenario-builder-reference.md](../../../docs/testing/scenario-builder-reference.md) — project-level ScenarioBuilder conventions, builder chains, and data-generation patterns.
- [docs/testing/acceptance/acceptance-testing-standard.md](../../../docs/testing/acceptance/acceptance-testing-standard.md) — how acceptance tests consume the shared scenario builders.
- [docs/testing/acceptance/power-playwright-reference.md](../../../docs/testing/acceptance/power-playwright-reference.md) — UI automation stack that consumes the data setup.

## Core concepts

This table is the single source of truth for the structural rules below — the procedure and checklist reference it rather than restating it.

| Concept                  | Purpose                                                                                                                                                                                                                                                                                                                                                                                                                                                                |
| ------------------------ | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| `Event`                  | A single actor action against the API (e.g. a user creating a record). Implements `ExecuteAsync(ScenarioContext context)`.                                                                                                                                                                                                                                                                                                                                             |
| `Event.Info`             | Nested class declaring the event's outputs as properties (e.g. `Guid ImportRecordId`). `ExecuteAsync` sets each via `context.Set(nameof(Info.PropertyName), value)` — never a hand-typed string.                                                                                                                                                                                                                                                                       |
| `Event.Builder`          | Nested `Builder<TEvent>` with `With*` overrides. **Field names/types must exactly match the event's fields** — the base builder copies configured values across by name (and type), not by position. **Value-type fields must be declared nullable** (`bool?`, `int?`, `Guid?`) on both builder and event, so the event can tell "explicitly configured" apart from "left at the type's default". Every overridable field needs a safe default when left unconfigured. |
| `CompositeEvent`         | A named business process composed of other events/composite events via `[ComposeUsing]`. Name it with a configurable-sounding verb ("Processes"), not one implying a fixed single step ("Completes").                                                                                                                                                                                                                                                                  |
| `CompositeEvent.Builder` | Nested `Builder<TCompositeEvent>` with one `By*` method per child, each calling `this.ConfigureEvent<TEvent, TEvent.Builder>(...)` — never inline logic. Only configured children run unless `.AndAllPreviousSteps()` / `.AndAllOtherSteps()` is used.                                                                                                                                                                                                                 |
| `Scenario`               | The test-facing entry point, composed via `[ComposeUsing]`. Nullable output properties (e.g. `Guid? ImportRecordId`) are populated automatically from `ScenarioContext` by matching the property name to its source `Info` property name.                                                                                                                                                                                                                              |
| `Scenario.Builder`       | Nested `Builder<TScenario>` with one configurator method per top-level `[ComposeUsing]` step, each calling `ConfigureEvent(...)`. Overrides `InitializeServices` to register dependencies (e.g. `ServiceClientFactory`) that events need injected.                                                                                                                                                                                                                     |
| `ScenarioContext`        | Shared bag passed between events. Earlier events `Set` values; later events `Get` them.                                                                                                                                                                                                                                                                                                                                                                                |
| `BuildAsync`             | Executes configured (and, at the scenario level, any un-configured-but-preceding) steps.                                                                                                                                                                                                                                                                                                                                                                               |

Full class templates: [references/templates.md](./references/templates.md).

## Generating Data with Bogus Fakers

Any records an event generates rather than derives from a configured override should come from a [`Bogus`](https://github.com/bchavez/Bogus) `Faker`, not a hardcoded value or ad hoc random logic. Each event exposes its `Faker` as a private, overridable field with a default instance, so:

- Tests get realistic, always-valid data for anything they don't explicitly configure.
- A test that cares about a specific value overrides it through the event's `Builder` rather than reaching into the `Faker`.
- Data generation rules live in one place per entity/record type, not duplicated across events or tests.

Conventions for every `Faker`:

- Default to the `en_GB` locale.
- Define rules that always produce a record valid enough to pass API validation — a `Faker` should never be the reason a test fails.
- A `Faker<TEntity>` targeting a Dataverse early-bound entity **must inherit from [`RecordFaker<TEntity>`](../../../tests/Defra.Imports.Scenarios/Fakers/RecordFaker.cs)** instead of `Faker<TEntity>` directly — it applies the `en_GB` locale and a randomly generated `Id` for every entity faker, so individual fakers only need to define rules for their own fields.
- For a `Faker<TEntity>` targeting a Dataverse early-bound entity, verify the exact fields it sets first (see below) — do not guess property names or constraints from the entity's logical name.

### Verifying entity fields before writing a Faker

Dataverse Model Builder casing is inconsistent per-attribute (e.g. `defraimp_ArrivalDate` vs `defraimp_departuredate` vs `defraimp_CommodityId`) and cannot be inferred from the logical (schema) name. Before writing `Faker` rules or direct field assignments for a Dataverse early-bound entity, verify the exact generated property name, CLR type, and `Entity.xml` constraints instead of guessing.

1. **Run the field metadata script** at [scripts/Get-DataverseEntityFieldMetadata.ps1](./scripts/Get-DataverseEntityFieldMetadata.ps1) using PowerShell 7 (`pwsh`), passing the entity's logical name (and, optionally, the specific fields you need via `-Fields`). It reads the entity's `Entity.xml` (plus any referenced `OptionSets/*.xml`) to report, per attribute: the generated C# property name (the attribute's `PhysicalName`, which is the exact casing Dataverse Model Builder uses), its CLR type, `Type`, `RequiredLevel`, `MaxLength`, `MinValue`/`MaxValue`, `Precision`, `ValidForCreateApi`, and option set values.
2. **Use the reported property name verbatim** in `RuleFor`/field-assignment expressions — do not assume it matches the logical name's casing.
3. **Respect the reported constraints** — clamp/truncate generated string values to `MaxLength`, keep numeric values within `MinValue`/`MaxValue`, and only assign an attribute if `ValidForCreateApi` is `1`.
4. **Re-run per entity, not per field.** The script reports every attribute by default (or a supplied subset via `-Fields`) in one pass — there's no need to look up fields one at a time.

## Procedure

1. **Identify the process, not the test.** Name the scenario/event after the real-world action (e.g. `ImportRecordScenario`, `UserSubmitsImportRecordEvent`), not after the test that happens to use it.
2. **Check for reuse first.** Search `Defra.Imports.Scenarios` for an existing `Event`, `CompositeEvent`, or `Scenario` that already covers the step you need. Extend it (add a `With*`/`By*` method or a new `[ComposeUsing]` step) rather than duplicating logic in a new class or in the test itself.
3. **Implement the leaf `Event`(s) first**, applying the `Event`/`Event.Info`/`Event.Builder` rules from Core concepts:
   - Constructor takes `eventId` plus any injected services (e.g. `ServiceClientFactory`, `ILogger<T>`), with compile-time `ComposeUsing` constructor args appearing _before_ the injected services.
   - `ExecuteAsync` performs the real API call (via `Microsoft.PowerPlatform.Dataverse.Client` / `Defra.Imports.Model` early-bound entities) that a real user/system action would perform — never write directly to fields a real process would derive.
   - Generate data via a `Faker` (see [Generating Data with Bogus Fakers](#generating-data-with-bogus-fakers)).
4. **Group multi-step processes into a `CompositeEvent`** when a business process is itself made of other events and callers may want to stop partway through (e.g. "a case has been assigned" vs "a case has been fully processed"). Use `[ComposeUsing(order, id, type)]` for each child.
5. **Define/extend the `Scenario` class** with `[ComposeUsing]` attributes declaring execution order, a nested `EventIds` static class for IDs, and output properties per the `Scenario` rules in Core concepts.
6. **Implement the `Scenario.Builder`** — one configurator method per top-level step (naming matches the event, e.g. `UserSubmitsImportRecord`). Override `InitializeServices` to register any services the events depend on (constructor-inject them into the builder, e.g. `ServiceClientFactory`).

## Quality checklist

- [ ] Every public class/member has an XML doc comment (StyleCop + `GenerateDocumentationFile` are enforced in `Defra.Imports.Scenarios`).
- [ ] `Event`/`CompositeEvent`/`Scenario` names describe the real business action, not the test that uses them.
- [ ] Reused an existing event/composite event/scenario instead of duplicating one where possible.
- [ ] `Builder` field names/types match their event's fields exactly, and every overridable field has a safe default when unconfigured.
- [ ] Value-type builder/event fields (`bool`, `int`, `Guid`, enums, etc.) are nullable, so "unconfigured" is distinguishable from an explicit default value.
- [ ] Event outputs are declared on a nested `Info` class and set via `context.Set(nameof(Info.X), value)` — no hand-typed context key strings.
- [ ] `Scenario` output properties are nullable and named to match the `Info` property they map from.
- [ ] `CompositeEvent.Builder`/`Scenario.Builder` configurator methods only call `ConfigureEvent(...)` — no inline business logic.
- [ ] No test-only shortcuts: events perform the same API call sequence a real user/system would.
- [ ] `Faker` classes default to `en_GB`, always produce valid records, and entity-backed ones inherit `RecordFaker<TEntity>` rather than `Faker<TEntity>` directly.
- [ ] Entity-backed `Faker` field names/casing, `MaxLength`, `RequiredLevel`, and range/option-set constraints were verified via [scripts/Get-DataverseEntityFieldMetadata.ps1](./scripts/Get-DataverseEntityFieldMetadata.ps1) rather than assumed from the logical name.
- [ ] New logic goes into `Defra.Imports.Scenarios`, not into `Marktek.Fluent.Testing.Engine`/`RecordGeneration` or ad-hoc test code (see [integration-tests instructions](../../instructions/integration-tests.instructions.md)).
