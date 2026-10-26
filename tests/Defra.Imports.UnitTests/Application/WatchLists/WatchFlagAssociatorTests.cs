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

    public sealed class WatchFlagAssociatorTests
    {
        private readonly Guid importApplicationId = Guid.NewGuid();
        private readonly Guid primaryRecordId = Guid.NewGuid();
        private readonly Mock<IOrganizationService> service = new Mock<IOrganizationService>(MockBehavior.Strict);
        private readonly List<defraimp_WatchFlag> updated = new List<defraimp_WatchFlag>();
        private readonly WatchFlagAssociator associator;

        public WatchFlagAssociatorTests()
        {
            this.associator = new WatchFlagAssociator(new WatchFlagQueries(new ImportsContext(this.service.Object)), this.service.Object);
            this.service
                .Setup(value => value.Update(It.IsAny<Entity>()))
                .Callback<Entity>(entity => this.updated.Add((defraimp_WatchFlag)entity));
        }

        public enum PrimaryRecord
        {
            Itahc,
            ImporterNotification,
        }

        public static IEnumerable<object[]> PrimaryRecords => new[]
        {
            new object[] { PrimaryRecord.Itahc },
            new object[] { PrimaryRecord.ImporterNotification },
        };

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new WatchFlagAssociator(null, this.service.Object));
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            var queries = new WatchFlagQueries(new ImportsContext(this.service.Object));

            Assert.Throws<ArgumentNullException>(() => new WatchFlagAssociator(queries, null));
        }

        [Fact]
        public void AssociateWatchFlags_NullItahcContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.associator.AssociateWatchFlags((AssociateItahcWatchFlagsContext)null));
        }

        [Fact]
        public void AssociateWatchFlags_NullNotificationContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.associator.AssociateWatchFlags((AssociateNotificationWatchFlagsContext)null));
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_NoPrimaryRecord_ClearsTheLinkOfEachLinkedWatchFlagAndReadsNothingElse(PrimaryRecord primary)
        {
            var linked = WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId);
            var queries = this.SetupReads(new Entity[] { linked });

            this.Associate(primary, null);

            Assert.Single(queries);
            var update = Assert.Single(this.updated);
            Assert.Equal(linked.defraimp_WatchFlagId, update.defraimp_WatchFlagId);
            Assert.Null(update.defraimp_ImportApplicationId);
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_NoPrimaryRecordAndNoLinkedWatchFlags_UpdatesNothing(PrimaryRecord primary)
        {
            this.SetupReads(new Entity[0]);

            this.Associate(primary, null);

            Assert.Empty(this.updated);
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_PrimaryRecordWithUnlinkedWatchFlags_LinksThemToTheImportApplication(PrimaryRecord primary)
        {
            var first = WatchFlagTestData.WatchFlag();
            var second = WatchFlagTestData.WatchFlag();
            this.SetupReads(new Entity[0], first, second);

            this.Associate(primary, this.primaryRecordId);

            Assert.Equal(new[] { first.defraimp_WatchFlagId, second.defraimp_WatchFlagId }, this.updated.Select(update => update.defraimp_WatchFlagId).ToArray());
            Assert.All(this.updated, update =>
            {
                Assert.Equal(this.importApplicationId, update.defraimp_ImportApplicationId.Id);
                Assert.Equal(defraimp_importapplication.EntityLogicalName, update.defraimp_ImportApplicationId.LogicalName);
            });
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_PrimaryRecordWithAWatchFlagLinkedToAnotherImportApplication_MovesItToTheImportApplication(PrimaryRecord primary)
        {
            var other = WatchFlagTestData.WatchFlag(importApplicationId: Guid.NewGuid());
            this.SetupReads(new Entity[0], other);

            this.Associate(primary, this.primaryRecordId);

            var update = Assert.Single(this.updated);
            Assert.Equal(other.defraimp_WatchFlagId, update.defraimp_WatchFlagId);
            Assert.Equal(this.importApplicationId, update.defraimp_ImportApplicationId.Id);
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_PrimaryRecordWithAWatchFlagAlreadyLinkedToTheImportApplication_DoesNotUpdateIt(PrimaryRecord primary)
        {
            this.SetupReads(new Entity[0], WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId));

            this.Associate(primary, this.primaryRecordId);

            Assert.Empty(this.updated);
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_PrimaryRecordWithNoWatchFlags_UpdatesNothing(PrimaryRecord primary)
        {
            this.SetupReads(new Entity[0], new Entity[0]);

            this.Associate(primary, this.primaryRecordId);

            Assert.Empty(this.updated);
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_LinkedWatchFlagsAndAPrimaryRecord_InterleavesTheReadsAndWritesInOrder(PrimaryRecord primary)
        {
            var linkedFirst = WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId);
            var linkedSecond = WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId);
            var unlinked = WatchFlagTestData.WatchFlag();
            this.SetupReads(new Entity[] { linkedFirst, linkedSecond }, unlinked);

            this.Associate(primary, this.primaryRecordId);

            var calls = this.service.Invocations.Select(invocation => invocation.Method.Name).ToArray();
            Assert.Equal(new[] { "Execute", "Update", "Update", "Execute", "Update" }, calls);
            Assert.Equal(
                new[] { linkedFirst.defraimp_WatchFlagId, linkedSecond.defraimp_WatchFlagId, unlinked.defraimp_WatchFlagId },
                this.updated.Select(update => update.defraimp_WatchFlagId).ToArray());
            Assert.Null(this.updated[0].defraimp_ImportApplicationId);
            Assert.Null(this.updated[1].defraimp_ImportApplicationId);
            Assert.Equal(this.importApplicationId, this.updated[2].defraimp_ImportApplicationId.Id);
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_WatchFlagReadAfterItsLinkWasCleared_IsUpdatedTwice(PrimaryRecord primary)
        {
            var watchFlag = WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId);
            this.SetupReads(new Entity[] { watchFlag }, WatchFlagTestData.WatchFlag(watchFlag.defraimp_WatchFlagId));

            this.Associate(primary, this.primaryRecordId);

            Assert.Equal(2, this.updated.Count);
            Assert.Null(this.updated[0].defraimp_ImportApplicationId);
            Assert.Equal(this.importApplicationId, this.updated[1].defraimp_ImportApplicationId.Id);
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_Updates_HoldOnlyTheIdentifierAndTheImportApplicationLookup(PrimaryRecord primary)
        {
            this.SetupReads(new Entity[] { WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId) }, WatchFlagTestData.WatchFlag());

            this.Associate(primary, this.primaryRecordId);

            var expected = new[] { defraimp_WatchFlag.Fields.defraimp_WatchFlagId, defraimp_WatchFlag.Fields.defraimp_ImportApplicationId };
            Assert.All(this.updated, update => Assert.Equal(expected.OrderBy(name => name), update.Attributes.Keys.OrderBy(name => name)));
            Assert.All(this.updated, update => Assert.Equal(update.defraimp_WatchFlagId, update.Id));
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_SeveralWatchFlags_GiveEachItsOwnImportApplicationReference(PrimaryRecord primary)
        {
            this.SetupReads(new Entity[0], WatchFlagTestData.WatchFlag(), WatchFlagTestData.WatchFlag());

            this.Associate(primary, this.primaryRecordId);

            Assert.NotSame(this.updated[0].defraimp_ImportApplicationId, this.updated[1].defraimp_ImportApplicationId);
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_WatchFlagsThatWereRead_AreNotModified(PrimaryRecord primary)
        {
            var linked = WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId);
            var unlinked = WatchFlagTestData.WatchFlag();
            this.SetupReads(new Entity[] { linked }, unlinked);

            this.Associate(primary, this.primaryRecordId);

            Assert.Equal(this.importApplicationId, linked.defraimp_ImportApplicationId.Id);
            Assert.Null(unlinked.defraimp_ImportApplicationId);
        }

        [Fact]
        public void AssociateWatchFlags_Itahc_ReadsTheItahcWatchFlagsOfTheImportApplicationThenThoseOfThePrimaryItahc()
        {
            var queries = this.SetupReads(new Entity[0], new Entity[0]);

            this.associator.AssociateWatchFlags(new AssociateItahcWatchFlagsContext(this.importApplicationId, this.primaryRecordId));

            Assert.Equal(2, queries.Count);
            this.AssertLinkedQuery(queries[0], defraimp_WatchFlag.Fields.defraimp_ItahcId);
            this.AssertPrimaryQuery(queries[1], defraimp_WatchFlag.Fields.defraimp_ItahcId);
        }

        [Fact]
        public void AssociateWatchFlags_Notification_ReadsTheNotificationWatchFlagsOfTheImportApplicationThenThoseOfThePrimaryNotification()
        {
            var queries = this.SetupReads(new Entity[0], new Entity[0]);

            this.associator.AssociateWatchFlags(new AssociateNotificationWatchFlagsContext(this.importApplicationId, this.primaryRecordId));

            Assert.Equal(2, queries.Count);
            this.AssertLinkedQuery(queries[0], defraimp_WatchFlag.Fields.defraimp_ImporterNotificationId);
            this.AssertPrimaryQuery(queries[1], defraimp_WatchFlag.Fields.defraimp_ImporterNotificationId);
        }

        [Theory]
        [MemberData(nameof(PrimaryRecords))]
        public void AssociateWatchFlags_WhenAnUpdateFails_DoesNotCatchTheFailure(PrimaryRecord primary)
        {
            this.SetupReads(new Entity[] { WatchFlagTestData.WatchFlag(importApplicationId: this.importApplicationId) });
            this.service.Setup(value => value.Update(It.IsAny<Entity>())).Throws(new InvalidOperationException("The update failed."));

            var exception = Assert.Throws<InvalidOperationException>(() => this.Associate(primary, null));

            Assert.Equal("The update failed.", exception.Message);
        }

        private IList<QueryExpression> SetupReads(Entity[] linkedWatchFlags, params Entity[] primaryRecordWatchFlags)
        {
            // The first read finds the watch flags linked to the import application and the second finds those of the
            // primary record. Without a primary record only the first is sent, and the queries returned show it.
            return this.service.ReturnsRecordsInSequence(defraimp_WatchFlag.EntityLogicalName, linkedWatchFlags, primaryRecordWatchFlags);
        }

        private void Associate(PrimaryRecord primary, Guid? primaryId)
        {
            if (primary == PrimaryRecord.Itahc)
            {
                this.associator.AssociateWatchFlags(new AssociateItahcWatchFlagsContext(this.importApplicationId, primaryId));
            }
            else
            {
                this.associator.AssociateWatchFlags(new AssociateNotificationWatchFlagsContext(this.importApplicationId, primaryId));
            }
        }

        private void AssertLinkedQuery(QueryExpression query, string lookup)
        {
            Assert.True(query.UsesOnlyAndFilters());
            var conditions = query.GetAllConditions();
            var application = Assert.Single(conditions, condition => condition.AttributeName == defraimp_WatchFlag.Fields.defraimp_ImportApplicationId);
            Assert.Equal(ConditionOperator.Equal, application.Operator);
            Assert.Equal(this.importApplicationId, Assert.Single(application.Values));
            var lookupCondition = Assert.Single(conditions, condition => condition.AttributeName == lookup);
            Assert.Equal(ConditionOperator.NotNull, lookupCondition.Operator);
        }

        private void AssertPrimaryQuery(QueryExpression query, string lookup)
        {
            var condition = Assert.Single(query.GetAllConditions());
            Assert.Equal(lookup, condition.AttributeName);
            Assert.Equal(ConditionOperator.Equal, condition.Operator);
            Assert.Equal(this.primaryRecordId, Assert.Single(condition.Values));
        }
    }
}
