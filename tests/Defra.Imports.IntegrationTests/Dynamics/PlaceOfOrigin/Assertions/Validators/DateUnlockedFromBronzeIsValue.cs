namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Assertions.Validators
{
    using System;
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class DateUnlockedFromBronzeIsValue : ISpecificationValidator<defraimp_placeoforigin>
    {

        private defraimp_placeoforigin placeOfOriginRecord;
        private DateTime expectedValue;

        public DateUnlockedFromBronzeIsValue(defraimp_placeoforigin placeOfOriginRecord, DateTime expectedValue)
        {
            this.placeOfOriginRecord = placeOfOriginRecord;
            this.expectedValue = expectedValue;
        }

        /// <inheritdoc/>
        public void Validate(defraimp_placeoforigin item)
        {
            this.placeOfOriginRecord.defraimp_DateUnlockedFromBronze.Should().BeSameDateAs(this.expectedValue);
        }
    }
}
