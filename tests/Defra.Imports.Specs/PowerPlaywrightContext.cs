namespace Defra.Imports.Specs
{
    using System;
    using PowerPlaywright.Framework.Pages;

    /// <summary>
    /// Tracks Power Playwright context.
    /// </summary>
    public class PowerPlaywrightContext
    {
        /// <summary>
        /// Gets or sets the active page.
        /// </summary>
        public IModelDrivenAppPage ActivePage { get; set; }

        /// <summary>
        /// Gets or sets the active logged in user.
        /// </summary>
        public Guid ActiveUserId { get; set; }

        /// <summary>
        /// Validates that there is an active session.
        /// </summary>
        /// <exception cref="Exception">Thrown if there is no active session.</exception>
        public void Validate()
        {
            if (this.ActivePage is null)
            {
                throw new Exception("There is no active Power Playwright session.");
            }
        }

        /// <summary>
        /// Validates that the active page is of a given type.
        /// </summary>
        /// <typeparam name="TPageType">The type of page expected.</typeparam>
        /// <exception cref="Exception">Thrown if the page is not of the expected type.</exception>
        public void ValidatePage<TPageType>()
            where TPageType : IModelDrivenAppPage
        {
            this.Validate();

            if (this.ActivePage is TPageType)
            {
                return;
            }

            throw new Exception($"Expected the active page to be of type {typeof(TPageType).Name} but found {this.ActivePage.GetType().Name}.");
        }
    }
}
