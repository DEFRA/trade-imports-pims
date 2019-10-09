using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Tests.Unit.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class DetermineInpsectionBaseTests
    {
        protected defraimp_importapplication _importApplication;
        protected Mock<ICrmRepository<defraimp_importapplication>> _mockImportApplicationRepo;
        protected Mock<ICrmRepository<defraimp_inspectioncoveragerule>> _mockCoverageRulesRepo;
        protected Mock<IAutonumberRepository> _mockAutoNumberRepo;
        protected DetermineInspectionContext _determineInspectionContext;

        public DetermineInpsectionBaseTests()
        {
            _importApplication = new defraimp_importapplication();
            _mockImportApplicationRepo = new Mock<ICrmRepository<defraimp_importapplication>>();
            _mockCoverageRulesRepo = new Mock<ICrmRepository<defraimp_inspectioncoveragerule>>();
            _mockAutoNumberRepo = new Mock<IAutonumberRepository>();

            _determineInspectionContext = new DetermineInspectionContext()
            {
                ImportApplication = _importApplication,
                ImportApplicationRepo = _mockImportApplicationRepo.Object,
                CoverageRulesRepo = _mockCoverageRulesRepo.Object,
                AutoNumberRepo = _mockAutoNumberRepo.Object,
            };
        }

        protected void SetupCoverageRulesRepoToReturnRules()
        {
            defraimp_inspectioncoveragerule inspectionRuleStub = new defraimp_inspectioncoveragerule()
            {
                defraimp_NumberOfRecordsUntilInspection = 2
            };
            List<defraimp_inspectioncoveragerule> rules = new List<defraimp_inspectioncoveragerule>();
            rules.Add(inspectionRuleStub);

            _mockCoverageRulesRepo
                .Setup(r => r.Find<defraimp_inspectioncoveragerule>(
                    It.IsAny<Expression<Func<defraimp_inspectioncoveragerule, bool>>>(),
                    It.IsAny<Expression<Func<defraimp_inspectioncoveragerule, defraimp_inspectioncoveragerule>>>()
                ))
                .Returns(() => rules.AsQueryable());
        }

        protected void SetupAutoNumberRepoToReturnValue(string autoNumberRecordName, int returnValue)
        {
            _mockAutoNumberRepo
                .Setup(r => r.GetAutonumberValue(autoNumberRecordName)).Returns(() => returnValue);
        }
    }
}
