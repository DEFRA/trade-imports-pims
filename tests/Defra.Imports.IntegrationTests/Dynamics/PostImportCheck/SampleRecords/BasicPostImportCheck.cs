namespace Defra.Imports.IntegrationTests.Dynamics.PostImportCheck.SampleRecords
{
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using System;

    class BasicPostImportCheck
    {
        public BasicPostImportCheck(Guid id, EntityReference relatedImportApplication)
        {
            PostImportCheck = new defraimp_importinspection
            {
                Id = id,
                defraimp_RelatedImportApplication = relatedImportApplication,
            };
        }

        public defraimp_importinspection PostImportCheck { get; }
    }
}
