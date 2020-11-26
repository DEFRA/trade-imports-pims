namespace Defra.Imports.Tests.Unit.BusinessLogic.ImporterNotification
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Threading.Tasks;
    using Defra.Imports.BusinessLogic.ImporterNotification;
    using Defra.Imports.Model;
    using Defra.Imports.Repositories;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    public class FlagRecordsOnWatchListNotificationBusinessLogicTests
    {
        private Mock<IOrganizationService> mockOrgService;
        private Mock<ICrmRepository<defraimp_WatchList>> mockWatchListRepo;
        private Mock<ICrmRepository<defraimp_WatchFlag>> mockWatchFlagRepo;

        public FlagRecordsOnWatchListNotificationBusinessLogicTests()
        {
            this.mockOrgService = new Mock<IOrganizationService>();
            this.mockWatchListRepo = new Mock<ICrmRepository<defraimp_WatchList>>();
            this.mockWatchFlagRepo = new Mock<ICrmRepository<defraimp_WatchFlag>>();
        }

        [Fact]
        public void FlagRecordIfOnWatchList_NotificationWithAImporterNameAndAMatchingWatchList_ShouldCreateAWatchFlag()
        {
            // Arrange
            string economicOperatorEntityName = "defraimp_consignee";
            Guid economicOperatorId = Guid.NewGuid();
            string economicOperatorIdentifier = "12345";

            Guid targetNotificationId = Guid.NewGuid();
            defraimp_ImporterNotification targetNotification = new defraimp_ImporterNotification()
            {
                Id = targetNotificationId,
                defraimp_ImporterNotificationId = targetNotificationId,
                defraimp_importercompanyname = economicOperatorIdentifier,
            };

            this.SetupTraderMockRepo(economicOperatorEntityName, economicOperatorId, "defraimp_name", economicOperatorIdentifier);

            Guid watchListId = Guid.NewGuid();
            defraimp_watchtype watchType = defraimp_watchtype.Consignee;
            this.SetupWatchListMockRepo(watchListId, watchType, economicOperatorEntityName, economicOperatorId);

            // Act
            // Assert
            this.RunAndVerifyCreatedWatchFlag(targetNotification, watchListId);

        }

        private void RunAndVerifyCreatedWatchFlag(defraimp_ImporterNotification target, Guid watchListId)
        {
            // Act =====
            FlagRecordsOnWatchListNotificationBusinessLogic logicToRun = new FlagRecordsOnWatchListNotificationBusinessLogic(this.mockOrgService.Object, target, this.mockWatchListRepo.Object, this.mockWatchFlagRepo.Object);
            logicToRun.FlagRecordIfOnWatchList();

            // Assert =====
            this.mockWatchFlagRepo.Verify(
                r => r.Create(
                    It.Is<defraimp_WatchFlag>(
                        e => e.defraimp_ImporterNotificationId.Id == target.Id &&
                             e.defraimp_WatchListId.Id == watchListId)));
        }

        private void SetupTraderMockRepo(string economicOperatorEntityName, Guid economicOperatorId, string economicOperatorIdentifierFieldName, string economicOperatorIdentifier)
        {
            // Setup the place of destination
            Entity trader = new Entity(economicOperatorEntityName);
            trader.Id = economicOperatorId;
            trader.Attributes.Add($"{economicOperatorEntityName}id", economicOperatorId);
            trader.Attributes.Add(economicOperatorIdentifierFieldName, economicOperatorIdentifier);

            EntityCollection traderCol = new EntityCollection(new List<Entity>() { trader });

            this.mockOrgService.Setup(
                o => o.RetrieveMultiple(
                    It.Is<QueryExpression>(
                        q => q.EntityName == economicOperatorEntityName))).Returns(traderCol);
        }

        private void SetupWatchListMockRepo(Guid watchListId, defraimp_watchtype watchType, string traderEntityName, Guid traderId)
        {
            defraimp_WatchList stubbedWatchList = new defraimp_WatchList()
            {
                Id = watchListId,
                defraimp_WatchListId = watchListId,
                defraimp_WatchType = watchType,
            };

            this.SetTraderIdForWatchType(stubbedWatchList, watchType, traderEntityName, traderId);

            IQueryable<defraimp_WatchList> stubbedWatchListQueryable = new List<defraimp_WatchList>()
            {
                stubbedWatchList,
            }.AsQueryable();

            this.mockWatchListRepo.Setup(
                r => r.Find(
                    It.IsAny<Expression<Func<defraimp_WatchList, bool>>>(),
                    It.IsAny<Expression<Func<defraimp_WatchList, defraimp_WatchList>>>())).Returns(stubbedWatchListQueryable);
        }

        private void SetTraderIdForWatchType(defraimp_WatchList stubbedWatchList, defraimp_watchtype watchType, string traderEntityName, Guid traderId)
        {
            switch (watchType)
            {
                case defraimp_watchtype.PlaceofOrigin:
                    stubbedWatchList.defraimp_PlaceOfOriginId = new EntityReference(traderEntityName, traderId);
                    break;
                case defraimp_watchtype.PlaceofDestination:
                    stubbedWatchList.defraimp_PlaceOfDestinationId = new EntityReference(traderEntityName, traderId);
                    break;
                case defraimp_watchtype.Consignee:
                    stubbedWatchList.defraimp_ConsigneeId = new EntityReference(traderEntityName, traderId);
                    break;
                case defraimp_watchtype.Transporter:
                    stubbedWatchList.defraimp_TransporterId = new EntityReference(traderEntityName, traderId);
                    break;
                case defraimp_watchtype.Veterinarian:
                    stubbedWatchList.defraimp_VeterinarianId = new EntityReference(traderEntityName, traderId);
                    break;
            }
        }

    }
}
