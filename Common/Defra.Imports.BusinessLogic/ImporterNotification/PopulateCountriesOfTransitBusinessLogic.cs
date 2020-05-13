using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImporterNotification
{
    public class PopulateCountriesOfTransitBusinessLogic
    {
        private defraimp_ImporterNotification _target;
        private IOrganizationService _orgSvc;
        private ILogWriter _logger;

        public PopulateCountriesOfTransitBusinessLogic(defraimp_ImporterNotification target, IOrganizationService orgSvc, ILogWriter logger)
        {
            _target = target;
            _orgSvc = orgSvc;
            _logger = logger;
        }

        public void RunLogic()
        {
            if (_target.defraimp_routetransitingstates != null)
            {
                DissasociateExistingCountries();
                AssociateRelatedCountries();
            }
        }

        private void DissasociateExistingCountries()
        {
            //// Retrieve the existing countries
            EntityCollection existingRelatedCountriesCollection = RetrieveExistingRelatedCountries();

            // Dissasociate the existing ones
            EntityReferenceCollection countriesToDissasociate = new EntityReferenceCollection();
            foreach (Entity existingRelationship in existingRelatedCountriesCollection.Entities)
            {
                Guid dissasociateCountryGuid = existingRelationship.GetAttributeValue<Guid>("defra_countryid");
                EntityReference dissasociateCountryRef = new EntityReference(defra_country.EntityLogicalName, dissasociateCountryGuid);
                countriesToDissasociate.Add(dissasociateCountryRef);
            }
            _orgSvc.Disassociate(_target.LogicalName, _target.Id, new Relationship("defraimp_ImporterNotification_CountriesofTransit"), countriesToDissasociate);
        }

        private EntityCollection RetrieveExistingRelatedCountries()
        {
            QueryExpression existingRelatedCountriesQuery = new QueryExpression("defraimp_importernotification_countriesoftransit");
            existingRelatedCountriesQuery.Criteria.AddCondition(new ConditionExpression("defraimp_importernotificationid", ConditionOperator.Equal, _target.Id));
            existingRelatedCountriesQuery.ColumnSet = new ColumnSet(new string[] { "defra_countryid", "defraimp_importernotificationid" });
            EntityCollection existingRelatedCountriesCollection = _orgSvc.RetrieveMultiple(existingRelatedCountriesQuery);
            return existingRelatedCountriesCollection;
        }

        private void AssociateRelatedCountries()
        {
            // Split the ISO code of the countries from the target entity
            string[] countryISOCodes = _target.defraimp_routetransitingstates.Split(',');

            if(countryISOCodes.Length > 0)
            {
                // Retrieve the countries
                var countryReferences = GetCountryReferencesFromISOCodes(countryISOCodes);

                EntityReferenceCollection countryRefCollection = new EntityReferenceCollection(countryReferences.ToList());

                // Link the countries to the current target
                _orgSvc.Associate(_target.LogicalName, _target.Id, new Relationship("defraimp_ImporterNotification_CountriesofTransit"), countryRefCollection);
            }
        }

        private IEnumerable<EntityReference> GetCountryReferencesFromISOCodes(string[] countryISOCodes)
        {
            QueryExpression qe = BuildCountryQuery(countryISOCodes);
            EntityCollection countryCollection = _orgSvc.RetrieveMultiple(qe);
            var countryEntities = countryCollection.Entities;
            var countryReferences = countryEntities.Select(e => new EntityReference(e.LogicalName, e.Id));
            return countryReferences;
        }

        private QueryExpression BuildCountryQuery(string[] countryISOCodes)
        {
            QueryExpression qe = new QueryExpression("defra_country");
            qe.ColumnSet = new ColumnSet(new string[] { "defra_countryid" });

            FilterExpression filterEx = new FilterExpression(LogicalOperator.Or);
            foreach (string country in countryISOCodes)
            {
                string sanitizedCountry = country.Trim();
                if(sanitizedCountry.Length == 2)
                {
                    filterEx.AddCondition(new ConditionExpression("defra_isocodealpha2", ConditionOperator.Equal, sanitizedCountry));
                }
                else if(sanitizedCountry.Length == 3)
                {
                    filterEx.AddCondition(new ConditionExpression("defra_isocodealpha3", ConditionOperator.Equal, sanitizedCountry));
                }
                else
                {
                    filterEx.AddCondition(new ConditionExpression("defra_name", ConditionOperator.Equal, sanitizedCountry));
                }
            }
            qe.Criteria = filterEx;

            return qe;
        }
    }
}
