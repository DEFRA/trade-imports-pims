namespace Defra.Imports.Scenarios.Extensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Model;
    using Microsoft.PowerPlatform.Dataverse.Client;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>
    /// Extensions for the <see cref="ServiceClient"/> class.
    /// </summary>
    public static class ServiceClientExtensions
    {
        // Keyed by username/application ID rather than by client instance, since every ServiceClient
        // used across a test run is cloned from the same environment and a given identifier always
        // resolves to the same system user ID.
        private static readonly ConcurrentDictionary<string, Task<Guid>> UserIdCache = new ConcurrentDictionary<string, Task<Guid>>();

        /// <summary>
        /// Retrieves the Dataverse system user ID for the given identifier, caching the result so repeated lookups for the same identifier do not requery Dataverse. The identifier is treated as an application ID if it parses as a <see cref="Guid"/> (the identifier used for application personas configured via <see cref="PersonaConfiguration.AppId"/>), and as a domain name otherwise.
        /// </summary>
        /// <param name="serviceClient">The service client used to query Dataverse.</param>
        /// <param name="username">The domain name of the user, or the application ID of an application user.</param>
        /// <returns>The system user ID.</returns>
        public static Task<Guid> RetrieveUserIdAsync(this ServiceClient serviceClient, string username)
        {
            if (serviceClient is null)
            {
                throw new ArgumentNullException(nameof(serviceClient));
            }

            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException($"'{nameof(username)}' cannot be null or empty.", nameof(username));
            }

            return UserIdCache.GetOrAdd(username, key => RetrieveUserIdCoreAsync(serviceClient, key));
        }

        private static async Task<Guid> RetrieveUserIdCoreAsync(ServiceClient serviceClient, string username)
        {
            var isApplicationId = Guid.TryParse(username, out var applicationId);

            var query = new QueryExpression(SystemUser.EntityLogicalName)
            {
                ColumnSet = new ColumnSet(false),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        isApplicationId
                            ? new ConditionExpression(SystemUser.Fields.ApplicationId, ConditionOperator.Equal, applicationId)
                            : new ConditionExpression(SystemUser.Fields.DomainName, ConditionOperator.Equal, username),
                    },
                },
            };

            var result = await serviceClient.RetrieveMultipleAsync(query).ConfigureAwait(false);
            var user = result.Entities.FirstOrDefault()
                ?? throw new InvalidOperationException($"No user exists in Dataverse with username '{username}'.");

            return user.Id;
        }
    }
}
