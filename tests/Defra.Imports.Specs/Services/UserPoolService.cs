namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Specs.Config;

    /// <summary>
    /// Manages the user pool.
    /// </summary>
    public sealed class UserPoolService
    {
        /// <summary>The maximum time a caller may hold a user before it is automatically returned to the pool.</summary>
        internal static readonly TimeSpan LeaseTimeout = TimeSpan.FromMinutes(5);

        private readonly List<Entry> users;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPoolService"/> class with the specified users.
        /// </summary>
        /// <param name="users">The users.</param>
        public UserPoolService(IEnumerable<(CredentialConfiguration, IEnumerable<Persona>, IEnumerable<string>)> users)
        {
            this.users = users
                .Select(u => new Entry(u.Item1, u.Item2, u.Item3))
                .ToList();
        }

        /// <summary>
        /// Gets a user from the pool matching the specified alias and persona criteria, waiting if necessary until one becomes available. The returned user is leased to the caller for a maximum of <see cref="LeaseTimeout"/>, after which it is automatically returned to the pool and any code holding the lease is signalled that it has been revoked. Callers should call <see cref="Release"/> to return the user to the pool as soon as they are finished with it, which will also signal any holders that the lease has been revoked; if they do not do so within the lease timeout, the user will be returned to the pool automatically when the timeout is exceeded.
        /// </summary>
        /// <param name="alias">The alias.</param>
        /// <param name="allowMultiplePersonas">Whether or not the user is allowed to have other personas in addition to the persona described by the alias.</param>
        /// <returns>A lease on the acquired user.</returns>
        /// <exception cref="InvalidOperationException">Thrown if no matching users exist.</exception>
        /// <exception cref="TimeoutException">Thrown if waiting for longer than 30 minutes.</exception>
        internal async Task<UserLease> GetByAliasAsync(string alias, bool allowMultiplePersonas)
        {
            var candidates = this.users
                .Where(e => e.Matches(alias, allowMultiplePersonas))
                .ToList();

            if (candidates.Count == 0)
            {
                throw new InvalidOperationException($"No user exists with alias '{alias}'.");
            }

            var waitTimeout = TimeSpan.FromMinutes(30);
            var cts = new CancellationTokenSource(waitTimeout);

            var tasks = candidates.ToDictionary(
                c => c,
                c => c.Gate.WaitAsync(cts.Token).ContinueWith(
                    t => c,
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default));

            var winnerTask = await Task.WhenAny(tasks.Values).ConfigureAwait(false);

            if (winnerTask.IsCanceled || winnerTask.IsFaulted)
            {
                cts.Dispose();
                throw new TimeoutException($"No user with alias '{alias}' became available within {waitTimeout.TotalMinutes} minutes.");
            }

            cts.Cancel();

            var winnerEntry = await winnerTask.ConfigureAwait(false);
            winnerEntry.MarkAcquired();

            foreach (var kvp in tasks.Where(kvp => kvp.Key != winnerEntry))
            {
                _ = kvp.Value.ContinueWith(
                    _ => kvp.Key.Gate.Release(),
                    CancellationToken.None,
                    TaskContinuationOptions.OnlyOnRanToCompletion,
                    TaskScheduler.Default);
            }

            _ = Task.WhenAll(tasks.Values).ContinueWith(
                _ => cts.Dispose(),
                CancellationToken.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

            // Create a lease CTS with the maximum hold time. If it fires before Release() is called
            // the gate is returned to the pool automatically and the revocation token is signalled
            // so any code holding the lease can detect it.
            var leaseCts = new CancellationTokenSource(LeaseTimeout);
            var lease = new UserLease(winnerEntry.Value, leaseCts);

            leaseCts.Token.Register(() =>
            {
                // Only release if the entry is still considered acquired (guards against a race
                // where Release() fires at almost the same instant as the timer).
                if (winnerEntry.TryRelease())
                {
                    lease.SignalRevoked();
                }
            });

            return lease;
        }

        /// <summary>
        /// Releases a previously acquired user lease, returning it to the pool and signalling any code holding the lease that it has been revoked. If the lease has already expired by the time this method is called, it will have no effect since the user will already have been returned to the pool and any holders will have already been signalled.
        /// </summary>
        /// <param name="lease">The lease.</param>
        /// <exception cref="InvalidOperationException">Thrown if the lease is for credentials not found in the pool.</exception>
        internal void Release(UserLease lease)
        {
            if (lease is null)
            {
                return;
            }

            var entry = this.users.FirstOrDefault(e => e.Value == lease.Credentials)
                ?? throw new InvalidOperationException("The provided credentials do not belong to the pool.");

            // Signal first so the revocation token fires before the gate opens, preventing any
            // new acquirer from seeing a non-revoked token on the old lease object.
            lease.SignalRevoked();

            // Only release the gate if the lease timer has not already done so.
            entry.TryRelease();
        }

        private sealed class Entry
        {
            private int acquireCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="Entry"/> class with the specified value, personas and aliases.
            /// </summary>
            /// <param name="value">The credentials.</param>
            /// <param name="personas">The personas.</param>
            /// <param name="aliases">the aliases.</param>
            public Entry(CredentialConfiguration value, IEnumerable<Persona> personas, IEnumerable<string> aliases)
            {
                this.Value = value;
                this.Aliases = new HashSet<string>(aliases, StringComparer.Ordinal);
                this.Personas = new HashSet<Persona>(personas);
            }

            public CredentialConfiguration Value { get; }

            public HashSet<string> Aliases { get; }

            public HashSet<Persona> Personas { get; set; }

            public SemaphoreSlim Gate { get; } = new SemaphoreSlim(1, 1);

            public void MarkAcquired()
            {
                Interlocked.Increment(ref this.acquireCount);
            }

            /// <summary>
            /// Attempts to release the gate. Returns <c>true</c> if this call performed the release;
            /// <c>false</c> if the gate had already been released (e.g. by a concurrent lease expiry).
            /// </summary>
            public bool TryRelease()
            {
                var remaining = Interlocked.Decrement(ref this.acquireCount);

                if (remaining < 0)
                {
                    // Already released — restore the counter and report back without throwing.
                    Interlocked.Increment(ref this.acquireCount);
                    return false;
                }

                this.Gate.Release();
                return true;
            }

            public bool Matches(string alias, bool allowAdditionalPersonas)
            {
                if (!this.Aliases.Contains(alias))
                {
                    return false;
                }

                return allowAdditionalPersonas || this.Personas.Count == 1;
            }
        }
    }
}