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
    public class AssociateWatchFlagsFromNotificationBusinessLogicTests
    {
        private Mock<ICrmRepository<defraimp_WatchFlag>> mockWatchFlagRepo;

        public AssociateWatchFlagsFromNotificationBusinessLogicTests()
        {
            this.mockWatchFlagRepo = new Mock<ICrmRepository<defraimp_WatchFlag>>();
        }

        [Fact]
        public void RunLogic_ImportApplicationWithExistingNotificationFlags_ShouldRemoveTheExistingFlagsFirst()
        {
            // Arrange
            Guid targetId = Guid.NewGuid();
            defraimp_importapplication stubbedTarget = this.CreateStubbedImportApplication(targetId, null);

            Guid watchFlagId = Guid.NewGuid();
            defraimp_WatchFlag stubbedWatchFlag = this.CreateStubbedWatchFlag(watchFlagId, stubbedTarget);

            this.SetupMockWatchFlagRepo(stubbedWatchFlag);

            // Act
            AssociateWatchFlagsFromPrimaryNotificationBusinessLogic businessLogic = new AssociateWatchFlagsFromPrimaryNotificationBusinessLogic(stubbedTarget, this.mockWatchFlagRepo.Object);
            businessLogic.RunLogic();

            // Assert
            Assert.Null(stubbedWatchFlag.defraimp_ImportApplicationId);

            this.mockWatchFlagRepo.Verify(r => r.Update(stubbedWatchFlag));
        }

        [Fact]
        public void RunLogic_ImportApplicationWithNotificationAssociatedToWatchFlags_ShouldAssociateImportApplicationWithFlags()
        {
            // Arrange
            Guid targetId = Guid.NewGuid();
            defraimp_importapplication stubbedTarget = this.CreateStubbedImportApplication(targetId, new EntityReference(defraimp_ImporterNotification.EntityLogicalName, Guid.NewGuid()));

            Guid watchFlagId = Guid.NewGuid();
            defraimp_WatchFlag stubbedWatchFlag = this.CreateStubbedWatchFlag(watchFlagId, stubbedTarget);

            this.SetupMockWatchFlagRepo(stubbedWatchFlag);

            // Act
            AssociateWatchFlagsFromPrimaryNotificationBusinessLogic businessLogic = new AssociateWatchFlagsFromPrimaryNotificationBusinessLogic(stubbedTarget, this.mockWatchFlagRepo.Object);
            businessLogic.RunLogic();

            // assert
            Assert.Equal(targetId, stubbedWatchFlag.defraimp_ImportApplicationId.Id);
            this.mockWatchFlagRepo.Verify(r => r.Update(stubbedWatchFlag));
        }

        private defraimp_importapplication CreateStubbedImportApplication(Guid id, EntityReference notificationRef)
        {
            return new defraimp_importapplication()
            {
                Id = id,
                defraimp_importapplicationId = id,
                defraimp_PrimaryImporterNotificationId = notificationRef,
            };
        }

        private defraimp_WatchFlag CreateStubbedWatchFlag(Guid id, defraimp_importapplication application)
        {
            return new defraimp_WatchFlag()
            {
                Id = id,
                defraimp_WatchFlagId = id,
                defraimp_ImportApplicationId = application.ToEntityReference(),
                defraimp_ImporterNotificationId = application.defraimp_PrimaryImporterNotificationId,
            };
        }

        private void SetupMockWatchFlagRepo(defraimp_WatchFlag stubbedWatchFlag)
        {
            this.mockWatchFlagRepo.Setup(
                r => r.Find(
                    It.IsAny<Expression<Func<defraimp_WatchFlag, bool>>>(),
                    It.IsAny<Expression<Func<defraimp_WatchFlag, defraimp_WatchFlag>>>()))
                .Returns(new List<defraimp_WatchFlag>() { stubbedWatchFlag }.AsQueryable());
        }

    }
}
