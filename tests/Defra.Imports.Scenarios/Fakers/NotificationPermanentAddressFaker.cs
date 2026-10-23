namespace Defra.Imports.Scenarios.Fakers
{
    using System;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// A <see cref="RecordFaker{TEntity}"/> for <see cref="defraimp_notificationpermanentaddress"/> records - the child records
    /// shown on an Importer Notification's Permanent Addresses tab.
    /// </summary>
    public class NotificationPermanentAddressFaker : RecordFaker<defraimp_notificationpermanentaddress>
    {
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
            this.RuleFor(a => a.defraimp_name, f => f.Random.Int(1, 999999).ToString());
            this.RuleFor(a => a.defraimp_id, f => f.Random.Int(100000, 999999).ToString());
            this.RuleFor(a => a.defraimp_type, f => f.PickRandom(PartyTypes));
            this.RuleFor(a => a.defraimp_status, f => f.PickRandom(PartyStatuses));
            this.RuleFor(a => a.defraimp_companyname, f => f.Company.CompanyName());
            this.RuleFor(a => a.defraimp_IndividualName, f => f.Name.FullName());
            this.RuleFor(a => a.defraimp_approvalnumber, f => f.Random.AlphaNumeric(8).ToUpperInvariant());
            this.RuleFor(a => a.defraimp_otheridentifier, f => f.Random.AlphaNumeric(8).ToUpperInvariant());
            this.RuleFor(a => a.defraimp_addressline1, f => f.Address.StreetAddress());
            this.RuleFor(a => a.defraimp_addressline2, f => f.Address.SecondaryAddress());
            this.RuleFor(a => a.defraimp_addressline3, f => f.Address.County());
            this.RuleFor(a => a.defraimp_addresscity, f => f.Address.City());
            this.RuleFor(a => a.defraimp_addresspostalzipcode, f => f.Address.ZipCode());
            this.RuleFor(a => a.defraimp_AddressCountryId, f => new EntityReference(defra_country.EntityLogicalName, f.PickRandom(CountryIds)));
            this.RuleFor(a => a.defraimp_addressemail, f => f.Internet.Email());
            this.RuleFor(a => a.defraimp_addresstelephone, f => f.Random.ReplaceNumbers("01#########"));
            this.RuleFor(a => a.defraimp_addressuktelephone, f => f.Random.ReplaceNumbers("07#########"));
            this.RuleFor(a => a.defraimp_internationalphonecode, f => "44");
            this.RuleFor(a => a.defraimp_internationalphonenumber, f => f.Random.ReplaceNumbers("7###########"));
        }
    }
}
