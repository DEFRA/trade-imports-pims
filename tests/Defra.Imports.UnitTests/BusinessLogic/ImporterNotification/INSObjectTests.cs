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

        // ?? PostalAddress ?????????????????????????????????????????????????????

        /// <summary>
        /// Tests that PostalAddress deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void PostalAddress_WithAllProperties_DeserializesCorrectly()
        {
            var json = @"{
                ""lineOne"": ""1 High Street"",
                ""lineTwo"": ""Flat 2"",
                ""cityName"": ""London"",
                ""postcodeCode"": ""SW1A 1AA"",
                ""countryId"": ""GB"",
                ""countryName"": ""United Kingdom"",
                ""countrySubDivisionName"": ""England""
            }";

            var result = json.FromJSON<PostalAddress>();

            Assert.Equal("1 High Street", result.LineOne);
            Assert.Equal("Flat 2", result.LineTwo);
            Assert.Equal("London", result.CityName);
            Assert.Equal("SW1A 1AA", result.PostcodeCode);
            Assert.Equal("GB", result.CountryId);
            Assert.Equal("United Kingdom", result.CountryName);
            Assert.Equal("England", result.CountrySubDivisionName);
        }

        /// <summary>
        /// Tests that PostalAddress optional properties are null when absent.
        /// </summary>
        [Fact]
        public void PostalAddress_WithEmptyJson_AllPropertiesAreNull()
        {
            var result = "{}".FromJSON<PostalAddress>();

            Assert.Null(result.LineOne);
            Assert.Null(result.LineTwo);
            Assert.Null(result.CityName);
            Assert.Null(result.PostcodeCode);
            Assert.Null(result.CountryId);
            Assert.Null(result.CountryName);
            Assert.Null(result.CountrySubDivisionName);
        }

        // ?? DefinedContact ????????????????????????????????????????????????????

        /// <summary>
        /// Tests that DefinedContact deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void DefinedContact_WithAllProperties_DeserializesCorrectly()
        {
            var json = @"{
                ""personName"": ""Jane Doe"",
                ""emailURIUniversalCommunication"": ""jane@example.com"",
                ""telephoneUniversalCommunication"": ""+44 7700 900000""
            }";

            var result = json.FromJSON<DefinedContact>();

            Assert.Equal("Jane Doe", result.PersonName);
            Assert.Equal("jane@example.com", result.EmailURIUniversalCommunication);
            Assert.Equal("+44 7700 900000", result.TelephoneUniversalCommunication);
        }

        /// <summary>
        /// Tests that DefinedContact properties are null when absent.
        /// </summary>
        [Fact]
        public void DefinedContact_WithEmptyJson_AllPropertiesAreNull()
        {
            var result = "{}".FromJSON<DefinedContact>();

            Assert.Null(result.PersonName);
            Assert.Null(result.EmailURIUniversalCommunication);
            Assert.Null(result.TelephoneUniversalCommunication);
        }

        // ?? CodedValue ????????????????????????????????????????????????????????

        /// <summary>
        /// Tests that CodedValue deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void CodedValue_WithAllProperties_DeserializesCorrectly()
        {
            var json = @"{ ""value"": ""CT1"", ""urlId"": ""url-1"", ""name"": ""Commercial"" }";

            var result = json.FromJSON<CodedValue>();

            Assert.Equal("CT1", result.Value);
            Assert.Equal("url-1", result.UrlId);
            Assert.Equal("Commercial", result.Name);
        }

        // ?? CountryCode ???????????????????????????????????????????????????????

        /// <summary>
        /// Tests that CountryCode deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void CountryCode_WithAllProperties_DeserializesCorrectly()
        {
            var json = @"{ ""value"": ""GB"", ""urlId"": ""url-gb"", ""name"": ""United Kingdom"" }";

            var result = json.FromJSON<CountryCode>();

            Assert.Equal("GB", result.Value);
            Assert.Equal("url-gb", result.UrlId);
            Assert.Equal("United Kingdom", result.Name);
        }

        // ?? Party ?????????????????????????????????????????????????????????????

        /// <summary>
        /// Tests that Party deserializes all scalar properties correctly.
        /// </summary>
        [Fact]
        public void Party_WithAllScalarProperties_DeserializesCorrectly()
        {
            var json = @"{
                ""identifier"": ""p-1"",
                ""urlId"": ""url-p1"",
                ""name"": ""ACME Ltd""
            }";

            var result = json.FromJSON<Party>();

            Assert.Equal("p-1", result.Identifier);
            Assert.Equal("url-p1", result.UrlId);
            Assert.Equal("ACME Ltd", result.Name);
        }

        /// <summary>
        /// Tests that Party deserializes a PostalAddress correctly.
        /// </summary>
        [Fact]
        public void Party_WithPostalAddress_DeserializesCorrectly()
        {
            var json = @"{
                ""name"": ""ACME Ltd"",
                ""postalAddress"": {
                    ""lineOne"": ""10 Street"",
                    ""cityName"": ""Bristol"",
                    ""postcodeCode"": ""BS1 1AA"",
                    ""countryId"": ""GB""
                }
            }";

            var result = json.FromJSON<Party>();

            Assert.NotNull(result.PostalAddress);
            Assert.Equal("10 Street", result.PostalAddress.LineOne);
            Assert.Equal("Bristol", result.PostalAddress.CityName);
            Assert.Equal("BS1 1AA", result.PostalAddress.PostcodeCode);
            Assert.Equal("GB", result.PostalAddress.CountryId);
        }

        /// <summary>
        /// Tests that Party deserializes DefinedContact entries correctly.
        /// </summary>
        [Fact]
        public void Party_WithDefinedContacts_DeserializesCorrectly()
        {
            var json = @"{
                ""name"": ""ACME Ltd"",
                ""definedContact"": [
                    {
                        ""personName"": ""Bob"",
                        ""emailURIUniversalCommunication"": ""bob@acme.com"",
                        ""telephoneUniversalCommunication"": ""01234 567890""
                    }
                ]
            }";

            var result = json.FromJSON<Party>();

            Assert.NotNull(result.DefinedContact);
            Assert.Single(result.DefinedContact);
            Assert.Equal("Bob", result.DefinedContact[0].PersonName);
            Assert.Equal("bob@acme.com", result.DefinedContact[0].EmailURIUniversalCommunication);
            Assert.Equal("01234 567890", result.DefinedContact[0].TelephoneUniversalCommunication);
        }

        /// <summary>
        /// Tests that Party deserializes PartyTypeCode entries correctly.
        /// </summary>
        [Fact]
        public void Party_WithPartyTypeCodes_DeserializesCorrectly()
        {
            var json = @"{
                ""name"": ""ACME Ltd"",
                ""partyRoleCode"": { ""value"": ""AG"" },
                ""partyTypeCode"": [
                    { ""value"": ""CT1"", ""name"": ""Commercial"" }
                ]
            }";

            var result = json.FromJSON<Party>();

            Assert.NotNull(result.PartyRoleCode);
            Assert.Equal("AG", result.PartyRoleCode.Value);
            Assert.NotNull(result.PartyTypeCode);
            Assert.Single(result.PartyTypeCode);
            Assert.Equal("CT1", result.PartyTypeCode[0].Value);
            Assert.Equal("Commercial", result.PartyTypeCode[0].Name);
        }

        /// <summary>
        /// Tests that Party optional properties are null when absent.
        /// </summary>
        [Fact]
        public void Party_WithEmptyJson_OptionalPropertiesAreNull()
        {
            var result = "{}".FromJSON<Party>();

            Assert.Null(result.Identifier);
            Assert.Null(result.UrlId);
            Assert.Null(result.Name);
            Assert.Null(result.PostalAddress);
            Assert.Null(result.DefinedContact);
            Assert.Null(result.PartyRoleCode);
            Assert.Null(result.PartyTypeCode);
        }

        // ?? Carrier ???????????????????????????????????????????????????????????

        /// <summary>
        /// Tests that Carrier deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void Carrier_WithAllProperties_DeserializesCorrectly()
        {
            var json = @"{
                ""name"": ""FastFreight"",
                ""identifier"": ""FF-99"",
                ""urlId"": ""url-ff"",
                ""partyRoleCode"": { ""value"": ""CA"" },
                ""partyTypeCode"": [ { ""value"": ""CT2"" } ],
                ""postalAddress"": { ""lineOne"": ""5 Dock Road"", ""countryId"": ""GB"" }
            }";

            var result = json.FromJSON<Carrier>();

            Assert.Equal("FastFreight", result.Name);
            Assert.Equal("FF-99", result.Identifier);
            Assert.Equal("url-ff", result.UrlId);
            Assert.NotNull(result.PartyRoleCode);
            Assert.Equal("CA", result.PartyRoleCode.Value);
            Assert.NotNull(result.PartyTypeCode);
            Assert.Single(result.PartyTypeCode);
            Assert.Equal("CT2", result.PartyTypeCode[0].Value);
            Assert.NotNull(result.PostalAddress);
            Assert.Equal("5 Dock Road", result.PostalAddress.LineOne);
        }

        /// <summary>
        /// Tests that Carrier optional properties are null when absent.
        /// </summary>
        [Fact]
        public void Carrier_WithEmptyJson_OptionalPropertiesAreNull()
        {
            var result = "{}".FromJSON<Carrier>();

            Assert.Null(result.Name);
            Assert.Null(result.Identifier);
            Assert.Null(result.UrlId);
            Assert.Null(result.PartyRoleCode);
            Assert.Null(result.PartyTypeCode);
            Assert.Null(result.PostalAddress);
        }

        // ?? TradeCountry ??????????????????????????????????????????????????????

        /// <summary>
        /// Tests that TradeCountry deserializes its Code and sub-divisions correctly.
        /// </summary>
        [Fact]
        public void TradeCountry_WithCodeAndSubDivisions_DeserializesCorrectly()
        {
            var json = @"{
                ""code"": { ""value"": ""GB"", ""urlId"": ""url-gb"", ""name"": ""United Kingdom"" },
                ""subordinateTradeCountrySubDivision"": [
                    {
                        ""identifier"": ""ENG"",
                        ""functionTypeCode"": { ""content"": ""region"" }
                    }
                ]
            }";

            var result = json.FromJSON<TradeCountry>();

            Assert.NotNull(result.Code);
            Assert.Equal("GB", result.Code.Value);
            Assert.Equal("url-gb", result.Code.UrlId);
            Assert.Equal("United Kingdom", result.Code.Name);
            Assert.NotNull(result.SubordinateTradeCountrySubDivision);
            Assert.Single(result.SubordinateTradeCountrySubDivision);
            Assert.Equal("ENG", result.SubordinateTradeCountrySubDivision[0].Identifier);
            Assert.NotNull(result.SubordinateTradeCountrySubDivision[0].FunctionTypeCode);
            Assert.Equal("region", result.SubordinateTradeCountrySubDivision[0].FunctionTypeCode.Content);
        }

        /// <summary>
        /// Tests that TradeCountry optional properties are null when absent.
        /// </summary>
        [Fact]
        public void TradeCountry_WithEmptyJson_OptionalPropertiesAreNull()
        {
            var result = "{}".FromJSON<TradeCountry>();

            Assert.Null(result.Code);
            Assert.Null(result.SubordinateTradeCountrySubDivision);
        }

        // ?? FinalDestinationLocation ??????????????????????????????????????????

        /// <summary>
        /// Tests that FinalDestinationLocation deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void FinalDestinationLocation_WithAllProperties_DeserializesCorrectly()
        {
            var json = @"{
                ""identifier"": ""CPH-001"",
                ""urlId"": ""url-cph"",
                ""name"": ""Farm A"",
                ""postalAddress"": { ""lineOne"": ""Farm Lane"", ""countryId"": ""GB"" }
            }";

            var result = json.FromJSON<FinalDestinationLocation>();

            Assert.Equal("CPH-001", result.Identifier);
            Assert.Equal("url-cph", result.UrlId);
            Assert.Equal("Farm A", result.Name);
            Assert.NotNull(result.PostalAddress);
            Assert.Equal("Farm Lane", result.PostalAddress.LineOne);
        }

        // ?? LogisticsLocation ?????????????????????????????????????????????????

        /// <summary>
        /// Tests that LogisticsLocation deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void LogisticsLocation_WithAllProperties_DeserializesCorrectly()
        {
            var json = @"{
                ""identifier"": ""PORT-LHR"",
                ""urlId"": ""url-lhr"",
                ""name"": ""Heathrow"",
                ""typeCode"": ""AIRPORT"",
                ""postalAddress"": { ""cityName"": ""London"", ""countryId"": ""GB"" }
            }";

            var result = json.FromJSON<LogisticsLocation>();

            Assert.Equal("PORT-LHR", result.Identifier);
            Assert.Equal("url-lhr", result.UrlId);
            Assert.Equal("Heathrow", result.Name);
            Assert.Equal("AIRPORT", result.TypeCode);
            Assert.NotNull(result.PostalAddress);
            Assert.Equal("London", result.PostalAddress.CityName);
        }

        // ?? TransportContractRelatedReferencedDocument ????????????????????????

        /// <summary>
        /// Tests that TransportContractRelatedReferencedDocument deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void TransportContractRelatedReferencedDocument_WithAllProperties_DeserializesCorrectly()
        {
            var json = @"{ ""typeCode"": ""AWB"", ""identifier"": ""DOC-XYZ"" }";

            var result = json.FromJSON<TransportContractRelatedReferencedDocument>();

            Assert.Equal("AWB", result.TypeCode);
            Assert.Equal("DOC-XYZ", result.Identifier);
        }

        // ?? TransportEvent ????????????????????????????????????????????????????

        /// <summary>
        /// Tests that TransportEvent deserializes all properties correctly including nested LogisticsLocation.
        /// </summary>
        [Fact]
        public void TransportEvent_WithAllProperties_DeserializesCorrectly()
        {
            var json = @"{
                ""scheduledOccurrenceDateTime"": ""2024-07-01 06:00:00"",
                ""actualOccurrenceDateTime"": ""2024-07-01 06:30:00"",
                ""occurrenceLogisticsLocation"": {
                    ""identifier"": ""PORT-DVR"",
                    ""name"": ""Dover""
                }
            }";

            var result = json.FromJSON<TransportEvent>();

            Assert.Equal("2024-07-01 06:00:00", result.ScheduledOccurrenceDateTime);
            Assert.Equal("2024-07-01 06:30:00", result.ActualOccurrenceDateTime);
            Assert.NotNull(result.OccurrenceLogisticsLocation);
            Assert.Equal("PORT-DVR", result.OccurrenceLogisticsLocation.Identifier);
            Assert.Equal("Dover", result.OccurrenceLogisticsLocation.Name);
        }

        /// <summary>
        /// Tests that TransportEvent optional properties are null when absent.
        /// </summary>
        [Fact]
        public void TransportEvent_WithEmptyJson_OptionalPropertiesAreNull()
        {
            var result = "{}".FromJSON<TransportEvent>();

            Assert.Null(result.ScheduledOccurrenceDateTime);
            Assert.Null(result.ActualOccurrenceDateTime);
            Assert.Null(result.OccurrenceLogisticsLocation);
        }

        // ?? MainCarriageLogisticsTransportMovement ????????????????????????????

        /// <summary>
        /// Tests that MainCarriageLogisticsTransportMovement deserializes all properties correctly.
        /// </summary>
        [Fact]
        public void MainCarriageLogisticsTransportMovement_WithAllProperties_DeserializesCorrectly()
        {
            var json = @"{
                ""identifier"": ""VESSEL-01"",
                ""modeCode"": 1,
                ""urlId"": ""url-v1"",
                ""transportContractRelatedReferencedDocument"": [
                    { ""typeCode"": ""BL"", ""identifier"": ""BL-001"" }
                ],
                ""arrivalEvent"": [
                    { ""scheduledOccurrenceDateTime"": ""2024-08-10 08:00:00"" }
                ],
                ""departureEvent"": [
                    { ""scheduledOccurrenceDateTime"": ""2024-08-05 12:00:00"" }
                ]
            }";

            var result = json.FromJSON<MainCarriageLogisticsTransportMovement>();

            Assert.Equal("VESSEL-01", result.Identifier);
            Assert.Equal(1, result.ModeCode);
            Assert.Equal("url-v1", result.UrlId);
            Assert.NotNull(result.TransportContractRelatedReferencedDocument);
            Assert.Single(result.TransportContractRelatedReferencedDocument);
            Assert.Equal("BL-001", result.TransportContractRelatedReferencedDocument[0].Identifier);
            Assert.NotNull(result.ArrivalEvent);
            Assert.Single(result.ArrivalEvent);
            Assert.Equal("2024-08-10 08:00:00", result.ArrivalEvent[0].ScheduledOccurrenceDateTime);
            Assert.NotNull(result.DepartureEvent);
            Assert.Single(result.DepartureEvent);
            Assert.Equal("2024-08-05 12:00:00", result.DepartureEvent[0].ScheduledOccurrenceDateTime);
        }

        /// <summary>
        /// Tests that MainCarriageLogisticsTransportMovement optional properties are null when absent.
        /// </summary>
        [Fact]
        public void MainCarriageLogisticsTransportMovement_WithEmptyJson_OptionalPropertiesAreNull()
        {
            var result = "{}".FromJSON<MainCarriageLogisticsTransportMovement>();

            Assert.Null(result.Identifier);
            Assert.Null(result.ModeCode);
            Assert.Null(result.UrlId);
            Assert.Null(result.TransportContractRelatedReferencedDocument);
            Assert.Null(result.ArrivalEvent);
            Assert.Null(result.DepartureEvent);
        }

        // ?? IncludedConsignmentItem ????????????????????????????????????????????

        /// <summary>
        /// Tests that IncludedConsignmentItem deserializes IncludedTradeLineItem entries correctly.
        /// </summary>
        [Fact]
        public void IncludedConsignmentItem_WithTradeLineItems_DeserializesCorrectly()
        {
            var json = @"{
                ""includedTradeLineItem"": [
                    { ""typeCode"": ""LIVE"", ""scientificName"": ""Bos taurus"" }
                ]
            }";

            var result = json.FromJSON<IncludedConsignmentItem>();

            Assert.NotNull(result.IncludedTradeLineItem);
            Assert.Single(result.IncludedTradeLineItem);
            Assert.Equal("LIVE", result.IncludedTradeLineItem[0].TypeCode);
            Assert.Equal("Bos taurus", result.IncludedTradeLineItem[0].ScientificName);
        }

        /// <summary>
        /// Tests that IncludedConsignmentItem IncludedTradeLineItem is null when absent.
        /// </summary>
        [Fact]
        public void IncludedConsignmentItem_WithEmptyJson_IncludedTradeLineItemIsNull()
        {
            var result = "{}".FromJSON<IncludedConsignmentItem>();

            Assert.Null(result.IncludedTradeLineItem);
        }

        // ?? SpecifiedConsignment ??????????????????????????????????????????????

        /// <summary>
        /// Tests that SpecifiedConsignment deserializes all party references correctly.
        /// </summary>
        [Fact]
        public void SpecifiedConsignment_WithAllParties_DeserializesCorrectly()
        {
            var json = @"{
                ""consignorParty"": { ""name"": ""Seller Co"" },
                ""consigneeParty"": { ""name"": ""Buyer Co"" },
                ""despatchParty"": { ""name"": ""Origin Farm"" },
                ""deliveryParty"": { ""name"": ""Destination Farm"" },
                ""importer"": { ""name"": ""Importer Co"" }
            }";

            var result = json.FromJSON<SpecifiedConsignment>();

            Assert.NotNull(result.ConsignorParty);
            Assert.Equal("Seller Co", result.ConsignorParty.Name);
            Assert.NotNull(result.ConsigneeParty);
            Assert.Equal("Buyer Co", result.ConsigneeParty.Name);
            Assert.NotNull(result.DespatchParty);
            Assert.Equal("Origin Farm", result.DespatchParty.Name);
            Assert.NotNull(result.DeliveryParty);
            Assert.Equal("Destination Farm", result.DeliveryParty.Name);
            Assert.NotNull(result.Importer);
            Assert.Equal("Importer Co", result.Importer.Name);
        }

        /// <summary>
        /// Tests that SpecifiedConsignment deserializes Carrier correctly.
        /// </summary>
        [Fact]
        public void SpecifiedConsignment_WithCarrier_DeserializesCorrectly()
        {
            var json = @"{
                ""carrier"": { ""name"": ""FastFreight"", ""identifier"": ""FF-01"" }
            }";

            var result = json.FromJSON<SpecifiedConsignment>();

            Assert.NotNull(result.Carrier);
            Assert.Equal("FastFreight", result.Carrier.Name);
            Assert.Equal("FF-01", result.Carrier.Identifier);
        }

        /// <summary>
        /// Tests that SpecifiedConsignment deserializes OriginCountry correctly.
        /// </summary>
        [Fact]
        public void SpecifiedConsignment_WithOriginCountry_DeserializesCorrectly()
        {
            var json = @"{
                ""originCountry"": {
                    ""code"": { ""value"": ""FR"" },
                    ""subordinateTradeCountrySubDivision"": [ { ""identifier"": ""BRE"" } ]
                }
            }";

            var result = json.FromJSON<SpecifiedConsignment>();

            Assert.NotNull(result.OriginCountry);
            Assert.Equal("FR", result.OriginCountry.Code.Value);
            Assert.Single(result.OriginCountry.SubordinateTradeCountrySubDivision);
            Assert.Equal("BRE", result.OriginCountry.SubordinateTradeCountrySubDivision[0].Identifier);
        }

        /// <summary>
        /// Tests that SpecifiedConsignment deserializes FinalDestinationLocation and UnloadingBaseportLocation correctly.
        /// </summary>
        [Fact]
        public void SpecifiedConsignment_WithLocations_DeserializesCorrectly()
        {
            var json = @"{
                ""finalDestinationLocation"": { ""identifier"": ""CPH-123"" },
                ""unloadingBaseportLocation"": { ""identifier"": ""PORT-DVR"" }
            }";

            var result = json.FromJSON<SpecifiedConsignment>();

            Assert.NotNull(result.FinalDestinationLocation);
            Assert.Equal("CPH-123", result.FinalDestinationLocation.Identifier);
            Assert.NotNull(result.UnloadingBaseportLocation);
            Assert.Equal("PORT-DVR", result.UnloadingBaseportLocation.Identifier);
        }

        /// <summary>
        /// Tests that SpecifiedConsignment deserializes MainCarriageLogisticsTransportMovement correctly.
        /// </summary>
        [Fact]
        public void SpecifiedConsignment_WithTransportMovement_DeserializesCorrectly()
        {
            var json = @"{
                ""mainCarriageLogisticsTransportMovement"": [
                    {
                        ""identifier"": ""VESSEL-99"",
                        ""modeCode"": 1,
                        ""arrivalEvent"": [ { ""scheduledOccurrenceDateTime"": ""2024-09-01 07:00:00"" } ]
                    }
                ]
            }";

            var result = json.FromJSON<SpecifiedConsignment>();

            Assert.NotNull(result.MainCarriageLogisticsTransportMovement);
            Assert.Single(result.MainCarriageLogisticsTransportMovement);
            var transport = result.MainCarriageLogisticsTransportMovement[0];
            Assert.Equal("VESSEL-99", transport.Identifier);
            Assert.Equal(1, transport.ModeCode);
            Assert.Single(transport.ArrivalEvent);
            Assert.Equal("2024-09-01 07:00:00", transport.ArrivalEvent[0].ScheduledOccurrenceDateTime);
        }

        /// <summary>
        /// Tests that SpecifiedConsignment deserializes IsOrHasUnweanedAnimals correctly.
        /// </summary>
        [Fact]
        public void SpecifiedConsignment_WithIsOrHasUnweanedAnimals_DeserializesCorrectly()
        {
            var json = @"{ ""isOrHasUnweanedAnimals"": true }";

            var result = json.FromJSON<SpecifiedConsignment>();

            Assert.True(result.IsOrHasUnweanedAnimals);
        }

        /// <summary>
        /// Tests that SpecifiedConsignment deserializes IncludedConsignmentItem entries correctly.
        /// </summary>
        [Fact]
        public void SpecifiedConsignment_WithIncludedConsignmentItems_DeserializesCorrectly()
        {
            var json = @"{
                ""includedConsignmentItem"": [
                    {
                        ""includedTradeLineItem"": [
                            { ""typeCode"": ""LIVE"", ""scientificName"": ""Bos taurus"" }
                        ]
                    }
                ]
            }";

            var result = json.FromJSON<SpecifiedConsignment>();

            Assert.NotNull(result.IncludedConsignmentItem);
            Assert.Single(result.IncludedConsignmentItem);
            Assert.Single(result.IncludedConsignmentItem[0].IncludedTradeLineItem);
            Assert.Equal("LIVE", result.IncludedConsignmentItem[0].IncludedTradeLineItem[0].TypeCode);
        }

        /// <summary>
        /// Tests that all SpecifiedConsignment optional properties are null when absent.
        /// </summary>
        [Fact]
        public void SpecifiedConsignment_WithEmptyJson_AllPropertiesAreNull()
        {
            var result = "{}".FromJSON<SpecifiedConsignment>();

            Assert.Null(result.ConsignorParty);
            Assert.Null(result.ConsigneeParty);
            Assert.Null(result.DespatchParty);
            Assert.Null(result.DeliveryParty);
            Assert.Null(result.Importer);
            Assert.Null(result.Carrier);
            Assert.Null(result.OriginCountry);
            Assert.Null(result.FinalDestinationLocation);
            Assert.Null(result.UnloadingBaseportLocation);
            Assert.Null(result.MainCarriageLogisticsTransportMovement);
            Assert.Null(result.IsOrHasUnweanedAnimals);
            Assert.Null(result.TransitTradeCountry);
            Assert.Null(result.IncludedConsignmentItem);
        }
    }
}