namespace Defra.Imports.IntegrationTests.Dynamics.Autonumber.Assertions
{
    using Defra.Imports.IntegrationTests.Dynamics.Autonumber.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class AutonumberRecordValidateCurrentNumber : BaseValidator<Guid, defraimp_autonumber>
    {
        private readonly ImportsContext context;
        private readonly defraimp_autonumber autonumberRecord;
        private readonly int expectedValue;

        public AutonumberRecordValidateCurrentNumber(ImportsContext context, Guid autonumberId, int expectedValue)
        {
            this.context = context;
            this.autonumberRecord = GetRecord(autonumberId);
            this.expectedValue = expectedValue;
        }

        public override defraimp_autonumber GetRecord(Guid id)
        {
            return context.defraimp_autonumberSet.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        public override List<ISpecificationValidator<defraimp_autonumber>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_autonumber>>
            {
               new CurrentNumberIsValue(this.autonumberRecord, this.expectedValue),
            };
        }
    }
}
