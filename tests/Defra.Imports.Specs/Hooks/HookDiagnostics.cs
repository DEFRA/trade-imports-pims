namespace Defra.Imports.Specs.Hooks
{
    using System;
    using Reqnroll;

    /// <summary>
    /// Shared diagnostics for <c>[BeforeTestRun]</c> hooks.
    /// </summary>
    internal static class HookDiagnostics
    {
        /// <summary>
        /// Logs the full detail of a <c>[BeforeTestRun]</c> hook failure.
        /// </summary>
        /// <remarks>
        /// A <c>[BeforeTestRun]</c> hook failure is reported to MSTest as a generic <see cref="AggregateException"/>
        /// by the Reqnroll-generated assembly hooks, which replaces the real exception's message with a fixed,
        /// unhelpful string. Writing the exception out directly to the console here ensures the real cause is still
        /// visible, especially on CI where the failure cannot be reproduced or debugged interactively.
        /// <see cref="IReqnrollOutputHelper"/> cannot be used here: <c>[BeforeTestRun]</c>/<c>[AfterTestRun]</c> hooks
        /// run once for the whole test run, before any test thread context exists, and are resolved from a
        /// container scope that does not have it registered (it is only available to feature/scenario-scoped hooks).
        /// Only pass non-secret <paramref name="context"/> values (e.g. a URL or client ID) - never credentials.
        /// </remarks>
        /// <param name="hookName">The name of the failing hook, for identification.</param>
        /// <param name="ex">The exception that was thrown.</param>
        /// <param name="context">Optional, non-secret context to help identify the cause (e.g. a URL or client ID).</param>
        public static void LogBeforeTestRunFailure(string hookName, Exception ex, string context = null)
        {
            var message = string.IsNullOrEmpty(context)
                ? $"[BeforeTestRun] {hookName} failed: {ex}"
                : $"[BeforeTestRun] {hookName} failed ({context}): {ex}";

            Console.WriteLine(message);
            Console.Error.WriteLine(message);
        }
    }
}
