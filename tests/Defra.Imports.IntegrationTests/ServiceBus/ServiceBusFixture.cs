namespace Defra.Imports.IntegrationTests.ServiceBus
{
    using System;
    using System.IO;
    using System.Text;
    using Microsoft.ServiceBus.Messaging;

    /// <summary>
    /// Provides on-demand access to an Azure Service Bus queue for integration tests.
    /// </summary>
    public class ServiceBusFixture : IDisposable
    {
        private readonly QueueClient queueClient;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceBusFixture"/> class.
        /// </summary>
        /// <param name="connectionString">The Service Bus connection string.</param>
        /// <param name="queueName">The name of the queue to send messages to.</param>
        public ServiceBusFixture(string connectionString, string queueName)
        {
            this.queueClient = QueueClient.CreateFromConnectionString(connectionString, queueName);
        }

        /// <summary>
        /// Sends a message to the queue.
        /// </summary>
        /// <param name="message">The message content to send.</param>
        public void SendMessage(string message)
        {
            using (var messageStream = new MemoryStream(Encoding.UTF8.GetBytes(message)))
            using (var messageToSend = new BrokeredMessage(messageStream)
            {
                SessionId = Guid.NewGuid().ToString(),
            })
            {
                this.queueClient.Send(messageToSend);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the underlying <see cref="QueueClient"/>.
        /// </summary>
        /// <param name="disposing">Whether this is being called from <see cref="Dispose()"/>.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                this.queueClient?.Close();
            }

            this.disposed = true;
        }
    }
}
