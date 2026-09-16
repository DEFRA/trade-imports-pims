namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Extensions;
    using Defra.Imports.Specs.Services;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using PowerPlaywright.Framework.Extensions;
    using PowerPlaywright.Framework.Controls.Pcf.Classes;
    using PowerPlaywright.Framework.Model;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;

    /// <summary>
    /// Step bindings relating to the <see cref="IEntityListPage"/> page.
    /// </summary>
    [Binding]
    public class EntityListPageSteps
    {
        private const string CommandButtonNew = "New";

        private readonly PowerPlaywrightContext powerPlaywrightCtx;
        private readonly ScenarioContext scenarioContext;
        private readonly KnownDefectRecorder defectRecorder;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityListPageSteps"/> class.
        /// </summary>
        /// <param name="powerPlaywrightCtx">The Power Playwright context.</param>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <param name="defectRecorder">The known defect recorder.</param>
        public EntityListPageSteps(PowerPlaywrightContext powerPlaywrightCtx, ScenarioContext scenarioContext, KnownDefectRecorder defectRecorder)
        {
            this.powerPlaywrightCtx = powerPlaywrightCtx;
            this.scenarioContext = scenarioContext;
            this.defectRecorder = defectRecorder;
        }

        private IEntityListPage EntityListPage
        {
            get
            {
                this.powerPlaywrightCtx.ValidatePage<IEntityListPage>();

                return (IEntityListPage)this.powerPlaywrightCtx.ActivePage;
            }
        }

        /// <summary>
        /// Asserts that the provided views are visible on the entity list page.
        /// </summary>
        /// <param name="expectedViews">The expected views.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the following views")]
        public async Task ThenISeeAnViewWithTheFollowingColumns(DataTable expectedViews)
        {
            // TODO: Update this implementation when Power Playwright supports retrieving the list of views.
            foreach (var expectedView in expectedViews.Rows.Select(r => r[0]))
            {
                try
                {
                    await this.EntityListPage.DataSet.SwitchViewAsync(expectedView);
                }
                catch
                {
                    throw new AssertionFailedException($"The view '{expectedView}' was not found.");
                }
            }
        }

        /// <summary>
        /// Toggles the selection of all rows within the current view.
        /// </summary>
        /// <param name="checkedState">The selected state.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When(@"^I (select|deselect) all rows within the view$")]
        public async Task WhenISelectAllRowsWithinTheView(bool checkedState)
        {
            var grid = this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>();
            await grid.ToggleSelectAllRowsAsync(select: checkedState);
        }

        /// <summary>
        /// Create a new record from the entity list page.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I click to create a new record from the view")]
        public async Task WhenIClickToCreateANewRecordFromTheView()
        {
            this.powerPlaywrightCtx.ActivePage = await this.EntityListPage.DataSet.CommandBar
                .ClickCommandAsync<IEntityRecordPage>(CommandButtonNew);
        }

        /// <summary>
        /// Verifies that a new record can be created from the view, recording a known defect where
        /// the command is not available to the signed in user.
        /// </summary>
        /// <param name="acceptanceCriterion">The acceptance criterion being verified.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I verify a Caseworker can create a new record from the view for {string}")]
        public async Task ThenIVerifyACaseworkerCanCreateANewRecordFromTheView(string acceptanceCriterion)
        {
            await this.defectRecorder.TryVerifyAsync(
                acceptanceCriterion,
                "Create a new Import Notification",
                $"The '{CommandButtonNew}' command is available on the Importer Notifications view.",
                async () => await this.WhenIClickToCreateANewRecordFromTheView());
        }

        /// <summary>
        /// Switches to the specified view on the entity list page.
        /// </summary>
        /// <param name="viewName">The name of the view to switch to.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I switch to the {string} view")]
        public async Task WhenISwitchToTheView(string viewName)
        {
            await this.EntityListPage.DataSet.SwitchViewAsync(viewName);
        }

        /// <summary>
        /// Asserts that the view is sorted by the given column and order.
        /// </summary>
        /// <param name="columnName">The column name.</param>
        /// <param name="order">The expected order.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the view is sorted by the {string} column in {word} order")]
        public async Task ThenISeeAnViewWithTheFollowingColumns(string columnName, ColumnSortOrder order)
        {
            var sortOrders = await this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>().GetSortOrdersAsync();

            var expectedSortSpec = new ColumnSortSpec(columnName, order);

            sortOrders.Should().HaveCount(1);
            sortOrders.Should().Contain(expectedSortSpec);
        }

        /// <summary>
        /// Asserts that a view with the specified name and columns is displayed on the entity list page.
        /// </summary>
        /// <param name="viewName">The view name.</param>
        /// <param name="expectedColumns">The column headers.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see a {string} view with the following columns")]
        [Then("I see an {string} view with the following columns")]
        public async Task ThenISeeAnViewWithTheFollowingColumns(string viewName, DataTable expectedColumns)
        {
            var isVisible = await this.EntityListPage.DataSet.IsVisibleAsync();

            isVisible.Should().BeTrue();

            await this.EntityListPage.DataSet.SwitchViewAsync(viewName);
            await this.EntityListPage.DataSet.Container.GetByText(viewName).WaitForAsync();
            var readOnlyGrid = this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>();

            var columns = await readOnlyGrid.GetColumnNamesAsync();
            var trimmedcolumns = columns.Select(c => c.Trim()).ToList();
            trimmedcolumns.Should().BeEquivalentTo(expectedColumns.Header.ToArray());
        }

        /// <summary>
        /// Asserts that a view with the specified name displays at least the given columns.
        /// </summary>
        /// <remarks>
        /// Unlike the equivalence assertion, this allows the view to expose additional columns and
        /// makes no assertion about the order in which the columns appear.
        /// </remarks>
        /// <param name="viewName">The view name.</param>
        /// <param name="expectedColumns">The column headers.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see a {string} view containing the following columns")]
        [Then("I see an {string} view containing the following columns")]
        public async Task ThenISeeAnViewContainingTheFollowingColumns(string viewName, DataTable expectedColumns)
        {
            var isVisible = await this.EntityListPage.DataSet.IsVisibleAsync();

            isVisible.Should().BeTrue();

            await this.EntityListPage.DataSet.SwitchViewAsync(viewName);
            await this.EntityListPage.DataSet.Container.GetByText(viewName).WaitForAsync();
            var readOnlyGrid = this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>();

            var columns = await readOnlyGrid.GetColumnNamesAsync();
            var trimmedcolumns = columns.Select(c => c.Trim()).ToList();
            trimmedcolumns.Should().Contain(expectedColumns.Header.ToArray());
        }

        /// <summary>
        /// Verifies the columns required by an acceptance criterion, recording any that the view does
        /// not provide as known defects.
        /// </summary>
        /// <remarks>
        /// Every required column is checked, so the report names all of the missing columns rather
        /// than only the first. A view that does not exist at all is recorded as a single defect
        /// covering every column, and execution continues.
        /// </remarks>
        /// <param name="acceptanceCriterion">The acceptance criterion being verified.</param>
        /// <param name="viewName">The view name.</param>
        /// <param name="expectedColumns">The columns required by the acceptance criterion.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I verify the columns required by {string} in the {string} view")]
        public async Task ThenIVerifyTheColumnsRequiredByInTheView(string acceptanceCriterion, string viewName, DataTable expectedColumns)
        {
            var requiredColumns = expectedColumns.Header.ToArray();

            (await this.EntityListPage.DataSet.IsVisibleAsync())
                .Should().BeTrue("the list view should be displayed.");

            List<string> actualColumns;

            try
            {
                await this.EntityListPage.DataSet.SwitchViewAsync(viewName);
                await this.EntityListPage.DataSet.Container.GetByText(viewName).WaitForAsync();

                actualColumns = (await this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>().GetColumnNamesAsync())
                    .Select(c => c.Trim())
                    .ToList();
            }
            catch (Exception ex)
            {
                foreach (var column in requiredColumns)
                {
                    this.defectRecorder.RecordDefect(
                        acceptanceCriterion,
                        $"{viewName} > {column}",
                        $"The '{viewName}' view shows a '{column}' column.",
                        $"The '{viewName}' view is not available. {ex.Message.Split('\r', '\n').FirstOrDefault()}");
                }

                return;
            }

            foreach (var column in requiredColumns)
            {
                if (actualColumns.Contains(column, StringComparer.OrdinalIgnoreCase))
                {
                    this.defectRecorder.RecordVerified(acceptanceCriterion, $"{viewName} > {column}");
                }
                else
                {
                    this.defectRecorder.RecordDefect(
                        acceptanceCriterion,
                        $"{viewName} > {column}",
                        $"The '{viewName}' view shows a '{column}' column.",
                        $"The column is not present. Columns actually shown: {string.Join(", ", actualColumns)}.");
                }
            }
        }

        /// <summary>
        /// Verifies the sort order required by an acceptance criterion, recording a known defect when
        /// the view is not sorted as required.
        /// </summary>
        /// <param name="acceptanceCriterion">The acceptance criterion being verified.</param>
        /// <param name="columnName">The column the view should be sorted by.</param>
        /// <param name="order">The expected sort order.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I verify the sort order required by {string} is the {string} column in {word} order")]
        public async Task ThenIVerifyTheSortOrderRequiredBy(string acceptanceCriterion, string columnName, ColumnSortOrder order)
        {
            var requirement = $"Sorted by '{columnName}' ({order})";
            var expected = $"The view is sorted by '{columnName}' in {order} order.";

            try
            {
                var sortOrders = await this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>().GetSortOrdersAsync();

                if (sortOrders.Contains(new ColumnSortSpec(columnName, order)))
                {
                    this.defectRecorder.RecordVerified(acceptanceCriterion, requirement);
                }
                else
                {
                    this.defectRecorder.RecordDefect(
                        acceptanceCriterion,
                        requirement,
                        expected,
                        $"The view is sorted by: {string.Join(", ", sortOrders.Select(s => $"{s.ColumnName} ({s.Order})"))}.");
                }
            }
            catch (Exception ex)
            {
                this.defectRecorder.RecordDefect(acceptanceCriterion, requirement, expected, ex.Message.Split('\r', '\n').FirstOrDefault());
            }
        }

        /// <summary>
        /// Asserts that the specified commands are visible on the grid.
        /// </summary>
        /// <param name="should">Whether or not the commands should be seen.</param>
        /// <param name="commands">The commands.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I (can|cannot) see the following commands on the grid")]
        public async Task ThenISeeTheFollowingCommandsOnTheGrid(bool should, DataTable commands)
        {
            var expectedCommands = commands.Rows.Select(r => r[0].ToString()).ToArray();
            var actualCommands = await this.EntityListPage.DataSet.CommandBar.GetCommandsAsync();

            if (should)
            {
                actualCommands.Should().Contain(expectedCommands);
            }
            else
            {
                actualCommands.Should().NotContain(expectedCommands);
            }
        }

        /// <summary>
        /// Asserts that the specified command is visible on multiple pages after navigating to them.
        /// </summary>
        /// <param name="should">Whether or not the command should be seen.</param>
        /// <param name="commandName">The command name.</param>
        /// <param name="pages">The pages to check (Group and Page columns).</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I (can|cannot) see the '(.*)' command in the following pages$")]
        public async Task ThenICanOrCannotSeeTheCommandInTheFollowingPages(bool should, string commandName, DataTable pages)
        {
            foreach (var row in pages.Rows)
            {
                var group = row["Group"];
                var page = row["Page"];

                this.powerPlaywrightCtx.ActivePage = await this.powerPlaywrightCtx.ActivePage.SiteMap
                    .OpenPageAsync<IEntityListPage>("Plants", group, page);

                if (page == "Contacts")
                {
                    // Workaround for contacts page not loading correctly on first navigation in Playwright test runs.
                    await this.powerPlaywrightCtx.ActivePage.Page.ReloadAndWaitForAppIdleAsync();
                }

                var isVisible = await this.EntityListPage.DataSet.IsVisibleAsync();
                isVisible.Should().BeTrue();

                await this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>().ToggleSelectAllRowsAsync();
                var actualCommands = await this.EntityListPage.DataSet.CommandBar.GetCommandsAsync();
                if (should)
                {
                    actualCommands.Should().Contain(
                        commandName,
                        $"Command '{commandName}' should be visible on '{group}' -> '{page}'");
                }
                else
                {
                    actualCommands.Should().NotContain(
                        commandName,
                        $"Command '{commandName}' should NOT be visible on '{group}' -> '{page}'");
                }
            }
        }

        /// <summary>
        /// Asserts that the specified commands are visible under a parent command on the grid.
        /// </summary>
        /// <param name="should">Whether or not the commands should be seen.</param>
        /// <param name="parentCommandName">Parent command name.</param>
        /// <param name="commands">Expected commands.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I (can|cannot) see the commands under the '(.*)' command on the grid$")]
        public async Task ThenICanSeeTheCommandsUnderTheCommandOnTheGrid(bool should, string parentCommandName, DataTable commands)
        {
            var expectedCommands = commands.Rows.Select(r => r[0].ToString()).ToArray();
            var actualCommands = await this.EntityListPage.DataSet.CommandBar.GetCommandsAsync(parentCommandName);

            if (should)
            {
                actualCommands.Should().Contain(expectedCommands);
            }
            else
            {
                actualCommands.Should().NotContain(expectedCommands);
            }
        }

        /// <summary>
        /// Searches the current list view using the search token from the created Importer Notification precondition.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I search the current view for the created Importer Notification")]
        public async Task WhenISearchTheCurrentViewForTheCreatedImporterNotification()
        {
            if (!this.scenarioContext.TryGetValue<string>(ScenarioContextKeys.CreatedImporterNotificationSearchToken, out var searchToken)
                && !this.scenarioContext.TryGetValue<string>(ScenarioContextKeys.CreatedImporterNotificationReferenceNumber, out searchToken))
            {
                throw new InvalidOperationException($"Unable to find {ScenarioContextKeys.CreatedImporterNotificationSearchToken} or {ScenarioContextKeys.CreatedImporterNotificationReferenceNumber} in scenario context.");
            }

            await this.SearchCurrentViewAsync(searchToken);
        }

        /// <summary>
        /// Searches the current list view using the value seeded against the named field of the created Importer Notification precondition.
        /// </summary>
        /// <param name="fieldName">The name of the field, as written in US-003 AC-3.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I search the current view for the created Importer Notification by {string}")]
        public async Task WhenISearchTheCurrentViewForTheCreatedImporterNotificationBy(string fieldName)
        {
            if (!this.scenarioContext.TryGetValue<IDictionary<string, string>>(ScenarioContextKeys.CreatedImporterNotificationSearchValues, out var searchValues)
                || !searchValues.TryGetValue(fieldName, out var searchToken))
            {
                throw new InvalidOperationException($"Unable to find a seeded search value for '{fieldName}' in scenario context.");
            }

            this.scenarioContext.AddOrUpdate(ScenarioContextKeys.CreatedImporterNotificationSearchToken, searchToken);

            await this.SearchCurrentViewAsync(searchToken);
        }

        /// <summary>
        /// Asserts that the created Importer Notification appears in the current view results.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the created Importer Notification in the current view")]
        public async Task ThenISeeTheCreatedImporterNotificationInTheCurrentView()
        {
            if (!this.scenarioContext.TryGetValue<string>(ScenarioContextKeys.CreatedImporterNotificationSearchToken, out var searchToken)
                && !this.scenarioContext.TryGetValue<string>(ScenarioContextKeys.CreatedImporterNotificationReferenceNumber, out searchToken))
            {
                throw new InvalidOperationException($"Unable to find {ScenarioContextKeys.CreatedImporterNotificationSearchToken} or {ScenarioContextKeys.CreatedImporterNotificationReferenceNumber} in scenario context.");
            }

            var foundMatch = false;
            for (var attempt = 0; attempt < 10 && !foundMatch; attempt++)
            {
                var rows = await this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>().GetRowDataAsync();
                foundMatch = rows.Any(r => r.Values.Any(v =>
                    !string.IsNullOrWhiteSpace(v) &&
                    v.IndexOf(searchToken, StringComparison.OrdinalIgnoreCase) >= 0));

                if (!foundMatch)
                {
                    await Task.Delay(2000);
                    await this.EntityListPage.DataSet.Container.Page.WaitForAppIdleAsync();
                }
            }

            foundMatch.Should().BeTrue($"Expected to find search token '{searchToken}' in current view results.");
        }

        /// <summary>
        /// Searches the current view using each of the criteria required by an acceptance criterion in
        /// turn, recording any that return no matching record as a known defect.
        /// </summary>
        /// <remarks>
        /// Every criterion is exercised regardless of the outcome of those before it, so the report
        /// states clearly which search criteria are supported and which are not.
        /// </remarks>
        /// <param name="acceptanceCriterion">The acceptance criterion being verified.</param>
        /// <param name="criteria">The search criteria, named as the acceptance criterion names them.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I search the current view using each criterion required by {string}")]
        public async Task WhenISearchTheCurrentViewUsingEachCriterionRequiredBy(string acceptanceCriterion, DataTable criteria)
        {
            if (!this.scenarioContext.TryGetValue<IDictionary<string, string>>(ScenarioContextKeys.CreatedImporterNotificationSearchValues, out var searchValues))
            {
                throw new InvalidOperationException($"Unable to find {ScenarioContextKeys.CreatedImporterNotificationSearchValues} in scenario context.");
            }

            foreach (var row in criteria.Rows)
            {
                var criterion = row["Search criterion"];
                var requirement = $"Free text search by {criterion}";
                var expected = $"Searching by {criterion} returns the matching Importer Notification.";

                if (!searchValues.TryGetValue(criterion, out var searchToken) || string.IsNullOrWhiteSpace(searchToken))
                {
                    this.defectRecorder.RecordDefect(
                        acceptanceCriterion,
                        requirement,
                        expected,
                        $"The solution has no attribute holding a {criterion}, so no test data can exist for this criterion.");

                    continue;
                }

                try
                {
                    await this.SearchCurrentViewAsync(searchToken);

                    if (await this.CurrentViewContainsAsync(searchToken))
                    {
                        this.defectRecorder.RecordVerified(acceptanceCriterion, requirement);
                    }
                    else
                    {
                        this.defectRecorder.RecordDefect(
                            acceptanceCriterion,
                            requirement,
                            expected,
                            $"Searching for '{searchToken}' returned no matching record. The quick find view does not search this field.");
                    }
                }
                catch (Exception ex)
                {
                    this.defectRecorder.RecordDefect(acceptanceCriterion, requirement, expected, ex.Message.Split('\r', '\n').FirstOrDefault());
                }
            }
        }

        /// <summary>
        /// Determines whether the current view contains a row matching the search token.
        /// </summary>
        /// <param name="searchToken">The search token.</param>
        /// <returns>A <see cref="Task"/> that resolves to true when a matching row is found.</returns>
        private async Task<bool> CurrentViewContainsAsync(string searchToken)
        {
            for (var attempt = 0; attempt < 5; attempt++)
            {
                var rows = await this.EntityListPage.DataSet.GetControl<IReadOnlyGrid>().GetRowDataAsync();

                if (rows.Any(r => r.Values.Any(v =>
                    !string.IsNullOrWhiteSpace(v) &&
                    v.IndexOf(searchToken, StringComparison.OrdinalIgnoreCase) >= 0)))
                {
                    return true;
                }

                await Task.Delay(2000);
                await this.EntityListPage.DataSet.Container.Page.WaitForAppIdleAsync();
            }

            return false;
        }

        private async Task SearchCurrentViewAsync(string searchToken)
        {
            var searchInput = this.EntityListPage.DataSet.Container
                .Locator("input[aria-label*='Search'], input[placeholder*='Search'], input[data-id*='quickFind']")
                .First;

            if (!await searchInput.IsVisibleAsync())
            {
                throw new InvalidOperationException("Unable to find a visible search input on the current view.");
            }

            await searchInput.FillAsync(searchToken);
            await searchInput.PressAsync("Enter");
            await this.EntityListPage.DataSet.Container.Page.WaitForAppIdleAsync();
        }
    }
}
