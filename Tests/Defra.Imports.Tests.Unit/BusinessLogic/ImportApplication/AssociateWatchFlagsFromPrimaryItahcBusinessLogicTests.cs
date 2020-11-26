using Defra.Imports.BusinessLogic.ImportApplication;
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
    public class AssociateWatchFlagsFromPrimaryItahcBusinessLogicTests
    {
        private Mock<ICrmRepository<defraimp_WatchFlag>> _mockWatchFlagRepo;

        public AssociateWatchFlagsFromPrimaryItahcBusinessLogicTests()
        {
            _mockWatchFlagRepo = new Mock<ICrmRepository<defraimp_WatchFlag>>();
        }

        [Fact]
        public void RunLogic_ImportApplicationWithExistingFlags_ShouldRemoveTheExistingFlagsFirst()
        {
            // Arrange
            Guid targetId = Guid.NewGuid();
            defraimp_importapplication target = new defraimp_importapplication()
            {
                Id = targetId,
                defraimp_importapplicationId = targetId
            };

            Guid watchFlagId = Guid.NewGuid();
            defraimp_WatchFlag stubbedWatchFlag = new defraimp_WatchFlag()
            {
                Id = watchFlagId,
                defraimp_WatchFlagId = watchFlagId,
                defraimp_ImportApplicationId = new EntityReference(defraimp_importapplication.EntityLogicalName, targetId),
            };
            List<defraimp_WatchFlag> stubbedItahcWatchFlags = new List<defraimp_WatchFlag>()
            {
                stubbedWatchFlag
            };

            _mockWatchFlagRepo.Setup(
                r => r.Find(
                    It.IsAny<Expression<Func<defraimp_WatchFlag, bool>>>(),
                    It.IsAny<Expression<Func<defraimp_WatchFlag, defraimp_WatchFlag>>>())).Returns(stubbedItahcWatchFlags.AsQueryable());

            AssociateWatchFlagsFromPrimaryItahcBusinessLogic businessLogic = new AssociateWatchFlagsFromPrimaryItahcBusinessLogic(target, _mockWatchFlagRepo.Object);

            // Act
            businessLogic.RunLogic();

            // Assert
            Assert.Null(stubbedWatchFlag.defraimp_ImportApplicationId);
            _mockWatchFlagRepo.Verify(r => r.Update(stubbedWatchFlag));
        }

        [Fact]
        public void RunLogic_ImportApplicationWithItahcAssociatedToWatchFlags_ShouldAssociateImportApplicationWithFlags()
        {
            // Arrange
            Guid targetId = Guid.NewGuid();
            EntityReference primaryItahcRef = new EntityReference(defraimp_itahc.EntityLogicalName, Guid.NewGuid());
            defraimp_importapplication target = new defraimp_importapplication()
            {
                Id = targetId,
                defraimp_importapplicationId = targetId,
                defraimp_PrimaryITAHCId = primaryItahcRef
            };

            Guid watchFlagId = Guid.NewGuid();
            defraimp_WatchFlag stubbedWatchFlag = new defraimp_WatchFlag()
            {
                Id = watchFlagId,
                defraimp_WatchFlagId = watchFlagId,
                defraimp_ImportApplicationId = new EntityReference(defraimp_importapplication.EntityLogicalName, targetId),
            };
            List<defraimp_WatchFlag> stubbedItahcWatchFlags = new List<defraimp_WatchFlag>()
            {
                stubbedWatchFlag
            };

            _mockWatchFlagRepo.Setup(
                r => r.Find(
                    It.IsAny<Expression<Func<defraimp_WatchFlag, bool>>>(),
                    It.IsAny<Expression<Func<defraimp_WatchFlag, defraimp_WatchFlag>>>())).Returns(stubbedItahcWatchFlags.AsQueryable());

            AssociateWatchFlagsFromPrimaryItahcBusinessLogic businessLogic = new AssociateWatchFlagsFromPrimaryItahcBusinessLogic(target, _mockWatchFlagRepo.Object);

            // Act
            businessLogic.RunLogic();

            // Assert
            Assert.Equal(targetId, stubbedWatchFlag.defraimp_ImportApplicationId.Id);
            _mockWatchFlagRepo.Verify(r => r.Update(stubbedWatchFlag));
        }

    }
}
