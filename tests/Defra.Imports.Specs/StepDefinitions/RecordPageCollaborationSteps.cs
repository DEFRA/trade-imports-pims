namespace Defra.Imports.Specs.StepDefinitions
{
    using System.Threading.Tasks;
    using Defra.Imports.PageObjects.Controls;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;

    /// <summary>
    /// Record page steps for notes timeline interactions.
    /// </summary>
    [Binding]
    public class RecordPageCollaborationSteps
    {
        private const string NotesControlName = "notescontrol";

        private readonly PowerPlaywrightContext powerPlaywrightCtx;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordPageCollaborationSteps"/> class.
        /// </summary>
        /// <param name="powerPlaywrightCtx">The Power Playwright context.</param>
        public RecordPageCollaborationSteps(PowerPlaywrightContext powerPlaywrightCtx)
        {
            this.powerPlaywrightCtx = powerPlaywrightCtx;
        }

        private INotesTimelineControl Notes => ((IEntityRecordPage)this.powerPlaywrightCtx.ActivePage)
            .Form.GetDataSet<INotesTimelineControl>(NotesControlName).Control;

        /// <summary>
        /// Adds a note to the record using the notes timeline.
        /// </summary>
        /// <param name="noteText">The note text.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I add the note {string}")]
        public async Task WhenIAddTheNote(string noteText)
        {
            await this.Notes.AddNoteAsync(noteText);
        }

        /// <summary>
        /// Asserts the notes timeline shows a note containing the given text.
        /// </summary>
        /// <param name="noteText">The note text.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see a note containing {string}")]
        public async Task ThenISeeANoteContaining(string noteText)
        {
            await this.Notes.WaitForNoteAsync(noteText);
        }
    }
}
