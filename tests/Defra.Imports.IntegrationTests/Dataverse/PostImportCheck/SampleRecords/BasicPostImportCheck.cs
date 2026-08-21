namespace Defra.Imports.IntegrationTests.Dataverse.PostImportCheck.SampleRecords
{
    using System;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;

    class BasicPostImportCheck
    {
        public BasicPostImportCheck(Guid id, EntityReference relatedImportApplication)
        {
            this.PostImportCheck = new defraimp_importinspection
            {
                Id = id,
                defraimp_RelatedImportApplication = relatedImportApplication,
            };
        }

        public defraimp_importinspection PostImportCheck { get; }
    }
}
