namespace Defra.Imports.Specs.Hooks
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Playwright;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Reqnroll;

    /// <summary>
    /// Hooks for tracing.
    /// </summary>
    [Binding]
    public class TracingHooks
    {
        private readonly ScenarioContext ctx;
        private readonly TestContext testContext;
        private readonly IReqnrollOutputHelper outputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TracingHooks"/> class.
        /// </summary>
        /// <param name="ctx">The scenario context.</param>
        /// <param name="testContext">The test context.</param>
        /// <param name="outputHelper">The output helper.</param>
        public TracingHooks(ScenarioContext ctx, TestContext testContext, IReqnrollOutputHelper outputHelper)
        {
            this.ctx = ctx;
            this.testContext = testContext;
            this.outputHelper = outputHelper;
        }

        /// <summary>
        /// Tears down tracing for the scenario.
        /// </summary>
        /// <remarks>
        /// Stops the tracing and saves the trace to a file if the test failed.
        /// </remarks>
        /// <param name="browserCtx">The browser context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [AfterScenario]
        public async Task TearDownTracingAsync(IBrowserContext browserCtx)
        {
            var shortScenarioName = GetShortSafeFileName(this.ctx.ScenarioInfo.Title);
            var outputPath = this.ctx.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError ?
                Path.Combine(
                    this.testContext.ResultsDirectory,
                    "playwright-traces",
                    $"{shortScenarioName}.zip")
                :
                null;

            try
            {
                await browserCtx.Tracing.StopAsync(new TracingStopOptions { Path = outputPath });
            }
            catch (Exception ex)
            {
                this.outputHelper.WriteLine($"An error occurred while stopping Playwright tracing: {ex.Message}.");
                return;
            }

            if (outputPath is null)
            {
                return;
            }

            this.testContext.AddResultFile(outputPath);
        }

        private static string GetShortSafeFileName(string title)
        {
            var safeTitle = string.Join(string.Empty, (title ?? string.Empty).Split(Path.GetInvalidFileNameChars()));
            if (safeTitle.Length > 48)
            {
                safeTitle = safeTitle.Substring(0, 48);
            }

            var hash = Math.Abs((title ?? string.Empty).Aggregate(17, (current, c) => (current * 31) + c));
            return $"{safeTitle}-{hash:x}";
        }
    }
}