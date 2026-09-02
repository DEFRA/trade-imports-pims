namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Metadata details for the notification message.
    /// </summary>
    [DataContract]
    public class Metadata
    {
        /// <summary>
        /// Gets correlation ID.
        /// </summary>
        [DataMember(Name = "correlationId")]
        public string CorrelationId { get; internal set; }

        /// <summary>
        /// Gets schema version.
        /// </summary>
        [DataMember(Name = "schemaVersion")]
        public string SchemaVersion { get; internal set; }

        /// <summary>
        /// Gets schema URL.
        /// </summary>
        [DataMember(Name = "schemaUrl")]
        public string SchemaUrl { get; internal set; }
    }
}