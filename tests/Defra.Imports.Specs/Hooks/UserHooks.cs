namespace Defra.Imports.Specs.Hooks
{
    using System;
    using System.Threading.Tasks;
    using Defra.Imports.Specs.Services;
    using Reqnroll;

    /// <summary>
    /// After scenario hooks.
    /// </summary>
    [Binding]
    public class UserHooks
    {
        private readonly UserPoolClient userPoolClient;
        private readonly IReqnrollOutputHelper outputHelper;
        private readonly ScenarioContext scenarioContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserHooks"/> class.
        /// </summary>
        /// <param name="userPoolClient">The user pool.</param>
        /// <param name="outputHelper">The output helper.</param>
        /// <param name="scenarioContext">The scenario context.</param>
        public UserHooks(UserPoolClient userPoolClient, IReqnrollOutputHelper outputHelper, ScenarioContext scenarioContext)
        {
            this.userPoolClient = userPoolClient;
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
            this.scenarioContext.TryGetValue(UserPoolClient.LeaseRevokedErrorKey, out LeaseRevokedException leaseError);

            try
            {
                await this.userPoolClient.ReleaseAsync();
            }
            catch (Exception ex)
            {
                this.outputHelper.WriteLine($"An error occurred while releasing the user: {ex.Message}.");
            }

            if (leaseError != null)
            {
                this.outputHelper.WriteLine(leaseError.Message);
            }
        }
    }
}