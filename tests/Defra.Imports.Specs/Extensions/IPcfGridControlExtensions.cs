namespace Defra.Imports.Specs.Extensions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using PowerPlaywright.Framework.Controls.Pcf;
    using PowerPlaywright.Framework.Extensions;

    /// <summary>
    /// Extensions to the <see cref="IPcfGridControl"/> interface.
    /// </summary>
    public static class IPcfGridControlExtensions
    {
        /// <summary>
        /// Open related record from the grid.
        /// </summary>
        /// <param name="grid">The grid control.</param>
        /// <param name="rowIndex">The index of the row.</param>
        /// <param name="columnName">The name of the column.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task NavigateRelatedRecord(this IPcfGridControl grid, int rowIndex, string columnName)
        {
            var columns = await grid.GetColumnNamesAsync();
            var columnIndex = columns
                    .ToList()
                    .IndexOf(columnName);

            var row = grid.Container.Locator($"div[role='row'][row-index='{rowIndex}']");
            var cell = row.Locator($"div[role='gridcell']:not([col-id='__row_status'])").Nth(columnIndex);
            var btnLink = cell.Locator($"button[role='link']");

            if (!await btnLink.IsVisibleAsync())
            {
                throw new InvalidOperationException($"Hyperlink button control could not be found within under the column '{columnName}' within row index '{rowIndex}'");
            }

            await btnLink.ClickAsync();
            await grid.Container.Page.WaitForAppIdleAsync();
        }
    }
}