namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;

    class PlaceOfOriginValidateApplicationCounter : BaseValidator<Guid, defraimp_placeoforigin>
    {
        private readonly ImportsContext context;
        private readonly defraimp_placeoforigin placeOfOriginRecord;
        private readonly int applicationCounterValue;
        private readonly int quotaCounterValue;

        public PlaceOfOriginValidateApplicationCounter(ImportsContext context, Guid placeOfOriginId, int applicationCounterValue, int quotaCounterValue)
        {
            this.context = context;
            this.placeOfOriginRecord = this.GetRecord(placeOfOriginId);
            this.applicationCounterValue = applicationCounterValue;
            this.quotaCounterValue = quotaCounterValue;
        }

        /// <inheritdoc/>
        public override defraimp_placeoforigin GetRecord(Guid id)
        {
            return this.context.defraimp_placeoforiginSet.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        /// <inheritdoc/>
        public override List<ISpecificationValidator<defraimp_placeoforigin>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_placeoforigin>>
            {
               new ApplicationCounterIsValue(this.placeOfOriginRecord, this.applicationCounterValue),
               new InspectionQuotaCounterIsValue(this.placeOfOriginRecord, this.quotaCounterValue),
            };
        }
    }
}
