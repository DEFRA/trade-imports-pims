namespace Defra.Imports.Tests.Integration.Dynamics.Autonumber.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class CurrentNumberIsValue : ISpecificationValidator<defraimp_autonumber>
    {
        private defraimp_autonumber autonumberRecord;
        private int expectedValue;

        public CurrentNumberIsValue(defraimp_autonumber autonumberRecord, int expectedValue)
        {
            this.autonumberRecord = autonumberRecord;
            this.expectedValue = expectedValue;
        }

        public void Validate(defraimp_autonumber item)
        {
            autonumberRecord.defraimp_CurrentNumber.Should().Be(this.expectedValue);
        }
    }
}
