namespace Defra.Imports.Specs.Services
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Extensions;
    using PowerPlaywright.Framework.Controls.Platform;

    /// <summary>
    /// A service for translating display names to logical names.
    /// </summary>
    public class DisplayNameTranslationService
    {
        private readonly FormMetadataService formMetadataSvc;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisplayNameTranslationService"/> class.
        /// </summary>
        /// <param name="formMetadataSvc">The form metadata service.</param>
        public DisplayNameTranslationService(FormMetadataService formMetadataSvc)
        {
            this.formMetadataSvc = formMetadataSvc;
        }

        /// <summary>
        /// Translate a field display name to a logical name.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        /// <param name="displayName">The display name.</param>
        /// <returns>The logical name.</returns>
        public async Task<string> TranslateFieldDisplayNameAsync(IMainForm mainForm, string displayName)
        {
            if (!TryGetExplicitLogicalName(displayName, out string logicalName))
            {
                logicalName = this.formMetadataSvc.GetControlLogicalName(
                    await mainForm.GetFormIdAsync(),
                    displayName,
                    out _,
                    tab: await mainForm.GetActiveTabAsync());
            }

            return logicalName;
        }

        /// <summary>
        /// Translate a dataset display name to a logical name.
        /// </summary>
        /// <param name="mainForm">The main form.</param>
        /// <param name="displayName">The display name.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<string> TranslateDataSetDisplayNameAsync(IMainForm mainForm, string displayName)
        {
            var logicalName = await this.TranslateFieldDisplayNameAsync(mainForm, displayName);

            return logicalName.Split('.').Last();
        }

        private static bool TryGetExplicitLogicalName(string displayName, out string logicalName)
        {
            var match = Regex.Match(displayName, "<([^>]+)>");

            logicalName = match.Groups[1].Value;

            return match.Success;
        }
    }
}
