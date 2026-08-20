namespace Defra.Imports.IntegrationTests.Dataverse.ConfigurationParameter.Assertions.Validators
{
    using Defra.Imports.Model;
    using FluentAssertions;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class ConfigurationParameterHasValue : ISpecificationValidator<defraexp_configurationparameter>
    {
        private string value;

        public ConfigurationParameterHasValue(string value)
        {
            this.value = value;
        }

        /// <inheritdoc/>
        public void Validate(defraexp_configurationparameter item)
        {
            item.defraexp_Value.Should().Be(this.value);
        }
    }
}
