using Defra.Imports.Model;
using Defra.Imports.Tests.Integration.Dynamics.Autonumber.Assertions.Validators;
using Marktek.Fluent.Testing.Engine;
using MarkTek.Fluent.Testing.RecordGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.Imports.Tests.Integration.Dynamics.Autonumber.Assertions
{
    public class AutonumberRecordValidateValues : BaseValidator<Guid, defraimp_autonumber>
    {
        private ImportsContext context;
        private string key;

        public AutonumberRecordValidateValues(ImportsContext context, string key)
        {
            this.context = context;
            this.key = key;
        }

        public override defraimp_autonumber GetRecord(Guid id)
        {
            return this.context.defraimp_autonumberSet.FirstOrDefault(x => x.Id == id);
        }

        public override List<ISpecificationValidator<defraimp_autonumber>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_autonumber>>
            {
                new AutoNumberHasKey(this.key),
            };
        }
    }
}
