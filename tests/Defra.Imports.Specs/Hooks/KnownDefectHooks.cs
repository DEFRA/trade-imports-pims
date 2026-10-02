namespace Defra.Imports.Specs.Hooks
{
    using System.Linq;
    using Defra.Imports.Specs.Services;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Reqnroll;

    /// <summary>
    /// Hooks for reporting known defects recorded during a scenario.
    /// </summary>
    [Binding]
    public class KnownDefectHooks
    {
        private readonly KnownDefectRecorder defectRecorder;
        private readonly ScenarioContext ctx;
        private readonly IReqnrollOutputHelper outputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownDefectHooks"/> class.
        /// </summary>
        /// <param name="defectRecorder">The known defect recorder.</param>
        /// <param name="ctx">The scenario context.</param>
        /// <param name="outputHelper">The output helper.</param>
        public KnownDefectHooks(KnownDefectRecorder defectRecorder, ScenarioContext ctx, IReqnrollOutputHelper outputHelper)
        {
            this.defectRecorder = defectRecorder;
            this.ctx = ctx;
            this.outputHelper = outputHelper;
        }

        /// <summary>
        /// Reports the known defects recorded during the scenario.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The report is always written to the test log. Where the scenario recorded known defects
        /// but did not otherwise fail, the scenario is marked inconclusive. This keeps the three
        /// outcomes distinguishable in the test results:
        /// </para>
        /// <list type="bullet">
        /// <item><description>Passed - the acceptance criterion is fully implemented.</description></item>
        /// <item><description>Not executed (inconclusive) - the implemented behaviour is correct, but part of the acceptance criterion is not yet implemented.</description></item>
        /// <item><description>Failed - implemented behaviour is broken, which is a genuine defect.</description></item>
        /// </list>
        /// </remarks>
        [AfterScenario(Order = 10000)]
        public void ReportKnownDefects()
        {
            if (!this.defectRecorder.HasDefects && !this.defectRecorder.Verified.Any())
            {
                return;
            }

            this.outputHelper.WriteLine(this.defectRecorder.BuildReport());

            if (this.defectRecorder.HasDefects && this.ctx.ScenarioExecutionStatus != ScenarioExecutionStatus.TestError)
            {
                Assert.Inconclusive(
                    $"{this.defectRecorder.Defects.Count} known defect(s) recorded. The behaviour that is implemented behaved as expected. See the acceptance criteria report in the test output for details.");
            }
        }
    }
}
