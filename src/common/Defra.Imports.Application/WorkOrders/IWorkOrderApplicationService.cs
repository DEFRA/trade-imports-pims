namespace Defra.Imports.Application.WorkOrders
{
    using System;
    using Defra.Imports.Dataverse.Queries;

    /// <summary>Defines the read-only example application use case.</summary>
    public interface IWorkOrderApplicationService
    {
        /// <summary>Loads context for a work order without applying business changes.</summary>
        /// <param name="workOrderId">The requested work order identifier.</param>
        /// <returns>The retrieved context.</returns>
        WorkOrderInitializationContext GetInitializationContext(Guid workOrderId);
    }
}
