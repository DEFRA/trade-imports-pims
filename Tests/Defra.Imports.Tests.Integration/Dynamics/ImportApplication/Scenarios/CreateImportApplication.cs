namespace Defra.Imports.Tests.Integration.Dynamics.ImportApplication.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class CreateImportApplication : IRecordCreator<defraimp_importapplication, Guid>
    {
        public CreateImportApplication(Guid id)
        {
            RecordToCreate = new defraimp_importapplication
            {
                Id = id,
            };
        }

        public CreateImportApplication(ImportsContext context, defraimp_importapplication sampleImportApplication)
        {
            this.context = context;
            this.RecordToCreate = sampleImportApplication;
        }

        private readonly ImportsContext context;

        public Guid Id { get; }

        public defraimp_importapplication RecordToCreate { get; }

        public Record<defraimp_importapplication, Guid> CreateRecord()
        {
            if (RecordToCreate != null)
            {
                context.AddObject(RecordToCreate);
                context.SaveChanges();
                return new Record<defraimp_importapplication, Guid>(RecordToCreate, RecordToCreate.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
