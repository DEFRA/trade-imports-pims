namespace Defra.Imports.IntegrationTests.Dynamics.PostImportCheck.Scenarios
{
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;

    class CreatePostImportCheck : IRecordCreator<defraimp_importinspection, Guid>
    {
        private readonly ImportsContext context;

        public CreatePostImportCheck(ImportsContext context, Guid id)
        {
            this.context = context;
            this.RecordToCreate = new defraimp_importinspection
            {
                Id = id,
            };
        }

        public CreatePostImportCheck(ImportsContext context, defraimp_importinspection samplePostImportCheck)
        {
            this.context = context;
            this.RecordToCreate = samplePostImportCheck;
        }

        public Guid Id { get; }

        public defraimp_importinspection RecordToCreate { get; }

        public Record<defraimp_importinspection, Guid> CreateRecord()
        {
            if (RecordToCreate != null)
            {
                this.context.AddObject(RecordToCreate);
                this.context.SaveChanges();
                return new Record<defraimp_importinspection, Guid>(this.RecordToCreate, this.RecordToCreate.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
