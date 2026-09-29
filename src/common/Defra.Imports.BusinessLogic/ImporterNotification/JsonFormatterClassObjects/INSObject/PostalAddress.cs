namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Postal address details.
    /// </summary>
    [DataContract]
    public class PostalAddress
    {
        /// <summary>
        /// Gets line one.
        /// </summary>
        [DataMember(Name = "lineOne")]
        public string LineOne { get; internal set; }

        /// <summary>
        /// Gets line two.
        /// </summary>
        [DataMember(Name = "lineTwo")]
        public string LineTwo { get; internal set; }

        /// <summary>
        /// Gets city name.
        /// </summary>
        [DataMember(Name = "cityName")]
        public string CityName { get; internal set; }

        /// <summary>
        /// Gets postcode code.
        /// </summary>
        [DataMember(Name = "postcodeCode")]
        public string PostcodeCode { get; internal set; }

        /// <summary>
        /// Gets country ID.
        /// </summary>
        [DataMember(Name = "countryId")]
        public string CountryId { get; internal set; }

        /// <summary>
        /// Gets country name.
        /// </summary>
        [DataMember(Name = "countryName")]
        public string CountryName { get; internal set; }

        /// <summary>
        /// Gets country sub division name.
        /// </summary>
        [DataMember(Name = "countrySubDivisionName")]
        public string CountrySubDivisionName { get; internal set; }
    }
}