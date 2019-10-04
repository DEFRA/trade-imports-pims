using System;
using System.Collections.Generic;
using System.Text;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;

namespace Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class P2DetermineInspection : IDetermineInspection
    {
        public void ExecuteInspection(defraimp_importapplication importApplication, ICrmRepository<defraimp_inspectioncoveragerule> coverageRulesRepo, IAutonumberRepository autoNumberRepo)
        {
            throw new NotImplementedException();
        }
    }
}
