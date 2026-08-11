namespace Defra.Imports.Specs.Extensions
{
    using System;
    using System.Threading.Tasks;
    using PowerPlaywright.Framework.Controls.Platform;

    /// <summary>
    /// Extensions to the <see cref="IQuickCreateForm"/> interface.
    /// </summary>
    public static class IQuickCreateFormExtensions
    {
        /// <summary>
        /// Gets the form ID.
        /// </summary>
        /// <param name="quickCreate">The quick create.</param>
        /// <returns>The form ID.</returns>
        public static async Task<Guid> GetFormIdAsync(this IQuickCreateForm quickCreate)
        {
            return Guid.Parse(await quickCreate.Container.Locator("[data-id='quickCreateRoot'][data-preview-id]").GetAttributeAsync("data-preview-id"));
        }
    }
}
