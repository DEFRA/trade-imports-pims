namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Individual trade product instance details.
    /// </summary>
    [DataContract]
    public class IndividualTradeProductInstance
    {
        /// <summary>
        /// Gets name.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; internal set; }

        /// <summary>
        /// Gets identifier.
        /// </summary>
        [DataMember(Name = "identifier")]
        public ProductIdentifier Identifier { get; internal set; }

        /// <summary>
        /// Gets permanent location.
        /// </summary>
        [DataMember(Name = "permanentLocation")]
        public PermanentLocation PermanentLocation { get; internal set; }
    }
}