# Acceptance Testing Standard

Authoritative standard for writing acceptance tests on Power Platform
projects using this stack: a Reqnroll + Power Playwright acceptance test
project alongside a companion ScenarioBuilder + Bogus test-data project. It
is written for consumption by AI agents (e.g. a Quality Engineer agent)
authoring or reviewing acceptance tests.

For deep dives, see the companion documents:

- [common-testing-conventions.md](../common-testing-conventions.md) - assertions, defensive coding, error handling, and logging conventions shared with integration tests
- [scenario-builder-reference.md](../scenario-builder-reference.md) - test data setup (shared with integration tests)
- [power-playwright-reference.md](power-playwright-reference.md) - UI automation
- [recipes.md](recipes.md) - copy-paste implementation patterns
- [anti-patterns.md](anti-patterns.md) - things to avoid, with reasons
- [examples.md](examples.md) - curated end-to-end examples

## The mental model

Every acceptance test is composed of three concerns, kept deliberately separate:

1. **Gherkin** (e.g. `Features/**/*.feature`) - describes behaviour from a persona's
   point of view. No technical detail.
2. **Step definitions** (e.g. `StepDefinitions/*.cs`) - translate Gherkin into
   (a) data setup via ScenarioBuilder, or (b) UI interaction via Power
   Playwright.
3. **Scenario Builder** - creates the Dataverse state a scenario needs *without* 
   going through the UI, so tests only exercise the UI/behaviour actually under test.

Rule of thumb: **if a precondition can be setup directly via API, it
should be** (via ScenarioBuilder). Only use Power Playwright for the behaviour
actually being verified.

## Conventions

### Feature files

- One `.feature` file per **user-facing capability**, named as a verb phrase:
  `Create an order.feature`, `Activate a work item.feature`,
  `Reject a submission.feature`. Not named after backlog items e.g. Azure Boards 
  user stories or features.
- Files are grouped into folders **by business entity/domain**, e.g.
  `Features/Orders/`, `Features/Work items/`, `Features/Submissions/`.
- `Feature:` title matches the file name.
- `Scenario:` titles are persona-led narratives that match the verb phrase of
  the feature file with additional context appended: `Approver views a work
  item`, `Administrator creates an order with a discount`.
- `Scenario Outline:` + `Examples:` is used for **permutation testing** across
  entity variants/categories:

  ```gherkin
  Scenario Outline: Approver rejects a submission
      Given a submitter has submitted a '<Category>' <Request Type> request
      ...
      Examples:
          | Request Type | Category |
          | standard     | type a   |
          | expedited    | type b   |
  ```
- An alternative to a `Scenario Outline`, when strict coverage across all permutations 
  is excessive, is to have a single step randomise the permutations:

  ```gherkin
  Scenario: Approver rejects a submission
      Given a submitter has submitted a request of any type and category
      ...
  ```

### Tags

| Tag                                             | Meaning                                                                                    | Usage rule                                                                                                                                    |
| ----------------------------------------------- | ------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------- |
| `@issue:XXXXX`                                  | Traces a scenario back to its issue-tracker story/defect. Multiple per scenario is normal. | Add at least one per scenario when the work originates from a ticket.                                                                         |
| `@ignore`                                       | Disables a scenario (WIP or known-broken).                                                 | Treat as a signal of debt; do not leave `@ignore` on new work without a tracking issue.                                                       |
| `@clean-up:order` / other custom lifecycle tags | Triggers a matching `[AfterScenario("tag")]` cleanup hook.                                 | Only introduce a new lifecycle tag if a corresponding hook is added; keep the pairing discoverable by naming the tag after the hook's intent. |

### Given/When/Then wording

- **Given**: establishes state - "a portal user has submitted...", "I am
  logged in as...", "I have opened...", "the application's work order is
  assigned to me". Given steps that create data should delegate to
  ScenarioBuilder or direct Dataverse setup, never the UI.
- **When**: a single user action - "I click the '...' command", "I enter the
  following details", "I confirm the dialog", "I save the record".
- **Then/But**: an observable outcome - "I see the following fields", "I do
  not see the following commands", "the record is successfully saved". `But`
  is used idiomatically for a contrasting assertion in the same scenario, not
  a second independent assertion.
- Steps that take a table of expected values are strongly preferred over one
  step per field - see the "field values" recipe in
  [recipes.md](recipes.md).

### Test isolation

- Each scenario builds only the data it needs, scoped to newly-created
  records (via faked IDs/GUIDs) - never depends on another scenario's leftover
  state.
- Persona/user assignment goes through a **user pool** (`UserPoolClient`) so
  scenarios can run in parallel without colliding on the same Dataverse user.
- Read-only reference/master data (e.g. currencies, categories, locations) is seeded
  once as part of the seed data import during deployment, not per test run or per scenario,
  because it is immutable shared context rather than scenario-specific state. This approach
  runs once and enables both automated testing and manual testing.

### Assertions, defensive coding, error handling, and logging

These conventions are shared with integration tests and documented once in
[common-testing-conventions.md](../common-testing-conventions.md). The
acceptance-specific addition to that shared standard: prefer asserting on a
**table of expected values** in one step over many single-field assertion
steps (see `Then I see the following fields`), and see the retry recipe in
[recipes.md](recipes.md) for the logging convention applied to step
bindings.

## Reqnroll usage rules

- **One class per entity/domain**, not one class per Given/When/Then. All
  steps that manipulate a given entity (e.g. orders) live in
  `OrderSteps.cs`, regardless of whether they are Given, When, or Then
  steps.
- State is shared two ways, and both are acceptable for their purpose:
  - **`PowerPlaywrightContext`** (constructor-injected) - tracks the
    currently active Power Playwright page/session and active user. Use this
    for anything related to "where the browser currently is".
  - **`ScenarioContext`** (Reqnroll built-in, keyed via constants in
    `ScenarioContextKeys.cs`) - use this for domain data (IDs, scenario
    objects, captured values) that steps need to pass to later steps.
- Dependency injection is Reqnroll's built-in container (`ObjectContainer`),
  wired exclusively in `Hooks/*.cs` classes, not scattered across step
  classes. New shared services should be registered in `ContainerHooks.cs`
  alongside existing registrations, with a `BeforeTestRun`/`BeforeScenario`
  order chosen to sit correctly relative to existing hooks (configuration and
  DI registration hooks run first, using large negative `Order` values).
- Custom `[StepArgumentTransformation]`s convert Gherkin phrases/tables into
  domain types (e.g. `(can|cannot)` -> `bool`, a data table ->
  `IEnumerable<Journey>`). Add a transformation instead of parsing strings
  inline in a step method when the same conversion is needed more than once.

## Preferred / Acceptable / Discouraged summary

|                 | Pattern                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       |
| --------------- | ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| **Preferred**   | ScenarioBuilder for all data setup; Power Playwright abstractions for all UI interaction; FluentAssertions for all `Then` assertions (grouped with `AssertionScope` where multiple related assertions exist); plain, domain-language exceptions for defensive failures in `Given`/`When` steps; entity-centric step classes; table-driven Given/Then steps; real `ILogger` registrations (not `NullLogger.Instance`) for `Scenario`/`Event` builders, with each setup call logged; DI registrations centralised in `Hooks/*.cs`; `[StepArgumentTransformation]`s for recurring Gherkin-to-domain-type conversions. |
| **Acceptable**  | Direct `ServiceClient` calls (via `ServiceClientFactory`) for one-off reference/lookup data that has no corresponding builder event yet (e.g. seeding a reference/lookup record); `RetryExtensions.RetryUntilSucceedsAsync` retries around eventually-consistent conditions (e.g. waiting for an integration to update a record).                                                                                                                                                          |
| **Discouraged** | Direct Playwright APIs (`IPage.Keyboard`, `IPage.EvaluateAsync`, raw locators) bypassing Power Playwright control abstractions except for narrow, documented workarounds; `Task.Delay` as a waiting strategy; assertion library exceptions thrown from `Given`/`When` steps; `NullLogger.Instance` in place of a real logger; DI registrations scattered across step classes instead of `Hooks/*.cs`.                                                                                       |

## When authoring a new acceptance test

1. Find (or create) the feature folder matching the business entity.
2. Write the `Feature:`/`Scenario:` using persona-first language and existing
   step wording where possible (search `Steps/Playwright` before inventing new
   phrasing).
3. Prefer an existing ScenarioBuilder chain for all preconditions; only add a
   new builder `Event`/extension when no existing composition covers the
   need.
4. Only use Power Playwright steps for the action/assertion actually under
   test.
5. Tag with at least one `@issue:XXXXX` when applicable.
6. Run and confirm the scenario is independently runnable (no ordering
   dependency on other scenarios).
