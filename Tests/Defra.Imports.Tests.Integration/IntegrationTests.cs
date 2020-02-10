using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Tests.Integration
{
    public abstract class IntegrationTests
    {
        protected CrmServiceClient _orgSvc;
        protected QueueClient _serviceBusQueueClient;

        public IntegrationTests()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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
            _orgSvc = new CrmServiceClient(cdsConnectionString);
        }

        private void InitialiseServiceBusConnection()
        {
            string serviceBusConnectionString = ConfigurationManager.ConnectionStrings["DevServiceBusConnection"].ConnectionString;
            string serviceBusQueueName = ConfigurationManager.AppSettings["DevServiceBusQueueName"];
            _serviceBusQueueClient = QueueClient.CreateFromConnectionString(serviceBusConnectionString, serviceBusQueueName);
        }

        protected void SendServiceBusMessage(string message)
        {
            MemoryStream messageStream = new MemoryStream(Encoding.UTF8.GetBytes(message));
            BrokeredMessage messageToSend = new BrokeredMessage(messageStream);
            _serviceBusQueueClient.Send(messageToSend);
        }
    }
}
