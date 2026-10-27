namespace Defra.Imports.UnitTests.Dataverse.Queries
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Client;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    public sealed class AutonumberQueriesTests
    {
        private readonly Guid autonumberId = Guid.NewGuid();
        private readonly Mock<IOrganizationService> service = new Mock<IOrganizationService>(MockBehavior.Strict);
        private readonly ImportsContext context;
        private readonly AutonumberQueries queries;

        public AutonumberQueriesTests()
        {
            this.context = new ImportsContext(this.service.Object);
            this.queries = new AutonumberQueries(this.context);
        }

        [Fact]
        public void Constructor_NullContext_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new AutonumberQueries(null));
        }

        [Fact]
        public void Constructor_Context_StopsTrackingBecauseTheQueriesAreReadOnly()
        {
            Assert.Equal(MergeOption.NoTracking, this.context.MergeOption);
        }

        [Fact]
        public void FindByKey_MatchingAutonumber_ReturnsItsIdentifierKeyAndCurrentNumber()
        {
            this.service.ReturnsRecords(
                defraimp_autonumber.EntityLogicalName,
                new defraimp_autonumber { Id = this.autonumberId, defraimp_Key = "KEY", defraimp_CurrentNumber = 7 });

            var autonumber = this.queries.FindByKey("KEY");

            Assert.Equal(this.autonumberId, autonumber.Id);
            Assert.Equal("KEY", autonumber.defraimp_Key);
            Assert.Equal(7, autonumber.defraimp_CurrentNumber);
        }

        [Fact]
        public void FindByKey_MatchingAutonumber_FiltersByKeyAndRetrievesOnlyTheThreeColumns()
        {
            var recorded = this.service.ReturnsRecords(
                defraimp_autonumber.EntityLogicalName,
                new defraimp_autonumber { Id = this.autonumberId, defraimp_Key = "KEY", defraimp_CurrentNumber = 7 });

            this.queries.FindByKey("KEY");

            var query = Assert.Single(recorded);
            Assert.True(query.UsesOnlyAndFilters());
            var condition = Assert.Single(query.GetAllConditions());
            Assert.Equal(defraimp_autonumber.Fields.defraimp_Key, condition.AttributeName);
            Assert.Equal(ConditionOperator.Equal, condition.Operator);
            Assert.Equal("KEY", Assert.Single(condition.Values));
            Assert.False(query.ColumnSet.AllColumns);
            Assert.Equal(
                new[]
                {
                    defraimp_autonumber.Fields.defraimp_autonumberId,
                    defraimp_autonumber.Fields.defraimp_Key,
                    defraimp_autonumber.Fields.defraimp_CurrentNumber,
                },
                query.ColumnSet.Columns.ToArray());
        }

        [Fact]
        public void FindByKey_SeveralMatches_ReturnsTheFirst()
        {
            this.service.ReturnsRecords(
                defraimp_autonumber.EntityLogicalName,
                new defraimp_autonumber { Id = this.autonumberId, defraimp_Key = "KEY" },
                new defraimp_autonumber { Id = Guid.NewGuid(), defraimp_Key = "KEY" });

            Assert.Equal(this.autonumberId, this.queries.FindByKey("KEY").Id);
        }

        [Fact]
        public void FindByKey_NoMatch_ReturnsNull()
        {
            this.service.ReturnsRecords(defraimp_autonumber.EntityLogicalName);

            Assert.Null(this.queries.FindByKey("KEY"));
        }
    }
}
