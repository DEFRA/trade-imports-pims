namespace Defra.Imports.BusinessLogic.ImportApplication.Contexts
{
    using Defra.Imports.BusinessLogic.RepoInterfaces;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;

    public class DetermineInspectionContext
    {
        public defraimp_importapplication ImportApplication { get; set; }

        public ICrmRepository<defraimp_importapplication> ImportApplicationRepo { get; set; }

        public ICrmRepository<defraimp_inspectioncoveragerule> CoverageRulesRepo { get; set; }

        public IAutonumberRepository AutoNumberRepo { get; set; }

        public IPlaceOfOriginRepository PlaceOfOriginRepo { get; set; }

        public IRepositoryFactory RepositoryFactory { get; set; }

        public AbstractRiskCounterManager RiskLevelCounterManager { get; set; }

        public IImportRiskCounterAuditor ImportRiskCounterAuditor { get; set; }

        public IConfigurationParameterRepository ConfigurationParameterRepo { get; set; }
    }
}
