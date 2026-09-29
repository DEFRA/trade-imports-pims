namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Included consignment item details.
    /// </summary>
    [DataContract]
    public class IncludedConsignmentItem
    {
        /// <summary>
        /// Gets included trade line items.
        /// </summary>
        [DataMember(Name = "includedTradeLineItem")]
        public IncludedTradeLineItem[] IncludedTradeLineItem { get; internal set; }
    }
}