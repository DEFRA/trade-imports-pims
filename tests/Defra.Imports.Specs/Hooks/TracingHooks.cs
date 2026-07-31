namespace Defra.Imports.Specs.Hooks
{
    using System;
    using System.IO;
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
            var outputPath = this.ctx.ScenarioExecutionStatus == ScenarioExecutionStatus.TestError ?
                Path.Combine(
                    this.testContext.ResultsDirectory,
                    "playwright-traces",
                    $"{string.Join(string.Empty, this.ctx.ScenarioInfo.Title.Split(Path.GetInvalidFileNameChars()))}.zip")
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
    }
}