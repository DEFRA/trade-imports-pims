namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Exchanged document details.
    /// </summary>
    [DataContract]
    public class ExchangedDocument
    {
        /// <summary>
        /// Gets identifier.
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; internal set; }

        /// <summary>
        /// Gets trader assigned ID.
        /// </summary>
        [DataMember(Name = "traderAssignedId")]
        public string TraderAssignedId { get; internal set; }

        /// <summary>
        /// Gets notification status code.
        /// </summary>
        [DataMember(Name = "notificationStatusCode")]
        public string NotificationStatusCode { get; internal set; }

        /// <summary>
        /// Gets version ID.
        /// </summary>
        [DataMember(Name = "versionId")]
        public int? VersionId { get; internal set; }

        /// <summary>
        /// Gets issue date time.
        /// </summary>
        [DataMember(Name = "issueDateTime")]
        public string IssueDateTime { get; internal set; }

        /// <summary>
        /// Gets issuer.
        /// </summary>
        [DataMember(Name = "issuer")]
        public Issuer Issuer { get; internal set; }

        /// <summary>
        /// Gets first signatory authentication.
        /// </summary>
        [DataMember(Name = "firstSignatoryAuthentication")]
        public FirstSignatoryAuthentication FirstSignatoryAuthentication { get; internal set; }

        /// <summary>
        /// Gets reference documents.
        /// </summary>
        [DataMember(Name = "referenceDocument")]
        public ReferenceDocument[] ReferenceDocument { get; internal set; }
    }
}