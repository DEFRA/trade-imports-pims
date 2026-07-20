namespace Defra.Imports.IntegrationTests.Dynamics.PlaceOfDestination.Scenarios
{
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;

    public class CreatePlaceOfDestination : IRecordCreator<defraimp_PlaceOfDestination, Guid>
    {
        private readonly ImportsContext context;
        private readonly defraimp_PlaceOfDestination recordToCreate;

        public CreatePlaceOfDestination(ImportsContext context, defraimp_PlaceOfDestination recordToCreate)
        {
            this.context = context;
            this.recordToCreate = recordToCreate;
        }

        public Record<defraimp_PlaceOfDestination, Guid> CreateRecord()
        {
            if (this.recordToCreate != null)
            {
                this.context.AddObject(this.recordToCreate);
                this.context.SaveChanges();
                return new Record<defraimp_PlaceOfDestination, Guid>(this.recordToCreate, this.recordToCreate.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
