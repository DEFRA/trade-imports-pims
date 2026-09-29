namespace Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Transport event details (departure or arrival).
    /// </summary>
    [DataContract]
    public class TransportEvent
    {
        /// <summary>
        /// Gets scheduled occurrence date time.
        /// </summary>
        [DataMember(Name = "scheduledOccurrenceDateTime")]
        public string ScheduledOccurrenceDateTime { get; internal set; }

        /// <summary>
        /// Gets actual occurrence date time.
        /// </summary>
        [DataMember(Name = "actualOccurrenceDateTime")]
        public string ActualOccurrenceDateTime { get; internal set; }

        /// <summary>
        /// Gets occurrence logistics location.
        /// </summary>
        [DataMember(Name = "occurrenceLogisticsLocation")]
        public LogisticsLocation OccurrenceLogisticsLocation { get; internal set; }
    }
}