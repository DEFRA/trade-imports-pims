//using Defra.Imports.BusinessLogic;
//using Defra.Imports.BusinessLogic.ImportApplication;
//using Defra.Imports.BusinessLogic.Logging;
//using Defra.Imports.BusinessLogic.RepoInterfaces;
//using Defra.Imports.Model;
//using Defra.Imports.Repositories;
//using Microsoft.Xrm.Sdk;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using Xunit;

//namespace Defra.Imports.Tests.Unit.BusinessLogic.ImportApplication
//{
//    public class DetermineInspectionRequirementBusinessLogicTests
//    {
//        private defraimp_importapplication _preImportApplication;
//        private defraimp_importapplication _postImportApplication;
//        private Mock<ICrmRepository<defraimp_importapplication>> _mockImportApplicationRepo;
//        private Mock<ICrmRepository<defraimp_inspectioncoveragerule>> _mockCoverageRulesRepo;
//        private Mock<ICrmRepository<defraimp_importrisklevel>> _mockImportRiskLevelRepo;
//        private Mock<IAutonumberRepository> _mockAutoNumberRepo;
//        private Mock<IPlaceOfOriginRepository> _mockPlaceOfOriginRepo;
//        private Mock<IRepositoryFactory> _mockRepositoryFactory;
//        private Mock<ILogWriter> _logWriter;
//        private DetermineInspectionRequirementBusinessLogic _determineInspectionRequirementBusinessLogic;

//        public DetermineInspectionRequirementBusinessLogicTests()
//        {
//            _preImportApplication = new defraimp_importapplication();
//            _postImportApplication = new defraimp_importapplication();
//            _mockImportApplicationRepo = new Mock<ICrmRepository<defraimp_importapplication>>();
//            _mockCoverageRulesRepo = new Mock<ICrmRepository<defraimp_inspectioncoveragerule>>();
//            _mockImportRiskLevelRepo = new Mock<ICrmRepository<defraimp_importrisklevel>>();
//            _mockAutoNumberRepo = new Mock<IAutonumberRepository>();
//            _mockPlaceOfOriginRepo = new Mock<IPlaceOfOriginRepository>();
//            _mockRepositoryFactory = new Mock<IRepositoryFactory>();
//            _logWriter = new Mock<ILogWriter>();

//            new DetermineInspectionRequirementBusinessLogic(_preImportApplication, _postImportApplication, _mockRepositoryFactory.Object, _logWriter.Object);
//        }

//        [Fact]
//        public void RunLogic_InitialUpdateOfRiskLevel_DoesNotIncrementTheP3Counter()
//        {
//            // Arrange

//            // Act
//            _determineInspectionRequirementBusinessLogic.RunLogic();

//            // Assert
//            _mockAutoNumberRepo.Verify(r => r.IncrementAutonumber(ImportApplicationConstants.P3_COUNTER_NAME), Times.Never);
//        }

//        [Fact]
//        public void RunLogic_PreviousRiskLevelP2AndCurrentRiskLevelNotP2AndNotFlaggedForInspection_DecrementsTheP2Counter()
//        {
//            // Arrange
//            _postImportApplication.defraimp_PreviousImportRiskLevelId = new EntityReference(defraimp_importrisklevel.EntityLogicalName, Guid.NewGuid());
//            _postImportApplication.defraimp_PreviousImportRiskLevelId.Name = "P2";

//            // Act
//            _determineInspectionRequirementBusinessLogic.RunLogic();

//            // Assert
//            _mockAutoNumberRepo.Verify(r => r.DecrementAutonumber(ImportApplicationConstants.P2_COUNTER_NAME));

//        }

//        [Fact]
//        public void RunLogic_PreviousRiskLevelP2QuotaMoreThanZeroCounterNegativeByMoreThan_ShouldDecreaseTheQuotaAndIncreaseTheCounterByThreshold()
//        {
//            // Arrange
//            _postImportApplication.defraimp_PreviousImportRiskLevelId = new EntityReference(defraimp_importrisklevel.EntityLogicalName, Guid.NewGuid());
//            _postImportApplication.defraimp_PreviousImportRiskLevelId.Name = "P2";

//            int threshold = 10;

//            SetupCoverageRulesMock(threshold);
//            SetupAutonumberMock(ImportApplicationConstants.P2_COUNTER_NAME, -threshold);
//            SetupAutonumberMock(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME, 1);

//            // Act
//            _determineInspectionRequirementBusinessLogic.RunLogic();

//            // Assert
//            _mockAutoNumberRepo.Verify(r => r.DecrementAutonumber(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME), Times.Once);
//            _mockAutoNumberRepo.Verify(r => r.IncrementAutonumber(ImportApplicationConstants.P2_COUNTER_NAME, threshold + 1), Times.Once);
//        }

//        private void SetupCoverageRulesMock(int counterThreshold)
//        {
//            List<defraimp_inspectioncoveragerule> stubbedCoverageRules = GetDummyCoverageRules(10);

//            _mockCoverageRulesRepo.Setup(r =>
//                r.Find(
//                    It.IsAny<Expression<Func<defraimp_inspectioncoveragerule, bool>>>(),
//                    It.IsAny<Expression<Func<defraimp_inspectioncoveragerule, defraimp_inspectioncoveragerule>>>()))
//                    .Returns(() => stubbedCoverageRules.AsQueryable());
//        }

//        private List<defraimp_inspectioncoveragerule> GetDummyCoverageRules(int counterThreshold)
//        {
//            List<defraimp_inspectioncoveragerule> stubbedCoverageRules = new List<defraimp_inspectioncoveragerule>();

//            defraimp_inspectioncoveragerule stubbedCoverageRule = new defraimp_inspectioncoveragerule()
//            {
//                defraimp_NumberOfRecordsUntilInspection = counterThreshold
//            };

//            stubbedCoverageRules.Add(stubbedCoverageRule);

//            return stubbedCoverageRules;
//        }

//        private void SetupAutonumberMock(string autoNumberName, int autoNumberReturnValue)
//        {
//            _mockAutoNumberRepo.Setup(r => r.GetAutonumberValue(autoNumberName)).Returns(() => autoNumberReturnValue);
//        }

//        [Fact]
//        public void RunLogic_PreviousRiskLevelP2AndCurrentRiskLevelNotP2AndFlaggedForInspection_IncrementsTheP2QuotaCounter()
//        {
//            // Arrange
//            _postImportApplication.defraimp_PreviousImportRiskLevelId = new EntityReference(defraimp_importrisklevel.EntityLogicalName, Guid.NewGuid());
//            _postImportApplication.defraimp_PreviousImportRiskLevelId.Name = "P2";
//            _postImportApplication.defraimp_InspectionRequired = defraimp_importapplication_defraimp_inspectionrequired.Yes;

//            // Act
//            _determineInspectionRequirementBusinessLogic.RunLogic();

//            // Assert
//            _mockAutoNumberRepo.Verify(r => r.IncrementAutonumber(ImportApplicationConstants.P2_QUOTA_COUNTER_NAME));

//        }

//    }
//}
