using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using System.Linq;

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
            AddCertificateLookupByReferenceField("defraimp_replacedreferencenumber", "defraimp_replacedbyid", "defraimp_replacesid");
            AddCertificateLookupByReferenceField("defraimp_replacingreferencenumber", "defraimp_replacesid", "defraimp_replacedbyid");
        }

        private void AddCertificateLookupByReferenceField(string referenceNumberField, string lookupField, string retrievedEntityLookupField)
        {
            if (_targetEntity.Attributes.Contains(referenceNumberField))
            {
                // Retrieve the itahc with the certificate reference number of the itahc in the replaces field
                Entity retrievedCert = this.RetrieveCertificateForReferenceNumber(_targetEntity.GetAttributeValue<string>(referenceNumberField));

                if (retrievedCert != null)
                {
                    _targetEntity[lookupField] = new EntityReference(retrievedCert.LogicalName, retrievedCert.Id);

                    // Update the target
                    Entity targetUpateInfo = new Entity(_targetEntity.LogicalName, _targetEntity.Id);
                    targetUpateInfo[lookupField] = new EntityReference(retrievedCert.LogicalName, retrievedCert.Id);
                    _certificateRepo.Update(targetUpateInfo);

                    // Update the retrieved entity
                    Entity retrievedEntityUpdateInfo = new Entity(_targetEntity.LogicalName, retrievedCert.Id);
                    retrievedEntityUpdateInfo[retrievedEntityLookupField] = _targetEntity.ToEntityReference();
                    _certificateRepo.Update(retrievedEntityUpdateInfo);
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
