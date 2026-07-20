namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Scenarios
{
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;
    using System;

    public class DeletePlaceOfOrigin : IExecutableAction<defraimp_placeoforigin, Guid>
    {
        private readonly ImportsContext context;

        public DeletePlaceOfOrigin(ImportsContext context, Guid id)
        {
            this.context = context;
            this.RecordToDelete = new defraimp_placeoforigin
            {
                Id = Guid.NewGuid(),
            };
        }

        public DeletePlaceOfOrigin(ImportsContext context, defraimp_placeoforigin samplePlaceOfOrigin)
        {
            this.context = context;
            this.RecordToDelete = samplePlaceOfOrigin;
        }

        public Guid Id { get; }

        public defraimp_placeoforigin RecordToDelete { get; }

        public void Execute(Guid id)
        {
            if (this.RecordToDelete != null)
            {
                this.context.ClearChanges();
                this.context.Attach(this.RecordToDelete);
                this.context.DeleteObject(this.RecordToDelete);
            }
        }
    }
}
