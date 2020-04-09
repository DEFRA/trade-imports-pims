using Defra.Imports.BusinessLogic.Itahc;
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

namespace Defra.Imports.Tests.Unit.BusinessLogic.Itahc
{
    public class PopulateReplacesAndReplacedByBusinessLogicTests
    {
        private Mock<ICrmRepository> _mockCertificateRepo;

        public PopulateReplacesAndReplacedByBusinessLogicTests()
        {
            _mockCertificateRepo = new Mock<ICrmRepository>();
        }

        [Fact]
        public void RunLogic_EmptyReplacedAndReplacingReferenceNumber_ShouldNotUpdateTheReplaceLookupFields()
        {
            defraimp_itahc target = new defraimp_itahc();

            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockCertificateRepo.Object, target);
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
                Id = Guid.NewGuid(),
                defraimp_ReplacedReferenceNumber = certRefNumber
            };

            this.SetupItahcMockRepoWithOneResult(retrievedRecordId, certRefNumber);

            // Act
            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockCertificateRepo.Object, target);
            logic.RunLogic();

            // Assert
            Assert.Equal(retrievedRecordId, target.defraimp_ReplacedById.Id);
            _mockCertificateRepo.Verify(r => r.Update(It.Is<Entity>(e => e.Id == target.Id)));
        }

        [Fact]
        public void RunLogic_TargetWithReplacedReferenceNumberThatDoesntExist_ShouldNotUpdateTheTargetReplacedById()
        {
            // Arrange
            string certRefNumber = "test ref";
            Guid retrievedRecordId = Guid.NewGuid();

            defraimp_itahc target = new defraimp_itahc()
            {
                Id = Guid.NewGuid(),
                defraimp_ReplacedReferenceNumber = certRefNumber
            };

            this.SetupItahcMockRepoWithNoResults();

            // Act
            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockCertificateRepo.Object, target);
            logic.RunLogic();

            // Assert
            Assert.Null(target.defraimp_ReplacedById);
            _mockCertificateRepo.Verify(r => r.Update(It.Is<Entity>(e => e.Id == target.Id)), Times.Never);
        }

        [Fact]
        public void RunLogic_TargetWithReplacingReferenceNumberThatExists_ShouldUpdateTheTargetReplacesId()
        {
            // Arrange
            string certRefNumber = "test ref";
            Guid retrievedRecordId = Guid.NewGuid();

            defraimp_itahc target = new defraimp_itahc()
            {
                Id = Guid.NewGuid(),
                defraimp_ReplacingReferenceNumber = certRefNumber
            };

            this.SetupItahcMockRepoWithOneResult(retrievedRecordId, certRefNumber);

            // Act
            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockCertificateRepo.Object, target);
            logic.RunLogic();

            // Assert
            Assert.Equal(retrievedRecordId, target.defraimp_ReplacesId.Id);
            _mockCertificateRepo.Verify(r => r.Update(It.Is<Entity>(e => e.Id == target.Id)));
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
            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockCertificateRepo.Object, target);
            logic.RunLogic();

            // Assert
            Assert.Null(target.defraimp_ReplacesId);
            _mockCertificateRepo.Verify(r => r.Update(It.Is<Entity>(e => e.Id == target.Id)), Times.Never);
        }

        [Fact]
        public void RunLogic_DocomWithReplacedAndReplacesReference_ShouldUpdateTheTargetReplacedByAndReplacingIds()
        {
            // Arrange
            string certRefNumber = "test ref";
            Guid retrievedRecordId = Guid.NewGuid();

            defraimp_docom target = new defraimp_docom()
            {
                defraimp_ReplacedReferenceNumber = certRefNumber,
                defraimp_ReplacingReferenceNumber = certRefNumber
            };

            this.SetupDocomMockRepoWithOneResult(retrievedRecordId, certRefNumber);

            // Act
            PopulateReplacesAndReplacedByBusinessLogic logic = new PopulateReplacesAndReplacedByBusinessLogic(_mockCertificateRepo.Object, target);
            logic.RunLogic();

            // Assert
            Assert.Equal(retrievedRecordId, target.defraimp_ReplacedById.Id);
            Assert.Equal(retrievedRecordId, target.defraimp_ReplacesId.Id);
        }

        private void SetupItahcMockRepoWithOneResult(Guid retrievedRecordId, string certificateReferenceNumber)
        {
            defraimp_itahc stubbedReplacedItahc = new defraimp_itahc()
            {
                Id = retrievedRecordId,
                defraimp_name = certificateReferenceNumber,
                defraimp_HealthCertificateNumber = certificateReferenceNumber,
                defraimp_itahcId = retrievedRecordId,
            };

            List<Entity> stubbedItahcs = new List<Entity>()
            {
                stubbedReplacedItahc
            };

            UpdateMockedIahcRepoWithStubbedOutput(stubbedItahcs);
        }

        private void SetupItahcMockRepoWithNoResults()
        {
            List<Entity> stubbedItahcs = new List<Entity>();
            UpdateMockedIahcRepoWithStubbedOutput(stubbedItahcs);
        }

        private void SetupDocomMockRepoWithOneResult(Guid retrievedRecordId, string certificateReferenceNumber)
        {
            defraimp_docom stubbedReplacedDocom = new defraimp_docom()
            {
                Id = retrievedRecordId,
                defraimp_name = certificateReferenceNumber,
                defraimp_docomId = retrievedRecordId,
            };

            List<Entity> stubbedDocoms = new List<Entity>()
            {
                stubbedReplacedDocom
            };

            UpdateMockedIahcRepoWithStubbedOutput(stubbedDocoms);
        }

        private void UpdateMockedIahcRepoWithStubbedOutput(List<Entity> stubbedItahcs)
        {
            _mockCertificateRepo.Setup(r => r.Find(
                It.IsAny<Expression<Func<Entity, bool>>>(),
                It.IsAny<Expression<Func<Entity, Entity>>>()
            )).Returns(stubbedItahcs.AsQueryable());
        }
    }
}
