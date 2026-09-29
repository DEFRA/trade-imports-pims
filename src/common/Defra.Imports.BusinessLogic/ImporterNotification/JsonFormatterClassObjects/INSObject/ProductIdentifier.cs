namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Product identifier details.
    /// </summary>
    [DataContract]
    public class ProductIdentifier
    {
        /// <summary>
        /// Gets type code.
        /// </summary>
        [DataMember(Name = "typeCode")]
        public string TypeCode { get; internal set; }

        /// <summary>
        /// Gets content.
        /// </summary>
        [DataMember(Name = "content")]
        public string Content { get; internal set; }

        /// <summary>
        /// Gets URL ID.
        /// </summary>
        [DataMember(Name = "urlId")]
        public string UrlId { get; internal set; }
    }
}