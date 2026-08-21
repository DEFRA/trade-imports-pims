namespace Defra.Imports.IntegrationTests.Dataverse.PlaceOfOrigin.Scenarios
{
    using System;
    using System.Linq;
    using Defra.Imports.Model;
    using Marktek.Fluent.Testing.Engine.Interfaces;

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

        /// <inheritdoc/>
        public void Execute(Guid id)
        {
            if (this.RecordToDelete != null)
            {
                // Remove records still referencing this place of origin so the FK constraints on
                // defraimp_previousplaceoforiginid / defraimp_counterhistory don't block the delete.
                this.RemoveReferencingRecords();

                this.context.ClearChanges();
                this.context.Attach(this.RecordToDelete);
                this.context.DeleteObject(this.RecordToDelete);
                this.context.SaveChanges();
            }
        }

        private void RemoveReferencingRecords()
        {
            this.context.ClearChanges();

            // Entities returned by these queries are already tracked by the context, so don't Attach them again.
            var referencingImportApplications = this.context.defraimp_importapplicationSet
                .Where(a => a.defraimp_PlaceofOriginid.Id == this.RecordToDelete.Id)
                .ToList();
            foreach (var importApplication in referencingImportApplications)
            {
                importApplication.defraimp_PlaceofOriginid = null;
                this.context.UpdateObject(importApplication);
            }

            var referencingCounterHistories = this.context.defraimp_counterhistorySet
                .Where(c => c.defraimp_PlaceOfOriginId.Id == this.RecordToDelete.Id)
                .ToList();
            foreach (var counterHistory in referencingCounterHistories)
            {
                this.context.DeleteObject(counterHistory);
            }

            this.context.SaveChanges();
        }
    }
}
