using Defra.Imports.BusinessLogic;
using Defra.Imports.BusinessLogic.ImportApplication.Contexts;
using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
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
        protected Mock<IRepositoryFactory> _mockRepositoryFactory;
        protected DetermineInspectionContext _determineInspectionContext;

        public DetermineInpsectionBaseTests()
        {
            _importApplication = new defraimp_importapplication();
            _mockImportApplicationRepo = new Mock<ICrmRepository<defraimp_importapplication>>();
            _mockCoverageRulesRepo = new Mock<ICrmRepository<defraimp_inspectioncoveragerule>>();
            _mockAutoNumberRepo = new Mock<IAutonumberRepository>();
            _mockRepositoryFactory = new Mock<IRepositoryFactory>();

            _determineInspectionContext = new DetermineInspectionContext()
            {
                ImportApplication = _importApplication,
                ImportApplicationRepo = _mockImportApplicationRepo.Object,
                CoverageRulesRepo = _mockCoverageRulesRepo.Object,
                AutoNumberRepo = _mockAutoNumberRepo.Object,
                RepositoryFactory = _mockRepositoryFactory.Object
            };

            SetupRepositoryFactory();
        }

        protected void SetupRepositoryFactory()
        {
            _mockRepositoryFactory
                .Setup(r => r.GetRepository<ImportsContext, defraimp_inspectioncoveragerule>())
                .Returns(_mockCoverageRulesRepo.Object);

            _mockRepositoryFactory
                .Setup(r => r.GetRepository<ImportsContext, defraimp_importapplication>())
                .Returns(_mockImportApplicationRepo.Object);
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
