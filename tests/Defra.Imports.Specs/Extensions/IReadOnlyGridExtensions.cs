namespace Defra.Imports.Specs.Extensions
{
    using System;
    using System.Threading.Tasks;
    using PowerPlaywright.Framework.Controls.Pcf.Classes;

    /// <summary>
    /// Extensions to the <see cref="IReadOnlyGrid"/> interface.
    /// </summary>
    public static class IReadOnlyGridExtensions
    {
        /// <summary>
        /// Gets the row ID.
        /// </summary>
        /// <param name="grid">The grid.</param>
        /// <param name="index">The zero-based row index.</param>
        /// <returns>The row ID.</returns>
        public static async Task<Guid> GetRowIdAsync(this IReadOnlyGrid grid, int index)
        {
            var row = grid.Container.Locator($"div[role='row'][row-index='{index}']");
            var id = await row.GetAttributeAsync("row-id");

            return Guid.Parse(id);
        }
    }
}
