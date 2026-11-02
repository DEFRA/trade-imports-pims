namespace Defra.Imports.Specs.Controls
{
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.Playwright;

    /// <summary>
    /// Encapsulates interactions with the timeline control on an entity record page.
    /// </summary>
    public class TimelineControl
    {
        private readonly IPage page;

        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineControl"/> class.
        /// </summary>
        /// <param name="page">The underlying Playwright page.</param>
        public TimelineControl(IPage page)
        {
            this.page = page;
        }

        private ILocator Timeline => this.page.Locator("[aria-label='Timeline'], [data-id='notescontrol'], [id*='Timeline']").First;

        /// <summary>
        /// Waits for the timeline to be visible.
        /// </summary>
        /// <returns>A task.</returns>
        public async Task WaitForVisibleAsync()
        {
            await this.Timeline.WaitForAsync();
        }

        /// <summary>
        /// Adds a note to the timeline.
        /// </summary>
        /// <param name="note">The note text.</param>
        /// <returns>A task.</returns>
        public async Task AddNoteAsync(string note)
        {
            await this.Timeline.WaitForAsync(new LocatorWaitForOptions { Timeout = 60000 });

            var addNoteButton = this.page.GetByRole(AriaRole.Button, new PageGetByRoleOptions { Name = "Add note" }).First;
            if (await addNoteButton.IsVisibleAsync())
            {
                await addNoteButton.ClickAsync();
            }

            var textbox = this.page
                .GetByPlaceholder("Type a note...")
                .Or(this.page.GetByLabel("Type a note..."))
                .Or(this.page.Locator("div[contenteditable='true'][role='textbox'][aria-label*='note' i]"))
                .First;

            await textbox.WaitForAsync(new LocatorWaitForOptions { Timeout = 60000 });
            await textbox.FillAsync(note);

            var postButton = this.page.GetByRole(
                AriaRole.Button,
                new PageGetByRoleOptions
                {
                    NameRegex = new Regex("^(Post|Save)$", RegexOptions.IgnoreCase),
                }).First;

            await postButton.WaitForAsync(new LocatorWaitForOptions { Timeout = 30000 });
            await postButton.ClickAsync();
        }

        /// <summary>
        /// Waits for the specified note text to be visible in a persisted timeline entry.
        /// </summary>
        /// <param name="note">The expected note text.</param>
        /// <returns>A task.</returns>
        public async Task WaitForNoteAsync(string note)
        {
            var savedTimelineEntry = this.Timeline
                .Locator(":not(textarea):not([contenteditable='true'])")
                .GetByText(note, new LocatorGetByTextOptions { Exact = false })
                .First;

            await savedTimelineEntry.WaitForAsync(new LocatorWaitForOptions { Timeout = 60000 });
        }
    }
}