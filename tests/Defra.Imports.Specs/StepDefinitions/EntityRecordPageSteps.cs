namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Extensions;
    using Defra.Imports.Specs.Model;
    using Defra.Imports.Specs.Services;
    using FluentAssertions;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using PowerPlaywright.Framework.Controls.Pcf;
    using PowerPlaywright.Framework.Controls.Pcf.Classes;
    using PowerPlaywright.Framework.Controls.Platform;
    using PowerPlaywright.Framework.Extensions;
    using PowerPlaywright.Framework.Model;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;
    using DataRow = PowerPlaywright.Framework.Model.DataRow;
    using DataTable = Reqnroll.DataTable;

    /// <summary>
    /// Step bindings relating to the <see cref="IEntityRecordPage"/> page.
    /// </summary>
    [Binding]
    public class EntityRecordPageSteps
    {
        private readonly ScenarioContext ctx;
        private readonly PowerPlaywrightContext powerPlaywrightCtx;
        private readonly FormMetadataService formMetadataSvc;
        private readonly PowerPlaywrightMetadataService powerPlaywrightMetadataSvc;
        private readonly EntityMetadataService entityMetadataSvc;
        private readonly ServiceClient serviceClient;
        private readonly RecordNavigatorService recordNavigator;
        private Bogus.Faker faker;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRecordPageSteps"/> class.
        /// </summary>
        /// <param name="ctx">The scenario context.</param>
        /// <param name="powerPlaywrightCtx">The Power Playwright context.</param>
        /// <param name="formMetadataSvc">The form metadata service.</param>
        /// <param name="powerPlaywrightMetadataSvc">The Power Playwright metadata service.</param>
        /// <param name="entityMetadataSvc">The entity metadata service.</param>
        /// <param name="serviceClient">The service client.</param>
        /// <param name="recordNavigator">The record navigator.</param>
        public EntityRecordPageSteps(ScenarioContext ctx, PowerPlaywrightContext powerPlaywrightCtx, FormMetadataService formMetadataSvc, PowerPlaywrightMetadataService powerPlaywrightMetadataSvc, EntityMetadataService entityMetadataSvc, ServiceClient serviceClient, RecordNavigatorService recordNavigator)
        {
            this.ctx = ctx;
            this.powerPlaywrightCtx = powerPlaywrightCtx;
            this.formMetadataSvc = formMetadataSvc;
            this.powerPlaywrightMetadataSvc = powerPlaywrightMetadataSvc;
            this.entityMetadataSvc = entityMetadataSvc;
            this.serviceClient = serviceClient;
            this.recordNavigator = recordNavigator;
            this.faker = new Bogus.Faker("en_GB");
        }

        private IEntityRecordPage RecordPage
        {
            get
            {
                this.powerPlaywrightCtx.ValidatePage<IEntityRecordPage>();

                return (IEntityRecordPage)this.powerPlaywrightCtx.ActivePage;
            }
        }

        /// <summary>
        /// Selects a tab on a form.
        /// </summary>
        /// <param name="relatedTab">The tab.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I select the {string} tab")]
        [Given("I have selected the {string} tab")]
        public async Task WhenISelectTheTab(string relatedTab)
        {
            await this.RecordPage.Form.OpenTabAsync(relatedTab);
        }

        /// <summary>
        /// Selects a related tab on a form.
        /// </summary>
        /// <param name="relatedTab">The tab.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I select the related {string} tab")]
        [Given("I have selected the related {string}")]
        public async Task WhenISelectTheRelatedTab(string relatedTab)
        {
            await this.RecordPage.Form.OpenRelatedTabAsync(relatedTab);
        }

        /// <summary>
        /// Enter a date into a date field.
        /// </summary>
        /// <param name="direction">The date direction.</param>
        /// <param name="field">Display name of the field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When(@"^I enter a date in the (future|past) for the '(.*)' field$")]
        public async Task IEnterADateIntoTheDateField(DateDirection direction, string field)
        {
            var formId = await this.RecordPage.GetFormIdAsync();
            var logicalName = this.formMetadataSvc.GetControlLogicalName(formId, field, out _);
            var control = this.RecordPage.Form.GetField<IDate>(logicalName).Control;

            var value = DateTime.Today.AddDays(direction == DateDirection.Past ? -3 : 3);

            await control.SetValueAsync(value);

            await this.RecordPage.Form.CommandBar.ClickCommandAsync("Save");
        }

        /// <summary>
        /// Clear the value with a field.
        /// </summary>
        /// <param name="displayName">The display name of the field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I clear the value in the {string} field")]
        public async Task WhenIClearTheValueInTheField(string displayName)
        {
            await this.ExecuteGenericFieldActionAsync(
                displayName,
                async (field, fieldContext) =>
                {
                    await field.SetValueAsync(fieldContext.ControlType, null);
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Clears the value in multiple fields.
        /// </summary>
        /// <param name="fields">The fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I clear the values in the following fields")]
        public async Task IDeleteTheValuesInTheFollowingFields(Table fields)
        {
            var tasks = fields.Rows.Select(async r => await this.ExecuteGenericFieldActionAsync(
                r[0],
                async (field, fieldContext) =>
                {
                    await field.SetValueAsync(fieldContext.ControlType, null);
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync()));

            await Task.WhenAll(tasks);

            await this.RecordPage.Form.CommandBar.ClickCommandAsync("Save");
        }

        /// <summary>
        /// Clicks a command on the form.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("I have clicked the {string} command")]
        [When("I click the {string} command")]
        public async Task WhenIClickTheCommand(string command)
        {
            await this.RecordPage.Form.CommandBar.ClickCommandAsync(command);
        }

        /// <summary>
        /// Clicks a command on the subgrid.
        /// </summary>
        /// <param name="subgridDisplayName">The display name of the subgrid.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When(@"I click to create a new record on the {string} subgrid")]
        public async Task ThenISeeACommandOnTheGrid(string subgridDisplayName)
        {
            var formId = await this.RecordPage.GetFormIdAsync();
            var tab = await this.RecordPage.Form.GetActiveTabAsync();
            var subgridName = this.formMetadataSvc.GetControlLogicalName(formId, subgridDisplayName, out _, tab: tab);

            var subgrid = this.RecordPage.Form.GetDataSet<IReadOnlyGrid>(subgridName);
            var isVisible = await subgrid.IsVisibleAsync();
            isVisible.Should().BeTrue();

            var commands = await subgrid.CommandBar.GetCommandsAsync();
            var newCommand = commands.FirstOrDefault(c => c.StartsWith("New "))
                ?? throw new InvalidOperationException($"The '{subgridDisplayName}' subgrid does not have a 'New' command.");

            this.ctx.AddOrUpdate(ScenarioContextKeys.AddNewToSubgridName, subgridName);
            this.ctx.AddOrUpdate(ScenarioContextKeys.AddNewToSubgridTotalRowCount, await subgrid.Control.GetTotalRowCountAsync());

            var quickCreate = await subgrid.CommandBar.ClickCommandWithDialogAsync<IQuickCreateForm>(newCommand);

            if (await quickCreate.Container.IsVisibleAsync())
            {
                this.ctx.AddOrUpdate(nameof(IQuickCreateForm), quickCreate);
            }
        }

        /// <summary>
        /// Enters a value in a field.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="displayName">The field display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I enter {string} in the {string} field")]
        public async Task WhenIEnterInTheField(string value, string displayName)
        {
            await this.ExecuteGenericFieldActionAsync(
                displayName,
                async (field, fieldContext) =>
                {
                    await field.SetValueAsync(fieldContext.ControlType, value);
                    await this.RecordPage.Page.Keyboard.PressAsync("Tab");
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Interaction with toogle controls.
        /// </summary>
        /// <param name="displayName">The field display name.</param>
        /// <param name="value">The field value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I toggle the {string} field to {string}")]
        public async Task WhenIToggleTheFieldTo(string displayName, string value)
        {
            await this.ExecuteControlActionAsync<IToggleControl>(
                displayName,
                async (control, fieldContext) =>
                {
                    await control.SetValueAsync(bool.Parse(value));
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Enters the specified details into the form.
        /// </summary>
        /// <param name="fields">The field values to populate.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I enter the following details")]
        [When("I enter the following details into the form")]
        public async Task WhenIEnterTheFollowingDetailsIntoTheForm(DataTable fields)
        {
            foreach (var row in fields.Rows)
            {
                await this.ExecuteGenericFieldActionAsync(
                    row["Field"],
                    async (field, fieldContext) =>
                    {
                        await field.SetValueAsync(fieldContext.ControlType, row["Value"]);
                    },
                    tab: await this.RecordPage.Form.Container.IsVisibleAsync() ? await this.RecordPage.Form.GetActiveTabAsync() : null);
            }
        }

        /// <summary>
        /// Enters the specified values into the form and saves.
        /// </summary>
        /// <param name="tableDisplayName">The table display name.</param>
        /// <param name="fields">The field values to populate.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I create a new {string} with the following deatils")]
        [When("I attempt to create a new {string} with the following deatils")]
        public async Task ICreateANewRecordWithTheFollowingDeatils(string tableDisplayName, DataTable fields)
        {
            foreach (var row in fields.Rows)
            {
                await this.ExecuteGenericFieldActionAsync(row["Field"], async (field, fieldContext) =>
                {
                    await field.SetValueAsync(fieldContext.ControlType, row["Value"]);
                });
            }

            await this.RecordPage.Form.CommandBar.ClickCommandAsync("Save");
        }

        /// <summary>
        /// Save the record.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I save the record")]
        public async Task WhenISaveTheRecord()
        {
            await this.RecordPage.Form.CommandBar.ClickCommandAsync("Save");
        }

        /// <summary>
        /// Assigns the record to the currently logged in user.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I assign the current record to me")]
        public async Task WhenIAssignTheCurrentRecordToMe()
        {
            var dialog = await this.RecordPage.Form.CommandBar.ClickCommandWithDialogAsync<IAssignDialog>("Assign");
            await dialog.AssignToMeAsync();
        }

        /// <summary>
        /// Modifies the records state.
        /// </summary>
        /// <param name="state">The record state.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When(@"^I (activate|deactivate) the record$")]
        public async Task WhenIModifyARecordsState(string state)
        {
            var commandLabel = Regex.Replace(state.ToLower(), @"^\w", m => m.Value.ToUpper());

            await this.RecordPage.Form.CommandBar.ClickCommandAsync(commandLabel);

            await this.RecordPage.SetStateDialog.ConfirmAsync();
        }

        /// <summary>
        /// User enters data into an editable grid.
        /// </summary>
        /// <param name="controlDisplayName">Editable grid friendly name.</param>
        /// <param name="fields">The data table containing the fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I enter the following values in the {string} editable grid")]
        public async Task WhenIEnterTheFollowingValuesInTheEditableGrid(string controlDisplayName, DataTable fields)
        {
            await this.ExecuteGenericEditableGridActionAsync(controlDisplayName, async (editableGrid) =>
            {
                for (int i = 0; i < fields.Rows.Count; i++)
                {
                    var row = fields.Rows[i];
                    await editableGrid.Control.UpdateRowAsyncV2(i, fields.Header.ToDictionary(h => h, h => row[h]));
                }

                if (!await this.IsDialogPresent())
                {
                    await editableGrid.Control.SaveChangesAsync();
                    await editableGrid.Container.Page.WaitForAppIdleAsync();
                }
            });
        }

        /// <summary>
        /// Opens a record in the subgrid.
        /// </summary>
        /// <param name="subgridDisplayName">The subgrid name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I open a record in the {string} subgrid")]
        public async Task WhenIOpenARecordInTheSubgrid(string subgridDisplayName)
        {
            var rowIndex = 0;

            await this.ExecuteGenericDataSetActionAsync(
                subgridDisplayName,
                async (dataSet, controlType) =>
                {
                    var control = dataSet.GetControl(controlType);

                    this.powerPlaywrightCtx.ActivePage = await control.OpenRecordAsync(rowIndex);
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// User interacts with sub-menu commands within sub grid.
        /// </summary>
        /// <param name="commands">The command. Subcommands can be indicated with ' -> '.</param>
        /// <param name="controlDisplayName">Subgrid friendly name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I click the {string} command on the {string} subgrid")]
        public async Task WhenIClickTheCommandOnTheSubgrid(string commands, string controlDisplayName)
        {
            await this.ExecuteDataSetActionAsync<IReadOnlyGrid>(controlDisplayName, async (subgrid) =>
            {
                await subgrid.CommandBar.ClickCommandAsync(commands.Split([" -> "], StringSplitOptions.RemoveEmptyEntries));
            });
        }

        /// <summary>
        /// User interacts with sub-menu commands within editable grid.
        /// </summary>
        /// <param name="menuCommandName">Menu command name.</param>
        /// <param name="subMenuCommandName">Sub-menu command name.</param>
        /// <param name="controlDisplayName">Editable grid friendly name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I click the {string} -> {string} command on the {string} editable grid")]
        public async Task WhenIClickTheCommandOnTheEditableGrid(string menuCommandName, string subMenuCommandName, string controlDisplayName)
        {
            await this.ExecuteGenericEditableGridActionAsync(controlDisplayName, async (editableGrid) =>
            {
                var quickCreate = await editableGrid.CommandBar.ClickCommandWithDialogAsync<IQuickCreateForm>(menuCommandName, subMenuCommandName);
                if (await quickCreate.Container.IsVisibleAsync())
                {
                    this.ctx.AddOrUpdate(nameof(IQuickCreateForm), quickCreate);
                }
            });
        }

        /// <summary>
        /// User interacts with menu commands within editable grid.
        /// </summary>
        /// <param name="menuCommandName">Menu command name.</param>
        /// <param name="controlDisplayName">Editable grid friendly name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I click the {string} command on the {string} editable grid")]
        public async Task WhenIClickTheCommandOnTheEditableGrid(string menuCommandName, string controlDisplayName)
        {
            await this.ExecuteGenericDataSetActionAsync(controlDisplayName, async (dataSet, controlType) =>
            {
                var quickCreate = await dataSet.CommandBar.ClickCommandWithDialogAsync<IQuickCreateForm>(menuCommandName);
                if (await quickCreate.Container.IsVisibleAsync())
                {
                    this.ctx.AddOrUpdate(nameof(IQuickCreateForm), quickCreate);
                }
            });
        }

        /// <summary>
        /// Selects a row in a subgrid.
        /// </summary>
        /// <param name="subgridDisplayName">The subgrid display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I select a row in the {string} subgrid")]
        [When("I select a row in the {string} editable grid")]
        public async Task WhenISelectARowInTheSubgrid(string subgridDisplayName)
        {
            int rowIndex = 0;

            await this.ExecuteGenericDataSetActionAsync(subgridDisplayName, async (dataSet, controlType) =>
            {
                var control = dataSet.GetControl(controlType);
                await control.ToggleSelectRowAsync(rowIndex);

                this.ctx.AddOrUpdate(ScenarioContextKeys.SelectedRow, rowIndex);
            });
        }

        /// <summary>
        /// Selects all row in a subgrid.
        /// </summary>
        /// <param name="subgridDisplayName">The subgrid display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I select all rows in the {string} subgrid")]
        public async Task WhenISelectAllRowsInTheSubgrid(string subgridDisplayName)
        {
            await this.ExecuteDataSetActionAsync<IReadOnlyGrid>(subgridDisplayName, async dataSet =>
            {
                await dataSet.Control.ToggleSelectAllRowsAsync();
                await dataSet.Container.Page.WaitForAppIdleAsync();
            });
        }

        /// <summary>
        /// Selects a row with a given value for a given column.
        /// </summary>
        /// <param name="subgridDisplayName">The name of the subgrid.</param>
        /// <param name="column">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I select a row in the {string} editable grid where {string} is {string}")]
        public async Task WhenISelectARowInTheEditableGridWhereIs(string subgridDisplayName, string column, string value)
        {
            await this.ExecuteGenericEditableGridActionAsync(subgridDisplayName, async (dataSet) =>
            {
                var data = await dataSet.Control.GetRowDataAsync();
                var rowToSelect = data.ToList().FindIndex(r => r[column] == value);

                if (rowToSelect == -1)
                {
                    throw new InvalidOperationException("There is no row with the specified value.");
                }

                await dataSet.Control.ToggleSelectRowAsync(rowToSelect);

                this.ctx.AddOrUpdate(ScenarioContextKeys.SelectedRow, rowToSelect);
            });
        }

        /// <summary>
        /// Selects a row with a given value for a given column.
        /// </summary>
        /// <param name="subgridDisplayName">The name of the subgrid.</param>
        /// <param name="column">The column name.</param>
        /// <param name="value">The value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I select a row in the {string} subgrid where {string} is {string}")]
        public async Task WhenISelectARowInTheSubgridWhereIs(string subgridDisplayName, string column, string value)
        {
            await this.ExecuteDataSetActionAsync<IReadOnlyGrid>(subgridDisplayName, async (dataSet) =>
            {
                var data = await dataSet.Control.GetRowDataAsync();
                var rowToSelect = data.ToList().FindIndex(r => r[column] == value);

                if (rowToSelect == -1)
                {
                    throw new InvalidOperationException("There is no row with the specified value.");
                }

                await dataSet.Control.ToggleSelectRowAsync(rowToSelect);

                this.ctx.AddOrUpdate(ScenarioContextKeys.SelectedRow, rowToSelect);
            });
        }

        /// <summary>
        /// Selects all row in an editable subgrid.
        /// </summary>
        /// <param name="subgridDisplayName">The subgrid display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I select all rows in the {string} editable subgrid")]
        public async Task WhenISelectAllRowsInTheEditableSubgrid(string subgridDisplayName)
        {
            await this.ExecuteGenericDataSetActionAsync(subgridDisplayName, async (dataSet, controlType) =>
            {
                var control = dataSet.GetControl(controlType);
                await control.ToggleSelectAllRowsAsync();

                await dataSet.Container.Page.WaitForAppIdleAsync();
            });
        }

        /// <summary>
        /// Selects all rows within an editable grid.
        /// </summary>
        /// <param name="subgridDisplayName">The editable grid display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I select all rows in the {string} editable grid")]
        public async Task WhenISelectAllRowsInTheEditableGrid(string subgridDisplayName)
        {
            await this.ExecuteDataSetActionAsync<IGridControl>(subgridDisplayName, async dataSet =>
            {
                await dataSet.Control.ToggleSelectAllRowsAsync();
                await dataSet.Container.Page.WaitForAppIdleAsync();
            });
        }

        /// <summary>
        /// Expands a row in a subgrid to view the nested subgrid.
        /// </summary>
        /// <param name="subgridDisplayName">The subgrid display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I expand a row in the {string} subgrid")]
        [When("I expand a row in the {string} editable grid")]
        [When("I expand the row in the {string} subgrid")]
        [When("I expand the row in the {string} editable grid")]
        public async Task WhenIExpandARowInTheSubgrid(string subgridDisplayName)
        {
            int rowIndex = 0;

            await this.ExecuteGenericDataSetActionAsync(subgridDisplayName, async (dataSet, controlType) =>
            {
                object nestedSubgrid = null;

                switch (controlType)
                {
                    case Type t when t == typeof(IGridControl):
                        nestedSubgrid = await ((IGridControl)dataSet.GetControl(controlType)).ExpandNestedSubgridAsync(rowIndex);
                        break;
                    case Type t when t == typeof(IPowerAppsOneGrid):
                        nestedSubgrid = await ((IPowerAppsOneGrid)dataSet.GetControl(controlType)).ExpandNestedSubgridAsync(rowIndex);
                        break;
                }

                this.ctx.AddOrUpdate(ScenarioContextKeys.NestedSubgrid, nestedSubgrid);
                this.ctx.AddOrUpdate(ScenarioContextKeys.NestedSubgridType, controlType);
            });
        }

        /// <summary>
        /// Enters one of the specified options into a choice field.
        /// </summary>
        /// <param name="displayName">The field.</param>
        /// <param name="expectedOptions">The options.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I enter one of the following options in the {string} field")]
        public async Task WhenIEnterOneOfTheFollowingOptionsInTheField(string displayName, DataTable expectedOptions)
        {
            await this.ExecuteGenericFieldActionAsync(
                displayName,
                async (f, fc) =>
                {
                    var choice = f.GetControl<IChoice>();
                    var availableOptions = await choice.GetAllOptionsAsync();

                    availableOptions.Should().Contain(expectedOptions.Rows.Select(r => r[0]));

                    await choice.SetValueAsync(this.faker.PickRandom(availableOptions));
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// User interacts with sub-menu commands within the form.
        /// </summary>
        /// <param name="menuCommandName">Menu command name.</param>
        /// <param name="subMenuCommandName">Sub-menu command name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I click the {string} -> {string} command")]
        public async Task WhenIClickTheSubCommand(string menuCommandName, string subMenuCommandName)
        {
            await this.RecordPage.Form.CommandBar.ClickCommandAsync(menuCommandName, subMenuCommandName);
        }

        /// <summary>
        /// Enters one of the specified options into a column in an editable grid.
        /// </summary>
        /// <param name="columnDisplayName">Column display name.</param>
        /// <param name="controlDisplayName">Control display name.</param>
        /// <param name="dataTable">Available options.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I enter one of the following options into the {string} column of the {string} editable grid")]
        public async Task WhenIEnterOneOfTheFollowingOptionsIntoTheColumnOfTheEditableGrid(string columnDisplayName, string controlDisplayName, DataTable dataTable)
        {
            var options = dataTable.Rows.Select(r => r[0]).ToArray();

            await this.ExecuteGenericEditableGridActionAsync(controlDisplayName, async (editableGrid) =>
            {
                await editableGrid.Control.UpdateRowAsync(0, new Dictionary<string, string>
                {
                    [columnDisplayName] = this.faker.Random.Shuffle(options).First(),
                });
            });
        }

        /// <summary>
        /// Enters a value into a column in an editable grid.
        /// </summary>
        /// <param name="value">The value to enter.</param>
        /// <param name="columnDisplayName">Column display name.</param>
        /// <param name="controlDisplayName">Control display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I enter {string} into the {string} column of the {string} editable grid")]
        public async Task WhenIEnterIntoTheColumnOfTheEditableGrid(string value, string columnDisplayName, string controlDisplayName)
        {
            await this.ExecuteGenericEditableGridActionAsync(controlDisplayName, async (editableGrid) =>
            {
                await editableGrid.Control.UpdateRowAsync(0, new Dictionary<string, string>
                {
                    [columnDisplayName] = value,
                });
            });
        }

        /// <summary>
        /// Verifies the rows in the nested subgrid.
        /// </summary>
        /// <param name="controlDisplayName">The subgrid display name.</param>
        /// <param name="expectedRows">The expected rows.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I can see the following rows in the {string} nested subgrid")]
        public async Task ThenICanSeeTheFollowingRowsInTheNestedSubgrid(string controlDisplayName, DataTable expectedRows)
        {
            IEnumerable<DataRow> rowData = null;

            switch (this.GetNestedSubgridType())
            {
                case Type t when t == typeof(IGridControl):
                    rowData = await this.GetNestedSubgrid<IGridControl>().GetRowDataAsync();
                    break;
                case Type t when t == typeof(IPowerAppsOneGrid):
                    rowData = await this.GetNestedSubgrid<IPowerAppsOneGrid>().GetRowDataAsync();
                    break;
            }

            rowData.Should().BeEquivalentTo(expectedRows.Rows.Select(r => r.ToDictionary(c => c.Key, c => c.Value)));
        }

        /// <summary>
        /// Switches the view in a subgrid.
        /// </summary>
        /// <param name="view">The view to switch to.</param>
        /// <param name="subgrid">The subgrid display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I switch to the {string} view in the {string} subgrid")]
        public async Task WhenISwitchToTheViewInTheSubgrid(string view, string subgrid)
        {
            await this.ExecuteDataSetActionAsync<IReadOnlyGrid>(subgrid, async dataSet =>
            {
                await dataSet.SwitchViewAsync(view);
            });
        }

        /// <summary>
        /// Saves the quick create.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I save the quick create")]
        public async Task WhenISaveTheQuickCreate()
        {
            await this.ctx.Get<IQuickCreateForm>(nameof(IQuickCreateForm)).SaveAndCloseAsync();
        }

        /// <summary>
        /// Saves the modified rows in an editable grid.
        /// </summary>
        /// <param name="controlDisplayName">Control display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I save the the modified rows in the {string} editable grid")]
        public async Task WhenISaveTheTheModifiedRowsInTheEditableGrid(string controlDisplayName)
        {
            await this.ExecuteGenericEditableGridActionAsync(controlDisplayName, async (editableGrid) =>
            {
                await editableGrid.Control.SaveChangesAsync();
            });
        }

        /// <summary>
        /// User enters data into an editable grid for all rows.
        /// </summary>
        /// <param name="controlDisplayName">Editable grid friendly name.</param>
        /// <param name="fields">The data table containing the fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I enter the following values in the {string} editable grid for all rows")]
        public async Task WhenIEnterTheFollowingValuesInTheEditableGridForAllRows(string controlDisplayName, DataTable fields)
        {
            await this.ExecuteGenericEditableGridActionAsync(controlDisplayName, async (editableGrid) =>
            {
                var dataRows = (await editableGrid.Control.GetRowDataAsync()).ToArray();

                for (var i = 0; i < dataRows.Length; i++)
                {
                    await editableGrid.Control.UpdateRowAsync(i, fields.Header.ToDictionary(h => h, h => fields.Rows[0][h]));
                }

                await editableGrid.Control.SaveChangesAsync();
            });
        }

        /// <summary>
        /// Verifies the visibility of a command.
        /// </summary>
        /// <param name="should">Whether or not the command should be seen.</param>
        /// <param name="command">The command.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I (can|cannot) see the '(.*)' command")]
        [Then(@"I(| do not) see the '(.*)' command")]
        public async Task ThenICanOrCanNotSeeTheCommand(bool should, string command)
        {
            var commands = await this.RecordPage.Form.CommandBar.GetCommandsAsync();

            if (should)
            {
                commands.Should().Contain(command);
            }
            else
            {
                commands.Should().NotContain(command);
            }
        }

        /// <summary>
        /// Verifies the visibility of commands.
        /// </summary>
        /// <param name="should">Whether or not the command should be seen.</param>
        /// <param name="dataTable">The commands.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I(| do not) see the following commands")]
        public async Task ThenISeeTheFollowingCommands(bool should, DataTable dataTable)
        {
            var expectedCommands = dataTable.Rows.Select(r => r[0]);
            var commands = await this.RecordPage.Form.CommandBar.GetCommandsAsync();

            if (should)
            {
                commands.Should().Contain(expectedCommands);
            }
            else
            {
                commands.Should().NotContain(expectedCommands);
            }
        }

        /// <summary>
        /// Asserts that the specified commands are visible.
        /// </summary>
        /// <param name="should">Whether or not the commands should be seen.</param>
        /// <param name="commands">The commands.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I (can|cannot) see the following commands on the record page")]
        public async Task ThenICanOrCannotSeeTheFollowingCommandsOnTheRecordPage(bool should, DataTable commands)
        {
            var expectedCommands = commands.Rows.Select(r => r[0].ToString()).ToArray();
            var actualCommands = await this.RecordPage.Form.CommandBar.GetCommandsAsync();

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
        /// Verifies the visibility of commands under a parent command.
        /// </summary>
        /// <param name="should">Whether or not the commands should be seen.</param>
        /// <param name="parentCommandName">Parent command.</param>
        /// <param name="commands">The commands.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I (can|cannot) see the commands under '(.*)'")]
        public async Task ThenICanOrCannotSeeTheCommandsUnder(bool should, string parentCommandName, DataTable commands)
        {
            var expectedCommands = commands.Rows.Select(r => r[0].ToString()).ToArray();
            var actualCommands = await this.RecordPage.Form.CommandBar.GetCommandsAsync(parentCommandName);

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
        /// Verifies there is a form notification with the given text.
        /// </summary>
        /// <param name="notificationLevel">The notification level.</param>
        /// <param name="formType">The form type.</param>
        /// <param name="message">The notification message.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see an (error|info) notification on the (quick create|form) stating '(.*)'")]
        [Then("I see a (warning) notification on the (quick create|form) stating '(.*)'")]
        public async Task ThenISeeAFormNotificationStating(FormNotificationLevel notificationLevel, FormType formType, string message)
        {
            var expectedNotification = new FormNotification(notificationLevel, await this.ReplaceTemplateValuesAsync(message));

            IEnumerable<FormNotification> notifications = null;

            if (formType == FormType.Form)
            {
                notifications = await this.RecordPage.Form.GetFormNotificationsAsync();
            }
            else
            {
                notifications = await this.ctx.Get<IQuickCreateForm>(nameof(IQuickCreateForm)).GetFormNotificationsAsync();
            }

            notifications.Should().Contain(expectedNotification);
        }

        /// <summary>
        /// Verifies that there are form notifications with the given type and message.
        /// </summary>
        /// <param name="formType">The form type.</param>
        /// <param name="expectedNotifications">The expected notifications.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I see the following notifications on the (quick create|form)")]
        public async Task ThenISeeFollowingNotifications(FormType formType, IEnumerable<FormNotification> expectedNotifications)
        {
            IEnumerable<FormNotification> notifications = null;

            if (formType == FormType.Form)
            {
                notifications = await this.RecordPage.Form.GetFormNotificationsAsync();
            }
            else
            {
                notifications = await this.ctx.Get<IQuickCreateForm>(nameof(IQuickCreateForm)).GetFormNotificationsAsync();
            }

            notifications.Should().Contain(expectedNotifications);
        }

        /// <summary>
        /// Verifies there is a form notification stating a date must be in the past.
        /// </summary>
        /// <param name="field">The display name of the field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I see an error message stating the date field '(.*)' must be in the past")]
        public async Task ThenTheDateCannotBeInTheFuture(string field)
        {
            var notifications = await this.RecordPage.Form.GetFormNotificationsAsync();

            notifications.Should().Contain(new FormNotification(FormNotificationLevel.Error, "The date for " + field + " must be in the past."));
        }

        /// <summary>
        /// Asserts the requirement level of a field.
        /// </summary>
        /// <param name="field">The field display name.</param>
        /// <param name="requirementLevel">The field requirement level.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the '(.*)' field is (required|recommended|optional)")]
        public async Task ThenTheFieldIsRequirementLevel(string field, FieldRequirementLevel requirementLevel)
        {
            await this.ExecuteGenericFieldActionAsync(field, async (f, fc) =>
            {
                var actualRequirementLevel = await f.GetRequirementLevelAsync();

                actualRequirementLevel.Should().Be(requirementLevel);
            });
        }

        /// <summary>
        /// Asserts that the provided fields are visible and are either editable or read-only.
        /// </summary>
        /// <param name="table">A table with headers 'Field', 'Editable' and "Mandatory".</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I see the following fields")]
        [Then(@"I see the following header fields")]
        public async Task ISeeTheFollowingFields(Table table)
        {
            var checkEditable = table.Header.Contains("Editable");
            var checkRequirementLevel = table.Header.Contains("Requirement Level");
            var checkPopulated = table.Header.Contains("Populated");

            var exceptions = new List<Exception>();
            var currentTab = !this.ctx.StepContext.StepInfo.Text.Contains("header") && await this.RecordPage.Form.Container.IsVisibleAsync() ? await this.RecordPage.Form.GetActiveTabAsync() : null;
            foreach (var row in table.Rows)
            {
                var fieldDisplayName = row["Field"];

                await this.ExecuteGenericFieldActionAsync(
                    fieldDisplayName,
                    async (field, fieldContext) =>
                    {
                        if (checkEditable)
                        {
                            var expectedDisabled = !bool.Parse(row["Editable"]);
                            var actualDisabled = fieldContext.IsQuickView || await field.IsDisabledAsync(); // Workaround due to bug in Power Playwright that returns true for IsDisabledAsync for quick view fields.
                            if (actualDisabled != expectedDisabled)
                            {
                                exceptions.Add(new AssertFailedException($"Expected {fieldDisplayName} Disabled: {expectedDisabled} but found Disabled: {actualDisabled}."));
                            }
                        }

                        if (checkRequirementLevel)
                        {
                            var expectedRequirementLevel = (FieldRequirementLevel)Enum.Parse(typeof(FieldRequirementLevel), row["Requirement Level"]);
                            var actualRequirementLevel = await field.GetRequirementLevelAsync();
                            if (actualRequirementLevel != expectedRequirementLevel)
                            {
                                exceptions.Add(new AssertFailedException($"Expected {fieldDisplayName} Requirement: {expectedRequirementLevel} but found Requirement: {actualRequirementLevel}."));
                            }
                        }

                        if (checkPopulated)
                        {
                            var expectedPopulated = bool.Parse(row["Populated"]);
                            var actualValue = await field.GetValueAsync(fieldContext.ControlType);
                            var actualPopulated = actualValue != null;
                            if (actualPopulated != expectedPopulated)
                            {
                                exceptions.Add(new AssertFailedException($"Expected {fieldDisplayName} Populated: {expectedPopulated} but found Populated: {actualPopulated}."));
                            }
                        }
                    },
                    tab: currentTab);
            }

            if (exceptions.Any())
            {
                throw new AssertFailedException(exceptions.Select(e => e.Message).Aggregate((a, b) => a + Environment.NewLine + b));
            }
        }

        /// <summary>
        /// Verifies the fields visible scoped a tab.
        /// </summary>
        /// <param name="tab">The tab.</param>
        /// <param name="dataTable">The fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the following fields in the {string} tab")]
        public async Task ThenISeeTheFollowingFieldsInTheTab(string tab, DataTable dataTable)
        {
            await this.RecordPage.Form.OpenTabAsync(tab);
            await this.ISeeTheFollowingFields(dataTable);
        }

        /// <summary>
        /// Asserts that the record is saved successfully.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the record is saved successfully")]
        public async Task ThenTheRecordIsSavedSuccessfully()
        {
            var notifications = await this.RecordPage.Form.GetFormNotificationsAsync();
            var errorNotifications = notifications.Where(n => n.Level == FormNotificationLevel.Error);

            errorNotifications.Should().BeEmpty();
        }

        /// <summary>
        /// Asserts that a field is or isn't editable.
        /// </summary>
        /// <param name="should">Whether or not the field should be editable.</param>
        /// <param name="fieldDisplayName">The field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I (can|cannot) edit the '(.*)' field")]
        public async Task ThenICanEditTheField(bool should, string fieldDisplayName)
        {
            await this.ExecuteGenericFieldActionAsync(
                fieldDisplayName,
                async (field, fieldContext) =>
                {
                    var isDisabled = await field.IsDisabledAsync();

                    if (should)
                    {
                        isDisabled.Should().BeFalse();
                    }
                    else
                    {
                        isDisabled.Should().BeTrue();
                    }
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Asserts that no field is editable on the current tab.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I cannot edit any field on the current tab")]
        public async Task ThenICannotEditAnyField()
        {
            var fields = await this.RecordPage.Form.GetFieldsAsync();
            var fieldsDisabledValue = await Task.WhenAll(fields.Select(f => f.IsDisabledAsync()));

            fieldsDisabledValue.Should().OnlyContain(isDisabled => isDisabled);
        }

        /// <summary>
        /// Asserts that the provided field is not visible.
        /// </summary>
        /// <param name="should">Whether the field should be visible.</param>
        /// <param name="fieldDisplayName">The field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I(| do not) see the '(.*)' field")]
        public async Task IDoOrDoNotSeeTheField(bool should, string fieldDisplayName)
        {
            await this.ExecuteGenericFieldActionAsync(
                fieldDisplayName,
                async (field, fieldContext) =>
                {
                    var isVisible = await field.IsVisibleAsync();

                    isVisible.Should().Be(should);
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Asserts that the provided subgrid is not visible.
        /// </summary>
        /// <param name="should">Whether the subgrid should be visible.</param>
        /// <param name="subgridDisplayName">The subgrid.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I (can|cannot) see the '(.*)' subgrid")]
        public async Task ISeeTheSubgrid(bool should, string subgridDisplayName)
        {
            var formId = await this.RecordPage.GetFormIdAsync();

            var match = Regex.Match(subgridDisplayName, "<([^>]+)>");
            var fieldLogicalName = match.Success ? match.Groups[1].Value : this.formMetadataSvc.GetControlLogicalName(formId, subgridDisplayName, out _);
            var dataSet = this.RecordPage.Form.GetDataSet(fieldLogicalName);

            var isVisible = await dataSet.IsVisibleAsync();

            if (should)
            {
                isVisible.Should().BeTrue();
            }
            else
            {
                isVisible.Should().BeFalse();
            }
        }

        /// <summary>
        /// Asserts that the provided fields not visible.
        /// </summary>
        /// <param name="table">A table with header 'Field'.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I do not see the following fields")]
        public async Task IDoNotSeeTheFollowingFields(Table table)
        {
            var tab = await this.RecordPage.Form.GetActiveTabAsync();
            var failures = new List<Exception>();

            foreach (var row in table.Rows)
            {
                try
                {
                    await this.ExecuteGenericFieldActionAsync(
                        row["Field"],
                        async (field, fieldContext) =>
                        {
                            var isVisible = await field.IsVisibleAsync();

                            isVisible.Should().BeFalse();
                        },
                        tab: tab);
                }
                catch (Exception ex) when (ex.Message?.IndexOf("Unable to find a control", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                }
                catch (Exception ex)
                {
                    failures.Add(ex);
                }
            }

            if (failures.Any())
            {
                throw new AggregateException(failures);
            }
        }

        /// <summary>
        /// Verifies the specified fields contain the specified values.
        /// </summary>
        /// <param name="fields">The expected fields and values.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the following field values")]
        public async Task ThenISeeTheFollowingFieldValues(DataTable fields)
        {
            foreach (var r in fields.Rows)
            {
                await this.ExecuteGenericFieldActionAsync(
                    r["Field"],
                    async (field, fieldContext) =>
                    {
                        var value = await field.GetValueAsync(fieldContext.ControlType);
                        value.ToString().Should().Be(r["Value"]);
                    },
                    tab: await this.RecordPage.Form.GetActiveTabAsync());
            }
        }

        /// <summary>
        /// Asserts that the specified date field contains today's date.
        /// </summary>
        /// <param name="displayName">The display name of the date field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I can see todays date in the {string} field")]
        public async Task ThenICanSeeTodaysDateInTheField(string displayName)
        {
            await this.ExecuteControlActionAsync<IDateTime>(
                displayName,
                async (field, fieldContext) =>
                {
                    var actualValue = await field.GetValueAsync();

                    actualValue.Value.Date.Should().Be(DateTime.Today.Date);
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Asserts that the specified fields are empty.
        /// </summary>
        /// <param name="dataTable">Expected fields to be empty.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the following fields are empty")]
        public async Task ThenTheFollowingFieldsAreEmpty(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                await this.ExecuteGenericFieldActionAsync(
                    row["Field"],
                    async (field, fieldContext) =>
                    {
                        var value = await field.GetValueAsync(fieldContext.ControlType);
                        value.Should().BeNull();
                    },
                    tab: await this.RecordPage.Form.GetActiveTabAsync());
            }
        }

        /// <summary>
        /// Verifies the specified fields contain the specified values.
        /// </summary>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="fieldDisplayName">The expected fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see a value of {string} in the {string} field")]
        public async Task ThenISeeAValueOfInTheField(string expectedValue, string fieldDisplayName)
        {
            await this.ExecuteGenericFieldActionAsync(
                fieldDisplayName,
                async (field, fieldContext) =>
                {
                    var actualValue = await field.GetValueAsync(fieldContext.ControlType);

                    actualValue.ToString().Should().Be(expectedValue);
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Verifies the specified fields do not contain the specified values.
        /// </summary>
        /// <param name="unexpectedValue">The unexpected value.</param>
        /// <param name="fieldDisplayName">The expected fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I do not see a value of {string} in the {string} field")]
        public async Task ThenIDoNotSeeAValueOfInTheField(string unexpectedValue, string fieldDisplayName)
        {
            await this.ExecuteGenericFieldActionAsync(
                fieldDisplayName,
                async (field, fieldContext) =>
                {
                    var actualValue = await field.GetValueAsync(fieldContext.ControlType);

                    actualValue.Should().NotBe(unexpectedValue);
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Asserts the total row count on the subgrid.
        /// </summary>
        /// <param name="expectedCount">The expected count.</param>
        /// <param name="subgridDisplayName">The display name of the subgrid.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see {int} records in the {string} subgrid")]
        public async Task ThenISeeRecordsInTheSubgrid(int expectedCount, string subgridDisplayName)
        {
            await this.ExecuteGenericDataSetActionAsync(subgridDisplayName, async (dataSet, controlType) =>
            {
                var control = dataSet.GetControl(controlType);
                var count = await control.GetTotalRowCountAsync();

                count.Should().Be(expectedCount);
            });
        }

        /// <summary>
        /// Asserts the total row count on the editable grid.
        /// </summary>
        /// <param name="expectedCount">The expected count.</param>
        /// <param name="subgridDisplayName">The display name of the editable grid.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see {int} records in the {string} editable grid")]
        public async Task ThenISeeRecordsInTheEditableGrid(int expectedCount, string subgridDisplayName)
        {
            await this.ExecuteGenericEditableGridActionAsync(subgridDisplayName, async (editableGrid) =>
            {
                var count = await editableGrid.Control.GetTotalRowCountAsync();

                count.Should().Be(expectedCount);
            });
        }

        /// <summary>
        /// Asserts that the given subgrid is visible with the specified columns.
        /// </summary>
        /// <param name="subgridDisplayName">The display name of the subgrid.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I see a {string} subgrid")]
        [Then(@"I see an {string} subgrid")]
        public async Task ThenISeeTheSubgridWithTheFollowingColumns(string subgridDisplayName)
        {
            await this.ExecuteDataSetActionAsync<IReadOnlyGrid>(subgridDisplayName, async (subgrid) =>
            {
                var isVisible = await subgrid.IsVisibleAsync();

                isVisible.Should().BeTrue();
            });
        }

        /// <summary>
        /// Asserts that the given subgrid is visible with the specified columns.
        /// </summary>
        /// <param name="subgridDisplayName">The display name of the subgrid.</param>
        /// <param name="expectedColumns">The columns.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I see a {string} subgrid with the following columns")]
        [Then(@"I see a {string} grid with the following columns")]
        [Then(@"I see an {string} subgrid with the following columns")]
        [Then(@"I see an {string} grid with the following columns")]
        public async Task ThenISeeTheSubgridWithTheFollowingColumns(string subgridDisplayName, Table expectedColumns)
        {
            await this.ExecuteGenericDataSetActionAsync(
                subgridDisplayName,
                async (dataSet, controlType) =>
                {
                    var control = dataSet.GetControl(controlType);

                    var columns = await control.GetColumnNamesAsync();
                    var columnsToExpect = expectedColumns.Rows.Any() ? expectedColumns.Rows.Select(r => r[0]) : expectedColumns.Header;
                    columns.Should().BeEquivalentTo(columnsToExpect.ToArray());
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Asserts that the given editable grid is visible with the specified columns.
        /// </summary>
        /// <param name="editableGridDisplayName">The display name of the subgrid.</param>
        /// <param name="expectedColumns">The columns.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I see a {string} editable grid with the following columns")]
        [Then(@"I see a {string} editable grid with the following columns")]
        [Then(@"I see an {string} editable grid with the following columns")]
        [Then(@"I see an {string} editable grid with the following columns")]
        public async Task ThenISeeAnEditableGridWithTheFollowingColumns(string editableGridDisplayName, Table expectedColumns)
        {
            await this.ExecuteGenericEditableGridActionAsync(editableGridDisplayName, async (editablegrid) =>
            {
                // TODO: Implement editability check
                var actualColumns = await editablegrid.Control.GetColumnNamesAsync();
                var columnsToExpect = expectedColumns.Rows.Any() ? expectedColumns.Rows.Select(r => r[0]) : expectedColumns.Header;

                actualColumns.Should().BeEquivalentTo(columnsToExpect.ToArray());
            });
        }

        /// <summary>
        /// Asserts that the given editable grid columns are read-only.
        /// </summary>
        /// <param name="editableGridDisplayName">The display name of the subgrid.</param>
        /// <param name="columns">The columns.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the following columns on the {string} editable grid are read-only")]
        public async Task ThenTheFollowingColumnsOnTheGridAreReadOnly(string editableGridDisplayName, DataTable columns)
        {
            var logicalName = await this.GetGridLogicalNameAsync(editableGridDisplayName);
            var editableGrid = this.RecordPage.Form.GetDataSet<IGridControl>(logicalName).Control;

            var editableColumns = await editableGrid.GetEditableColumnsAsync(0);

            editableColumns.Should().NotContain(columns.Rows.Select(r => r[0]));
        }

        /// <summary>
        /// Asserts that the given subgrid is visible with the specified columns.
        /// </summary>
        /// <param name="should">Whether or not the command should be visible.</param>
        /// <param name="command">The command.</param>
        /// <param name="subgridDisplayName">The display name of the subgrid.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I(| do not) see a '(.*)' command on the '(.*)' grid")]
        [Then(@"I(| do not) see an '(.*)' command on the '(.*)' grid")]
        public async Task ThenISeeACommandOnTheGrid(bool should, string command, string subgridDisplayName)
        {
            await this.ExecuteDataSetActionAsync<IReadOnlyGrid>(subgridDisplayName, async (subgrid) =>
            {
                var isVisible = await subgrid.IsVisibleAsync();
                isVisible.Should().BeTrue();

                var commands = await subgrid.CommandBar.GetCommandsAsync();

                if (should)
                {
                    commands.Should().Contain(command);
                }
                else
                {
                    commands.Should().NotContain(command);
                }
            });
        }

        /// <summary>
        /// Asserts that the related gridis visible with the specified columns.
        /// </summary>
        /// <param name="should">Whether or not the command should be visible.</param>
        /// <param name="command">The command.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I(| do not) see a '(.*)' command on the related grid")]
        [Then(@"I(| do not) see an '(.*)' command on the related grid")]
        public async Task ThenISeeACommandOnTheRelatedGrid(bool should, string command)
        {
            var relatedGrid = await this.RecordPage.Form.GetRelatedDataSetAsync();

            var commands = await relatedGrid.CommandBar.GetCommandsAsync();

            if (should)
            {
                commands.Should().Contain(command);
            }
            else
            {
                commands.Should().NotContain(command);
            }
        }

        /// <summary>
        /// Asserts the speciefied command is visible within the editable grid.
        /// </summary>
        /// <param name="should">Whether or not the command should be visible.</param>
        /// <param name="command">The command.</param>
        /// <param name="subgridDisplayName">The display name of the editable grid.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I(| do not) see the '(.*)' command on the '(.*)' editable grid")]
        public async Task ThenIDoNotSeeTheCommandOnTheEditableGrid(bool should, string command, string subgridDisplayName)
        {
            var formId = await this.RecordPage.GetFormIdAsync();
            var tab = await this.RecordPage.Form.GetActiveTabAsync();
            var subgridName = this.formMetadataSvc.GetControlLogicalName(formId, subgridDisplayName, out _, tab: tab);

            var subgrid = this.RecordPage.Form.GetDataSet<IGridControl>(subgridName);
            var commands = await subgrid.CommandBar.GetCommandsAsync();

            if (should)
            {
                commands.Should().Contain(command);
            }
            else
            {
                commands.Should().NotContain(command);
            }
        }

        /// <summary>
        /// Asserts that the given subgrid is visible with the specified columns.
        /// </summary>
        /// <param name="should">Whether or not the command should be visible.</param>
        /// <param name="subgridDisplayName">The display name of the subgrid.</param>
        /// <param name="expectedCommands">The expected commands.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"I(| do not) see the following commands on the '(.*)' subgrid")]
        public async Task ThenISeeACommandOnTheGrid(bool should, string subgridDisplayName, DataTable expectedCommands)
        {
            await this.ExecuteDataSetActionAsync<IReadOnlyGrid>(subgridDisplayName, async (dataSet) =>
            {
                var commands = await dataSet.CommandBar.GetCommandsAsync();
                await this.RecordPage.Page.Keyboard.PressAsync("Escape"); // Workaround to dismiss flyout
                var missingCommands = expectedCommands.Rows.Select(r => r[0]).Except(commands);

                if (should)
                {
                    Assert.IsEmpty(missingCommands);
                }
                else
                {
                    Assert.AreEqual(expectedCommands.Rows.Count, missingCommands.Count());
                }
            });
        }

        /// <summary>
        /// Asserts that the selected row has a given value for a given column.
        /// </summary>
        /// <param name="subgridDisplayName">The name of the subgrid.</param>
        /// <param name="column">The column name.</param>
        /// <param name="value">The expected value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the previously selected row in the {string} editable grid has an {string} value of {string}")]
        public async Task ThenTheSelectedRowInTheEditableGridHasAnValueOf(string subgridDisplayName, string column, string value)
        {
            if (!this.ctx.TryGetValue<int>(ScenarioContextKeys.SelectedRow, out var selectedRow))
            {
                throw new InvalidOperationException("No selected row found in the context.");
            }

            await this.ExecuteGenericEditableGridActionAsync(subgridDisplayName, async (dataSet) =>
            {
                await dataSet.CommandBar.ClickCommandAsync("Refresh");
                var data = await dataSet.Control.GetRowDataAsync();
                var selectedRowValue = data.ElementAt(selectedRow)[column];

                selectedRowValue.Should().Be(value);
            });
        }

        /// <summary>
        /// Asserts that the selected row has a given value for a given column.
        /// </summary>
        /// <param name="subgridDisplayName">The name of the subgrid.</param>
        /// <param name="column">The column name.</param>
        /// <param name="value">The expected value.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the previously selected row in the {string} subgrid has an {string} value of {string}")]
        public async Task ThenTheSelectedRowInTheSubgridHasAnValueOf(string subgridDisplayName, string column, string value)
        {
            if (!this.ctx.TryGetValue<int>(ScenarioContextKeys.SelectedRow, out var selectedRow))
            {
                throw new InvalidOperationException("No selected row found in the context.");
            }

            await this.ExecuteDataSetActionAsync<IReadOnlyGrid>(subgridDisplayName, async (dataSet) =>
            {
                var data = await dataSet.Control.GetRowDataAsync();
                var selectedRowValue = data.ElementAt(selectedRow)[column];

                selectedRowValue.Should().Be(value);
            });
        }

        /// <summary>
        /// Asserts that the form is editable.
        /// </summary>
        /// <param name="table">The tabs.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the following tabs")]
        public async Task ISeeTheFollowingTabs(DataTable table)
        {
            var tabs = await this.RecordPage.Form.GetAllTabsAsync();

            tabs.Should().BeEquivalentTo(table.Header.ToArray(), "because the tabs should be visible");
        }

        /// <summary>
        /// Asserts that the tabs are not visible.
        /// </summary>
        /// <param name="table">The tabs.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I do not see the following tabs")]
        public async Task IDoNotSeeTheFollowingTabs(DataTable table)
        {
            var tabs = await this.RecordPage.Form.GetAllTabsAsync();

            tabs.Should().NotContain(table.Header.ToArray(), "because these tabs should not be visible");
        }

        /// <summary>
        /// Asserts that a tab is visible.
        /// </summary>
        /// <param name="tab">The tab name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the {string} tab")]
        public async Task ThenISeeTheTab(string tab)
        {
            var tabs = await this.RecordPage.Form.GetAllTabsAsync();

            tabs.Should().Contain(tab, "because the tab should be visible");
        }

        /// <summary>
        /// Asserts that the form is read-only.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the form is read-only")]
        public async Task ThenTheFormIsReadOnly()
        {
            var isReadOnly = await this.RecordPage.Form.IsDisabledAsync();

            isReadOnly.Should().BeTrue("because the form should be read-only");
        }

        /// <summary>
        /// Asserts that the form is editable.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the form is editable")]
        public async Task ThenTheFormIsEditable()
        {
            var isReadOnly = await this.RecordPage.Form.IsDisabledAsync();

            isReadOnly.Should().BeFalse("because the form should be editable");
        }

        /// <summary>
        /// Verifies that a field is populated.
        /// </summary>
        /// <param name="fieldName">The field label.</param>
        /// <param name="shouldBePopulated">Whether or not it should be populated.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"^the '(.*)' field (is|is not) populated")]
        public async Task ThenTheFieldIsOrIsNotPopulated(string fieldName, bool shouldBePopulated)
        {
            await this.ExecuteGenericFieldActionAsync(
                fieldName,
                async (field, fieldContext) =>
                {
                    var value = await field.GetValueAsync(fieldContext.ControlType);

                    if (shouldBePopulated)
                    {
                        value.Should().NotBeNull();
                    }
                    else
                    {
                        value.Should().BeNull();
                    }
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Verifies that multiple fields are populated or not populated.
        /// </summary>
        /// <param name="shouldBePopulated">Whether or not it should be populated.</param>
        /// <param name="fields">The fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"^the following fields (are|are not) populated")]
        public async Task ThenTheFollowingFieldsAreOrAreNotPopulated(bool shouldBePopulated, Table fields)
        {
            var tasks = fields.Rows.Select(async r => await this.ExecuteGenericFieldActionAsync(
                r[0],
                async (field, fieldContext) =>
                {
                    var value = await field.GetValueAsync(fieldContext.ControlType);

                    if (shouldBePopulated)
                    {
                        value.Should().NotBeNull();
                    }
                    else
                    {
                        value.Should().BeNull();
                    }
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync()));

            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// Verifies that two fields are or aren't set to the same value.
        /// </summary>
        /// <param name="targetField">The target field.</param>
        /// <param name="should">Whether they should be the same value.</param>
        /// <param name="sourceField">The source field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"the '(.*)' field (is|is not) set to the same value as the '(.*)' field")]
        public async Task ThenTheFollowingFieldsAreOrAreNotPopulated(string targetField, bool should, string sourceField)
        {
            var formId = await this.RecordPage.GetFormIdAsync();
            var sourceFieldType = this.powerPlaywrightMetadataSvc.GetPowerPlaywrightControlClass(formId, sourceField);
            var sourceFieldLogicalName = this.formMetadataSvc.GetControlLogicalName(formId, sourceField, out _);
            var sourceValue = await this.RecordPage.Form.GetField(sourceFieldLogicalName).GetValueAsync(sourceFieldType);

            var targetFieldType = this.powerPlaywrightMetadataSvc.GetPowerPlaywrightControlClass(formId, targetField);
            var targetFieldLogicalName = this.formMetadataSvc.GetControlLogicalName(formId, targetField, out _);
            var targetValue = await this.RecordPage.Form.GetField(targetFieldLogicalName).GetValueAsync(targetFieldType);

            if (should)
            {
                sourceValue.Should().Be(targetValue);
            }
            else
            {
                sourceValue.Should().NotBe(targetValue);
            }
        }

        /// <summary>
        /// Asserts that a field is read-only.
        /// </summary>
        /// <param name="displayName">The field dislay name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the {string} field is read-only")]
        public async Task ThenTheFieldIsReadOnly(string displayName)
        {
            await this.ExecuteGenericFieldActionAsync(displayName, async (field, fieldContext) =>
            {
                var actualDisabled = fieldContext.IsQuickView || await field.IsDisabledAsync(); // Workaround due to bug in Power Playwright that returns true for IsDisabledAsync for quick view fields.

                actualDisabled.Should().BeTrue();
            });
        }

        /// <summary>
        /// Asserts a lookup field is populated with the current user.
        /// </summary>
        /// <param name="controlDisplayName">The control display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then(@"'(.*)' field is populated with the current user")]
        public async Task ThenFieldIsPopulatedWithTheCurrentUser(string controlDisplayName)
        {
            await this.ExecuteControlActionAsync<ILookup>(
                controlDisplayName,
                async (control, fieldContext) =>
                {
                    var actualValue = Regex.Replace(await control.GetValueAsync(), @"\s*\(Offline\)", string.Empty);

                    var expectedValue = await this.RecordPage.Page.EvaluateAsync<string>("() => Xrm.Utility.getGlobalContext().userSettings.userName");

                    actualValue.Should().Be(expectedValue);
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Asserts that a quick create can be saved.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the record is successfully added to the subgrid")]
        public async Task ThenICanSaveANewRecordToTheSubgrid()
        {
            if (!this.ctx.TryGetValue<string>(ScenarioContextKeys.AddNewToSubgridName, out var subgridName) || !this.ctx.TryGetValue<int>(ScenarioContextKeys.AddNewToSubgridTotalRowCount, out var subgridRowCount))
            {
                throw new InvalidOperationException("A record is not being added to a subgrid.");
            }

            var newTotalRowCount = await this.RecordPage.Form.GetDataSet<IReadOnlyGrid>(subgridName).Control.GetTotalRowCountAsync();

            newTotalRowCount.Should().BeGreaterThan(subgridRowCount, "because a new record should have been added to the subgrid");
        }

        /// <summary>
        /// Asserts that all fields provided in the table are visible on the form.
        /// </summary>
        /// <param name="expectedVisibility">The expected visibility.</param>
        /// <param name="fields">The table with the fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous job.</returns>
        [Then("^I (can|cannot) see the following fields on the form$")]
        [Then("^I (can|cannot) see the following fields$")]
        public async Task ThenICanOrCannotSeeTheFollowingFieldsOnTheForm(bool expectedVisibility, DataTable fields)
        {
            var visibilityResults = new ConcurrentDictionary<string, bool>();

            foreach (var fieldName in fields.Rows.Select(r => r["Field"]))
            {
                await this.ExecuteGenericFieldActionAsync(fieldName, async (field, fieldContext) =>
                {
                    visibilityResults.TryAdd(field.Name, await field.IsVisibleAsync());
                });
            }

            var fieldsWithIncorrectVisibility = visibilityResults.Where(kvp => kvp.Value != expectedVisibility).Select(kvp => kvp.Key);
            if (fieldsWithIncorrectVisibility.Any())
            {
                throw new Exception($"The following fields were not {(expectedVisibility ? "visible" : "hidden")} when expected: {string.Join(",", fieldsWithIncorrectVisibility)}.");
            }
        }

        /// <summary>
        /// Asserts that the record has been successfully created via the UI.
        /// </summary>
        /// <param name="tableDisplayName">The table display name.</param>
        [Then("the {string} record is successfully saved")]
        public void ThenTheRecordIsSuccessfullySaved(string tableDisplayName)
        {
            var expectedLogicalName = this.entityMetadataSvc.GetTableLogicalName(tableDisplayName);
            var actualLogicalName = this.RecordPage.GetEntityLogicalName();
            var actualRecordId = this.RecordPage.GetRecordId();

            actualRecordId.Should().NotBe(Guid.Empty, "because the record should have been saved and have a valid GUID");
            actualLogicalName.Should().Be(expectedLogicalName, $"the {expectedLogicalName} form should be displayed when creating a new record, but found {actualLogicalName}.");
        }

        /// <summary>
        /// Asserts that the provided fields are lookups and are visible on the form.
        /// </summary>
        /// <param name="fields">The field display names.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous job.</returns>
        [Then("the following fields allow me to navigate to a related record")]
        public async Task ThenTheFollowingFieldsAllowMeToNavigateToARelatedRecord(DataTable fields)
        {
            var expectedLookups = fields.Rows.Select(r => r[0]);

            var formId = await this.RecordPage.GetFormIdAsync();
            var logicalNames = expectedLookups.Select(l => this.formMetadataSvc.GetControlLogicalName(formId, l, out _));

            var exceptions = new List<Exception>();
            foreach (var logicalName in logicalNames)
            {
                var field = this.RecordPage.Form.GetField<ILookup>(logicalName);

                if (await field.Control.Container.IsVisibleAsync() == false)
                {
                    exceptions.Add(new AssertFailedException($"Expected lookup field '{logicalName}' to be visible, but it was not."));
                }
            }

            if (exceptions.Any())
            {
                throw new AssertFailedException(exceptions.Select(e => e.Message).Aggregate((a, b) => a + Environment.NewLine + b));
            }
        }

        /// <summary>
        /// Asserts that the provided fields in the editable grid contain the expected values.
        /// </summary>
        /// <param name="editableGridDisplayName">The display name of the editable grid.</param>
        /// <param name="fields">The table with the expected field values.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous job.</returns>
        [Then("I see the following values in the {string} editable grid")]
        public async Task ThenISeeTheFollowingValuesInTheEditableGrid(string editableGridDisplayName, DataTable fields)
        {
            var logicalName = await this.GetGridLogicalNameAsync(editableGridDisplayName);
            var editableGrid = this.RecordPage.Form.GetDataSet<IGridControl>(logicalName).Control;
            var assertionExceptions = new List<Exception>();

            var rowData = await editableGrid.GetRowDataAsync();
            foreach (var row in fields.Rows)
            {
                foreach (var key in row.Keys)
                {
                    try
                    {
                        var value = rowData.Select(r => r[key]).FirstOrDefault();
                        value.Should().Be(row[key]);
                    }
                    catch (Exception ex)
                    {
                        assertionExceptions.Add(ex);
                    }
                }
            }

            if (assertionExceptions.Count > 0)
            {
                throw new AggregateException("Errors encountered while performing assertions", assertionExceptions);
            }
        }

        /// <summary>
        /// Asserts that the specified field is mandatory or not.
        /// </summary>
        /// <param name="fieldDisplayName">The display name of the field.</param>
        /// <param name="shouldText">Indicates whether the field should be mandatory or not.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous job.</returns>
        [Then("the '(.*?)' field (is|is not) mandatory")]
        public async Task ThenTheFieldIsMandatory(string fieldDisplayName, string shouldText)
        {
            await this.ExecuteGenericFieldActionAsync(
                fieldDisplayName,
                async (field, fieldContext) =>
                {
                    var requirementLevel = await field.GetRequirementLevelAsync();
                    var isMandatory = requirementLevel.ToString().Equals("Required", StringComparison.OrdinalIgnoreCase);

                    var should = shouldText == "is";

                    if (should)
                    {
                        isMandatory.Should().BeTrue();
                    }
                    else
                    {
                        isMandatory.Should().BeFalse();
                    }
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Asserts that the specified columns in the editable grid contain the expected error messages.
        /// </summary>
        /// <param name="editableGridDisplayName">The display name of the editable grid.</param>
        /// <param name="errors">The expected error messages.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous job.</returns>
        [Then("I see an error message in the {string} editable grid for the following columns")]
        public async Task ThenISeeAnErrorMessageInTheEditableGridForTheFollowingColumns(string editableGridDisplayName, DataTable errors)
        {
            var logicalName = await this.GetGridLogicalNameAsync(editableGridDisplayName);
            var editableGrid = this.RecordPage.Form.GetDataSet<IGridControl>(logicalName).Control;

            var actualErrors = await editableGrid.GetErrorNotificationsAsync();
            var expectedErrors = errors.Rows.ToDictionary(r => r["Column"], r => r["Message"]);

            actualErrors.Should().BeEquivalentTo(expectedErrors);
        }

        /// <summary>
        /// Asserts all columns within an editable grid are read-only.
        /// </summary>
        /// <param name="editableGridDisplayName">Editable grid friendly name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous job.</returns>
        [Then("all columns on the {string} editable grid are read-only")]
        public async Task ThenAllColumnsOnTheEditableGridAreReadOnly(string editableGridDisplayName)
        {
            var logicalName = await this.GetGridLogicalNameAsync(editableGridDisplayName);
            var editableGrid = this.RecordPage.Form.GetDataSet<IGridControl>(logicalName).Control;

            var selectedRows = await editableGrid.GetSelectedRowsAsync();
            foreach (var rowIndex in selectedRows)
            {
                var editableColumns = await editableGrid.GetEditableColumnsAsync(rowIndex);
                editableColumns.Count().Should().Be(0);
            }
        }

        /// <summary>
        /// Asserts the commands are or are not displayed.
        /// </summary>
        /// <param name="should">Whether or not the commands are displayed.</param>
        /// <param name="editableGridDisplayName">Editable grid friendly name.</param>
        /// <param name="expectedCommands">The command names.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous job.</returns>
        [Then(@"I(| do not) see the following commands on the '(.*)' editable grid")]
        public async Task ThenIDoNotSeeTheFollowingCommandsOnTheEditableGrid(bool should, string editableGridDisplayName, DataTable expectedCommands)
        {
            await this.ExecuteGenericDataSetActionAsync(editableGridDisplayName, async (dataSet, controlType) =>
            {
                var commands = await dataSet.CommandBar.GetCommandsAsync();
                var missingCommands = expectedCommands.Rows.Select(r => r[0]).Except(commands);
                if (should)
                {
                    Assert.IsEmpty(missingCommands);
                }
                else
                {
                    Assert.AreEqual(expectedCommands.Rows.Count, missingCommands.Count());
                }

                await this.RecordPage.Page.Keyboard.PressAsync("Escape"); // Workaround to dismiss flyout
            });
        }

        /// <summary>
        /// Asserts the columns within the editable grid contains data.
        /// </summary>
        /// <param name="editableGridDisplayName">Editable grid friendly name.</param>
        /// <param name="expectedColumns">The expected columns.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous job.</returns>
        [Then("I see a value in the following columns in the {string} editable grid")]
        public async Task ThenISeeAValueInTheFollowingColumnsInTheEditableGrid(string editableGridDisplayName, DataTable expectedColumns)
        {
            var logicalName = await this.GetGridLogicalNameAsync(editableGridDisplayName);
            var editableGrid = this.RecordPage.Form.GetDataSet<IGridControl>(logicalName);

            var dataRow = (await editableGrid.Control.GetRowDataAsync()).First();
            foreach (var column in expectedColumns.Header)
            {
                dataRow[column].Should().NotBeNullOrEmpty();
            }
        }

        /// <summary>
        /// Asserts the options within a choice field.
        /// </summary>
        /// <param name="fieldName">The field.</param>
        /// <param name="table">The expected options.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous job.</returns>
        [Then("I can see the following options in the {string} field")]
        public async Task ThenICanSeeTheFollowingOptionsInTheField(string fieldName, DataTable table)
        {
            await this.ExecuteGenericFieldActionAsync(fieldName, async (field, fieldContext) =>
            {
                if (fieldContext.ControlType != typeof(IChoice))
                {
                    throw new InvalidOperationException($"Field '{fieldName}' is not a choice field.");
                }

                var expectedOptions = table.Rows.Select(r => r[0]);

                var choice = field.GetControl<IChoice>();
                var actualOptions = await choice.GetAllOptionsAsync();

                actualOptions.Should().BeEquivalentTo(expectedOptions, "because the field options should match the expected options");
            });
        }

        /// <summary>
        /// Asserts that a price list can be seen in the results of a lookup control.
        /// </summary>
        /// <param name="priceListName">The price list name.</param>
        /// <param name="controlDisplayName">The control display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I can see {string} price list within the {string} lookup results view")]
        public async Task ThenICanSeePriceListWithinTheLookupResultsView(string priceListName, string controlDisplayName)
        {
            var formId = await this.RecordPage.GetFormIdAsync();
            var logicalName = this.formMetadataSvc.GetControlLogicalName(formId, controlDisplayName, out _);
            var control = this.RecordPage.Form.GetField<ILookup>(logicalName).Control;

            var filteredItems = await control.GetSearchResultsAsync();
            filteredItems.Should().Contain(i => i.Contains(priceListName), because: $"the {controlDisplayName} lookup should contain the {priceListName} price list");
        }

        /// <summary>
        /// Verifies the specified fields contain the specified values.
        /// </summary>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="fieldDisplayName">The expected fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see a value of {decimal} in the {string} field")]
        public async Task ThenISeeADecimalValueOfInTheField(decimal expectedValue, string fieldDisplayName)
        {
            await this.ExecuteGenericFieldActionAsync(
                fieldDisplayName,
                async (field, fieldContext) =>
                {
                    var actualValue = await field.GetValueAsync(fieldContext.ControlType);

                    actualValue.Should().Be(expectedValue);
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Verifies the specified field calculated contain the specified values.
        /// </summary>
        /// <param name="expectedValue">The expected value.</param>
        /// <param name="fieldDisplayName">The expected fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see a value of {decimal} in the {string} calculated field")]
        public async Task ThenISeeADecimalValueOfInTheCalculatedField(decimal expectedValue, string fieldDisplayName)
        {
            await this.ExecuteGenericFieldActionAsync(
                fieldDisplayName,
                async (field, fieldContext) =>
                {
                    await field.RecalculateAsync();
                    var actualValue = await field.GetValueAsync(fieldContext.ControlType);

                    actualValue.Should().Be(expectedValue);
                },
                tab: await this.RecordPage.Form.GetActiveTabAsync());
        }

        /// <summary>
        /// Asserts rows within the given editable grid contain the expected values for the specified columns.
        /// </summary>
        /// <param name="controlDisplayName">The display name of the control.</param>
        /// <param name="dataTable">The expected values for the editable grid.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the following values in the {string} editable grid for all rows")]
        public async Task ThenISeeTheFollowingValuesInTheEditableGridForAllRows(string controlDisplayName, DataTable dataTable)
        {
            await this.ExecuteDataSetActionAsync<IGridControl>(controlDisplayName, async (subgrid) =>
            {
                var expected = dataTable.Rows
                    .Select(row => row.Keys.ToDictionary(k => k, k => row[k]))
                    .First();

                var actualRows = (await subgrid.Control.GetRowDataAsync())
                    .Select(r => r.Keys.ToDictionary(k => k, k => r[k]))
                    .ToArray();

                foreach (var row in actualRows)
                {
                    var actualData = row.Where(kvp => dataTable.Header.Contains(kvp.Key))
                        .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

                    actualData.Should().BeEquivalentTo(expected);
                }
            });
        }

        /// <summary>
        /// Asserts the values within a lookup view.
        /// </summary>
        /// <param name="should">Whether the values should be visible.</param>
        /// <param name="controlDisplayName">The control display name.</param>
        /// <param name="dataTable">The expected values.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I (can|cannot) see the following values within the '(.*)' lookup results view")]
        public async Task ThenICanSeeTheFollowingValuesWithinTheLookupResultsView(bool should, string controlDisplayName, DataTable dataTable)
        {
            var expected = dataTable.Rows.Select(x => x["Value"]);

            await this.ExecuteControlActionAsync<ISimpleLookupControl>(controlDisplayName, async (field, fieldContext) =>
            {
                var actual = await field.GetSearchResultsAsync();

                if (should)
                {
                    actual.SelectMany(x => x).Should().BeEquivalentTo(expected);
                }
                else
                {
                    actual.SelectMany(x => x).Should().NotContain(expected);
                }
            });
        }

        /// <summary>
        /// Asserts field values within a form.
        /// </summary>
        /// <param name="dataTable">Expected field values.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I can see the following field values")]
        public async Task ThenICanSeeTheFollowingFieldValues(DataTable dataTable)
        {
            foreach (var row in dataTable.Rows)
            {
                await this.ExecuteGenericFieldActionAsync(
                    row["Field"],
                    async (field, fieldContext) =>
                    {
                        string actualValue = (await field.GetValueAsync(fieldContext.ControlType)).ToString();

                        actualValue.Should().Be(row["Value"], because: $"the field should contain {row["Value"]}.");
                    },
                    tab: await this.RecordPage.Form.GetActiveTabAsync());
            }
        }

        /// <summary>
        /// Compares two field values.
        /// </summary>
        /// <param name="source">Source field.</param>
        /// <param name="comparison">Field comparison.</param>
        /// <param name="target">Target field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^'([^']*)' (equals|does not equal) '([^']*)'")]
        public async Task ThenEquals(string source, FieldComparison comparison, string target)
        {
            var activeTab = await this.RecordPage.Form.GetActiveTabAsync();
            var sourceValue = await this.ExecuteGenericFieldActionAsync(
                source,
                async (f, sourceContext) =>
                {
                    return (await f.GetValueAsync(sourceContext.ControlType)).ToString();
                },
                tab: activeTab);

            var targetValue = await this.ExecuteGenericFieldActionAsync(
                target,
                async (f, targetContext) =>
                {
                    return (await f.GetValueAsync(targetContext.ControlType)).ToString();
                },
                tab: activeTab);

            CompareFieldValues(sourceValue, targetValue, comparison, source, target);
        }

        /// <summary>
        /// Asserts that a field is x number of days in the future/past of a given date field.
        /// </summary>
        /// <param name="fieldDisplayName">The field display name.</param>
        /// <param name="numberOfDays">The number of days.</param>
        /// <param name="dateDirection">The date direction.</param>
        /// <param name="referenceFieldDisplayName">The reference field display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("{string} is {int} days in the {word} from {string}")]
        public async Task ThenIsDaysAfter(string fieldDisplayName, int numberOfDays, DateDirection dateDirection, string referenceFieldDisplayName)
        {
            string activeTab = null;
            if (await this.RecordPage.Form.Container.IsVisibleAsync())
            {
                activeTab = await this.RecordPage.Form.GetActiveTabAsync();
            }

            DateTime? value = null;
            await this.ExecuteControlActionAsync<IDate>(
                fieldDisplayName,
                async (f, sourceContext) =>
                {
                    value = await f.GetValueAsync();
                },
                tab: activeTab);

            DateTime? referenceValue = null;
            await this.ExecuteControlActionAsync<IDate>(
                referenceFieldDisplayName,
                async (f, targetContext) =>
                {
                    referenceValue = await f.GetValueAsync();
                },
                tab: activeTab);

            var expectedValue = dateDirection == DateDirection.Future ?
                referenceValue.Value.AddDays(numberOfDays)
                :
                referenceValue.Value.AddDays(-numberOfDays);

            value.Value.Date.Should().Be(expectedValue.Date);
        }

        /// <summary>
        /// Asserts that the quick create has saved successfully (no longer visible).
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the quick create is saved successfully")]
        public async Task ThenTheQuickCreateIsSavedSuccessfully()
        {
            if (!this.ctx.TryGetValue<IQuickCreateForm>(nameof(IQuickCreateForm), out var quickCreate))
            {
                throw new Exception("No quick create was found in the context.");
            }

            var quickCreateVisible = await quickCreate.Container.IsVisibleAsync();

            quickCreateVisible.Should().Be(false);
        }

        /// <summary>
        /// Compares two field values.
        /// </summary>
        /// <param name="source">Source field.</param>
        /// <param name="comparison">Field comparison.</param>
        /// <param name="target">Target field.</param>
        /// <param name="modifier">The modifier.</param>
        /// <param name="targetModifier">Target modifier field.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^'([^']*)' (equals|does not equal) '([^']*)' (multiplied by) '([^']*)'")]
        public async Task ThenEqualsModifier(string source, FieldComparison comparison, string target, FieldModifier modifier, string targetModifier)
        {
            var activeTab = await this.RecordPage.Form.GetActiveTabAsync();
            var sourceValue = await this.ExecuteGenericFieldActionAsync(
                source,
                async (f, sourceContext) =>
                {
                    return (await f.GetValueAsync(sourceContext.ControlType)).ToString();
                },
                tab: activeTab);

            var targetValue = await this.ExecuteGenericFieldActionAsync(
                target,
                async (f, targetContext) =>
                {
                    return (await f.GetValueAsync(targetContext.ControlType)).ToString();
                },
                tab: activeTab);

            var targetModifierValue = await this.ExecuteGenericFieldActionAsync(
                targetModifier,
                async (f, targetContext) =>
                {
                    return (await f.GetValueAsync(targetContext.ControlType)).ToString();
                },
                tab: activeTab);

            targetValue = ModifyFieldValues(targetValue, targetModifierValue, modifier);

            if (decimal.TryParse(sourceValue, out var sourceDecimal) && decimal.TryParse(targetValue, out var targetDecimal))
            {
                sourceValue = sourceDecimal.ToString("0.####");
                targetValue = targetDecimal.ToString("0.####");
            }

            CompareFieldValues(sourceValue, targetValue, comparison, source, $"{target} {modifier} {targetModifier}");
        }

        /// <summary>
        /// Asserts the given message is seen within the editable grid error notification area.
        /// </summary>
        /// <param name="message">The expected message.</param>
        /// <param name="controlDisplayName">The display name of the control.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I see an alert message '(.*)' within the '(.*)' editable grid")]
        public async Task ThenISeeAnAlertMessageWithinTheEditableGrid(string message, string controlDisplayName)
        {
            await this.ExecuteGenericEditableGridActionAsync(controlDisplayName, async (grid) =>
            {
                var actualMessage = await grid.Control.GetAlertMessageAsync();
                actualMessage.Should().Be(message, because: $"the alert message should be {message}");
            });
        }

        /// <summary>
        /// Asserts the entity form type.
        /// </summary>
        /// <param name="displayName">Entity display name.</param>
        [Then("I see the {string} form")]
        public void ThenISeeTheForm(string displayName)
        {
            var entityLogicalName = this.entityMetadataSvc.GetTableLogicalName(displayName);

            var match = FormUrlRegex(entityLogicalName).Match(this.RecordPage.Page.Url).Success;

            match.Should().BeTrue();
        }

        private static string ModifyFieldValues(string sourceValue, string targetValue, FieldModifier modifier)
        {
            switch (modifier)
            {
                case FieldModifier.MultipliedBy:
                    return (Convert.ToDecimal(sourceValue) * Convert.ToDecimal(targetValue)).ToString();
                default:
                    throw new NotSupportedException($"The {modifier} modifier is not yet supported.");
            }
        }

        private static void CompareFieldValues(string sourceValue, string targetValue, FieldComparison comparison, string sourceField, string targetField)
        {
            switch (comparison)
            {
                case FieldComparison.Equals:
                    sourceValue.Should().Be(targetValue, because: $"{targetField} should equal {sourceField}.");
                    break;
            }
        }

        private static bool IsHeaderField(string logicalName) =>
            logicalName?.StartsWith("header_", StringComparison.OrdinalIgnoreCase) == true;

        private static string ReplaceTemplatedValue(string template, Entity record, string field, string placeholder)
        {
            var replacement = string.Empty;

            if (record.FormattedValues.Contains(field))
            {
                replacement = record.FormattedValues[field];
            }
            else if (record.Attributes.Contains(field) && record[field] != null)
            {
                replacement = record[field].ToString();
            }

            if (record[field] is Money m)
            {
                replacement = "£" + ((m.Value % 1 == 0) ? m.Value.ToString("0") : m.Value.ToString("0.##"));
            }

            return template.Replace($"{{{{{placeholder}}}}}", replacement);
        }

        private static IEnumerable<string> GetFieldsFromTemplate(string expectedMessage)
        {
            return Regex.Matches(expectedMessage, @"{{\s*(.+?)\s*}}")
                .Cast<Match>()
                .Select(m => m.Groups[1].Value);
        }

        private static Regex FormUrlRegex(string entityLogicalName)
        {
            return new Regex($@".*pagetype=entityrecord&etn={Regex.Escape(entityLogicalName)}.*", RegexOptions.Compiled);
        }

        private async Task<IField> ResolveFieldAsync(string fieldLogicalName, FieldLocation fieldLocation, IQuickCreateForm quickCreate = null)
        {
            if (quickCreate != null)
            {
                return quickCreate.GetField(fieldLogicalName);
            }
            else if (fieldLocation == FieldLocation.Header)
            {
                var header = await this.RecordPage.Form.ExpandHeaderAsync();

                return header.GetField(fieldLogicalName.Replace("header_", string.Empty));
            }

            return this.RecordPage.Form.GetField(fieldLogicalName);
        }

        private async Task ExecuteDataSetActionAsync<TControl>(string subgridDisplayName, Func<IDataSet<TControl>, Task> action)
            where TControl : IPcfControl
        {
            var formId = await this.RecordPage.GetFormIdAsync();

            var match = Regex.Match(subgridDisplayName, "<([^>]+)>");
            var fieldLogicalName = match.Success ? match.Groups[1].Value : this.formMetadataSvc.GetControlLogicalName(formId, subgridDisplayName, out _);
            var dataSet = this.RecordPage.Form.GetDataSet<TControl>(fieldLogicalName.Split('.').Last());

            await action(dataSet);
        }

        private async Task ExecuteGenericDataSetActionAsync(string subgridDisplayName, Func<IDataSet, Type, Task> action, string tab = null)
        {
            string fieldLogicalName = string.Empty;
            var formId = await this.RecordPage.GetFormIdAsync();
            Guid fieldFormId = formId;

            var match = Regex.Match(subgridDisplayName, "<([^>]+)>");
            fieldLogicalName = match.Success ? match.Groups[1].Value : this.formMetadataSvc.GetControlLogicalName(formId, subgridDisplayName, out fieldFormId, tab: tab);
            var dataSet = this.RecordPage.Form.GetDataSet(fieldLogicalName.Split('.').Last());
            var controlType = this.ResolveDataSetType(fieldFormId, fieldLogicalName.Split('.').Last());

            await action(dataSet, controlType);
        }

        private async Task<string> GetGridLogicalNameAsync(string displayName)
        {
            var formId = await this.RecordPage.GetFormIdAsync();
            var match = Regex.Match(displayName, "<([^>]+)>");
            var overriddenLogicalName = match.Success ? match.Groups[1].Value : null;
            var activeTab = await this.RecordPage.Form.GetActiveTabAsync();
            var subgridName = overriddenLogicalName ?? this.formMetadataSvc.GetControlLogicalName(formId, displayName, out _, tab: activeTab);

            if (subgridName.Contains('.'))
            {
                subgridName = subgridName.Split('.')[1];
            }

            return subgridName;
        }

        private FieldLocation ResolveFieldLocation(string fieldLogicalName, Guid fieldFormId, string fieldName)
        {
            if (!string.IsNullOrEmpty(fieldLogicalName))
            {
                return IsHeaderField(fieldLogicalName)
                    ? FieldLocation.Header
                    : FieldLocation.Body;
            }

            return this.formMetadataSvc.GetControlLocation(fieldFormId, fieldName);
        }

        private async Task<(Guid formId, IQuickCreateForm quickCreate)> ResolveFormAsync()
        {
            this.ctx.TryGetValue<IQuickCreateForm>(nameof(IQuickCreateForm), out var quickCreate);

            if (quickCreate != null && await quickCreate.Container.IsVisibleAsync())
            {
                return (await quickCreate.GetFormIdAsync(), quickCreate);
            }

            return (await this.RecordPage.GetFormIdAsync(), null);
        }

        private FieldContext ResolveFieldContext(string fieldName, string tab, Guid formId)
        {
            string fieldLogicalName = string.Empty;
            Guid fieldFormId = formId;
            var quickViewMatch = Regex.Match(fieldName, @"^(.*?)\s*<[^.]+\.(.+)>");
            var fieldMatch = Regex.Match(fieldName, "<([^>]+)>");

            if (quickViewMatch.Success)
            {
                this.formMetadataSvc.GetControlLogicalName(formId, quickViewMatch.Groups[1].Value, out fieldFormId, tab);
                fieldLogicalName = fieldMatch.Success
                    ? fieldMatch.Groups[1].Value
                    : throw new InvalidOperationException("Unable to resolve field logical name.");
            }
            else
            {
                fieldLogicalName = fieldMatch.Success
                    ? fieldMatch.Groups[1].Value
                    : this.formMetadataSvc.GetControlLogicalName(formId, fieldName, out fieldFormId, tab);
            }

            var sanitisedFieldName = fieldMatch.Success ? Regex.Replace(fieldName, @"\s*<[^>]*>", string.Empty).Trim() : fieldName;
            var columnName = this.formMetadataSvc.GetControlColumnName(fieldFormId, sanitisedFieldName, out _, fieldFormId == formId ? tab : null);
            var controlType = this.ResolveControlType(fieldFormId, columnName);

            var fieldLocation = this.ResolveFieldLocation(fieldLogicalName, fieldFormId, fieldName);

            return new FieldContext(fieldLocation, controlType, fieldFormId != formId, fieldLogicalName, sanitisedFieldName);
        }

        private Type ResolveControlType(Guid formId, string columnName)
        {
            return this.powerPlaywrightMetadataSvc.GetPowerPlaywrightControlInterface(formId, columnName);
        }

        private Type ResolveDataSetType(Guid formId, string dataSetName)
        {
            return this.powerPlaywrightMetadataSvc.GetPowerPlaywrightDataSetClass(formId, dataSetName);
        }

        private async Task CollapseHeaderAsync(FieldContext fieldContext)
        {
            if (fieldContext.Location == FieldLocation.Header)
            {
                await this.RecordPage.Form.CollapseHeaderAsync();
            }
        }

        private async Task ExecuteControlActionAsync<TControl>(string fieldName, Func<TControl, FieldContext, Task> action, string tab = null)
            where TControl : IPcfControl
        {
            var (formId, quickCreate) = await this.ResolveFormAsync();

            var fieldContext = this.ResolveFieldContext(fieldName, tab, formId);

            var field = await this.ResolveFieldAsync(fieldContext.LogicalName, fieldContext.Location, quickCreate);

            await action(field.GetControl<TControl>(), fieldContext);

            await this.CollapseHeaderAsync(fieldContext);
        }

        private async Task ExecuteGenericFieldActionAsync(string fieldName, Func<IField, FieldContext, Task> action, string tab = null)
        {
            var (formId, quickCreate) = await this.ResolveFormAsync();
            var fieldContext = this.ResolveFieldContext(fieldName, tab, formId);
            var field = await this.ResolveFieldAsync(fieldContext.LogicalName, fieldContext.Location, quickCreate);

            await action(field, fieldContext);

            await this.RecordPage.Page.WaitForAppIdleAsync();
            await this.CollapseHeaderAsync(fieldContext);
        }

        private async Task<T> ExecuteGenericFieldActionAsync<T>(string fieldName, Func<IField, FieldContext, Task<T>> action, string tab = null)
        {
            var (formId, quickCreate) = await this.ResolveFormAsync();
            var fieldContext = this.ResolveFieldContext(fieldName, tab, formId);
            var field = await this.ResolveFieldAsync(fieldContext.LogicalName, fieldContext.Location, quickCreate);

            var response = await action(field, fieldContext);

            await this.CollapseHeaderAsync(fieldContext);

            return response;
        }

        private async Task NavigateToContextRecordAsync(string contextKey, string entityLogicalName)
        {
            if (!this.ctx.TryGetValue<Guid>(contextKey, out var id))
            {
                throw new InvalidOperationException($"Unable to find {entityLogicalName} with context key {contextKey} in the scenario context.");
            }

            this.powerPlaywrightCtx.Validate();

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator
                .NavigateToRecordAsync(new EntityReference(entityLogicalName, id));
        }

        private async Task<string> ReplaceTemplateValuesAsync(string template)
        {
            var logicalNames = GetFieldsFromTemplate(template).ToArray();
            if (!logicalNames.Any())
            {
                return template;
            }

            var entityLogicalName = this.RecordPage.GetEntityLogicalName();
            var recordId = this.RecordPage.GetRecordId();

            var topLevelFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var lookupFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var relatedRequests = new List<(string lookupField, string relatedField, string originalPlaceholder)>();

            foreach (var name in logicalNames)
            {
                if (name.Contains("."))
                {
                    var parts = name.Split(new[] { '.' }, 2);
                    lookupFields.Add(parts[0]);
                    relatedRequests.Add((parts[0], parts[1], name));
                }
                else
                {
                    topLevelFields.Add(name);
                }
            }

            var primaryColumns = topLevelFields.Union(lookupFields).ToArray();

            var record = await this.serviceClient.RetrieveAsync(entityLogicalName, recordId.Value, new ColumnSet(primaryColumns));
            if (record == null)
            {
                return template;
            }

            foreach (var logicalName in topLevelFields)
            {
                var replacement = string.Empty;

                if (record.FormattedValues.Contains(logicalName))
                {
                    replacement = record.FormattedValues[logicalName];
                }
                else if (record.Attributes.Contains(logicalName) && record[logicalName] != null)
                {
                    replacement = record[logicalName].ToString();
                }

                template = ReplaceTemplatedValue(template, record, logicalName, logicalName);
            }

            var groupedByLookup = relatedRequests
                .GroupBy(r => r.lookupField, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => (x.relatedField, placeholder: x.originalPlaceholder)).ToList(),
                    StringComparer.OrdinalIgnoreCase);

            foreach (var kv in groupedByLookup)
            {
                var lookupField = kv.Key;
                var requestsForLookup = kv.Value;

                if (!(record.Attributes.Contains(lookupField) && record[lookupField] is EntityReference er) || er.Id == Guid.Empty)
                {
                    continue;
                }

                var relatedFields = requestsForLookup.Select(r => r.relatedField).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();

                var relatedRecord = await this.serviceClient.RetrieveAsync(er.LogicalName, er.Id, new ColumnSet(relatedFields));
                if (relatedRecord == null)
                {
                    continue;
                }

                foreach (var (relatedField, placeholder) in requestsForLookup)
                {
                    template = ReplaceTemplatedValue(template, relatedRecord, relatedField, placeholder);
                }
            }

            return template;
        }

        private async Task ExecuteGenericEditableGridActionAsync(string controlDisplayName, Func<IDataSet<IGridControl>, Task> action)
        {
            var logicalName = await this.GetGridLogicalNameAsync(controlDisplayName);
            var editableGrid = this.RecordPage.Form.GetDataSet<IGridControl>(logicalName);

            await action(editableGrid);
        }

        private Type GetNestedSubgridType()
        {
            return this.ctx.TryGetValue(ScenarioContextKeys.NestedSubgridType, out Type type)
                ? type
                : throw new InvalidOperationException("No nested subgrid type found in the scenario context.");
        }

        private T GetNestedSubgrid<T>()
        {
            return this.ctx.TryGetValue<T>(ScenarioContextKeys.NestedSubgrid, out var subgrid)
                ? subgrid
                : throw new InvalidOperationException("No nested subgrid found in the scenario context.");
        }

        private async Task<bool> IsDialogPresent()
        {
            return await this.RecordPage.ConfirmDialog.IsVisibleAsync()
                || await this.RecordPage.AlertDialog.IsVisibleAsync()
                || await this.RecordPage.ErrorDialog.IsVisibleAsync();
        }

        private class FieldContext
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FieldContext"/> class.
            /// </summary>
            /// <param name="location">The field location.</param>
            /// <param name="controlType">The control type.</param>
            /// <param name="isQuickView">Whether the field is on a quick view.</param>
            /// <param name="logicalName">The field logical name.</param>
            /// <param name="label">The field label.</param>
            public FieldContext(FieldLocation location, Type controlType, bool isQuickView, string logicalName, string label)
            {
                this.Location = location;
                this.ControlType = controlType;
                this.IsQuickView = isQuickView;
                this.LogicalName = logicalName;
                this.Label = label;
            }

            /// <summary>
            /// Gets the field location.
            /// </summary>
            public FieldLocation Location { get; }

            /// <summary>
            /// Gets the control type.
            /// </summary>
            public Type ControlType { get; }

            /// <summary>
            /// Gets a value indicating whether whether or not the field is on a quick view.
            /// </summary>
            public bool IsQuickView { get; }

            /// <summary>
            /// Gets the field logical name.
            /// </summary>
            public string LogicalName { get; }

            /// <summary>
            /// Gets the field label.
            /// </summary>
            public string Label { get; }
        }
    }
}
