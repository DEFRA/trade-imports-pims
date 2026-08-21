namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Party details.
    /// </summary>
    [DataContract]
    public class Party
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
        /// Gets party role code.
        /// </summary>
        [DataMember(Name = "partyRoleCode")]
        public CodedValue PartyRoleCode { get; internal set; }

        /// <summary>
        /// Gets party type codes.
        /// </summary>
        [DataMember(Name = "partyTypeCode")]
        public CodedValue[] PartyTypeCode { get; internal set; }

        /// <summary>
        /// Gets postal address.
        /// </summary>
        [DataMember(Name = "postalAddress")]
        public PostalAddress PostalAddress { get; internal set; }

        /// <summary>
        /// Gets defined contacts.
        /// </summary>
        [DataMember(Name = "definedContact")]
        public DefinedContact[] DefinedContact { get; internal set; }
    }
}