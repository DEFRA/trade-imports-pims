namespace Defra.Imports.Specs.Services
{
    using System.Threading;
    using Defra.Imports.Specs.Config;

    /// <summary>
    /// Represents a time-limited lease on a user from the <see cref="UserPoolService"/>.
    /// </summary>
    internal sealed class UserLease
    {
        private readonly CancellationTokenSource leaseCts;
        private bool explicitlyReleased;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLease"/> class.
        /// </summary>
        /// <param name="credentials">The acquired user credentials.</param>
        /// <param name="leaseCts">The <see cref="CancellationTokenSource"/> that will be cancelled when the lease expires or is released.</param>
        internal UserLease(CredentialConfiguration credentials, CancellationTokenSource leaseCts)
        {
            this.Credentials = credentials;
            this.leaseCts = leaseCts;
        }

        /// <summary>
        /// Gets the credentials for the leased user.
        /// </summary>
        internal CredentialConfiguration Credentials { get; }

        /// <summary>
        /// Gets a <see cref="CancellationToken"/> that is cancelled when the lease expires or is explicitly released.
        /// Tests can observe this token to detect mid-scenario revocation.
        /// </summary>
        internal CancellationToken RevocationToken => this.leaseCts.Token;

        /// <summary>
        /// Gets a value indicating whether the lease was revoked automatically by the timer
        /// (as opposed to being released explicitly via <see cref="SignalRevoked"/>).
        /// </summary>
        internal bool IsExpired => this.leaseCts.IsCancellationRequested && !this.explicitlyReleased;

        /// <summary>
        /// Cancels the lease CTS so the revocation token fires immediately on an explicit release.
        /// Marks the lease as explicitly released so <see cref="IsExpired"/> returns <c>false</c>.
        /// </summary>
        internal void SignalRevoked()
        {
            this.explicitlyReleased = true;

            if (!this.leaseCts.IsCancellationRequested)
            {
                this.leaseCts.Cancel();
            }
        }
    }
}
