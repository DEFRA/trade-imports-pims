namespace Defra.Imports.IntegrationTests.Dataverse.WatchList.Scenarios
{
    using System;
    using Defra.Imports.Model;
    using MarkTek.Fluent.Testing.RecordGeneration;

    public class CreateWatchList : IRecordCreator<defraimp_WatchList, Guid>
    {
        public readonly ImportsContext context;
        private readonly defraimp_WatchList recordToCreate;

        public CreateWatchList(ImportsContext context, defraimp_WatchList recordToCreate)
        {
            this.context = context;
            this.recordToCreate = recordToCreate;
        }

        /// <inheritdoc/>
        public Record<defraimp_WatchList, Guid> CreateRecord()
        {
            if (this.recordToCreate != null)
            {
                this.context.AddObject(this.recordToCreate);
                this.context.SaveChanges();
                return new Record<defraimp_WatchList, Guid>(this.recordToCreate, this.recordToCreate.Id);
            }
            else
            {
                return null;
            }
        }
    }
}
