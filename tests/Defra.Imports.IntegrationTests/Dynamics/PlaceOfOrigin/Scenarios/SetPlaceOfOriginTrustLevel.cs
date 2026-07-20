namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Scenarios
{
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;
    using System;

    class SetPlaceOfOriginTrustLevel : IExecutableAction<defraimp_placeoforigin, Guid>
    {
        private readonly ImportsContext context;
        private readonly Guid placeOfOriginId;
        private readonly defraimp_trustlevel trustLevel;

        public SetPlaceOfOriginTrustLevel(ImportsContext context, Guid placeOfOriginId, defraimp_trustlevel trustLevel)
        {
            this.context = context;
            this.placeOfOriginId = placeOfOriginId;
            this.trustLevel = trustLevel;
        }

        public void Execute(Guid id)
        {
            defraimp_placeoforigin placeOfOriginToUpdate = new defraimp_placeoforigin
            {
                Id = this.placeOfOriginId,
                defraimp_TrustLevel = this.trustLevel,
            };

            this.context.ClearChanges();
            this.context.Attach(placeOfOriginToUpdate);
            this.context.UpdateObject(placeOfOriginToUpdate);
            this.context.SaveChanges();
            this.context.Detach(placeOfOriginToUpdate);
        }
    }
}
