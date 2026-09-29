namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Reference document details.
    /// </summary>
    [DataContract]
    public class ReferenceDocument
    {
        /// <summary>
        /// Gets type code.
        /// </summary>
        [DataMember(Name = "typeCode")]
        public string TypeCode { get; internal set; }

        /// <summary>
        /// Gets identifier.
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; internal set; }

        /// <summary>
        /// Gets issue date time.
        /// </summary>
        [DataMember(Name = "issueDateTime")]
        public string IssueDateTime { get; internal set; }

        /// <summary>
        /// Gets attachment binary objects.
        /// </summary>
        [DataMember(Name = "attachmentBinaryObject")]
        public AttachmentBinaryObject[] AttachmentBinaryObject { get; internal set; }
    }
}