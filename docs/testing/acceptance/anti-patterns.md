# Anti-Patterns

Common anti-patterns to avoid on acceptance test projects using this stack,
why each should be avoided, and what to do instead. Treat presence of these
patterns in existing code as debt to avoid *repeating*, not necessarily as
an immediate mandate to refactor unrelated code.

## 1. Direct `ServiceClient` calls where a ScenarioBuilder event exists

**Observed:** Steps calling `serviceClient.CreateAsync`/`UpdateAsync`/
`Execute` directly for entities that already have a corresponding
`Event`/`Scenario` builder (e.g. directly updating an order or creating an
account inline in a step, instead of composing the equivalent builder call).

**Why avoid it:** bypasses the captured scenario outputs
(`scenario.SomeEvent.Info`) later steps rely on, duplicates setup logic that
already exists in a tested, reusable form, and makes it harder to reason
about *why* a record is in a given state (no named `Event` to read).

**Instead:** find or extend the relevant `Event`/`Scenario` builder. If no
builder covers the need and it is truly one-off reference data, direct
`ServiceClient` use is acceptable (see [recipes.md](recipes.md)) - but if the
same direct-call snippet is duplicated across step files, promote it to a
builder event.

## 2. Direct Playwright API usage bypassing Power Playwright abstractions

**Observed:** `this.RecordPage.Page.Keyboard.PressAsync(...)`,
`this.RecordPage.Page.EvaluateAsync<string>("() => Xrm...")`,
`Page.GetByText(...).WaitForAsync(...)` used directly in step definitions
alongside Power Playwright's typed control API.

**Why avoid it:** raw locators/selectors are brittle to platform UI changes
in a way Power Playwright's abstraction is designed to insulate against; it
reintroduces exactly the maintenance cost Power Playwright exists to remove.

**Instead:** use the Power Playwright control/page abstraction
(`GetField<T>`, `GetControl<T>`, `CommandBar`, dialog types). Only drop to
raw Playwright for genuinely unsupported operations (e.g. pressing `Tab` to
commit a field, dismissing a flyout with `Escape`), and keep such usages
narrow and commented with *why* no abstraction exists.

## 3. `Task.Delay` as a waiting strategy

**Observed:** a small number of steps use `await Task.Delay(10000);` as a
fallback wait.

**Why avoid it:** fixed delays are either too short (flaky) or too long
(slow test suite) and don't communicate *what* is being waited for.

**Instead:** use Power Playwright's `WaitForAppIdleAsync()` for UI
settling, or `RetryExtensions.RetryUntilSucceedsAsync` around the specific
condition/assertion being waited on (see [recipes.md](recipes.md)).

## 4. Inconsistent dialog-detection patterns

**Observed:** some code checks each known dialog type in sequence
(`if (ConfirmDialog.IsVisibleAsync()) ... else if (AlertDialog...) ...`),
while other code assumes a specific dialog type is present without checking
alternatives.

**Why avoid it:** the "assume" pattern will throw an unclear error if the app
under test surfaces a different dialog than expected (e.g. a validation
error instead of a quick-create form), making failures harder to diagnose.

**Instead:** when a command's outcome dialog type is not guaranteed, check
for the possible dialog types explicitly (as in `ModelDrivenAppPageSteps`)
rather than assuming a single outcome.

## 5. `@ignore` used without a tracking mechanism

**Observed:** feature files carrying `@ignore`, disabling execution
indefinitely.

**Why avoid it:** ignored scenarios silently stop providing regression
coverage and tend to be forgotten; the underlying reason for disabling them
gets lost.

**Instead:** only add `@ignore` alongside a linked `@issue:XXXXX`
tracking the reason, and treat a long-lived `@ignore` as a backlog item, not
a permanent state.

## 6. Step definitions that assert on raw booleans instead of FluentAssertions

**Observed:** the overwhelming majority of the suite uses
`actual.Should().Be(expected)`-style assertions; any `Then` step written
with plain conditionals/`Assert.*`/manual `if (...) throw` for business
assertions breaks from this convention.

**Why avoid it:** inconsistent assertion styles produce inconsistent, less
readable failure messages and make step definitions harder to review at a
glance.

**Instead:** always use FluentAssertions for outcome assertions in `Then`
step definitions.

## 6a. Multiple ungrouped assertions in one step

**Observed:** a step asserts on several related values one after another
(or inside a loop) with independent `.Should()` calls, so the first failure
stops execution and hides whether the remaining values were also wrong.

**Why avoid it:** a test run can only report one failure at a time, forcing
an extra fix-and-rerun cycle to discover subsequent failures that were
already present, and makes multi-field/multi-row failures slower to
diagnose.

**Instead:** wrap related assertions (including assertions inside a loop)
in a FluentAssertions `AssertionScope` so every failure in the group is
reported together, with a reason attached to each assertion for context
(see the `AssertionScope` example in
[common-testing-conventions.md](../common-testing-conventions.md)).

## 6b. Assertion library exceptions thrown from `Given`/`When` steps

**Observed:** a `Given`/`When` step uses a FluentAssertions (or other
assertion library) call to check that a precondition or setup action
succeeded, e.g. `record.Should().NotBeNull()` after polling for a
record created by an asynchronous process.

**Why avoid it:** it blurs the line between "setup failed" and "the
behaviour under test failed", and assertion library failure messages are
written for comparing expected/actual test values, not for describing *why*
a precondition could not be established - they tend to surface as generic,
hard-to-diagnose messages (e.g. a bare timeout) instead of domain language.

**Instead:** throw a plain, domain-language exception from `Given`/`When`
steps when a precondition fails (see "Defensive coding" in
[common-testing-conventions.md](../common-testing-conventions.md)). Reserve
assertion library usage for `Then` steps verifying the behaviour actually
under test.

## 7. One step per field instead of table-driven steps

**Observed:** occasional single-field assertion/setup steps exist alongside
the dominant table-driven convention (`Then I see the following fields`,
`When I enter the following details`).

**Why avoid it:** produces long, repetitive Gherkin and a proliferation of
near-duplicate step definitions, working against the entity-centric,
table-driven convention established elsewhere.

**Instead:** extend an existing table-driven step (add a row) rather than
writing a new single-field step, unless the field genuinely needs bespoke
handling no table-driven step can express.

## 8. Building setup logic inline in Playwright step classes

**Observed:** some Playwright step classes (UI-focused, entity-named)
contain sizeable inline Dataverse setup logic (creating/updating records)
rather than delegating to `Scenarios/Events`.

**Why avoid it:** blurs the intended separation between "how data is set up"
(ScenarioBuilder, the shared test-data project) and "how the UI is exercised"
(Power Playwright, the acceptance test project), making setup logic harder
to discover, reuse, and unit test independently of the UI suite.

**Instead:** push reusable setup logic into a `Scenarios/Events` builder in
the test-data project, and keep Playwright step classes focused on driving
and asserting the UI.

## 9. Wiring ScenarioBuilder with `NullLogger.Instance`

**Observed:** `Scenario`/`Event` builders registered with
`NullLogger.Instance` instead of a logger wired to the test output.

**Why avoid it:** discards diagnostic detail (which events ran, what data
they created) exactly when it's most needed - a failing CI run with no
local repro.

**Instead:** register a real `ILogger` implementation (e.g. one that writes
through the test framework's output helper) so ScenarioBuilder's data-setup
actions are visible in CI logs (see [recipes.md](recipes.md)).
