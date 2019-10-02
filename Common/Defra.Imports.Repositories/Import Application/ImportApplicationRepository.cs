namespace Defra.Imports.Repositories
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Defra.Imports.Model;
    using Defra.Imports.BusinessLogic.RepoInterfaces;

    class ImportApplicationRepository : IImportApplicationRepository
    {
        private readonly IOrganizationService orgSvc;
        private readonly ITracingService tracingService;

        public ImportApplicationRepository(IOrganizationService svc, ITracingService service)
        {
            this.orgSvc = svc;
            this.tracingService = service;
        }

        public defraimp_importapplication GetImportApplicationWithID(Guid id, ColumnSet columnSet)
        {
            return orgSvc.Retrieve(defraimp_importapplication.EntityLogicalName, id, columnSet) as defraimp_importapplication;
        }
    }
}
