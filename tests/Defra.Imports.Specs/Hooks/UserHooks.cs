namespace Defra.Imports.Specs.Hooks
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Defra.Imports.Specs.Services;
    using Reqnroll;

    /// <summary>
    /// Hooks relating to the user pool.
    /// </summary>
    [Binding]
    public class UserHooks
    {
        private readonly ScenarioUserClient scenarioUserClient;
        private readonly IReqnrollOutputHelper outputHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserHooks"/> class.
        /// </summary>
        /// <param name="scenarioUserClient">The scenario's user pool client.</param>
        /// <param name="outputHelper">The output helper.</param>
        public UserHooks(ScenarioUserClient scenarioUserClient, IReqnrollOutputHelper outputHelper)
        {
            this.scenarioUserClient = scenarioUserClient;
            this.outputHelper = outputHelper;
        }

        /// <summary>
        /// Verifies that every statically assigned persona user (one explicitly listed under a persona's <see cref="Defra.Imports.Scenarios.Config.PersonaConfiguration.Users"/> in <c>environment.json</c>) actually has the full Dataverse security configuration (business unit, security roles, team memberships and column security profiles) described by their assigned persona(s), so misconfigured environments are caught up front rather than surfacing as confusing scenario failures.
        /// </summary>
        /// <param name="userPoolService">The shared user pool service.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [BeforeTestRun(Order = -19990)]
        public static async Task VerifyStaticUserPersonaConfiguration(UserPoolService userPoolService)
        {
            try
            {
                await userPoolService.VerifyStaticUserConfigurationAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                HookDiagnostics.LogBeforeTestRunFailure(nameof(VerifyStaticUserPersonaConfiguration), ex);
                throw;
            }
        }

        /// <summary>
        /// Removes the user from the users in use list, returning it to the pool.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [AfterScenario(Order = -100000)]
        public async Task RemoveUserFromUsersInUse()
        {
            try
            {
                await this.scenarioUserClient.ReleaseAsync();
            }
            catch (Exception ex)
            {
                this.outputHelper.WriteLine($"An error occurred while releasing the user: {ex.Message}.");
            }
            finally
            {
                this.scenarioUserClient.Dispose();
            }
        }
    }
}