namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Final destination location details.
    /// </summary>
    [DataContract]
    public class FinalDestinationLocation
    {
        /// <summary>
        /// Gets identifier.
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; internal set; }

        /// <summary>
        /// Gets URL ID.
        /// </summary>
        [DataMember(Name = "urlId")]
        public string UrlId { get; internal set; }

        /// <summary>
        /// Gets name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; internal set; }

        /// <summary>
        /// Gets postal address.
        /// </summary>
        [DataMember(Name = "postalAddress")]
        public PostalAddress PostalAddress { get; internal set; }
    }
}