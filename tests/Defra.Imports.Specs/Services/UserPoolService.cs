namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Specs.Config;
    using Reqnroll;

    /// <summary>
    /// Manages the user pool.
    /// </summary>
    public sealed class UserPoolService
    {
        /// <summary>The maximum time a caller may hold a user before it is automatically returned to the pool.</summary>
        internal static readonly TimeSpan LeaseTimeout = TimeSpan.FromMinutes(5);

        private readonly List<Entry> users;
        private readonly IDictionary<Persona, PersonaConfiguration> personaConfigurations;
        private readonly IPersonaConfigurationApplier personaApplicator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPoolService"/> class with the specified users.
        /// </summary>
        /// <param name="credentials">The credentials of every user in the pool.</param>
        /// <param name="personaConfigurations">The configuration for every known persona. A credential is treated as explicitly assigned to a persona if its username appears in that persona's <see cref="PersonaConfiguration.Users"/>.</param>
        /// <param name="personaApplicator">The applicator used to dynamically configure users for personas that have no explicitly assigned users.</param>
        internal UserPoolService(
            IEnumerable<CredentialConfiguration> credentials,
            IDictionary<Persona, PersonaConfiguration> personaConfigurations,
            IPersonaConfigurationApplier personaApplicator)
        {
            if (credentials is null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            this.personaConfigurations = personaConfigurations ?? throw new ArgumentNullException(nameof(personaConfigurations));
            this.personaApplicator = personaApplicator ?? throw new ArgumentNullException(nameof(personaApplicator));

            this.users = credentials
                .Select(c => new Entry(c, this.GetAssignedPersonas(c.Username)))
                .ToList();
        }

        /// <summary>
        /// Gets a user from the pool with exactly the specified personas, waiting if necessary until one becomes available. If no user has been explicitly configured (via <see cref="PersonaConfiguration.Users"/>) for every one of the requested personas, an unassigned user is instead borrowed from the pool and dynamically configured to match, for the duration of the lease. The returned user is leased to the caller for a maximum of <see cref="LeaseTimeout"/>, after which it is automatically returned to the pool (and any dynamically applied configuration removed) and any code holding the lease is signalled that it has been revoked. Callers should call <see cref="ReleaseAsync"/> to return the user to the pool as soon as they are finished with it, which will also signal any holders that the lease has been revoked; if they do not do so within the lease timeout, the user will be returned to the pool automatically when the timeout is exceeded.
        /// </summary>
        /// <param name="personas">The personas the returned user must have.</param>
        /// <returns>A lease on the acquired user.</returns>
        /// <exception cref="ArgumentException">Thrown if no personas are specified.</exception>
        /// <exception cref="InvalidOperationException">Thrown if no matching users exist.</exception>
        /// <exception cref="TimeoutException">Thrown if waiting for longer than 30 minutes.</exception>
        internal async Task<UserLease> GetAsync(IEnumerable<Persona> personas)
        {
            if (personas is null)
            {
                throw new ArgumentNullException(nameof(personas));
            }

            var requested = new HashSet<Persona>(personas);

            if (requested.Count == 0)
            {
                throw new ArgumentException("At least one persona must be specified.", nameof(personas));
            }

            var unknownPersonas = requested.Where(p => !this.personaConfigurations.ContainsKey(p)).ToList();
            if (unknownPersonas.Count > 0)
            {
                throw new InvalidOperationException($"No configuration exists for the following personas: {string.Join(", ", unknownPersonas)}.");
            }

            var candidates = this.users.Where(e => e.IsStatic && e.Personas.SetEquals(requested)).ToList();
            var isDynamic = false;

            if (candidates.Count == 0)
            {
                // Only personas with no explicitly configured users are eligible to be dynamically applied to an unassigned user.
                var canApplyDynamically = requested.All(p => this.personaConfigurations[p].Users == null || !this.personaConfigurations[p].Users.Any());

                if (!canApplyDynamically)
                {
                    throw new InvalidOperationException($"No user exists for the requested personas '{string.Join(", ", requested)}'.");
                }

                candidates = this.users.Where(e => !e.IsStatic).ToList();
                isDynamic = true;

                if (candidates.Count == 0)
                {
                    throw new InvalidOperationException($"No unassigned user is available to dynamically configure for the requested personas '{string.Join(", ", requested)}'.");
                }
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
                throw new TimeoutException($"No user for the requested personas '{string.Join(", ", requested)}' became available within {waitTimeout.TotalMinutes} minutes.");
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

            if (isDynamic && !(winnerEntry.PersonaStateVerified && winnerEntry.Personas.SetEquals(requested)))
            {
                try
                {
                    // Strip any leftover configuration from the previous dynamic lease here, right
                    // before applying the new one, rather than at release time (which is unreliable).
                    // An unverified entry may carry stale remote configuration from a process that
                    // exited early, so it must be removed defensively regardless of the tracked personas.
                    if (!winnerEntry.PersonaStateVerified || winnerEntry.Personas.Count > 0)
                    {
                        await this.personaApplicator.RemoveAsync(winnerEntry.Value.Username).ConfigureAwait(false);
                    }

                    var configurations = requested.Select(p => this.personaConfigurations[p]).ToList();
                    await this.personaApplicator.ApplyAsync(winnerEntry.Value.Username, configurations).ConfigureAwait(false);
                    winnerEntry.Personas = requested;
                    winnerEntry.PersonaStateVerified = true;
                }
                catch (Exception ex)
                {
                    winnerEntry.TryClaimRelease();
                    winnerEntry.OpenGate();
                    throw;
                }
            }

            // Create a lease CTS with the maximum hold time. If it fires before Release() is called
            // the gate is returned to the pool automatically and the revocation token is signalled
            // so any code holding the lease can detect it.
            var leaseCts = new CancellationTokenSource(LeaseTimeout);
            var lease = new UserLease(winnerEntry.Value, leaseCts);

            leaseCts.Token.Register(() => _ = this.ReleaseEntryAsync(winnerEntry, lease));

            return lease;
        }

        /// <summary>
        /// Releases a previously acquired user lease, returning it to the pool and signalling any code holding the lease that it has been revoked. Any dynamically applied persona configuration is left in place until the entry is next leased, at which point it is removed just before the new configuration is applied. If the lease has already expired by the time this method is called, it will have no effect since the user will already have been returned to the pool and any holders will have already been signalled.
        /// </summary>
        /// <param name="lease">The lease.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the lease is for credentials not found in the pool.</exception>
        internal Task ReleaseAsync(UserLease lease)
        {
            if (lease is null)
            {
                return Task.CompletedTask;
            }

            var entry = this.users.FirstOrDefault(e => e.Value == lease.Credentials)
                ?? throw new InvalidOperationException("The provided credentials do not belong to the pool.");

            return this.ReleaseEntryAsync(entry, lease);
        }

        private Task ReleaseEntryAsync(Entry entry, UserLease lease)
        {
            // Signal first so the revocation token fires before the gate opens, preventing any
            // new acquirer from seeing a non-revoked token on the old lease object.
            lease.SignalRevoked();

            // Only proceed if the lease timer has not already done so.
            if (entry.TryClaimRelease())
            {
                entry.OpenGate();
            }

            return Task.CompletedTask;
        }

        private IEnumerable<Persona> GetAssignedPersonas(string username)
        {
            return this.personaConfigurations
                .Where(p => p.Value.Users != null && p.Value.Users.Contains(username))
                .Select(p => p.Key);
        }

        private sealed class Entry
        {
            private int acquireCount;

            /// <summary>
            /// Initializes a new instance of the <see cref="Entry"/> class with the specified value and personas.
            /// </summary>
            /// <param name="value">The credentials.</param>
            /// <param name="personas">The personas explicitly configured for this user.</param>
            public Entry(CredentialConfiguration value, IEnumerable<Persona> personas)
            {
                this.Value = value;
                this.Personas = new HashSet<Persona>(personas ?? Array.Empty<Persona>());
                this.IsStatic = this.Personas.Count > 0;
            }

            public CredentialConfiguration Value { get; }

            /// <summary>
            /// Gets a value indicating whether this user was explicitly configured for its personas, as opposed to being an unassigned user available for dynamic configuration.
            /// </summary>
            public bool IsStatic { get; }

            public HashSet<Persona> Personas { get; set; }

            /// <summary>
            /// Gets or sets a value indicating whether <see cref="Personas"/> reflects a remove/apply this process actually performed. A previous process may have exited before releasing a dynamically configured user, leaving stale configuration in place that this process has no record of, so a freshly constructed entry cannot be trusted until it has been forcibly synced at least once.
            /// </summary>
            public bool PersonaStateVerified { get; set; }

            public SemaphoreSlim Gate { get; } = new SemaphoreSlim(1, 1);

            public void MarkAcquired()
            {
                Interlocked.Increment(ref this.acquireCount);
            }

            /// <summary>
            /// Attempts to claim responsibility for releasing this entry. Returns <c>true</c> if this call claimed it (and the caller must eventually call <see cref="OpenGate"/>);
            /// <c>false</c> if it was already claimed by a concurrent caller (e.g. a concurrent lease expiry).
            /// </summary>
            public bool TryClaimRelease()
            {
                var remaining = Interlocked.Decrement(ref this.acquireCount);

                if (remaining < 0)
                {
                    // Already claimed — restore the counter and report back without throwing.
                    Interlocked.Increment(ref this.acquireCount);
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Opens the gate, making this entry available to the next caller. Must only be called after a successful <see cref="TryClaimRelease"/>.
            /// </summary>
            public void OpenGate()
            {
                this.Gate.Release();
            }
        }
    }
}