# Recipes

Reusable, copy-paste-ready solutions for common acceptance test needs on
Power Platform projects using this stack. Each recipe states intent, the
typical implementation, example code, and caveats. Prefer these over
inventing a new approach.

## Create a Dataverse record for a persona-driven journey

**Intent:** get a record into a known state as a precondition, without using
the UI.

**Typical implementation:** compose a ScenarioBuilder `Event`/`Scenario`
chain and store the result in `ScenarioContext`.

```csharp
[Given("a submitter has submitted a request with a category of {string}")]
public async Task GivenASubmitterHasSubmittedARequest(string category)
{
    var scenario = await this.scenarioBuilder
        .SubmitterSubmitsRequest(a => a
            .WithOneOfTheFollowingCategories(Category.Parse(category)))
        .BuildAsync();

    this.scenarioContext.Set(scenario);
}
```

**Notes/caveats:** if appicable, check `ScenarioContext` for an existing scenario 
of the same type first (`TryGetValue`) - if found, continue building on it with
`BuildAsync(scenario)` instead of creating a second, unrelated record.

## Progress a scenario to a specific point in its lifecycle

**Intent:** get a record (and its related child records) into a known state
partway through its lifecycle, without using the UI.

**Typical implementation:** drill into the relevant composite event via its
builder to configure the nested step you care about, rather than creating
child records directly and relating them manually.

```csharp
var scenario = await this.scenarioBuilder
    .SubmitterSubmitsRequest(a => a.WithOneOfTheFollowingTypes(RequestType.Standard))
    .ApproverProcessesWorkItem(a => a
        .ByProcessingWorkItemTasks(b => b
            .ByProcessingReviewTask(c => c
                .ByCreatingRecord()
                .AndAllPreviousSteps())
            .AndAllPreviousSteps())
        .AndAllPreviousSteps())
    .BuildAsync();
```

**Notes/caveats:** `AndAllPreviousSteps()` is required at child event
level to also run sibling events you didn't explicitly configure. Forgetting
it silently skips setup steps other assertions may depend on. `AndAllOtherSteps`
will execute all sibling events in the composite event.

## Prepare security roles / configure users

**Intent:** ensure the acting user has the right business unit/security
roles for a persona before the scenario runs.

**Typical implementation:** handled by `UserPoolService`, driven by `environment.json`'s `personas` section. Each
credential in the pool falls into one of two categories:

- **Static users** - explicitly listed under a persona's `Users` in
  `environment.json`. These are expected to already have the correct
  business unit/security roles/teams/column security profiles permanently
  configured; `UserPoolService` never modifies them.
- **Dynamic (pooled) users** - any credential not listed against a
  persona's `Users`. These are only eligible for personas that have no
  statically assigned users. When a scenario requests such a persona,
  `UserPoolService` borrows an unassigned user and uses
  `IPersonaConfigurationApplier` to apply the requested persona's
  configuration to it for the duration of the lease, stripping any stale
  configuration left over from a previous lease first.

Individual scenarios simply request a persona and never configure roles or
business units directly:

```gherkin
Given I am logged in to the '<YourApp>' app as '<a persona>'
```

**Notes/caveats:** do not assign roles/business units inside a step
definition - add or adjust the persona's configuration in `environment.json`
instead. Only list specific users under a persona's `Users` if that persona
must always use pre-configured users; otherwise leave `Users` empty so
`UserPoolService` can dynamically configure any available pooled user for
it.

## Open a model-driven app / open a form

**Intent:** get to a known page before interacting with it.

**Typical implementation:**

```csharp
// First navigation in the scenario (also logs in)
homePage = await this.powerPlaywright.LaunchAppAsync(
    browserContext, this.testConfig.Url, AppLogicalName, username, password);

// Navigate directly to a specific existing record
this.powerPlaywrightCtx.ActivePage = await this.powerPlaywrightCtx.ActivePage.ClientApi
    .NavigateToRecordAsync(logicalName, recordId)
```

## Execute a command (ribbon/command bar button)

**Intent:** trigger a business process action exposed as a command bar
button (Save, Activate, Cancel, custom ribbon buttons).

**Typical implementation:**

```csharp
await this.RecordPage.Form.CommandBar.ClickCommandAsync("Cancel");
await this.RecordPage.ConfirmDialog.ConfirmAsync();
```

For commands that navigate or open a dialog, capture the typed result:

```csharp
this.powerPlaywrightCtx.ActivePage = await this.EntityListPage.DataSet.CommandBar
    .ClickCommandAsync<IEntityRecordPage>("New");

var quickCreate = await subgrid.CommandBar.ClickCommandWithDialogAsync<IQuickCreateForm>("New");
```

**Notes/caveats:** after a command that shows a confirmation/alert dialog,
always explicitly confirm/dismiss it - do not assume the dialog closes
itself. The exception is scenarios that end on the dialog being shown.

## Validate field values

**Intent:** assert multiple field values/states in one readable step.

**Typical implementation:** a `Then` step taking a `DataTable` of field name
-> expected value/attributes, resolved via `ExecuteGenericFieldActionAsync`
by display name:

```gherkin
Then I see the following field values
    | Field         | Value    |
    | Status        | Inactive |
    | Status Reason | Failed   |
```

```csharp
[Then("I see the following field values")]
public async Task ThenISeeTheFollowingFieldValues(Table table)
{
    foreach (var row in table.Rows)
    {
        await this.ExecuteGenericFieldActionAsync(row["Field"], async (field, fieldContext) =>
        {
            var actual = await field.GetValueAsync(fieldContext.ControlType);
            ...
        });
    }
}
```

**Notes/caveats:** prefer one table-driven step over one step per field.

## Validate a business process / automation outcome

**Intent:** assert that a plugin/workflow/Power Automate flow produced the
expected side effect (e.g. a work order was created after risk assessment).

**Typical implementation:** poll with `RetryExtensions.RetryUntilSucceedsAsync`
rather than asserting immediately, since these are asynchronous. This
project does not depend on Polly for this - a fixed attempt count with a
fixed delay is all that's needed, and adding a retry library for that would
be unjustified complexity:

```csharp
await RetryExtensions.RetryUntilSucceedsAsync(
    async () =>
    {
        var workItem = await this.serviceClient.WaitForWorkItemForRequestAsync(requestId);

        if (workItem is null)
        {
            throw new Exception($"Work item for request '{requestId}' was not created within the expected time");
        }
    },
    this.logger);
```

Or use a dedicated extension helper if one already exists (see
`ServiceClientExtensions.WaitForFieldValueAsync`, `WaitForRecordsAsync`)
rather than writing a new bespoke polling loop. `RetryUntilSucceedsAsync`
logs each retry attempt via the passed-in logger so a flaky failure can be
diagnosed from CI output alone.

**Notes/caveats:** never assert immediately after triggering an async
process; always poll with a bounded number of retries and a sensible
interval so failures surface within a reasonable time. When this recipe is
used in a `Given`/`When` step (establishing state rather than verifying the
behaviour under test), throw a plain, domain-language exception - as above -
never an assertion library exception; name the record/request involved so
the failure is diagnosable without needing to reproduce the run.

## Validate an integration (external system side effect)

**Intent:** confirm data reached (or was correctly received from) an
external system.

**Typical implementation:** ScenarioBuilder events exist for external-system
personas (e.g. `Scenarios/Events/ExternalSystemA`,
`Scenarios/Events/ExternalSystemB`) that simulate the external system's
action against Dataverse directly (since the real external system isn't
invoked in tests). Compose them into the scenario like any other event:

```csharp
var scenario = await this.scenarioBuilder
    .SubmitterSubmitsRequest(a => a
        .WithOneOfTheFollowingTypes(RequestType.Standard))
    .ExternalSystemProcessesRequest()
    .BuildAsync();
```

**Notes/caveats:** these events simulate the integration's *effect*, not the
integration transport itself - they are not end-to-end tests of the external
system connection.
