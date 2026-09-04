namespace Defra.Imports.BusinessLogic.ImporterNotification
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Defra.Imports.BusinessLogic.Extensions;
    using Defra.Imports.BusinessLogic.ImporterNotification.JsonFormatterClassObjects.INSObject;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>
    /// Class to handle the create or update of the importer notification and related records based on input.
    /// </summary>
    public class ProcessINSASBMessage
    {
        private readonly IOrganizationService orgSvc;
        private readonly ILogWriter logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessINSASBMessage"/> class.
        /// </summary>
        /// <param name="orgSvc">Org service.</param>
        /// <param name="logger">Logger.</param>
        public ProcessINSASBMessage(IOrganizationService orgSvc, ILogWriter logger)
        {
            this.orgSvc = orgSvc;
            this.logger = logger;
        }

        /// <summary>
        /// Method to process the service bus message.
        /// </summary>
        /// <param name="message">Service bus message.</param>
        /// <returns>Returns a tuple with success status (bool) and response message (string).</returns>
        public Tuple<bool, string> UpsertImporterNotification(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                var errorMessage = "Error processing Importer Notification - service bus message is null or empty";
                this.logger.Log(Severity.Error, nameof(ProcessINSASBMessage), errorMessage);
                return Tuple.Create(false, errorMessage);
            }

            if (!this.TryDeserializeMessage(message, out var insObject, out var deserializeError))
            {
                return Tuple.Create(false, deserializeError);
            }

            if (string.IsNullOrWhiteSpace(insObject?.Data?.ExchangedDocument?.Identifier))
            {
                var errorMessage = "Error processing Importer Notification - message does not contain data.exchangedDocument.identifier";
                this.logger.Log(Severity.Error, nameof(ProcessINSASBMessage), errorMessage);
                return Tuple.Create(false, errorMessage);
            }

            try
            {
                var existingNotification = this.FindExistingNotification(insObject.Data.ExchangedDocument.Identifier);

                return existingNotification != null
                    ? this.TryUpdateExisting(existingNotification, insObject)
                    : this.TryCreateNew(insObject);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error processing Importer Notification: {ex.Message}";
                this.logger.Log(Severity.Error, nameof(ProcessINSASBMessage), errorMessage);
                return Tuple.Create(false, errorMessage);
            }
        }

        /// <summary>
        /// Populates the importer notification fields based on the INSObject data.
        /// </summary>
        /// <param name="importerNotification">Importer notification to populate.</param>
        /// <param name="insObject">Data object to populate from.</param>
        /// <param name="isUpdate">Indicates whether the operation is an update.</param>
        /// <returns>The populated importer notification entity.</returns>
        public defraimp_ImporterNotification PopulateImporterNotificationFields(defraimp_ImporterNotification importerNotification, INSObject insObject, bool isUpdate)
        {
            var countries = this.GetCountriesFromInsObject(insObject);

            importerNotification.defraimp_AggregateVersion = insObject.AggregateVersion;
            importerNotification.defraimp_type = this.ResolveNotificationType(insObject.Data?.Type);

            this.ApplySubmissionDetails(importerNotification, insObject);

            if (isUpdate)
            {
                this.ApplyLastUpdatedDetails(importerNotification, insObject);
            }

            if (insObject.Data?.ExchangedDocument != null)
            {
                importerNotification = this.PopulateExchangedDocumentInformation(importerNotification, insObject, isUpdate, countries);
            }

            if (insObject.Data?.SpecifiedConsignment != null)
            {
                importerNotification = this.PopulateConsignmentDetails(importerNotification, insObject, countries);
            }

            return importerNotification;
        }

        /// <summary>
        /// Populating the exchanged document information from the INSObject into the importer notification entity.
        /// </summary>
        /// <param name="importerNotification">Importer notification to be updated.</param>
        /// <param name="insObject">INS object with the information to update.</param>
        /// <param name="isUpdate">Is the importer notification being updated or created.</param>
        /// <param name="countries">Dictionary of country codes to defra_country entities.</param>
        /// <returns>Updated importer notification entity.</returns>
        public defraimp_ImporterNotification PopulateExchangedDocumentInformation(defraimp_ImporterNotification importerNotification, INSObject insObject, bool isUpdate, Dictionary<string, defra_country> countries)
        {
            var document = insObject.Data.ExchangedDocument;

            importerNotification.defraimp_Name = document.Identifier;
            importerNotification.defraimp_TraderReference = document.TraderAssignedId;
            importerNotification.defraimp_Version = document.VersionId;
            importerNotification.defraimp_status = this.ResolveNotificationStatus(document.NotificationStatusCode);

            this.ApplyIssuerDetails(importerNotification, document.Issuer, countries);

            return importerNotification;
        }

        /// <summary>
        /// Populate Consignment level information from the INSObject into the importer notification entity.
        /// </summary>
        /// <param name="importerNotification">Importer notification to be populated.</param>
        /// <param name="insObject">INSObject containing consignment details.</param>
        /// <param name="countries">Dictionary of country codes to defra_country entities.</param>
        /// <returns>Populated importer notification entity.</returns>
        public defraimp_ImporterNotification PopulateConsignmentDetails(defraimp_ImporterNotification importerNotification, INSObject insObject, Dictionary<string, defra_country> countries)
        {
            var consignment = insObject.Data.SpecifiedConsignment;

            this.ApplyOriginDetails(importerNotification, consignment, countries);
            this.ApplyConsigneeDetails(importerNotification, consignment.ConsigneeParty, countries);
            this.ApplyImporterDetails(importerNotification, consignment.Importer, countries);
            this.ApplyConsignorDetails(importerNotification, consignment.ConsignorParty, countries);
            this.ApplyPlaceOfOriginDetails(importerNotification, consignment.DespatchParty, countries);
            this.ApplyPlaceOfDestinationDetails(importerNotification, consignment, countries);
            this.ApplyTransporterDetails(importerNotification, consignment.Carrier, countries);
            this.ApplyTransportMovementDetails(importerNotification, consignment);

            if (consignment.FinalDestinationLocation != null)
            {
                importerNotification.defraimp_cphnumber = consignment.FinalDestinationLocation.Identifier;
            }

            if (consignment.UnloadingBaseportLocation != null)
            {
                importerNotification.defraimp_portofentry = consignment.UnloadingBaseportLocation.Identifier;
            }

            return importerNotification;
        }

        /// <summary>
        /// Formats an address into a single line string.
        /// </summary>
        /// <param name="line1">Address line 1.</param>
        /// <param name="line2">Address line 2.</param>
        /// <param name="city">City name.</param>
        /// <param name="postcode">Postcode.</param>
        /// <returns>Formatted address string.</returns>
        public string FormatAddress(string line1, string line2, string city, string postcode)
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(line1))
            {
                parts.Add(line1);
            }

            if (!string.IsNullOrWhiteSpace(line2))
            {
                parts.Add(line2);
            }

            if (!string.IsNullOrWhiteSpace(city))
            {
                parts.Add(city);
            }

            if (!string.IsNullOrWhiteSpace(postcode))
            {
                parts.Add(postcode);
            }

            return string.Join(", ", parts);
        }

        /// <summary>
        /// Compares two postal addresses to determine if they are the same.
        /// </summary>
        /// <param name="address1">First postal address.</param>
        /// <param name="address2">Second postal address.</param>
        /// <returns>True if addresses match, false otherwise.</returns>
        public bool IsSameAddress(PostalAddress address1, PostalAddress address2)
        {
            if (address1 == null && address2 == null)
            {
                return true;
            }

            if (address1 == null || address2 == null)
            {
                return false;
            }

            return string.Equals(address1.LineOne, address2.LineOne, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(address1.LineTwo, address2.LineTwo, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(address1.CityName, address2.CityName, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(address1.PostcodeCode, address2.PostcodeCode, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(address1.CountryId, address2.CountryId, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Retrieves country records from Dataverse based on country codes in the INSObject.
        /// </summary>
        /// <param name="insObject">The INS object containing country codes.</param>
        /// <returns>Dictionary of country code to defra_country entity.</returns>
        public Dictionary<string, defra_country> GetCountriesFromInsObject(INSObject insObject)
        {
            var countryCodes = this.CollectCountryCodesFromInsObject(insObject);

            if (countryCodes.Count == 0)
            {
                return new Dictionary<string, defra_country>(StringComparer.OrdinalIgnoreCase);
            }

            var query = this.BuildCountryQuery(countryCodes);
            var results = this.orgSvc.RetrieveMultiple(query);

            return this.BuildCountryDictionary(countryCodes, results);
        }

        private static StatusChange GetStatusChange(INSObject insObject, Func<StatusChange, bool> predicate, bool ascending)
        {
            if (insObject.StatusChanges == null || insObject.StatusChanges.Length == 0)
            {
                return null;
            }

            var ordered = ascending
                ? insObject.StatusChanges.OrderBy(sc => sc.DateChanged)
                : insObject.StatusChanges.OrderByDescending(sc => sc.DateChanged);

            return ordered.FirstOrDefault(predicate);
        }

        private Tuple<bool, string> TryUpdateExisting(defraimp_ImporterNotification existing, INSObject insObject)
        {
            var identifier = insObject.Data.ExchangedDocument.Identifier;

            if (existing.defraimp_AggregateVersion < insObject.AggregateVersion)
            {
                this.PopulateImporterNotificationFields(existing, insObject, true);
                this.orgSvc.Update(existing);
                this.DeleteExistingConsignmentItems(existing);
                this.ApplyConsignmentItemDetails(existing, insObject.Data?.SpecifiedConsignment?.IncludedConsignmentItem);

                var successMessage = $"Importer Notification with Name: {identifier} updated successfully.";
                this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), successMessage);
                return Tuple.Create(true, successMessage);
            }

            if (!existing.defraimp_AggregateVersion.HasValue)
            {
                return this.TryUpdateByLastUpdatedDate(existing, insObject);
            }

            var infoMessage = $"No update needed for Importer Notification with Name: {identifier}. Existing version: {existing.defraimp_AggregateVersion}, Incoming version: {insObject.AggregateVersion}";
            this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), infoMessage);
            return Tuple.Create(false, infoMessage);
        }

        private Tuple<bool, string> TryUpdateByLastUpdatedDate(defraimp_ImporterNotification existing, INSObject insObject)
        {
            var identifier = insObject.Data.ExchangedDocument.Identifier;
            var lastStatusChange = insObject.StatusChanges != null && insObject.StatusChanges.Length > 0
                ? insObject.StatusChanges.OrderByDescending(sc => sc.DateChanged).FirstOrDefault()
                : null;

            if (lastStatusChange == null)
            {
                var infoMessage = $"No update needed for Importer Notification with Name: {identifier}. Existing record is up to date (no status change found).";
                this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), infoMessage);
                return Tuple.Create(false, infoMessage);
            }

            var cultureInfo = new CultureInfo("en-GB");
            DateTime.TryParse(lastStatusChange.DateChanged, cultureInfo, DateTimeStyles.None, out var updatedDate);

            if (updatedDate > existing.defraimp_lastupdated)
            {
                this.PopulateImporterNotificationFields(existing, insObject, true);
                this.orgSvc.Update(existing);
                this.DeleteExistingConsignmentItems(existing);
                this.ApplyConsignmentItemDetails(existing, insObject.Data?.SpecifiedConsignment?.IncludedConsignmentItem);

                var successMessage = $"Importer Notification with Name: {identifier} updated successfully based on last updated date.";
                this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), successMessage);
                return Tuple.Create(true, successMessage);
            }

            var noUpdateMessage = $"No update needed for Importer Notification with Name: {identifier}. Existing record is up to date based on last updated date.";
            this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), noUpdateMessage);
            return Tuple.Create(false, noUpdateMessage);
        }

        private Tuple<bool, string> TryCreateNew(INSObject insObject)
        {
            var identifier = insObject.Data.ExchangedDocument.Identifier;
            var newNotification = new defraimp_ImporterNotification();
            this.PopulateImporterNotificationFields(newNotification, insObject, false);

            if (newNotification.defraimp_status != defraimp_importernotificationstatus.Draft)
            {
                var importerNotificationId = this.orgSvc.Create(newNotification);
                newNotification.Id = importerNotificationId;

                this.ApplyConsignmentItemDetails(newNotification, insObject.Data?.SpecifiedConsignment?.IncludedConsignmentItem);

                var successMessage = $"Importer Notification with Name: {identifier} created successfully.";
                this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), successMessage);
                return Tuple.Create(true, successMessage);
            }

            var draftMessage = $"Importer Notification with Name: {identifier} is in Draft status. Not creating record.";
            this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), draftMessage);
            return Tuple.Create(false, draftMessage);
        }

        private HashSet<string> CollectCountryCodesFromInsObject(INSObject insObject)
        {
            var codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            this.AddIfNotNull(codes, insObject.Data?.ExchangedDocument?.Issuer?.PostalAddress?.CountryId);

            var consignment = insObject.Data?.SpecifiedConsignment;
            if (consignment != null)
            {
                this.AddIfNotNull(codes, consignment.OriginCountry?.Code?.Value);
                this.AddIfNotNull(codes, consignment.ConsigneeParty?.PostalAddress?.CountryId);
                this.AddIfNotNull(codes, consignment.Importer?.PostalAddress?.CountryId);
                this.AddIfNotNull(codes, consignment.ConsignorParty?.PostalAddress?.CountryId);
                this.AddIfNotNull(codes, consignment.DespatchParty?.PostalAddress?.CountryId);
                this.AddIfNotNull(codes, consignment.DeliveryParty?.PostalAddress?.CountryId);
                this.AddIfNotNull(codes, consignment.Carrier?.PostalAddress?.CountryId);
            }

            return codes;
        }

        private void AddIfNotNull(HashSet<string> set, string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                set.Add(value);
            }
        }

        private QueryExpression BuildCountryQuery(HashSet<string> countryCodes)
        {
            var filter = new FilterExpression(LogicalOperator.Or);
            foreach (var code in countryCodes)
            {
                filter.AddCondition(defra_country.Fields.defra_isocodealpha2, ConditionOperator.Equal, code);
            }

            return new QueryExpression(defra_country.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(
                    defra_country.Fields.defra_countryId,
                    defra_country.Fields.defra_isocodealpha2,
                    defra_country.Fields.defra_name),
                Criteria = filter,
            };
        }

        private Dictionary<string, defra_country> BuildCountryDictionary(HashSet<string> countryCodes, EntityCollection results)
        {
            var dictionary = new Dictionary<string, defra_country>(StringComparer.OrdinalIgnoreCase);

            foreach (var entity in results.Entities)
            {
                var country = entity.ToEntity<defra_country>();
                if (!string.IsNullOrWhiteSpace(country.defra_isocodealpha2))
                {
                    dictionary[country.defra_isocodealpha2] = country;
                }
            }

            foreach (var code in countryCodes)
            {
                if (!dictionary.ContainsKey(code))
                {
                    this.logger.Log(Severity.Warning, nameof(ProcessINSASBMessage), $"Country with ISO code '{code}' not found in Dataverse.");
                }
            }

            return dictionary;
        }

        private void ApplyOriginDetails(defraimp_ImporterNotification importerNotification, SpecifiedConsignment consignment, Dictionary<string, defra_country> countries)
        {
            if (consignment.OriginCountry?.Code != null && countries.TryGetValue(consignment.OriginCountry.Code.Value, out var originCountry))
            {
                importerNotification.defraimp_CountryofOriginId = originCountry.ToEntityReference();
            }

            if (consignment.OriginCountry?.SubordinateTradeCountrySubDivision != null &&
                consignment.OriginCountry.SubordinateTradeCountrySubDivision.Length > 0)
            {
                importerNotification.defraimp_commoditiesregionoforigin =
                    consignment.OriginCountry.SubordinateTradeCountrySubDivision[0].Identifier;
            }
        }

        private void ApplyConsigneeDetails(defraimp_ImporterNotification importerNotification, Party consignee, Dictionary<string, defra_country> countries)
        {
            if (consignee == null)
            {
                return;
            }

            importerNotification.defraimp_consigneecompanyname = consignee.Name;

            if (consignee.PostalAddress != null)
            {
                var address = consignee.PostalAddress;
                importerNotification.defraimp_consigneeaddressaddressline1 = address.LineOne;
                importerNotification.defraimp_consigneeaddressaddressline2 = address.LineTwo;
                importerNotification.defraimp_consigneeaddresscity = address.CityName;
                importerNotification.defraimp_consigneeaddresspostalzipcode = address.PostcodeCode;

                if (countries.TryGetValue(address.CountryId, out var country))
                {
                    importerNotification.defraimp_ConsigneeAddressCountryId = country.ToEntityReference();
                }
            }

            if (consignee.DefinedContact != null && consignee.DefinedContact.Length > 0)
            {
                var contact = consignee.DefinedContact[0];
                importerNotification.defraimp_consigneeaddressemail = contact.EmailURIUniversalCommunication;
                importerNotification.defraimp_consigneeaddresstelephone = contact.TelephoneUniversalCommunication;
            }
        }

        private void ApplyImporterDetails(defraimp_ImporterNotification importerNotification, Party importer, Dictionary<string, defra_country> countries)
        {
            if (importer == null)
            {
                return;
            }

            importerNotification.defraimp_importercompanyname = importer.Name;

            if (importer.PostalAddress != null)
            {
                var address = importer.PostalAddress;
                importerNotification.defraimp_importeraddressaddressline1 = address.LineOne;
                importerNotification.defraimp_importeraddressaddressline2 = address.LineTwo;
                importerNotification.defraimp_importeraddresscity = address.CityName;
                importerNotification.defraimp_importeraddresspostalzipcode = address.PostcodeCode;

                if (countries.TryGetValue(address.CountryId, out var country))
                {
                    importerNotification.defraimp_ImporterAddressCountryid = country.ToEntityReference();
                }
            }

            if (importer.DefinedContact != null && importer.DefinedContact.Length > 0)
            {
                var contact = importer.DefinedContact[0];
                importerNotification.defraimp_importeraddressemail = contact.EmailURIUniversalCommunication;
                importerNotification.defraimp_importeraddresstelephone = contact.TelephoneUniversalCommunication;
            }
        }

        private void ApplyConsignorDetails(defraimp_ImporterNotification importerNotification, Party consignor, Dictionary<string, defra_country> countries)
        {
            if (consignor == null)
            {
                return;
            }

            importerNotification.defraimp_consignorcompanyname = consignor.Name;

            if (consignor.PostalAddress != null)
            {
                var address = consignor.PostalAddress;
                importerNotification.defraimp_consignoraddressaddressline1 = address.LineOne;
                importerNotification.defraimp_consignoraddressaddressline2 = address.LineTwo;
                importerNotification.defraimp_consignoraddresscity = address.CityName;
                importerNotification.defraimp_consignoraddresspostalzipcode = address.PostcodeCode;

                if (countries.TryGetValue(address.CountryId, out var country))
                {
                    importerNotification.defraimp_ConsignorAddressCountryid = country.ToEntityReference();
                }
            }

            if (consignor.DefinedContact != null && consignor.DefinedContact.Length > 0)
            {
                var contact = consignor.DefinedContact[0];
                importerNotification.defraimp_consignoraddressemail = contact.EmailURIUniversalCommunication;
                importerNotification.defraimp_consignoraddresstelephone = contact.TelephoneUniversalCommunication;
            }
        }

        private void ApplyPlaceOfOriginDetails(defraimp_ImporterNotification importerNotification, Party despatchParty, Dictionary<string, defra_country> countries)
        {
            if (despatchParty == null)
            {
                return;
            }

            importerNotification.defraimp_PlaceofOriginCompanyName = despatchParty.Name;

            if (despatchParty.PostalAddress != null)
            {
                var address = despatchParty.PostalAddress;
                importerNotification.defraimp_PlaceofOriginAddressLine1 = address.LineOne;
                importerNotification.defraimp_PlaceofOriginAddressLine2 = address.LineTwo;
                importerNotification.defraimp_PlaceofOriginCity = address.CityName;
                importerNotification.defraimp_PlaceofOriginPostcode = address.PostcodeCode;

                if (countries.TryGetValue(address.CountryId, out var country))
                {
                    importerNotification.defraimp_PlaceofOriginCountryId = country.ToEntityReference();
                }
            }

            if (despatchParty.DefinedContact != null && despatchParty.DefinedContact.Length > 0)
            {
                var contact = despatchParty.DefinedContact[0];
                importerNotification.defraimp_PlaceofOriginEmail = contact.EmailURIUniversalCommunication;
                importerNotification.defraimp_PlaceofOriginPhone = contact.TelephoneUniversalCommunication;
            }
        }

        private void ApplyPlaceOfDestinationDetails(defraimp_ImporterNotification importerNotification, SpecifiedConsignment consignment, Dictionary<string, defra_country> countries)
        {
            var deliveryParty = consignment.DeliveryParty;

            if (deliveryParty == null)
            {
                return;
            }

            importerNotification.defraimp_placeofdestinationcompanyname = deliveryParty.Name;

            if (deliveryParty.PostalAddress != null)
            {
                var address = deliveryParty.PostalAddress;
                importerNotification.defraimp_placeofdestinationaddressaddressline1 = address.LineOne;
                importerNotification.defraimp_placeofdestinationaddressaddressline2 = address.LineTwo;
                importerNotification.defraimp_placeofdestinationaddresscity = address.CityName;
                importerNotification.defraimp_placeofdestinationaddresspostalzipcode = address.PostcodeCode;

                if (countries.TryGetValue(address.CountryId, out var country))
                {
                    importerNotification.defraimp_PlaceofDestinationCountryid = country.ToEntityReference();
                }
            }

            if (deliveryParty.DefinedContact != null && deliveryParty.DefinedContact.Length > 0)
            {
                var contact = deliveryParty.DefinedContact[0];
                importerNotification.defraimp_placeofdestinationaddressemail = contact.EmailURIUniversalCommunication;
                importerNotification.defraimp_placeofdestinationaddresstelephone = contact.TelephoneUniversalCommunication;
            }

            importerNotification.defraimp_isplaceofdestinationthepermanentaddress =
                this.ResolvePermanentAddressFlag(consignment, deliveryParty.PostalAddress);
        }

        private bool? ResolvePermanentAddressFlag(SpecifiedConsignment consignment, PostalAddress destinationAddress)
        {
            var firstItem = consignment.IncludedConsignmentItem?.Length > 0
                ? consignment.IncludedConsignmentItem[0]
                : null;

            var tradeLineItem = firstItem?.IncludedTradeLineItem?.Length > 0
                ? firstItem.IncludedTradeLineItem[0]
                : null;

            var productInstance = tradeLineItem?.IndividualTradeProductInstance?.Length > 0
                ? tradeLineItem.IndividualTradeProductInstance[0]
                : null;

            if (productInstance?.PermanentLocation == null)
            {
                return null;
            }

            return this.IsSameAddress(destinationAddress, productInstance.PermanentLocation.PostalAddress);
        }

        private void ApplyTransporterDetails(defraimp_ImporterNotification importerNotification, Carrier transporter, Dictionary<string, defra_country> countries)
        {
            if (transporter == null)
            {
                return;
            }

            importerNotification.defraimp_transportercompanyname = transporter.Name;
            importerNotification.defraimp_transporterapprovalnumber = transporter.Identifier;

            if (transporter.PostalAddress != null)
            {
                var address = transporter.PostalAddress;
                importerNotification.defraimp_transporteraddressaddressline1 = address.LineOne;
                importerNotification.defraimp_transporteraddressaddressline2 = address.LineTwo;
                importerNotification.defraimp_transporteraddresscity = address.CityName;
                importerNotification.defraimp_transporteraddresspostalzipcode = address.PostcodeCode;

                if (countries.TryGetValue(address.CountryId, out var country))
                {
                    importerNotification.defraimp_TransporterAddressCountryid = country.ToEntityReference();
                }
            }

            // TODO - check we want to take first party type code in this scenario
            if (transporter.PartyTypeCode != null && transporter.PartyTypeCode.Length > 0)
            {
                importerNotification.defraimp_transportertype = transporter.PartyTypeCode[0].Value;
            }

            // TODO - transporter status and approval number
        }

        private void ApplyTransportMovementDetails(defraimp_ImporterNotification importerNotification, SpecifiedConsignment consignment)
        {
            if (consignment.MainCarriageLogisticsTransportMovement == null ||
                consignment.MainCarriageLogisticsTransportMovement.Length == 0)
            {
                return;
            }

            var transport = consignment.MainCarriageLogisticsTransportMovement[0];

            if (transport.ArrivalEvent != null && transport.ArrivalEvent.Length > 0)
            {
                var cultureInfo = new CultureInfo("en-GB");
                importerNotification.defraimp_ArrivalDate = DateTime.TryParse(
                    transport.ArrivalEvent[0].ScheduledOccurrenceDateTime, cultureInfo, DateTimeStyles.None, out var arrivalDate)
                    ? arrivalDate
                    : (DateTime?)null;
            }

            importerNotification.defraimp_MeansofTransporttoEntryPointType = this.ResolveMeansOfTransportType(transport.ModeCode);
            importerNotification.defraimp_MeansofTransporttoEntryPointId = transport.Identifier;

            if (transport.TransportContractRelatedReferencedDocument != null &&
                transport.TransportContractRelatedReferencedDocument.Length > 0)
            {
                importerNotification.defraimp_MeansofTransporttoEntryPointDocument =
                    transport.TransportContractRelatedReferencedDocument[0].Identifier;
            }
        }

        private string ResolveMeansOfTransportType(int? modeCode)
        {
            switch (modeCode)
            {
                case 1: return "Ship";
                case 2: return "Railway Wagon";
                case 3: return "Road Vehicle";
                case 4: return "Aeroplane";
                default: return null;
            }
        }

        private defraimp_importernotificationstatus ResolveNotificationStatus(string statusCode)
        {
            switch (statusCode)
            {
                case "SUBMITTED":
                    return defraimp_importernotificationstatus.Submitted;
                case "AMEND":
                    return defraimp_importernotificationstatus.Amend;
                case "DRAFT":
                    return defraimp_importernotificationstatus.Draft;
                case "DELETED":
                    return defraimp_importernotificationstatus.Deleted;
                default:
                    throw new InvalidOperationException($"Unsupported notification status code '{statusCode}'.");
            }
        }

        private void ApplyIssuerDetails(defraimp_ImporterNotification importerNotification, Issuer issuer, Dictionary<string, defra_country> countries)
        {
            if (issuer == null)
            {
                return;
            }

            importerNotification.defraimp_personresponsiblecompanyname = issuer.Name;

            if (issuer.PostalAddress != null)
            {
                var address = issuer.PostalAddress;
                importerNotification.defraimp_personresponsibleaddress =
                    this.FormatAddress(address.LineOne, address.LineTwo, address.CityName, address.PostcodeCode);

                if (countries.TryGetValue(address.CountryId, out var personResponsibleCountry))
                {
                    importerNotification.defraimp_PersonResponsibleCountryId = personResponsibleCountry.ToEntityReference();
                }
            }

            if (issuer.DefinedContact != null && issuer.DefinedContact.Length > 0)
            {
                var contact = issuer.DefinedContact[0];
                importerNotification.defraimp_personresponsiblename = contact.PersonName;
                importerNotification.defraimp_personresponsibleemail = contact.EmailURIUniversalCommunication;
                importerNotification.defraimp_personresponsiblephone = contact.TelephoneUniversalCommunication;
            }
        }

        private void ApplyConsignmentItemDetails(defraimp_ImporterNotification importerNotification, IncludedConsignmentItem[] includedConsignmentItem)
        {
            if (importerNotification == null || importerNotification.Id == Guid.Empty)
            {
                return;
            }

            if (includedConsignmentItem == null || includedConsignmentItem.Length == 0)
            {
                return;
            }

            foreach (var consignmentItem in includedConsignmentItem)
            {
                if (consignmentItem?.IncludedTradeLineItem == null || consignmentItem.IncludedTradeLineItem.Length == 0)
                {
                    continue;
                }

                foreach (var lineItem in consignmentItem.IncludedTradeLineItem)
                {
                    if (lineItem == null)
                    {
                        continue;
                    }

                    var numberOfAnimals = lineItem.SpecifiedLineTradeDelivery != null && lineItem.SpecifiedLineTradeDelivery.Length > 0
                        ? lineItem.SpecifiedLineTradeDelivery[0]?.ProductUnitQuantity?.Content
                        : null;

                    var numberOfPackages = lineItem.PhysicalReferencedLogisticsPackage != null && lineItem.PhysicalReferencedLogisticsPackage.Length > 0
                        ? lineItem.PhysicalReferencedLogisticsPackage[0]?.ItemQuantity
                        : null;

                    var commodityComplement = new defraimp_commoditycomplement
                    {
                        defraimp_ImporterNotificationId = importerNotification.ToEntityReference(),
                        defraimp_NumberofAnimals = numberOfAnimals.HasValue ? numberOfAnimals.Value.ToString(CultureInfo.InvariantCulture) : null,
                        defraimp_NumberofPackages = numberOfPackages,
                        defraimp_name = lineItem.ScientificName,
                        defraimp_commodityid = lineItem.ApplicableClassification?.Length > 0 ? lineItem.ApplicableClassification[0]?.ClassCode?.Value : null,
                        defraimp_commoditydescription = lineItem.Description != null && lineItem.Description.Length > 0
                            ? string.Join(", ", lineItem.Description)
                            : null,
                        defraimp_speciesname = lineItem.ScientificName,
                        defraimp_speciescommonname = lineItem.CommonName,
                    };

                    this.orgSvc.Create(commodityComplement);
                }
            }
        }

        private void DeleteExistingConsignmentItems(defraimp_ImporterNotification existing)
        {
var query = new QueryExpression(defraimp_commoditycomplement.EntityLogicalName);
query.Criteria.AddCondition(new ConditionExpression(defraimp_commoditycomplement.Fields.defraimp_ImporterNotificationId, ConditionOperator.Equal, existing.Id));
query.ColumnSet = new ColumnSet();

            var results = this.orgSvc.RetrieveMultiple(query);

            foreach (var commodity in results.Entities)
            {
                this.orgSvc.Delete(defraimp_commoditycomplement.EntityLogicalName, commodity.Id);
            }
        }

        private bool TryDeserializeMessage(string message, out INSObject insObject, out string errorMessage)
        {
            try
            {
                insObject = message.FromJSON<INSObject>();
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = $"Error deserializing message: {ex.Message}";
                this.logger.Log(Severity.Error, nameof(ProcessINSASBMessage), errorMessage);
                insObject = null;
                return false;
            }
        }

        private defraimp_ImporterNotification FindExistingNotification(string identifier)
        {
            var query = new QueryExpression(defraimp_ImporterNotification.EntityLogicalName);
            query.Criteria.AddCondition(new ConditionExpression(defraimp_ImporterNotification.Fields.defraimp_Name, ConditionOperator.Equal, identifier));
            query.ColumnSet = new ColumnSet(new string[]
            {
                defraimp_ImporterNotification.Fields.defraimp_Name,
                defraimp_ImporterNotification.Fields.defraimp_AggregateVersion,
                defraimp_ImporterNotification.Fields.defraimp_lastupdated,
            });

            var results = this.orgSvc.RetrieveMultiple(query);
            return results.Entities.Count > 0
                ? results.Entities[0].ToEntity<defraimp_ImporterNotification>()
                : null;
        }

        private defraimp_importernotificationtype ResolveNotificationType(string type)
        {
            return !string.IsNullOrWhiteSpace(type) && type.Equals("GBN-AG", StringComparison.OrdinalIgnoreCase)
                ? defraimp_importernotificationtype.GBNAG
                : defraimp_importernotificationtype.CHEDA;
        }

        private void ApplySubmissionDetails(defraimp_ImporterNotification importerNotification, INSObject insObject)
        {
            var submittedStatusChange = GetStatusChange(insObject, sc => sc.Status == "SUBMITTED", ascending: true);

            if (submittedStatusChange == null)
            {
                return;
            }

            var cultureInfo = new CultureInfo("en-GB");
            if (DateTime.TryParse(submittedStatusChange.DateChanged, cultureInfo, DateTimeStyles.None, out var submissionDate))
            {
                importerNotification.defraimp_submissiondate = submissionDate;
            }

            if (submittedStatusChange.Actor != null)
            {
                importerNotification.defraimp_submittedbydisplayname = submittedStatusChange.Actor.DisplayName;
            }
        }

        private void ApplyLastUpdatedDetails(defraimp_ImporterNotification importerNotification, INSObject insObject)
        {
            var lastStatusChange = GetStatusChange(insObject, sc => true, ascending: false);

            if (lastStatusChange == null)
            {
                return;
            }

            var cultureInfo = new CultureInfo("en-GB");
            if (DateTime.TryParse(lastStatusChange.DateChanged, cultureInfo, DateTimeStyles.None, out var updatedDate))
            {
                importerNotification.defraimp_lastupdated = updatedDate;
            }

            if (lastStatusChange.Actor != null)
            {
                importerNotification.defraimp_lastupdatedbydisplayname = lastStatusChange.Actor.DisplayName;
            }
        }
    }
}
