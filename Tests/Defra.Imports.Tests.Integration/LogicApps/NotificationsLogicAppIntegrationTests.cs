using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Integration.LogicApps
{
    public class NotificationsLogicAppIntegrationTests : IntegrationTests
    {
        public NotificationsLogicAppIntegrationTests()
            : base(ConfigurationManager.ConnectionStrings["DevServiceBusConnection"].ConnectionString, ConfigurationManager.AppSettings["DevServiceBusNotificationQueueName"])
        {
        }

        [Fact]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_AValidNotificationJSONMessage_NotificationIsCreatedInDynamics()
        {
            // Arrange
            string expectedReferenceNumber = "IMP.GB.2020.1282123";
            string jsonContents = ReadTestData("NOTIFICATION1.json");

            // Act
            SendServiceBusMessage(jsonContents);
            Thread.Sleep(150000);

            // Assert
            List<Entity> notifications = GetNotificationsByReference(expectedReferenceNumber);
            Assert.True(notifications.Count > 0);

            // Clear down
            ClearDownDynamicsEntities(notifications);
        }

        private List<Entity> GetNotificationsByReference(string referenceNumber)
        {
            QueryExpression qe = new QueryExpression("defraimp_importnotification");
            qe.Criteria.AddCondition(new ConditionExpression("defraimp_name", ConditionOperator.Equal, referenceNumber));
            qe.ColumnSet.AddColumn("defraimp_name");

            EntityCollection eCollection = _orgSvc.RetrieveMultiple(qe);
            return eCollection.Entities.ToList();
        }

        private void ClearDownDynamicsEntities(List<Entity> entitiesToDelete)
        {
            foreach(Entity entity in entitiesToDelete)
            {
                _orgSvc.Delete(entity.LogicalName, entity.Id);
            }
        }
    }
}
