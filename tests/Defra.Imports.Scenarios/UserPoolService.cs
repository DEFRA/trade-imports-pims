namespace Defra.Imports.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios.Config;

    /// <summary>
    /// Manages the user pool.
    /// </summary>
    public sealed class UserPoolService : IDisposable
    {
        private readonly List<Entry> users;
        private readonly IDictionary<Persona, PersonaConfiguration> personaConfigurations;
        private readonly IPersonaConfigurationApplier personaApplicator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPoolService"/> class with the specified users.
        /// </summary>
        /// <param name="usernames">The username of every user in the pool. Full credentials (e.g. passwords) are not required here since the pool only ever needs to identify and impersonate users, not log in as them.</param>
        /// <param name="personaConfigurations">The configuration for every known persona. A user is treated as explicitly assigned to a persona if its username appears in that persona's <see cref="PersonaConfiguration.Users"/>.</param>
        /// <param name="personaApplicator">The applicator used to dynamically configure users for personas that have no explicitly assigned users.</param>
        public UserPoolService(
            IEnumerable<string> usernames,
            IDictionary<Persona, PersonaConfiguration> personaConfigurations,
            IPersonaConfigurationApplier personaApplicator)
        {
            if (usernames is null)
            {
                throw new ArgumentNullException(nameof(usernames));
            }

            this.personaConfigurations = personaConfigurations ?? throw new ArgumentNullException(nameof(personaConfigurations));
            this.personaApplicator = personaApplicator ?? throw new ArgumentNullException(nameof(personaApplicator));

            this.users = usernames
                .Select(u => new Entry(u, this.GetAssignedPersonas(u)))
                .ToList();
        }

        /// <summary>
        /// Gets a user from the pool with exactly the specified personas, waiting if necessary until one becomes available. A statically assigned user matching the exact persona set is used immediately if one is idle; if no such user exists, or every one of them is already in use, an unassigned user is instead borrowed from the pool and dynamically configured to match, for the duration the caller holds it. If a statically assigned match exists but all are currently in use, waiting for one of them to free up is raced against waiting for an unassigned user to become free for dynamic configuration, and whichever becomes available first is used. Dynamically applied configuration remains in place until the user is next acquired, at which point it is removed immediately before the next configuration is applied. Callers must call <see cref="ReleaseAsync"/> to return the user to the pool as soon as they are finished with it; there is no automatic reclaim, so a caller that never releases will permanently remove the user from the pool for the remainder of the run.
        /// </summary>
        /// <param name="personas">The personas the returned user must have.</param>
        /// <returns>The username of the acquired user.</returns>
        /// <exception cref="ArgumentException">Thrown if no personas are specified.</exception>
        /// <exception cref="InvalidOperationException">Thrown if no matching users exist.</exception>
        /// <exception cref="TimeoutException">Thrown if waiting for longer than 30 minutes.</exception>
        public async Task<string> GetAsync(params Persona[] personas)
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

            var staticCandidates = this.users.Where(e => e.IsStatic && e.Personas.SetEquals(requested)).ToList();
            var unassigned = this.users.Where(e => !e.IsStatic).ToList();

            List<Entry> candidates;

            if (staticCandidates.Count == 0)
            {
                if (unassigned.Count == 0)
                {
                    throw new InvalidOperationException($"No unassigned user is available to dynamically configure for the requested personas '{string.Join(", ", requested)}'.");
                }

                candidates = unassigned;
            }
            else
            {
                candidates = staticCandidates;

                if (unassigned.Count > 0 && staticCandidates.All(e => e.Gate.CurrentCount == 0))
                {
                    // Every statically assigned user is currently in use; race waiting for one of them to be
                    // released against waiting for an unassigned user to become free for dynamic configuration.
                    candidates = staticCandidates.Concat(unassigned).ToList();
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

            if (!winnerEntry.IsStatic && !(winnerEntry.PersonaStateVerified && winnerEntry.Personas.SetEquals(requested)))
            {
                try
                {
                    // Strip any leftover configuration from the previous acquisition here, right
                    // before applying the new one, rather than at release time (which is unreliable).
                    // An unverified entry may carry stale remote configuration from a process that
                    // exited early, so it must be removed defensively regardless of the tracked personas.
                    if (!winnerEntry.PersonaStateVerified || winnerEntry.Personas.Count > 0)
                    {
                        await this.personaApplicator.RemoveAsync(winnerEntry.Value).ConfigureAwait(false);
                    }

                    var configurations = requested.Select(p => this.personaConfigurations[p]).ToList();
                    await this.personaApplicator.ApplyAsync(winnerEntry.Value, configurations).ConfigureAwait(false);
                    winnerEntry.Personas = requested;
                    winnerEntry.PersonaStateVerified = true;
                }
                catch (Exception)
                {
                    winnerEntry.Gate.Release();
                    throw;
                }
            }

            return winnerEntry.Value;
        }

        /// <summary>
        /// Releases a previously acquired user, returning it to the pool. Any dynamically applied persona configuration is left in place until the entry is next acquired, at which point it is removed just before the new configuration is applied.
        /// </summary>
        /// <param name="username">The username of the user to release.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the username is not found in the pool.</exception>
        public Task ReleaseAsync(string username)
        {
            if (username is null)
            {
                return Task.CompletedTask;
            }

            var entry = this.users.FirstOrDefault(e => e.Value == username)
                ?? throw new InvalidOperationException("The provided user does not belong to the pool.");

            entry.Gate.Release();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Resolves the app registration ID for the requested personas when they are represented by an application user, returning <c>null</c> when the request is entirely for pooled users.
        /// </summary>
        /// <param name="personas">The requested personas.</param>
        /// <returns>The shared application ID for the requested personas, or <c>null</c> if none of them are represented by an application user.</returns>
        public Guid? TryGetAppId(IEnumerable<Persona> personas)
        {
            if (personas is null)
            {
                throw new ArgumentNullException(nameof(personas));
            }

            var requested = personas.ToList();
            var appIdMatches = requested
                .Select(p => this.personaConfigurations.TryGetValue(p, out var config) ? config.AppId : null)
                .ToList();

            var appIds = appIdMatches
                .Where(appId => appId.HasValue)
                .Select(appId => appId.Value)
                .Distinct()
                .ToList();

            if (appIds.Count == 0)
            {
                return null;
            }

            if (appIdMatches.Count != requested.Count)
            {
                throw new InvalidOperationException("Application-user personas cannot be combined with pooled personas in the same request.");
            }

            if (appIds.Count > 1)
            {
                throw new InvalidOperationException($"The requested personas resolve to multiple application IDs: {string.Join(", ", appIds)}.");
            }

            return appIds[0];
        }

        private IEnumerable<Persona> GetAssignedPersonas(string username)
        {
            return this.personaConfigurations
                .Where(p => p.Value.Users != null && p.Value.Users.Contains(username))
                .Select(p => p.Key);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.personaApplicator.Dispose();
        }

        private sealed class Entry
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Entry"/> class with the specified value and personas.
            /// </summary>
            /// <param name="value">The username.</param>
            /// <param name="personas">The personas explicitly configured for this user.</param>
            public Entry(string value, IEnumerable<Persona> personas)
            {
                this.Value = value;
                this.Personas = new HashSet<Persona>(personas ?? Array.Empty<Persona>());
                this.IsStatic = this.Personas.Count > 0;
            }

            public string Value { get; }

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
        }
    }
}
