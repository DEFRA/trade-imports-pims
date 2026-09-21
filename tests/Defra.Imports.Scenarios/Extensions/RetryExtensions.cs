namespace Defra.Imports.Scenarios.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// A minimal, dependency-free retry helper for polling eventually-consistent
    /// conditions (e.g. waiting for an integration/plugin/flow to complete),
    /// without taking on a third-party retry library.
    /// </summary>
    public static class RetryExtensions
    {
        /// <summary>
        /// Repeatedly invokes <paramref name="action"/> until it completes without
        /// throwing, or <paramref name="retryCount"/> attempts have been made, at
        /// which point the final exception is rethrown.
        /// </summary>
        /// <param name="action">The action to attempt.</param>
        /// <param name="logger">A logger used to record each retry attempt.</param>
        /// <param name="retryCount">The maximum number of attempts before giving up.</param>
        /// <param name="delay">The fixed delay between attempts. Defaults to 10 seconds.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task RetryUntilSucceedsAsync(Func<Task> action, ILogger logger, int retryCount = 6, TimeSpan? delay = null)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (retryCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "Retry count must be greater than zero.");
            }

            if (delay.HasValue && delay.Value < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(delay), delay, "Delay must not be negative.");
            }

            var waitBetweenAttempts = delay ?? TimeSpan.FromSeconds(10);

            for (var attempt = 1; ; attempt++)
            {
                try
                {
                    await action();
                    return;
                }
                catch (Exception ex) when (ex is not OperationCanceledException && attempt < retryCount)
                {
                    logger.LogWarning(ex, "Retry attempt {Attempt}", attempt);
                    await Task.Delay(waitBetweenAttempts);
                }
            }
        }
    }
}
