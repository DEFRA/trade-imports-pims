namespace Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class InspectionQuotaCounterIsValue : ISpecificationValidator<defraimp_placeoforigin>
    {

        private defraimp_placeoforigin placeOfOriginRecord;
        private int expectedValue;

        public InspectionQuotaCounterIsValue(defraimp_placeoforigin placeOfOriginRecord, int expectedValue)
        {
            this.placeOfOriginRecord = placeOfOriginRecord;
            this.expectedValue = expectedValue;
        }

        public void Validate(defraimp_placeoforigin item)
        {
            this.placeOfOriginRecord.defraimp_InspectionQuotaCounter.Should().Be(this.expectedValue);
        }
    }
}
