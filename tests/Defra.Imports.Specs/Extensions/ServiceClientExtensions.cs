namespace Defra.Imports.Specs.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.Threading.Tasks;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Messages;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>
    /// Extensions for the <see cref="ServiceClient"/> class.
    /// </summary>
    public static class ServiceClientExtensions
    {
        /// <summary>
        /// Gets the ID of any record for a given table.
        /// </summary>
        /// <param name="client">The service client.</param>
        /// <param name="logicalName">The logical name.</param>
        /// <returns>The record ID.</returns>
        /// <exception cref="Exception">Thrown if no records have been created.</exception>
        public static async Task<Guid> GetAnyRecordIdAsync(this ServiceClient client, string logicalName)
        {
            var result = await client.RetrieveMultipleAsync(new QueryExpression(logicalName) { TopCount = 1 });
            if (!result.Entities.Any())
            {
                throw new Exception($"No records have been created for the {logicalName} table.");
            }

            return result.Entities.First().Id;
        }

        /// <summary>
        /// Performs an <see cref="ExecuteMultipleRequest"/>.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <param name="requests">The requests.</param>
        /// <param name="settings">The settings. Passing null will default to fail-fast settings (return responses, no continue on error).</param>
        /// <param name="throwOnError">Whether to throw an exception if any of the responses fail.</param>
        /// <returns>The execute multiple response.</returns>
        public static ExecuteMultipleResponse ExecuteMultiple(this ServiceClient serviceClient, IEnumerable<OrganizationRequest> requests, ExecuteMultipleSettings settings = null, bool throwOnError = true)
        {
            if (settings == null)
            {
                settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = false,
                    ReturnResponses = true,
                };
            }

            var requestCollection = new OrganizationRequestCollection();
            requestCollection.AddRange(requests);

            var executeMultipleResponse = (ExecuteMultipleResponse)serviceClient.Execute(
                new ExecuteMultipleRequest
                {
                    Requests = requestCollection,
                    Settings = settings,
                });

            if (throwOnError && executeMultipleResponse.IsFaulted)
            {
                if (!settings.ContinueOnError)
                {
                    throw new FaultException<OrganizationServiceFault>(executeMultipleResponse.Responses.First(r => r.Fault != null).Fault);
                }
                else
                {
                    throw new AggregateException(executeMultipleResponse.Responses.Where(r => r.Fault != null).Select(r => new FaultException<OrganizationServiceFault>(r.Fault)));
                }
            }

            return executeMultipleResponse;
        }

        /// <summary>
        /// Attempts to create the specified <see cref="Entity"/> record using the provided <see cref="ServiceClient"/>.
        /// </summary>
        /// <param name="serviceClient">The service client.</param>
        /// <param name="entity">The entity instance.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <remarks>
        /// This method suppresses any exceptions thrown by the <see cref="ServiceClient.CreateAsync(Entity)"/> call. Use with caution if you require visibility into specific failure causes.
        /// </remarks>
        public static async Task<Guid> TryCreateAsync(this ServiceClient serviceClient, Entity entity)
        {
            var success = false;

            try
            {
                await serviceClient.CreateAsync(entity);
                success = true;
            }
            catch
            {
            }

            return success ? entity.Id : Guid.Empty;
        }
    }
}