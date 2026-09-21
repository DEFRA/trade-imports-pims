namespace Defra.Imports.Scenarios.Logging
{
    using System;
    using System.Collections.Concurrent;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// An <see cref="ILoggerProvider"/> that creates <see cref="MsTestLogger"/> instances writing to an MSTest <see cref="TestContext"/>.
    /// </summary>
    public class MsTestLoggerProvider : ILoggerProvider
    {
        private readonly TestContext context;
        private readonly ConcurrentDictionary<string, MsTestLogger> loggers = new ConcurrentDictionary<string, MsTestLogger>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MsTestLoggerProvider"/> class.
        /// </summary>
        /// <param name="context">The MSTest test context.</param>
        public MsTestLoggerProvider(TestContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <inheritdoc/>
        public ILogger CreateLogger(string categoryName)
        {
            return this.loggers.GetOrAdd(categoryName, name => new MsTestLogger(this.context, name));
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            this.loggers.Clear();
        }
    }
}
