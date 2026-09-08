namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Specs.Config;
    using Reqnroll;

    /// <summary>
    /// A client for acquiring users from the user pool within a scenario, which manages the lease on the acquired user and ensures it is released back to the pool when the scenario finishes. Scenarios should acquire users through this class rather than directly through <see cref="UserPoolService"/> to ensure proper lease management and cleanup.
    /// </summary>
    public sealed class UserPoolClient : IDisposable
    {
        /// <summary>
        /// The key in the <see cref="ScenarioContext"/> dictionary under which a <see cref="LeaseRevokedException"/> is stored when a lease is automatically revoked due to timeout, so that it can be re-thrown on the scenario thread by UserHooks to fail the scenario with a clear message about the cause of the failure.
        /// </summary>
        public const string LeaseRevokedErrorKey = "UserPoolClient.LeaseRevokedError";

        private readonly UserPoolService userPoolService;
        private readonly IReqnrollOutputHelper outputHelper;
        private readonly ScenarioContext scenarioContext;
        private bool disposed;

        private UserLease currentLease;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPoolClient"/> class.
        /// </summary>
        /// <param name="userPoolService">The user pool service.</param>
        /// <param name="outputHelper">The output helper.</param>
        /// <param name="scenarioContext">The scenario context.</param>
        public UserPoolClient(UserPoolService userPoolService, IReqnrollOutputHelper outputHelper, ScenarioContext scenarioContext)
        {
            this.userPoolService = userPoolService;
            this.outputHelper = outputHelper;
            this.scenarioContext = scenarioContext;
        }

        /// <summary>
        /// Gets a <see cref="CancellationToken"/> that is cancelled if the current user lease is revoked
        /// (either because the lease timeout was hit, or because <see cref="ReleaseAsync"/> was called).
        /// Returns <see cref="CancellationToken.None"/> when no user is currently held.
        /// </summary>
        public CancellationToken LeaseRevocationToken => this.currentLease?.RevocationToken ?? CancellationToken.None;

        /// <summary>
        /// Gets credentials for a user from the pool with exactly the specified personas, waiting if necessary until one becomes available, and begins a lease on that user which will be automatically revoked after a maximum of <see cref="UserPoolService.LeaseTimeout"/>. If no user has been explicitly configured for every one of the requested personas, an unassigned user is borrowed from the pool and dynamically configured to match for the duration of the lease. The returned credentials should be used to log in as that user and perform test actions; when the test is finished with the user, it should call <see cref="ReleaseAsync"/> to end the lease and return the user to the pool. If the test does not call <see cref="ReleaseAsync"/> within the lease timeout, the lease will be automatically revoked and any code holding the lease can observe this through the <see cref="LeaseRevocationToken"/>. Scenarios should not call this method more than once without releasing in between, as only one user can be held at a time.
        /// </summary>
        /// <param name="personas">The personas the returned user must have.</param>
        /// <returns>The user credentials.</returns>
        /// <exception cref="InvalidOperationException">Thrown if a user has already been requested from the pool for this scenario.</exception>
        public async Task<CredentialConfiguration> GetAsync(IEnumerable<Persona> personas)
        {
            if (this.currentLease != null)
            {
                throw new InvalidOperationException("You can only request a single user from the pool at a time.");
            }

            this.outputHelper.WriteLine("Waiting for user with personas: " + string.Join(", ", personas));
            this.currentLease = await this.userPoolService.GetAsync(personas).ConfigureAwait(false);
            this.outputHelper.WriteLine($"Running as user with username: {this.currentLease.Credentials.Username}. Lease will expire in {UserPoolService.LeaseTimeout.TotalMinutes} minutes.");

            var capturedLease = this.currentLease;

            // When the lease is auto-revoked by the timer, store a LeaseRevokedException in the
            // ScenarioContext dictionary so that UserHooks can re-throw it on the scenario thread,
            // failing the scenario with a clear message. The registration fires on a threadpool
            // thread so we cannot throw directly here.
            capturedLease.RevocationToken.Register(() =>
            {
                if (capturedLease.IsExpired)
                {
                    var ex = new LeaseRevokedException(
                        capturedLease.Credentials.Username,
                        UserPoolService.LeaseTimeout);

                    this.scenarioContext[LeaseRevokedErrorKey] = ex;
                    this.outputHelper.WriteLine(ex.Message);
                }
            });

            return this.currentLease.Credentials;
        }

        /// <summary>
        /// Releases the currently held user back to the pool, ending the lease. If no user is currently held, this method does nothing. Scenarios should call this method as soon as they are finished with a leased user to ensure it is returned to the pool promptly for use by other scenarios, and to signal any code holding the lease that it has been revoked. If they do not call this method within the lease timeout, the lease will be automatically revoked when the timeout is exceeded and the user will be returned to the pool at that time.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ReleaseAsync()
        {
            if (this.currentLease == null)
            {
                return;
            }

            this.outputHelper.WriteLine("Releasing user with username: " + this.currentLease.Credentials.Username);
            await this.userPoolService.ReleaseAsync(this.currentLease).ConfigureAwait(false);

            this.currentLease = null;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;

            if (this.currentLease != null)
            {
                this.outputHelper.WriteLine($"Releasing user '{this.currentLease.Credentials.Username}' during disposal — was the AfterScenario hook skipped?");
                this.userPoolService.ReleaseAsync(this.currentLease).GetAwaiter().GetResult();
                this.currentLease = null;
            }
        }
    }
}