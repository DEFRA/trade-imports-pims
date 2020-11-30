using Defra.Imports.Model;
using FluentAssertions;
using MarkTek.Fluent.Testing.RecordGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Tests.Integration.Dynamics.ConfigurationParameter.Assertions.Validators
{
    public class ConfigurationParameterHasValue : ISpecificationValidator<defraexp_configurationparameter>
    {
        private string value;

        public ConfigurationParameterHasValue(string value)
        {
            this.value = value;
        }

        public void Validate(defraexp_configurationparameter item)
        {
            item.defraexp_Value.Should().Be(this.value);
        }
    }
}
