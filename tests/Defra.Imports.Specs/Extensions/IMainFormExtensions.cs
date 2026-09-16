namespace Defra.Imports.Specs.Extensions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Playwright;
    using PowerPlaywright.Framework.Controls.Pcf.Classes;
    using PowerPlaywright.Framework.Controls.Platform;
    using PowerPlaywright.Framework.Extensions;

    /// <summary>
    /// Extensions to <see cref="IMainForm"/>.
    /// </summary>
    public static class IMainFormExtensions
    {
        /// <summary>
        /// Gets the form ID for the entity record page.
        /// </summary>
        /// <param name="mainForm">The record page.</param>
        /// <returns>A <see cref="Task"/> representing the async operation.</returns>
        public static async Task<Guid> GetFormIdAsync(this IMainForm mainForm)
        {
            var route = await mainForm.Container.Page.Locator("#navigationcontextprovider").GetAttributeAsync("route");

            return Guid.Parse(route.Split('/').Last());
        }

        /// <summary>
        /// Opens a related tab on the form.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        /// <param name="relatedTabName">The related tab name.</param>
        /// <returns>A <see cref="Task"/> representing an asynchronous operation.</returns>
        public static async Task OpenRelatedTabAsync(this IMainForm mainForm, string relatedTabName)
        {
            // TODO: Move to Power Playwright library.
            await mainForm.Container.Page.WaitForAppIdleAsync();

            await mainForm.Container.GetByRole(AriaRole.Tab, new LocatorGetByRoleOptions { Name = "Related" }).ClickAsync();
            await mainForm.Container.Page.WaitForAppIdleAsync();
            await mainForm.Container.Page
                .GetByRole(AriaRole.Menu)
                .GetByRole(AriaRole.Menuitem)
                .Filter(new LocatorFilterOptions
                {
                    Has = mainForm.Container.Page.GetByText(relatedTabName, new PageGetByTextOptions { Exact = true }),
                }).ClickAsync();

            await mainForm.Container.Page.WaitForAppIdleAsync();
        }

        /// <summary>
        /// Opens a tab on the form, matching the tab name exactly.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        /// <param name="tabName">The tab name.</param>
        /// <returns>A <see cref="Task"/> representing an asynchronous operation.</returns>
        public static async Task OpenTabByExactNameAsync(this IMainForm mainForm, string tabName)
        {
            await mainForm.Container.Page.WaitForAppIdleAsync();

            var tab = mainForm.Container
                .GetByRole(AriaRole.Tab, new LocatorGetByRoleOptions { Name = tabName, Exact = true })
                .First;

            // The form renders its tab list progressively, so poll rather than checking once.
            // A tab that is not on the tab strip may still be reachable via the overflow ("...") menu.
            const int MaxAttempts = 15;

            for (var attempt = 0; attempt < MaxAttempts; attempt++)
            {
                if (await tab.IsVisibleAsync())
                {
                    await tab.ClickAsync();
                    await mainForm.Container.Page.WaitForAppIdleAsync();

                    return;
                }

                var overflow = await GetTabOverflowButtonAsync(mainForm);

                if (overflow != null)
                {
                    await overflow.ClickAsync();
                    await mainForm.Container.Page.WaitForAppIdleAsync();

                    var flyoutItem = mainForm.Container.Page
                        .GetByRole(AriaRole.Menuitem, new PageGetByRoleOptions { Name = tabName })
                        .First;

                    if (await flyoutItem.IsVisibleAsync())
                    {
                        await flyoutItem.ClickAsync();
                        await mainForm.Container.Page.WaitForAppIdleAsync();

                        return;
                    }

                    // Close the flyout again so the next attempt starts from a clean state.
                    await mainForm.Container.Page.Keyboard.PressAsync("Escape");
                }

                await mainForm.Container.Page.WaitForTimeoutAsync(1000);
            }

            var available = await mainForm.Container.GetByRole(AriaRole.Tab).AllTextContentsAsync();

            throw new InvalidOperationException(
                $"Unable to find a tab named '{tabName}' on the form. Tabs actually rendered: [{string.Join(" | ", available)}].");
        }

        /// <summary>
        /// Gets the tab list overflow ("...") button, or null when every tab fits on the tab strip.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        /// <returns>A <see cref="Task"/> representing an asynchronous operation.</returns>
        private static async Task<ILocator> GetTabOverflowButtonAsync(IMainForm mainForm)
        {
            var candidates = new[]
            {
                "[data-id='tablist-overflowButton']",
                "[data-id='moreTabsButton']",
                "[id*='tablist-overflowButton']",
                "button[aria-label='More tabs']",
                "[role='tab'][aria-haspopup='true']",
                "[role='tablist'] [aria-label*='More']",
                "[role='tablist'] button[title*='More']",
            };

            foreach (var candidate in candidates)
            {
                var locator = mainForm.Container.Locator(candidate).First;

                if (await locator.IsVisibleAsync())
                {
                    return locator;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a related grid on the form.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<IDataSet<IReadOnlyGrid>> GetRelatedDataSetAsync(this IMainForm mainForm)
        {
            IDataSet<IReadOnlyGrid> dataSet = null;
            bool dataSetVisible = false;
            for (int i = 1; i < 10; i++)
            {
                dataSet = mainForm.GetDataSet<IReadOnlyGrid>(i.ToString());
                dataSetVisible = await dataSet.IsVisibleAsync();

                if (dataSetVisible)
                {
                    break;
                }
            }

            if (!dataSetVisible)
            {
                throw new Exception("No visible related data set found on the form.");
            }

            return dataSet;
        }
    }
}