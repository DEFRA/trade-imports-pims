using Defra.Imports.BusinessLogic.Itahc;
using Defra.Imports.Model;
using Defra.Imports.Repositories;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
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
    public class FlagRecordOnWatchListBusinessLogicTests
    {
        private Mock<IOrganizationService> _mockOrgService;
        private Mock<ICrmRepository<defraimp_WatchList>> _mockWatchListRepo;
        private Mock<ICrmRepository<defraimp_WatchFlag>> _mockWatchFlagRepo;

        public FlagRecordOnWatchListBusinessLogicTests()
        {
            _mockOrgService = new Mock<IOrganizationService>();
            _mockWatchListRepo = new Mock<ICrmRepository<defraimp_WatchList>>();
            _mockWatchFlagRepo = new Mock<ICrmRepository<defraimp_WatchFlag>>();
        }

        [Fact]
        public void FlagRecordIfOnWatchList_ItahcWithAConsigneeNameAndAMatchingWatchList_ShouldCreateAWatchFlag()
        {
            // Arrange =====
            string traderEntityName = "defraimp_consignee";
            Guid traderId = Guid.NewGuid();
            string traderIdentifier = "12345";

            // Setup the target
            Guid targetId = Guid.NewGuid();
            defraimp_itahc target = new defraimp_itahc()
            {
                Id = targetId,
                defraimp_itahcId = targetId,
                defraimp_ConsigneeName = traderIdentifier
            };

            // Setup the trader mock repo
            SetupTraderMockRepo("defraimp_consignee", traderId, "defraimp_name", traderIdentifier);

            // Setup the watch list repo
            Guid watchListId = Guid.NewGuid();
            defraimp_watchtype watchType = defraimp_watchtype.Consignee;
            SetupWatchListMockRepo(watchListId, watchType, traderEntityName, traderId);

            // Act =====
            FlagRecordOnWatchListBusinessLogic logicToRun = new FlagRecordOnWatchListBusinessLogic(_mockOrgService.Object, target, _mockWatchListRepo.Object, _mockWatchFlagRepo.Object);
            logicToRun.FlagRecordIfOnWatchList();

            // Assert =====
            _mockWatchFlagRepo.Verify(
                r => r.Create(
                    It.Is<defraimp_WatchFlag>(
                        e => e.defraimp_ItahcId.Id == target.Id &&
                             e.defraimp_WatchListId.Id == watchListId)));
        }

        [Fact]
        public void FlagRecordIfOnWatchList_ItahcWithADestinationNameAndAMatchingWatchList_ShouldCreateAWatchFlag()
        {
            // Arrange =====
            string placeOfDestinationEntityName = "defraimp_placeofdestination";
            string traderIdentifier = "12345";
            Guid placeOfDestinationId = Guid.NewGuid();

            // Setup the target
            Guid targetId = Guid.NewGuid();
            defraimp_itahc target = new defraimp_itahc()
            {
                Id = targetId,
                defraimp_itahcId = targetId,
                defraimp_PlaceOfDestinationName = traderIdentifier
            };

            // Setup the place of destination
            SetupTraderMockRepo(placeOfDestinationEntityName, placeOfDestinationId, "defraimp_name", traderIdentifier);

            // Setup the watch list repo
            Guid watchListId = Guid.NewGuid();
            defraimp_watchtype watchType = defraimp_watchtype.PlaceofDestination;
            SetupWatchListMockRepo(watchListId, watchType, placeOfDestinationEntityName, placeOfDestinationId);

            // Act =====
            FlagRecordOnWatchListBusinessLogic logicToRun = new FlagRecordOnWatchListBusinessLogic(_mockOrgService.Object, target, _mockWatchListRepo.Object, _mockWatchFlagRepo.Object);
            logicToRun.FlagRecordIfOnWatchList();

            // Assert =====
            _mockWatchFlagRepo.Verify(
                r => r.Create(
                    It.Is<defraimp_WatchFlag>(
                        e => e.defraimp_ItahcId.Id == target.Id &&
                             e.defraimp_WatchListId.Id == watchListId)));
        }

        [Fact]
        public void FlagRecordIfOnWatchList_ItahcWithAOVNameAndAMatchingWatchList_ShouldCreateAWatchFlag()
        {
            // Arrange =====
            string traderEntityName = "defraimp_veterinarian";
            Guid traderId = Guid.NewGuid();
            string traderIdentifier = "12345";

            // Setup the target
            Guid targetId = Guid.NewGuid();
            defraimp_itahc target = new defraimp_itahc()
            {
                Id = targetId,
                defraimp_itahcId = targetId,
                defraimp_OVName = traderIdentifier
            };

            // Setup the trader mock repo
            SetupTraderMockRepo("defraimp_veterinarian", traderId, "defraimp_name", traderIdentifier);

            // Setup the watch list repo
            Guid watchListId = Guid.NewGuid();
            defraimp_watchtype watchType = defraimp_watchtype.Veterinarian;
            SetupWatchListMockRepo(watchListId, watchType, traderEntityName, traderId);

            // Act =====
            FlagRecordOnWatchListBusinessLogic logicToRun = new FlagRecordOnWatchListBusinessLogic(_mockOrgService.Object, target, _mockWatchListRepo.Object, _mockWatchFlagRepo.Object);
            logicToRun.FlagRecordIfOnWatchList();

            // Assert =====
            _mockWatchFlagRepo.Verify(
                r => r.Create(
                    It.Is<defraimp_WatchFlag>(
                        e => e.defraimp_ItahcId.Id == target.Id &&
                             e.defraimp_WatchListId.Id == watchListId)));
        }

        private void SetupTraderMockRepo(string traderEntityName, Guid traderId, string traderIdentifierFieldName, string traderIdentifier)
        {
            // Setup the place of destination
            Entity trader = new Entity(traderEntityName);
            trader.Id = traderId;
            trader.Attributes.Add($"{traderEntityName}id", traderId);
            trader.Attributes.Add(traderIdentifierFieldName, traderIdentifier);

            EntityCollection traderCol = new EntityCollection(new List<Entity>() { trader });

            _mockOrgService.Setup(
                o => o.RetrieveMultiple(
                    It.Is<QueryExpression>(
                        q => q.EntityName == traderEntityName))).Returns(traderCol);
        }

        private void SetupWatchListMockRepo(Guid watchListId, defraimp_watchtype watchType, string traderEntityName, Guid traderId)
        {
            defraimp_WatchList stubbedWatchList = new defraimp_WatchList()
            {
                Id = watchListId,
                defraimp_WatchListId = watchListId,
                defraimp_WatchType = watchType,
            };

            SetTraderIdForWatchType(stubbedWatchList, watchType, traderEntityName, traderId);

            IQueryable<defraimp_WatchList> stubbedWatchListQueryable = new List<defraimp_WatchList>()
            {
                stubbedWatchList,
            }.AsQueryable();

            _mockWatchListRepo.Setup(
                r => r.Find(
                    It.IsAny<Expression<Func<defraimp_WatchList, bool>>>(),
                    It.IsAny<Expression<Func<defraimp_WatchList, defraimp_WatchList>>>())).Returns(stubbedWatchListQueryable);
        }

        private void SetTraderIdForWatchType(defraimp_WatchList stubbedWatchList, defraimp_watchtype watchType, string traderEntityName, Guid traderId)
        {
            switch (watchType)
            {
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
