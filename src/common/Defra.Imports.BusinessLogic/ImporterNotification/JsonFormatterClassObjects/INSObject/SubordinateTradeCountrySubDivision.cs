namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Subordinate trade country sub division details.
    /// </summary>
    [DataContract]
    public class SubordinateTradeCountrySubDivision
    {
        /// <summary>
        /// Gets identifier.
        /// </summary>
        [DataMember(Name = "identifier")]
        public string Identifier { get; internal set; }

        /// <summary>
        /// Gets function type code.
        /// </summary>
        [DataMember(Name = "functionTypeCode")]
        public FunctionTypeCode FunctionTypeCode { get; internal set; }
    }
}