namespace Defra.Imports.UnitTests.Workflows.ImporterNotification
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.Model;
    using Defra.Imports.Workflows.ImporterNotification;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="UpsertImporterNotification"/>.
    /// </summary>
    public class UpsertImporterNotificationTests : WorkflowActivityTests<UpsertImporterNotification>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsertImporterNotificationTests"/> class.
        /// </summary>
        public UpsertImporterNotificationTests()
        {
            // Default: importer notification lookups return empty collection (no existing record)
            this.OrgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection());

            // Default: country lookups return empty collection
            this.OrgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defra_country.EntityLogicalName)))
                .Returns(new EntityCollection());

            // Default: commodity complement lookups return empty collection (no existing complements to delete)
            this.OrgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_commoditycomplement.EntityLogicalName)))
                .Returns(new EntityCollection());
        }

        /// <summary>
        /// Tests that invoking with a valid SUBMITTED message creates a record and sets Response to true with a success message.
        /// </summary>
        [Fact]
        public void Execute_WithValidSubmittedMessage_CreatesRecordAndSetsResponseTrueWithSuccessMessage()
        {
            // Arrange
            this.OrgSvcMock
                .Setup(o => o.Create(It.IsAny<Entity>()))
                .Returns(Guid.NewGuid());

            var inputs = BuildInputs(BuildMessage("INS-001", 1, "SUBMITTED"));

            // Act
            var outputs = this.WorkflowInvoker.Invoke(inputs);

            // Assert
            Assert.True((bool)outputs["Response"]);
            Assert.Contains("created successfully", (string)outputs["Message"]);

            this.OrgSvcMock.Verify(
                o => o.Create(It.Is<Entity>(e => e.LogicalName == defraimp_ImporterNotification.EntityLogicalName)),
                Times.Once);
        }

        /// <summary>
        /// Tests that invoking with a DRAFT message does not create a record and sets Response to false with a draft message.
        /// </summary>
        [Fact]
        public void Execute_WithDraftMessage_DoesNotCreateRecordAndSetsResponseFalseWithDraftMessage()
        {
            // Arrange
            var inputs = BuildInputs(BuildMessage("INS-002", 1, "DRAFT"));

            // Act
            var outputs = this.WorkflowInvoker.Invoke(inputs);

            // Assert
            Assert.False((bool)outputs["Response"]);
            Assert.Contains("Draft status", (string)outputs["Message"]);

            this.OrgSvcMock.Verify(o => o.Create(It.IsAny<Entity>()), Times.Never);
        }

        /// <summary>
        /// Tests that invoking with a null message sets Response to false and includes an error message.
        /// </summary>
        [Fact]
        public void Execute_WithNullMessage_SetsResponseFalseWithErrorMessage()
        {
            // Arrange
            var inputs = BuildInputs(null);

            // Act
            var outputs = this.WorkflowInvoker.Invoke(inputs);

            // Assert
            Assert.False((bool)outputs["Response"]);
            Assert.Contains("service bus message is null or empty", (string)outputs["Message"]);
        }

        /// <summary>
        /// Tests that invoking with invalid JSON sets Response to false and includes a deserialization error message.
        /// </summary>
        [Fact]
        public void Execute_WithInvalidJson_SetsResponseFalseWithDeserializationError()
        {
            // Arrange
            var inputs = BuildInputs("not valid json {{{");

            // Act
            var outputs = this.WorkflowInvoker.Invoke(inputs);

            // Assert
            Assert.False((bool)outputs["Response"]);
            Assert.Contains("Error deserializing message", (string)outputs["Message"]);
        }

        /// <summary>
        /// Tests that invoking with a message missing the identifier sets Response to false and includes an error message.
        /// </summary>
        [Fact]
        public void Execute_WithMissingIdentifier_SetsResponseFalseWithErrorMessage()
        {
            // Arrange
            var inputs = BuildInputs(@"{ ""aggregateVersion"": 1, ""data"": { ""exchangedDocument"": {} } }");

            // Act
            var outputs = this.WorkflowInvoker.Invoke(inputs);

            // Assert
            Assert.False((bool)outputs["Response"]);
            Assert.Contains("data.exchangedDocument.identifier", (string)outputs["Message"]);
        }

        /// <summary>
        /// Tests that invoking when an existing record has a lower version updates the record and sets Response to true.
        /// </summary>
        [Fact]
        public void Execute_WhenExistingRecordHasLowerVersion_UpdatesRecordAndSetsResponseTrue()
        {
            // Arrange
            var existingRecord = new defraimp_ImporterNotification
            {
                Id = Guid.NewGuid(),
                defraimp_Name = "INS-010",
                defraimp_AggregateVersion = 1,
            };

            this.OrgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { existingRecord }));

            var inputs = BuildInputs(BuildMessage("INS-010", 2, "SUBMITTED"));

            // Act
            var outputs = this.WorkflowInvoker.Invoke(inputs);

            // Assert
            Assert.True((bool)outputs["Response"]);
            Assert.Contains("updated successfully", (string)outputs["Message"]);

            this.OrgSvcMock.Verify(
                o => o.Update(It.Is<Entity>(e => e.LogicalName == defraimp_ImporterNotification.EntityLogicalName)),
                Times.Once);
        }

        /// <summary>
        /// Tests that invoking when an existing record has an equal or higher version does not update and sets Response to false.
        /// </summary>
        [Fact]
        public void Execute_WhenExistingRecordHasEqualVersion_DoesNotUpdateAndSetsResponseFalse()
        {
            // Arrange
            var existingRecord = new defraimp_ImporterNotification
            {
                Id = Guid.NewGuid(),
                defraimp_Name = "INS-011",
                defraimp_AggregateVersion = 3,
            };

            this.OrgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { existingRecord }));

            var inputs = BuildInputs(BuildMessage("INS-011", 3, "SUBMITTED"));

            // Act
            var outputs = this.WorkflowInvoker.Invoke(inputs);

            // Assert
            Assert.False((bool)outputs["Response"]);
            Assert.Contains("No update needed", (string)outputs["Message"]);

            this.OrgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
        }

        /// <summary>
        /// Tests that invoking when the org service throws sets Response to false and includes an error message.
        /// </summary>
        [Fact]
        public void Execute_WhenOrgServiceThrows_SetsResponseFalseWithErrorMessage()
        {
            // Arrange
            this.OrgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Throws(new InvalidOperationException("CRM unavailable"));

            var inputs = BuildInputs(BuildMessage("INS-099", 1, "SUBMITTED"));

            // Act
            var outputs = this.WorkflowInvoker.Invoke(inputs);

            // Assert
            Assert.False((bool)outputs["Response"]);
            Assert.Contains("Error processing", (string)outputs["Message"]);
        }

        private static Dictionary<string, object> BuildInputs(string asbMessage)
        {
            return new Dictionary<string, object>
            {
                { "ASBMessage", asbMessage },
            };
        }

        private static string BuildMessage(string identifier, int aggregateVersion, string notificationStatusCode)
        {
            return $@"{{
                ""aggregateVersion"": {aggregateVersion},
                ""data"": {{
                    ""exchangedDocument"": {{
                        ""identifier"": ""{identifier}"",
                        ""notificationStatusCode"": ""{notificationStatusCode}"",
                        ""versionId"": 1
                    }}
                }}
            }}";
        }
    }
}