using Defra.Imports.Model;
using FluentAssertions;
using MarkTek.Fluent.Testing.RecordGeneration;

namespace Defra.Imports.IntegrationTests.Dynamics.ConfigurationParameter.Assertions.Validators
{
    public class ConfigurationParameterHasKey : ISpecificationValidator<defraexp_configurationparameter>
    {
        private string key;

        public ConfigurationParameterHasKey(string key)
        {
            this.key = key;
        }

        public void Validate(defraexp_configurationparameter item)
        {
            item.defraexp_Key.Should().Be(this.key);
        }
    }
}
