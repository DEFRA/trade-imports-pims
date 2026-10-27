namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using System.Linq;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class AutonumberCountersTests
    {
        private const string MissingKey = "missing_key";

        private readonly InspectionDataverse dataverse = new InspectionDataverse();
        private readonly defraimp_autonumber counter = InspectionTestData.Autonumber("counter_key", 5);
        private readonly InspectionServices services;

        public AutonumberCountersTests()
        {
            this.dataverse.Seed(this.counter);
            this.services = new InspectionServices(this.dataverse);
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AutonumberCounters(null, this.dataverse.Service.Object));
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AutonumberCounters(new AutonumberQueries(new ImportsContext(this.dataverse.Service.Object)), null));
        }

        [Fact]
        public void GetRecord_KeyExists_ReturnsTheRecord()
        {
            var record = this.services.AutonumberCounters.GetRecord("counter_key");

            Assert.Equal(this.counter.Id, record.Id);
            Assert.Equal(5, record.defraimp_CurrentNumber);
        }

        [Fact]
        public void GetRecord_NoKey_ReturnsNull()
        {
            Assert.Null(this.services.AutonumberCounters.GetRecord(MissingKey));
        }

        [Fact]
        public void GetValue_KeyExists_ReturnsTheNumber()
        {
            Assert.Equal(5, this.services.AutonumberCounters.GetValue("counter_key"));
        }

        [Fact]
        public void GetValue_NullNumber_ReturnsZero()
        {
            var nullNumber = InspectionTestData.Autonumber("null_key", null);
            this.dataverse.Seed(nullNumber);

            Assert.Equal(0, this.services.AutonumberCounters.GetValue("null_key"));
        }

        [Fact]
        public void GetValue_NoKey_ThrowsInvalidPluginExecutionException()
        {
            var exception = Assert.Throws<InvalidPluginExecutionException>(() => this.services.AutonumberCounters.GetValue(MissingKey));

            Assert.Equal("Autonumber record key not found.", exception.Message);
        }

        [Fact]
        public void Increment_KeyExists_ReadsThenSendsOneUpdateWithTheNewValue()
        {
            this.services.AutonumberCounters.Increment("counter_key");

            this.AssertSingleUpdate(6);
        }

        [Fact]
        public void Decrement_KeyExists_ReadsThenSendsOneUpdateWithTheNewValue()
        {
            this.services.AutonumberCounters.Decrement("counter_key");

            this.AssertSingleUpdate(4);
        }

        [Fact]
        public void Set_KeyExists_ReadsThenSendsOneUpdateWithTheNewValue()
        {
            this.services.AutonumberCounters.Set("counter_key", 9);

            this.AssertSingleUpdate(9);
        }

        [Fact]
        public void Increment_NullNumber_SendsAnUpdateWhoseNumberIsStillNull()
        {
            var nullNumber = InspectionTestData.Autonumber("null_key", null);
            this.dataverse.Seed(nullNumber);

            this.services.AutonumberCounters.Increment("null_key");

            Assert.Single(this.dataverse.Updates);
            Assert.Null(this.dataverse.Updates[0].ToEntity<defraimp_autonumber>().defraimp_CurrentNumber);
        }

        [Fact]
        public void Increment_MissingKey_ThrowsNullReferenceExceptionWithoutUpdating()
        {
            Assert.Throws<NullReferenceException>(() => this.services.AutonumberCounters.Increment(MissingKey));

            Assert.Empty(this.dataverse.Updates);
        }

        [Fact]
        public void Decrement_MissingKey_ThrowsNullReferenceExceptionWithoutUpdating()
        {
            Assert.Throws<NullReferenceException>(() => this.services.AutonumberCounters.Decrement(MissingKey));

            Assert.Empty(this.dataverse.Updates);
        }

        [Fact]
        public void Set_MissingKey_ThrowsNullReferenceExceptionWithoutUpdating()
        {
            Assert.Throws<NullReferenceException>(() => this.services.AutonumberCounters.Set(MissingKey, 1));

            Assert.Empty(this.dataverse.Updates);
        }

        private void AssertSingleUpdate(int expectedValue)
        {
            Assert.Equal(new[] { "Query:defraimp_autonumber", "Update:defraimp_autonumber" }, this.dataverse.Events);
            Assert.Single(this.dataverse.Updates);

            var update = this.dataverse.Updates[0];
            Assert.Equal(this.counter.Id, update.Id);
            Assert.Equal(
                new[] { "defraimp_autonumberid", "defraimp_currentnumber", "defraimp_key" },
                update.Attributes.Select(attribute => attribute.Key).OrderBy(key => key, StringComparer.Ordinal).ToArray());
            Assert.Equal(expectedValue, update.ToEntity<defraimp_autonumber>().defraimp_CurrentNumber);
        }
    }
}
