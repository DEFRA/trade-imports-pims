namespace Defra.Imports.UnitTests.Infrastructure.Logging
{
    using System;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.UnitTests.Support;
    using Xunit;

    public sealed class UntimestampedTracingLogWriterTests
    {
        private readonly RecordingTracingService tracing = new RecordingTracingService();

        [Fact]
        public void Constructor_NullTracingService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new UntimestampedTracingLogWriter(null));
        }

        [Fact]
        public void Log_AnyMessage_TracesTheTagSeverityAndMessageWithoutATimestamp()
        {
            new UntimestampedTracingLogWriter(this.tracing).Log(Severity.Error, "Tag", "Message");

            Assert.Equal("Tag: Error: Message", Assert.Single(this.tracing.Messages));
        }

        [Fact]
        public void Type_IsATracingServiceLogWriter()
        {
            var writer = new UntimestampedTracingLogWriter(this.tracing);

            Assert.IsAssignableFrom<TracingServiceLogWriter>(writer);
        }

        [Fact]
        public void Type_HasExactlyOnePublicConstructorSoTheContainerCanResolveIt()
        {
            Assert.Single(typeof(UntimestampedTracingLogWriter).GetConstructors());
        }
    }
}
