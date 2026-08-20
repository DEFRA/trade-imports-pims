namespace Defra.Imports.IntegrationTests.LogicApps
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading;
    using Defra.Imports.IntegrationTests.ServiceBus;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    [TestClass]
    [Ignore("These tests are ignored pending review")]
    public class NotificationsLogicAppIntegrationTests : IntegrationTests, IDisposable
    {
        private readonly ServiceBusFixture serviceBus;
        private readonly ServiceClient appUserClient;
        private bool disposed;

        public NotificationsLogicAppIntegrationTests()
        {
            this.serviceBus = this.GetServiceBusFixture(TestConfig.ServiceBus.NotificationQueue);
            this.appUserClient = this.GetAppUserClient();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the underlying <see cref="ServiceBusFixture"/>.
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
                this.serviceBus?.Dispose();
            }

            this.disposed = true;
        }

        private void SendServiceBusMessage(string message)
        {
            this.serviceBus.SendMessage(message);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_AValidNotificationJSONMessage_NotificationIsCreatedInDynamics()
        {
            // Arrange
            string expectedReferenceNumber = "IMP.GB.2020.1282123";
            string jsonContents = this.ReadTestData("NOTIFICATION1.json");

            // Act
            this.SendServiceBusMessage(jsonContents);
            Thread.Sleep(150000);

            // Assert
            List<Entity> notifications = this.GetNotificationsByReference(expectedReferenceNumber);
            Assert.IsTrue(notifications.Count > 0);

            // Clear down
            this.ClearDownDynamicsEntities(notifications);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_FiveMessagesFinalMessageCancelled_NotificationShouldBeCreatedAndThenCancelled()
        {
            // Arrange
            string expectedReferenceNumber = "IMP.GB.2020.1462627";
            List<string> messages = new List<string>();
            messages.Add(this.ReadTestData("NOTIFICATION_1_SUBMITTED.json"));
            messages.Add(this.ReadTestData("NOTIFICATION_2_AMEND.json"));
            messages.Add(this.ReadTestData("NOTIFICATION_3_SUBMITTED.json"));
            messages.Add(this.ReadTestData("NOTIFICATION_4_AMEND.json"));
            messages.Add(this.ReadTestData("NOTIFICATION_5_CANCELLED.json"));

            // Act
            foreach(string jsonMessage in messages)
            {
                this.SendServiceBusMessage(jsonMessage);
            }

            Thread.Sleep(150000);

            // Assert
            List<Entity> notifications = this.GetNotificationsByReference(expectedReferenceNumber);
            Assert.IsTrue(notifications.Count > 0);
            Assert.IsTrue(notifications[0].GetAttributeValue<OptionSetValue>("defraimp_status").Value == 714100008);

            // Clear down
            this.ClearDownDynamicsEntities(notifications);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_NotificationWithNoIdentifiers_NotificationShouldBeCreatedSuccessfully()
        {
            // Arrange
            string expectedReferenceNumber = "IMP.GB.2020.1548741";
            string jsonContents = this.ReadTestData("NOTIFICATION_NO_IDENTIFIERS.json");

            // Act
            this.SendServiceBusMessage(jsonContents);
            Thread.Sleep(150000);

            // Assert
            List<Entity> notifications = this.GetNotificationsByReference(expectedReferenceNumber);
            Assert.IsTrue(notifications.Count > 0);

            // Clear down
            this.ClearDownDynamicsEntities(notifications);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_NotificationWithSingleQuoteText_NotificationShouldBeCreatedSuccessfully()
        {
            // Arrange
            string expectedReferenceNumber = "IMP.GB.2020.1462664";
            string jsonContents = this.ReadTestData("NOTIFICATION_CONTAINING_SINGLE_QUOTE.json");

            // Act
            this.SendServiceBusMessage(jsonContents);
            Thread.Sleep(150000);

            // Assert
            List<Entity> notifications = this.GetNotificationsByReference(expectedReferenceNumber);
            Assert.IsTrue(notifications.Count > 0);

            // Clear down
            this.ClearDownDynamicsEntities(notifications);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_NotificationWithDocuments_NotificationAndDocumentsShouldBeCreatedSuccesfully()
        {
            string expectedReferenceNumber = "IMP.GB.2020.1499116";
            string jsonContents = this.ReadTestData("NOTIFICATION_WITH_DOCS.json");

            // Act
            this.SendServiceBusMessage(jsonContents);
            Thread.Sleep(150000);

            // Assert
            List<Entity> notifications = this.GetNotificationsByReference(expectedReferenceNumber);
            Assert.IsTrue(notifications.Count > 0);

            // Clear down
            this.ClearDownDynamicsEntities(notifications);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_NotificationWithCountryOfDestination_NotificationShouldBeCreatedSuccesfully()
        {
            string expectedReferenceNumber = "IMP.GB.2020.1586510";
            string jsonContents = this.ReadTestData("NOTIFICATION_COUNTRY_OF_DESTINATION.json");

            // Act
            this.SendServiceBusMessage(jsonContents);
            Thread.Sleep(150000);

            // Assert
            List<Entity> notifications = this.GetNotificationsByReference(expectedReferenceNumber);
            Assert.IsTrue(notifications.Count > 0);

            // Clear down
            this.ClearDownDynamicsEntities(notifications);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_SequentialNotificationsWithDocument_NotificationShouldUpdateSequentially()
        {
            string expectedReferenceNumber = "IMP.GB.2020.1589711";
            string jsonContents1A = this.ReadTestData("NOTIFICATION_SEQUENTIAL_1_A.json");
            string jsonContents1B = this.ReadTestData("NOTIFICATION_SEQUENTIAL_1_B.json");
            string jsonContents2A = this.ReadTestData("NOTIFICATION_SEQUENTIAL_2_A.json");
            string jsonContents2B = this.ReadTestData("NOTIFICATION_SEQUENTIAL_2_B.json");

            // Act
            this.SendServiceBusMessage(jsonContents1A);
            this.SendServiceBusMessage(jsonContents1B);
            this.SendServiceBusMessage(jsonContents2A);
            this.SendServiceBusMessage(jsonContents2B);
            Thread.Sleep(150000);

            // Assert
            List<Entity> notifications = this.GetNotificationsByReference(expectedReferenceNumber);
            Assert.IsTrue(notifications.Count > 0);

            // Clear down
            this.ClearDownDynamicsEntities(notifications);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_NotificationWithPortOfExitDetails_ShouldCreateNotifictionWithPortOfExitDetails()
        {
            string expectedReferenceNumber = "IMP.GB.2020.19999999";
            string jsonContents = this.ReadTestData("NOTIFICATION_PORT_OF_EXIT.json");

            // Act
            this.SendServiceBusMessage(jsonContents);
            Thread.Sleep(150000);

            // Assert
            List<Entity> notifications = this.GetNotificationsByReference(expectedReferenceNumber);
            Assert.IsTrue(notifications.Count > 0);

            // Clear down
            this.ClearDownDynamicsEntities(notifications);
        }

        [TestMethod]
        [ExcludeFromCodeCoverage]
        public void SendToSBQueue_NotificationWithoutDestinationPostcode_ShouldCreateNotificationWithDevolvedOfficeUnknown()
        {
            string expectedReferenceNumber = "IMP.GB.2020.1111111";
            string jsonContents = this.ReadTestData("NOTIFICATION_NO_DESTINATION_POSTCODE.json");

            // Act
            this.SendServiceBusMessage(jsonContents);
            Thread.Sleep(150000);

            // Assert
            List<Entity> notifications = this.GetNotificationsByReference(expectedReferenceNumber);
            Assert.IsTrue(notifications.Count > 0);

            // Clear down
            this.ClearDownDynamicsEntities(notifications);
        }

        private List<Entity> GetNotificationsByReference(string referenceNumber)
        {
            QueryExpression qe = new QueryExpression("defraimp_importernotification");
            qe.Criteria.AddCondition(new ConditionExpression("defraimp_name", ConditionOperator.Equal, referenceNumber));
            qe.ColumnSet.AddColumn("defraimp_name");
            qe.ColumnSet.AddColumn("defraimp_status");

            EntityCollection eCollection = this.appUserClient.RetrieveMultiple(qe);
            return eCollection.Entities.ToList();
        }

        private void ClearDownDynamicsEntities(List<Entity> entitiesToDelete)
        {
            foreach(Entity entity in entitiesToDelete)
            {
                this.appUserClient.Delete(entity.LogicalName, entity.Id);
            }
        }
    }
}