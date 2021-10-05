namespace Defra.Imports.Tests.Integration.Dynamics.ImportApplication.SampleRecords
{
    using System;
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;

    class BasicImportApplication
    {
        public BasicImportApplication(Guid id)
        {
            ImportApplication = new defraimp_importapplication
            {
                Id = id,
                defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ImportNotification,
                defraimp_DevolvedOfficeId = Teams.EnglandTeam,
            };
        }

        public defraimp_importapplication ImportApplication { get; }
    }
}
