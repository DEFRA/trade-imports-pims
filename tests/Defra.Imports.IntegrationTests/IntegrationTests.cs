namespace Defra.Imports.IntegrationTests
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Net;
    using System.Text;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.ServiceBus.Messaging;
    using Xunit;

    [Collection("Sequential")]
    public abstract class IntegrationTests
    {
        protected ServiceClient _orgSvc;
        protected QueueClient _serviceBusQueueClient;
        private string _serviceBusConnectionString;
        private string _serviceBusQueueName;

        public IntegrationTests()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            this._serviceBusConnectionString = ConfigurationManager.ConnectionStrings["DevServiceBusConnection"].ConnectionString;
            this._serviceBusQueueName = ConfigurationManager.AppSettings["DevServiceBusQueueName"];
            this.InitaliseConnections();
        }

        public IntegrationTests(string serviceBusConnectionString, string serviceBusQueueName)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            this._serviceBusConnectionString = serviceBusConnectionString;
            this._serviceBusQueueName = serviceBusQueueName;
            this.InitaliseConnections();
        }

        private void InitaliseConnections()
        {
            this.InitialiseDynamicsConnection();
            this.InitialiseServiceBusConnection();
        }

        private void InitialiseDynamicsConnection()
        {
            string cdsConnectionString = ConfigurationManager.ConnectionStrings["DevCdsConnection"].ConnectionString;
            if (cdsConnectionString.Contains("[ReplaceMe]"))
            {
                throw new Exception("You need to populate the DevCdsConnection connection string in app.config");
            }
            this._orgSvc = new ServiceClient(cdsConnectionString);
        }

        private void InitialiseServiceBusConnection()
        {
            this._serviceBusQueueClient = QueueClient.CreateFromConnectionString(this._serviceBusConnectionString, this._serviceBusQueueName);
        }

        protected void SetServiceBusConnection(string serviceBusConnectionString, string serviceBusQueueName)
        {
            this._serviceBusConnectionString = serviceBusConnectionString;
            this._serviceBusQueueName = serviceBusQueueName;
            this.InitialiseServiceBusConnection();
        }

        protected void SendServiceBusMessage(string message)
        {
            MemoryStream messageStream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            BrokeredMessage messageToSend = new BrokeredMessage(messageStream);
            messageToSend.SessionId = Guid.NewGuid().ToString();
            this._serviceBusQueueClient.Send(messageToSend);
        }

        protected string ReadTestData(string fileName)
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\TestData\\{fileName}";
            string fileContents = this.ReadFileContents(filePath);
            return fileContents;
        }

        private string ReadFileContents(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
