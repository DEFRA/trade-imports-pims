namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class TrustLevelIsValue : ISpecificationValidator<defraimp_placeoforigin>
    {
        private defraimp_placeoforigin placeOfOriginRecord;
        private defraimp_trustlevel expectedValue;

        public TrustLevelIsValue(defraimp_placeoforigin placeOfOriginRecord, defraimp_trustlevel expectedValue)
        {
            this.placeOfOriginRecord = placeOfOriginRecord;
            this.expectedValue = expectedValue;
        }

        public void Validate(defraimp_placeoforigin item)
        {
            this.placeOfOriginRecord.defraimp_TrustLevel.Should().Be(this.expectedValue);
        }
    }
}
