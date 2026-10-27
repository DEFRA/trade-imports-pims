namespace Defra.Imports.UnitTests.Support
{
    using System;
    using System.Activities;
    using System.Collections.Generic;
    using Xunit;

    // Base class for tests of CodeActivity entry points. It supplies a simulated workflow host and the
    // assertions most activity tests need, so derived tests describe only the activity's contract.
    public abstract class WorkflowActivityTestBase<TActivity>
        where TActivity : CodeActivity, new()
    {
        protected WorkflowRuntime Runtime { get; } = new WorkflowRuntime();

        protected TActivity Activity { get; } = new TActivity();

        protected IDictionary<string, object> Invoke(string inputName, object input)
        {
            return this.Runtime.Invoke(this.Activity, inputName, input);
        }

        // WorkflowInvoker wraps failures, so the expected exception is searched for among the inner exceptions.
        // The strict service mock proves nothing was read or written.
        protected void AssertInvokeFailsWithoutCallingService<TInner>(string inputName, object input)
            where TInner : Exception
        {
            var exception = Assert.ThrowsAny<Exception>(() => this.Invoke(inputName, input));

            Assert.Contains(exception.Flatten(), inner => inner is TInner);
            this.Runtime.Service.VerifyNoOtherCalls();
        }
    }
}
