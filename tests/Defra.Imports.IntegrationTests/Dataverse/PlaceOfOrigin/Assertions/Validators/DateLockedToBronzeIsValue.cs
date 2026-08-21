namespace Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.Assertions.Validators
{
    using System;
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class DateLockedToBronzeIsValue : ISpecificationValidator<defraimp_placeoforigin>
    {

        private defraimp_placeoforigin placeOfOriginRecord;
        private DateTime expectedValue;

        public DateLockedToBronzeIsValue(defraimp_placeoforigin placeOfOriginRecord, DateTime expectedValue)
        {
            this.placeOfOriginRecord = placeOfOriginRecord;
            this.expectedValue = expectedValue;
        }

        /// <inheritdoc/>
        public void Validate(defraimp_placeoforigin item)
        {
            this.placeOfOriginRecord.defraimp_DateLockedtoBronze.Should().BeSameDateAs(this.expectedValue);
        }
    }
}
