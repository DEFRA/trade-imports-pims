namespace Defra.Imports.Tests.Integration.Dynamics.Transporter.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class CreateTransporter : IRecordCreator<defraimp_Transporter, Guid>
    {
        private ImportsContext context;
        private defraimp_Transporter recordToCreate;

        public CreateTransporter(ImportsContext context, defraimp_Transporter recordToCreate)
        {
            this.context = context;
            this.recordToCreate = recordToCreate;
        }

        public Record<defraimp_Transporter, Guid> CreateRecord()
        {
            if (this.recordToCreate != null)
            {
                this.context.AddObject(this.recordToCreate);
                this.context.SaveChanges();
                return new Record<defraimp_Transporter, Guid>(this.recordToCreate, this.recordToCreate.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
