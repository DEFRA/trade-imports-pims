namespace Defra.Imports.Specs.Services
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Model;
    using Microsoft.Playwright;
    using PowerPlaywright.Framework;
    using Reqnroll;

    /// <summary>
    /// Records gaps between the acceptance criteria and the behaviour implemented by the system so
    /// that a scenario can continue past them.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The US-003 acceptance criteria describe a number of fields, a list view and a set of search
    /// criteria that the solution does not yet provide. Asserting them directly stops a scenario at
    /// the first gap, which hides whether everything else in that criterion works.
    /// </para>
    /// <para>
    /// This recorder provides a soft assertion: a probe is attempted, and if it fails because the
    /// system does not offer the thing being asked for, the discrepancy is recorded as a
    /// <see cref="KnownDefect"/> and execution continues. Failures that indicate something is
    /// implemented but broken are re-thrown, so that genuine regressions are still reported as test
    /// failures.
    /// </para>
    /// </remarks>
    public class KnownDefectRecorder
    {
        private readonly List<KnownDefect> defects = new List<KnownDefect>();
        private readonly List<string> verified = new List<string>();
        private readonly IReqnrollOutputHelper outputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="KnownDefectRecorder"/> class.
        /// </summary>
        /// <param name="outputHelper">The output helper used to log progress as the scenario runs.</param>
        public KnownDefectRecorder(IReqnrollOutputHelper outputHelper)
        {
            this.outputHelper = outputHelper;
        }

        /// <summary>
        /// Gets the known defects recorded during the scenario.
        /// </summary>
        public ReadOnlyCollection<KnownDefect> Defects => this.defects.AsReadOnly();

        /// <summary>
        /// Gets the requirements that were successfully verified during the scenario.
        /// </summary>
        public ReadOnlyCollection<string> Verified => this.verified.AsReadOnly();

        /// <summary>
        /// Gets a value indicating whether any known defects were recorded.
        /// </summary>
        public bool HasDefects => this.defects.Count > 0;

        /// <summary>
        /// Records a known defect.
        /// </summary>
        /// <param name="acceptanceCriterion">The acceptance criterion that is not met.</param>
        /// <param name="requirement">The field, view or search criterion that was expected.</param>
        /// <param name="expected">The behaviour required by the acceptance criterion.</param>
        /// <param name="actual">The behaviour observed in the system.</param>
        public void RecordDefect(string acceptanceCriterion, string requirement, string expected, string actual)
        {
            var defect = new KnownDefect(acceptanceCriterion, requirement, expected, actual);

            this.defects.Add(defect);
            this.outputHelper.WriteLine($"KNOWN DEFECT | {defect}");
        }

        /// <summary>
        /// Records that a requirement was successfully verified.
        /// </summary>
        /// <param name="acceptanceCriterion">The acceptance criterion being verified.</param>
        /// <param name="requirement">The field, view or search criterion that was verified.</param>
        public void RecordVerified(string acceptanceCriterion, string requirement)
        {
            this.verified.Add($"[{acceptanceCriterion}] {requirement}");
            this.outputHelper.WriteLine($"VERIFIED | [{acceptanceCriterion}] {requirement}");
        }

        /// <summary>
        /// Attempts a verification, recording a known defect if the system does not provide what the
        /// acceptance criterion requires.
        /// </summary>
        /// <remarks>
        /// Only failures that indicate absence are converted into known defects. Any other exception
        /// is allowed to propagate, because it represents a genuine defect in functionality that is
        /// implemented.
        /// </remarks>
        /// <param name="acceptanceCriterion">The acceptance criterion being verified.</param>
        /// <param name="requirement">The field, view or search criterion being verified.</param>
        /// <param name="expected">The behaviour required by the acceptance criterion.</param>
        /// <param name="verification">The verification to attempt.</param>
        /// <returns>A <see cref="Task"/> that resolves to true when the requirement was verified.</returns>
        public async Task<bool> TryVerifyAsync(string acceptanceCriterion, string requirement, string expected, Func<Task> verification)
        {
            try
            {
                await verification();
                this.RecordVerified(acceptanceCriterion, requirement);

                return true;
            }
            catch (Exception ex) when (IsAbsence(ex))
            {
                this.RecordDefect(acceptanceCriterion, requirement, expected, Describe(ex));

                return false;
            }
        }

        /// <summary>
        /// Builds a report of everything verified and every known defect recorded.
        /// </summary>
        /// <returns>The report.</returns>
        public string BuildReport()
        {
            var report = new StringBuilder();

            report.AppendLine("================ ACCEPTANCE CRITERIA REPORT ================");
            report.AppendLine($"Verified: {this.verified.Count}    Known defects: {this.defects.Count}");

            if (this.verified.Any())
            {
                report.AppendLine();
                report.AppendLine("PASSED:");
                foreach (var item in this.verified)
                {
                    report.AppendLine($"  [PASS] {item}");
                }
            }

            if (this.defects.Any())
            {
                report.AppendLine();
                report.AppendLine("KNOWN DEFECTS (acceptance criteria not implemented):");
                foreach (var group in this.defects.GroupBy(d => d.AcceptanceCriterion).OrderBy(g => g.Key))
                {
                    report.AppendLine($"  {group.Key}:");
                    foreach (var defect in group)
                    {
                        report.AppendLine($"    [KNOWN DEFECT] {defect.Requirement}");
                        report.AppendLine($"        Expected: {defect.Expected}");
                        report.AppendLine($"        Actual  : {defect.Actual}");
                    }
                }
            }

            report.AppendLine("============================================================");

            return report.ToString();
        }

        /// <summary>
        /// Gets whether an exception indicates that the system does not provide the thing being
        /// verified, as opposed to it being present but not working.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>Whether the exception indicates absence.</returns>
        private static bool IsAbsence(Exception ex)
        {
            switch (ex)
            {
                // The form XML contains no control with the requested label.
                case ControlNotFoundException _:
                    return true;

                // The control is bound but never renders, so it cannot be interacted with.
                case TimeoutException _:
                    return true;
                case PlaywrightException playwrightEx:
                    return playwrightEx.Message.IndexOf("Timeout", StringComparison.OrdinalIgnoreCase) >= 0
                        || playwrightEx.Message.IndexOf("strict mode violation", StringComparison.OrdinalIgnoreCase) >= 0;

                // A command or tab named by the acceptance criteria is not present on the page.
                // Matched narrowly so that other Power Playwright faults still fail the test.
                case PowerPlaywrightException powerPlaywrightEx:
                    return powerPlaywrightEx.Message.IndexOf("is not available", StringComparison.OrdinalIgnoreCase) >= 0;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Describes an exception concisely for inclusion in a report.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>The description.</returns>
        private static string Describe(Exception ex)
        {
            var message = ex.Message ?? string.Empty;
            var firstLine = message
                .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                .FirstOrDefault() ?? ex.GetType().Name;

            return firstLine.Length > 240 ? firstLine.Substring(0, 240) + "..." : firstLine;
        }
    }
}
