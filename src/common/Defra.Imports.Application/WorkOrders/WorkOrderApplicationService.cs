namespace Defra.Imports.Application.WorkOrders
{
    using System;
    using Defra.Imports.Dataverse.Queries;

    /// <summary>Read-only composition example; no existing work order business rules are migrated.</summary>
    public sealed class WorkOrderApplicationService : IWorkOrderApplicationService
    {
        private readonly WorkOrderQueries queries;

        /// <summary>Initializes a new instance of the <see cref="WorkOrderApplicationService"/> class.</summary>
        /// <param name="queries">The business-oriented Dataverse query service.</param>
        public WorkOrderApplicationService(WorkOrderQueries queries)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
        }

        /// <inheritdoc/>
        public WorkOrderInitializationContext GetInitializationContext(Guid workOrderId)
        {
            return this.queries.GetInitializationContext(workOrderId);
        }
    }
}
