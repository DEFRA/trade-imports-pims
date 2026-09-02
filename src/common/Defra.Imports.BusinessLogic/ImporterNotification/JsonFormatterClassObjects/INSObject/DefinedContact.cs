namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Defined contact details.
    /// </summary>
    [DataContract]
    public class DefinedContact
    {
        /// <summary>
        /// Gets person name.
        /// </summary>
        [DataMember(Name = "personName")]
        public string PersonName { get; internal set; }

        /// <summary>
        /// Gets email URI universal communication.
        /// </summary>
        [DataMember(Name = "emailURIUniversalCommunication")]
        public string EmailURIUniversalCommunication { get; internal set; }

        /// <summary>
        /// Gets telephone universal communication.
        /// </summary>
        [DataMember(Name = "telephoneUniversalCommunication")]
        public string TelephoneUniversalCommunication { get; internal set; }
    }
}