namespace Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.Assertions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.Model;
    using Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.Assertions.Validators;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class PlaceOfOriginValidateLockedToBronze : BaseValidator<Guid, defraimp_placeoforigin>
    {
        private readonly ImportsContext context;
        private readonly defraimp_placeoforigin placeOfOriginRecord;
        private readonly DateTime expectedLockedToBronzeDate;
        private readonly defraimp_trustlevel expectedTrustLevel = defraimp_trustlevel.Bronze;

        public PlaceOfOriginValidateLockedToBronze(ImportsContext context, Guid placeOfOriginId, DateTime expectedLockedToBronzeDate)
        {
            this.context = context;
            this.placeOfOriginRecord = GetRecord(placeOfOriginId);
            this.expectedLockedToBronzeDate = expectedLockedToBronzeDate;
        }

        public override defraimp_placeoforigin GetRecord(Guid id)
        {
            return this.context.defraimp_placeoforiginSet.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
        }

        public override List<ISpecificationValidator<defraimp_placeoforigin>> GetValidators()
        {
            return new List<ISpecificationValidator<defraimp_placeoforigin>>
            {
               new TrustLevelIsValue(this.placeOfOriginRecord, this.expectedTrustLevel),
               new DateLockedToBronzeIsValue(this.placeOfOriginRecord, this.expectedLockedToBronzeDate),
            };
        }
    }
}
