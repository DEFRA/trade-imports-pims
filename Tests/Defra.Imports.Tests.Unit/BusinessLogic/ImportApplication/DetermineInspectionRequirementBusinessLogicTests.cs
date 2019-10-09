using Defra.Imports.BusinessLogic.ImportApplication;
using Defra.Imports.BusinessLogic.Logging;
using Defra.Imports.BusinessLogic.RepoInterfaces;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Unit.BusinessLogic.ImportApplication
{
    public class DetermineInspectionRequirementBusinessLogicTests
    {
        private defraimp_importapplication _importApplication;
        private Mock<ICrmRepository<defraimp_importapplication>> _mockImportApplicationRepo;
        private Mock<ICrmRepository<defraimp_inspectioncoveragerule>> _mockCoverageRulesRepo;
        private Mock<IAutonumberRepository> _mockAutoNumberRepo;
        private Mock<ILogWriter> _logWriter;
        private DetermineInspectionRequirementBusinessLogic _determineInspectionRequirementBusinessLogic;

        public DetermineInspectionRequirementBusinessLogicTests()
        {
            _importApplication = new defraimp_importapplication();
            _mockImportApplicationRepo = new Mock<ICrmRepository<defraimp_importapplication>>();
            _mockCoverageRulesRepo = new Mock<ICrmRepository<defraimp_inspectioncoveragerule>>();
            _mockAutoNumberRepo = new Mock<IAutonumberRepository>();
            _logWriter = new Mock<ILogWriter>();

            _determineInspectionRequirementBusinessLogic = new DetermineInspectionRequirementBusinessLogic(_importApplication, _mockImportApplicationRepo.Object, _mockCoverageRulesRepo.Object, _mockAutoNumberRepo.Object, _logWriter.Object); 
        }

        [Fact]
        public void RunLogic_InitialUpdateOfRiskLevel_IncrementsTheP3Counter()
        {
            // Arrange

            // Act
            _determineInspectionRequirementBusinessLogic.RunLogic();

            // Assert
            _mockAutoNumberRepo.Verify(r => r.IncrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME), Times.Once);
        }

        [Fact]
        public void RunLogic_PreviousRiskLevelP2AndCurrentRiskLevelNotP2AndNotFlaggedForInspection_DecrementsTheP2QuotaCounter()
        {
            // Arrange
            _importApplication.defraimp_PreviousImportRiskLevelId = new EntityReference(defraimp_importrisklevel.EntityLogicalName, Guid.NewGuid());
            _importApplication.defraimp_PreviousImportRiskLevelId.Name = "P2";

            // Act
            _determineInspectionRequirementBusinessLogic.RunLogic();

            // Assert
            _mockAutoNumberRepo.Verify(r => r.DecrementAutonumber(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME));

        }

        [Fact]
        public void RunLogic_PreviousRiskLevelP2AndCurrentRiskLevelNotP2AndFlaggedForInspection_IncrementsTheP2QuotaCounter()
        {
            // Arrange
            _importApplication.defraimp_PreviousImportRiskLevelId = new EntityReference(defraimp_importrisklevel.EntityLogicalName, Guid.NewGuid());
            _importApplication.defraimp_PreviousImportRiskLevelId.Name = "P2";
            _importApplication.defraimp_InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;

            // Act
            _determineInspectionRequirementBusinessLogic.RunLogic();

            // Assert
            _mockAutoNumberRepo.Verify(r => r.IncrementAutonumber(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME));

        }

    }
}
