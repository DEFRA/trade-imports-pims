namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Function type code details.
    /// </summary>
    [DataContract]
    public class FunctionTypeCode
    {
        /// <summary>
        /// Gets content.
        /// </summary>
        [DataMember(Name = "content")]
        public string Content { get; internal set; }
    }
}