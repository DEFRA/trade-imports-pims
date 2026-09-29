namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Specified line trade delivery details.
    /// </summary>
    [DataContract]
    public class SpecifiedLineTradeDelivery
    {
        /// <summary>
        /// Gets product unit quantity.
        /// </summary>
        [DataMember(Name = "productUnitQuantity")]
        public ProductUnitQuantity ProductUnitQuantity { get; internal set; }
    }
}