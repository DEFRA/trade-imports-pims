namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Scenarios.Config;

    /// <summary>
    /// A client for acquiring users from the user pool within a scenario, which manages the acquired user(s) and ensures they are released back to the pool when the scenario is finished. Callers should acquire users through this class rather than directly through <see cref="UserPoolService"/> to ensure proper cleanup. Distinct persona (or persona-set) requests are acquired and cached independently, so multiple different personas can be held concurrently. Instances are intended to be created once per scenario.
    /// </summary>
    public sealed class ScenarioUserClient : IDisposable
    {
        private readonly UserPoolService userPoolService;
        private readonly IDictionary<string, CredentialConfiguration> credentialsByUsername;
        private readonly Dictionary<string, string> usernamesByKey = new Dictionary<string, string>();
        private readonly object usernamesByKeyLock = new object();
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioUserClient"/> class.
        /// </summary>
        /// <param name="userPoolService">The user pool service.</param>
        /// <param name="credentials">The full credentials (including passwords) for every user that may be borrowed from <paramref name="userPoolService"/>, used to resolve the credentials to log in as a borrowed user.</param>
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
        /// Raised with informational messages describing acquisition/release activity, for callers to surface via their own logging/output mechanism.
        /// </summary>
        public event Action<string> Logged;

        /// <summary>
        /// Gets credentials for a user from the pool with exactly the specified personas, waiting if necessary until one becomes available. If no user has been explicitly configured for every one of the requested personas, an unassigned user is borrowed from the pool and dynamically configured to match for the duration it is held. The returned credentials should be used to log in as that user and perform test actions. Distinct persona combinations are acquired and cached independently, so this method can be called for several different combinations without releasing in between; calling it again for a combination already held returns the same credentials. When the caller is finished with all acquired users, it should call <see cref="ReleaseAsync"/> to return the users to the pool.
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

            lock (this.usernamesByKeyLock)
            {
                if (this.usernamesByKey.TryGetValue(key, out var cachedUsername))
                {
                    return this.GetCredentials(cachedUsername);
                }
            }

            this.Logged?.Invoke("Waiting for user with personas: " + string.Join(", ", personas));
            var username = await this.userPoolService.GetAsync(personas).ConfigureAwait(false);
            this.Logged?.Invoke($"Running as user with username: {username}.");

            lock (this.usernamesByKeyLock)
            {
                this.usernamesByKey[key] = username;
            }

            return this.GetCredentials(username);
        }

        /// <summary>
        /// Releases every user currently held by this client back to the pool. If no user is currently held, this method does nothing. Callers should call this method as soon as they are finished with all acquired users to ensure they are returned to the pool promptly for use elsewhere.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ReleaseAsync()
        {
            List<KeyValuePair<string, string>> users;
            lock (this.usernamesByKeyLock)
            {
                users = this.usernamesByKey.ToList();
            }

            foreach (var user in users)
            {
                this.Logged?.Invoke("Releasing user with username: " + user.Value);
                await this.userPoolService.ReleaseAsync(user.Value).ConfigureAwait(false);

                lock (this.usernamesByKeyLock)
                {
                    if (this.usernamesByKey.TryGetValue(user.Key, out var currentUsername) && currentUsername == user.Value)
                    {
                        this.usernamesByKey.Remove(user.Key);
                    }
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

            List<string> usernames;
            lock (this.usernamesByKeyLock)
            {
                usernames = this.usernamesByKey.Values.ToList();
                this.usernamesByKey.Clear();
            }

            foreach (var username in usernames)
            {
                this.Logged?.Invoke($"Releasing user '{username}' during disposal — was the release step skipped?");
                this.userPoolService.ReleaseAsync(username).GetAwaiter().GetResult();
            }
        }

        private CredentialConfiguration GetCredentials(string username)
        {
            if (!this.credentialsByUsername.TryGetValue(username, out var credentials))
            {
                throw new InvalidOperationException($"No credentials have been configured for the '{username}' user.");
            }

            return credentials;
        }
    }
}
