namespace Defra.Imports.Scenarios.Fakers
{
    using System;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// A <see cref="RecordFaker{TEntity}"/> for <c>defraimp_notificationpermanentaddress</c> records - the child records
    /// shown on an Importer Notification's Permanent Addresses tab.
    /// </summary>
    /// <remarks>
    /// Late-bound because the table is not yet in the early-bound model. Once it is, switch this to the generated class.
    /// </remarks>
    public class NotificationPermanentAddressFaker : RecordFaker<Entity>
    {
        /// <summary>
        /// The logical name of the table.
        /// </summary>
        public const string EntityLogicalName = "defraimp_notificationpermanentaddress";

        /// <summary>
        /// The schema name of the one-to-many relationship from the Importer Notification, used to deep insert these records.
        /// </summary>
        public const string ImporterNotificationRelationshipName = "defraimp_defraimp_importernotification_defraimp_notificationpermanentaddress_ImporterNotificationId";

        private static readonly string[] PartyTypes = { "commercial", "private" };
        private static readonly string[] PartyStatuses = { "approved", "nonapproved", "suspended" };

        // Same defra_country reference records as ImporterNotificationFaker (data/core/extract/core_defra_country_1.json).
        private static readonly Guid[] CountryIds =
        {
            new Guid("0a9db7ed-b2d3-e911-a861-000d3ab1dad7"), // United Kingdom of Great Britain and Northern Ireland (GB)
            new Guid("be9bb7ed-b2d3-e911-a861-000d3ab1dad7"), // France (FR)
            new Guid("fe9bb7ed-b2d3-e911-a861-000d3ab1dad7"), // Republic of Ireland (IE)
            new Guid("649cb7ed-b2d3-e911-a861-000d3ab1dad7"), // Netherlands (NL)
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationPermanentAddressFaker"/> class.
        /// </summary>
        public NotificationPermanentAddressFaker()
        {
            // Value lengths respect the table's column limits (e.g. UK telephone 11, international code 3, international number 12).
            this.CustomInstantiator(f =>
            {
                var address = new Entity(EntityLogicalName);

                address["defraimp_name"] = f.Random.Int(1, 999999).ToString();
                address["defraimp_id"] = f.Random.Int(100000, 999999).ToString();
                address["defraimp_type"] = f.PickRandom(PartyTypes);
                address["defraimp_status"] = f.PickRandom(PartyStatuses);
                address["defraimp_companyname"] = f.Company.CompanyName();
                address["defraimp_individualname"] = f.Name.FullName();
                address["defraimp_approvalnumber"] = f.Random.AlphaNumeric(8).ToUpperInvariant();
                address["defraimp_otheridentifier"] = f.Random.AlphaNumeric(8).ToUpperInvariant();
                address["defraimp_addressline1"] = f.Address.StreetAddress();
                address["defraimp_addressline2"] = f.Address.SecondaryAddress();
                address["defraimp_addressline3"] = f.Address.County();
                address["defraimp_addresscity"] = f.Address.City();
                address["defraimp_addresspostalzipcode"] = f.Address.ZipCode();
                address["defraimp_addresscountryid"] = new EntityReference(defra_country.EntityLogicalName, f.PickRandom(CountryIds));
                address["defraimp_addressemail"] = f.Internet.Email();
                address["defraimp_addresstelephone"] = f.Random.ReplaceNumbers("01#########");
                address["defraimp_addressuktelephone"] = f.Random.ReplaceNumbers("07#########");
                address["defraimp_internationalphonecode"] = "44";
                address["defraimp_internationalphonenumber"] = f.Random.ReplaceNumbers("7###########");

                return address;
            });

            // A late-bound entity needs its primary key attribute set explicitly for the Id to be honoured on create.
            this.FinishWith((f, e) => e["defraimp_notificationpermanentaddressid"] = e.Id);
        }
    }
}
