namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Included trade line item details.
    /// </summary>
    [DataContract]
    public class IncludedTradeLineItem
    {
        /// <summary>
        /// Gets applicable classification.
        /// </summary>
        [DataMember(Name = "applicableClassification")]
        public ApplicableClassification[] ApplicableClassification { get; internal set; }

        /// <summary>
        /// Gets type code.
        /// </summary>
        [DataMember(Name = "typeCode")]
        public string TypeCode { get; internal set; }

        /// <summary>
        /// Gets URL ID.
        /// </summary>
        [DataMember(Name = "urlId")]
        public string UrlId { get; internal set; }

        /// <summary>
        /// Gets descriptions.
        /// </summary>
        [DataMember(Name = "description")]
        public string[] Description { get; internal set; }

        /// <summary>
        /// Gets scientific name.
        /// </summary>
        [DataMember(Name = "scientificName")]
        public string ScientificName { get; internal set; }

        /// <summary>
        /// Gets common name.
        /// </summary>
        [DataMember(Name = "commonName")]
        public string CommonName { get; internal set; }

        /// <summary>
        /// Gets physical referenced logistics packages.
        /// </summary>
        [DataMember(Name = "physicalReferencedLogisticsPackage")]
        public PhysicalReferencedLogisticsPackage[] PhysicalReferencedLogisticsPackage { get; internal set; }

        /// <summary>
        /// Gets individual trade product instances.
        /// </summary>
        [DataMember(Name = "individualTradeProductInstance")]
        public IndividualTradeProductInstance[] IndividualTradeProductInstance { get; internal set; }

        /// <summary>
        /// Gets specified line trade deliveries.
        /// </summary>
        [DataMember(Name = "specifiedLineTradeDelivery")]
        public SpecifiedLineTradeDelivery[] SpecifiedLineTradeDelivery { get; internal set; }
    }
}