namespace Defra.Imports.Tests.Integration.Dynamics.PlaceOfOrigin.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;

    class SetQuotaCounter : IWaitableAction
    {
        private readonly ImportsContext context;
        private readonly Guid placeOfOriginId;
        private readonly int value;

        public SetQuotaCounter(ImportsContext context, Guid placeOfOriginId, int value)
        {
            this.context = context;
            this.placeOfOriginId = placeOfOriginId;
            this.value = value;
        }

        public void Execute()
        {
            defraimp_placeoforigin placeOfOriginToUpdate = new defraimp_placeoforigin
            {
                Id = this.placeOfOriginId,
                defraimp_InspectionQuotaCounter = this.value,
            };

            this.context.ClearChanges();
            this.context.Attach(placeOfOriginToUpdate);
            this.context.UpdateObject(placeOfOriginToUpdate);
            this.context.SaveChanges();
            this.context.Detach(placeOfOriginToUpdate);
        }
    }
}
