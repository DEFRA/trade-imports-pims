namespace Defra.Imports.UnitTests.BusinessLogic.ImporterNotification
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

        /// <summary>
        /// Tests that UpsertImporterNotification returns false and logs an error when given invalid JSON.
        /// </summary>
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

        /// <summary>
        /// Tests that UpsertImporterNotification returns false and logs an error when the message is null.
        /// </summary>
        [Fact]
        public void UpsertImporterNotification_WithNullMessage_ReturnsFalseAndLogsError()
        {
            // Act
            var result = this.sut.UpsertImporterNotification(null);

            // Assert
            Assert.False(result.Item1);
            Assert.Contains("service bus message is null or empty", result.Item2);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Error,
                    nameof(ProcessINSASBMessage),
                    It.Is<string>(m => m.Contains("service bus message is null or empty"))),
                Times.Once);
        }

        /// <summary>
        /// Tests that UpsertImporterNotification returns false and logs an error when the message is whitespace.
        /// </summary>
        [Fact]
        public void UpsertImporterNotification_WithWhitespaceMessage_ReturnsFalseAndLogsError()
        {
            // Act
            var result = this.sut.UpsertImporterNotification("   ");

            // Assert
            Assert.False(result.Item1);
            Assert.Contains("service bus message is null or empty", result.Item2);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Error,
                    nameof(ProcessINSASBMessage),
                    It.Is<string>(m => m.Contains("service bus message is null or empty"))),
                Times.Once);
        }

        /// <summary>
        /// Tests that UpsertImporterNotification returns false and logs an error when the message has no identifier.
        /// </summary>
        [Fact]
        public void UpsertImporterNotification_WithMissingIdentifier_ReturnsFalseAndLogsError()
        {
            // Arrange
            var message = @"{ ""aggregateVersion"": 1, ""data"": { ""exchangedDocument"": {} } }";

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.False(result.Item1);
            Assert.Contains("data.exchangedDocument.identifier", result.Item2);

            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Error,
                    nameof(ProcessINSASBMessage),
                    It.Is<string>(m => m.Contains("data.exchangedDocument.identifier"))),
                Times.Once);
        }

        /// <summary>
        /// Tests that UpsertImporterNotification creates a new record when no existing record is found and the status is SUBMITTED.
        /// </summary>
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

        /// <summary>
        /// Tests that UpsertImporterNotification creates a new record when no existing record is found and the status is AMEND.
        /// </summary>
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

        /// <summary>
        /// Tests that UpsertImporterNotification does not create a new record when no existing record is found and the status is DRAFT, and returns false with a draft message.
        /// </summary>
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

        /// <summary>
        /// Tests that UpsertImporterNotification creates a new record when no existing record is found and the status is DELETED.
        /// </summary>
        [Fact]
        public void UpsertImporterNotification_WhenNoExistingRecordAndStatusDeleted_CreatesRecordAndReturnsTrueWithSuccessMessage()
        {
            // Arrange
            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection());

            this.orgSvcMock
                .Setup(o => o.Create(It.IsAny<Entity>()))
                .Returns(Guid.NewGuid());

            var message = BuildMessage("INS-004", 1, "DELETED");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.True(result.Item1);
            Assert.Contains("created successfully", result.Item2);

            this.orgSvcMock.Verify(
                o => o.Create(It.Is<Entity>(e => e.LogicalName == defraimp_ImporterNotification.EntityLogicalName)),
                Times.Once);
        }

        /// <summary>
        /// Tests that UpsertImporterNotification returns false and logs an error when the notification status code is unsupported.
        /// </summary>
        [Fact]
        public void UpsertImporterNotification_WhenUnsupportedStatusCode_ReturnsFalseAndLogsError()
        {
            // Arrange
            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection());

            var message = BuildMessage("INS-005", 1, "UNKNOWN_STATUS");

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

        /// <summary>
        /// Tests that UpsertImporterNotification updates an existing record when the incoming version is higher than the existing version, and returns true with a success message.
        /// </summary>
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

        /// <summary>
        /// Tests that UpsertImporterNotification does not update an existing record when the incoming version is equal to the existing version, and returns false with a no update message.
        /// </summary>
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

        /// <summary>
        /// Tests that UpsertImporterNotification does not update an existing record when the incoming version is lower than the existing version, and returns false with a no update message.
        /// </summary>
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

        /// <summary>
        /// Tests that UpsertImporterNotification updates an existing record with no aggregate version when the incoming status change date is newer than the existing last updated date.
        /// </summary>
        [Fact]
        public void UpsertImporterNotification_WhenExistingRecordHasNoAggregateVersionAndIncomingDateIsNewer_UpdatesRecordAndReturnsTrueWithSuccessMessage()
        {
            // Arrange
            var existingRecord = new defraimp_ImporterNotification
            {
                Id = Guid.NewGuid(),
                defraimp_Name = "INS-020",
                defraimp_lastupdated = new DateTime(2024, 1, 1, 10, 0, 0),
            };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { existingRecord }));

            var message = BuildMessageWithStatusChanges("INS-020", 1, "SUBMITTED", "2024-06-01 12:00:00");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.True(result.Item1);
            Assert.Contains("INS-020", result.Item2);
            Assert.Contains("updated successfully", result.Item2);

            this.orgSvcMock.Verify(
                o => o.Update(It.Is<Entity>(e => e.LogicalName == defraimp_ImporterNotification.EntityLogicalName)),
                Times.Once);
        }

        /// <summary>
        /// Tests that UpsertImporterNotification does not update an existing record with no aggregate version when the incoming status change date is older than the existing last updated date.
        /// </summary>
        [Fact]
        public void UpsertImporterNotification_WhenExistingRecordHasNoAggregateVersionAndIncomingDateIsOlder_DoesNotUpdateAndReturnsFalseWithNoUpdateMessage()
        {
            // Arrange
            var existingRecord = new defraimp_ImporterNotification
            {
                Id = Guid.NewGuid(),
                defraimp_Name = "INS-021",
                defraimp_lastupdated = new DateTime(2025, 1, 1, 10, 0, 0),
            };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { existingRecord }));

            var message = BuildMessageWithStatusChanges("INS-021", 1, "SUBMITTED", "2024-01-01 08:00:00");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.False(result.Item1);
            Assert.Contains("No update needed", result.Item2);
            Assert.Contains("last updated date", result.Item2);

            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
            this.orgSvcMock.Verify(o => o.Create(It.IsAny<Entity>()), Times.Never);
        }

        /// <summary>
        /// Tests that UpsertImporterNotification does not update an existing record with no aggregate version when there are no status changes in the incoming message.
        /// </summary>
        [Fact]
        public void UpsertImporterNotification_WhenExistingRecordHasNoAggregateVersionAndNoStatusChanges_DoesNotUpdateAndReturnsFalseWithNoUpdateMessage()
        {
            // Arrange
            var existingRecord = new defraimp_ImporterNotification
            {
                Id = Guid.NewGuid(),
                defraimp_Name = "INS-022",
                defraimp_lastupdated = new DateTime(2024, 1, 1, 10, 0, 0),
            };

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { existingRecord }));

            var message = BuildMessage("INS-022", 1, "SUBMITTED");

            // Act
            var result = this.sut.UpsertImporterNotification(message);

            // Assert
            Assert.False(result.Item1);
            Assert.Contains("No update needed", result.Item2);
            Assert.Contains("no status change found", result.Item2);

            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
            this.orgSvcMock.Verify(o => o.Create(It.IsAny<Entity>()), Times.Never);
        }

        /// <summary>
        /// Tests that UpsertImporterNotification returns false and logs an error when the organization service throws an exception during retrieval.
        /// </summary>
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

        /// <summary>
        /// Tests that FormatAddress returns a single comma-delimited string when all parts are provided.
        /// </summary>
        [Fact]
        public void FormatAddress_WithAllParts_ReturnsSingleCommaDelimitedString()
        {
            // Act
            var result = this.sut.FormatAddress("1 High Street", "Flat 2", "London", "SW1A 1AA");

            // Assert
            Assert.Equal("1 High Street, Flat 2, London, SW1A 1AA", result);
        }

        /// <summary>
        /// Tests that FormatAddress excludes null or empty parts from the resulting string.
        /// </summary>
        [Fact]
        public void FormatAddress_WithNullAndEmptyParts_ExcludesBlankParts()
        {
            // Act
            var result = this.sut.FormatAddress("1 High Street", string.Empty, "London", null);

            // Assert
            Assert.Equal("1 High Street, London", result);
        }

        /// <summary>
        /// Tests that FormatAddress returns an empty string when all parts are null.
        /// </summary>
        [Fact]
        public void FormatAddress_WithAllNullParts_ReturnsEmptyString()
        {
            // Act
            var result = this.sut.FormatAddress(null, null, null, null);

            // Assert
            Assert.Equal(string.Empty, result);
        }

        /// <summary>
        /// Tests that IsSameAddress returns true when both addresses are null.
        /// </summary>
        [Fact]
        public void IsSameAddress_BothNull_ReturnsTrue()
        {
            // Act
            var result = this.sut.IsSameAddress(null, null);

            // Assert
            Assert.True(result);
        }

        /// <summary>
        /// Tests that IsSameAddress returns false when the first address is null and the second is populated.
        /// </summary>
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

        /// <summary>
        /// Tests that IsSameAddress returns false when the second address is null and the first is populated.
        /// </summary>
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

        /// <summary>
        /// Tests that IsSameAddress returns true when both addresses are identical in all fields.
        /// </summary>
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

        /// <summary>
        /// Tests that IsSameAddress returns false when the addresses have different postcodes.
        /// </summary>
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

        /// <summary>
        /// Tests that IsSameAddress returns false when the addresses have different countries.
        /// </summary>
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

        /// <summary>
        /// Tests that IsSameAddress returns true when the addresses are identical but have different casing in their fields.
        /// </summary>
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

        // ── AddIfNotNull / CollectCountryCodesFromInsObject / BuildCountryQuery / BuildCountryDictionary ──

        /// <summary>
        /// Tests that GetCountriesFromInsObject returns an empty dictionary when no country codes are present in the message.
        /// </summary>
        [Fact]
        public void GetCountriesFromInsObject_WithNoCountryCodes_ReturnsEmptyDictionary()
        {
            // Arrange — message has no postal addresses, so no country codes are collected (AddIfNotNull skips nulls)
            var insObject = BuildInsObject("INS-100", 1, "SUBMITTED");

            // Act
            var result = this.sut.GetCountriesFromInsObject(insObject);

            // Assert
            Assert.Empty(result);
            this.orgSvcMock.Verify(
                o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defra_country.EntityLogicalName)),
                Times.Never);
        }

        /// <summary>
        /// Tests that GetCountriesFromInsObject builds a query and returns a dictionary keyed by ISO code
        /// when country codes are present (exercises BuildCountryQuery and BuildCountryDictionary).
        /// </summary>
        [Fact]
        public void GetCountriesFromInsObject_WithCountryCodes_QueriesDataverseAndReturnsDictionary()
        {
            // Arrange — provide a country code via the issuer postal address
            var gbCountry = new defra_country { defra_isocodealpha2 = "GB" };
            gbCountry.Id = Guid.NewGuid();

            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defra_country.EntityLogicalName)))
                .Returns(new EntityCollection(new List<Entity> { gbCountry }));

            var insObject = BuildInsObjectWithIssuerCountry("INS-101", 1, "SUBMITTED", "GB");

            // Act
            var result = this.sut.GetCountriesFromInsObject(insObject);

            // Assert
            Assert.True(result.ContainsKey("GB"));
            Assert.Equal(gbCountry.Id, result["GB"].Id);

            this.orgSvcMock.Verify(
                o => o.RetrieveMultiple(It.Is<QueryExpression>(qe =>
                    qe.EntityName == defra_country.EntityLogicalName &&
                    qe.Criteria.Conditions.Count > 0)),
                Times.Once);
        }

        /// <summary>
        /// Tests that GetCountriesFromInsObject logs a warning for each country code not found in Dataverse
        /// (exercises the missing-country loop in BuildCountryDictionary).
        /// </summary>
        [Fact]
        public void GetCountriesFromInsObject_WithMissingCountry_LogsWarning()
        {
            // Arrange — Dataverse returns nothing for the requested country
            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defra_country.EntityLogicalName)))
                .Returns(new EntityCollection());

            var insObject = BuildInsObjectWithIssuerCountry("INS-102", 1, "SUBMITTED", "ZZ");

            // Act
            this.sut.GetCountriesFromInsObject(insObject);

            // Assert
            this.loggerMock.Verify(
                l => l.Log(
                    Severity.Warning,
                    nameof(ProcessINSASBMessage),
                    It.Is<string>(m => m.Contains("ZZ"))),
                Times.Once);
        }

        // ── ApplyIssuerDetails ────────────────────────────────────────────────

        /// <summary>
        /// Tests that ApplyIssuerDetails maps company name, formatted address, and contact details
        /// onto the notification when an issuer with a postal address and contact is present.
        /// </summary>
        [Fact]
        public void ApplyIssuerDetails_WithFullIssuer_MapsAllFieldsCorrectly()
        {
            // Arrange
            var message = BuildMessageWithIssuer("INS-200", 1, "SUBMITTED",
                issuerName: "Responsible Co",
                line1: "10 Test St", line2: "Floor 2", city: "London", postcode: "SW1A 1AA",
                country: "GB", personName: "Jane Doe", email: "jane@example.com", phone: "01234 567890");

            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Equal("Responsible Co", created.defraimp_personresponsiblecompanyname);
            Assert.Equal("10 Test St, Floor 2, London, SW1A 1AA", created.defraimp_personresponsibleaddress);
            Assert.Equal("Jane Doe", created.defraimp_personresponsiblename);
            Assert.Equal("jane@example.com", created.defraimp_personresponsibleemail);
            Assert.Equal("01234 567890", created.defraimp_personresponsiblephone);
        }

        /// <summary>
        /// Tests that ApplyIssuerDetails does not set any person-responsible fields when the issuer is null.
        /// </summary>
        [Fact]
        public void ApplyIssuerDetails_WithNullIssuer_DoesNotSetAnyFields()
        {
            // Arrange — message has no issuer
            var created = CaptureCreatedEntity(BuildMessage("INS-201", 1, "SUBMITTED"));

            // Assert
            Assert.Null(created.defraimp_personresponsiblecompanyname);
            Assert.Null(created.defraimp_personresponsiblename);
            Assert.Null(created.defraimp_personresponsibleemail);
        }

        // ── ApplyOriginDetails ────────────────────────────────────────────────

        /// <summary>
        /// Tests that ApplyOriginDetails sets the region of origin from subordinate country subdivision.
        /// </summary>
        [Fact]
        public void ApplyOriginDetails_WithOriginCountryAndSubDivision_SetsRegionOfOrigin()
        {
            // Arrange
            var message = BuildMessageWithOriginCountry("INS-210", 1, "SUBMITTED", countryCode: "FR", regionIdentifier: "BRE");
            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Equal("BRE", created.defraimp_commoditiesregionoforigin);
        }

        /// <summary>
        /// Tests that ApplyOriginDetails does not set region of origin when no subdivision is present.
        /// </summary>
        [Fact]
        public void ApplyOriginDetails_WithNoSubDivision_DoesNotSetRegionOfOrigin()
        {
            // Arrange
            var message = BuildMessageWithOriginCountry("INS-211", 1, "SUBMITTED", countryCode: "FR", regionIdentifier: null);
            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Null(created.defraimp_commoditiesregionoforigin);
        }

        // ── ApplyConsigneeDetails ─────────────────────────────────────────────

        /// <summary>
        /// Tests that ApplyConsigneeDetails maps all consignee fields when a full consignee party is present.
        /// </summary>
        [Fact]
        public void ApplyConsigneeDetails_WithFullConsignee_MapsAllFields()
        {
            // Arrange
            var message = BuildMessageWithParty("INS-220", "consigneeParty",
                name: "Buyer Co", line1: "1 Buy St", line2: "Apt 3", city: "Bristol", postcode: "BS1 1AA",
                country: "GB", email: "buyer@example.com", phone: "01234 000001");

            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Equal("Buyer Co", created.defraimp_consigneecompanyname);
            Assert.Equal("1 Buy St", created.defraimp_consigneeaddressaddressline1);
            Assert.Equal("Apt 3", created.defraimp_consigneeaddressaddressline2);
            Assert.Equal("Bristol", created.defraimp_consigneeaddresscity);
            Assert.Equal("BS1 1AA", created.defraimp_consigneeaddresspostalzipcode);
            Assert.Equal("buyer@example.com", created.defraimp_consigneeaddressemail);
            Assert.Equal("01234 000001", created.defraimp_consigneeaddresstelephone);
        }

        /// <summary>
        /// Tests that ApplyConsigneeDetails does not set any fields when ConsigneeParty is absent.
        /// </summary>
        [Fact]
        public void ApplyConsigneeDetails_WithNullConsignee_DoesNotSetAnyFields()
        {
            // Arrange — message has specifiedConsignment but no consigneeParty
            var message = BuildMessageWithEmptyConsignment("INS-221", 1, "SUBMITTED");
            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Null(created.defraimp_consigneecompanyname);
            Assert.Null(created.defraimp_consigneeaddressemail);
        }

        // ── ApplyImporterDetails ──────────────────────────────────────────────

        /// <summary>
        /// Tests that ApplyImporterDetails maps all importer fields when a full importer party is present.
        /// </summary>
        [Fact]
        public void ApplyImporterDetails_WithFullImporter_MapsAllFields()
        {
            // Arrange
            var message = BuildMessageWithParty("INS-230", "importer",
                name: "Importer Co", line1: "2 Import Rd", line2: null, city: "Leeds", postcode: "LS1 1BB",
                country: "GB", email: "imp@example.com", phone: "01234 000002");

            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Equal("Importer Co", created.defraimp_importercompanyname);
            Assert.Equal("2 Import Rd", created.defraimp_importeraddressaddressline1);
            Assert.Equal("Leeds", created.defraimp_importeraddresscity);
            Assert.Equal("LS1 1BB", created.defraimp_importeraddresspostalzipcode);
            Assert.Equal("imp@example.com", created.defraimp_importeraddressemail);
        }

        /// <summary>
        /// Tests that ApplyImporterDetails does not set any fields when Importer is absent.
        /// </summary>
        [Fact]
        public void ApplyImporterDetails_WithNullImporter_DoesNotSetAnyFields()
        {
            // Arrange
            var message = BuildMessageWithEmptyConsignment("INS-231", 1, "SUBMITTED");
            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Null(created.defraimp_importercompanyname);
        }

        // ── ApplyConsignorDetails ─────────────────────────────────────────────

        /// <summary>
        /// Tests that ApplyConsignorDetails maps all consignor fields when a full consignor party is present.
        /// </summary>
        [Fact]
        public void ApplyConsignorDetails_WithFullConsignor_MapsAllFields()
        {
            // Arrange
            var message = BuildMessageWithParty("INS-240", "consignorParty",
                name: "Seller Co", line1: "3 Sell Ave", line2: null, city: "Paris", postcode: "75001",
                country: "GB", email: "seller@example.com", phone: "01234 000003");

            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Equal("Seller Co", created.defraimp_consignorcompanyname);
            Assert.Equal("3 Sell Ave", created.defraimp_consignoraddressaddressline1);
            Assert.Equal("Paris", created.defraimp_consignoraddresscity);
            Assert.Equal("seller@example.com", created.defraimp_consignoraddressemail);
        }

        /// <summary>
        /// Tests that ApplyConsignorDetails does not set any fields when ConsignorParty is absent.
        /// </summary>
        [Fact]
        public void ApplyConsignorDetails_WithNullConsignor_DoesNotSetAnyFields()
        {
            // Arrange
            var message = BuildMessageWithEmptyConsignment("INS-241", 1, "SUBMITTED");
            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Null(created.defraimp_consignorcompanyname);
        }

        // ── ApplyPlaceOfOriginDetails ─────────────────────────────────────────

        /// <summary>
        /// Tests that ApplyPlaceOfOriginDetails maps all place-of-origin fields when a despatch party is present.
        /// </summary>
        [Fact]
        public void ApplyPlaceOfOriginDetails_WithFullDespatchParty_MapsAllFields()
        {
            // Arrange
            var message = BuildMessageWithParty("INS-250", "despatchParty",
                name: "Origin Farm", line1: "4 Farm Ln", line2: null, city: "Lyon", postcode: "69001",
                country: "GB", email: "farm@example.com", phone: "01234 000004");

            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Equal("Origin Farm", created.defraimp_PlaceofOriginCompanyName);
            Assert.Equal("4 Farm Ln", created.defraimp_PlaceofOriginAddressLine1);
            Assert.Equal("Lyon", created.defraimp_PlaceofOriginCity);
            Assert.Equal("farm@example.com", created.defraimp_PlaceofOriginEmail);
        }

        /// <summary>
        /// Tests that ApplyPlaceOfOriginDetails does not set any fields when DespatchParty is absent.
        /// </summary>
        [Fact]
        public void ApplyPlaceOfOriginDetails_WithNullDespatchParty_DoesNotSetAnyFields()
        {
            // Arrange
            var message = BuildMessageWithEmptyConsignment("INS-251", 1, "SUBMITTED");
            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Null(created.defraimp_PlaceofOriginCompanyName);
        }

        // ── ApplyPlaceOfDestinationDetails ────────────────────────────────────

        /// <summary>
        /// Tests that ApplyPlaceOfDestinationDetails maps all place-of-destination fields when a delivery party is present.
        /// </summary>
        [Fact]
        public void ApplyPlaceOfDestinationDetails_WithFullDeliveryParty_MapsAllFields()
        {
            // Arrange
            var message = BuildMessageWithParty("INS-260", "deliveryParty",
                name: "Dest Farm", line1: "5 Dest Rd", line2: null, city: "Leeds", postcode: "LS1 1CC",
                country: "GB", email: "dest@example.com", phone: "01234 000005");

            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Equal("Dest Farm", created.defraimp_placeofdestinationcompanyname);
            Assert.Equal("5 Dest Rd", created.defraimp_placeofdestinationaddressaddressline1);
            Assert.Equal("Leeds", created.defraimp_placeofdestinationaddresscity);
            Assert.Equal("dest@example.com", created.defraimp_placeofdestinationaddressemail);
        }

        /// <summary>
        /// Tests that ApplyPlaceOfDestinationDetails does not set any fields when DeliveryParty is absent.
        /// </summary>
        [Fact]
        public void ApplyPlaceOfDestinationDetails_WithNullDeliveryParty_DoesNotSetAnyFields()
        {
            // Arrange
            var message = BuildMessageWithEmptyConsignment("INS-261", 1, "SUBMITTED");
            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Null(created.defraimp_placeofdestinationcompanyname);
        }

        // ── ResolvePermanentAddressFlag ────────────────────────────────────────

        /// <summary>
        /// Tests that ResolvePermanentAddressFlag returns true when the delivery party address
        /// matches the permanent location address on the product instance.
        /// </summary>
        [Fact]
        public void ResolvePermanentAddressFlag_WhenAddressesMatch_SetsFieldToTrue()
        {
            // Arrange
            var message = BuildMessageWithPermanentAddress("INS-270",
                deliveryAddress: new[] { "5 Dest Rd", "Leeds", "LS1 1CC", "GB" },
                permanentAddress: new[] { "5 Dest Rd", "Leeds", "LS1 1CC", "GB" });

            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.True(created.defraimp_isplaceofdestinationthepermanentaddress);
        }

        /// <summary>
        /// Tests that ResolvePermanentAddressFlag returns false when the delivery party address
        /// does not match the permanent location address on the product instance.
        /// </summary>
        [Fact]
        public void ResolvePermanentAddressFlag_WhenAddressesDiffer_SetsFieldToFalse()
        {
            // Arrange
            var message = BuildMessageWithPermanentAddress("INS-271",
                deliveryAddress: new[] { "5 Dest Rd", "Leeds", "LS1 1CC", "GB" },
                permanentAddress: new[] { "99 Other St", "York", "YO1 1XX", "GB" });

            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.False(created.defraimp_isplaceofdestinationthepermanentaddress);
        }

        /// <summary>
        /// Tests that ResolvePermanentAddressFlag returns null when there are no consignment items.
        /// </summary>
        [Fact]
        public void ResolvePermanentAddressFlag_WithNoConsignmentItems_SetsFieldToNull()
        {
            // Arrange — delivery party present but no includedConsignmentItem
            var message = BuildMessageWithParty("INS-272", "deliveryParty",
                name: "Dest Farm", line1: "5 Dest Rd", line2: null, city: "Leeds", postcode: "LS1 1CC",
                country: "GB", email: "dest@example.com", phone: "01234 000005");

            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Null(created.defraimp_isplaceofdestinationthepermanentaddress);
        }

        // ── ApplyTransporterDetails ───────────────────────────────────────────

        /// <summary>
        /// Tests that ApplyTransporterDetails maps all transporter fields including party type code.
        /// </summary>
        [Fact]
        public void ApplyTransporterDetails_WithFullCarrier_MapsAllFields()
        {
            // Arrange
            var message = BuildMessageWithCarrier("INS-280",
                name: "FastFreight", carrierIdentifier: "FF-99",
                line1: "6 Carrier Way", city: "Dover", postcode: "CT16 1AA",
                country: "GB", partyTypeCode: "CT1");

            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Equal("FastFreight", created.defraimp_transportercompanyname);
            Assert.Equal("FF-99", created.defraimp_transporterapprovalnumber);
            Assert.Equal("6 Carrier Way", created.defraimp_transporteraddressaddressline1);
            Assert.Equal("Dover", created.defraimp_transporteraddresscity);
            Assert.Equal("CT16 1AA", created.defraimp_transporteraddresspostalzipcode);
            Assert.Equal("CT1", created.defraimp_transportertype);
        }

        /// <summary>
        /// Tests that ApplyTransporterDetails does not set any fields when Carrier is absent.
        /// </summary>
        [Fact]
        public void ApplyTransporterDetails_WithNullCarrier_DoesNotSetAnyFields()
        {
            // Arrange
            var message = BuildMessageWithEmptyConsignment("INS-281", 1, "SUBMITTED");
            var created = CaptureCreatedEntity(message);

            // Assert
            Assert.Null(created.defraimp_transportercompanyname);
            Assert.Null(created.defraimp_transporterapprovalnumber);
        }

        // ── ResolveMeansOfTransportType / ApplyTransportMovementDetails ────────

        /// <summary>
        /// Tests that ApplyTransportMovementDetails maps modeCode 1 to "Ship".
        /// </summary>
        [Fact]
        public void ApplyTransportMovementDetails_WithModeCode1_SetsTransportTypeToShip()
        {
            var created = CaptureCreatedEntity(BuildMessageWithTransport("INS-290", modeCode: 1, arrivalDate: "2024-09-01 06:00:00", transportId: "V1", documentId: "BL-1"));

            Assert.Equal("Ship", created.defraimp_MeansofTransporttoEntryPointType);
        }

        /// <summary>
        /// Tests that ApplyTransportMovementDetails maps modeCode 2 to "Railway Wagon".
        /// </summary>
        [Fact]
        public void ApplyTransportMovementDetails_WithModeCode2_SetsTransportTypeToRailwayWagon()
        {
            var created = CaptureCreatedEntity(BuildMessageWithTransport("INS-291", modeCode: 2, arrivalDate: "2024-09-01 06:00:00", transportId: "T1", documentId: null));

            Assert.Equal("Railway Wagon", created.defraimp_MeansofTransporttoEntryPointType);
        }

        /// <summary>
        /// Tests that ApplyTransportMovementDetails maps modeCode 3 to "Road Vehicle".
        /// </summary>
        [Fact]
        public void ApplyTransportMovementDetails_WithModeCode3_SetsTransportTypeToRoadVehicle()
        {
            var created = CaptureCreatedEntity(BuildMessageWithTransport("INS-292", modeCode: 3, arrivalDate: "2024-09-01 06:00:00", transportId: "R1", documentId: null));

            Assert.Equal("Road Vehicle", created.defraimp_MeansofTransporttoEntryPointType);
        }

        /// <summary>
        /// Tests that ApplyTransportMovementDetails maps modeCode 4 to "Aeroplane".
        /// </summary>
        [Fact]
        public void ApplyTransportMovementDetails_WithModeCode4_SetsTransportTypeToAeroplane()
        {
            var created = CaptureCreatedEntity(BuildMessageWithTransport("INS-293", modeCode: 4, arrivalDate: "2024-09-01 06:00:00", transportId: "A1", documentId: null));

            Assert.Equal("Aeroplane", created.defraimp_MeansofTransporttoEntryPointType);
        }

        /// <summary>
        /// Tests that ApplyTransportMovementDetails sets transport type to null for an unknown modeCode.
        /// </summary>
        [Fact]
        public void ApplyTransportMovementDetails_WithUnknownModeCode_SetsTransportTypeToNull()
        {
            var created = CaptureCreatedEntity(BuildMessageWithTransport("INS-294", modeCode: 99, arrivalDate: "2024-09-01 06:00:00", transportId: "X1", documentId: null));

            Assert.Null(created.defraimp_MeansofTransporttoEntryPointType);
        }

        /// <summary>
        /// Tests that ApplyTransportMovementDetails sets the arrival date, transport ID, and document.
        /// </summary>
        [Fact]
        public void ApplyTransportMovementDetails_WithFullTransportMovement_MapsAllFields()
        {
            // Arrange
            var created = CaptureCreatedEntity(BuildMessageWithTransport("INS-295", modeCode: 1, arrivalDate: "2024-09-01 06:00:00", transportId: "VESSEL-01", documentId: "BL-001"));

            // Assert
            Assert.Equal(new DateTime(2024, 9, 1, 6, 0, 0), created.defraimp_ArrivalDate);
            Assert.Equal("VESSEL-01", created.defraimp_MeansofTransporttoEntryPointId);
            Assert.Equal("BL-001", created.defraimp_MeansofTransporttoEntryPointDocument);
        }

        /// <summary>
        /// Tests that ApplyTransportMovementDetails sets arrival date to null when the date string is invalid.
        /// </summary>
        [Fact]
        public void ApplyTransportMovementDetails_WithInvalidArrivalDate_SetsArrivalDateToNull()
        {
            var created = CaptureCreatedEntity(BuildMessageWithTransport("INS-296", modeCode: 1, arrivalDate: "not-a-date", transportId: "V1", documentId: null));

            Assert.Null(created.defraimp_ArrivalDate);
        }

        /// <summary>
        /// Tests that ApplyTransportMovementDetails does not set any transport fields when no transport movement is present.
        /// </summary>
        [Fact]
        public void ApplyTransportMovementDetails_WithNoTransportMovement_DoesNotSetAnyFields()
        {
            var message = BuildMessageWithEmptyConsignment("INS-297", 1, "SUBMITTED");
            var created = CaptureCreatedEntity(message);

            Assert.Null(created.defraimp_MeansofTransporttoEntryPointType);
            Assert.Null(created.defraimp_MeansofTransporttoEntryPointId);
            Assert.Null(created.defraimp_ArrivalDate);
        }

        // ── Private helpers ───────────────────────────────────────────────────

        private defraimp_ImporterNotification CaptureCreatedEntity(string message)
        {
            this.orgSvcMock
                .Setup(o => o.RetrieveMultiple(It.Is<QueryExpression>(qe => qe.EntityName == defraimp_ImporterNotification.EntityLogicalName)))
                .Returns(new EntityCollection());

            defraimp_ImporterNotification created = null;
            this.orgSvcMock
                .Setup(o => o.Create(It.IsAny<Entity>()))
                .Callback<Entity>(e => created = (defraimp_ImporterNotification)e)
                .Returns(Guid.NewGuid());

            this.sut.UpsertImporterNotification(message);

            Assert.NotNull(created);
            return created;
        }

        private static INSObject BuildInsObject(string identifier, int aggregateVersion, string statusCode)
        {
            return BuildMessage(identifier, aggregateVersion, statusCode).FromJSON<INSObject>();
        }

        private static INSObject BuildInsObjectWithIssuerCountry(string identifier, int aggregateVersion, string statusCode, string countryId)
        {
            return BuildMessageWithIssuer(identifier, aggregateVersion, statusCode,
                issuerName: "Test Co", line1: "1 St", line2: null, city: "City", postcode: "AA1 1AA",
                personName: null, email: null, phone: null,
                country: countryId).FromJSON<INSObject>();
        }

        private static string BuildMessageWithIssuer(string identifier, int aggregateVersion, string statusCode,
            string issuerName, string line1, string line2, string city, string postcode,
            string personName, string email, string phone, string country)
        {
            var contact = (personName != null || email != null || phone != null)
                ? $@", ""definedContact"": [{{ ""personName"": {ToJsonValue(personName)}, ""emailURIUniversalCommunication"": {ToJsonValue(email)}, ""telephoneUniversalCommunication"": {ToJsonValue(phone)} }}]"
                : string.Empty;

            return $@"{{
              ""aggregateVersion"": {aggregateVersion},
              ""data"": {{
                ""exchangedDocument"": {{
                  ""identifier"": ""{identifier}"",
                  ""notificationStatusCode"": ""{statusCode}"",
                  ""versionId"": 1,
                  ""issuer"": {{
                    ""name"": {ToJsonValue(issuerName)},
                    ""postalAddress"": {{
                      ""lineOne"": {ToJsonValue(line1)},
                      ""lineTwo"": {ToJsonValue(line2)},
                      ""cityName"": {ToJsonValue(city)},
                      ""postcodeCode"": {ToJsonValue(postcode)},
                      ""countryId"": {ToJsonValue(country)}
                    }}{contact}
                  }}
                }}
              }}
            }}";
        }

        private static string BuildMessageWithOriginCountry(string identifier, int aggregateVersion, string statusCode, string countryCode, string regionIdentifier)
        {
            var subDivision = regionIdentifier != null
                ? $@"""subordinateTradeCountrySubDivision"": [{{ ""identifier"": ""{regionIdentifier}"" }}],"
                : string.Empty;

            return $@"{{
              ""aggregateVersion"": {aggregateVersion},
              ""data"": {{
                ""exchangedDocument"": {{ ""identifier"": ""{identifier}"", ""notificationStatusCode"": ""{statusCode}"", ""versionId"": 1 }},
                ""specifiedConsignment"": {{
                  ""originCountry"": {{
                    {subDivision}
                    ""code"": {{ ""value"": ""{countryCode}"" }}
                  }}
                }}
              }}
            }}";
        }

        private static string BuildMessageWithParty(string identifier, string partyProperty,
            string name, string line1, string line2, string city, string postcode, string country,
            string email, string phone)
        {
            return $@"{{
              ""aggregateVersion"": 1,
              ""data"": {{
                ""exchangedDocument"": {{ ""identifier"": ""{identifier}"", ""notificationStatusCode"": ""SUBMITTED"", ""versionId"": 1 }},
                ""specifiedConsignment"": {{
                  ""{partyProperty}"": {{
                    ""name"": {ToJsonValue(name)},
                    ""postalAddress"": {{
                      ""lineOne"": {ToJsonValue(line1)},
                      ""lineTwo"": {ToJsonValue(line2)},
                      ""cityName"": {ToJsonValue(city)},
                      ""postcodeCode"": {ToJsonValue(postcode)},
                      ""countryId"": {ToJsonValue(country)}
                    }},
                    ""definedContact"": [{{
                      ""emailURIUniversalCommunication"": {ToJsonValue(email)},
                      ""telephoneUniversalCommunication"": {ToJsonValue(phone)}
                    }}]
                  }}
                }}
              }}
            }}";
        }

        private static string BuildMessageWithCarrier(string identifier, string name, string carrierIdentifier,
            string line1, string city, string postcode, string country, string partyTypeCode)
        {
            return $@"{{
              ""aggregateVersion"": 1,
              ""data"": {{
                ""exchangedDocument"": {{ ""identifier"": ""{identifier}"", ""notificationStatusCode"": ""SUBMITTED"", ""versionId"": 1 }},
                ""specifiedConsignment"": {{
                  ""carrier"": {{
                    ""name"": {ToJsonValue(name)},
                    ""identifier"": {ToJsonValue(carrierIdentifier)},
                    ""postalAddress"": {{
                      ""lineOne"": {ToJsonValue(line1)},
                      ""cityName"": {ToJsonValue(city)},
                      ""postcodeCode"": {ToJsonValue(postcode)},
                      ""countryId"": {ToJsonValue(country)}
                    }},
                    ""partyTypeCode"": [{{ ""value"": {ToJsonValue(partyTypeCode)} }}]
                  }}
                }}
              }}
            }}";
        }

        private static string BuildMessageWithTransport(string identifier, int modeCode, string arrivalDate, string transportId, string documentId)
        {
            var document = documentId != null
                ? $@"""transportContractRelatedReferencedDocument"": [{{ ""identifier"": ""{documentId}"" }}],"
                : string.Empty;

            return $@"{{
              ""aggregateVersion"": 1,
              ""data"": {{
                ""exchangedDocument"": {{ ""identifier"": ""{identifier}"", ""notificationStatusCode"": ""SUBMITTED"", ""versionId"": 1 }},
                ""specifiedConsignment"": {{
                  ""mainCarriageLogisticsTransportMovement"": [{{
                    ""identifier"": ""{transportId}"",
                    ""modeCode"": {modeCode},
                    {document}
                    ""arrivalEvent"": [{{ ""scheduledOccurrenceDateTime"": ""{arrivalDate}"" }}]
                  }}]
                }}
              }}
            }}";
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

        private static string BuildMessageWithEmptyConsignment(string identifier, int aggregateVersion, string statusCode)
        {
            return $@"{{
              ""aggregateVersion"": {aggregateVersion},
              ""data"": {{
                ""exchangedDocument"": {{ ""identifier"": ""{identifier}"", ""notificationStatusCode"": ""{statusCode}"", ""versionId"": 1 }},
                ""specifiedConsignment"": {{}}
              }}
            }}";
        }

        private static string BuildMessageWithPermanentAddress(string identifier, string[] deliveryAddress, string[] permanentAddress)
        {
            return $@"{{
              ""aggregateVersion"": 1,
              ""data"": {{
                ""exchangedDocument"": {{ ""identifier"": ""{identifier}"", ""notificationStatusCode"": ""SUBMITTED"", ""versionId"": 1 }},
                ""specifiedConsignment"": {{
                  ""deliveryParty"": {{
                    ""name"": ""Dest Farm"",
                    ""postalAddress"": {{
                      ""lineOne"": ""{deliveryAddress[0]}"",
                      ""cityName"": ""{deliveryAddress[1]}"",
                      ""postcodeCode"": ""{deliveryAddress[2]}"",
                      ""countryId"": ""{deliveryAddress[3]}""
                    }}
                  }},
                  ""includedConsignmentItem"": [{{
                    ""includedTradeLineItem"": [{{
                      ""individualTradeProductInstance"": [{{
                        ""permanentLocation"": {{
                          ""postalAddress"": {{
                            ""lineOne"": ""{permanentAddress[0]}"",
                            ""cityName"": ""{permanentAddress[1]}"",
                            ""postcodeCode"": ""{permanentAddress[2]}"",
                            ""countryId"": ""{permanentAddress[3]}""
                          }}
                        }}
                      }}]
                    }}]
                  }}]
                }}
              }}
            }}";
        }

        private static string BuildMessageWithStatusChanges(string identifier, int aggregateVersion, string notificationStatusCode, string statusChangeDateChanged)
        {
            return $@"{{
              ""aggregateVersion"": {aggregateVersion},
              ""data"": {{
                ""exchangedDocument"": {{
                  ""identifier"": ""{identifier}"",
                  ""notificationStatusCode"": ""{notificationStatusCode}"",
                  ""versionId"": 1
                }}
              }},
              ""statusChanges"": [
                {{
                  ""status"": ""{notificationStatusCode}"",
                  ""dateChanged"": ""{statusChangeDateChanged}""
                }}
              ]
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