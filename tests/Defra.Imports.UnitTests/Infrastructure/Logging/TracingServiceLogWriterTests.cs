namespace Defra.Imports.UnitTests.Infrastructure.Logging
{
    using System;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class TracingServiceLogWriterTests
    {
        private readonly RecordingTracingService tracing = new RecordingTracingService();

        [Fact]
        public void Constructor_NullTracingService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TracingServiceLogWriter(null));
        }

        [Theory]
        [InlineData(Severity.Verbose, "Verbose")]
        [InlineData(Severity.Info, "Info")]
        [InlineData(Severity.Warning, "Warning")]
        [InlineData(Severity.Error, "Error")]
        public void Log_WithoutTimestamps_TracesTheTagSeverityAndMessage(Severity severity, string expectedSeverity)
        {
            new TracingServiceLogWriter(this.tracing).Log(severity, "Tag", "Message");

            Assert.Equal("Tag: " + expectedSeverity + ": Message", Assert.Single(this.tracing.Messages));
        }

        [Fact]
        public void Log_WithTimestamps_PrefixesAnIso8601Timestamp()
        {
            new TracingServiceLogWriter(this.tracing, true).Log(Severity.Warning, "Tag", "Message");

            Assert.Matches(@"^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}\.\d+Z?: Tag: Warning: Message$", Assert.Single(this.tracing.Messages));
        }

        [Fact]
        public void Log_MessageContainingBraces_IsTracedLiterally()
        {
            new TracingServiceLogWriter(this.tracing).Log(Severity.Info, "Tag", "Value {0}");

            Assert.Equal("Tag: Info: Value {0}", Assert.Single(this.tracing.Messages));
        }
    }
}
