namespace Defra.Imports.IntegrationTests.Dynamics.Consignee.Scenarios
{
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;
    using System;

    public class CreateConsignee : IRecordCreator<defraimp_Consignee, Guid>
    {
        private ImportsContext context;
        private defraimp_Consignee recordToCreate;

        public CreateConsignee(ImportsContext context, defraimp_Consignee recordToCreate)
        {
            this.context = context;
            this.recordToCreate = recordToCreate;
        }

        public Record<defraimp_Consignee, Guid> CreateRecord()
        {
            if (this.recordToCreate != null)
            {
                this.context.AddObject(this.recordToCreate);
                this.context.SaveChanges();
                return new Record<defraimp_Consignee, Guid>(this.recordToCreate, this.recordToCreate.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
