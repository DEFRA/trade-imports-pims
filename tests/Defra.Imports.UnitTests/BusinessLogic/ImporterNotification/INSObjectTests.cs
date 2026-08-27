namespace Defra.Imports.UnitTests.BusinessLogic.ImporterNotification
{
    using Defra.Imports.BusinessLogic.Extensions;
    using Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="INSObject"/> and its related DataContract classes.
    /// </summary>
    public class INSObjectTests
    {
        // ?? INSObject ??????????????????????????????????????????????????????????

        /// <summary>
        /// Tests that all top-level scalar properties on INSObject deserialize correctly.
        /// </summary>
        [Fact]
        public void INSObject_WithAllTopLevelProperties_DeserializesCorrectly()
        {
            // Arrange
            var json = @"{
                ""eventId"": ""evt-1"",
                ""aggregateId"": ""agg-1"",
                ""aggregateType"": ""ImporterNotification"",
                ""subType"": ""CHEDA"",
                ""aggregateVersion"": 3,
                ""eventType"": ""UPDATED"",
                ""timestamp"": ""2024-01-15T10:30:00Z""
            }";

            // Act
            var result = json.FromJSON<INSObject>();

            // Assert
            Assert.Equal("evt-1", result.EventId);
            Assert.Equal("agg-1", result.AggregateId);
            Assert.Equal("ImporterNotification", result.AggregateType);
            Assert.Equal("CHEDA", result.SubType);
            Assert.Equal(3, result.AggregateVersion);
            Assert.Equal("UPDATED", result.EventType);
            Assert.Equal("2024-01-15T10:30:00Z", result.Timestamp);
        }

        /// <summary>
        /// Tests that the Metadata property on INSObject deserializes correctly.
        /// </summary>
        [Fact]
        public void INSObject_WithMetadata_DeserializesCorrectly()
        {
            // Arrange
            var json = @"{
                ""aggregateVersion"": 1,
                ""metadata"": {
                    ""correlationId"": ""corr-123"",
                    ""schemaVersion"": ""1.0"",
                    ""schemaUrl"": ""https://schema.example.com/v1""
                }
            }";

            // Act
            var result = json.FromJSON<INSObject>();

            // Assert
            Assert.NotNull(result.Metadata);
            Assert.Equal("corr-123", result.Metadata.CorrelationId);
            Assert.Equal("1.0", result.Metadata.SchemaVersion);
            Assert.Equal("https://schema.example.com/v1", result.Metadata.SchemaUrl);
        }

        /// <summary>
        /// Tests that the top-level Actor property on INSObject deserializes correctly.
        /// </summary>
        [Fact]
        public void INSObject_WithActor_DeserializesCorrectly()
        {
            // Arrange
            var json = @"{
                ""aggregateVersion"": 1,
                ""actor"": {
                    ""id"": ""user-42"",
                    ""source"": ""portal"",
                    ""userType"": ""IMPORTER"",
                    ""displayName"": ""Jane Smith"",
                    ""organisationId"": ""org-99""
                }
            }";

            // Act
            var result = json.FromJSON<INSObject>();

            // Assert
            Assert.NotNull(result.Actor);
            Assert.Equal("user-42", result.Actor.Id);
            Assert.Equal("portal", result.Actor.Source);
            Assert.Equal("IMPORTER", result.Actor.UserType);
            Assert.Equal("Jane Smith", result.Actor.DisplayName);
            Assert.Equal("org-99", result.Actor.OrganisationId);
        }

        /// <summary>
        /// Tests that the StatusChanges array on INSObject deserializes correctly with multiple entries.
        /// </summary>
        [Fact]
        public void INSObject_WithMultipleStatusChanges_DeserializesCorrectly()
        {
            // Arrange
            var json = @"{
                ""aggregateVersion"": 1,
                ""statusChanges"": [
                    {
                        ""status"": ""SUBMITTED"",
                        ""dateChanged"": ""2024-01-10 09:00:00"",
                        ""actor"": { ""displayName"": ""Alice"" }
                    },
                    {
                        ""status"": ""AMEND"",
                        ""dateChanged"": ""2024-01-12 14:30:00"",
                        ""actor"": { ""displayName"": ""Bob"" }
                    }
                ]
            }";

            // Act
            var result = json.FromJSON<INSObject>();

            // Assert
            Assert.NotNull(result.StatusChanges);
            Assert.Equal(2, result.StatusChanges.Length);
            Assert.Equal("SUBMITTED", result.StatusChanges[0].Status);
            Assert.Equal("2024-01-10 09:00:00", result.StatusChanges[0].DateChanged);
            Assert.Equal("Alice", result.StatusChanges[0].Actor.DisplayName);
            Assert.Equal("AMEND", result.StatusChanges[1].Status);
            Assert.Equal("Bob", result.StatusChanges[1].Actor.DisplayName);
        }

        /// <summary>
        /// Tests that StatusChanges is null when the property is absent from the JSON.
        /// </summary>
        [Fact]
        public void INSObject_WithNoStatusChanges_StatusChangesIsNull()
        {
            // Arrange
            var json = @"{ ""aggregateVersion"": 1 }";

            // Act
            var result = json.FromJSON<INSObject>();

            // Assert
            Assert.Null(result.StatusChanges);
        }

        /// <summary>
        /// Tests that the Data property on INSObject deserializes correctly.
        /// </summary>
        [Fact]
        public void INSObject_WithData_DeserializesCorrectly()
        {
            // Arrange
            var json = @"{
                ""aggregateVersion"": 1,
                ""data"": {
                    ""$model"": ""model-v1"",
                    ""$type"": ""GBN-AG"",
                    ""exchangedDocument"": {
                        ""identifier"": ""INS-123""
                    }
                }
            }";

            // Act
            var result = json.FromJSON<INSObject>();

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal("model-v1", result.Data.Model);
            Assert.Equal("GBN-AG", result.Data.Type);
            Assert.NotNull(result.Data.ExchangedDocument);
            Assert.Equal("INS-123", result.Data.ExchangedDocument.Identifier);
        }

        // ?? Metadata ??????????????????????????????????????????????????????????

        /// <summary>
        /// Tests that Metadata deserializes all properties correctly when provided directly.
        /// </summary>
        [Fact]
        public void Metadata_WithAllProperties_DeserializesCorrectly()
        {
            // Arrange
            var json = @"{
                ""correlationId"": ""c-001"",
                ""schemaVersion"": ""2.0"",
                ""schemaUrl"": ""https://example.com/schema""
            }";

            // Act
            var result = json.FromJSON<Metadata>();

            // Assert
            Assert.Equal("c-001", result.CorrelationId);
            Assert.Equal("2.0", result.SchemaVersion);
            Assert.Equal("https://example.com/schema", result.SchemaUrl);
        }

        /// <summary>
        /// Tests that Metadata properties are null when the JSON object is empty.
        /// </summary>
        [Fact]
        public void Metadata_WithEmptyJson_AllPropertiesAreNull()
        {
            // Act
            var result = "{}".FromJSON<Metadata>();

            // Assert
            Assert.Null(result.CorrelationId);
            Assert.Null(result.SchemaVersion);
            Assert.Null(result.SchemaUrl);
        }

        // ?? Actor ?????????????????????????????????????????????????????????????

        /// <summary>
        /// Tests that Actor deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void Actor_WithAllProperties_DeserializesCorrectly()
        {
            // Arrange
            var json = @"{
                ""id"": ""u-1"",
                ""source"": ""internal"",
                ""userType"": ""ADMIN"",
                ""displayName"": ""John Doe"",
                ""organisationId"": ""org-5""
            }";

            // Act
            var result = json.FromJSON<Actor>();

            // Assert
            Assert.Equal("u-1", result.Id);
            Assert.Equal("internal", result.Source);
            Assert.Equal("ADMIN", result.UserType);
            Assert.Equal("John Doe", result.DisplayName);
            Assert.Equal("org-5", result.OrganisationId);
        }

        /// <summary>
        /// Tests that Actor properties are null when the JSON object is empty.
        /// </summary>
        [Fact]
        public void Actor_WithEmptyJson_AllPropertiesAreNull()
        {
            // Act
            var result = "{}".FromJSON<Actor>();

            // Assert
            Assert.Null(result.Id);
            Assert.Null(result.Source);
            Assert.Null(result.UserType);
            Assert.Null(result.DisplayName);
            Assert.Null(result.OrganisationId);
        }

        // ?? StatusChange ??????????????????????????????????????????????????????

        /// <summary>
        /// Tests that StatusChange deserializes all properties correctly including a nested Actor.
        /// </summary>
        [Fact]
        public void StatusChange_WithAllProperties_DeserializesCorrectly()
        {
            // Arrange
            var json = @"{
                ""status"": ""SUBMITTED"",
                ""dateChanged"": ""2024-03-01 08:00:00"",
                ""actor"": {
                    ""id"": ""u-7"",
                    ""displayName"": ""Carol""
                }
            }";

            // Act
            var result = json.FromJSON<StatusChange>();

            // Assert
            Assert.Equal("SUBMITTED", result.Status);
            Assert.Equal("2024-03-01 08:00:00", result.DateChanged);
            Assert.NotNull(result.Actor);
            Assert.Equal("u-7", result.Actor.Id);
            Assert.Equal("Carol", result.Actor.DisplayName);
        }

        /// <summary>
        /// Tests that Actor on StatusChange is null when not present in the JSON.
        /// </summary>
        [Fact]
        public void StatusChange_WithNoActor_ActorIsNull()
        {
            // Arrange
            var json = @"{ ""status"": ""DRAFT"", ""dateChanged"": ""2024-01-01 00:00:00"" }";

            // Act
            var result = json.FromJSON<StatusChange>();

            // Assert
            Assert.Equal("DRAFT", result.Status);
            Assert.Null(result.Actor);
        }

        // ?? Data ??????????????????????????????????????????????????????????????

        /// <summary>
        /// Tests that Data deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void Data_WithAllProperties_DeserializesCorrectly()
        {
            // Arrange
            var json = @"{
                ""$model"": ""ins-model"",
                ""$type"": ""CHEDA"",
                ""exchangedDocument"": { ""identifier"": ""DOC-1"" },
                ""specifiedConsignment"": {}
            }";

            // Act
            var result = json.FromJSON<Data>();

            // Assert
            Assert.Equal("ins-model", result.Model);
            Assert.Equal("CHEDA", result.Type);
            Assert.NotNull(result.ExchangedDocument);
            Assert.Equal("DOC-1", result.ExchangedDocument.Identifier);
            Assert.NotNull(result.SpecifiedConsignment);
        }

        /// <summary>
        /// Tests that ExchangedDocument and SpecifiedConsignment are null when absent.
        /// </summary>
        [Fact]
        public void Data_WithNoNestedObjects_NestedPropertiesAreNull()
        {
            // Act
            var result = "{}".FromJSON<Data>();

            // Assert
            Assert.Null(result.Model);
            Assert.Null(result.Type);
            Assert.Null(result.ExchangedDocument);
            Assert.Null(result.SpecifiedConsignment);
        }

        // ?? ExchangedDocument ?????????????????????????????????????????????????

        /// <summary>
        /// Tests that ExchangedDocument deserializes all scalar properties correctly.
        /// </summary>
        [Fact]
        public void ExchangedDocument_WithAllScalarProperties_DeserializesCorrectly()
        {
            // Arrange
            var json = @"{
                ""identifier"": ""INS-999"",
                ""traderAssignedId"": ""TR-001"",
                ""notificationStatusCode"": ""SUBMITTED"",
                ""versionId"": 4,
                ""issueDateTime"": ""2024-06-01T12:00:00Z""
            }";

            // Act
            var result = json.FromJSON<ExchangedDocument>();

            // Assert
            Assert.Equal("INS-999", result.Identifier);
            Assert.Equal("TR-001", result.TraderAssignedId);
            Assert.Equal("SUBMITTED", result.NotificationStatusCode);
            Assert.Equal(4, result.VersionId);
            Assert.Equal("2024-06-01T12:00:00Z", result.IssueDateTime);
        }

        /// <summary>
        /// Tests that ExchangedDocument optional properties are null when absent.
        /// </summary>
        [Fact]
        public void ExchangedDocument_WithNoOptionalProperties_OptionalPropertiesAreNull()
        {
            // Arrange
            var json = @"{ ""identifier"": ""INS-001"", ""notificationStatusCode"": ""DRAFT"" }";

            // Act
            var result = json.FromJSON<ExchangedDocument>();

            // Assert
            Assert.Null(result.TraderAssignedId);
            Assert.Null(result.VersionId);
            Assert.Null(result.IssueDateTime);
            Assert.Null(result.Issuer);
            Assert.Null(result.FirstSignatoryAuthentication);
            Assert.Null(result.ReferenceDocument);
        }
    }
}