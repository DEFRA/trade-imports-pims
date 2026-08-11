namespace Defra.Imports.IntegrationTests.Dynamics.Autonumber.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.IntegrationTests.Dynamics.Autonumber.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class AutonumberRecordValidateValues : BaseValidator<Guid, defraimp_autonumber>
    {
        private ImportsContext context;
        private string key;

        public AutonumberRecordValidateValues(ImportsContext context, string key)
        {
            this.context = context;
            this.key = key;
        }

        /// <inheritdoc/>
        public override defraimp_autonumber GetRecord(Guid id)
        {
            return this.context.defraimp_autonumberSet.FirstOrDefault(x => x.Id == id);
        }

        /// <inheritdoc/>
        public override List<ISpecificationValidator<defraimp_autonumber>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_autonumber>>
            {
                new AutoNumberHasKey(this.key),
            };
        }
    }
}
