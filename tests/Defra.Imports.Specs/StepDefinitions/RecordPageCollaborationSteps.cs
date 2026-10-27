namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using FluentAssertions;
    using FluentAssertions.Execution;
    using Microsoft.Playwright;
    using Reqnroll;

    /// <summary>
    /// Record page steps for the share menu, notes timeline and document links, which Power Playwright doesn't support.
    /// </summary>
    [Binding]
    public class RecordPageCollaborationSteps
    {
        private const string DocumentPopupContextKey = "DocumentLinkPopup";

        private readonly PowerPlaywrightContext powerPlaywrightCtx;
        private readonly ScenarioContext ctx;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordPageCollaborationSteps"/> class.
        /// </summary>
        /// <param name="powerPlaywrightCtx">The Power Playwright context.</param>
        /// <param name="ctx">The scenario context.</param>
        public RecordPageCollaborationSteps(PowerPlaywrightContext powerPlaywrightCtx, ScenarioContext ctx)
        {
            this.powerPlaywrightCtx = powerPlaywrightCtx;
            this.ctx = ctx;
        }

        private IPage Page => this.powerPlaywrightCtx.ActivePage.Page;

        /// <summary>
        /// Opens the share menu on the record page.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I open the share menu")]
        public async Task WhenIOpenTheShareMenu()
        {
            // Share renders as its own button beside the command bar rather than as a command within it.
            var shareButton = this.Page.Locator("button[aria-label='Share']").First;

            await shareButton.WaitForAsync();
            await shareButton.ClickAsync();
        }

        /// <summary>
        /// Asserts the open share menu offers the given options.
        /// </summary>
        /// <param name="options">A table with header 'Option'.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the following share options")]
        public async Task ThenISeeTheFollowingShareOptions(DataTable options)
        {
            using (new AssertionScope())
            {
                foreach (var option in options.Rows.Select(r => r["Option"]))
                {
                    var menuItem = this.Page.GetByRole(AriaRole.Menuitem, new PageGetByRoleOptions { Name = option, Exact = true });

                    await menuItem.WaitForAsync();
                    (await menuItem.IsVisibleAsync()).Should().BeTrue($"the share menu should offer '{option}'");
                }
            }
        }

        /// <summary>
        /// Adds a note to the record using the notes timeline.
        /// </summary>
        /// <param name="noteText">The note text.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I add the note {string}")]
        public async Task WhenIAddTheNote(string noteText)
        {
            // "notescontrol" is the form's fixed control name for the notes timeline.
            var addTimelineRecordButton = this.Page.Locator("[data-id='notescontrol-action_bar_add_command']");
            await addTimelineRecordButton.WaitForAsync();
            await addTimelineRecordButton.ClickAsync();

            var noteMenuItem = this.Page.GetByRole(AriaRole.Menuitem, new PageGetByRoleOptions { Name = "Note", Exact = true });
            await noteMenuItem.WaitForAsync();
            await noteMenuItem.ClickAsync();

            // The composer renders inline on the timeline: a title input, a CKEditor body and a split save button.
            var titleField = this.Page.Locator("[data-id*='create_note_medium_title' i]").First;
            await titleField.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 15000 });
            await titleField.FillAsync("Importer Notification note");

            var bodyField = this.Page.Locator("[id^='create_note_notesText'] div.ck-editor__editable_inline").First;
            await bodyField.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 15000 });
            await bodyField.ClickAsync();
            await bodyField.PressSequentiallyAsync(noteText);

            // CKEditor only syncs its model, and enables Save, once the field loses focus.
            await bodyField.PressAsync("Tab");

            var saveButton = this.Page.Locator("#splitsave-button");
            await saveButton.WaitForAsync();

            const int maxAttempts = 3;
            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                await saveButton.ClickAsync();

                try
                {
                    await titleField.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Hidden, Timeout = 10000 });
                    break;
                }
                catch (TimeoutException) when (attempt < maxAttempts)
                {
                }
            }
        }

        /// <summary>
        /// Asserts the notes timeline shows a note containing the given text.
        /// </summary>
        /// <param name="noteText">The note text.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see a note containing {string}")]
        public async Task ThenISeeANoteContaining(string noteText)
        {
            var note = this.Page.GetByText(noteText, new PageGetByTextOptions { Exact = false }).First;

            await note.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible, Timeout = 15000 });
        }

        /// <summary>
        /// Asserts the first document in the documents subgrid has a visible link.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see a document URL link in the documents subgrid")]
        public async Task ThenISeeADocumentUrlLinkInTheDocumentsSubgrid()
        {
            var link = this.GetFirstDocumentLink();

            await link.WaitForAsync(new LocatorWaitForOptions { State = WaitForSelectorState.Visible });
        }

        /// <summary>
        /// Clicks the first document URL link in the documents subgrid.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I click the first document URL in the documents subgrid")]
        public async Task WhenIClickTheFirstDocumentUrlInTheDocumentsSubgrid()
        {
            var popup = await this.Page.Context.RunAndWaitForPageAsync(async () => await this.GetFirstDocumentLink().ClickAsync());

            this.ctx.Set(popup, DocumentPopupContextKey);
        }

        /// <summary>
        /// Asserts the clicked document link opened a new tab.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("the document opens in a new tab")]
        public async Task ThenTheDocumentOpensInANewTab()
        {
            var popup = this.ctx.Get<IPage>(DocumentPopupContextKey);

            await popup.WaitForURLAsync(new Regex("^https?://"), new PageWaitForURLOptions { WaitUntil = WaitUntilState.Commit, Timeout = 15000 });

            popup.Url.Should().StartWith("http", "the document link should open the document URL in a new tab");
        }

        private ILocator GetFirstDocumentLink()
        {
            return this.Page.Locator("[data-lp-id*='documents_Subgrid'] a[href^='http']").First;
        }
    }
}
