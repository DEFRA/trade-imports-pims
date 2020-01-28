using Defra.Imports.BusinessLogic.ImportApplication;
using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
using Defra.Imports.Model;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Unit.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class P3DetermineInspectionTests : DetermineInpsectionBaseTests
    {
        private P3DetermineInspection _P3DetermineInspection;

        public P3DetermineInspectionTests()
            : base()
        {
            _P3DetermineInspection = new P3DetermineInspection();
        }

        [Fact]
        public void ExecuteInspection_WithAnyValues_ShouldRetrieveTheCountOfTheP2Counter()
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockAutoNumberRepo.Verify(r => r.GetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME));

        }

        [Fact]
        public void ExecuteInspection_CurrentCountMoreThanTheCoverageRule_ResetsTheCounterToZero()
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();
            SetupAutoNumberRepoToReturnValue(ImportApplicationConstants.P3_COUNTER_NAME, 3);

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockAutoNumberRepo.Verify(r => r.SetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME, 0), Times.Once);

        }

        [Fact]
        public void ExecuteInspection_CurrentCountMoreThanTheCoverageRule_UpdatesImportApplicationInspectionRequiredToYes()
        {
            // Arrange
            SetupCurrentCountMoreThanCoverageRule();

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_importapplicationId == _importApplication.defraimp_importapplicationId)), Times.Once);
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_InspectionRequired.Value == defraimp_importapplication_defraimp_inspectionrequired.Yes)), Times.Once);

        }

        [Fact]
        public void ExecuteInspection_CurrentCountLessThanTheCoverageRule_DoesntSetTheInspectionDeclinedReason()
        {
            // Arrange
            SetupCurrentCountMoreThanCoverageRule();

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => string.IsNullOrEmpty(e.defraimp_InspectionDeclinedReason))), Times.Once);
        }

        private void SetupCurrentCountMoreThanCoverageRule()
        {
            SetupCoverageRulesRepoToReturnRules();
            SetupAutoNumberRepoToReturnValue(ImportApplicationConstants.P3_COUNTER_NAME, 3);
            _importApplication.defraimp_importapplicationId = Guid.NewGuid();
        }

        [Fact]
        public void ExecuteInspection_CurrentCountLessThanTheCoverageRule_UpdatesImportApplicationInspectionRequiredToNo()
        {
            // Arrange
            SetupCurrentCountLessThanCoverageRule();

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_importapplicationId == _importApplication.defraimp_importapplicationId)), Times.Once);
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_InspectionRequired.Value == defraimp_importapplication_defraimp_inspectionrequired.No)), Times.Once);

        }

        [Fact]
        public void ExecuteInspection_CurrentCountLessThanTheCoverageRule_SetsTheInspectionDeclinedReason()
        {
            // Arrange
            SetupCurrentCountLessThanCoverageRule();

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => !string.IsNullOrEmpty(e.defraimp_InspectionDeclinedReason))), Times.Once);
        }

        private void SetupCurrentCountLessThanCoverageRule()
        {
            SetupCoverageRulesRepoToReturnRules();
            SetupAutoNumberRepoToReturnValue(ImportApplicationConstants.P3_COUNTER_NAME, 0);
            _importApplication.defraimp_importapplicationId = Guid.NewGuid();
        }

    }
}
