namespace Defra.Imports.Repositories
{
    using System;
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

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
