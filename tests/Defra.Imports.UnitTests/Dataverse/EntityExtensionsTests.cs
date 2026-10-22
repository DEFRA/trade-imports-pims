namespace Defra.Imports.UnitTests.Dataverse
{
    using System;
    using Defra.Imports.Dataverse;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class EntityExtensionsTests
    {
        private readonly Guid id = Guid.NewGuid();

        [Fact]
        public void Clone_NullEntity_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => ((defraimp_WatchList)null).Clone());
        }

        [Fact]
        public void Clone_Entity_ReturnsDifferentInstanceOfTheSameType()
        {
            var original = new Entity("account", this.id);

            var clone = original.Clone();

            Assert.NotSame(original, clone);
            Assert.IsType<Entity>(clone);
            Assert.Equal(original.LogicalName, clone.LogicalName);
            Assert.Equal(this.id, clone.Id);
        }

        [Fact]
        public void Clone_TypedEntity_ReturnsTheSameEarlyBoundType()
        {
            var original = WatchListTestData.CreateWatchList(this.id, Guid.NewGuid(), name: "Name");

            var clone = original.Clone();

            Assert.NotSame(original, clone);
            Assert.Equal(this.id, clone.Id);
            Assert.Equal("Name", clone.defraimp_Name);
        }

        [Fact]
        public void Clone_AttributeAddedToClone_DoesNotChangeTheOriginal()
        {
            var original = new Entity("account", this.id) { ["name"] = "Original" };

            var clone = original.Clone();
            clone["name"] = "Changed";
            clone["extra"] = 1;

            Assert.Equal("Original", original["name"]);
            Assert.False(original.Contains("extra"));
        }

        [Fact]
        public void Clone_Entity_IsShallowSoAttributeValuesAreShared()
        {
            var reference = new EntityReference("contact", Guid.NewGuid());
            var original = new Entity("account", this.id) { ["primarycontactid"] = reference };

            var clone = original.Clone();

            Assert.Same(reference, clone["primarycontactid"]);
        }

        [Fact]
        public void Clone_TypedEntityWithoutAnIdentifier_DoesNotAddAnIdentifierAttribute()
        {
            var original = WatchListTestData.CreateWatchList(Guid.Empty, Guid.NewGuid(), name: "Name");

            var clone = original.Clone();

            Assert.Equal(original.Attributes.Count, clone.Attributes.Count);
            Assert.False(clone.Contains(defraimp_WatchList.Fields.defraimp_WatchListId));
        }

        [Fact]
        public void GetChanges_NullArguments_ThrowArgumentNullException()
        {
            var entity = new Entity("account");

            Assert.Throws<ArgumentNullException>(() => ((Entity)null).GetChanges(entity));
            Assert.Throws<ArgumentNullException>(() => entity.GetChanges(null));
        }

        [Fact]
        public void GetChanges_NothingChanged_ReturnsEntityWithNoAttributes()
        {
            var original = new Entity("account", this.id) { ["name"] = "Name" };

            var changes = original.Clone().GetChanges(original);

            Assert.Empty(changes.Attributes);
        }

        [Fact]
        public void GetChanges_ChangedAndNewValues_ReturnsOnlyThoseValues()
        {
            var original = new Entity("account", this.id) { ["name"] = "Name", ["telephone1"] = "0123" };
            var updated = original.Clone();
            updated["name"] = "Changed";
            updated["fax"] = "0456";

            var changes = updated.GetChanges(original);

            Assert.Equal(2, changes.Attributes.Count);
            Assert.Equal("Changed", changes["name"]);
            Assert.Equal("0456", changes["fax"]);
        }

        [Fact]
        public void GetChanges_Entity_KeepsTheLogicalNameAndIdentifier()
        {
            var original = new Entity("account", this.id);
            var updated = original.Clone();
            updated["name"] = "Changed";

            var changes = updated.GetChanges(original);

            Assert.Equal("account", changes.LogicalName);
            Assert.Equal(this.id, changes.Id);
        }

        [Fact]
        public void GetChanges_ValueRemovedFromUpdated_IsNotReported()
        {
            var original = new Entity("account", this.id) { ["name"] = "Name" };
            var updated = new Entity("account", this.id);

            Assert.Empty(updated.GetChanges(original).Attributes);
        }

        [Fact]
        public void GetChanges_NullReplacedByValue_IsReportedAsAChange()
        {
            var original = new Entity("account", this.id) { ["name"] = null };
            var updated = original.Clone();
            updated["name"] = "Name";

            Assert.Equal("Name", updated.GetChanges(original)["name"]);
        }

        [Fact]
        public void GetChanges_StringsWithTheSameText_AreNotChanges()
        {
            var original = new Entity("account", this.id) { ["name"] = new string("Name".ToCharArray()) };
            var updated = new Entity("account", this.id) { ["name"] = new string("Name".ToCharArray()) };

            Assert.Empty(updated.GetChanges(original).Attributes);
        }

        [Fact]
        public void GetChanges_StringsDifferingInCase_AreChanges()
        {
            var original = new Entity("account", this.id) { ["name"] = "name" };
            var updated = new Entity("account", this.id) { ["name"] = "Name" };

            Assert.Equal("Name", updated.GetChanges(original)["name"]);
        }

        [Fact]
        public void GetChanges_DifferentEntityReferenceInstancesForTheSameRecord_AreNotChanges()
        {
            var contactId = Guid.NewGuid();
            var original = new Entity("account", this.id) { ["primarycontactid"] = new EntityReference("contact", contactId) };
            var updated = new Entity("account", this.id) { ["primarycontactid"] = new EntityReference("contact", contactId) };

            Assert.Empty(updated.GetChanges(original).Attributes);
        }

        [Fact]
        public void GetChanges_EntityReferenceToADifferentRecord_IsAChange()
        {
            var original = new Entity("account", this.id) { ["primarycontactid"] = new EntityReference("contact", Guid.NewGuid()) };
            var updated = new Entity("account", this.id) { ["primarycontactid"] = new EntityReference("contact", Guid.NewGuid()) };

            Assert.Contains("primarycontactid", updated.GetChanges(original).Attributes.Keys);
        }

        [Fact]
        public void GetChanges_MoneyWithTheSameAmount_IsNotAChange()
        {
            var original = new Entity("account", this.id) { ["revenue"] = new Money(10m) };
            var updated = new Entity("account", this.id) { ["revenue"] = new Money(10m) };

            Assert.Empty(updated.GetChanges(original).Attributes);
        }

        [Fact]
        public void GetChanges_MoneyWithADifferentAmount_IsAChange()
        {
            var original = new Entity("account", this.id) { ["revenue"] = new Money(10m) };
            var updated = new Entity("account", this.id) { ["revenue"] = new Money(11m) };

            Assert.Contains("revenue", updated.GetChanges(original).Attributes.Keys);
        }

        [Fact]
        public void GetChanges_OptionSetValueWithTheSameValue_IsNotAChange()
        {
            var original = new Entity("account", this.id) { ["industrycode"] = new OptionSetValue(1) };
            var updated = new Entity("account", this.id) { ["industrycode"] = new OptionSetValue(1) };

            Assert.Empty(updated.GetChanges(original).Attributes);
        }

        [Fact]
        public void GetChanges_OptionSetValueWithADifferentValue_IsAChange()
        {
            var original = new Entity("account", this.id) { ["industrycode"] = new OptionSetValue(1) };
            var updated = new Entity("account", this.id) { ["industrycode"] = new OptionSetValue(2) };

            Assert.Contains("industrycode", updated.GetChanges(original).Attributes.Keys);
        }

        [Fact]
        public void GetChanges_DateTimeWithTheSameValue_IsNotAChange()
        {
            var original = new Entity("account", this.id) { ["lastusedincampaign"] = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc) };
            var updated = new Entity("account", this.id) { ["lastusedincampaign"] = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc) };

            Assert.Empty(updated.GetChanges(original).Attributes);
        }

        [Fact]
        public void GetChanges_DateTimeWithADifferentValue_IsAChange()
        {
            var original = new Entity("account", this.id) { ["lastusedincampaign"] = new DateTime(2026, 1, 2, 3, 4, 5, DateTimeKind.Utc) };
            var updated = new Entity("account", this.id) { ["lastusedincampaign"] = new DateTime(2026, 1, 2, 3, 4, 6, DateTimeKind.Utc) };

            Assert.Contains("lastusedincampaign", updated.GetChanges(original).Attributes.Keys);
        }
    }
}
