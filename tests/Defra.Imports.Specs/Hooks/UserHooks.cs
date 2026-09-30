namespace Defra.Imports.Specs.Hooks
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.Scenarios;
    using Reqnroll;

    /// <summary>
    /// Hooks relating to the user pool.
    /// </summary>
    [Binding]
    public class UserHooks
    {
        private readonly ScenarioUserClient scenarioUserClient;
        private readonly IReqnrollOutputHelper outputHelper;
        private readonly ScenarioContext scenarioContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserHooks"/> class.
        /// </summary>
        /// <param name="scenarioUserClient">The scenario's user pool client.</param>
        /// <param name="outputHelper">The output helper.</param>
        /// <param name="scenarioContext">The scenario context.</param>
        public UserHooks(ScenarioUserClient scenarioUserClient, IReqnrollOutputHelper outputHelper, ScenarioContext scenarioContext)
        {
            this.scenarioUserClient = scenarioUserClient;
            this.outputHelper = outputHelper;
            this.scenarioContext = scenarioContext;
        }

        /// <summary>
        /// Removes the user from the users in use list and fails the scenario if the lease was
        /// automatically revoked due to the lease timeout being exceeded.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [AfterScenario(Order = -100000)]
        public async Task RemoveUserFromUsersInUse()
        {
            // Check for a stored lease revocation error before releasing, so the scenario is
            // failed with the original revocation message rather than a generic cleanup error.
            this.scenarioContext.TryGetValue(ScenarioContextKeys.LeaseRevokedErrorKey, out LeaseRevokedException leaseError);

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

            if (leaseError != null)
            {
                this.outputHelper.WriteLine(leaseError.Message);
            }
        }
    }
}