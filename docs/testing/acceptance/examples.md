# Examples

Curated, representative examples for authoring acceptance tests on Power
Platform projects using this stack (Reqnroll + Power Playwright +
ScenarioBuilder). Domain entities below (`Request`, `Work item`, `Line
item`, etc.) are illustrative placeholders - substitute your own project's
business entities and field names.

## 1. A complete feature file (Scenario Outline + Examples)

```gherkin
Feature: Reject a submission

@issue:PROJ-5579
Scenario Outline: An approver rejects a submission
    Given I am logged in to the '<YourApp>' app as '<an approver>'
    And a submitter has submitted a '<Category>' <Request Type> request
	And the request's work item is assigned to me
	And I have opened the work item
	When I click the 'Reject' command
	And I confirm the dialog
	And I enter one of the following options in the 'Reason for Rejection' field
		| Option   |
		| Reason A |
		| Reason B |
		| Reason C |
	And I save the record
	Then I see the following field values
		| Field         | Value    |
		| Status        | Inactive |
		| Status Reason | Rejected |

	Examples:
		| Request Type  | Category |
		| standard      | type a   |
		| expedited     | type b   |
```

Why this is representative: `@issue:` traceability and a persona-first `Given` chain that
delegates data setup to ScenarioBuilder ("a submitter has submitted...",
"the request's work item is assigned to me", "I have created a record
for..."), a single UI action (`When`), and a single table-driven assertion
(`Then`).

## 2. A feature file asserting UI structure (tabs)

```gherkin
Feature: View a work item

@issue:PROJ-1576 @issue:PROJ-1248
Scenario: An approver views a work item
	Given I am logged in to the '<YourApp>' app as '<an approver>'
	And a request's work item is assigned to me
	When I open the request's work item
	Then I see the following tabs
		| Tab C | Tab D | Related |
	But I do not see the following tabs
		| Tab A | Tab B |
```

Why this is representative: multiple `@issue:` tags on one scenario, `But`/`And`
used to contrast positive and negative assertions in the same flow, and an
implicit setup step condensed into the precondition it exists to support
(`a request's work item is assigned to me`, rather than a separate
no-information `a submitter has submitted a request` step followed by `the
request's work item is assigned to me`).

## 3. A ScenarioBuilder-driven `Given` step

```csharp
/// <summary>
/// A submitter has submitted a request of one of the given types.
/// </summary>
/// <param name="requestTypes">The request types to choose from.</param>
/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

Illustrative source location: `Steps/Playwright/RequestSteps.cs`.

## 4. A deep, multi-event ScenarioBuilder chain

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

Illustrative source location: `Steps/Playwright/RequestSteps.cs`. Demonstrates
composing a `Scenario` -> composite `Event` -> composite `Event` -> leaf
`Event`, with `AndAllPreviousSteps()` at every level so unconfigured sibling
steps still run.

## 5. Resuming a scenario across multiple `Given` steps

```csharp
/// <summary>
/// I have scheduled a review, resuming the current scenario if one has already been started.
/// </summary>
/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
[Given("I have scheduled a review")]
public async Task GivenIHaveScheduledAReview()
{
    if (!this.scenarioContext.TryGetValue<RequestScenario>(out var scenario))
    {
        // New scenario
        scenario = await this.scenarioBuilder
            .ApproverProcessesWorkItem(a => a
                .ByAssigningWorkItem(b => b.WithAssignee(this.powerPlaywrightCtx.ActiveUserId))
                .ByProcessingWorkItemTasks(c => c
                    .ByProcessingReviewTask(d => d
                        .ByScheduling()
                        .AndAllPreviousSteps()))
                .AndAllPreviousSteps())
            .BuildAsync();
    }
    else
    {
        // Existing scenario
        scenario = await this.scenarioBuilder
            .ApproverProcessesWorkItem(a => a
                .ByAssigningWorkItem(b => b.WithAssignee(this.powerPlaywrightCtx.ActiveUserId))
                .ByProcessingWorkItemTasks(c => c.ByProcessingReviewTask(d => d.ByScheduling()))
                .AndAllPreviousSteps())
            .BuildAsync(scenario);
    }

    this.scenarioContext.Set(scenario);
}
```

Illustrative source location: `Steps/Playwright/WorkItemTaskSteps.cs`.

## 6. A Power Playwright UI action step (command + dialog)

```csharp
/// <summary>
/// Cancels the work item with the given reason, confirming the resulting dialog.
/// </summary>
/// <param name="reason">The reason for cancelling.</param>
/// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
[When("I cancel the work item with a reason of {string}")]
public async Task WhenICancelTheWorkItemWithAReasonOf(string reason)
{
    var reasonField = this.RecordPage.Form.GetField<IChoice>("new_reasonforcancelling");
    await reasonField.Control.SetValueAsync(reason);
    await this.RecordPage.Form.CommandBar.ClickCommandAsync("Cancel");
    await this.RecordPage.ConfirmDialog.ConfirmAsync();
}
```

Illustrative source location: `Steps/Playwright/WorkItemSteps.cs`.

## 7. Retry around an eventually-consistent assertion

```csharp
await RetryExtensions.RetryUntilSucceedsAsync(
    async () =>
    {
        // operation/assertion that depends on an async process completing
    },
    this.logger);
```

Illustrative source location: `Steps/Playwright/IntegrationSteps.cs`. Log
each retry attempt via an injected logger so a flaky failure can be
diagnosed from CI output alone.

## 10. A `Scenario` class declaring its composed events

```csharp
[ComposeUsing(order: 0, SubmitterSubmitsRequestEventId, typeof(SubmitterSubmitsRequestEvent))]
[ComposeUsing(order: 1, ApproverProcessesWorkItemEventId, typeof(ApproverProcessesWorkItemEvent))]
[ComposeUsing(order: 2, SubmitterSubmitsAppealEventId, typeof(SubmitterSubmitsAppealEvent))]
public sealed class RequestScenario : Scenario
{
    public SubmitterSubmitsRequestEvent.Info SubmitterSubmitsRequestEvent { get; internal set; }
    public ApproverProcessesWorkItemEvent.Info ApproverProcessesWorkItemEvent { get; internal set; }
    // ... one property per composed event's captured output
}
```

Illustrative source location: `Scenarios/RequestScenario.cs`.

## 11. Registering scenario builders and Power Playwright in DI

```csharp
[BeforeScenario]
public void SetupScenarioBuilder()
{
    this.objectContainer.RegisterInstanceAs(
        new RequestScenario.Builder(
            this.objectContainer.Resolve<ServiceClientFactory>(),
            this.objectContainer.Resolve<ILogger<RequestScenario.Builder>>()));
}

[BeforeScenario(Order = -9998)]
public async Task SetupPowerPlaywright()
{
    var powerPlaywright = await PowerPlaywright.CreateAsync(new PowerPlaywrightConfiguration { /* ... */ });
    this.objectContainer.RegisterInstanceAs(powerPlaywright);
}
```

Illustrative source location: `Hooks/ContainerHooks.cs`. New shared services
should be registered the same way, with an `Order` chosen relative to the
existing registrations. Resolve a real `ILogger<T>` (wired to the test
framework's output) rather than `NullLogger.Instance`, so ScenarioBuilder's
data-setup actions are visible in CI logs.
