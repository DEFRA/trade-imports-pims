namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Physical referenced logistics package details.
    /// </summary>
    [DataContract]
    public class PhysicalReferencedLogisticsPackage
    {
        /// <summary>
        /// Gets level code.
        /// </summary>
        [DataMember(Name = "levelCode")]
        public int? LevelCode { get; internal set; }

        /// <summary>
        /// Gets type code.
        /// </summary>
        [DataMember(Name = "typeCode")]
        public string TypeCode { get; internal set; }

        /// <summary>
        /// Gets item quantity.
        /// </summary>
        [DataMember(Name = "itemQuantity")]
        public int? ItemQuantity { get; internal set; }
    }
}