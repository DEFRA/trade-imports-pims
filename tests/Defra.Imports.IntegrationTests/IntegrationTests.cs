using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using Xunit;

namespace Defra.Imports.IntegrationTests
{
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
            _serviceBusConnectionString = ConfigurationManager.ConnectionStrings["DevServiceBusConnection"].ConnectionString;
            _serviceBusQueueName = ConfigurationManager.AppSettings["DevServiceBusQueueName"];
            InitaliseConnections();
        }

        public IntegrationTests(string serviceBusConnectionString, string serviceBusQueueName)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _serviceBusConnectionString = serviceBusConnectionString;
            _serviceBusQueueName = serviceBusQueueName;
            InitaliseConnections();
        }

        private void InitaliseConnections()
        {
            InitialiseDynamicsConnection();
            InitialiseServiceBusConnection();
        }

        private void InitialiseDynamicsConnection()
        {
            string cdsConnectionString = ConfigurationManager.ConnectionStrings["DevCdsConnection"].ConnectionString;
            if (cdsConnectionString.Contains("[ReplaceMe]"))
            {
                throw new Exception("You need to populate the DevCdsConnection connection string in app.config");
            }
            _orgSvc = new ServiceClient(cdsConnectionString);
        }

        private void InitialiseServiceBusConnection()
        {
            _serviceBusQueueClient = QueueClient.CreateFromConnectionString(_serviceBusConnectionString, _serviceBusQueueName);
        }

        protected void SetServiceBusConnection(string serviceBusConnectionString, string serviceBusQueueName)
        {
            _serviceBusConnectionString = serviceBusConnectionString;
            _serviceBusQueueName = serviceBusQueueName;
            InitialiseServiceBusConnection();
        }

        protected void SendServiceBusMessage(string message)
        {
            MemoryStream messageStream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            BrokeredMessage messageToSend = new BrokeredMessage(messageStream);
            messageToSend.SessionId = Guid.NewGuid().ToString();
            _serviceBusQueueClient.Send(messageToSend);
        }

        protected string ReadTestData(string fileName)
        {
            string filePath = $"{Directory.GetCurrentDirectory()}\\TestData\\{fileName}";
            string fileContents = ReadFileContents(filePath);
            return fileContents;
        }

        private string ReadFileContents(string filePath)
        {
            return File.ReadAllText(filePath);
        }
    }
}
