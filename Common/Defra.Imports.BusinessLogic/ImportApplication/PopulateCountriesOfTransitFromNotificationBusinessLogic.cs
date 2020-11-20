namespace Defra.Imports.BusinessLogic.ImportApplication
{
    using System;
    using System.Linq;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>
    /// A business logic class to populate transit countries relationships from an importer notification.
    /// </summary>
    public class PopulateCountriesOfTransitFromNotificationBusinessLogic
    {
        private IOrganizationService orgService;
        private readonly string IMPORT_APPLICATION_COUNTRY_RELATIONSHIP_NAME = "defraimp_defraimp_importapplication_defra_country";

        /// <summary>
        /// Initializes a new instance of the <see cref="PopulateCountriesOfTransitFromNotificationBusinessLogic"/> class.
        /// </summary>
        /// <param name="orgService">The organization service used to connect to dynamics</param>
        public PopulateCountriesOfTransitFromNotificationBusinessLogic(IOrganizationService orgService)
        {
            this.orgService = orgService;
        }

        /// <summary>
        /// Related transit countries to an import record which are populated on an importer notification.
        /// </summary>
        /// <param name="importerNotificationRef">The reference to the importer notification to populate transit countries from</param>
        /// <param name="importRecordRef">The reference to the import record to populate transit countries to</param>
        public void PopulateCountriesOfTransit(EntityReference importerNotificationRef, EntityReference importRecordRef)
        {
            this.DissasociateExistingCountries(importRecordRef);
            this.AssociateCountriesFromNotification(importerNotificationRef, importRecordRef);
        }

        private void DissasociateExistingCountries(EntityReference importRecordRef)
        {
            // Retrieve transit countries currently linked to the Import Record and remove relationship
            EntityCollection previousImportTransitCountries = this.RetrieveRelatedCountries(defraimp_importapplication_defra_country.EntityLogicalName, "defraimp_importapplicationid", importRecordRef.Id, this.orgService);
            if (previousImportTransitCountries.Entities.Any())
            {
                EntityReferenceCollection countriesToDissasociate = new EntityReferenceCollection();

                foreach (Entity existingRelationship in previousImportTransitCountries.Entities)
                {
                    Guid dissasociateCountryGuid = existingRelationship.GetAttributeValue<Guid>("defra_countryid");
                    EntityReference dissasociateCountryRef = new EntityReference(defra_country.EntityLogicalName, dissasociateCountryGuid);
                    countriesToDissasociate.Add(dissasociateCountryRef);
                }

                this.orgService.Disassociate(importRecordRef.LogicalName, importRecordRef.Id, new Relationship(IMPORT_APPLICATION_COUNTRY_RELATIONSHIP_NAME), countriesToDissasociate);
            }
        }

        private void AssociateCountriesFromNotification(EntityReference importerNotificationRef, EntityReference importRecordRef)
        {
            // Retrieve transit countries linked to the import notification and add relationship to import record
            EntityCollection notificationTransitCountries = this.RetrieveRelatedCountries(defraimp_ImporterNotification_CountriesofTransit.EntityLogicalName, "defraimp_importernotificationid", importerNotificationRef.Id, this.orgService);
            if (notificationTransitCountries.Entities.Any())
            {
                EntityReferenceCollection countriesToAssociate = new EntityReferenceCollection();

                foreach (Entity notificationRelationship in notificationTransitCountries.Entities)
                {
                    Guid associateCountryGuid = notificationRelationship.GetAttributeValue<Guid>("defra_countryid");
                    EntityReference associateCountryRef = new EntityReference(defra_country.EntityLogicalName, associateCountryGuid);
                    countriesToAssociate.Add(associateCountryRef);
                }

                this.orgService.Associate(importRecordRef.LogicalName, importRecordRef.Id, new Relationship(IMPORT_APPLICATION_COUNTRY_RELATIONSHIP_NAME), countriesToAssociate);
            }
        }

        private EntityCollection RetrieveRelatedCountries(string relationshipName, string lookupSchemaName, Guid lookupId, IOrganizationService orgSvc)
        {
            QueryExpression existingRelatedCountriesQuery = new QueryExpression(relationshipName);
            existingRelatedCountriesQuery.Criteria.AddCondition(new ConditionExpression(lookupSchemaName, ConditionOperator.Equal, lookupId));
            existingRelatedCountriesQuery.ColumnSet = new ColumnSet(new string[] { "defra_countryid" });
            EntityCollection existingRelatedCountriesCollection = orgSvc.RetrieveMultiple(existingRelatedCountriesQuery);
            return existingRelatedCountriesCollection;
        }

    }
}
