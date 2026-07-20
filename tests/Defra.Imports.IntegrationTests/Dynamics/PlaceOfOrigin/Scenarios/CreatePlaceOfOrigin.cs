namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfOrigin.Scenarios
{
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;

    public class CreatePlaceOfOrigin : IRecordCreator<defraimp_placeoforigin, Guid>
    {
        private readonly ImportsContext context;

        public CreatePlaceOfOrigin(ImportsContext context, Guid id)
        {
            this.context = context;
            this.RecordToCreate = new defraimp_placeoforigin
            {
                Id = Guid.NewGuid(),
            };
        }

        public CreatePlaceOfOrigin(ImportsContext context, defraimp_placeoforigin samplePlaceOfOrigin)
        {
            this.context = context;
            this.RecordToCreate = samplePlaceOfOrigin;
        }

        public Guid Id { get; }

        public defraimp_placeoforigin RecordToCreate { get; }

        public Record<defraimp_placeoforigin, Guid> CreateRecord()
        {
            if (this.RecordToCreate != null)
            {
                this.context.AddObject(this.RecordToCreate);
                this.context.SaveChanges();
                return new Record<defraimp_placeoforigin, Guid>(this.RecordToCreate, this.RecordToCreate.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
