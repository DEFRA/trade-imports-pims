namespace Defra.Imports.Scenarios.Logging
{
    using System;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// An <see cref="ILogger"/> implementation that writes to an MSTest <see cref="TestContext"/>.
    /// </summary>
    public class MsTestLogger : ILogger
    {
        private readonly TestContext context;
        private readonly string categoryName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MsTestLogger"/> class.
        /// </summary>
        /// <param name="context">The MSTest test context.</param>
        /// <param name="categoryName">The category name to prefix messages with (e.g. the logging type's full name).</param>
        public MsTestLogger(TestContext context, string categoryName = null)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.categoryName = categoryName;
        }

        /// <inheritdoc/>
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        /// <inheritdoc/>
        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        /// <inheritdoc/>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
            {
                return;
            }

            var message = formatter(state, exception);

            if (string.IsNullOrEmpty(message) && exception == null)
            {
                return;
            }

            var line = string.IsNullOrEmpty(this.categoryName)
                ? $"{logLevel}: {message}"
                : $"{logLevel}: [{this.categoryName}] {message}";

            if (exception != null)
            {
                line = $"{line}{Environment.NewLine}{exception}";
            }

            try
            {
                this.context.WriteLine(line);
            }
            catch (InvalidOperationException)
            {
                // The TestContext is no longer valid (e.g. the test has already completed) - there's nowhere useful left to write the message.
            }
        }

        private sealed class NullScope : IDisposable
        {
            public static readonly NullScope Instance = new NullScope();

            private NullScope()
            {
            }

            public void Dispose()
            {
            }
        }
    }
}
