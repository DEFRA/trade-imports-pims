namespace Defra.Imports.IntegrationTests.Dataverse.Autonumber.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.IntegrationTests.Dataverse.Autonumber.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class AutonumberRecordValidateCurrentNumber : BaseValidator<Guid, defraimp_autonumber>
    {
        private readonly ImportsContext context;
        private readonly defraimp_autonumber autonumberRecord;
        private readonly int expectedValue;

        public AutonumberRecordValidateCurrentNumber(ImportsContext context, Guid autonumberId, int expectedValue)
        {
            this.context = context;
            this.autonumberRecord = this.GetRecord(autonumberId);
            this.expectedValue = expectedValue;
        }

        /// <inheritdoc/>
        public override defraimp_autonumber GetRecord(Guid id)
        {
            return this.context.defraimp_autonumberSet.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        /// <inheritdoc/>
        public override List<ISpecificationValidator<defraimp_autonumber>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_autonumber>>
            {
               new CurrentNumberIsValue(this.autonumberRecord, this.expectedValue),
            };
        }
    }
}
