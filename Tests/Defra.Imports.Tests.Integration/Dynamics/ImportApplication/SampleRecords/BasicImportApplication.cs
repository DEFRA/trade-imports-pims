using Defra.Imports.Model;
using Defra.Imports.Model.ReferenceData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Tests.Integration.Dynamics.ImportApplication.SampleRecords
{
    class BasicImportApplication
    {
        public BasicImportApplication(Guid id)
        {
            ImportApplication = new defraimp_importapplication
            {
                Id = id,
                defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.IMP,
                defraimp_DevolvedOfficeId = Teams.EnglandTeam,
            };
        }

        public defraimp_importapplication ImportApplication { get; }
    }
}
