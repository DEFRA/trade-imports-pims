namespace Defra.Imports.UnitTests.Application.InspectionRequirements
{
    using System;
    using Defra.Imports.Application.InspectionRequirements;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class InspectionStrategyFactoryTests
    {
        private readonly InspectionServices services = new InspectionServices(new InspectionDataverse().WithReferenceData());

        [Theory]
        [InlineData("p1", typeof(P1InspectionStrategy))]
        [InlineData("P1", typeof(P1InspectionStrategy))]
        [InlineData("p2", typeof(P2InspectionStrategy))]
        [InlineData("P2", typeof(P2InspectionStrategy))]
        [InlineData("p3", typeof(P3InspectionStrategy))]
        [InlineData("P3", typeof(P3InspectionStrategy))]
        [InlineData("tb", typeof(TbInspectionStrategy))]
        [InlineData("Tb", typeof(TbInspectionStrategy))]
        [InlineData("Unknown Priority", typeof(UnknownInspectionStrategy))]
        [InlineData("", typeof(UnknownInspectionStrategy))]
        public void GetStrategy_ReturnsTheStrategyForTheRiskLevelName(string riskLevel, Type expected)
        {
            Assert.IsType(expected, this.services.StrategyFactory.GetStrategy(riskLevel));
        }

        [Fact]
        public void GetStrategy_NullRiskLevel_ThrowsNullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => this.services.StrategyFactory.GetStrategy(null));
        }

        [Fact]
        public void GetStrategy_RepeatedCalls_ReturnTheSameInstance()
        {
            Assert.Same(this.services.StrategyFactory.GetStrategy("p1"), this.services.StrategyFactory.GetStrategy("p1"));
        }
    }
}
