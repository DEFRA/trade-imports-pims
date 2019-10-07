using Defra.Imports.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspectionStrategies;
using Moq;
using Defra.Imports.Repositories;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.BusinessLogic.ImportApplication;
using System.Linq.Expressions;

namespace Defra.Imports.Tests.Unit.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class P3DetermineInspectionTests
    {
        private defraimp_importapplication _importApplication;
        private Mock<ICrmRepository<defraimp_importapplication>> _mockImportApplicationRepo;
        private Mock<ICrmRepository<defraimp_inspectioncoveragerule>> _mockCoverageRulesRepo;
        private Mock<IAutonumberRepository> _mockAutoNumberRepo;
        private P2DetermineInspection _p2DetermineInspection;

        public P3DetermineInspectionTests()
        {
            _importApplication = new defraimp_importapplication();
            _mockImportApplicationRepo = new Mock<ICrmRepository<defraimp_importapplication>>();
            _mockCoverageRulesRepo = new Mock<ICrmRepository<defraimp_inspectioncoveragerule>>();
            _mockAutoNumberRepo = new Mock<IAutonumberRepository>();

            _p2DetermineInspection = new P2DetermineInspection();
        }

        [Fact]
        public void ExecuteInspection_WithValidValues_CallsTheIncrementAutonumberMethodWithP2Counter()
        {
            // Arrange
            defraimp_inspectioncoveragerule inspectionRuleStub = new defraimp_inspectioncoveragerule();

            _mockCoverageRulesRepo
                .Setup(r => r.Find<defraimp_inspectioncoveragerule>(
                    It.IsAny<Expression<Func<defraimp_inspectioncoveragerule, bool>>>(), 
                    It.IsAny<Expression<Func<defraimp_inspectioncoveragerule, defraimp_inspectioncoveragerule>>>()
                ))
                .Returns(() => null);


            // Act
            _p2DetermineInspection.ExecuteInspection(_importApplication, _mockImportApplicationRepo.Object, _mockCoverageRulesRepo.Object, _mockAutoNumberRepo.Object);

            // Assert
            _mockAutoNumberRepo.Verify(r => r.IncrementAutonumber(ImportApplicationConstants.P2_COUNTER_NAME), Times.Once);

        }
    }
}
