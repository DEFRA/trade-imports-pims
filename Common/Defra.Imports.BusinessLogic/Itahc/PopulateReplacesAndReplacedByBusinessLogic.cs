using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Defra.Imports.BusinessLogic.Itahc
{
    public class PopulateReplacesAndReplacedByBusinessLogic
    {
        private ICrmRepository _certificateRepo;
        private Entity _targetEntity;

        public PopulateReplacesAndReplacedByBusinessLogic(ICrmRepository certificateRepo, Entity targetEntity)
        {
            _certificateRepo = certificateRepo;
            _targetEntity = targetEntity;
        }

        public void RunLogic()
        {
            AddCertificateLookupByReferenceField("defraimp_replacedreferencenumber", "defraimp_replacedbyid");
            AddCertificateLookupByReferenceField("defraimp_replacingreferencenumber", "defraimp_replacesid");
        }

        private void AddCertificateLookupByReferenceField(string referenceNumberField, string lookupField)
        {
            if (_targetEntity.Attributes.Contains(referenceNumberField))
            {
                // Retrieve the itahc with the certificate reference number of the itahc in the replaces field
                Entity retrievedCert = this.RetrieveCertificateForReferenceNumber(_targetEntity.GetAttributeValue<string>(referenceNumberField));

                if (retrievedCert != null)
                {
                    _targetEntity[lookupField] = new EntityReference(retrievedCert.LogicalName, retrievedCert.Id);
                }
            }
        }

        private Entity RetrieveCertificateForReferenceNumber(string referenceNumber)
        {
            return _certificateRepo.Find(
                    e => ((string)e["defraimp_name"]) == referenceNumber,
                    e => new Entity() { Id = e.Id }
                ).FirstOrDefault();
        }

    }
}
