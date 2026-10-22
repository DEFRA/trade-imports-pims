namespace Defra.Imports.Dataverse.Queries
{
    using System;

    /// <summary>A read-only example projection, not an SDK entity or execution context.</summary>
    public sealed class WorkOrderInitializationContext
    {
        /// <summary>Initializes a new instance of the <see cref="WorkOrderInitializationContext"/> class.</summary>
        /// <param name="workOrderId">The retrieved work order identifier.</param>
        public WorkOrderInitializationContext(Guid workOrderId)
        {
            this.WorkOrderId = workOrderId;
        }

        /// <summary>Gets the retrieved work order identifier.</summary>
        public Guid WorkOrderId { get; }
    }
}
