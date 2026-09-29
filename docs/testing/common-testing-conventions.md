# Common Testing Conventions

Conventions shared by both automated test layers in this repository -
[acceptance tests](acceptance/acceptance-testing-standard.md) (Reqnroll +
Power Playwright) and [integration tests](integration/integration-testing-standard.md)
(MSTest + `ServiceClient`). Both layers build preconditions through
**ScenarioBuilder** and share the same expectations for assertions,
defensive coding, error handling, and logging - this document is the single
source of truth for those shared conventions so neither standard has to
duplicate them.

Where a layer has its own terminology, this document uses:

- **precondition-setting code** - a `Given`/`When` step (acceptance) or the
  `Arrange`/`Act` portion of a test (integration).
- **verification code** - a `Then` step (acceptance) or the `Assert`
  portion of a test (integration).

## Assertions

- All verification code uses **FluentAssertions** (`.Should()...`). Plain
  `Assert.*`/boolean `if` + throw is not the repository convention.
- FluentAssertions (or any assertion library) exceptions are reserved for
  verification code. **Precondition-setting code must never throw an
  assertion library exception** - if a precondition fails to establish
  correctly, throw a plain, high-level exception written in domain language
  instead (see "Defensive coding" below).
- Prefer asserting on a **table/collection of expected values** in one step
  over many single-field assertions.
- Group related assertions with FluentAssertions' `AssertionScope` so that all
  failures are reported together instead of the first failure masking the
  rest, and add a reason to each assertion for additional context:

  ```csharp
  using (new AssertionScope($"request {actualRequest.Id}"))
  {
      actualRequest.Status.Should().Be(expectedStatus, because: "the request should have moved to the expected status");
      actualRequest.StatusReason.Should().Be(expectedReason, because: "the request should record why its status changed");
  }
  ```

## Defensive coding

- Code tests defensively: expect failures at every junction (data setup,
  navigation, waiting on asynchronous processes, UI/API interaction), not
  just at the final assertion.
- When precondition-setting code could fail (e.g. polling for an
  asynchronously-created record times out), throw an easily diagnosable,
  plain exception written in domain language rather than a generic/technical
  one. For example, prefer `"The work item for request '{requestId}' was not
  created within the expected time"` over a bare timeout message such as
  `"records did not appear within 180s"`.
- These defensive exceptions are never assertion library exceptions (see
  "Assertions" above) - assertion library usage is reserved for
  verification code that checks the behaviour actually under test.

## Error handling

- Transient/eventually-consistent conditions (e.g. waiting for an
  integration or async process to update a record) use the
  `RetryExtensions.RetryUntilSucceedsAsync` helper, or an explicit
  `waitForDeletion`/`waitFor...` parameter where the client API supports it -
  not `Task.Delay` loops. `RetryExtensions` lives in the shared
  `Defra.Imports.Scenarios` project so it is usable from both acceptance
  (`Defra.Imports.Specs`) and integration (`Defra.Imports.IntegrationTests`)
  tests.
- This project deliberately does not take a dependency on Polly - the retry
  need here is a fixed attempt count with a fixed delay, which doesn't
  warrant a third-party library. Treat any `Task.Delay`-based wait as an
  anti-pattern to fix, not a template to copy (see
  [anti-patterns.md](acceptance/anti-patterns.md)).

## Logging

- ScenarioBuilder `Scenario`/`Event` builders accept an `ILogger` dependency
  (from `Microsoft.Extensions.Logging`) rather than `NullLogger.Instance`.
  Register a real logger (e.g. one that writes through the test framework's
  output) so data-setup actions are diagnosable from CI output, not just
  local runs.
- Every Dataverse/API request made during data setup is logged with the
  operation and the key identifying details of its payload, e.g. `"creating
  contact: <id>"`, `"updating order <id>: status=<status>"` - not just that a
  request happened, but which entity/record and which values were involved.
  This lets a CI failure be diagnosed from logs alone, without reproducing
  locally.
- Step bindings/test methods log key waits/retries and non-obvious actions
  through the same injected logger, rather than failing silently or relying
  only on assertion messages.
- This applies equally to verification code, not just precondition-setting
  code: the action under test and the state retrieved to verify it (e.g.
  the `ServiceClient` call in an integration test's Act, or the UI action
  and retrieved values in an acceptance test's When/Then) should be logged
  through the same injected logger, so a failing assertion can be diagnosed
  from CI logs without reproducing locally.
