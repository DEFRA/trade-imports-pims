namespace Defra.Imports.PageObjects.Strategies
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Playwright;
    using PowerPlaywright.Framework;
    using PowerPlaywright.Framework.Controls;
    using PowerPlaywright.Framework.Controls.Pcf;
    using PowerPlaywright.Framework.Controls.Pcf.Attributes;
    using PowerPlaywright.Framework.Pages;

    /// <summary>
    /// Base class for PCF controls.
    /// </summary>
    public abstract class PcfControlBase : PcfControl
    {
        private readonly IEnvironmentInfoProvider infoProvider;

        private PcfControlAttribute pcfControlAttribute;

        /// <summary>
        /// Initializes a new instance of the <see cref="PcfControlBase"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="appPage">The app page.</param>
        /// <param name="infoProvider">The environment info provider.</param>
        /// <param name="parent">The parent control.</param>
        protected PcfControlBase(string name, IAppPage appPage, IEnvironmentInfoProvider infoProvider, IControl parent = null)
            : base(name, appPage, parent)
        {
            this.infoProvider = infoProvider;
        }

        /// <summary>
        /// Gets the <see cref="PcfControlAttribute"/> applicable to this control.
        /// </summary>
        public PcfControlAttribute PcfControlAttribute
        {
            get
            {
                if (pcfControlAttribute == null)
                {
                    pcfControlAttribute = GetType().GetInterfaces()
                        .FirstOrDefault(i => i.GetInterfaces().Contains(typeof(IPcfControl)) && i.GetCustomAttribute<PcfControlAttribute>() != null)
                        ?.GetCustomAttribute<PcfControlAttribute>();

                    if (pcfControlAttribute == null)
                    {
                        throw new InvalidOperationException($"PcfControlAttribute not found on {GetType().Name}.");
                    }
                }

                return pcfControlAttribute;
            }
        }

        /// <summary>
        /// Returns the control ID for the target environment.
        /// </summary>
        /// <returns>The control ID.</returns>
        protected Guid GetControlId()
        {
            return infoProvider.ControlIds.TryGetValue(PcfControlAttribute.Name, out var controlId)
                ? controlId
                : throw new InvalidOperationException($"Control ID for {PcfControlAttribute.Name} not found in environment info.");
        }

        /// <inheritdoc/>
        protected override ILocator GetRoot(ILocator context)
        {
            return context.Locator($"//div[starts-with(@data-lp-id, '{PcfControlAttribute.Name}|') or starts-with(@data-lp-id, '{GetControlId()}|')]");
        }
    }
}