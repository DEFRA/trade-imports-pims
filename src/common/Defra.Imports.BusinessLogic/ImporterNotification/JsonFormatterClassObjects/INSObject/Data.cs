namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Data object containing notification details.
    /// </summary>
    [DataContract]
    public class Data
    {
        /// <summary>
        /// Gets model.
        /// </summary>
        [DataMember(Name = "$model")]
        public string Model { get; internal set; }

        /// <summary>
        /// Gets type.
        /// </summary>
        [DataMember(Name = "$type")]
        public string Type { get; internal set; }

        /// <summary>
        /// Gets exchanged document.
        /// </summary>
        [DataMember(Name = "exchangedDocument")]
        public ExchangedDocument ExchangedDocument { get; internal set; }

        /// <summary>
        /// Gets specified consignment.
        /// </summary>
        [DataMember(Name = "specifiedConsignment")]
        public SpecifiedConsignment SpecifiedConsignment { get; internal set; }
    }
}