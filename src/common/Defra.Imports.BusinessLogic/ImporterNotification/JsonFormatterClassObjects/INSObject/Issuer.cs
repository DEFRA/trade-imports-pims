namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Issuer details.
    /// </summary>
    [DataContract]
    public class Issuer
    {
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

        /// <summary>
        /// Gets defined contacts.
        /// </summary>
        [DataMember(Name = "definedContact")]
        public DefinedContact[] DefinedContact { get; internal set; }
    }
}