# Integration Testing Standard

This standard defines a lightweight pattern for integration tests against Dataverse, Logic Apps, and Service Bus in this repository. It is intended for MSTest-based tests in `Defra.Imports.IntegrationTests` that exercise real platform behaviour through `ServiceClient`, while using the same `Defra.Imports.Scenarios` data builders as acceptance tests.

This is intentionally lighter than the acceptance-testing standard because there is no UI, no Gherkin layer, and no persona-driven navigation. See:

- [common-testing-conventions.md](../common-testing-conventions.md) - shared rules for assertions, defensive coding, error handling, and logging.
- [scenario-builder-reference.md](../scenario-builder-reference.md) - shared test-data setup patterns.
- [integration-tests.instructions.md](../../../.github/instructions/integration-tests.instructions.md) - the repository rule requiring ScenarioBuilder instead of legacy `Marktek.Fluent.Testing.Engine`.

## Core rule

Every integration test has three separate concerns:

1. **Arrange** - build the Dataverse state using **ScenarioBuilder**.
2. **Act** - perform the behaviour under test through a real **`ServiceClient`** using the correct persona or user context.
3. **Assert** - verify the resulting state with **FluentAssertions**.

Rule of thumb: if you are creating data, use ScenarioBuilder; if you are verifying behaviour, use `ServiceClient` directly.

## Worked example

The below example uses some pseudo-code for conciseness (`SalesScenarioBuilder`, `OpportunityState`, `DeactivateAsync`, and `RetrieveOpportunityAsync`), but the principles remain the same.

```csharp
[TestMethod]
public async Task Deactivate_WhenAccountHasAnOpenOpportunity_DeactivatesOpportunity()
{
    var expectedStateCode = OpportunityState.Lost;

    var scenario = await this.SalesScenarioBuilder
        .UserCreatesOpportunity()
        .BuildAsync();

    Opportunity actualOpportunity = null;

    using (var client = ClientFactory.GetClient(scenario.Account.OwnerId.Id))
    {
        await client.DeactivateAsync(scenario.Account.ToEntityReference());

        actualOpportunity = await client.RetrieveOpportunityAsync(scenario.Opportunity.Id);
    }

    actualOpportunity.StateCode.Should().Be(expectedStateCode, because: "closing an account should also close its open opportunities");
}
```

### What this example is doing

- **Test name** follows `Action_Condition_Expectation`, naming the behaviour under test, the precondition, and the expected result in a sentence-like form.
- **Arrange** - the account + open opportunity precondition is created through a `SalesScenarioBuilder` chain rather than direct record creation. The scenario exposes the created records (`scenario.Account`, `scenario.Opportunity`) so the test does not need to re-query Dataverse for data it already owns.
- **Act** - a `ServiceClient` is acquired using `GetClient(Guid systemUserId)` because the operation depends on the actual owner of the account, not merely a matching persona. The action (`DeactivateAsync`) is called directly through the API surface that a real caller would use.
- **Assert** - the resulting state is fetched through the same client and checked with a single FluentAssertions assertion, including a `because` reason that explains the expectation.
- **Scope** - the `ServiceClient` is wrapped in a `using` block to keep the connection scoped to the test action and assertion rather than the whole test lifetime.

## Conventions

- **Test classes** should be named `<Entity>Tests` for entity-bound behaviours, mirroring the acceptance suite. For unbound global actions with no owning entity, use `<Action>Tests` instead.
- **Test methods** should follow `Action_Condition_Expectation` (for example, `Deactivate_WhenAccountHasAnOpenOpportunity_DeactivatesOpportunity`).
- **Personas** - use `GetClient(Persona.X)` when the behaviour belongs to that persona. Do not default to an admin or app-user client unless the behaviour is specifically application- or service-scoped. If the behaviour depends on a specific user from the scenario, use `GetClient(Guid systemUserId)` and pass the `systemuserid` of the user.
- **Assertions** follow [common-testing-conventions.md](../common-testing-conventions.md): use FluentAssertions with a `because` reason, and group related assertions in an `AssertionScope` when appropriate.
- **Data setup** always goes through `Defra.Imports.Scenarios`. See [integration-tests.instructions.md](../../../.github/instructions/integration-tests.instructions.md) for the rule and rationale, and [scenario-builder-reference.md](../scenario-builder-reference.md) for the builder mechanics (`Event`, `CompositeEvent`, `ComposeUsing`, and incremental `BuildAsync`).

## When authoring a new integration test

1. Find or create the test class that matches the entity or capability under test.
2. Build all preconditions through existing or new `Defra.Imports.Scenarios` builders; never hand-roll records or use the legacy Marktek generators.
3. Act using a `ServiceClient` for the persona or user context that owns the behaviour under test, calling the API operation directly.
4. Assert the resulting state through `ServiceClient` queries and FluentAssertions, with a `because` reason.
5. Confirm the test is independently runnable and does not depend on another test's leftover state.
