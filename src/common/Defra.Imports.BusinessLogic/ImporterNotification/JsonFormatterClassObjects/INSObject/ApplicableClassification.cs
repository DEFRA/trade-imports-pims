namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Applicable classification details.
    /// </summary>
    [DataContract]
    public class ApplicableClassification
    {
        /// <summary>
        /// Gets system ID.
        /// </summary>
        [DataMember(Name = "systemId")]
        public string SystemId { get; internal set; }

        /// <summary>
        /// Gets system name.
        /// </summary>
        [DataMember(Name = "systemName")]
        public string SystemName { get; internal set; }

        /// <summary>
        /// Gets class name.
        /// </summary>
        [DataMember(Name = "className")]
        public string[] ClassName { get; internal set; }

        /// <summary>
        /// Gets class code.
        /// </summary>
        [DataMember(Name = "classCode")]
        public ClassCode ClassCode { get; internal set; }
    }
}