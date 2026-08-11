namespace Defra.Imports.Specs.StepDefinitions
{
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Extensions;
    using FluentAssertions;
    using FluentAssertions.Execution;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityListPageSteps"/> class.
        /// </summary>
        /// <param name="powerPlaywrightCtx">The Power Playwright context.</param>
        public EntityListPageSteps(PowerPlaywrightContext powerPlaywrightCtx)
        {
            this.powerPlaywrightCtx = powerPlaywrightCtx;
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
    }
}
