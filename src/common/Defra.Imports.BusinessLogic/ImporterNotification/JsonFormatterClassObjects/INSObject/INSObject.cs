namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Message sent from INS portal to the service bus.
    /// </summary>
    [DataContract]
    public class INSObject
    {
        /// <summary>
        /// Gets event ID.
        /// </summary>
        [DataMember(Name = "eventId")]
        public string EventId { get; internal set; }

        /// <summary>
        /// Gets aggregate ID.
        /// </summary>
        [DataMember(Name = "aggregateId")]
        public string AggregateId { get; internal set; }

        /// <summary>
        /// Gets aggregate type.
        /// </summary>
        [DataMember(Name = "aggregateType")]
        public string AggregateType { get; internal set; }

        /// <summary>
        /// Gets sub type.
        /// </summary>
        [DataMember(Name = "subType")]
        public string SubType { get; internal set; }

        /// <summary>
        /// Gets aggregate version.
        /// </summary>
        [DataMember(Name = "aggregateVersion")]
        public int AggregateVersion { get; internal set; }

        /// <summary>
        /// Gets event type.
        /// </summary>
        [DataMember(Name = "eventType")]
        public string EventType { get; internal set; }

        /// <summary>
        /// Gets timestamp.
        /// </summary>
        [DataMember(Name = "timestamp")]
        public string Timestamp { get; internal set; }

        /// <summary>
        /// Gets metadata.
        /// </summary>
        [DataMember(Name = "metadata")]
        public Metadata Metadata { get; internal set; }

        /// <summary>
        /// Gets actor.
        /// </summary>
        [DataMember(Name = "actor")]
        public Actor Actor { get; internal set; }

        /// <summary>
        /// Gets status changes.
        /// </summary>
        [DataMember(Name = "statusChanges")]
        public StatusChange[] StatusChanges { get; internal set; }

        /// <summary>
        /// Gets data.
        /// </summary>
        [DataMember(Name = "data")]
        public Data Data { get; internal set; }
    }
}
