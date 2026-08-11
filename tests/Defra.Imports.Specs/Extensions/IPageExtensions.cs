namespace Defra.Imports.Specs.Extensions
{
    using System.Threading.Tasks;
    using Microsoft.Playwright;
    using PowerPlaywright.Framework.Extensions;

    /// <summary>
    /// Extensions to the <see cref="IPage"/> interface.
    /// </summary>
    internal static class IPageExtensions
    {
        /// <summary>
        /// Reloads the page and waits for the load state and app idle.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        internal static async Task ReloadAndWaitForAppIdleAsync(this IPage page)
        {
            await page.ReloadAsync();
            await page.WaitForLoadStateAsync();
            await page.WaitForAppIdleAsync();
        }
    }
}
