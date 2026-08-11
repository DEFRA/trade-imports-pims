namespace Defra.Imports.IntegrationTests
{
    using System;
    using Defra.Imports.Model;

    public class TestCasesBase : IDisposable
    {
        protected readonly CommonDataServiceFixture fixture;
        protected readonly ImportsContext context;
        protected bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="TestCasesBase"/> class. This class is used for configuring the environment for tests.
        /// </summary>
        public TestCasesBase()
        {
            this.fixture = new CommonDataServiceFixture();
            this.context = new ImportsContext(this.fixture.AdminTestClient);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.context.Dispose();
                    this.fixture.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}
