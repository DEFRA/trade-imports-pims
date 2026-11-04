namespace Defra.Imports.PageObjects.Strategies
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.PageObjects.Controls;
    using Microsoft.Playwright;
    using PowerPlaywright.Framework;
    using PowerPlaywright.Framework.Controls;
    using PowerPlaywright.Framework.Controls.Pcf.Attributes;
    using PowerPlaywright.Framework.Extensions;
    using PowerPlaywright.Framework.Pages;

    /// <summary>
    /// A strategy for the <see cref="ICoreCheckboxControl"/>.
    /// </summary>
    [PcfControlStrategy(1, 0, 16)]
    public class CoreCheckboxControl : PcfControlBase, ICoreCheckboxControl
    {
        private readonly ILocator checkbox;

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreCheckboxControl"/> class.
        /// </summary>
        /// <param name="name">The name given to the control.</param>
        /// <param name="appPage">The app page.</param>
        /// <param name="infoProvider">The info provider.</param>
        /// <param name="parent">The parent control.</param>
        public CoreCheckboxControl(string name, IAppPage appPage, IEnvironmentInfoProvider infoProvider, IControl parent = null)
            : base(name, appPage, infoProvider, parent)
        {
            this.checkbox = this.Container.GetByRole(AriaRole.Checkbox);
        }

        /// <inheritdoc/>
        public async Task<bool> GetValueAsync()
        {
            await this.Container.Page.WaitForAppIdleAsync();

            return await this.checkbox.IsCheckedAsync();
        }

        /// <inheritdoc/>
        public async Task SetValueAsync(bool value)
        {
            await this.Container.Page.WaitForAppIdleAsync();

            if (await this.checkbox.IsDisabledAsync())
            {
                throw new InvalidOperationException($"The checkbox control for {this.Name} is disabled.");
            }

            if (await this.checkbox.IsCheckedAsync() == value)
            {
                return;
            }

            await this.checkbox.SetCheckedAsync(value);

            await this.Container.Page.WaitForAppIdleAsync();
        }
    }
}
