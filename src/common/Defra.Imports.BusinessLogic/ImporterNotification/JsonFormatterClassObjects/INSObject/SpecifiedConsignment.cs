namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specified consignment details.
    /// </summary>
    [DataContract]
    public class SpecifiedConsignment
    {
        /// <summary>
        /// Gets consignor party.
        /// </summary>
        [DataMember(Name = "consignorParty")]
        public Party ConsignorParty { get; internal set; }

        /// <summary>
        /// Gets consignee party.
        /// </summary>
        [DataMember(Name = "consigneeParty")]
        public Party ConsigneeParty { get; internal set; }

        /// <summary>
        /// Gets despatch party.
        /// </summary>
        [DataMember(Name = "despatchParty")]
        public Party DespatchParty { get; internal set; }

        /// <summary>
        /// Gets delivery party.
        /// </summary>
        [DataMember(Name = "deliveryParty")]
        public Party DeliveryParty { get; internal set; }

        /// <summary>
        /// Gets importer.
        /// </summary>
        [DataMember(Name = "importer")]
        public Party Importer { get; internal set; }

        /// <summary>
        /// Gets carrier.
        /// </summary>
        [DataMember(Name = "carrier")]
        public Carrier Carrier { get; internal set; }

        /// <summary>
        /// Gets origin country.
        /// </summary>
        [DataMember(Name = "originCountry")]
        public TradeCountry OriginCountry { get; internal set; }

        /// <summary>
        /// Gets final destination location.
        /// </summary>
        [DataMember(Name = "finalDestinationLocation")]
        public FinalDestinationLocation FinalDestinationLocation { get; internal set; }

        /// <summary>
        /// Gets unloading baseport location.
        /// </summary>
        [DataMember(Name = "unloadingBaseportLocation")]
        public LogisticsLocation UnloadingBaseportLocation { get; internal set; }

        /// <summary>
        /// Gets main carriage logistics transport movements.
        /// </summary>
        [DataMember(Name = "mainCarriageLogisticsTransportMovement")]
        public MainCarriageLogisticsTransportMovement[] MainCarriageLogisticsTransportMovement { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether consignment has unweaned animals.
        /// </summary>
        [DataMember(Name = "isOrHasUnweanedAnimals")]
        public bool? IsOrHasUnweanedAnimals { get; internal set; }

        /// <summary>
        /// Gets transit trade country.
        /// </summary>
        [DataMember(Name = "transitTradeCountry")]
        public TradeCountry TransitTradeCountry { get; internal set; }

        /// <summary>
        /// Gets included consignment items.
        /// </summary>
        [DataMember(Name = "includedConsignmentItem")]
        public IncludedConsignmentItem[] IncludedConsignmentItem { get; internal set; }
    }
}