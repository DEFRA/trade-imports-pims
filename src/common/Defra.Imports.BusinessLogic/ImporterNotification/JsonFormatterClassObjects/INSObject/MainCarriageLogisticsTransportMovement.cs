namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Main carriage logistics transport movement details.
    /// </summary>
    [DataContract]
    public class MainCarriageLogisticsTransportMovement
    {
        /// <summary>
        /// Gets identifier.
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; internal set; }

        /// <summary>
        /// Gets mode code.
        /// </summary>
        [DataMember(Name = "modeCode")]
        public int? ModeCode { get; internal set; }

        /// <summary>
        /// Gets transport contract related referenced documents.
        /// </summary>
        [DataMember(Name = "transportContractRelatedReferencedDocument")]
        public TransportContractRelatedReferencedDocument[] TransportContractRelatedReferencedDocument { get; internal set; }

        /// <summary>
        /// Gets departure events.
        /// </summary>
        [DataMember(Name = "departureEvent")]
        public TransportEvent[] DepartureEvent { get; internal set; }

        /// <summary>
        /// Gets arrival events.
        /// </summary>
        [DataMember(Name = "arrivalEvent")]
        public TransportEvent[] ArrivalEvent { get; internal set; }

        /// <summary>
        /// Gets URL ID.
        /// </summary>
        [DataMember(Name = "urlId")]
        public string UrlId { get; internal set; }
    }
}