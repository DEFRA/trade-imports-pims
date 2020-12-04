namespace Defra.Imports.Tests.Integration.Dynamics.PostImportCheck.SampleRecords
{
    using System;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;

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
