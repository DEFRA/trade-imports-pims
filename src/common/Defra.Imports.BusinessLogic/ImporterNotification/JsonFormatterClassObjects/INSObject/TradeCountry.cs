namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Trade country details.
    /// </summary>
    [DataContract]
    public class TradeCountry
    {
        /// <summary>
        /// Gets code.
        /// </summary>
        [DataMember(Name = "code")]
        public CountryCode Code { get; internal set; }

        /// <summary>
        /// Gets subordinate trade country sub divisions.
        /// </summary>
        [DataMember(Name = "subordinateTradeCountrySubDivision")]
        public SubordinateTradeCountrySubDivision[] SubordinateTradeCountrySubDivision { get; internal set; }
    }
}