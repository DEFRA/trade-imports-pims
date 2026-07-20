namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;

    class DateLockedToBronzeIsValue : ISpecificationValidator<defraimp_placeoforigin>
    {

        private defraimp_placeoforigin placeOfOriginRecord;
        private DateTime expectedValue;

        public DateLockedToBronzeIsValue(defraimp_placeoforigin placeOfOriginRecord, DateTime expectedValue)
        {
            this.placeOfOriginRecord = placeOfOriginRecord;
            this.expectedValue = expectedValue;
        }

        public void Validate(defraimp_placeoforigin item)
        {
            this.placeOfOriginRecord.defraimp_DateLockedtoBronze.Should().BeSameDateAs(this.expectedValue);
        }
    }
}
