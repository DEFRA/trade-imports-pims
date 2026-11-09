namespace Defra.Imports.PageObjects.Strategies
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Defra.Imports.PageObjects.Controls;
    using Microsoft.Playwright;
    using PowerPlaywright.Framework;
    using PowerPlaywright.Framework.Controls;
    using PowerPlaywright.Framework.Controls.Pcf.Attributes;
    using PowerPlaywright.Framework.Extensions;
    using PowerPlaywright.Framework.Pages;

    /// <summary>
    /// A strategy for the <see cref="ITimelineWallControl"/>.
    /// </summary>
    [PcfControlStrategy(1, 26093, 1)]
    public class TimelineWallControl : PcfControlBase, ITimelineWallControl
    {
        private const int NoteTimeoutMs = 15000;

        private readonly ILocator addButton;
        private readonly ILocator createNoteTitle;
        private readonly ILocator createNoteText;
        private readonly ILocator saveButton;
        private readonly ILocator notes;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineWallControl"/> class.
        /// </summary>
        /// <param name="name">The name given to the control.</param>
        /// <param name="appPage">The app page.</param>
        /// <param name="infoProvider">The info provider.</param>
        /// <param name="parent">The parent control.</param>
        public TimelineWallControl(string name, IAppPage appPage, IEnvironmentInfoProvider infoProvider, IControl parent = null)
            : base(name, appPage, infoProvider, parent)
        {
            this.addButton = this.Container.Locator($"[data-id='{name}-action_bar_add_command']");
            this.createNoteTitle = this.Container.Locator("[data-id*='create_note_medium_title' i]").First;
            this.createNoteText = this.Container.Locator("[id^='create_note_notesText'] div.ck-editor__editable_inline").First;
            this.saveButton = this.Container.Locator("#splitsave-button");
            this.notes = this.Container.Locator("[id^='timeline_field_notetext']");
        }

        /// <inheritdoc/>
        public async Task AddNoteAsync(string title, string noteText)
        {
            await this.ClickAndWaitForAppIdleAsync(this.addButton);

            // The menu renders in a callout outside the timeline.
            await this.ClickAndWaitForAppIdleAsync(this.Page.GetByRole(AriaRole.Menuitem, new PageGetByRoleOptions { Name = "Note", Exact = true }));

            await this.createNoteTitle.FillAsync(title);

            await this.createNoteText.ClickAsync();
            await this.createNoteText.PressSequentiallyAsync(noteText);

            // CKEditor only syncs its model, and enables Save, once the field loses focus.
            await this.createNoteText.PressAsync("Tab");

            await this.ClickAndWaitForAppIdleAsync(this.saveButton);

            await this.WaitForNoteAsync(noteText);
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyList<string>> GetNotesAsync()
        {
            return await this.notes.AllInnerTextsAsync();
        }

        /// <inheritdoc/>
        protected override ILocator GetRoot(ILocator context)
        {
            // Remove this override when Power Playwright can get generic controls from a form; the host container passed in does not contain the timeline.
            return context.Page.Locator($"div[data-lp-id^='MscrmControls.TimelineWallControl.TimelineWall|{this.Name}|']").First;
        }

        // PowerPlaywright's own ClickAndWaitForAppIdleAsync is not public in 0.41.7.
        private async Task ClickAndWaitForAppIdleAsync(ILocator locator)
        {
            await locator.ClickAsync();
            await this.Page.WaitForAppIdleAsync();
        }

        private async Task WaitForNoteAsync(string noteText)
        {
            await this.notes
                .Filter(new LocatorFilterOptions { HasText = noteText })
                .First
                .WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = NoteTimeoutMs });
        }
    }
}
