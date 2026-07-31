namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class ApplicationCounterIsValue : ISpecificationValidator<defraimp_placeoforigin>
    {

        private defraimp_placeoforigin placeOfOriginRecord;
        private int expectedValue;

        public ApplicationCounterIsValue(defraimp_placeoforigin placeOfOriginRecord, int expectedValue)
        {
            this.placeOfOriginRecord = placeOfOriginRecord;
            this.expectedValue = expectedValue;
        }

        /// <inheritdoc/>
        public void Validate(defraimp_placeoforigin item)
        {
            this.placeOfOriginRecord.defraimp_ApplicationCounter.Should().Be(this.expectedValue);
        }
    }
}
