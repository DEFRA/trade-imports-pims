namespace Defra.Imports.PageObjects.Strategies
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.PageObjects.Controls;
    using Microsoft.Playwright;
    using PowerPlaywright.Framework;
    using PowerPlaywright.Framework.Controls;
    using PowerPlaywright.Framework.Controls.Pcf.Attributes;
    using PowerPlaywright.Framework.Pages;

    /// <summary>
    /// A strategy for the <see cref="INotesTimelineControl"/>.
    /// </summary>
    [PcfControlStrategy(1, 26093, 1)]
    public class NotesTimelineControl : PcfControlBase, INotesTimelineControl
    {
        private const string DefaultNoteTitle = "Importer Notification note";
        private const int ComposerTimeoutMs = 15000;
        private const int SaveAttempts = 3;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesTimelineControl"/> class.
        /// </summary>
        /// <param name="name">The name given to the control.</param>
        /// <param name="appPage">The app page.</param>
        /// <param name="infoProvider">The info provider.</param>
        /// <param name="parent">The parent control.</param>
        public NotesTimelineControl(string name, IAppPage appPage, IEnvironmentInfoProvider infoProvider, IControl parent = null)
            : base(name, appPage, infoProvider, parent)
        {
        }

        /// <inheritdoc/>
        public async Task AddNoteAsync(string noteText)
        {
            var addButton = this.Container.Locator("[data-id='notescontrol-action_bar_add_command']");
            await addButton.WaitForAsync();
            await addButton.ClickAsync();

            // The menu renders in a callout outside the timeline.
            var noteMenuItem = this.Page.GetByRole(AriaRole.Menuitem, new PageGetByRoleOptions { Name = "Note", Exact = true });
            await noteMenuItem.WaitForAsync();
            await noteMenuItem.ClickAsync();

            var titleField = this.Container.Locator("[data-id*='create_note_medium_title' i]").First;
            await titleField.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = ComposerTimeoutMs });
            await titleField.FillAsync(DefaultNoteTitle);

            var bodyField = this.Container.Locator("[id^='create_note_notesText'] div.ck-editor__editable_inline").First;
            await bodyField.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = ComposerTimeoutMs });
            await bodyField.ClickAsync();
            await bodyField.PressSequentiallyAsync(noteText);

            // CKEditor only syncs its model, and enables Save, once the field loses focus.
            await bodyField.PressAsync("Tab");

            var saveButton = this.Container.Locator("#splitsave-button");
            await saveButton.WaitForAsync();

            for (var attempt = 1; attempt <= SaveAttempts; attempt++)
            {
                await saveButton.ClickAsync();

                try
                {
                    await titleField.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Hidden, Timeout = 10000 });
                    break;
                }
                catch (TimeoutException) when (attempt < SaveAttempts)
                {
                }
            }
        }

        /// <inheritdoc/>
        public async Task WaitForNoteAsync(string noteText)
        {
            var note = this.Container
                .Locator("[id^='timeline_field_notetext']")
                .Filter(new LocatorFilterOptions { HasText = noteText })
                .First;

            await note.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = ComposerTimeoutMs });
        }

        /// <inheritdoc/>
        protected override ILocator GetRoot(ILocator context)
        {
            // The dataset host container the framework passes in does not contain the timeline.
            return context.Page.Locator("div[data-lp-id^='MscrmControls.TimelineWallControl.TimelineWall|notescontrol|']").First;
        }
    }
}
