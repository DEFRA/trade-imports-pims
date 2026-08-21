namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Attachment binary object details.
    /// </summary>
    [DataContract]
    public class AttachmentBinaryObject
    {
        /// <summary>
        /// Gets URI.
        /// </summary>
        [DataMember(Name = "uri")]
        public string Uri { get; internal set; }

        /// <summary>
        /// Gets filename.
        /// </summary>
        [DataMember(Name = "filename")]
        public string Filename { get; internal set; }

        /// <summary>
        /// Gets MIME code.
        /// </summary>
        [DataMember(Name = "mimeCode")]
        public string MimeCode { get; internal set; }
    }
}