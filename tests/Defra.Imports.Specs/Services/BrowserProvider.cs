namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Playwright;
    using Reqnroll;

    /// <summary>
    /// Provides a single <see cref="IBrowser"/> per test worker (test thread), shared by every scenario that worker runs.
    /// </summary>
    /// <remarks>
    /// Instances are resolved from the Reqnroll test thread container, so there is one per worker. All instances are
    /// tracked so they can be disposed once the whole test run has finished.
    /// </remarks>
    public sealed class BrowserProvider : IAsyncDisposable
    {
        private static readonly ConcurrentBag<BrowserProvider> Instances = new ConcurrentBag<BrowserProvider>();

        private readonly SemaphoreSlim gate = new SemaphoreSlim(1, 1);
        private IBrowser browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserProvider"/> class.
        /// </summary>
        public BrowserProvider()
        {
            Instances.Add(this);
        }

        /// <summary>
        /// Gets the <see cref="BrowserProvider"/> for the worker running the current scenario.
        /// </summary>
        /// <param name="testThreadContext">The test thread context.</param>
        /// <returns>The worker's browser provider.</returns>
        public static BrowserProvider ForWorker(TestThreadContext testThreadContext)
        {
            return testThreadContext.TestThreadContainer.Resolve<BrowserProvider>();
        }

        /// <summary>
        /// Disposes the browsers of every worker.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task DisposeAllAsync()
        {
            while (Instances.TryTake(out var instance))
            {
                await instance.DisposeAsync();
            }
        }

        /// <summary>
        /// Gets the worker's browser, launching it if it has not been launched or has disconnected.
        /// </summary>
        /// <param name="playwright">The Playwright instance.</param>
        /// <returns>The browser.</returns>
        public async Task<IBrowser> GetBrowserAsync(IPlaywright playwright)
        {
            await this.gate.WaitAsync();
            try
            {
                if (this.browser == null || !this.browser.IsConnected)
                {
                    this.browser = await playwright.Chromium.LaunchAsync(
                        new BrowserTypeLaunchOptions
                        {
                            Headless = bool.TryParse(Environment.GetEnvironmentVariable("HEADLESS"), out var headless) && headless,
                            Channel = "msedge",
                        });
                }

                return this.browser;
            }
            finally
            {
                this.gate.Release();
            }
        }

        /// <inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            var current = this.browser;
            this.browser = null;

            if (current != null)
            {
                try
                {
                    await current.DisposeAsync();
                }
                catch (PlaywrightException)
                {
                    // The browser has already gone away; nothing left to release.
                }
            }
        }
    }
}
