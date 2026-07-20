using Defra.Imports.Model;
using FluentAssertions;
using MarkTek.Fluent.Testing.RecordGeneration;

namespace Defra.Imports.IntegrationTests.Dynamics.Autonumber.Assertions.Validators
{
    public class AutoNumberHasKey : ISpecificationValidator<defraimp_autonumber>
    {
        private string key;

        public AutoNumberHasKey(string key)
        {
            this.key = key;
        }

        public void Validate(defraimp_autonumber item)
        {
            item.defraimp_Key.Should().Be(this.key);
        }
    }
}
