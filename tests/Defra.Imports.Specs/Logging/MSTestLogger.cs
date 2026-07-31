namespace Defra.Imports.Specs
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// An <see cref="ILogger"/> implementation for MSTest.
    /// </summary>
    public class MsTestLogger : ILogger, IDisposable
    {
        private readonly TestContext context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsTestLogger"/> class.
        /// </summary>
        /// <param name="context">The MSTest test context.</param>
        public MsTestLogger(TestContext context)
        {
            this.context = context;
        }

        /// <inheritdoc/>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            this.context.WriteLine("{0}: {1} {2}", logLevel, state.ToString(), (exception == null) ? string.Empty : exception.ToString());
        }

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
        }
    }
}