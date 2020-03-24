using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.BusinessLogic.ImportApplication.Factories;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Defra.Imports.BusinessLogic.ImportApplication.Contexts
{
    public class DetermineInspectionContext
    {
        public defraimp_importapplication ImportApplication { get; set; }
        public ICrmRepository<defraimp_importapplication> ImportApplicationRepo { get; set; }
        public ICrmRepository<defraimp_inspectioncoveragerule> CoverageRulesRepo { get; set; }
        public IAutonumberRepository AutoNumberRepo { get; set; }
        public IPlaceOfOriginRepository PlaceOfOriginRepo { get; set; }
        public IRepositoryFactory RepositoryFactory { get; set; }
        public IRiskLevelCounterManager RiskLevelCounterManager { get; set; }
    }
}
