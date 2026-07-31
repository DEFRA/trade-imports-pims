namespace Defra.Imports.Specs.Services
{
    using System;

    /// <summary>
    /// Thrown when a user lease is revoked automatically due to the lease timeout being exceeded.
    /// </summary>
    public sealed class LeaseRevokedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LeaseRevokedException"/> class.
        /// </summary>
        /// <param name="username">The username whose lease was revoked.</param>
        /// <param name="leaseTimeout">The lease duration that was exceeded.</param>
        public LeaseRevokedException(string username, TimeSpan leaseTimeout)
            : base($"The lease for user '{username}' was automatically revoked after {leaseTimeout.TotalMinutes} minutes. The scenario held the user for too long.")
        {
        }
    }
}
