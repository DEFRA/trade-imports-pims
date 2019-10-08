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
    public class P2DetermineInspectionTests : DetermineInpsectionBaseTests
    {
        private P2DetermineInspection _p2DetermineInspection;

        public P2DetermineInspectionTests()
            : base()
        {
            _p2DetermineInspection = new P2DetermineInspection();
        }

        [Fact]
        public void ExecuteInspection_WithValidValues_CallsTheIncrementAutonumberMethodWithP2Counter()
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();

            // Act
            _p2DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockAutoNumberRepo.Verify(r => r.IncrementAutonumber(ImportApplicationConstants.P2_COUNTER_NAME), Times.Once);

        }

        [Fact]
        public void ExecuteInspection_WhenQuotaIsMoreThanZero_ShouldDecrementQuotaOnce()
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();
            SetupAutoNumberRepoToReturnValue(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME, 1);
 
            // Act
            _p2DetermineInspection.ExecuteInspection(_determineInspectionContext);


            // Assert
            _mockAutoNumberRepo.Verify(r => r.DecrementAutonumber(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME), Times.Once);
        }

        [Fact]
        public void ExecuteInspection_WhenQuotaIsMoreThanZero_ShouldUpdateImportApplicationInspectionRequiredToYes()
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();
            SetupAutoNumberRepoToReturnValue(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME, 1);
            _importApplication.defraimp_importapplicationId = Guid.NewGuid();

            // Act
            _p2DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_InspectionRequired.Value == defraimp_importapplication_defraimp_inspectionrequired.Yes)));
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_importapplicationId.Value == _importApplication.defraimp_importapplicationId)));
        }

        [Fact]
        public void ExecuteInpsection_CurrentCountMoreThanCoverageRule_ResetsTheAutoNumberValueToZero()
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();
            SetupAutoNumberRepoToReturnValue(ImportApplicationConstants.P2_COUNTER_NAME, 3);

            // Act
            _p2DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockAutoNumberRepo.Verify(r => r.SetAutonumberValue(ImportApplicationConstants.P2_COUNTER_NAME, 0), Times.Once);
        }

        [Fact]
        public void ExecuteInpsection_CurrentCountMoreThanCoverageRule_UpdatesImportApplicationInspectionRequiredToYes()
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();
            SetupAutoNumberRepoToReturnValue(ImportApplicationConstants.P2_COUNTER_NAME, 3);
            _importApplication.defraimp_importapplicationId = Guid.NewGuid();

            // Act
            _p2DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_InspectionRequired.Value == defraimp_importapplication_defraimp_inspectionrequired.Yes)));
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_importapplicationId == _importApplication.defraimp_importapplicationId)));
        }

        [Fact]
        public void ExecuteInpsection_CurrentCountLessThanCoverageRule_UpdatesImportApplicationInspectionRequiredToNo()
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();
            SetupAutoNumberRepoToReturnValue(ImportApplicationConstants.P2_COUNTER_NAME, 0);
            _importApplication.defraimp_importapplicationId = Guid.NewGuid();

            // Act
            _p2DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_InspectionRequired.Value == defraimp_importapplication_defraimp_inspectionrequired.No)));
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_importapplicationId == _importApplication.defraimp_importapplicationId)));
        }
    }
}
