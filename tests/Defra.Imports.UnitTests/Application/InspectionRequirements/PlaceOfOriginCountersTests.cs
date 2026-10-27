namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using System.Linq;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class PlaceOfOriginCountersTests
    {
        private readonly InspectionDataverse dataverse = new InspectionDataverse();
        private readonly defraimp_placeoforigin place = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Bronze, applicationCounter: 3, quotaCounter: 2);
        private readonly Guid missingId = Guid.NewGuid();
        private readonly PlaceOfOriginCounters counters;

        public PlaceOfOriginCountersTests()
        {
            this.dataverse.Seed(this.place);
            this.counters = new PlaceOfOriginCounters(new PlaceOfOriginQueries(new ImportsContext(this.dataverse.Service.Object)), this.dataverse.Service.Object);
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PlaceOfOriginCounters(null, this.dataverse.Service.Object));
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new PlaceOfOriginCounters(new PlaceOfOriginQueries(new ImportsContext(this.dataverse.Service.Object)), null));
        }

        [Fact]
        public void Find_Exists_ReturnsThePlaceOfOrigin()
        {
            Assert.Equal(this.place.Id, this.counters.Find(this.place.Id).Id);
        }

        [Fact]
        public void Find_Missing_ReturnsNull()
        {
            Assert.Null(this.counters.Find(this.missingId));
        }

        [Fact]
        public void GetApplicationCounterValue_Exists_ReturnsTheCounter()
        {
            Assert.Equal(3, this.counters.GetApplicationCounterValue(this.place.Id));
        }

        [Fact]
        public void GetApplicationCounterValue_NullCounter_ReturnsZero()
        {
            var nullCounter = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Bronze, applicationCounter: null);
            this.dataverse.Seed(nullCounter);

            Assert.Equal(0, this.counters.GetApplicationCounterValue(nullCounter.Id));
        }

        [Fact]
        public void GetApplicationCounterValue_Missing_ReturnsZero()
        {
            Assert.Equal(0, this.counters.GetApplicationCounterValue(this.missingId));
        }

        [Fact]
        public void GetQuotaCounterValue_Exists_ReturnsTheCounter()
        {
            Assert.Equal(2, this.counters.GetQuotaCounterValue(this.place.Id));
        }

        [Fact]
        public void GetQuotaCounterValue_NullCounter_ReturnsZero()
        {
            var nullCounter = InspectionTestData.PlaceOfOrigin(defraimp_trustlevel.Bronze, quotaCounter: null);
            this.dataverse.Seed(nullCounter);

            Assert.Equal(0, this.counters.GetQuotaCounterValue(nullCounter.Id));
        }

        [Fact]
        public void GetQuotaCounterValue_Missing_ReturnsZero()
        {
            Assert.Equal(0, this.counters.GetQuotaCounterValue(this.missingId));
        }

        [Fact]
        public void IncrementApplicationCounter_ReadsThenSendsOneUpdateWithTheNewValue()
        {
            this.counters.IncrementApplicationCounter(this.place.Id);

            this.AssertSingleUpdate("defraimp_applicationcounter", 4, "Query:defraimp_placeoforigin", "Update:defraimp_placeoforigin");
        }

        [Fact]
        public void DecrementApplicationCounter_ReadsThenSendsOneUpdateWithTheNewValue()
        {
            this.counters.DecrementApplicationCounter(this.place.Id);

            this.AssertSingleUpdate("defraimp_applicationcounter", 2, "Query:defraimp_placeoforigin", "Update:defraimp_placeoforigin");
        }

        [Fact]
        public void SetApplicationCounter_SendsOneUpdateWithoutReading()
        {
            this.counters.SetApplicationCounter(this.place.Id, 7);

            this.AssertSingleUpdate("defraimp_applicationcounter", 7, "Update:defraimp_placeoforigin");
        }

        [Fact]
        public void IncrementQuotaCounter_ReadsThenSendsOneUpdateWithTheNewValue()
        {
            this.counters.IncrementQuotaCounter(this.place.Id);

            this.AssertSingleUpdate("defraimp_inspectionquotacounter", 3, "Query:defraimp_placeoforigin", "Update:defraimp_placeoforigin");
        }

        [Fact]
        public void DecrementQuotaCounter_ReadsThenSendsOneUpdateWithTheNewValue()
        {
            this.counters.DecrementQuotaCounter(this.place.Id);

            this.AssertSingleUpdate("defraimp_inspectionquotacounter", 1, "Query:defraimp_placeoforigin", "Update:defraimp_placeoforigin");
        }

        [Fact]
        public void IncrementApplicationCounter_Missing_SendsAnUpdateWithOne()
        {
            this.counters.IncrementApplicationCounter(this.missingId);

            Assert.Single(this.dataverse.Updates);
            Assert.Equal(1, this.dataverse.Updates[0].ToEntity<defraimp_placeoforigin>().defraimp_ApplicationCounter);
        }

        [Fact]
        public void DecrementApplicationCounter_Missing_SendsAnUpdateWithMinusOne()
        {
            this.counters.DecrementApplicationCounter(this.missingId);

            Assert.Single(this.dataverse.Updates);
            Assert.Equal(-1, this.dataverse.Updates[0].ToEntity<defraimp_placeoforigin>().defraimp_ApplicationCounter);
        }

        private void AssertSingleUpdate(string counterAttribute, int expectedValue, params string[] expectedEvents)
        {
            Assert.Equal(expectedEvents, this.dataverse.Events);
            Assert.Single(this.dataverse.Updates);

            var update = this.dataverse.Updates[0];
            Assert.Equal(this.place.Id, update.Id);
            Assert.Equal(
                new[] { counterAttribute, "defraimp_placeoforiginid" },
                update.Attributes.Select(attribute => attribute.Key).OrderBy(key => key, StringComparer.Ordinal).ToArray());

            var stored = update.ToEntity<defraimp_placeoforigin>();
            var value = counterAttribute == "defraimp_applicationcounter" ? stored.defraimp_ApplicationCounter : stored.defraimp_InspectionQuotaCounter;
            Assert.Equal(expectedValue, value);
        }
    }
}
