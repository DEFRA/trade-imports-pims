namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Extensions;
    using FluentAssertions;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;

    /// <summary>
    /// Stesp relating to a business process flow.
    /// </summary>
    [Binding]
    public class BusinessProcessFlowSteps
    {
        private readonly ScenarioContext scenarioContext;
        private readonly PowerPlaywrightContext powerPlaywrightCtx;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessProcessFlowSteps"/> class.
        /// </summary>
        /// <param name="scenarioContext">The scenario context.</param>
        /// <param name="powerPlaywrightCtx">The playwright context.</param>
        public BusinessProcessFlowSteps(ScenarioContext scenarioContext, PowerPlaywrightContext powerPlaywrightCtx)
        {
            this.scenarioContext = scenarioContext;
            this.powerPlaywrightCtx = powerPlaywrightCtx;
        }

        private IEntityRecordPage RecordPage
        {
            get
            {
                this.powerPlaywrightCtx.ValidatePage<IEntityRecordPage>();

                return (IEntityRecordPage)this.powerPlaywrightCtx.ActivePage;
            }
        }

        /// <summary>
        /// Ensures the specified business process flow is active for the current record.
        /// </summary>
        /// <param name="processName">The name of the business process flow.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("the {string} business process flow is active")]
        public async Task GivenTheBusinessProcessFlowIsActive(string processName)
        {
            this.scenarioContext.AddOrUpdate(ScenarioContextKeys.ActiveBusinessProcessFlow, processName);
        }

        /// <summary>
        /// Asserts fields within the active process stage.
        /// </summary>
        /// <param name="expectedFields">The expected fields.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the following fields in the current business process stage")]
        public async Task ThenISeeTheFollowingFieldsInTheCurrentBusinessProcessStage(DataTable expectedFields)
        {
            var checkEditable = expectedFields.Header.Contains("Editable");
            var exceptions = new List<Exception>();

            await this.RecordPage.Form.BusinessProcess
                .ExecuteStageActionAsync(async (actualFields) =>
                {
                    var actualFieldLabels = (await Task.WhenAll(actualFields
                        .Select(async f => await f.GetLabelAsync())))
                        .Where(label => !string.IsNullOrEmpty(label));

                    actualFieldLabels.Should().BeEquivalentTo(expectedFields.Rows.Select(r => r[0]));

                    if (checkEditable)
                    {
                        foreach (var (row, index) in expectedFields.Rows.Select((r, i) => (r, i)))
                        {
                            var expectedDisabledState = !bool.Parse(row["Editable"]);

                            var field = actualFields.ElementAt(index);

                            var actualDisabledState = await field.IsDisabledAsync();

                            if (expectedDisabledState != actualDisabledState)
                            {
                                exceptions.Add(new Exception($"Expected field '{row[0]}' to be {(expectedDisabledState ? "disabled" : "enabled")}, but it was {(actualDisabledState ? "disabled" : "enabled")}."));
                            }
                        }
                    }
                });

            if (exceptions.Any())
            {
                throw new AggregateException("One or more field assertions failed.", exceptions);
            }
        }

        /// <summary>
        /// Asserts the stages in the active business process flow.
        /// </summary>
        /// <param name="expectedStages">The expected stages.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the following stages in the business process flow")]
        public async Task ThenISeeTheFollowingStagesInTheBusinessProcessFlow(DataTable expectedStages)
        {
            var actualStages = await this.RecordPage.Form.BusinessProcess.GetStagesAsync();

            actualStages.Should().BeEquivalentTo(expectedStages.Rows.Select(r => r[0]));
        }
    }
}