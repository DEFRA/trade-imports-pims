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
        /// Repeatedly evaluates <paramref name="condition"/> until it returns true or
        /// <paramref name="retryCount"/> attempts have been made.
        /// </summary>
        /// <remarks>
        /// Unlike <see cref="RetryUntilSucceedsAsync(Func{Task}, ILogger, int, TimeSpan?)"/> this
        /// reports an unmet condition by returning false rather than throwing, so that a caller can
        /// treat the absence of something as a reportable outcome instead of a failure. It exits as
        /// soon as the condition is met rather than always waiting for the full delay.
        /// </remarks>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="betweenAttempts">An optional action invoked between attempts, for example to settle the UI.</param>
        /// <param name="logger">An optional logger used to record each attempt.</param>
        /// <param name="retryCount">The maximum number of attempts before giving up.</param>
        /// <param name="delay">The fixed delay between attempts. Defaults to 2 seconds.</param>
        /// <returns>A <see cref="Task"/> that resolves to true when the condition was met.</returns>
        public static async Task<bool> WaitUntilAsync(
            Func<Task<bool>> condition,
            Func<Task> betweenAttempts = null,
            ILogger logger = null,
            int retryCount = 5,
            TimeSpan? delay = null)
        {
            if (condition is null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (retryCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(retryCount), retryCount, "Retry count must be greater than zero.");
            }

            if (delay.HasValue && delay.Value < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(delay), delay, "Delay must not be negative.");
            }

            var waitBetweenAttempts = delay ?? TimeSpan.FromSeconds(2);

            for (var attempt = 1; attempt <= retryCount; attempt++)
            {
                if (await condition())
                {
                    return true;
                }

                if (attempt == retryCount)
                {
                    break;
                }

                logger?.LogDebug("Condition not yet met on attempt {Attempt} of {RetryCount}.", attempt, retryCount);

                await Task.Delay(waitBetweenAttempts);

                if (betweenAttempts != null)
                {
                    await betweenAttempts();
                }
            }

            return false;
        }

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
