using Defra.Imports.Model;
using FluentAssertions;
using MarkTek.Fluent.Testing.RecordGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Tests.Integration.Dynamics.Autonumber.Assertions.Validators
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
