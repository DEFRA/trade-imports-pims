namespace Defra.Imports.Repositories
{
    using System;
    using System.Linq;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;

    class PlaceOfOriginRepository : IPlaceOfOriginRepository
    {
        private readonly IOrganizationService orgSvc;
        private readonly ITracingService tracingService;

        public PlaceOfOriginRepository(IOrganizationService svc)
        {
            this.orgSvc = svc;
        }

        public int GetApplicationCounterValue(Guid placeOfOriginId)
        {
            defraimp_placeoforigin placeOfOriginRecord = Find(placeOfOriginId);
            if (placeOfOriginRecord != null)
            {
                int value = placeOfOriginRecord.defraimp_ApplicationCounter ?? 0;
                return value;
            }
            else
            {
                return 0;
            }

        }

        public int GetQuotaCounterValue(Guid placeOfOriginId)
        {
            defraimp_placeoforigin placeOfOriginRecord = Find(placeOfOriginId);
            if (placeOfOriginRecord != null)
            {
                int value = placeOfOriginRecord.defraimp_InspectionQuotaCounter ?? 0;
                return value;
            }
            else
            {
                return 0;
            }
        }

        public int GetHealthCertificateCounterValue(Guid placeOfOriginId)
        {
            defraimp_placeoforigin placeOfOriginRecord = Find(placeOfOriginId);
            if (placeOfOriginRecord != null)
            {
                int value = placeOfOriginRecord.defraimp_NumberofApplications ?? 0;
                return value;
            }
            else
            {
                return 0;
            }
        }


        public int GetNumberOfRecordsSinceLastCheckValue(Guid placeOfOriginId)
        {
            defraimp_placeoforigin placeOfOriginRecord = Find(placeOfOriginId);
            if (placeOfOriginRecord != null)
            {
                int value = placeOfOriginRecord.defraimp_numberofapplicationssincelastinspection ?? 0;
                return value;
            }
            else
            {
                return 0;
            }
        }

        public defraimp_placeoforigin Find(Guid placeOfOriginId)
        {
            if (orgSvc != null)
            {
                QueryExpression query = new QueryExpression(defraimp_placeoforigin.EntityLogicalName);
                query.ColumnSet = new ColumnSet(true);
                ConditionExpression keyCondition = new ConditionExpression("defraimp_placeoforiginid", ConditionOperator.Equal, placeOfOriginId);
                query.Criteria.AddCondition(keyCondition);

                defraimp_placeoforigin placeOfOrigin = orgSvc.RetrieveMultiple(query).Entities.FirstOrDefault() as defraimp_placeoforigin;

                return placeOfOrigin;
            }
            else
            {
                return null;
            }
        }

        public void IncrementApplicationCounter(Guid placeOfOriginId)
        {
            int currentCounterValue = GetApplicationCounterValue(placeOfOriginId);
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_ApplicationCounter = currentCounterValue + 1,

            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }

        public void SetApplicationCounter(Guid placeOfOriginId, int value)
        {
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_ApplicationCounter = value,

            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }

        public void DecrementApplicationCounter(Guid placeOfOriginId)
        {
            int currentCounterValue = GetApplicationCounterValue(placeOfOriginId);
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_ApplicationCounter = currentCounterValue - 1,

            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }

        public void IncrementQuotaCounter(Guid placeOfOriginId)
        {
            int currentCounterValue = GetQuotaCounterValue(placeOfOriginId);
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_InspectionQuotaCounter = currentCounterValue + 1,

            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }

        public void DecrementQuotaCounter(Guid placeOfOriginId)
        {
            int currentCounterValue = GetQuotaCounterValue(placeOfOriginId);
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_InspectionQuotaCounter = currentCounterValue - 1,

            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }


        public void SetQuotaCounter(Guid placeOfOriginId, int value)
        {
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_InspectionQuotaCounter = value,
            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }

        public void IncrementHealthCertificateCounter(Guid placeOfOriginId)
        {
            int currentCounterValue = GetHealthCertificateCounterValue(placeOfOriginId);
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_NumberofApplications = currentCounterValue + 1,

            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }

        public void DecrementHealthCertificateCounter(Guid placeOfOriginId)
        {
            int currentCounterValue = GetHealthCertificateCounterValue(placeOfOriginId);
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_NumberofApplications = currentCounterValue - 1,

            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }

        public void SetNumberOfRecordsSinceLastCheckValue(Guid placeOfOriginId, int value)
        {
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_numberofapplicationssincelastinspection = value,
            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }

        public void IncrementNumberOfRecordsSinceLastCheck(Guid placeOfOriginId)
        {
            int currentCounterValue = GetNumberOfRecordsSinceLastCheckValue(placeOfOriginId);
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_numberofapplicationssincelastinspection = currentCounterValue + 1,
            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }

        public void DecrementNumberOfRecordsSinceLastCheck(Guid placeOfOriginId)
        {
            int currentCounterValue = GetNumberOfRecordsSinceLastCheckValue(placeOfOriginId);
            defraimp_placeoforigin updatedPlaceOfOriginRecord = new defraimp_placeoforigin
            {
                Id = placeOfOriginId,
                defraimp_numberofapplicationssincelastinspection = currentCounterValue - 1,
            };
            orgSvc.Update(updatedPlaceOfOriginRecord);
        }
    }
}
