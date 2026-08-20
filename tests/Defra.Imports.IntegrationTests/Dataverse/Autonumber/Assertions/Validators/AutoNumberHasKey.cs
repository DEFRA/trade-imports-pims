namespace Defra.Imports.IntegrationTests.Dataverse.Autonumber.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class AutoNumberHasKey : ISpecificationValidator<defraimp_autonumber>
    {
        private string key;

        public AutoNumberHasKey(string key)
        {
            this.key = key;
        }

        /// <inheritdoc/>
        public void Validate(defraimp_autonumber item)
        {
            item.defraimp_Key.Should().Be(this.key);
        }
    }
}
