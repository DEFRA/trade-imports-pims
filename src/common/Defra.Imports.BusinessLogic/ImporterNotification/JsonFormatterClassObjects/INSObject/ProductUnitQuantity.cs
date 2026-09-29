namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Product unit quantity details.
    /// </summary>
    [DataContract]
    public class ProductUnitQuantity
    {
        /// <summary>
        /// Gets content.
        /// </summary>
        [DataMember(Name = "content")]
        public int? Content { get; internal set; }

        /// <summary>
        /// Gets unit code.
        /// </summary>
        [DataMember(Name = "unitCode")]
        public string UnitCode { get; internal set; }
    }
}