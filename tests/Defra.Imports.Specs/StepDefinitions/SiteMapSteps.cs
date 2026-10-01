namespace Defra.Imports.Specs.StepDefinitions
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Specs.Extensions;
    using Defra.Imports.Specs.Services;
    using FluentAssertions;
    using Microsoft.Xrm.Sdk;
    using PowerPlaywright.Framework.Pages;
    using Reqnroll;

    /// <summary>
    /// Steps relating to the site map.
    /// </summary>
    [Binding]
    public class SiteMapSteps
    {
        private const string ContextKeyActiveArea = nameof(ContextKeyActiveArea);

        private readonly ServiceClientFactory clientFactory;
        private readonly EntityMetadataService entityMetadataService;
        private readonly PowerPlaywrightContext powerPlaywrightCtx;
        private readonly ScenarioContext ctx;
        private readonly TestDataService testDataService;
        private readonly RecordNavigatorService recordNavigator;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapSteps"/> class.
        /// </summary>
        /// <param name="clientFactory">The client factory.</param>
        /// <param name="entityMetadataSvc">The entity metadata service.</param>
        /// <param name="powerPlaywrightCtx">The PowerPlaywright context.</param>
        /// <param name="ctx">The scenario context.</param>
        /// <param name="testDataService">The test data service.</param>
        /// <param name="recordNavigator">The record navigator service.</param>
        public SiteMapSteps(ServiceClientFactory clientFactory, EntityMetadataService entityMetadataSvc, PowerPlaywrightContext powerPlaywrightCtx, ScenarioContext ctx, TestDataService testDataService, RecordNavigatorService recordNavigator)
        {
            this.clientFactory = clientFactory;
            this.entityMetadataService = entityMetadataSvc;
            this.powerPlaywrightCtx = powerPlaywrightCtx;
            this.ctx = ctx;
            this.testDataService = testDataService;
            this.recordNavigator = recordNavigator;
        }

        /// <summary>
        /// Navigates to a record of the specified table.
        /// </summary>
        /// <param name="tableDisplayName">The display name of the table.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("I have opened a {string}")]
        [Given("I have opened an {string}")]
        public async Task GivenIHaveOpenedA(string tableDisplayName)
        {
            this.powerPlaywrightCtx.Validate();

            var logicalName = this.entityMetadataService.GetTableLogicalName(tableDisplayName);

            Guid id;
            using (var client = this.clientFactory.GetClient(this.powerPlaywrightCtx.ActiveUserId))
            {
                id = await client.GetAnyRecordIdAsync(logicalName);
            }

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(new EntityReference(logicalName, id));
        }

        /// <summary>
        /// Navigates to a record by its alias in the test data service.
        /// </summary>
        /// <param name="alias">The alias of the record in the test data service.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("I have opened {string}")]
        public async Task GivenIHaveOpened(string alias)
        {
            this.powerPlaywrightCtx.Validate();

            var reference = this.testDataService.GetRecordByAlias(alias);

            this.powerPlaywrightCtx.ActivePage = await this.recordNavigator.NavigateToRecordAsync(reference);
        }

        /// <summary>
        /// Navigates to a subarea in the sitemap.
        /// </summary>
        /// <param name="area">The area to navigate to.</param>
        /// <param name="group">The group within the area to navigate to.</param>
        /// <param name="page">The page within the group to navigate to.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Given("I have navigated to {string} -> {string} -> {string}")]
        [When("I navigate to {string} -> {string} -> {string}")]
        public async Task WhenIOpenTheSubAreaUnderTheArea(string area, string group, string page)
        {
            this.powerPlaywrightCtx.Validate();

            this.powerPlaywrightCtx.ActivePage = await this.powerPlaywrightCtx.ActivePage.SiteMap
                .OpenPageAsync<IModelDrivenAppPage>(area, group, page);

            if (page == "Contacts")
            {
                // Workaround for contacts page not loading correctly on first navigation in Playwright test runs.
                await this.powerPlaywrightCtx.ActivePage.Page.ReloadAndWaitForAppIdleAsync();
            }
            else if (page == "Work Orders")
            {
                // Workaround for work orders page not loading correctly when the MRU grid state is cached in local storage.
                await this.powerPlaywrightCtx.ActivePage.Page.EvaluateAsync("localStorage.removeItem('MRU_Grid_ControlIdmsdyn_workorder')");
                await this.powerPlaywrightCtx.ActivePage.Page.ReloadAndWaitForAppIdleAsync();
            }

            this.ctx.AddOrUpdate(ContextKeyActiveArea, area);
        }

        /// <summary>
        /// Navigates to an area in the sitemap.
        /// </summary>
        /// <param name="area">The area to navigate to.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [When("I navigate to the {string} area")]
        public async Task WhenINavigateToTheArea(string area)
        {
            this.powerPlaywrightCtx.Validate();

            // TODO: Add functionality to open an area (without specifying a group or page) into Power Playwright
            var groups = await this.powerPlaywrightCtx.ActivePage.SiteMap.GetGroupsAsync(area);
            var pages = await this.powerPlaywrightCtx.ActivePage.SiteMap.GetPagesAsync(area, groups.First());

            this.powerPlaywrightCtx.ActivePage = await this.powerPlaywrightCtx.ActivePage.SiteMap
                .OpenPageAsync<IModelDrivenAppPage>(area, groups.First(), pages.First());

            this.ctx.AddOrUpdate(ContextKeyActiveArea, area);
        }

        /// <summary>
        /// Asserts the visibility of pages.
        /// </summary>
        /// <param name="pages">The pages (a table with the headers 'Page' and, optionally, 'Group'.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("I see the following pages")]
        public async Task ISeeTheFollowingPages(DataTable pages)
        {
            if (!this.ctx.TryGetValue<string>(ContextKeyActiveArea, out var activeArea))
            {
                throw new InvalidOperationException("No active area recorded in the context.");
            }

            if (pages.Header.Contains("Group"))
            {
                var expectedGroups = pages.Rows.GroupBy(r => r["Group"], r => r["Page"]);

                foreach (var expectedGroup in expectedGroups)
                {
                    var actualGroupPages = await this.powerPlaywrightCtx.ActivePage.SiteMap.GetPagesAsync(activeArea, expectedGroup.Key);

                    actualGroupPages.Should().Contain(expectedGroup.ToArray());
                }
            }
            else
            {
                var expectedPages = pages.Rows.Select(r => r["Page"]);
                var actualPages = await this.powerPlaywrightCtx.ActivePage.SiteMap.GetPagesAsync(activeArea);

                actualPages.Should().Contain(expectedPages.ToArray());
            }
        }

        /// <summary>
        /// Asserts that a subarea of the sitemap depending on requirement visible or not visible.
        /// </summary>
        /// <param name="should">Whether they should be visible.</param>
        /// <param name="area">The area.</param>
        /// <param name="pages">The pages.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Then("^I (can|cannot) see the following pages on the navigation panel in the {string} area")]
        public async Task ICanSeeTheFollowingPages(bool should, string area, DataTable pages)
        {
            var expectedPages = pages.Rows.Select(row => row.Values.Last()).ToList();

            var actualPages = await this.powerPlaywrightCtx.ActivePage.SiteMap.GetPagesAsync(area);

            if (should)
            {
                actualPages.Should().Contain(expectedPages);
            }
            else
            {
                actualPages.Should().NotContain(expectedPages);
            }
        }
    }
}
