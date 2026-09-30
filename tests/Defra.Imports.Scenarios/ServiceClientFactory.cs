namespace Defra.Imports.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.PowerPlatform.Dataverse.Client;

    /// <summary>
    /// Base integration test class.
    /// </summary>
    public class ServiceClientFactory : IDisposable
    {
        private readonly ServiceClient baseClient;
        private readonly ILogger logger;
        private readonly UserPoolService userPoolService;

        private readonly Dictionary<string, ServiceClient> personaClients = new Dictionary<string, ServiceClient>();
        private readonly Dictionary<string, UserLease> personaLeases = new Dictionary<string, UserLease>();

        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceClientFactory"/> class.
        /// </summary>
        /// <param name="url">The environment URL.</param>
        /// <param name="clientId">The client ID of the application user.</param>
        /// <param name="clientSecret">The client secret of the application user.</param>
        /// <param name="userPoolService">The user pool service used to resolve persona-based clients via <see cref="GetClientAsync(Persona[])"/>. May be <c>null</c> if persona-based clients are not required. Ownership is transferred to the factory, which disposes it when the factory itself is disposed. The factory maintains its own cache of leased clients independently of any other consumer of the pool.</param>
        /// <param name="logger">The logger.</param>
        public ServiceClientFactory(Uri url, Guid clientId, string clientSecret, UserPoolService userPoolService = null, ILogger logger = null)
        {
            if (url is null)
            {
                throw new ArgumentNullException(nameof(url));
            }

            if (string.IsNullOrEmpty(clientSecret))
            {
                throw new ArgumentException($"'{nameof(clientSecret)}' cannot be null or empty.", nameof(clientSecret));
            }

            // Taken from https://github.com/microsoft/PowerApps-Samples/tree/master/dataverse/Xrm%20Tooling/TPLCrmServiceClient.
            OptimiseThreads();
            OptimiseConnections();

            this.logger = logger;
            this.baseClient = new ServiceClient(url, clientId.ToString(), clientSecret, true, logger);
            this.userPoolService = userPoolService;
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated with the configured application user.
        /// </summary>
        /// <returns>A <see cref="ServiceClient"/> instance.</returns>
        public ServiceClient GetAppUserClient()
        {
            return this.baseClient.Clone();
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance impersonating the given system user.
        /// </summary>
        /// <param name="systemUserId">The ID of the user to impersonate.</param>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as the given persona.</returns>
        public ServiceClient GetClient(Guid systemUserId)
        {
            this.logger?.LogInformation($"Getting client for system user ID: {systemUserId}.");

            var impersonatedClient = this.baseClient.Clone();
            impersonatedClient.CallerId = systemUserId;

            this.logger?.LogInformation($"Authenticated as caller ID: {systemUserId}.");

            return impersonatedClient;
        }

        /// <summary>
        /// Gets a <see cref="ServiceClient"/> instance authenticated as a user with exactly the given personas, leasing a user from the configured <see cref="UserPoolService"/>. Distinct persona combinations are leased and cached independently, so this method may be called for several different combinations without releasing in between - the caller is responsible for retaining each client for the duration it is needed and eventually calling <see cref="ReleaseClientAsync"/>, rather than the factory disposing them early.
        /// </summary>
        /// <param name="personas">The personas the leased user must have.</param>
        /// <returns>A <see cref="ServiceClient"/> instance authenticated as a user with the given personas.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no user pool service has been configured.</exception>
        public async Task<ServiceClient> GetClientAsync(params Persona[] personas)
        {
            if (this.userPoolService is null)
            {
                throw new InvalidOperationException("No user pool service has been configured for this factory.");
            }

            if (personas is null)
            {
                throw new ArgumentNullException(nameof(personas));
            }

            var key = PersonaSetKey.Create(personas);

            if (this.personaClients.TryGetValue(key, out var existingClient))
            {
                return existingClient;
            }

            this.logger?.LogInformation($"Getting client for personas: {string.Join(", ", personas)}.");

            var lease = await this.userPoolService.GetAsync(personas).ConfigureAwait(false);
            var systemUserId = await PersonaConfigurationApplier.RetrieveUserIdAsync(this.baseClient, lease.Username).ConfigureAwait(false);

            var impersonatedClient = this.baseClient.Clone();
            impersonatedClient.CallerId = systemUserId;

            this.personaClients[key] = impersonatedClient;
            this.personaLeases[key] = lease;

            this.logger?.LogInformation($"Authenticated as caller ID: {systemUserId}.");

            return impersonatedClient;
        }

        /// <summary>
        /// Releases every persona client currently held by this factory (via <see cref="GetClientAsync(Persona[])"/>), returning their leased users to the pool and disposing the clients.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ReleaseClientAsync()
        {
            if (this.personaLeases.Count == 0)
            {
                return;
            }

            foreach (var lease in this.personaLeases.Values)
            {
                await this.userPoolService.ReleaseAsync(lease).ConfigureAwait(false);
            }

            this.personaLeases.Clear();

            foreach (var client in this.personaClients.Values)
            {
                client.Dispose();
            }

            this.personaClients.Clear();
        }

        /// <summary>
        /// Disposes the test class.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs test clean-up. Disposing cascades to any held persona leases and clients and the configured <see cref="UserPoolService"/>, so callers only need to dispose this factory.
        /// </summary>
        /// <param name="disposing">Disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    foreach (var lease in this.personaLeases.Values)
                    {
                        this.userPoolService.ReleaseAsync(lease).GetAwaiter().GetResult();
                    }

                    this.personaLeases.Clear();

                    foreach (var client in this.personaClients.Values)
                    {
                        client.Dispose();
                    }

                    this.personaClients.Clear();

                    this.userPoolService?.Dispose();
                    this.baseClient.Dispose();
                }

                this.disposedValue = true;
            }
        }

        private static void OptimiseThreads()
        {
            ThreadPool.SetMinThreads(100, 100);
        }

        private static void OptimiseConnections()
        {
            System.Net.ServicePointManager.DefaultConnectionLimit = 65000;
            System.Net.ServicePointManager.Expect100Continue = false;
            System.Net.ServicePointManager.UseNagleAlgorithm = false;
        }
    }
}