namespace Defra.Imports.UnitTests.Workflows.ImporterNotification
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.BusinessLogic.Extensions;
    using Defra.Imports.BusinessLogic.ImporterNotification;
    using Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="ProcessINSASBMessage"/>.
    /// </summary>
    public class ProcessINSASBMessageTests
    {
        private readonly Mock<IOrganizationService> orgSvcMock;
        private readonly Mock<ILogWriter> loggerMock;
        private readonly ProcessINSASBMessage sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessINSASBMessageTests"/> class.
        /// </summary>
        public ProcessINSASBMessageTests()
        {
            this.orgSvcMock = new Mock<IOrganizationService>();
            this.loggerMock = new Mock<ILogWriter>();
            this.sut = new ProcessINSASBMessage(this.orgSvcMock.Object, this.loggerMock.Object);

            // Default: country lookups return empty collection
            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defra_country.EntityLogicalName)))
                .Returns(new EntityCollection());
        }

        [Fact]
        [Trait("Category", "Deserialization")]
        public void UpsertImporterNotification_WithInvalidJson_ReturnsFalseAndLogsError()
        {
            // Act
            var result = this.sut.UpsertImporterNotification("not valid json {{{{");

            // Assert
            Assert.False(result.Item1);
            Assert.Contains("Error deserializing message", result.Item2);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Error,
                    nameof(ProcessINSASBMessage),
                    It.Is<string>(m => m.Contains("Error deserializing message"))),
                Times.Once);
        }

        [Fact]
        public void UpsertImporterNotification_WhenNoExistingRecordAndStatusSubmitted_CreatesRecordAndReturnsTrueWithSuccessMessage()
        {
            // Arrange
            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection());

            this.orgSvcMock
                .Setup(o => o.Create(It.IsAny<Entity>()))
                .Returns(Guid.NewGuid());

            var message = BuildMessage("INS-001", 1, "SUBMITTED");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.True(result.Item1);
            Assert.Contains("INS-001", result.Item2);
            Assert.Contains("created successfully", result.Item2);

            this.orgSvcMock.Verify(
                o => o.Create(It.Is<Entity>(e => e.LogicalName == defraimp_ImporterNotification.EntityLogicalName)),
                Times.Once);

            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void UpsertImporterNotification_WhenNoExistingRecordAndStatusAmend_CreatesRecordAndReturnsTrueWithSuccessMessage()
        {
            // Arrange
            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection());

            this.orgSvcMock
                .Setup(o => o.Create(It.IsAny<Entity>()))
                .Returns(Guid.NewGuid());

            var message = BuildMessage("INS-002", 1, "AMEND");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.True(result.Item1);
            Assert.Contains("created successfully", result.Item2);

            this.orgSvcMock.Verify(
                o => o.Create(It.Is<Entity>(e => e.LogicalName == defraimp_ImporterNotification.EntityLogicalName)),
                Times.Once);
        }

        [Fact]
        public void UpsertImporterNotification_WhenNoExistingRecordAndStatusDraft_DoesNotCreateAndReturnsFalseWithDraftMessage()
        {
            // Arrange
            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection());

            var message = BuildMessage("INS-003", 1, "DRAFT");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.False(result.Item1);
            Assert.Contains("Draft status", result.Item2);

            this.orgSvcMock.Verify(o => o.Create(It.IsAny<Entity>()), Times.Never);
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void UpsertImporterNotification_WhenExistingRecordWithLowerVersion_UpdatesRecordAndReturnsTrueWithSuccessMessage()
        {
            // Arrange
            var existingRecord = new defraimp_ImporterNotification
            {
                Id = Guid.NewGuid(),
                defraimp_Name = "INS-010",
                defraimp_AggregateVersion = 1,
            };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { existingRecord }));

            var message = BuildMessage("INS-010", 2, "SUBMITTED");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.True(result.Item1);
            Assert.Contains("INS-010", result.Item2);
            Assert.Contains("updated successfully", result.Item2);

            this.orgSvcMock.Verify(
                o => o.Update(It.Is<Entity>(e => e.LogicalName == defraimp_ImporterNotification.EntityLogicalName)),
                Times.Once);

            this.orgSvcMock.Verify(o => o.Create(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void UpsertImporterNotification_WhenExistingRecordWithEqualVersion_DoesNotUpdateAndReturnsFalseWithNoUpdateMessage()
        {
            // Arrange
            var existingRecord = new defraimp_ImporterNotification
            {
                Id = Guid.NewGuid(),
                defraimp_Name = "INS-011",
                defraimp_AggregateVersion = 3,
            };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { existingRecord }));

            var message = BuildMessage("INS-011", 3, "SUBMITTED");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.False(result.Item1);
            Assert.Contains("No update needed", result.Item2);
            Assert.Contains("INS-011", result.Item2);

            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
            this.orgSvcMock.Verify(o => o.Create(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void UpsertImporterNotification_WhenExistingRecordWithHigherVersion_DoesNotUpdateAndReturnsFalseWithNoUpdateMessage()
        {
            // Arrange
            var existingRecord = new defraimp_ImporterNotification
            {
                Id = Guid.NewGuid(),
                defraimp_Name = "INS-012",
                defraimp_AggregateVersion = 5,
            };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { existingRecord }));

            var message = BuildMessage("INS-012", 4, "SUBMITTED");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.False(result.Item1);
            Assert.Contains("No update needed", result.Item2);

            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
            this.orgSvcMock.Verify(o => o.Create(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void UpsertImporterNotification_WhenOrgServiceThrowsOnRetrieve_ReturnsFalseAndLogsError()
        {
            // Arrange
            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Throws(new InvalidOperationException("CRM unavailable"));

            var message = BuildMessage("INS-099", 1, "SUBMITTED");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.False(result.Item1);
            Assert.Contains("Error processing Importer Notification", result.Item2);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Error,
                    nameof(ProcessINSASBMessage),
                    It.Is<string>(m => m.Contains("Error processing Importer Notification"))),
                Times.Once);
        }

        [Fact]
        public void FormatAddress_WithAllParts_ReturnsSingleCommaDelimitedString()
        {
            // Act
            var result = this.sut.FormatAddress("1 High Street", "Flat 2", "London", "SW1A 1AA");

            // Assert
            Assert.Equal("1 High Street, Flat 2, London, SW1A 1AA", result);
        }

        [Fact]
        public void FormatAddress_WithNullAndEmptyParts_ExcludesBlankParts()
        {
            // Act
            var result = this.sut.FormatAddress("1 High Street", string.Empty, "London", null);

            // Assert
            Assert.Equal("1 High Street, London", result);
        }

        [Fact]
        public void FormatAddress_WithAllNullParts_ReturnsEmptyString()
        {
            // Act
            var result = this.sut.FormatAddress(null, null, null, null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void IsSameAddress_BothNull_ReturnsTrue()
        {
            // Act
            var result = this.sut.IsSameAddress(null, null);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSameAddress_FirstAddressNullSecondPopulated_ReturnsFalse()
        {
            // Arrange
            var address = BuildPostalAddress("1 High Street", null, "London", "SW1A 1AA", "GB");

            // Act
            var result = this.sut.IsSameAddress(null, address);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSameAddress_SecondAddressNullFirstPopulated_ReturnsFalse()
        {
            // Arrange
            var address = BuildPostalAddress("1 High Street", null, "London", "SW1A 1AA", "GB");

            // Act
            var result = this.sut.IsSameAddress(address, null);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSameAddress_IdenticalAddresses_ReturnsTrue()
        {
            // Arrange
            var address1 = BuildPostalAddress("1 High Street", "Flat 2", "London", "SW1A 1AA", "GB");
            var address2 = BuildPostalAddress("1 High Street", "Flat 2", "London", "SW1A 1AA", "GB");

            // Act
            var result = this.sut.IsSameAddress(address1, address2);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsSameAddress_AddressesWithDifferentPostcode_ReturnsFalse()
        {
            // Arrange
            var address1 = BuildPostalAddress("1 High Street", null, "London", "SW1A 1AA", "GB");
            var address2 = BuildPostalAddress("1 High Street", null, "London", "EC1A 1BB", "GB");

            // Act
            var result = this.sut.IsSameAddress(address1, address2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSameAddress_AddressesWithDifferentCountry_ReturnsFalse()
        {
            // Arrange
            var address1 = BuildPostalAddress("1 High Street", null, "London", "SW1A 1AA", "GB");
            var address2 = BuildPostalAddress("1 High Street", null, "London", "SW1A 1AA", "FR");

            // Act
            var result = this.sut.IsSameAddress(address1, address2);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsSameAddress_IdenticalAddressesDifferentCasing_ReturnsTrue()
        {
            // Arrange
            var address1 = BuildPostalAddress("1 high street", null, "LONDON", "sw1a 1aa", "gb");
            var address2 = BuildPostalAddress("1 HIGH STREET", null, "london", "SW1A 1AA", "GB");

            // Act
            var result = this.sut.IsSameAddress(address1, address2);

            // Assert
            Assert.True(result);
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

        private static PostalAddress BuildPostalAddress(string lineOne, string lineTwo, string city, string postcode, string countryId)
        {
            var json = $@"{{
              ""lineOne"": {ToJsonValue(lineOne)},
              ""lineTwo"": {ToJsonValue(lineTwo)},
              ""cityName"": {ToJsonValue(city)},
              ""postcodeCode"": {ToJsonValue(postcode)},
              ""countryId"": {ToJsonValue(countryId)}
            }}";
            return json.FromJSON<PostalAddress>();
        }

        private static string ToJsonValue(string value)
        {
            return value == null ? "null" : $"\"{value}\"";
        }
    }
}