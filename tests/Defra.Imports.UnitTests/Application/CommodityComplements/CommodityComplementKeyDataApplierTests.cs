namespace Defra.Imports.UnitTests.Application.CommodityComplements
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Application.CommodityComplements;
    using Defra.Imports.Application.Constants;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Projections;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    public sealed class CommodityComplementKeyDataApplierTests
    {
        private const string AllowedKey = KeyDataPairKeys.NumberOfAnimals;
        private const string AllowedAttribute = defraimp_commoditycomplement.Fields.defraimp_NumberofAnimals;
        private const string ComplementSetName = "42";
        private const string UnmappedKey = "some_other_key";

        private readonly Guid importerNotificationId = Guid.NewGuid();
        private readonly Guid parameterSetId = Guid.NewGuid();
        private readonly Guid commodityComplementId = Guid.NewGuid();
        private readonly Mock<IOrganizationService> service = new Mock<IOrganizationService>(MockBehavior.Strict);
        private readonly Mock<ILogWriter> logger = new Mock<ILogWriter>(MockBehavior.Strict);
        private readonly CommodityComplementKeyDataApplier applier;
        private Entity updated;

        public CommodityComplementKeyDataApplierTests()
        {
            this.applier = new CommodityComplementKeyDataApplier(
                new CommodityComplementQueries(new ImportsContext(this.service.Object)),
                this.service.Object,
                this.logger.Object);
            this.service
                .Setup(value => value.Update(It.IsAny<Entity>()))
                .Callback<Entity>(entity => this.updated = entity);
            this.logger.Setup(value => value.Log(It.IsAny<Severity>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Fact]
        public void Constructor_NullQueries_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new CommodityComplementKeyDataApplier(null, this.service.Object, this.logger.Object));
        }

        [Fact]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            var queries = new CommodityComplementQueries(new ImportsContext(this.service.Object));

            Assert.Throws<ArgumentNullException>(() => new CommodityComplementKeyDataApplier(queries, null, this.logger.Object));
        }

        [Fact]
        public void Constructor_NullLogger_ThrowsArgumentNullException()
        {
            var queries = new CommodityComplementQueries(new ImportsContext(this.service.Object));

            Assert.Throws<ArgumentNullException>(() => new CommodityComplementKeyDataApplier(queries, this.service.Object, null));
        }

        [Fact]
        public void Apply_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.applier.Apply(null));
        }

        [Fact]
        public void Apply_MatchingCommodityComplement_UpdatesOnlyTheMappedAttribute()
        {
            this.ReturnsCommodityComplement();

            this.applier.Apply(this.CreateContext());

            var update = Assert.IsType<defraimp_commoditycomplement>(this.updated);
            Assert.Equal(this.commodityComplementId, update.Id);
            Assert.Equal("12", update[AllowedAttribute]);
            Assert.Equal(2, update.Attributes.Count);
            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Once);
        }

        [Fact]
        public void Apply_MatchingCommodityComplement_DoesNotTraceAnything()
        {
            this.ReturnsCommodityComplement();

            this.applier.Apply(this.CreateContext());

            this.logger.VerifyNoOtherCalls();
        }

        [Fact]
        public void Apply_ParameterSetNameOnTheReference_SearchesByNotificationAndComplementIdentifier()
        {
            var queries = this.ReturnsCommodityComplement();

            this.applier.Apply(this.CreateContext());

            var conditions = Assert.Single(queries).GetAllConditions();
            Assert.Contains(conditions, condition => condition.AttributeName == defraimp_commoditycomplement.Fields.defraimp_ImporterNotificationId && Equals(condition.Values.Single(), this.importerNotificationId));
            Assert.Contains(conditions, condition => condition.AttributeName == defraimp_commoditycomplement.Fields.defraimp_complementid && Equals(condition.Values.Single(), 42));
        }

        [Fact]
        public void Apply_ParameterSetNameOnTheReference_DoesNotReadTheParameterSet()
        {
            this.ReturnsCommodityComplement();

            this.applier.Apply(this.CreateContext());

            this.service.VerifyQueried(defraimp_commoditycomplementparameterset.EntityLogicalName, Times.Never());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Apply_NoParameterSetNameOnTheReference_ReadsTheNameAndUpdates(string referenceName)
        {
            this.ReturnsParameterSet(ComplementSetName);
            var queries = this.ReturnsCommodityComplement();

            this.applier.Apply(this.CreateContext(complementSetName: referenceName));

            this.service.VerifyQueried(defraimp_commoditycomplementparameterset.EntityLogicalName, Times.Once());
            Assert.Contains(Assert.Single(queries).GetAllConditions(), condition => condition.AttributeName == defraimp_commoditycomplement.Fields.defraimp_complementid && Equals(condition.Values.Single(), 42));
            Assert.Equal(this.commodityComplementId, this.updated.Id);
        }

        [Fact]
        public void Apply_NoParameterSetNameOnTheReference_ReadsOnlyTheNameColumn()
        {
            var parameterSetQueries = this.ReturnsParameterSet(ComplementSetName);
            this.ReturnsCommodityComplement();

            this.applier.Apply(this.CreateContext(complementSetName: null));

            var columns = Assert.Single(parameterSetQueries).ColumnSet;
            Assert.False(columns.AllColumns);
            Assert.Equal(new[] { defraimp_commoditycomplementparameterset.Fields.defraimp_name }, columns.Columns);
        }

        [Fact]
        public void Apply_BlankParameterSetNameRead_TracesAWarningAndUpdatesNothing()
        {
            this.ReturnsParameterSet("   ");

            this.applier.Apply(this.CreateContext(complementSetName: null));

            this.VerifyWarning("is not a valid complement ID");
            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
            this.service.VerifyQueried(defraimp_commoditycomplement.EntityLogicalName, Times.Never());
        }

        [Fact]
        public void Apply_ParameterSetNotFound_ThrowsInvalidOperationException()
        {
            this.service.ReturnsRecords(defraimp_commoditycomplementparameterset.EntityLogicalName);

            Assert.Throws<InvalidOperationException>(
                () => this.applier.Apply(this.CreateContext(complementSetName: null)));
            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Theory]
        [InlineData("not-a-number")]
        [InlineData("4.2")]
        [InlineData("99999999999")]
        public void Apply_NonNumericComplementSetName_TracesAWarningAndUpdatesNothing(string name)
        {
            this.applier.Apply(this.CreateContext(complementSetName: name));

            this.VerifyWarning($"Cannot apply key data pair '{AllowedKey}' - commodity complement parameter set name '{name}' is not a valid complement ID.");
            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
            this.service.VerifyQueriedAny(Times.Never());
        }

        [Fact]
        public void Apply_NoMatchingCommodityComplement_TracesAWarningAndUpdatesNothing()
        {
            this.service.ReturnsRecords(defraimp_commoditycomplement.EntityLogicalName);

            this.applier.Apply(this.CreateContext());

            this.VerifyWarning($"Cannot apply key data pair '{AllowedKey}' - no matching commodity complement found for importer notification {this.importerNotificationId} and complement ID 42.");
            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void Apply_NoImporterNotification_TracesAWarningAndReadsNothing()
        {
            this.applier.Apply(new ApplyCommodityComplementKeyDataContext(AllowedKey, "12", null, this.parameterSetId, ComplementSetName));

            this.VerifyWarning($"Cannot apply key data pair '{AllowedKey}' - it is missing the importer notification or commodity complement parameter set reference.");
            this.service.VerifyQueriedAny(Times.Never());
            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void Apply_NoParameterSetReference_TracesAWarningAndReadsNothing()
        {
            this.applier.Apply(new ApplyCommodityComplementKeyDataContext(AllowedKey, "12", this.importerNotificationId, null, null));

            this.VerifyWarning("it is missing the importer notification or commodity complement parameter set reference.");
            this.service.VerifyQueriedAny(Times.Never());
            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(UnmappedKey)]
        public void Apply_KeyIsNotUnderstood_DoesNothing(string key)
        {
            this.applier.Apply(this.CreateContext(name: key));

            this.logger.VerifyNoOtherCalls();
            this.service.VerifyQueriedAny(Times.Never());
            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void Apply_KeyDiffersOnlyByCase_DoesNothing()
        {
            this.applier.Apply(this.CreateContext(name: AllowedKey.ToUpperInvariant()));

            this.logger.VerifyNoOtherCalls();
            this.service.VerifyQueriedAny(Times.Never());
            this.service.Verify(value => value.Update(It.IsAny<Entity>()), Times.Never);
        }

        [Fact]
        public void Apply_UpdateFails_PropagatesTheFailure()
        {
            this.ReturnsCommodityComplement();
            this.service.Setup(value => value.Update(It.IsAny<Entity>())).Throws<InvalidOperationException>();

            Assert.Throws<InvalidOperationException>(() => this.applier.Apply(this.CreateContext()));
        }

        [Fact]
        public void Apply_WarningTraced_UsesTheApplierAsTheTag()
        {
            this.service.ReturnsRecords(defraimp_commoditycomplement.EntityLogicalName);

            this.applier.Apply(this.CreateContext());

            this.logger.Verify(
                value => value.Log(Severity.Warning, nameof(CommodityComplementKeyDataApplier), It.IsAny<string>()),
                Times.Once);
        }

        private ApplyCommodityComplementKeyDataContext CreateContext(string name = AllowedKey, string complementSetName = ComplementSetName)
        {
            return new ApplyCommodityComplementKeyDataContext(name, "12", this.importerNotificationId, this.parameterSetId, complementSetName);
        }

        private IList<QueryExpression> ReturnsCommodityComplement()
        {
            return this.service.ReturnsRecords(
                defraimp_commoditycomplement.EntityLogicalName,
                new defraimp_commoditycomplement { Id = this.commodityComplementId });
        }

        private IList<QueryExpression> ReturnsParameterSet(string name)
        {
            return this.service.ReturnsRecords(
                defraimp_commoditycomplementparameterset.EntityLogicalName,
                new defraimp_commoditycomplementparameterset { Id = this.parameterSetId, defraimp_name = name });
        }

        private void VerifyWarning(string messageFragment)
        {
            this.logger.Verify(
                value => value.Log(Severity.Warning, It.IsAny<string>(), It.Is<string>(message => message.Contains(messageFragment))),
                Times.Once);
        }
    }
}
