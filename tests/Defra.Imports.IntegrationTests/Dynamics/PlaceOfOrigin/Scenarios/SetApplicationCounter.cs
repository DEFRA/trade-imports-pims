namespace Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;

    class SetApplicationCounter : IWaitableAction
    {
        private readonly ImportsContext context;
        private readonly Guid placeOfOriginId;
        private readonly int value;

        public SetApplicationCounter(ImportsContext context, Guid placeOfOriginId, int value)
        {
            this.context = context;
            this.placeOfOriginId = placeOfOriginId;
            this.value = value;
        }

        /// <inheritdoc/>
        public void Execute()
        {
            defraimp_placeoforigin placeOfOriginToUpdate = new defraimp_placeoforigin
            {
                Id = this.placeOfOriginId,
                defraimp_ApplicationCounter = this.value,
            };

            this.context.ClearChanges();
            this.context.Attach(placeOfOriginToUpdate);
            this.context.UpdateObject(placeOfOriginToUpdate);
            this.context.SaveChanges();
            this.context.Detach(placeOfOriginToUpdate);
        }
    }
}
