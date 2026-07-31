namespace Defra.Imports.Specs.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Playwright;
    using PowerPlaywright.Framework.Controls.Pcf;
    using PowerPlaywright.Framework.Extensions;

    /// <summary>
    /// Extensions to the <see cref="IGridControl"/>.
    /// </summary>
    public static class IGridControlExtensions
    {
        /// <summary>
        /// Gets the seleced row indexes.
        /// </summary>
        /// <param name="grid">Grid control instance.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<List<int>> GetSelectedRowsAsync(this IGridControl grid)
        {
            var rows = grid.Parent.Container.GetByRole(AriaRole.Row)
                .Filter(new LocatorFilterOptions
                {
                    Has = grid.Parent.Container.Page.GetByRole(
                        AriaRole.Checkbox,
                        new PageGetByRoleOptions { Checked = true }),
                    HasNot = grid.Parent.Container.Page.Locator("[aria-label='Header']"),
                });

            var selectedRowCount = await rows.CountAsync();
            var selectedRows = new List<int>(selectedRowCount);

            for (int i = 0; i < selectedRowCount; i++)
            {
                var row = rows.Nth(i);

                var rowIndex = await row.GetAttributeAsync("aria-rowindex");
                selectedRows.Add(int.Parse(rowIndex) - 2);
            }

            return selectedRows;
        }

        /// <summary>
        /// Saves modified rows.
        /// </summary>
        /// <param name="grid">Grid control instance.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task SaveChangesAsync(this IGridControl grid)
        {
            var button = grid.Parent.Container.Locator("button[title='Save']");

            await button.ClickAsync();
        }

        /// <summary>
        /// Gets an alert message.
        /// </summary>
        /// <param name="grid">Grid control instance.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<string> GetAlertMessageAsync(this IGridControl grid)
        {
            var alert = grid.Parent.Container.Locator("div[role='alert']:not([aria-live='assertive'])");

            return await alert.InnerTextAsync();
        }

        /// <summary>
        /// Sets the values of cells in a specified row, handling both text inputs and dropdowns.
        /// </summary>
        /// <param name="grid">The grid control instance.</param>
        /// <param name="rowIndex">The index of the row to update.</param>
        /// <param name="values">A dictionary of column names and their corresponding values.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task UpdateRowAsyncV2(this IGridControl grid, int rowIndex, IDictionary<string, string> values)
        {
            var row = GetVisibleRows(grid).Nth(rowIndex);
            var columnsToProcess = values.Keys.ToList();

            while (columnsToProcess.Count != 0)
            {
                var visibleColumns = (await GetVisibleHeaders(grid).AllInnerTextsAsync()).ToList();
                var visibleColumnsToProcess = visibleColumns.Where(columnsToProcess.Contains);

                foreach (var column in visibleColumnsToProcess)
                {
                    var cell = row.GetByRole(AriaRole.Gridcell)
                        .Filter(new LocatorFilterOptions { HasNot = grid.Parent.Container.Page.GetByRole(AriaRole.Checkbox) })
                        .Nth(visibleColumns.IndexOf(column));

                    await cell.ClickAsync();
                    await grid.Parent.Container.Page.WaitForAppIdleAsync();

                    var listBox = grid.Parent.Container.Page.Locator("div[role='listbox']");
                    var isListBoxVisible = await IsListBoxVisibleAsync(cell, listBox);

                    if (isListBoxVisible)
                    {
                        var option = listBox.GetByRole(AriaRole.Option, new LocatorGetByRoleOptions { Name = values[column], Exact = true });

                        if (await option.IsVisibleAsync())
                        {
                            await option.ClickAsync();
                            await grid.Parent.Container.Page.WaitForAppIdleAsync();
                        }
                    }
                    else
                    {
                        var input = cell.GetByRole(AriaRole.Textbox).Or(cell.GetByRole(AriaRole.Combobox));
                        await input.FillAsync(values[column]);
                    }

                    await grid.Parent.Container.Page.Keyboard.PressAsync("Tab");
                    await grid.Parent.Container.Page.WaitForAppIdleAsync();

                    columnsToProcess.Remove(column);
                }

                if (columnsToProcess.Count != 0)
                {
                    var visibleHeaders = GetVisibleHeaders(grid);
                    await ScrollHorizontalAsync(grid, (await visibleHeaders.Last.BoundingBoxAsync()).X / 2);
                }
            }
        }

        private static async Task ScrollHorizontalAsync(this IGridControl grid, float offset)
        {
            await grid.Parent.Container.HoverAsync();
            await grid.Parent.Container.Page.Mouse.WheelAsync(offset, 0);
            await grid.Parent.Container.Page.WaitForAppIdleAsync();
        }

        private static ILocator GetVisibleRows(IGridControl grid)
        {
            return grid.Parent.Container.GetByRole(AriaRole.Row)
                .Filter(new LocatorFilterOptions
                {
                    HasNot = grid.Container.Page.Locator("[aria-label='Header']"),
                    Has = grid.Container.Page.GetByRole(AriaRole.Gridcell),
                });
        }

        private static ILocator GetVisibleHeaders(IGridControl grid)
        {
            return grid.Parent.Container.Locator("[role='columnheader']:not([aria-colindex='1'])")
                .Filter(new LocatorFilterOptions
                {
                    HasNot = grid.Parent.Container.Page.GetByRole(AriaRole.Img, new PageGetByRoleOptions { Name = "Navigate", Exact = true }),
                });
        }

        /// <summary>
        /// Ensures the listbox is visible, toggling dropdown if necessary.
        /// </summary>
        /// <param name="cell">The grid cell locator.</param>
        /// <param name="listBox">The listbox locator.</param>
        /// <returns>True if listbox is visible, false otherwise.</returns>
        private static async Task<bool> IsListBoxVisibleAsync(ILocator cell, ILocator listBox)
        {
            if (await listBox.IsVisibleAsync())
            {
                return true;
            }

            var btn = cell.Locator("button[aria-label='Toggle Dropdown']");
            if (await btn.IsVisibleAsync())
            {
                await btn.ClickAsync();
                return await listBox.IsVisibleAsync();
            }

            return false;
        }
    }
}