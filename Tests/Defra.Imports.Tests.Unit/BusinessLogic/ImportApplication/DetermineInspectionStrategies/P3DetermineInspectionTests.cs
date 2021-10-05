using Defra.Imports.BusinessLogic.ImportApplication;
using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
using Defra.Imports.Model;
using Microsoft.Xrm.Sdk;
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
        public void ExecuteInspection_TypeOfItahcAndPrimaryItahc_ShouldRetrieveTheCountOfTheP3CounterAndUpdateCountedToTrue()
        {
            // Arrange
            _importApplication.defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC;
            EntityReference itahcEntityRef = new EntityReference(defraimp_itahc.EntityLogicalName, Guid.NewGuid());
            _importApplication.defraimp_PrimaryITAHCId = itahcEntityRef;

            SetupCoverageRulesRepoToReturnRules();
            SetupConfigurationParameterRepoToReturnTracesEnabled("True");
            SetupP3AutonumberRepo(1);
            SetupRiskLevelCounterManager();

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockAutoNumberRepo.Verify(r => r.GetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME));
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(o => o.defraimp_ImportRecordCounted == true)));
        }

        [Fact]
        public void ExecuteInspection_TypeOfItahcLandbridgeAndPrimaryItahc_ShouldRetrieveTheCountOfTheP3CounterAndUpdateCountedToTrue()
        {
            // Arrange
            _importApplication.defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHCLandbridge;
            EntityReference itahcEntityRef = new EntityReference(defraimp_itahc.EntityLogicalName, Guid.NewGuid());
            _importApplication.defraimp_PrimaryITAHCId = itahcEntityRef;

            SetupCoverageRulesRepoToReturnRules();
            SetupConfigurationParameterRepoToReturnTracesEnabled("True");
            SetupP3AutonumberRepo(1);
            SetupRiskLevelCounterManager();

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockAutoNumberRepo.Verify(r => r.GetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME));
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>(o => o.defraimp_ImportRecordCounted == true)));
        }

        [Fact]
        public void ExecuteInspection_CountHigherThanRule_ShouldUpdateImportApplicationToInspectionRequired()
        {
            // Arrange
            _importApplication.defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC;
            EntityReference itahcEntityRef = new EntityReference(defraimp_itahc.EntityLogicalName, Guid.NewGuid());
            _importApplication.defraimp_PrimaryITAHCId = itahcEntityRef;

            SetupCoverageRulesRepoToReturnRules();
            SetupConfigurationParameterRepoToReturnTracesEnabled("True");
            SetupP3AutonumberRepo(3);
            SetupRiskLevelCounterManager();

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(
                r => r.Update(
                    It.Is<defraimp_importapplication>(o => o.defraimp_InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.Yes)
                )
            );
        }

        [Fact]
        public void ExecuteInspection_QuotaEquals0_ShouldUpdateImportApplicationToNoInspectionRequired()
        {
            // Arrange
            _importApplication.defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC;
            EntityReference itahcEntityRef = new EntityReference(defraimp_itahc.EntityLogicalName, Guid.NewGuid());
            _importApplication.defraimp_PrimaryITAHCId = itahcEntityRef;

            SetupCoverageRulesRepoToReturnRules();
            SetupConfigurationParameterRepoToReturnTracesEnabled("True");
            SetupP3AutonumberRepo(1);
            SetupRiskLevelCounterManager();

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(
                r => r.Update(
                    It.Is<defraimp_importapplication>(o => o.defraimp_InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.No)
                )
            );

        }

        [Fact]
        public void ExecuteInspection_QuotaEqualsMoreThan0_ShouldUpdateImportApplicationToInspectionRequired()
        {
            // Arrange
            _importApplication.defraimp_ImportApplicationType = defraimp_importapplication_defraimp_importapplicationtype.ITAHC;
            EntityReference itahcEntityRef = new EntityReference(defraimp_itahc.EntityLogicalName, Guid.NewGuid());
            _importApplication.defraimp_PrimaryITAHCId = itahcEntityRef;

            SetupCoverageRulesRepoToReturnRules();
            SetupConfigurationParameterRepoToReturnTracesEnabled("True");
            SetupP3AutonumberRepo(1);
            SetupP3QuotaAutonumberRepo(1);

            SetupRiskLevelCounterManager();

            // Act
            _P3DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(
                r => r.Update(
                    It.Is<defraimp_importapplication>(o => o.defraimp_InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.Yes)
                )
            );
        }

        private void SetupCurrentCountMoreThanCoverageRule()
        {
            SetupCoverageRulesRepoToReturnRules();
            SetupAutoNumberRepoToReturnValue(ImportApplicationConstants.P3_COUNTER_NAME, 3);
            _importApplication.defraimp_importapplicationId = Guid.NewGuid();
        }

        private void SetupConfigurationParameterRepoToReturnTracesEnabled(string tracesEnabled)
        {
            _mockConfigurationParameterRepo
                .Setup(r => r.GetConfigurationParameterValueByKey("defraimp_traces_enabled"))
                .Returns(tracesEnabled);
        }

        private void SetupP3AutonumberRepo(int currentCount)
        {
            _mockAutoNumberRepo.Setup(r => r.GetAutonumberValue(ImportApplicationConstants.P3_COUNTER_NAME)).Returns(currentCount);
            defraimp_autonumber autoNumberStub = new defraimp_autonumber()
            {
                defraimp_autonumberId = Guid.NewGuid(),
                defraimp_CurrentNumber = currentCount,
                defraimp_Key = ImportApplicationConstants.P3_QUOTA_COUNTER_NAME,
                defraimp_name = "P3 Counter"
            };
            _mockAutoNumberRepo.Setup(r => r.GetAutonumberWithKey(ImportApplicationConstants.P3_COUNTER_NAME)).Returns(autoNumberStub);
        }

        private void SetupP3QuotaAutonumberRepo(int quotaCount)
        {
            _mockAutoNumberRepo.Setup(r => r.GetAutonumberValue(ImportApplicationConstants.P3_QUOTA_COUNTER_NAME)).Returns(1);
            defraimp_autonumber autoNumberStub = new defraimp_autonumber()
            {
                defraimp_autonumberId = Guid.NewGuid(),
                defraimp_CurrentNumber = 1,
                defraimp_Key = ImportApplicationConstants.P3_QUOTA_COUNTER_NAME,
                defraimp_name = "P3 Quota Counter"
            };
            _mockAutoNumberRepo.Setup(r => r.GetAutonumberWithKey(ImportApplicationConstants.P3_QUOTA_COUNTER_NAME)).Returns(autoNumberStub);
        }

        private void SetupRiskLevelCounterManager()
        {
            _determineInspectionContext.RiskLevelCounterManager = new AutonumberRiskCounterManager(
            _determineInspectionContext.ImportApplicationRepo,
            _determineInspectionContext.AutoNumberRepo,
            "P3",
            _determineInspectionContext.CoverageRulesRepo,
            _logWriter.Object);

            _determineInspectionContext.RiskLevelCounterManager.CounterTransactionEvent += (CounterTransactionDetail transactionDetails) => { };
        }

        private void SetupCurrentCountLessThanCoverageRule()
        {
            SetupCoverageRulesRepoToReturnRules();
            SetupAutoNumberRepoToReturnValue(ImportApplicationConstants.P3_COUNTER_NAME, 0);
            _importApplication.defraimp_importapplicationId = Guid.NewGuid();
        }

    }
}
