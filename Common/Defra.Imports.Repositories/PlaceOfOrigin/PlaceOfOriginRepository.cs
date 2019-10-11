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

        public int GetCounterValue(Guid placeOfOriginId)
        {
            throw new NotImplementedException();
        }

        public void DecrementAutonumber(Guid placeOfOriginId)
        {
            throw new NotImplementedException();
        }

        public defraimp_placeoforigin GetPlaceOfOrigin(Guid placeOfOriginId)
        {
            throw new NotImplementedException();
        }

        public void IncrementAutonumber(Guid placeOfOriginId)
        {
            throw new NotImplementedException();
        }

        public void SetAutonumberValue(Guid placeOfOriginId)
        {
            throw new NotImplementedException();
        }
    }
}
