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