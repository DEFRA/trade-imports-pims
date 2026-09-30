namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Scenarios.Config;

    /// <summary>
    /// A client for acquiring users from the user pool within a scenario, which manages the lease(s) on the acquired user(s) and ensures they are released back to the pool when the scenario is finished. Callers should acquire users through this class rather than directly through <see cref="UserPoolService"/> to ensure proper lease management and cleanup. Distinct persona (or persona-set) requests are leased and cached independently, so multiple different personas can be held concurrently. Instances are intended to be created once per scenario.
    /// </summary>
    public sealed class ScenarioUserClient : IDisposable
    {
        private readonly UserPoolService userPoolService;
        private readonly IDictionary<string, CredentialConfiguration> credentialsByUsername;
        private readonly Dictionary<string, UserLease> leasesByKey = new Dictionary<string, UserLease>();
        private readonly object leasesByKeyLock = new object();
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioUserClient"/> class.
        /// </summary>
        /// <param name="userPoolService">The user pool service.</param>
        /// <param name="credentials">The full credentials (including passwords) for every user that may be leased from <paramref name="userPoolService"/>, used to resolve the credentials to log in as a leased user.</param>
        public ScenarioUserClient(UserPoolService userPoolService, IEnumerable<CredentialConfiguration> credentials)
        {
            this.userPoolService = userPoolService ?? throw new ArgumentNullException(nameof(userPoolService));

            if (credentials is null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            this.credentialsByUsername = credentials.ToDictionary(c => c.Username);
        }

        /// <summary>
        /// Raised with informational messages describing lease acquisition/release activity, for callers to surface via their own logging/output mechanism.
        /// </summary>
        public event Action<string> Logged;

        /// <summary>
        /// Raised when a held lease is automatically revoked due to the lease timeout being exceeded, from a threadpool thread.
        /// </summary>
        public event Action<LeaseRevokedException> Revoked;

        /// <summary>
        /// Gets credentials for a user from the pool with exactly the specified personas, waiting if necessary until one becomes available, and begins a lease on that user which will be automatically revoked after a maximum of <see cref="UserPoolService.LeaseTimeout"/>. If no user has been explicitly configured for every one of the requested personas, an unassigned user is borrowed from the pool and dynamically configured to match for the duration of the lease. The returned credentials should be used to log in as that user and perform test actions. Distinct persona combinations are leased and cached independently, so this method can be called for several different combinations without releasing in between; calling it again for a combination already held returns the same credentials. When the caller is finished with all leased users, it should call <see cref="ReleaseAsync"/> to end the leases and return the users to the pool. If it does not do so within the lease timeout, a lease will be automatically revoked and any code holding it can observe this through the <see cref="Revoked"/> event.
        /// </summary>
        /// <param name="personas">The personas the returned user must have.</param>
        /// <returns>The user credentials.</returns>
        public async Task<CredentialConfiguration> GetAsync(params Persona[] personas)
        {
            if (personas is null)
            {
                throw new ArgumentNullException(nameof(personas));
            }

            var key = PersonaSetKey.Create(personas);

            lock (this.leasesByKeyLock)
            {
                if (this.leasesByKey.TryGetValue(key, out var cachedLease))
                {
                    return this.GetCredentials(cachedLease);
                }
            }

            this.Logged?.Invoke("Waiting for user with personas: " + string.Join(", ", personas));
            var lease = await this.userPoolService.GetAsync(personas).ConfigureAwait(false);
            this.Logged?.Invoke($"Running as user with username: {lease.Username}. Lease will expire in {UserPoolService.LeaseTimeout.TotalMinutes} minutes.");

            lock (this.leasesByKeyLock)
            {
                this.leasesByKey[key] = lease;
            }

            // The registration fires on a threadpool thread, so revocation is signalled via the
            // Revoked event rather than thrown directly.
            lease.RevocationToken.Register(() =>
            {
                if (!lease.IsExpired)
                {
                    return;
                }

                UserLease removedLease;
                lock (this.leasesByKeyLock)
                {
                    removedLease = this.leasesByKey.TryGetValue(key, out var value) ? value : null;
                    if (removedLease == lease)
                    {
                        this.leasesByKey.Remove(key);
                    }
                }

                if (removedLease == lease)
                {
                    var ex = new LeaseRevokedException(lease.Username, UserPoolService.LeaseTimeout);
                    this.Revoked?.Invoke(ex);
                    this.Logged?.Invoke(ex.Message);
                }
            });

            return this.GetCredentials(lease);
        }

        /// <summary>
        /// Releases every user currently held by this client back to the pool, ending their leases. If no user is currently held, this method does nothing. Callers should call this method as soon as they are finished with all leased users to ensure they are returned to the pool promptly for use elsewhere. If they do not call this method within a lease's timeout, that lease will be automatically revoked when the timeout is exceeded and the user will be returned to the pool at that time.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ReleaseAsync()
        {
            List<UserLease> leases;
            lock (this.leasesByKeyLock)
            {
                leases = this.leasesByKey.Values.ToList();
                this.leasesByKey.Clear();
            }

            foreach (var lease in leases)
            {
                this.Logged?.Invoke("Releasing user with username: " + lease.Username);
                await this.userPoolService.ReleaseAsync(lease).ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            List<UserLease> leases;
            lock (this.leasesByKeyLock)
            {
                leases = this.leasesByKey.Values.ToList();
                this.leasesByKey.Clear();
            }

            foreach (var lease in leases)
            {
                this.Logged?.Invoke($"Releasing user '{lease.Username}' during disposal — was the release step skipped?");
                this.userPoolService.ReleaseAsync(lease).GetAwaiter().GetResult();
            }
        }

        private CredentialConfiguration GetCredentials(UserLease lease)
        {
            if (!this.credentialsByUsername.TryGetValue(lease.Username, out var credentials))
            {
                throw new InvalidOperationException($"No credentials have been configured for the '{lease.Username}' user.");
            }

            return credentials;
        }
    }
}
