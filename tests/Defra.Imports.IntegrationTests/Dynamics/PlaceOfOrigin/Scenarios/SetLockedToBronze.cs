namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;

    class SetLockedToBronze : IExecutableAction<defraimp_placeoforigin, Guid>
    {
        private readonly ImportsContext context;
        private readonly Guid placeOfOriginId;
        private readonly bool lockedToBronze;

        public SetLockedToBronze(ImportsContext context, Guid placeOfOriginId, bool lockedToBronze)
        {
            this.context = context;
            this.placeOfOriginId = placeOfOriginId;
            this.lockedToBronze = lockedToBronze;
        }

        /// <inheritdoc/>
        public void Execute(Guid id)
        {
            defraimp_placeoforigin placeOfOriginToUpdate = new defraimp_placeoforigin
            {
                Id = this.placeOfOriginId,
                defraimp_LocktoBronze = this.lockedToBronze,
            };

            this.context.ClearChanges();
            this.context.Attach(placeOfOriginToUpdate);
            this.context.UpdateObject(placeOfOriginToUpdate);
            this.context.SaveChanges();
            this.context.Detach(placeOfOriginToUpdate);
        }
    }
}
