namespace Defra.Imports.IntegrationTests.Dynamics.ConfigurationParameter.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.IntegrationTests.Dynamics.ConfigurationParameter.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class ConfigurationParameterValidateValues : BaseValidator<Guid, defraexp_configurationparameter>
    {
        private readonly ImportsContext context;
        private readonly string key;
        private readonly string value;

        public ConfigurationParameterValidateValues(ImportsContext context, string key, string value = null)
        {
            this.context = context;
            this.key = key;
            this.value = value;
        }

        /// <inheritdoc/>
        public override defraexp_configurationparameter GetRecord(Guid id)
        {
            return this.context.defraexp_configurationparameterSet.FirstOrDefault(x => x.Id == id);
        }

        /// <inheritdoc/>
        public override List<ISpecificationValidator<defraexp_configurationparameter>> GetValidators()
        {
            var validators = new List<ISpecificationValidator<defraexp_configurationparameter>>
            {
                new ConfigurationParameterHasKey(this.key),
            };

            if (this.value != null)
            {
                validators.Add(new ConfigurationParameterHasValue(this.value));
            }

            return validators;
        }
    }
}
