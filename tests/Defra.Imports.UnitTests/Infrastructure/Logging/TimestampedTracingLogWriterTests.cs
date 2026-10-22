namespace Defra.Imports.UnitTests.Infrastructure.Logging
{
    using System;
    using System.Linq;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Logging;
    using Defra.Imports.UnitTests.Support;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    public sealed class TimestampedTracingLogWriterTests
    {
        private readonly RecordingTracingService tracing = new RecordingTracingService();

        [Fact]
        public void Constructor_NullTracingService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new TimestampedTracingLogWriter(null));
        }

        [Fact]
        public void Log_AnyMessage_PrefixesATimestamp()
        {
            new TimestampedTracingLogWriter(this.tracing).Log(Severity.Error, "Tag", "Message");

            Assert.Matches(@"^\d{4}-\d{2}-\d{2}T.*: Tag: Error: Message$", Assert.Single(this.tracing.Messages));
        }

        [Fact]
        public void Type_HasExactlyOnePublicConstructorSoTheContainerCanResolveIt()
        {
            Assert.Single(typeof(TimestampedTracingLogWriter).GetConstructors());
        }

        [Fact]
        public void Resolve_RegisteredAsALogWriter_ReturnsItWithTheScopesTracingService()
        {
            var root = new PluginContainer();
            root.Register<ILogWriter, TimestampedTracingLogWriter>();
            using (var scope = root.CreateExecutionScope())
            {
                scope.Register<ITracingService>(this.tracing);

                scope.Resolve<ILogWriter>().Log(Severity.Info, "Tag", "Message");

                Assert.EndsWith(": Tag: Info: Message", this.tracing.Messages.Single(), StringComparison.Ordinal);
            }
        }
    }
}
