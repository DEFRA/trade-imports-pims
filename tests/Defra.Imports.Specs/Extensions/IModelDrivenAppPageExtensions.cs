namespace Defra.Imports.Specs.Extensions
{
    using System;
    using System.Threading.Tasks;
    using PowerPlaywright.Framework.Pages;

    /// <summary>
    /// Extensions for the <see cref="IModelDrivenAppPage"/> interface.
    /// </summary>
    public static class IModelDrivenAppPageExtensions
    {
        /// <summary>
        /// Gets the active user ID.
        /// </summary>
        /// <param name="appPage">The app page.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static Task<Guid> GetActiveUserIdAsync(this IModelDrivenAppPage appPage)
        {
            return appPage.Page.EvaluateAsync<Guid>("Xrm.Utility.getGlobalContext().userSettings.userId");
        }
    }
}
