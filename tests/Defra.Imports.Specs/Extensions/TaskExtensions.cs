namespace Defra.Imports.Specs.Extensions
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Extensions to the <see cref="Task{T}"/> class.
    /// </summary>
    internal static class TaskExtensions
    {
        /// <summary>
        /// Executes a task with a cancellation token, throwing an <see cref="OperationCanceledException"/> if the token is cancelled before the task completes. This is necessary because <see cref="Task{T}"/> does not natively support cancellation tokens, and without this, a cancelled navigation task would simply hang until it eventually timed out, rather than allowing for more immediate cancellation and cleanup. By using this extension method, we can ensure that our navigation tasks are responsive to cancellation requests, improving the reliability and responsiveness of our tests.
        /// </summary>
        /// <typeparam name="T">The return type.</typeparam>
        /// <param name="task">The task.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task that completes with the result of the original task, or throws an <see cref="OperationCanceledException"/> if the cancellation token is cancelled before the task completes.</returns>
        internal static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            using (cancellationToken.Register(s => ((TaskCompletionSource<bool>)s).TrySetResult(true), tcs))
            {
                if (task != await Task.WhenAny(task, tcs.Task).ConfigureAwait(false))
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            return await task.ConfigureAwait(false);
        }
    }
}