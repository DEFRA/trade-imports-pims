using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
using Defra.Imports.Model;
using Moq;
using System;
using Xunit;

namespace Defra.Imports.UnitTests.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class UnkownDetermineInspectionTests : DetermineInpsectionBaseTests
    {
        private UnknownDetermineInspection _unknownDetermineInspection;

        public UnkownDetermineInspectionTests()
            : base()
        {
            _unknownDetermineInspection = new UnknownDetermineInspection();
        }

        [Fact]
        public void ExecuteInspection_AnImportApplication_UpdatesTheImportApplicationInspectionRequiredToUndetermined()
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();
            _importApplication.defraimp_importapplicationId = Guid.NewGuid();

            // Act
            _unknownDetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_InspectionRequired.Value == defraimp_importapplication_defraimp_inspectionrequired.Undetermined)));
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(e => e.defraimp_importapplicationId.Value == _importApplication.defraimp_importapplicationId)));

        }

    }
}
