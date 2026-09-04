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
        private readonly IReqnrollOutputHelper outputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPoolService"/> class with the specified users.
        /// </summary>
        /// <param name="credentials">The credentials of every user in the pool.</param>
        /// <param name="personaConfigurations">The configuration for every known persona. A credential is treated as explicitly assigned to a persona if its username appears in that persona's <see cref="PersonaConfiguration.Users"/>.</param>
        /// <param name="personaApplicator">The applicator used to dynamically configure users for personas that have no explicitly assigned users.</param>
        /// <param name="outputHelper">The output helper used to log diagnostic information.</param>
        internal UserPoolService(
            IEnumerable<CredentialConfiguration> credentials,
            IDictionary<Persona, PersonaConfiguration> personaConfigurations,
            IPersonaConfigurationApplier personaApplicator,
            IReqnrollOutputHelper outputHelper)
        {
            if (credentials is null)
            {
                throw new ArgumentNullException(nameof(credentials));
            }

            this.personaConfigurations = personaConfigurations ?? throw new ArgumentNullException(nameof(personaConfigurations));
            this.personaApplicator = personaApplicator ?? throw new ArgumentNullException(nameof(personaApplicator));
            this.outputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));

            this.users = credentials
                .Select(c => new Entry(c, this.GetAssignedPersonas(c.Username)))
                .ToList();

            this.outputHelper.WriteLine($"UserPoolService initialized with {this.users.Count} user(s), of which {this.users.Count(e => e.IsStatic)} are statically assigned to personas.");
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

            this.outputHelper.WriteLine($"UserPoolService.GetAsync: requested personas '{string.Join(", ", requested)}'.");

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

                candidates = this.users.Where(e => !e.IsStatic && e.Personas.Count == 0).ToList();
                isDynamic = true;

                if (candidates.Count == 0)
                {
                    throw new InvalidOperationException($"No unassigned user is available to dynamically configure for the requested personas '{string.Join(", ", requested)}'.");
                }

                this.outputHelper.WriteLine($"UserPoolService.GetAsync: no statically assigned user found, {candidates.Count} unassigned user(s) eligible for dynamic configuration.");
            }
            else
            {
                this.outputHelper.WriteLine($"UserPoolService.GetAsync: {candidates.Count} statically assigned user(s) found.");
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
                this.outputHelper.WriteLine($"UserPoolService.GetAsync: timed out after {waitTimeout.TotalMinutes} minutes waiting for personas '{string.Join(", ", requested)}'.");
                throw new TimeoutException($"No user for the requested personas '{string.Join(", ", requested)}' became available within {waitTimeout.TotalMinutes} minutes.");
            }

            cts.Cancel();

            var winnerEntry = await winnerTask.ConfigureAwait(false);
            winnerEntry.MarkAcquired();

            this.outputHelper.WriteLine($"UserPoolService.GetAsync: acquired user '{winnerEntry.Value.Username}' ({(isDynamic ? "dynamic" : "static")}).");

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

            if (isDynamic)
            {
                try
                {
                    var configurations = requested.Select(p => this.personaConfigurations[p]).ToList();
                    await this.personaApplicator.ApplyAsync(winnerEntry.Value.Username, configurations).ConfigureAwait(false);
                    winnerEntry.Personas = requested;
                    this.outputHelper.WriteLine($"UserPoolService.GetAsync: applied dynamic persona configuration for '{string.Join(", ", requested)}' to user '{winnerEntry.Value.Username}'.");
                }
                catch (Exception ex)
                {
                    this.outputHelper.WriteLine($"UserPoolService.GetAsync: failed to apply dynamic persona configuration to user '{winnerEntry.Value.Username}': {ex.Message}.");
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
        /// Releases a previously acquired user lease, removing any dynamically applied persona configuration and returning it to the pool, and signalling any code holding the lease that it has been revoked. If the lease has already expired by the time this method is called, it will have no effect since the user will already have been returned to the pool and any holders will have already been signalled.
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

        private async Task ReleaseEntryAsync(Entry entry, UserLease lease)
        {
            var wasExpired = lease.IsExpired;

            // Signal first so the revocation token fires before the gate opens, preventing any
            // new acquirer from seeing a non-revoked token on the old lease object.
            lease.SignalRevoked();

            // Only proceed if the lease timer has not already done so.
            if (!entry.TryClaimRelease())
            {
                this.outputHelper.WriteLine($"UserPoolService.ReleaseEntryAsync: user '{entry.Value.Username}' has already been released, skipping.");
                return;
            }

            this.outputHelper.WriteLine($"UserPoolService.ReleaseEntryAsync: releasing user '{entry.Value.Username}'{(wasExpired ? " (lease timed out)" : string.Empty)}.");

            if (!entry.IsStatic && entry.Personas.Count > 0)
            {
                await this.personaApplicator.RemoveAsync(entry.Value.Username).ConfigureAwait(false);
                entry.Personas = new HashSet<Persona>();
                this.outputHelper.WriteLine($"UserPoolService.ReleaseEntryAsync: removed dynamic persona configuration from user '{entry.Value.Username}'.");
            }

            entry.OpenGate();
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