namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Country code details.
    /// </summary>
    [DataContract]
    public class CountryCode
    {
        /// <summary>
        /// Gets value.
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; internal set; }

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
    }
}