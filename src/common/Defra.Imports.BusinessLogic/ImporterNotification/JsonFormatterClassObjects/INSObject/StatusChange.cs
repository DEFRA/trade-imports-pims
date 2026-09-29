namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Status change details for the notification.
    /// </summary>
    [DataContract]
    public class StatusChange
    {
        /// <summary>
        /// Gets status.
        /// </summary>
        [DataMember(Name = "status")]
        public string Status { get; internal set; }

        /// <summary>
        /// Gets date changed.
        /// </summary>
        [DataMember(Name = "dateChanged")]
        public string DateChanged { get; internal set; }

        /// <summary>
        /// Gets actor who performed the status change.
        /// </summary>
        [DataMember(Name = "actor")]
        public Actor Actor { get; internal set; }
    }
}