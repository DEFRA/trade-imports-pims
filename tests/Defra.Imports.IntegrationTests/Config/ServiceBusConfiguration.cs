namespace Defra.Imports.IntegrationTests.Config
{
    /// <summary>
    /// Configuration for connecting to Azure Service Bus.
    /// </summary>
    public class ServiceBusConfiguration
    {
        /// <summary>
        /// Gets or sets the Service Bus connection string.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the queue name for health certificate messages.
        /// </summary>
        public string HealthCertQueue { get; set; }

        /// <summary>
        /// Gets or sets the queue name for notification messages.
        /// </summary>
        public string NotificationQueue { get; set; }
    }
}
