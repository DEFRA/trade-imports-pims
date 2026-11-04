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
    /// A strategy for the <see cref="IFlipSwitchControl"/>.
    /// </summary>
    [PcfControlStrategy(1, 0, 10)]
    public class FlipSwitchControl : PcfControlBase, IFlipSwitchControl
    {
        private readonly ILocator flipSwitch;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlipSwitchControl"/> class.
        /// </summary>
        /// <param name="name">The name given to the control.</param>
        /// <param name="appPage">The app page.</param>
        /// <param name="infoProvider">The info provider.</param>
        /// <param name="parent">The parent control.</param>
        public FlipSwitchControl(string name, IAppPage appPage, IEnvironmentInfoProvider infoProvider, IControl parent = null)
            : base(name, appPage, infoProvider, parent)
        {
            this.flipSwitch = this.Container.GetByRole(AriaRole.Checkbox);
        }

        /// <inheritdoc/>
        public async Task<bool> GetValueAsync()
        {
            await this.Container.Page.WaitForAppIdleAsync();

            await this.CheckVisibility();

            return await this.flipSwitch.IsCheckedAsync();
        }

        /// <inheritdoc/>
        public async Task SetValueAsync(bool value)
        {
            await this.Container.Page.WaitForAppIdleAsync();

            await this.CheckVisibility();

            if (await this.flipSwitch.IsDisabledAsync())
            {
                throw new InvalidOperationException($"The flipswitch control for {this.GetControlId()} is disabled.");
            }

            if (await this.GetValueAsync() == value)
            {
                return;
            }

            await this.flipSwitch.ClickAsync();

            await this.Container.Page.WaitForAppIdleAsync();
        }

        private async Task CheckVisibility()
        {
            if (!await this.flipSwitch.IsVisibleAsync())
            {
                throw new InvalidOperationException($"The flipswitch control for {this.GetControlId()} is not visible.");
            }
        }
    }
}
