namespace Defra.Imports.PageObjects.Controls
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using PowerPlaywright.Framework.Controls.Pcf;
    using PowerPlaywright.Framework.Controls.Pcf.Attributes;

    /// <summary>
    /// The timeline wall control used to show and add notes on a record form.
    /// </summary>
    [PcfControl("MscrmControls.TimelineWallControl.TimelineWall")]
    public interface ITimelineWallControl : IPcfControl
    {
        /// <summary>
        /// Adds a note to the timeline.
        /// </summary>
        /// <param name="title">The note title.</param>
        /// <param name="noteText">The note text.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task AddNoteAsync(string title, string noteText);

        /// <summary>
        /// Gets the text of the notes shown in the timeline.
        /// </summary>
        /// <returns>The note texts.</returns>
        Task<IReadOnlyList<string>> GetNotesAsync();
    }
}
