namespace Defra.Imports.BusinessLogic.ImporterNotification
{
    using System;
    using System.Collections.Generic;
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
        private IOrganizationService orgSvc;
        private ILogWriter logger;

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

            INSObject insObject;
            try
            {
                insObject = message.FromJSON<INSObject>();
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error deserializing message: {ex.Message}";
                this.logger.Log(Severity.Error, nameof(ProcessINSASBMessage), errorMessage);
                return Tuple.Create(false, errorMessage);
            }

            if (string.IsNullOrWhiteSpace(insObject?.Data?.ExchangedDocument?.Identifier))
            {
                var errorMessage = "Error processing Importer Notification - message does not contain data.exchangedDocument.identifier";
                this.logger.Log(Severity.Error, nameof(ProcessINSASBMessage), errorMessage);
                return Tuple.Create(false, errorMessage);
            }

            try
            {
                QueryExpression getImporterNotification = new QueryExpression(defraimp_ImporterNotification.EntityLogicalName);
                getImporterNotification.Criteria.AddCondition(new ConditionExpression(defraimp_ImporterNotification.Fields.defraimp_Name, ConditionOperator.Equal, insObject.Data.ExchangedDocument.Identifier));
                getImporterNotification.ColumnSet = new ColumnSet(new string[]
                {
                    defraimp_ImporterNotification.Fields.defraimp_Name,
                    defraimp_ImporterNotification.Fields.defraimp_AggregateVersion,
                    defraimp_ImporterNotification.Fields.defraimp_lastupdated,
                });

                EntityCollection importerNotifications = this.orgSvc.RetrieveMultiple(getImporterNotification);

                if (importerNotifications.Entities.Count > 0)
                {
                    // Update existing record
                    defraimp_ImporterNotification existingImporterNotification = importerNotifications.Entities[0].ToEntity<defraimp_ImporterNotification>();

                    if (existingImporterNotification.defraimp_AggregateVersion < insObject.AggregateVersion)
                    {
                        this.PopulateImporterNotificationFields(existingImporterNotification, insObject, true);
                        this.orgSvc.Update(existingImporterNotification);
                        var successMessage = $"Importer Notification with Name: {insObject.Data.ExchangedDocument.Identifier} updated successfully.";
                        this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), successMessage);
                        return Tuple.Create(true, successMessage);
                    }
                    else if (!existingImporterNotification.defraimp_AggregateVersion.HasValue)
                    {
                        // Existing record has no aggregate version, so check against last updated value to determine if the version is newer
                        // Get the last status change ordered by date
                        StatusChange lastStatusChange = null;
                        if (insObject.StatusChanges != null && insObject.StatusChanges.Length > 0)
                        {
                            lastStatusChange = insObject.StatusChanges
                                .OrderByDescending(sc => sc.DateChanged)
                                .FirstOrDefault();
                        }

                        if (lastStatusChange != null)
                        {
                            DateTime.TryParse(lastStatusChange.DateChanged, out var updatedDate);
                            if (updatedDate > existingImporterNotification.defraimp_lastupdated)
                            {
                                this.PopulateImporterNotificationFields(existingImporterNotification, insObject, true);
                                this.orgSvc.Update(existingImporterNotification);
                                var successMessage = $"Importer Notification with Name: {insObject.Data.ExchangedDocument.Identifier} updated successfully based on last updated date.";
                                this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), successMessage);
                                return Tuple.Create(true, successMessage);
                            }
                            else
                            {
                                var infoMessage = $"No update needed for Importer Notification with Name: {insObject.Data.ExchangedDocument.Identifier}. Existing record is up to date based on last updated date.";
                                this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), infoMessage);
                                return Tuple.Create(false, infoMessage);
                            }
                        }
                        else
                        {
                            var infoMessage = $"No update needed for Importer Notification with Name: {insObject.Data.ExchangedDocument.Identifier}. Existing record is up to date (no status change found).";
                            this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), infoMessage);
                            return Tuple.Create(false, infoMessage);
                        }
                    }
                    else
                    {
                        // No update needed, existing record is up to date
                        var infoMessage = $"No update needed for Importer Notification with Name: {insObject.Data.ExchangedDocument.Identifier}. Existing version: {existingImporterNotification.defraimp_AggregateVersion}, Incoming version: {insObject.AggregateVersion}";
                        this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), infoMessage);
                        return Tuple.Create(false, infoMessage);
                    }
                }
                else
                {
                    // Create new record
                    defraimp_ImporterNotification newImporterNotification = new defraimp_ImporterNotification();
                    this.PopulateImporterNotificationFields(newImporterNotification, insObject, false);

                    if (newImporterNotification.defraimp_status != defraimp_importernotificationstatus.Draft)
                    {
                        this.orgSvc.Create(newImporterNotification);
                        var successMessage = $"Importer Notification with Name: {insObject.Data.ExchangedDocument.Identifier} created successfully.";
                        this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), successMessage);
                        return Tuple.Create(true, successMessage);
                    }
                    else
                    {
                        var draftMessage = $"Importer Notification with Name: {insObject.Data.ExchangedDocument.Identifier} is in Draft status. Not creating record.";
                        this.logger.Log(Severity.Info, nameof(ProcessINSASBMessage), draftMessage);
                        return Tuple.Create(false, draftMessage);
                    }
                }
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
            // Get countries from Dataverse based on country codes in the INSObject
            var countries = this.GetCountriesFromInsObject(insObject);

            // aggregate version is used to determine if the record is up to date with the latest version of the data
            importerNotification.defraimp_AggregateVersion = insObject.AggregateVersion;

            // Get submission info from status changes
            // Get the first SUBMITTED status change ordered by date
            StatusChange submittedStatusChange = null;
            if (insObject.StatusChanges != null && insObject.StatusChanges.Length > 0)
            {
                submittedStatusChange = insObject.StatusChanges
                    .Where(sc => sc.Status == "SUBMITTED")
                    .OrderBy(sc => sc.DateChanged)
                    .FirstOrDefault();
            }

            // Use the submitted status change if found
            if (submittedStatusChange != null)
            {
                // Using submittedStatusChange.DateChanged for submission date
                // and submittedStatusChange.Actor for submitted by information
                if (DateTime.TryParse(submittedStatusChange.DateChanged, out var submissionDate))
                {
                    importerNotification.defraimp_submissiondate = submissionDate;
                }

                if (submittedStatusChange.Actor != null)
                {
                    importerNotification.defraimp_submittedbydisplayname = submittedStatusChange.Actor.DisplayName;
                }
            }

            // Last updated details
            if (isUpdate)
            {
                // Get info from status changes
                // Get the last status change ordered by date
                StatusChange lastStatusChange = null;
                if (insObject.StatusChanges != null && insObject.StatusChanges.Length > 0)
                {
                    lastStatusChange = insObject.StatusChanges
                        .OrderByDescending(sc => sc.DateChanged)
                        .FirstOrDefault();
                }

                // Use the status change if found
                if (lastStatusChange != null)
                {
                    // Using lastStatusChange.DateChanged for last updated date
                    // and lastStatusChange.Actor for last updated by information
                    if (DateTime.TryParse(lastStatusChange.DateChanged, out var updatedDate))
                    {
                        importerNotification.defraimp_lastupdated = updatedDate;
                    }

                    if (lastStatusChange.Actor != null)
                    {
                        importerNotification.defraimp_lastupdatedbydisplayname = lastStatusChange.Actor.DisplayName;
                    }
                }
            }

            // Type - Map from insObject.Data.Type
            if (!string.IsNullOrWhiteSpace(insObject.Data?.Type) && insObject.Data.Type.Equals("GBN-AG", StringComparison.OrdinalIgnoreCase))
            {
                importerNotification.defraimp_type = defraimp_importernotificationtype.GBNAG;
            }
            else
            {
                importerNotification.defraimp_type = defraimp_importernotificationtype.CHEDA;
            }

            // Basic notification details
            if (insObject.Data?.ExchangedDocument != null)
            {
                importerNotification = this.PopulateExchangedDocumentInformation(importerNotification, insObject, isUpdate, countries);
            }

            // Consignment details
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
        /// <param name="countries" >Dictionary of country codes to defra_country entities.</param>
        /// <returns>Updated importer notification entity.</returns>
        public defraimp_ImporterNotification PopulateExchangedDocumentInformation(defraimp_ImporterNotification importerNotification, INSObject insObject, bool isUpdate, Dictionary<string, defra_country> countries)
        {
            importerNotification.defraimp_Name = insObject.Data.ExchangedDocument.Identifier;

            // Trader Reference
            importerNotification.defraimp_TraderReference = insObject.Data.ExchangedDocument.TraderAssignedId;

            // Status reason
            switch (insObject.Data.ExchangedDocument.NotificationStatusCode)
            {
                case "SUBMITTED":
                    importerNotification.defraimp_status = defraimp_importernotificationstatus.Submitted;
                    break;
                case "AMEND":
                    importerNotification.defraimp_status = defraimp_importernotificationstatus.Amend;
                    break;
                case "DRAFT":
                    importerNotification.defraimp_status = defraimp_importernotificationstatus.Draft;
                    break;
                case "DELETED":
                    importerNotification.defraimp_status = defraimp_importernotificationstatus.Deleted;
                    break;
                default:
                    importerNotification.defraimp_status = defraimp_importernotificationstatus.Submitted;
                    break;
            }

            importerNotification.defraimp_Version = insObject.Data.ExchangedDocument.VersionId;

            // Process included clauses from first signatory authentication
            if (insObject.Data.ExchangedDocument.FirstSignatoryAuthentication?.IncludedClause != null)
            {
                var includedClauses = insObject.Data.ExchangedDocument.FirstSignatoryAuthentication.IncludedClause;

                // Find PURPOSE clause
                var purposeClause = includedClauses.FirstOrDefault(ic => ic.Identifier == "PURPOSE");
                if (purposeClause != null)
                {
                    // TODO - get mappings for purpose clause content to defraimp_PurposeofConsignment option set values
                    //importerNotification.defraimp_PurposeofConsignment = purposeClause.Content;
                }

                // Find INTERNAL_MARKET_PURPOSE clause
                var internalMarketPurposeClause = includedClauses.FirstOrDefault(ic => ic.Identifier == "INTERNAL_MARKET_PURPOSE");
                if (internalMarketPurposeClause != null)
                {
                    // TODO - get mappings for internal market purpose clause content to defraimp_commoditiesInternalMarketPurpose option set values
                    //importerNotification.defraimp_commoditiesInternalMarketPurpose = internalMarketPurposeClause.Content;
                }

                // Find GOODS_CERTIFIED_AS clause
                var goodsCertifiedAsClause = includedClauses.FirstOrDefault(ic => ic.Identifier == "GOODS_CERTIFIED_AS");
                if (goodsCertifiedAsClause != null)
                {
                    // TODO - get mappings for goods certified as clause content to defraimp_commoditiescertifiedfor option set values
                    //importerNotification.defraimp_commoditiescertifiedfor = goodsCertifiedAsClause.Content;
                }
            }

            // Person Responsible (mapped from Issuer contact details)
            if (insObject.Data.ExchangedDocument?.Issuer != null)
            {
                var issuer = insObject.Data.ExchangedDocument.Issuer;
                importerNotification.defraimp_personresponsiblecompanyname = issuer.Name;

                if (issuer.PostalAddress != null)
                {
                    var address = issuer.PostalAddress;
                    importerNotification.defraimp_personresponsibleaddress =
                        this.FormatAddress(address.LineOne, address.LineTwo, address.CityName, address.PostcodeCode);

                    // Use the country dictionary to set person responsible country lookup field
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

            // Country of origin
            if (consignment.OriginCountry?.Code != null)
            {
                // Use the country dictionary to set origin country lookup field
                if (countries.TryGetValue(consignment.OriginCountry.Code.Value, out var originCountry))
                {
                    importerNotification.defraimp_CountryofOriginId = originCountry.ToEntityReference();
                }
            }

            // Region of origin from subordinate country subdivisions
            if (consignment.OriginCountry?.SubordinateTradeCountrySubDivision != null &&
                consignment.OriginCountry.SubordinateTradeCountrySubDivision.Length > 0)
            {
                importerNotification.defraimp_commoditiesregionoforigin =
                    consignment.OriginCountry.SubordinateTradeCountrySubDivision[0].Identifier;
            }

            // Consignee details (mapped from ConsigneeParty)
            if (consignment.ConsigneeParty != null)
            {
                var consignee = consignment.ConsigneeParty;
                importerNotification.defraimp_consigneecompanyname = consignee.Name;

                if (consignee.PostalAddress != null)
                {
                    var address = consignee.PostalAddress;
                    importerNotification.defraimp_consigneeaddressaddressline1 = address.LineOne;
                    importerNotification.defraimp_consigneeaddressaddressline2 = address.LineTwo;
                    importerNotification.defraimp_consigneeaddresscity = address.CityName;
                    importerNotification.defraimp_consigneeaddresspostalzipcode = address.PostcodeCode;

                    // Use the country dictionary to set consignee country lookup field
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

            // Importer details
            if (consignment.Importer != null)
            {
                var importer = consignment.Importer;
                importerNotification.defraimp_importercompanyname = importer.Name;

                if (importer.PostalAddress != null)
                {
                    var address = importer.PostalAddress;
                    importerNotification.defraimp_importeraddressaddressline1 = address.LineOne;
                    importerNotification.defraimp_importeraddressaddressline2 = address.LineTwo;
                    importerNotification.defraimp_importeraddresscity = address.CityName;
                    importerNotification.defraimp_importeraddresspostalzipcode = address.PostcodeCode;

                    // Use the country dictionary to set importer country lookup field
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

            // Consignor details (Exporter)
            if (consignment.ConsignorParty != null)
            {
                var consignor = consignment.ConsignorParty;
                importerNotification.defraimp_consignorcompanyname = consignor.Name;

                if (consignor.PostalAddress != null)
                {
                    var address = consignor.PostalAddress;
                    importerNotification.defraimp_consignoraddressaddressline1 = address.LineOne;
                    importerNotification.defraimp_consignoraddressaddressline2 = address.LineTwo;
                    importerNotification.defraimp_consignoraddresscity = address.CityName;
                    importerNotification.defraimp_consignoraddresspostalzipcode = address.PostcodeCode;

                    // Use the country dictionary to set consignor country lookup field
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

            // Place of Origin (mapped from DespatchParty)
            if (consignment.DespatchParty != null)
            {
                var placeOfOrigin = consignment.DespatchParty;
                importerNotification.defraimp_PlaceofOriginCompanyName = placeOfOrigin.Name;

                if (placeOfOrigin.PostalAddress != null)
                {
                    var address = placeOfOrigin.PostalAddress;
                    importerNotification.defraimp_PlaceofOriginAddressLine1 = address.LineOne;
                    importerNotification.defraimp_PlaceofOriginAddressLine2 = address.LineTwo;
                    importerNotification.defraimp_PlaceofOriginCity = address.CityName;
                    importerNotification.defraimp_PlaceofOriginPostcode = address.PostcodeCode;
                    // Use the country dictionary to set place of origin country lookup field
                    if (countries.TryGetValue(address.CountryId, out var country))
                    {
                        importerNotification.defraimp_PlaceofOriginCountryId = country.ToEntityReference();
                    }
                }

                if (placeOfOrigin.DefinedContact != null && placeOfOrigin.DefinedContact.Length > 0)
                {
                    var contact = placeOfOrigin.DefinedContact[0];
                    importerNotification.defraimp_PlaceofOriginEmail = contact.EmailURIUniversalCommunication;
                    importerNotification.defraimp_PlaceofOriginPhone = contact.TelephoneUniversalCommunication;
                }
            }

            // Place of Destination (mapped from DeliveryParty)
            if (consignment.DeliveryParty != null)
            {
                var placeOfDestination = consignment.DeliveryParty;
                importerNotification.defraimp_placeofdestinationcompanyname = placeOfDestination.Name;

                if (placeOfDestination.PostalAddress != null)
                {
                    var address = placeOfDestination.PostalAddress;
                    importerNotification.defraimp_placeofdestinationaddressaddressline1 = address.LineOne;
                    importerNotification.defraimp_placeofdestinationaddressaddressline2 = address.LineTwo;
                    importerNotification.defraimp_placeofdestinationaddresscity = address.CityName;
                    importerNotification.defraimp_placeofdestinationaddresspostalzipcode = address.PostcodeCode;

                    // Use the country dictionary to set place of destination country lookup field
                    if (countries.TryGetValue(address.CountryId, out var country))
                    {
                        importerNotification.defraimp_PlaceofDestinationCountryid = country.ToEntityReference();
                    }
                }

                if (placeOfDestination.DefinedContact != null && placeOfDestination.DefinedContact.Length > 0)
                {
                    var contact = placeOfDestination.DefinedContact[0];
                    importerNotification.defraimp_placeofdestinationaddressemail = contact.EmailURIUniversalCommunication;
                    importerNotification.defraimp_placeofdestinationaddresstelephone = contact.TelephoneUniversalCommunication;
                }

                // Check if place of destination is permanent address by comparing with individual trade product instance permanent location
                if (consignment.IncludedConsignmentItem != null && consignment.IncludedConsignmentItem.Length > 0)
                {
                    var firstItem = consignment.IncludedConsignmentItem[0];
                    if (firstItem.IncludedTradeLineItem != null && firstItem.IncludedTradeLineItem.Length > 0)
                    {
                        var tradeLineItem = firstItem.IncludedTradeLineItem[0];
                        if (tradeLineItem.IndividualTradeProductInstance != null && tradeLineItem.IndividualTradeProductInstance.Length > 0)
                        {
                            var productInstance = tradeLineItem.IndividualTradeProductInstance[0];
                            if (productInstance.PermanentLocation != null)
                            {
                                // Compare addresses to determine if they match
                                bool isSameAddress = this.IsSameAddress(
                                    placeOfDestination.PostalAddress,
                                    productInstance.PermanentLocation.PostalAddress);
                                importerNotification.defraimp_isplaceofdestinationthepermanentaddress = isSameAddress;
                            }
                        }
                    }
                }
            }

            // Transporter details (mapped from Carrier)
            if (consignment.Carrier != null)
            {
                var transporter = consignment.Carrier;
                importerNotification.defraimp_transportercompanyname = transporter.Name;
                importerNotification.defraimp_transporterapprovalnumber = transporter.Identifier;

                if (transporter.PostalAddress != null)
                {
                    var address = transporter.PostalAddress;
                    importerNotification.defraimp_transporteraddressaddressline1 = address.LineOne;
                    importerNotification.defraimp_transporteraddressaddressline2 = address.LineTwo;
                    importerNotification.defraimp_transporteraddresscity = address.CityName;
                    importerNotification.defraimp_transporteraddresspostalzipcode = address.PostcodeCode;

                    // Use the country dictionary to set transporter country lookup field
                    if (countries.TryGetValue(address.CountryId, out var country))
                    {
                        importerNotification.defraimp_TransporterAddressCountryid = country.ToEntityReference();
                    }
                }

                // TODO - check we want to take first party type code in this scenario
                // Transporter type from party type codes
                if (transporter.PartyTypeCode != null && transporter.PartyTypeCode.Length > 0)
                {
                    importerNotification.defraimp_transportertype = transporter.PartyTypeCode[0].Value;
                }

                // TODO - transporter status and approval number
            }

            // CPH number
            if (consignment.FinalDestinationLocation != null)
            {
                importerNotification.defraimp_cphnumber = consignment.FinalDestinationLocation.Identifier;
            }

            // Port of entry from unloading location
            if (consignment.UnloadingBaseportLocation != null)
            {
                importerNotification.defraimp_portofentry = consignment.UnloadingBaseportLocation.Identifier;
            }

            // Arrival date from transport movement
            if (consignment.MainCarriageLogisticsTransportMovement != null &&
                consignment.MainCarriageLogisticsTransportMovement.Length > 0)
            {
                var transport = consignment.MainCarriageLogisticsTransportMovement[0];
                if (transport.ArrivalEvent != null && transport.ArrivalEvent.Length > 0)
                {
                    if (DateTime.TryParse(transport.ArrivalEvent[0].ScheduledOccurrenceDateTime, out var arrivalDate))
                    {
                        importerNotification.defraimp_ArrivalDate = arrivalDate;
                    }
                    else
                    {
                        importerNotification.defraimp_ArrivalDate = null;
                    }
                }

                // Means of transport details
                var modeCode = transport.ModeCode;
                switch (modeCode)
                {
                    case 1:
                        importerNotification.defraimp_MeansofTransporttoEntryPointType = "Ship";
                        break;
                    case 2:
                        importerNotification.defraimp_MeansofTransporttoEntryPointType = "Railway Wagon";
                        break;
                    case 3:
                        importerNotification.defraimp_MeansofTransporttoEntryPointType = "Road Vehicle";
                        break;
                    case 4:
                        importerNotification.defraimp_MeansofTransporttoEntryPointType = "Aeroplane";
                        break;
                    default:
                        break;
                }

                // Means of transport ID (e.g., flight number, vessel name, etc.)
                importerNotification.defraimp_MeansofTransporttoEntryPointId = transport.Identifier;

                // Transport contract document
                if (transport.TransportContractRelatedReferencedDocument != null &&
                    transport.TransportContractRelatedReferencedDocument.Length > 0)
                {
                    importerNotification.defraimp_MeansofTransporttoEntryPointDocument = transport.TransportContractRelatedReferencedDocument[0].Identifier;
                }
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
            var countryCodes = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Collect all unique country codes from the INSObject
            if (insObject.Data?.ExchangedDocument?.Issuer?.PostalAddress?.CountryId != null)
            {
                countryCodes.Add(insObject.Data.ExchangedDocument.Issuer.PostalAddress.CountryId);
            }

            if (insObject.Data?.SpecifiedConsignment != null)
            {
                var consignment = insObject.Data.SpecifiedConsignment;

                if (consignment.OriginCountry?.Code?.Value != null)
                {
                    countryCodes.Add(consignment.OriginCountry.Code.Value);
                }

                if (consignment.ConsigneeParty?.PostalAddress?.CountryId != null)
                {
                    countryCodes.Add(consignment.ConsigneeParty.PostalAddress.CountryId);
                }

                if (consignment.Importer?.PostalAddress?.CountryId != null)
                {
                    countryCodes.Add(consignment.Importer.PostalAddress.CountryId);
                }

                if (consignment.ConsignorParty?.PostalAddress?.CountryId != null)
                {
                    countryCodes.Add(consignment.ConsignorParty.PostalAddress.CountryId);
                }

                if (consignment.DespatchParty?.PostalAddress?.CountryId != null)
                {
                    countryCodes.Add(consignment.DespatchParty.PostalAddress.CountryId);
                }

                if (consignment.DeliveryParty?.PostalAddress?.CountryId != null)
                {
                    countryCodes.Add(consignment.DeliveryParty.PostalAddress.CountryId);
                }

                if (consignment.Carrier?.PostalAddress?.CountryId != null)
                {
                    countryCodes.Add(consignment.Carrier.PostalAddress.CountryId);
                }
            }

            // Remove any null or empty values
            countryCodes.RemoveWhere(string.IsNullOrWhiteSpace);

            // If no country codes found, return empty dictionary
            if (countryCodes.Count == 0)
            {
                return new Dictionary<string, defra_country>(StringComparer.OrdinalIgnoreCase);
            }

            // Build query to retrieve countries
            QueryExpression query = new QueryExpression(defra_country.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(
                    defra_country.Fields.defra_countryId,
                    defra_country.Fields.defra_isocodealpha2,
                    defra_country.Fields.defra_name),
            };

            // Add filter conditions for country codes
            FilterExpression filter = new FilterExpression(LogicalOperator.Or);
            foreach (var countryCode in countryCodes)
            {
                filter.AddCondition(
                    defra_country.Fields.defra_isocodealpha2,
                    ConditionOperator.Equal,
                    countryCode);
            }

            query.Criteria = filter;

            // Execute query
            EntityCollection countries = this.orgSvc.RetrieveMultiple(query);

            // Build dictionary of country code to entity
            var countryDictionary = new Dictionary<string, defra_country>(StringComparer.OrdinalIgnoreCase);
            foreach (var entity in countries.Entities)
            {
                var country = entity.ToEntity<defra_country>();
                if (!string.IsNullOrWhiteSpace(country.defra_isocodealpha2))
                {
                    countryDictionary[country.defra_isocodealpha2] = country;
                }
            }

            // Log any missing countries
            foreach (var countryCode in countryCodes)
            {
                if (!countryDictionary.ContainsKey(countryCode))
                {
                    this.logger.Log(
                        Severity.Warning,
                        nameof(ProcessINSASBMessage),
                        $"Country with ISO code '{countryCode}' not found in Dataverse.");
                }
            }

            return countryDictionary;
        }
    }
}
