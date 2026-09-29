namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Actor details representing a user or system performing an action.
    /// </summary>
    [DataContract]
    public class Actor
    {
        /// <summary>
        /// Gets ID.
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; internal set; }

        /// <summary>
        /// Gets source.
        /// </summary>
        [DataMember(Name = "source")]
        public string Source { get; internal set; }

        /// <summary>
        /// Gets user type.
        /// </summary>
        [DataMember(Name = "userType")]
        public string UserType { get; internal set; }

        /// <summary>
        /// Gets display name.
        /// </summary>
        [DataMember(Name = "displayName")]
        public string DisplayName { get; internal set; }

        /// <summary>
        /// Gets organisation ID.
        /// </summary>
        [DataMember(Name = "organisationId")]
        public string OrganisationId { get; internal set; }
    }
}