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
        private ICrmRepository<defraimp_itahc> _itahcRepo;
        private defraimp_itahc _targetEntity;

        public PopulateReplacesAndReplacedByBusinessLogic(ICrmRepository<defraimp_itahc> itahcRepo, defraimp_itahc targetEntity)
        {
            _itahcRepo = itahcRepo;
            _targetEntity = targetEntity;
        }

        public void RunLogic()
        {
            if (_targetEntity.defraimp_ReplacedReferenceNumber != null)
            {
                // Retrieve the itahc with the certificate reference number of the itahc in the replaced field
                defraimp_itahc replacedItahc = this.RetrieveItahcForReferenceNumber(_targetEntity.defraimp_ReplacedReferenceNumber);

                if (replacedItahc != null)
                {
                    _targetEntity.defraimp_ReplacedById = new EntityReference(defraimp_itahc.EntityLogicalName, replacedItahc.defraimp_itahcId.Value);
                }
            }

            if (_targetEntity.defraimp_ReplacingReferenceNumber != null)
            {
                // Retrieve the itahc with the certificate reference number of the itahc in the replaces field
                defraimp_itahc replacingItahc = this.RetrieveItahcForReferenceNumber(_targetEntity.defraimp_ReplacingReferenceNumber);

                if (replacingItahc != null)
                {
                    _targetEntity.defraimp_ReplacesId = new EntityReference(defraimp_itahc.EntityLogicalName, replacingItahc.defraimp_itahcId.Value);
                }
            }
        }

        private defraimp_itahc RetrieveItahcForReferenceNumber(string referenceNumber)
        {
            return _itahcRepo.Find(
                    e => e.defraimp_name == referenceNumber,
                    e => new defraimp_itahc() { defraimp_itahcId = e.defraimp_itahcId }
                ).FirstOrDefault();
        }

    }
}
