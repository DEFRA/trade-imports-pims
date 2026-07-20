namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Assertions
{
    using Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Assertions.Validators;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PlaceOfOriginValidateUnlockedFromBronze : BaseValidator<Guid, defraimp_placeoforigin>
    {
        private readonly ImportsContext context;
        private readonly defraimp_placeoforigin placeOfOriginRecord;
        private readonly DateTime expectedLockedToBronzeDate;
        private readonly defraimp_trustlevel expectedTrustLevel;

        public PlaceOfOriginValidateUnlockedFromBronze(ImportsContext context, Guid placeOfOriginId, DateTime expectedLockedToBronzeDate, defraimp_trustlevel expectedTrustLevel)
        {
            this.context = context;
            this.placeOfOriginRecord = GetRecord(placeOfOriginId);
            this.expectedLockedToBronzeDate = expectedLockedToBronzeDate;
            this.expectedTrustLevel = expectedTrustLevel;
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
               new DateUnlockedFromBronzeIsValue(this.placeOfOriginRecord, this.expectedLockedToBronzeDate),
            };
        }
    }
}
