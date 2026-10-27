namespace Defra.Imports.UnitTests.Application.WatchLists
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    public sealed class WatchFlagCreatorTests
    {
        private const string Identifier = "Acme";
        private const string WatchListName = "Watch list";
        private const string RecordName = "Record 1";

        private readonly Guid recordId = Guid.NewGuid();
        private readonly Guid economicOperatorId = Guid.NewGuid();
        private readonly Guid watchListId = Guid.NewGuid();
        private readonly Mock<IOrganizationService> service = new Mock<IOrganizationService>(MockBehavior.Strict);
        private readonly List<defraimp_WatchFlag> created = new List<defraimp_WatchFlag>();
        private readonly WatchFlagCreator creator;

        public WatchFlagCreatorTests()
        {
            this.creator = new WatchFlagCreator(new WatchFlagQueries(new ImportsContext(this.service.Object)), this.service.Object);
            this.service
                .Setup(value => value.Create(It.IsAny<Entity>()))
                .Callback<Entity>(entity => this.created.Add((defraimp_WatchFlag)entity))
                .Returns(Guid.NewGuid());
        }

        public static IEnumerable<object[]> ItahcWatchTypes => new[]
        {
            new object[] { defraimp_watchtype.PlaceofOrigin },
            new object[] { defraimp_watchtype.PlaceofDestination },
            new object[] { defraimp_watchtype.Consignee },
            new object[] { defraimp_watchtype.Transporter },
            new object[] { defraimp_watchtype.Veterinarian },
        };

        public static IEnumerable<object[]> NotificationWatchTypes => new[]
        {
            new object[] { defraimp_watchtype.PlaceofOrigin },
            new object[] { defraimp_watchtype.PlaceofDestination },
            new object[] { defraimp_watchtype.Consignee },
            new object[] { defraimp_watchtype.Transporter },
        };

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WatchFlagCreator(null, this.service.Object));
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            var queries = new WatchFlagQueries(new ImportsContext(this.service.Object));

            Assert.Throws<ArgumentNullException>(() => new WatchFlagCreator(queries, null));
        }

        [Fact]
        public void CreateWatchFlags_NullItahcContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.creator.CreateWatchFlags((CreateItahcWatchFlagsContext)null));
        }

        [Fact]
        public void CreateWatchFlags_NullNotificationContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.creator.CreateWatchFlags((CreateNotificationWatchFlagsContext)null));
        }

        [Theory]
        [MemberData(nameof(ItahcWatchTypes))]
        public void CreateWatchFlags_ItahcNamingAnOperatorOnAMatchingWatchList_CreatesAWatchFlag(defraimp_watchtype watchType)
        {
            var itahc = this.CreateItahc(watchType, Identifier);
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, watchType, this.economicOperatorId));
            this.SetupEconomicOperator(watchType, this.economicOperatorId);

            this.creator.CreateWatchFlags(itahc);

            var flag = Assert.Single(this.created);
            Assert.Equal(this.recordId, flag.defraimp_ItahcId.Id);
            Assert.Equal(defraimp_itahc.EntityLogicalName, flag.defraimp_ItahcId.LogicalName);
            Assert.Equal(this.watchListId, flag.defraimp_WatchListId.Id);
            Assert.Equal(defraimp_WatchList.EntityLogicalName, flag.defraimp_WatchListId.LogicalName);
            Assert.Null(flag.defraimp_ImporterNotificationId);
            Assert.Equal($"{WatchListName} - {RecordName}", flag.defraimp_Name);
        }

        [Theory]
        [MemberData(nameof(NotificationWatchTypes))]
        public void CreateWatchFlags_NotificationNamingAnOperatorOnAMatchingWatchList_CreatesAWatchFlag(defraimp_watchtype watchType)
        {
            var notification = this.CreateNotification(watchType, Identifier);
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, watchType, this.economicOperatorId));
            this.SetupEconomicOperator(watchType, this.economicOperatorId);

            this.creator.CreateWatchFlags(notification);

            var flag = Assert.Single(this.created);
            Assert.Equal(this.recordId, flag.defraimp_ImporterNotificationId.Id);
            Assert.Equal(defraimp_ImporterNotification.EntityLogicalName, flag.defraimp_ImporterNotificationId.LogicalName);
            Assert.Equal(this.watchListId, flag.defraimp_WatchListId.Id);
            Assert.Null(flag.defraimp_ItahcId);
            Assert.Equal($"{WatchListName} - {RecordName}", flag.defraimp_Name);
        }

        [Fact]
        public void CreateWatchFlags_ItahcFields_AreCheckedAgainstTheMatchingWatchType()
        {
            var context = new CreateItahcWatchFlagsContext(this.ItahcReference(), RecordName, "origin", "destination", "consignee", "transporter", "veterinarian");
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, defraimp_watchtype.Veterinarian, this.economicOperatorId));
            var queried = new Dictionary<defraimp_watchtype, IList<QueryExpression>>();
            foreach (var watchType in new[] { defraimp_watchtype.PlaceofOrigin, defraimp_watchtype.PlaceofDestination, defraimp_watchtype.Consignee, defraimp_watchtype.Transporter, defraimp_watchtype.Veterinarian })
            {
                queried[watchType] = this.SetupEconomicOperator(watchType, this.economicOperatorId);
            }

            this.creator.CreateWatchFlags(context);

            // Only the veterinarian watch list exists, so only the veterinarian name is looked up.
            this.service.VerifyQueriedAny(Times.Exactly(2));
            var query = Assert.Single(queried[defraimp_watchtype.Veterinarian]);
            Assert.Equal("veterinarian", Assert.Single(query.GetAllConditions()).Values.Single());
            Assert.Single(this.created);
        }

        [Fact]
        public void CreateWatchFlags_NotificationFields_AreCheckedAgainstTheMatchingWatchType()
        {
            var context = new CreateNotificationWatchFlagsContext(this.NotificationReference(), RecordName, "consignor", "destination", "importer", "transporter");
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, defraimp_watchtype.Consignee, this.economicOperatorId));
            var queried = this.SetupEconomicOperator(defraimp_watchtype.Consignee, this.economicOperatorId);

            this.creator.CreateWatchFlags(context);

            Assert.Equal("importer", Assert.Single(Assert.Single(queried).GetAllConditions()).Values.Single());
            Assert.Single(this.created);
        }

        [Fact]
        public void CreateWatchFlags_NoIdentifiers_ReadsTheWatchListsOnceAndCreatesNothing()
        {
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, defraimp_watchtype.Consignee, this.economicOperatorId));

            this.creator.CreateWatchFlags(this.CreateItahc(defraimp_watchtype.Consignee, null));

            this.service.VerifyQueried(defraimp_WatchList.EntityLogicalName, Times.Once());
            this.service.VerifyQueriedAny(Times.Once());
            Assert.Empty(this.created);
        }

        [Fact]
        public void CreateWatchFlags_EmptyIdentifier_IsIgnored()
        {
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, defraimp_watchtype.Consignee, this.economicOperatorId));

            this.creator.CreateWatchFlags(this.CreateItahc(defraimp_watchtype.Consignee, string.Empty));

            this.service.VerifyQueriedAny(Times.Once());
            Assert.Empty(this.created);
        }

        [Fact]
        public void CreateWatchFlags_WatchListOfAnotherType_DoesNotLookUpTheOperator()
        {
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, defraimp_watchtype.Transporter, this.economicOperatorId));

            this.creator.CreateWatchFlags(this.CreateItahc(defraimp_watchtype.Consignee, Identifier));

            this.service.VerifyQueriedAny(Times.Once());
            Assert.Empty(this.created);
        }

        [Fact]
        public void CreateWatchFlags_WatchListWithoutTheLookup_DoesNotLookUpTheOperator()
        {
            this.SetupWatchLists(new defraimp_WatchList { Id = this.watchListId, defraimp_Name = WatchListName, defraimp_WatchType = defraimp_watchtype.Consignee });

            this.creator.CreateWatchFlags(this.CreateItahc(defraimp_watchtype.Consignee, Identifier));

            this.service.VerifyQueriedAny(Times.Once());
            Assert.Empty(this.created);
        }

        [Fact]
        public void CreateWatchFlags_OperatorNotFound_CreatesNothing()
        {
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, defraimp_watchtype.Consignee, this.economicOperatorId));
            this.service.ReturnsRecords(defraimp_Consignee.EntityLogicalName);

            this.creator.CreateWatchFlags(this.CreateItahc(defraimp_watchtype.Consignee, Identifier));

            this.service.VerifyQueried(defraimp_Consignee.EntityLogicalName, Times.Once());
            Assert.Empty(this.created);
        }

        [Fact]
        public void CreateWatchFlags_OperatorOfAnotherWatchList_CreatesNothing()
        {
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, defraimp_watchtype.Consignee, Guid.NewGuid()));
            this.SetupEconomicOperator(defraimp_watchtype.Consignee, this.economicOperatorId);

            this.creator.CreateWatchFlags(this.CreateItahc(defraimp_watchtype.Consignee, Identifier));

            Assert.Empty(this.created);
        }

        [Fact]
        public void CreateWatchFlags_SeveralMatchingWatchLists_FlagsOnlyTheFirst()
        {
            var secondWatchListId = Guid.NewGuid();
            this.SetupWatchLists(
                this.CreateWatchList(this.watchListId, defraimp_watchtype.Consignee, this.economicOperatorId),
                this.CreateWatchList(secondWatchListId, defraimp_watchtype.Consignee, this.economicOperatorId));
            this.SetupEconomicOperator(defraimp_watchtype.Consignee, this.economicOperatorId);

            this.creator.CreateWatchFlags(this.CreateItahc(defraimp_watchtype.Consignee, Identifier));

            Assert.Equal(this.watchListId, Assert.Single(this.created).defraimp_WatchListId.Id);
        }

        [Fact]
        public void CreateWatchFlags_SeveralOperatorsWithTheName_ChecksOnlyTheFirst()
        {
            var otherOperatorId = Guid.NewGuid();
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, defraimp_watchtype.Consignee, otherOperatorId));
            this.service.ReturnsRecords(
                defraimp_Consignee.EntityLogicalName,
                new defraimp_Consignee { Id = this.economicOperatorId },
                new defraimp_Consignee { Id = otherOperatorId });

            this.creator.CreateWatchFlags(this.CreateItahc(defraimp_watchtype.Consignee, Identifier));

            Assert.Empty(this.created);
        }

        [Fact]
        public void CreateWatchFlags_SeveralIdentifiersOnWatchLists_CreatesAFlagForEachInOrder()
        {
            var destinationOperatorId = Guid.NewGuid();
            var destinationWatchListId = Guid.NewGuid();
            var context = new CreateItahcWatchFlagsContext(this.ItahcReference(), RecordName, Identifier, Identifier, null, null, null);
            this.SetupWatchLists(
                this.CreateWatchList(this.watchListId, defraimp_watchtype.PlaceofOrigin, this.economicOperatorId),
                this.CreateWatchList(destinationWatchListId, defraimp_watchtype.PlaceofDestination, destinationOperatorId));
            this.SetupEconomicOperator(defraimp_watchtype.PlaceofOrigin, this.economicOperatorId);
            this.SetupEconomicOperator(defraimp_watchtype.PlaceofDestination, destinationOperatorId);

            this.creator.CreateWatchFlags(context);

            Assert.Equal(new[] { this.watchListId, destinationWatchListId }, this.created.Select(flag => flag.defraimp_WatchListId.Id).ToArray());
        }

        [Fact]
        public void CreateWatchFlags_RecordWithoutAName_NamesTheFlagWithAnEmptyRecordName()
        {
            var context = new CreateItahcWatchFlagsContext(this.ItahcReference(), null, null, null, Identifier, null, null);
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, defraimp_watchtype.Consignee, this.economicOperatorId));
            this.SetupEconomicOperator(defraimp_watchtype.Consignee, this.economicOperatorId);

            this.creator.CreateWatchFlags(context);

            Assert.Equal($"{WatchListName} - ", Assert.Single(this.created).defraimp_Name);
        }

        [Fact]
        public void CreateWatchFlags_Itahc_ReadsTheWatchListsForTodayBeforeLookingUpOperators()
        {
            var recorded = this.SetupWatchLists();
            var before = DateTime.Now.Date;

            this.creator.CreateWatchFlags(this.CreateItahc(defraimp_watchtype.Consignee, Identifier));

            var after = DateTime.Now.Date;
            var start = Assert.Single(Assert.Single(recorded).GetAllConditions(), condition => condition.AttributeName == defraimp_WatchList.Fields.defraimp_StartDate);
            var date = Assert.IsType<DateTime>(Assert.Single(start.Values));
            Assert.InRange(date, before, after);
        }

        [Fact]
        public void CreateWatchFlags_SeveralFlags_EachGetTheirOwnReferenceToTheRecord()
        {
            var context = new CreateItahcWatchFlagsContext(this.ItahcReference(), RecordName, Identifier, Identifier, null, null, null);
            this.SetupWatchLists(
                this.CreateWatchList(this.watchListId, defraimp_watchtype.PlaceofOrigin, this.economicOperatorId),
                this.CreateWatchList(Guid.NewGuid(), defraimp_watchtype.PlaceofDestination, this.economicOperatorId));
            this.SetupEconomicOperator(defraimp_watchtype.PlaceofOrigin, this.economicOperatorId);
            this.SetupEconomicOperator(defraimp_watchtype.PlaceofDestination, this.economicOperatorId);

            this.creator.CreateWatchFlags(context);

            Assert.Equal(2, this.created.Count);
            Assert.NotSame(this.created[0].defraimp_ItahcId, this.created[1].defraimp_ItahcId);
            Assert.NotSame(context.Itahc, this.created[0].defraimp_ItahcId);
            Assert.Equal(this.recordId, this.created[1].defraimp_ItahcId.Id);
        }

        [Fact]
        public void CreateWatchFlags_WhenTheCreateFails_DoesNotCatchTheFailure()
        {
            this.SetupWatchLists(this.CreateWatchList(this.watchListId, defraimp_watchtype.Consignee, this.economicOperatorId));
            this.SetupEconomicOperator(defraimp_watchtype.Consignee, this.economicOperatorId);
            this.service.Setup(value => value.Create(It.IsAny<Entity>())).Throws(new InvalidOperationException("The create failed."));

            var exception = Assert.Throws<InvalidOperationException>(() => this.creator.CreateWatchFlags(this.CreateItahc(defraimp_watchtype.Consignee, Identifier)));

            Assert.Equal("The create failed.", exception.Message);
        }

        private EntityReference ItahcReference()
        {
            return new EntityReference(defraimp_itahc.EntityLogicalName, this.recordId);
        }

        private EntityReference NotificationReference()
        {
            return new EntityReference(defraimp_ImporterNotification.EntityLogicalName, this.recordId);
        }

        private CreateItahcWatchFlagsContext CreateItahc(defraimp_watchtype watchType, string identifier)
        {
            return new CreateItahcWatchFlagsContext(
                this.ItahcReference(),
                RecordName,
                watchType == defraimp_watchtype.PlaceofOrigin ? identifier : null,
                watchType == defraimp_watchtype.PlaceofDestination ? identifier : null,
                watchType == defraimp_watchtype.Consignee ? identifier : null,
                watchType == defraimp_watchtype.Transporter ? identifier : null,
                watchType == defraimp_watchtype.Veterinarian ? identifier : null);
        }

        private CreateNotificationWatchFlagsContext CreateNotification(defraimp_watchtype watchType, string identifier)
        {
            return new CreateNotificationWatchFlagsContext(
                this.NotificationReference(),
                RecordName,
                watchType == defraimp_watchtype.PlaceofOrigin ? identifier : null,
                watchType == defraimp_watchtype.PlaceofDestination ? identifier : null,
                watchType == defraimp_watchtype.Consignee ? identifier : null,
                watchType == defraimp_watchtype.Transporter ? identifier : null);
        }

        private defraimp_WatchList CreateWatchList(Guid id, defraimp_watchtype watchType, Guid operatorId)
        {
            var reference = new EntityReference(EconomicOperatorTestData.LogicalName(watchType), operatorId);
            var watchList = new defraimp_WatchList { Id = id, defraimp_WatchListId = id, defraimp_Name = WatchListName, defraimp_WatchType = watchType };
            switch (watchType)
            {
                case defraimp_watchtype.PlaceofOrigin:
                    watchList.defraimp_PlaceOfOriginId = reference;
                    break;
                case defraimp_watchtype.PlaceofDestination:
                    watchList.defraimp_PlaceOfDestinationId = reference;
                    break;
                case defraimp_watchtype.Consignee:
                    watchList.defraimp_ConsigneeId = reference;
                    break;
                case defraimp_watchtype.Transporter:
                    watchList.defraimp_TransporterId = reference;
                    break;
                default:
                    watchList.defraimp_VeterinarianId = reference;
                    break;
            }

            return watchList;
        }

        private IList<QueryExpression> SetupWatchLists(params defraimp_WatchList[] watchLists)
        {
            return this.service.ReturnsRecords(defraimp_WatchList.EntityLogicalName, watchLists);
        }

        private IList<QueryExpression> SetupEconomicOperator(defraimp_watchtype watchType, Guid id)
        {
            return this.service.ReturnsRecords(EconomicOperatorTestData.LogicalName(watchType), EconomicOperatorTestData.Create(watchType, id));
        }
    }
}
