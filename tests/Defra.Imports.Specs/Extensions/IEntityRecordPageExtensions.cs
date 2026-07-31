namespace Defra.Imports.Specs.Extensions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using PowerPlaywright.Framework.Pages;

    /// <summary>
    /// Extensions for the <see cref="IEntityRecordPage"/>.
    /// </summary>
    public static class IEntityRecordPageExtensions
    {
        /// <summary>
        /// Gets the form ID for the entity record page.
        /// </summary>
        /// <param name="recordPage">The record page.</param>
        /// <returns>A <see cref="Task"/> representing the async operation.</returns>
        public static async Task<Guid> GetFormIdAsync(this IEntityRecordPage recordPage)
        {
            var route = await recordPage.Page.Locator("#navigationcontextprovider").GetAttributeAsync("route");

            return Guid.Parse(route.Split('/').Last());
        }

        /// <summary>
        /// Gets the logical name of the entity.
        /// </summary>
        /// <param name="recordPage">The record page.</param>
        /// <returns>The entity logical name.</returns>
        public static string GetEntityLogicalName(this IEntityRecordPage recordPage)
        {
            return HttpUtility.ParseQueryString(recordPage.Page.Url)["etn"];
        }

        /// <summary>
        /// Gets the ID of the record.
        /// </summary>
        /// <param name="recordPage">The record page.</param>
        /// <returns>The record ID.</returns>
        public static Guid? GetRecordId(this IEntityRecordPage recordPage)
        {
            var id = HttpUtility.ParseQueryString(recordPage.Page.Url)["id"];

            return string.IsNullOrEmpty(id) ? (Guid?)null : Guid.Parse(id);
        }
    }
}
