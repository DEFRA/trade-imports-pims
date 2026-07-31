namespace Defra.Imports.IntegrationTests.Dynamics.ImportApplication.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class CreateImportApplication : IRecordCreator<defraimp_importapplication, Guid>
    {
        private readonly ImportsContext context;

        public CreateImportApplication(ImportsContext context, Guid id)
        {
            this.context = context;
            this.RecordToCreate = new defraimp_importapplication
            {
                Id = id,
            };
        }

        public CreateImportApplication(ImportsContext context, defraimp_importapplication sampleImportApplication)
        {
            this.context = context;
            this.RecordToCreate = sampleImportApplication;
        }

        public Guid Id { get; }

        public defraimp_importapplication RecordToCreate { get; }

        /// <inheritdoc/>
        public Record<defraimp_importapplication, Guid> CreateRecord()
        {
            if (this.RecordToCreate != null)
            {
                this.context.AddObject(this.RecordToCreate);
                this.context.SaveChanges();
                return new Record<defraimp_importapplication, Guid>(this.RecordToCreate, this.RecordToCreate.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
