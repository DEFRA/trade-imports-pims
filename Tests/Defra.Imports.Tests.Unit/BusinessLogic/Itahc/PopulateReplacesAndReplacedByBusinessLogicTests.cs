using Defra.Imports.BusinessLogic.Itahc;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Defra.Imports.Tests.Unit.BusinessLogic.Itahc
{
    public class PopulateReplacesAndReplacedByBusinessLogicTests
    {
        private Mock<ICrmRepository<defraimp_itahc>> _mockItahcRepo;

        public PopulateReplacesAndReplacedByBusinessLogicTests()
        {
            _mockItahcRepo = new Mock<ICrmRepository<defraimp_itahc>>();
        }

        [Fact]
        public void RunLogic_EmptyReplacedAndReplacingReferenceNumber_ShouldNotUpdateTheReplaceLookupFields()
        {
            defraimp_itahc target = new defraimp_itahc();

            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockItahcRepo.Object, target);
            logic.RunLogic();

            Assert.Null(target.defraimp_ReplacedById);
            Assert.Null(target.defraimp_ReplacesId);
        }

        [Fact]
        public void RunLogic_TargetWithReplacedReferenceNumberThatExists_ShouldUpdateTheTargetReplacedById()
        {
            // Arrange
            string certRefNumber = "test ref";
            Guid retrievedRecordId = Guid.NewGuid();

            defraimp_itahc target = new defraimp_itahc()
            {
                defraimp_ReplacedReferenceNumber = certRefNumber
            };

            this.SetupItahcMockRepoWithOneResult(retrievedRecordId, certRefNumber);

            // Act
            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockItahcRepo.Object, target);
            logic.RunLogic();

            // Assert
            Assert.Equal(retrievedRecordId, target.defraimp_ReplacedById.Id);
        }

        [Fact]
        public void RunLogic_TargetWithReplacedReferenceNumberThatDoesntExist_ShouldNotUpdateTheTargetReplacedById()
        {
            // Arrange
            string certRefNumber = "test ref";
            Guid retrievedRecordId = Guid.NewGuid();

            defraimp_itahc target = new defraimp_itahc()
            {
                defraimp_ReplacedReferenceNumber = certRefNumber
            };

            this.SetupItahcMockRepoWithNoResults();

            // Act
            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockItahcRepo.Object, target);
            logic.RunLogic();

            // Assert
            Assert.Null(target.defraimp_ReplacedById);
        }

        [Fact]
        public void RunLogic_TargetWithReplacingReferenceNumberThatExists_ShouldUpdateTheTargetReplacesId()
        {
            // Arrange
            string certRefNumber = "test ref";
            Guid retrievedRecordId = Guid.NewGuid();

            defraimp_itahc target = new defraimp_itahc()
            {
                defraimp_ReplacingReferenceNumber = certRefNumber
            };

            this.SetupItahcMockRepoWithOneResult(retrievedRecordId, certRefNumber);

            // Act
            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockItahcRepo.Object, target);
            logic.RunLogic();

            // Assert
            Assert.Equal(retrievedRecordId, target.defraimp_ReplacesId.Id);
        }

        [Fact]
        public void RunLogic_TargetWithReplacingReferenceNumberThatDoesntExist_ShouldNotUpdateTheTargetReplacesId()
        {
            // Arrange
            string certRefNumber = "test ref";
            Guid retrievedRecordId = Guid.NewGuid();

            defraimp_itahc target = new defraimp_itahc()
            {
                defraimp_ReplacingReferenceNumber = certRefNumber
            };

            this.SetupItahcMockRepoWithNoResults();

            // Act
            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockItahcRepo.Object, target);
            logic.RunLogic();

            // Assert
            Assert.Null(target.defraimp_ReplacesId);
        }

        private void SetupItahcMockRepoWithOneResult(Guid retrievedRecordId, string certificateReferenceNumber)
        {
            defraimp_itahc stubbedReplacedItahc = new defraimp_itahc()
            {
                defraimp_HealthCertificateNumber = certificateReferenceNumber,
                defraimp_itahcId = retrievedRecordId,
            };

            List<defraimp_itahc> stubbedItahcs = new List<defraimp_itahc>()
            {
                stubbedReplacedItahc
            };

            UpdateMockedIahcRepoWithStubbedOutput(stubbedItahcs);
        }

        private void SetupItahcMockRepoWithNoResults()
        {
            List<defraimp_itahc> stubbedItahcs = new List<defraimp_itahc>();
            UpdateMockedIahcRepoWithStubbedOutput(stubbedItahcs);
        }

        private void UpdateMockedIahcRepoWithStubbedOutput(List<defraimp_itahc> stubbedItahcs)
        {
            _mockItahcRepo.Setup(r => r.Find(
                It.IsAny<Expression<Func<defraimp_itahc, bool>>>(),
                It.IsAny<Expression<Func<defraimp_itahc, defraimp_itahc>>>()
            )).Returns(stubbedItahcs.AsQueryable());
        }
    }
}
