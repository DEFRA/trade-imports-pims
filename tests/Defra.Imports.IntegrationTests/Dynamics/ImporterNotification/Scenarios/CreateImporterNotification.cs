using Defra.Imports.Model;
using MarkTek.Fluent.Testing.RecordGeneration;
using System;

namespace Defra.Imports.IntegrationTests.Dynamics.ImporterNotification.Scenarios
{
    public class CreateImporterNotification : IRecordCreator<defraimp_ImporterNotification, Guid>
    {
        public CreateImporterNotification(Guid id)
        {
            RecordToCreate = new defraimp_ImporterNotification
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

        public Record<defraimp_ImporterNotification, Guid> CreateRecord()
        {
            if (RecordToCreate != null)
            {
                context.AddObject(RecordToCreate);
                context.SaveChanges();
                return new Record<defraimp_ImporterNotification, Guid>(RecordToCreate, RecordToCreate.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
