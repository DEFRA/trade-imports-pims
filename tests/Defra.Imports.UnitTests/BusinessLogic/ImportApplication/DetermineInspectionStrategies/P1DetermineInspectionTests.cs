using Defra.Imports.BusinessLogic.ImportApplication.DetermineInspection.Strategies;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Xunit;

namespace Defra.Imports.UnitTests.BusinessLogic.ImportApplication.DetermineInspectionStrategies
{
    public class P1DetermineInspectionTests : DetermineInpsectionBaseTests
    {
        private P1DetermineInspection _p1DetermineInspection;
        private Mock<ICrmRepository<defraimp_goldbronzecommodity>> _mockGoldBronzeCommodityRepo;
        private Mock<ICrmRepository<defraimp_goldbronzecountriesnn>> _mockGoldBronzeCountryNNRepo;
        private Mock<IPlaceOfOriginRepository> _placeOfOriginRepo;


        public P1DetermineInspectionTests()
            : base()
        {
            _p1DetermineInspection = new P1DetermineInspection();
            _mockGoldBronzeCommodityRepo = new Mock<ICrmRepository<defraimp_goldbronzecommodity>>();
            _mockGoldBronzeCountryNNRepo = new Mock<ICrmRepository<defraimp_goldbronzecountriesnn>>();
            _placeOfOriginRepo = new Mock<IPlaceOfOriginRepository>();
           
            _mockRepositoryFactory
                .Setup(r => r.GetRepository<ImportsContext, defraimp_goldbronzecommodity>())
                .Returns(_mockGoldBronzeCommodityRepo.Object);

            _mockRepositoryFactory
              .Setup(r => r.GetRepository<ImportsContext, defraimp_goldbronzecountriesnn>())
              .Returns(_mockGoldBronzeCountryNNRepo.Object);

            _placeOfOriginRepo
                .Setup(r => r.Find(new Guid("1477b550-b6e6-e911-a866-000d3ab0dc71"))).Returns(new defraimp_placeoforigin());
        }

        [Fact(Skip = "Needs to be changed for latest changes")]
        public void ExecuteInspection_ImportApplicationWithoutCommodityId_ImportApplicationIsUpdatedToUndetermined()
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();
            _importApplication.Id = Guid.NewGuid();
            _importApplication.defraimp_CommodityTypeId = null;
            _importApplication.defraimp_CountryofOriginId = null;

            // Act
            _p1DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(
                r => r.Update(
                    It.Is<defraimp_importapplication>(
                        e =>
                            e.defraimp_importapplicationId == _importApplication.Id && 
                            e.defraimp_InspectionRequired == defraimp_importapplication_defraimp_inspectionrequired.Undetermined &&
                            e.defraimp_InspectionRequiredReason == defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown
                    )
                )
            );
        }

        public void SetupGoldBronzeCommodityRepo()
        {
            defraimp_goldbronzecommodity goldBronzeCommodityStub = new defraimp_goldbronzecommodity()
            {
                Id = new Guid("1477b550-b6e6-e911-a866-000d3ab0dc71")
            };
            List<defraimp_goldbronzecommodity> rules = new List<defraimp_goldbronzecommodity>();
            rules.Add(goldBronzeCommodityStub);

            _mockGoldBronzeCommodityRepo
                .Setup(r => r.Find<defraimp_goldbronzecommodity>(
                    It.IsAny<Expression<Func<defraimp_goldbronzecommodity, bool>>>(),
                    It.IsAny<Expression<Func<defraimp_goldbronzecommodity, defraimp_goldbronzecommodity>>>()
                ))
                .Returns(() => rules.AsQueryable());
        }

        public void SetupGoldBronzeCommodityNNRepo()
        {
            defraimp_goldbronzecountriesnn goldBronzeCountriesnnStub = new defraimp_goldbronzecountriesnn();
            List<defraimp_goldbronzecountriesnn> rules = new List<defraimp_goldbronzecountriesnn>();
            rules.Add(goldBronzeCountriesnnStub);

            _mockGoldBronzeCountryNNRepo
                .Setup(r => r.Find<defraimp_goldbronzecountriesnn>(
                    It.IsAny<Expression<Func<defraimp_goldbronzecountriesnn, bool>>>(),
                    It.IsAny<Expression<Func<defraimp_goldbronzecountriesnn, defraimp_goldbronzecountriesnn>>>()
                ))
                .Returns(() => rules.AsQueryable());
        }

        public static IEnumerable<object[]> InspectionRequiredTestData()
        {
            //Commodity, country, inspection required?, inspection required reason
            yield return new object[] { null, null, defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown };
            yield return new object[] { new EntityReference("defraexp_commoditytype", Guid.NewGuid()), null, defraimp_importapplication_defraimp_inspectionrequired.Discretionary, defraimp_importapplication_defraimp_inspectionrequiredreason.VerifiedPlaceofOriginMissing };
            yield return new object[] { null, new EntityReference("defraimp_placeoforigin", Guid.NewGuid()), defraimp_importapplication_defraimp_inspectionrequired.Undetermined, defraimp_importapplication_defraimp_inspectionrequiredreason.RiskLevelUnknown };
        }

        [Theory(Skip = "Needs to be changed for latest changes")]
        [MemberData(nameof(InspectionRequiredTestData))]
        public void ExecuteInspection_ImportApplicationRequiredAndRequiredReason_InspectionWithMissingFieldsResultsInExpectedOutcome(EntityReference commodityType, EntityReference placeOfOrigin, defraimp_importapplication_defraimp_inspectionrequired expectedInspectionRequired, defraimp_importapplication_defraimp_inspectionrequiredreason expectedInspectionRequiredReason)
        {
            // Arrange
            SetupCoverageRulesRepoToReturnRules();
            SetupGoldBronzeCommodityRepo();
            SetupGoldBronzeCommodityNNRepo();
            SetupRepositoryFactory();

            _importApplication.Id = Guid.NewGuid();
            _importApplication.defraimp_CommodityTypeId = commodityType;
            _importApplication.defraimp_PlaceofOriginid = placeOfOrigin;

            // Act
            _p1DetermineInspection.ExecuteInspection(_determineInspectionContext);

            // Assert
            _mockImportApplicationRepo.Verify(r => r.Update(It.Is<defraimp_importapplication>( 
                e => e.defraimp_importapplicationId == _importApplication.Id 
                && e.defraimp_InspectionRequired == expectedInspectionRequired 
                && e.defraimp_InspectionRequiredReason == expectedInspectionRequiredReason)
                )
            );
        }
    }
}
