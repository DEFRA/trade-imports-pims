namespace Defra.Imports.IntegrationTests.Dataverse.ImportApplication.SampleRecords
{
    using System;
    using Defra.Imports.Model;
    using Defra.Imports.Model.ReferenceData;

    class BasicImportApplication
    {
        public BasicImportApplication(Guid id)
        {
            this.ImportApplication = new defraimp_importapplication
            {
                Id = id,
                defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ImportNotification,
                defraimp_DevolvedOfficeId = Teams.EnglandTeam,
            };
        }

        public defraimp_importapplication ImportApplication { get; }
    }
}
