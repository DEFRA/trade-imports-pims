namespace Defra.Imports.PageObjects.Controls
{
    using System.Threading.Tasks;
    using PowerPlaywright.Framework.Controls.Pcf;
    using PowerPlaywright.Framework.Controls.Pcf.Attributes;

    /// <summary>
    /// The timeline wall control used to show and add notes on a record form.
    /// </summary>
    [PcfControl("MscrmControls.TimelineWallControl.TimelineWall")]
    public interface INotesTimelineControl : IPcfControl
    {
        /// <summary>
        /// Adds a note to the timeline.
        /// </summary>
        /// <param name="noteText">The note text.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddNoteAsync(string noteText);

        /// <summary>
        /// Waits for a note containing the given text to be shown in the timeline.
        /// </summary>
        /// <param name="noteText">The note text.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task WaitForNoteAsync(string noteText);
    }
}
