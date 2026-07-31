namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class CreateImporterNotification : IRecordCreator<defraimp_ImporterNotification, Guid>
    {
        public CreateImporterNotification(Guid id)
        {
            this.RecordToCreate = new defraimp_ImporterNotification
            {
                Id = id,
            };
        }

        public CreateImporterNotification(ImportsContext context, defraimp_ImporterNotification samepleImporterNotification)
        {
            this.context = context;
            this.RecordToCreate = samepleImporterNotification;
        }

        private readonly ImportsContext context;

        public Guid Id { get; }

        public defraimp_ImporterNotification RecordToCreate { get; }

        /// <inheritdoc/>
        public Record<defraimp_ImporterNotification, Guid> CreateRecord()
        {
            if (this.RecordToCreate != null)
            {
                this.context.AddObject(this.RecordToCreate);
                this.context.SaveChanges();
                return new Record<defraimp_ImporterNotification, Guid>(this.RecordToCreate, this.RecordToCreate.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
