namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Extensions;
    using Microsoft.Playwright;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Microsoft.Xrm.Sdk;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;

    /// <summary>
    /// Provides safe navigation using <see cref="PowerPlaywright.Framework.Controls.Platform.IClientApi"/>.
    /// </summary>
    public class RecordNavigatorService
    {
        private readonly IReqnrollOutputHelper outputHelper;
        private readonly TestContext testContext;
        private readonly PowerPlaywrightContext powerPlaywrightCtx;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecordNavigatorService"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper.</param>
        /// <param name="testContext">The test context.</param>
        /// <param name="powerPlaywrightCtx">The Power Playwright context.</param>
        public RecordNavigatorService(IReqnrollOutputHelper outputHelper, TestContext testContext, PowerPlaywrightContext powerPlaywrightCtx)
        {
            this.outputHelper = outputHelper;
            this.testContext = testContext;
            this.powerPlaywrightCtx = powerPlaywrightCtx;
        }

        /// <summary>
        /// Navigates to a record with a timeout to prevent hanging tests in case of navigation failures. If navigation fails, a screenshot will be taken and attached to the test results for debugging purposes.
        /// </summary>
        /// <param name="record">The record to navigate to.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task<IModelDrivenAppPage> NavigateToRecordAsync(EntityReference record)
        {
            this.outputHelper.WriteLine($"Navigating to {record.LogicalName} {record.Id} via client API.");

            using (var timeoutCts = CancellationTokenSource.CreateLinkedTokenSource(this.testContext.CancellationToken))
            {
                timeoutCts.CancelAfter(TimeSpan.FromSeconds(120));

                try
                {
                    var appPage = await this.powerPlaywrightCtx.ActivePage.ClientApi
                        .NavigateToRecordAsync(record.LogicalName, record.Id)
                        .WithCancellation(timeoutCts.Token)
                        .ConfigureAwait(false);

                    this.outputHelper.WriteLine($"Finished navigating.");
                    return appPage;
                }
                catch (OperationCanceledException) when (!this.testContext.CancellationToken.IsCancellationRequested)
                {
                    // Timed out, not cancelled by the test context.
                    this.outputHelper.WriteLine($"Failed navigating.");

                    var screenshotPath = $"/playwright-screenshots/{record.Id}-navigation-timeout.jpg";
                    await this.powerPlaywrightCtx.ActivePage.Page
                        .ScreenshotAsync(new PageScreenshotOptions { Path = screenshotPath })
                        .ConfigureAwait(false);
                    this.testContext.AddResultFile(screenshotPath);

                    throw new Exception($"Failed to navigate to {record.LogicalName} {record.Id}. Check the attached screenshot for more details.");
                }
            }
        }
    }
}
