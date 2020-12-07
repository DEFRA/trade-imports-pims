namespace Defra.Imports.Tests.Integration.Dynamics.PlaceOfDestination.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;

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
