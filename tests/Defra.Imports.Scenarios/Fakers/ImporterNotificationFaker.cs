namespace Defra.Imports.Scenarios.Fakers
{
    using System;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// A <see cref="RecordFaker{TEntity}"/> for <see cref="defraimp_ImporterNotification"/> records. Generates realistic
    /// values for the minimal set of fields common to a newly submitted IPAFFS notification - established by reviewing
    /// the representative Service Bus payloads under <c>Defra.Imports.IntegrationTests/TestData/NOTIFICATION*.json</c>
    /// (excluding amend/cancelled/error-case fixtures) and cross-referencing how each is mapped onto the Dataverse record
    /// by <c>Create_a_new_record_-_Notification</c> in <c>EUImportsNotifications.logicapp.json</c>.
    /// Also populates the Consignor (Place of Origin/hidden Consignors tab), Consignee, Importer, Transporter, Place of
    /// Destination and Person Responsible party field groups, and every country lookup, by default - grounded in the
    /// Importer Notification main FormXml's tab/control <c>datafieldname</c>s rather than the logical name alone. Country
    /// lookups reference <see cref="defra_country"/> reference data IDs known to exist in every target environment (see
    /// <c>data/core/extract/core_defra_country_1.json</c>) rather than resolving a country by ISO code at runtime.
    /// Deliberately does not set:
    /// <list type="bullet">
    /// <item><description>Commodity and species lookup fields (e.g. commodity complement) - the Logic App resolves
    /// these against Dataverse lookups rather than carrying them as plain values.</description></item>
    /// <item><description>The Charity ("consignor two") party fields - only relevant when importing from a charity,
    /// so set by <c>LogicAppSubmitsImporterNotificationEvent.Builder.WithImportingFromCharity</c> instead.</description></item>
    /// <item><description>Port of exit - present in only one of the reviewed payloads as that scenario's dedicated
    /// focus, so should be set by a dedicated builder method instead.</description></item>
    /// </list>
    /// Also sets a handful of fields not evidenced in the reviewed payloads (CPH number, means of transport from entry
    /// point, type of IMP, responsible for transport, commodities region of origin/intended for, veterinary document) in
    /// case they were added to the IPAFFS message schema after the integration test fixtures were authored.
    /// Also creates the <see cref="defraimp_commoditycomplement"/>, <see cref="defraimp_commoditycomplementparameterset"/>
    /// (with its own nested identifier/key-data-pair records) and <see cref="defraimp_ipaffsdocument"/> child records the
    /// Logic App creates alongside the notification (see <c>Create_a_new_record_-_Commodity_Complement</c>,
    /// <c>Create_a_new_record_-_Commodity_Complement_Parameter_Set</c> and <c>Create_a_new_record_-_Document</c>), via the
    /// early-bound deep-insert relationship properties, so all are created in the same Dataverse operation as the parent.
    /// </summary>
    public class ImporterNotificationFaker : RecordFaker<defraimp_ImporterNotification>
    {
        private static readonly string[] Ports = { "Dover", "Portsmouth", "Harwich", "Holyhead", "Felixstowe" };
        private static readonly string[] TransitingStates = { "NIR", "EN", "AT", "LT" };
        private static readonly string[] ImpTypes = { "POAO", "HRFNAO" };

        // Raw enum tokens per notification-schema.json's CommodityIntendedFor definition - passed through verbatim by
        // the Logic App with no dictionary conversion (unlike animalsCertifiedAs).
        private static readonly string[] CommodityIntendedForOptions = { "human", "feedingstuff", "further", "other" };
        private static readonly string[] ResponsibleForTransportOptions = { "Importer", "Exporter", "Transporter" };

        // Enum tokens per notification-schema.json's MeansOfTransport.type definition - excludes the combined
        // ("Road vehicle Aeroplane" etc.) values, which represent a less common multi-leg journey.
        private static readonly string[] MeansOfTransportTypes = { "Aeroplane", "Road Vehicle", "Railway Wagon", "Ship", "Other" };

        private static readonly defraimp_animalcertifiedas[] AnimalsCertifiedAsOptions =
        {
            defraimp_animalcertifiedas.Breeding,
            defraimp_animalcertifiedas.Pets,
            defraimp_animalcertifiedas.Commercialsale,
        };

        // Status values per notification-schema.json's EconomicOperator.status definition - shared by every party field group.
        private static readonly string[] PartyStatuses = { "approved", "nonapproved", "suspended" };

        // defra_country records guaranteed to exist in every target environment (data/core/extract/core_defra_country_1.json),
        // identified by their real defra_isocodealpha2 value rather than a guessed/placeholder record.
        private static readonly Guid[] CountryIds =
        {
            new Guid("0a9db7ed-b2d3-e911-a861-000d3ab1dad7"), // United Kingdom of Great Britain and Northern Ireland (GB)
            new Guid("be9bb7ed-b2d3-e911-a861-000d3ab1dad7"), // France (FR)
            new Guid("fe9bb7ed-b2d3-e911-a861-000d3ab1dad7"), // Republic of Ireland (IE)
            new Guid("649cb7ed-b2d3-e911-a861-000d3ab1dad7"), // Netherlands (NL)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ImporterNotificationFaker"/> class.
        /// </summary>
        /// <param name="commodityComplementFaker">
        /// The faker used to generate the commodity complement child record(s) created with the notification. Defaults to
        /// a plain <see cref="CommodityComplementFaker"/>, generating a single record - matching every reviewed payload
        /// having exactly one.
        /// </param>
        /// <param name="commodityComplementParameterSetFaker">
        /// The faker used to generate the commodity complement parameter set child record(s) created with the
        /// notification. Defaults to a plain <see cref="CommodityComplementParameterSetFaker"/>, generating a single
        /// record - matching every reviewed payload having exactly one.
        /// </param>
        /// <param name="ipaffsDocumentFaker">
        /// The faker used to generate the IPAFFS document child record(s) created with the notification. Defaults to a
        /// plain <see cref="IpaffsDocumentFaker"/>, generating two records - reflecting the typical count across the
        /// reviewed payloads that had any.
        /// </param>
        public ImporterNotificationFaker(
            CommodityComplementFaker commodityComplementFaker = null,
            CommodityComplementParameterSetFaker commodityComplementParameterSetFaker = null,
            IpaffsDocumentFaker ipaffsDocumentFaker = null)
        {
            var commodityComplements = (commodityComplementFaker ?? new CommodityComplementFaker()).Generate(1);
            var commodityComplementParameterSets = (commodityComplementParameterSetFaker ?? new CommodityComplementParameterSetFaker()).Generate(1);
            var documents = (ipaffsDocumentFaker ?? new IpaffsDocumentFaker()).Generate(2);

            this.RuleFor(n => n.defraimp_IpaffsId, f => f.Random.Int(100000, 999999));
            this.RuleFor(n => n.defraimp_submissiondate, f => f.Date.Recent(7));

            // Reference number format observed across every reviewed payload: IMP.GB.<year>.1<IPAFFS id>.
            this.RuleFor(n => n.defraimp_Name, (f, n) => $"IMP.GB.{n.defraimp_submissiondate.Value.Year}.1{n.defraimp_IpaffsId}");
            this.RuleFor(n => n.defraimp_Version, f => f.Random.Int(1, 3));

            // Every reviewed payload is an "IMP" notification - the only type the submission event represents. Status is
            // fixed to "Submitted" as that's what a newly submitted notification is; other statuses (e.g. Amend) are the
            // responsibility of dedicated builder methods for the scenarios that need them.
            this.RuleFor(n => n.defraimp_type, f => defraimp_importernotificationtype.IMP);
            this.RuleFor(n => n.defraimp_status, f => defraimp_importernotificationstatus.Submitted);

            this.RuleFor(n => n.defraimp_submittedbydisplayname, f => f.Name.FullName());
            this.RuleFor(n => n.defraimp_submittedbyuserid, f => f.Random.Guid().ToString());
            this.RuleFor(n => n.defraimp_lastupdated, (f, n) => n.defraimp_submissiondate);
            this.RuleFor(n => n.defraimp_lastupdatedbydisplayname, (f, n) => n.defraimp_submittedbydisplayname);
            this.RuleFor(n => n.defraimp_lastupdatedbyuserid, (f, n) => n.defraimp_submittedbyuserid);

            // Date-only field (Entity.xml Format: date) - no time-of-day component.
            this.RuleFor(n => n.defraimp_ArrivalDate, f => DateTime.SpecifyKind(f.Date.Soon(14).Date, DateTimeKind.Utc));
            this.RuleFor(n => n.defraimp_ArrivalTime, f => f.Date.Recent().ToString("HH:mm"));

            this.RuleFor(n => n.defraimp_cphnumber, f => f.Random.Replace("##/###/####"));
            this.RuleFor(n => n.defraimp_ImpType, f => f.PickRandom(ImpTypes));
            this.RuleFor(n => n.defraimp_portofentry, f => f.PickRandom(Ports));
            this.RuleFor(n => n.defraimp_routetransitingstates, f => f.PickRandom(TransitingStates));
            this.RuleFor(n => n.defraimp_responsiblefortransport, f => f.PickRandom(ResponsibleForTransportOptions));

            this.RuleFor(n => n.defraimp_commoditiesanimalscertifiedas, f => f.PickRandom(AnimalsCertifiedAsOptions));

            // Short region code (schema: maxLength 10) - deliberately not a full country code, to avoid confusion with
            // the country ISO code lookup fields elsewhere on this entity.
            this.RuleFor(n => n.defraimp_commoditiesregionoforigin, f => f.Random.AlphaNumeric(3).ToUpperInvariant());
            this.RuleFor(n => n.defraimp_commoditiescommodityintendedfor, f => f.PickRandom(CommodityIntendedForOptions));

            // Per notification-schema.json's commodityComplement.commodityID definition (minLength 1, maxLength 20) -
            // narrower than the Dataverse field's own 100-character limit, so bounded to the real IPAFFS constraint.
            this.RuleFor(n => n.defraimp_CommodityId, f => f.Random.String2(f.Random.Int(4, 10), "0123456789"));

            this.RuleFor(n => n.defraimp_meansoftransportfromentrypointtype, f => f.PickRandom(MeansOfTransportTypes));
            this.RuleFor(n => n.defraimp_meansoftransportfromentrypointid, f => f.Vehicle.Vin());
            this.RuleFor(n => n.defraimp_meansoftransportfromentrypointdocument, f => f.Random.Replace("DOC-#####"));

            this.RuleFor(n => n.defraimp_veterinaryinformationveterinarydocument, f => f.Random.Replace("VET-########"));

            // Only 1 of the reviewed payloads had a complex (multi-commodity) notification selected.
            this.RuleFor(n => n.defraimp_complexcommodityselected, f => f.Random.Bool(0.1f));

            this.RuleFor(n => n.defraimp_CountryofOriginId, f => new EntityReference(defra_country.EntityLogicalName, f.PickRandom(CountryIds)));
            this.RuleFor(n => n.defraimp_transitdestinationcountryid, f => new EntityReference(defra_country.EntityLogicalName, f.PickRandom(CountryIds)));

            // Consignor fields - back both the "Place of Origin" tab and the hidden "Consignors" tab (same datafieldnames on both).
            this.RuleFor(n => n.defraimp_consignorindividualname, f => f.Name.FullName());
            this.RuleFor(n => n.defraimp_consignorcompanyname, f => f.Company.CompanyName());
            this.RuleFor(n => n.defraimp_consignoraddressemail, f => f.Internet.Email());
            this.RuleFor(n => n.defraimp_consignoraddresstelephone, f => f.Phone.PhoneNumber());
            this.RuleFor(n => n.defraimp_consignoraddressuktelephone, f => f.Random.ReplaceNumbers("###########"));
            this.RuleFor(n => n.defraimp_consignorinternationalphonenumber, f => f.Random.ReplaceNumbers("##########"));
            this.RuleFor(n => n.defraimp_consignoraddressaddressline1, f => f.Address.StreetAddress());
            this.RuleFor(n => n.defraimp_consignoraddressaddressline2, f => f.Address.SecondaryAddress());
            this.RuleFor(n => n.defraimp_consignoraddressaddressline3, f => f.Address.County());
            this.RuleFor(n => n.defraimp_consignoraddresscity, f => f.Address.City());
            this.RuleFor(n => n.defraimp_consignoraddresspostalzipcode, f => f.Address.ZipCode());
            this.RuleFor(n => n.defraimp_ConsignorAddressCountryid, f => new EntityReference(defra_country.EntityLogicalName, f.PickRandom(CountryIds)));
            this.RuleFor(n => n.defraimp_consignorapprovalnumber, f => f.Random.Replace("APP-#####"));
            this.RuleFor(n => n.defraimp_consignorstatus, f => f.PickRandom(PartyStatuses));

            // Per notification-schema.json's EconomicOperator.type enum - the Place of Origin tab is backed by these
            // same Consignor fields, so "premises of origin" is the grounded type for this party.
            this.RuleFor(n => n.defraimp_consignortype, f => "premises of origin");
            this.RuleFor(n => n.defraimp_consignorotheridentifier, f => f.Random.Replace("ID-#####"));

            // Consignee fields - back the hidden "Consignee" tab.
            this.RuleFor(n => n.defraimp_consigneeindividualname, f => f.Name.FullName());
            this.RuleFor(n => n.defraimp_consigneecompanyname, f => f.Company.CompanyName());
            this.RuleFor(n => n.defraimp_consigneeaddressemail, f => f.Internet.Email());
            this.RuleFor(n => n.defraimp_consigneeaddresstelephone, f => f.Phone.PhoneNumber());
            this.RuleFor(n => n.defraimp_consigneeaddressuktelephone, f => f.Random.ReplaceNumbers("###########"));
            this.RuleFor(n => n.defraimp_consigneeinternationalphonenumber, f => f.Random.ReplaceNumbers("##########"));
            this.RuleFor(n => n.defraimp_consigneeaddressaddressline1, f => f.Address.StreetAddress());
            this.RuleFor(n => n.defraimp_consigneeaddressaddressline2, f => f.Address.SecondaryAddress());
            this.RuleFor(n => n.defraimp_consigneeaddressaddressline3, f => f.Address.County());
            this.RuleFor(n => n.defraimp_consigneeaddresscity, f => f.Address.City());
            this.RuleFor(n => n.defraimp_consigneeaddresspostalzipcode, f => f.Address.ZipCode());
            this.RuleFor(n => n.defraimp_ConsigneeAddressCountryId, f => new EntityReference(defra_country.EntityLogicalName, f.PickRandom(CountryIds)));
            this.RuleFor(n => n.defraimp_consigneeapprovalnumber, f => f.Random.Replace("APP-#####"));
            this.RuleFor(n => n.defraimp_consigneestatus, f => f.PickRandom(PartyStatuses));
            this.RuleFor(n => n.defraimp_consigneetype, f => "consignee");
            this.RuleFor(n => n.defraimp_consigneeotheridentifier, f => f.Random.Replace("ID-#####"));

            // Importer fields - back the "Importer" tab. Only Telephone (no UK/international split) is shown on this tab.
            this.RuleFor(n => n.defraimp_importercompanyname, f => f.Company.CompanyName());
            this.RuleFor(n => n.defraimp_importeraddressemail, f => f.Internet.Email());
            this.RuleFor(n => n.defraimp_importeraddresstelephone, f => f.Phone.PhoneNumber());
            this.RuleFor(n => n.defraimp_importerapprovalnumber, f => f.Random.Replace("APP-#####"));
            this.RuleFor(n => n.defraimp_importerstatus, f => f.PickRandom(PartyStatuses));
            this.RuleFor(n => n.defraimp_importertype, f => "importer");
            this.RuleFor(n => n.defraimp_importeraddressaddressline1, f => f.Address.StreetAddress());
            this.RuleFor(n => n.defraimp_importeraddressaddressline2, f => f.Address.SecondaryAddress());
            this.RuleFor(n => n.defraimp_importeraddressaddressline3, f => f.Address.County());
            this.RuleFor(n => n.defraimp_importeraddresscity, f => f.Address.City());
            this.RuleFor(n => n.defraimp_importeraddresspostalzipcode, f => f.Address.ZipCode());
            this.RuleFor(n => n.defraimp_ImporterAddressCountryid, f => new EntityReference(defra_country.EntityLogicalName, f.PickRandom(CountryIds)));

            // Transporter fields - back the "Transporter" tab's "Transporter Details" section. Only Telephone (no
            // UK/international split) is shown there.
            this.RuleFor(n => n.defraimp_transportercompanyname, f => f.Company.CompanyName());
            this.RuleFor(n => n.defraimp_transporteraddressemail, f => f.Internet.Email());
            this.RuleFor(n => n.defraimp_transporteraddresstelephone, f => f.Phone.PhoneNumber());
            this.RuleFor(n => n.defraimp_transporterapprovalnumber, f => f.Random.Replace("APP-#####"));
            this.RuleFor(n => n.defraimp_transporterstatus, f => f.PickRandom(PartyStatuses));
            this.RuleFor(n => n.defraimp_transportertype, f => "commercial transporter");
            this.RuleFor(n => n.defraimp_transporteraddressaddressline1, f => f.Address.StreetAddress());
            this.RuleFor(n => n.defraimp_transporteraddressaddressline2, f => f.Address.SecondaryAddress());
            this.RuleFor(n => n.defraimp_transporteraddressaddressline3, f => f.Address.County());
            this.RuleFor(n => n.defraimp_transporteraddresscity, f => f.Address.City());
            this.RuleFor(n => n.defraimp_transporteraddresspostalzipcode, f => f.Address.ZipCode());
            this.RuleFor(n => n.defraimp_TransporterAddressCountryid, f => new EntityReference(defra_country.EntityLogicalName, f.PickRandom(CountryIds)));

            // Place of Destination fields - back the "Place of Destination" tab.
            this.RuleFor(n => n.defraimp_placeofdestinationcompanyname, f => f.Company.CompanyName());
            this.RuleFor(n => n.defraimp_placeofdestinationaddressemail, f => f.Internet.Email());
            this.RuleFor(n => n.defraimp_placeofdestinationaddresstelephone, f => f.Phone.PhoneNumber());
            this.RuleFor(n => n.defraimp_placeofdestinationapprovalnumber, f => f.Random.Replace("APP-#####"));
            this.RuleFor(n => n.defraimp_placeofdestinationstatus, f => f.PickRandom(PartyStatuses));
            this.RuleFor(n => n.defraimp_placeofdestinationtype, f => "destination");
            this.RuleFor(n => n.defraimp_placeofdestinationaddressaddressline1, f => f.Address.StreetAddress());
            this.RuleFor(n => n.defraimp_placeofdestinationaddressaddressline2, f => f.Address.SecondaryAddress());
            this.RuleFor(n => n.defraimp_placeofdestinationaddressaddressline3, f => f.Address.County());
            this.RuleFor(n => n.defraimp_placeofdestinationaddresscity, f => f.Address.City());
            this.RuleFor(n => n.defraimp_placeofdestinationaddresspostalzipcode, f => f.Address.ZipCode());
            this.RuleFor(n => n.defraimp_PlaceofDestinationCountryid, f => new EntityReference(defra_country.EntityLogicalName, f.PickRandom(CountryIds)));
            this.RuleFor(n => n.defraimp_isplaceofdestinationthepermanentaddress, f => f.Random.Bool());

            // Person Responsible fields - back the "Person Responsible" tab (a narrower field set than the other
            // parties - no Approval Number/Status/Type, and Address is a single free-text field, not split into lines).
            this.RuleFor(n => n.defraimp_personresponsiblecompanyname, f => f.Name.FullName());
            this.RuleFor(n => n.defraimp_personresponsibleemail, f => f.Internet.Email());
            this.RuleFor(n => n.defraimp_personresponsiblephone, f => f.Phone.PhoneNumber());
            this.RuleFor(n => n.defraimp_personresponsibleaddress, f => $"{f.Address.StreetAddress()}, {f.Address.City()}, {f.Address.ZipCode()}");
            this.RuleFor(n => n.defraimp_PersonResponsibleCountryId, f => new EntityReference(defra_country.EntityLogicalName, f.PickRandom(CountryIds)));

            // Deep insert: these create the related records in the same Dataverse operation as the notification itself.
            this.RuleFor(n => n.defraimp_importernotification_defraimp_commoditycomplement_ImporterNotification, f => commodityComplements);
            this.RuleFor(n => n.defraimp_importernotification_defraimp_commoditycomplementparameterset_ImporterNotification, f => commodityComplementParameterSets);
            this.RuleFor(n => n.defraimp_defraimp_importernotification_defraimp_ipaffsdocument_ImporterNotificationId, f => documents);
        }
    }
}
