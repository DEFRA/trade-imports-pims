namespace Defra.Imports.Dataverse.Queries
{
    using System;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>Read-only scaffold assuming the Field Service work order table is installed.</summary>
    public class WorkOrderQueries
    {
        private readonly IOrganizationService service;

        /// <summary>Initializes a new instance of the <see cref="WorkOrderQueries"/> class.</summary>
        /// <param name="service">The current invocation's organisation service.</param>
        public WorkOrderQueries(IOrganizationService service)
        {
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        /// <summary>Retrieves the minimal work order projection without changing the record.</summary>
        /// <param name="workOrderId">The non-empty work order identifier.</param>
        /// <returns>The projection, or an exception if the work order does not exist.</returns>
        public WorkOrderInitializationContext GetInitializationContext(Guid workOrderId)
        {
            if (workOrderId == Guid.Empty)
            {
                throw new ArgumentException("A work order ID is required.", nameof(workOrderId));
            }

            var query = new QueryExpression("msdyn_workorder")
            {
                ColumnSet = new ColumnSet("msdyn_workorderid"),
                TopCount = 1,
            };
            query.Criteria.AddCondition("msdyn_workorderid", ConditionOperator.Equal, workOrderId);
            var records = this.service.RetrieveMultiple(query);
            if (records.Entities.Count == 0)
            {
                throw new InvalidOperationException("The requested work order was not found.");
            }

            return new WorkOrderInitializationContext(records.Entities[0].Id);
        }
    }
}
