# Power Playwright Reference

How Power Platform acceptance test projects use the `PowerPlaywright` NuGet
package (typically under `Steps/Playwright/`) to automate the model-driven
app UI. See the [upstream docs](https://github.com/ewingjm/power-playwright)
for the raw API (`IModelDrivenAppPage`, `IEntityRecordPage`,
`IEntityListPage`, page/control abstractions); this document describes
conventions for using it consistently on a project.

## Bootstrapping and session

Power Playwright is created once per scenario, and the active page/user are tracked
in `PowerPlaywrightContext`:

```csharp
public class PowerPlaywrightContext
{
    public IModelDrivenAppPage ActivePage { get; set; }
    public Guid ActiveUserId { get; set; }

    public void Validate() { /* throws if no active session */ }
    public void ValidatePage<TPageType>() { /* throws if ActivePage is the wrong type */ }
}
```

Every step that interacts with the UI reads/writes `powerPlaywrightCtx.ActivePage`
rather than holding its own page reference - this is what lets step classes
be composed across a scenario ("open the record" in one step class, "assert
a field" in another).

## Navigation

```csharp
// First navigation of a scenario - launch the app and log in
homePage = await this.powerPlaywright.LaunchAppAsync(
    browserContext, this.testConfig.Url, AppLogicalName, username, password);

// Open a new record form
this.powerPlaywrightCtx.ActivePage = await this.powerPlaywrightCtx.ActivePage.ClientApi
    .OpenFormAsync(logicalName);

// Navigate directly to an existing record, with a bounded timeout
var appPage = await this.powerPlaywrightCtx.ActivePage.ClientApi
    .NavigateToRecordAsync(record.LogicalName, record.Id);
```

## Form interaction

Use the typed control interfaces (`ITextControl`, `IDate`, `IDateTime`,
`IChoice`, `ILookup`, `IYesNo`, `IReadOnlyGrid`, `IGridControl`) via
`Form.GetField<T>()` / `Form.GetControl<T>()` - never raw locators for form
fields.

```csharp
// Set a value generically, keyed off the field's declared control type
await this.ExecuteGenericFieldActionAsync(displayName, async (field, fieldContext) =>
{
    await field.SetValueAsync(fieldContext.ControlType, value);
    await this.RecordPage.Page.Keyboard.PressAsync("Tab"); // commit the field
});

// Set a date field directly when the control type is already known
var control = this.RecordPage.Form.GetField<IDate>(logicalName).Control;
await control.SetValueAsync(DateTime.Today.AddDays(3));

// Read a value back for assertion
var actualValue = await field.GetValueAsync(fieldContext.ControlType);
```

The `ExecuteGenericFieldActionAsync`/`ExecuteGenericDataSetActionAsync`
helpers in `EntityRecordPageSteps.cs` resolve a field/subgrid by its
**display name** (as it appears in the feature file / data table) to the
correct control, so steps stay declarative:

```gherkin
Then I see the following fields
    | Field                 | Populated | Editable | Requirement Level |
    | Site / Batch ID       | false     | true     | Required          |
```

## Grid / subgrid interaction

```csharp
// Open a record from a subgrid row
await this.ExecuteGenericDataSetActionAsync(subgridDisplayName, async (dataSet, controlType) =>
{
    var control = dataSet.GetControl(controlType);
    this.powerPlaywrightCtx.ActivePage = await control.OpenRecordAsync(rowIndex);
});

// Find and select a row by column value
var data = await dataSet.Control.GetRowDataAsync();
var rowToSelect = data.ToList().FindIndex(r => r[column] == value);
await dataSet.Control.ToggleSelectRowAsync(rowToSelect);

// Switch a list page's view, then read its columns
await this.EntityListPage.DataSet.SwitchViewAsync(viewName);
var columns = await this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>().GetColumnNamesAsync();
```

## Command bar

```csharp
await this.RecordPage.Form.CommandBar.ClickCommandAsync("Save");

// Clicking a command that navigates to a new page
this.powerPlaywrightCtx.ActivePage = await this.EntityListPage.DataSet.CommandBar
    .ClickCommandAsync<IEntityRecordPage>("New");

// Clicking a command that opens a dialog
var quickCreate = await subgrid.CommandBar.ClickCommandWithDialogAsync<IQuickCreateForm>("New");
```

## Waiting strategies

**Preferred** - rely on Power Playwright's built-in idle detection:

```csharp
await page.ReloadAsync();
await page.WaitForLoadStateAsync();
await page.WaitForAppIdleAsync();
```

**Acceptable** - `RetryExtensions.RetryUntilSucceedsAsync` around an
assertion/query that depends on an eventually-consistent backend process
(integration, plugin, Flow):

```csharp
await RetryExtensions.RetryUntilSucceedsAsync(
    async () =>
    {
        // assertion or query that may not have propagated yet
    },
    this.logger);
```

**Discouraged** - a bare `Task.Delay(...)` or `Thread.Sleep(...)`. 

## Authentication

- Each test user's browser storage state is cached to disk after first login
  (`context.StorageStateAsync(...)`) and reused on subsequent scenarios for
  the same user, avoiding repeated interactive login.
- Credentials/usernames come from the `UserPoolClient` (a leased pool per
  persona), not hardcoded values.

## Preferred / Acceptable / Discouraged

| | Pattern |
|---|---|
| **Preferred** | Power Playwright typed control/page interfaces (`GetField<T>`, `GetControl<T>`, `CommandBar.ClickCommandAsync`, grid `Control.*` methods); `WaitForAppIdleAsync()`; FluentAssertions. |
| **Acceptable** | Narrow, justified drops to raw Playwright (`IPage.Keyboard.PressAsync("Tab"/"Escape")` to commit a field or dismiss a flyout, `IPage.EvaluateAsync` to read `Xrm.Utility` global context) when Power Playwright has no equivalent; `RetryExtensions.RetryUntilSucceedsAsync` retries for eventually-consistent state. |
| **Discouraged** | Reaching for raw Playwright locators (`Page.GetByText(...)`, `ILocator.WaitForAsync`) where a Power Playwright control abstraction exists; `Task.Delay` as a wait. |
