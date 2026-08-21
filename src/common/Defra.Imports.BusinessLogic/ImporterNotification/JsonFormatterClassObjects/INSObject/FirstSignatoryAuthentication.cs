namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// First signatory authentication details.
    /// </summary>
    [DataContract]
    public class FirstSignatoryAuthentication
    {
        /// <summary>
        /// Gets included clauses.
        /// </summary>
        [DataMember(Name = "includedClause")]
        public IncludedClause[] IncludedClause { get; internal set; }
    }
}