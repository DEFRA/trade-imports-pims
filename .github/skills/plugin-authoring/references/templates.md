# Plug-in templates

A configuration-driven plug-in, shown in the order you build it: tests first, then the smallest code that passes them. The code was compiled and run against this repository's analysers and test project with no added warnings.

The capability is "copy configured attributes from the triggering record onto a related record". It is named for that capability, not for any first use. Which lookup and which attributes are in the step's unsecure configuration, so the first requirement is one row of configuration and the next similar requirement is another step, not another plug-in.

Adapt the shape, not the behaviour. Look up generated names first, and use the early-bound model wherever the table is known at compile time. The late-bound `Entity` here is deliberate, because the names are configuration.

## Order of work

1. Settings: configuration is parsed and validated in one place.
2. Business logic: the rule, with a second configuration as proof.
3. Plug-in: the thin adapter.

Run each test and confirm that it fails for the right reason before writing the code for it. A missing type is a compile failure; add the smallest stub so that the test fails on behaviour.

## 1. Settings

Test: `tests/Defra.Imports.UnitTests/BusinessLogic/Mapping/MapFieldsToRelatedRecordSettingsTests.cs`.

```csharp
namespace Defra.Imports.UnitTests.BusinessLogic.Mapping
{
    using Defra.Imports.BusinessLogic.Mapping;
    using Microsoft.Xrm.Sdk;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="MapFieldsToRelatedRecordSettings"/>.
    /// </summary>
    public sealed class MapFieldsToRelatedRecordSettingsTests
    {
        /// <summary>
        /// Tests that the lookup and every mapping are read from the configuration.
        /// </summary>
        [Fact]
        public void Parse_WithAValidConfiguration_ReadsTheLookupAndMappings()
        {
            // Arrange
            const string config =
                "<MapFieldsToRelatedRecord lookup='parentid'><FieldMappings>" +
                "<FieldMapping mapfrom='a' mapto='b' /><FieldMapping mapfrom='c' mapto='d' />" +
                "</FieldMappings></MapFieldsToRelatedRecord>";

            // Act
            var settings = MapFieldsToRelatedRecordSettings.Parse(config);

            // Assert
            Assert.Equal("parentid", settings.LookupAttribute);
            Assert.Equal(2, settings.FieldMappings.Count);
            Assert.Equal("b", settings.FieldMappings["a"]);
            Assert.Equal("d", settings.FieldMappings["c"]);
        }

        /// <summary>
        /// Tests that missing, malformed or incomplete configuration is rejected with a clear message.
        /// </summary>
        /// <param name="config">The configuration.</param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not xml")]
        [InlineData("<MapFieldsToRelatedRecord><FieldMappings><FieldMapping mapfrom='a' mapto='b' /></FieldMappings></MapFieldsToRelatedRecord>")]
        [InlineData("<MapFieldsToRelatedRecord lookup='parentid'><FieldMappings /></MapFieldsToRelatedRecord>")]
        public void Parse_WithInvalidConfiguration_Throws(string config)
        {
            // Act and assert
            Assert.Throws<InvalidPluginExecutionException>(() => MapFieldsToRelatedRecordSettings.Parse(config));
        }
    }
}
```

Code: `src/common/Defra.Imports.BusinessLogic/Mapping/MapFieldsToRelatedRecordSettings.cs`. It reuses the repository's existing `FieldMappingConfigParser` rather than inventing a second mapping format. Add it to `Defra.Imports.BusinessLogic.projitems`.

```csharp
namespace Defra.Imports.BusinessLogic.Mapping
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Defra.Imports.BusinessLogic.Utils;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// The configuration of the map-fields-to-related-record capability, parsed from a step's unsecure configuration.
    /// </summary>
    public sealed class MapFieldsToRelatedRecordSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapFieldsToRelatedRecordSettings"/> class.
        /// </summary>
        /// <param name="lookupAttribute">The logical name of the lookup that points at the record to update.</param>
        /// <param name="fieldMappings">The attributes to copy, keyed by source attribute with the target attribute as the value.</param>
        public MapFieldsToRelatedRecordSettings(string lookupAttribute, IReadOnlyDictionary<string, string> fieldMappings)
        {
            this.LookupAttribute = lookupAttribute ?? throw new ArgumentNullException(nameof(lookupAttribute));
            this.FieldMappings = fieldMappings ?? throw new ArgumentNullException(nameof(fieldMappings));
        }

        /// <summary>
        /// Gets the logical name of the lookup that points at the record to update.
        /// </summary>
        public string LookupAttribute { get; }

        /// <summary>
        /// Gets the attributes to copy, keyed by source attribute with the target attribute as the value.
        /// </summary>
        public IReadOnlyDictionary<string, string> FieldMappings { get; }

        /// <summary>
        /// Parses the configuration, for example
        /// <c>&lt;MapFieldsToRelatedRecord lookup="parentid"&gt;&lt;FieldMappings&gt;&lt;FieldMapping mapfrom="a" mapto="b" /&gt;&lt;/FieldMappings&gt;&lt;/MapFieldsToRelatedRecord&gt;</c>.
        /// </summary>
        /// <param name="config">The step configuration.</param>
        /// <returns>The settings.</returns>
        /// <exception cref="InvalidPluginExecutionException">The configuration is missing or invalid.</exception>
        public static MapFieldsToRelatedRecordSettings Parse(string config)
        {
            if (string.IsNullOrWhiteSpace(config))
            {
                throw new InvalidPluginExecutionException("The step has no configuration. Supply a MapFieldsToRelatedRecord element with a lookup attribute and field mappings.");
            }

            var document = new XmlDocument();
            try
            {
                document.LoadXml(config);
            }
            catch (XmlException exception)
            {
                throw new InvalidPluginExecutionException("The step configuration is not valid XML.", exception);
            }

            var lookup = document.DocumentElement?.GetAttribute("lookup");
            if (string.IsNullOrWhiteSpace(lookup))
            {
                throw new InvalidPluginExecutionException("The step configuration must set the lookup attribute on the root element.");
            }

            var mappings = new FieldMappingConfigParser().ParseMappingConfig(config);
            if (mappings.Count == 0)
            {
                throw new InvalidPluginExecutionException("The step configuration must contain at least one FieldMapping.");
            }

            return new MapFieldsToRelatedRecordSettings(lookup, mappings);
        }
    }
}
```

## 2. Business logic

Test: `tests/Defra.Imports.UnitTests/BusinessLogic/Mapping/MapFieldsToRelatedRecordBusinessLogicTests.cs`. Write them in this order, simplest first: nothing to do, one mapping, a second configuration, nothing mapped, and no mutation. The second-configuration test is the proof that nothing is hard-coded; keep one in every configuration-driven class.

```csharp
namespace Defra.Imports.UnitTests.BusinessLogic.Mapping
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.Mapping;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="MapFieldsToRelatedRecordBusinessLogic"/>.
    /// </summary>
    public sealed class MapFieldsToRelatedRecordBusinessLogicTests
    {
        private readonly Mock<IOrganizationService> orgSvcMock = new Mock<IOrganizationService>();
        private readonly Mock<ILogWriter> logWriterMock = new Mock<ILogWriter>();

        /// <summary>
        /// Tests that nothing is written when the source has no related record.
        /// </summary>
        [Fact]
        public void Map_WhenTheLookupIsEmpty_WritesNothing()
        {
            // Arrange
            var sut = this.CreateSut("parentid", new Dictionary<string, string> { { "a", "b" } });
            var source = new Entity("child", Guid.NewGuid()) { ["a"] = "value" };

            // Act
            sut.Map(source);

            // Assert
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
        }

        /// <summary>
        /// Tests that a mapped value is copied onto the related record.
        /// </summary>
        [Fact]
        public void Map_WithOneMapping_UpdatesTheRelatedRecordWithTheMappedValue()
        {
            // Arrange
            var parent = new EntityReference("parent", Guid.NewGuid());
            var sut = this.CreateSut("parentid", new Dictionary<string, string> { { "a", "b" } });
            var source = new Entity("child", Guid.NewGuid()) { ["parentid"] = parent, ["a"] = "value" };
            Entity updated = null;
            this.orgSvcMock.Setup(o => o.Update(It.IsAny<Entity>())).Callback<Entity>(e => updated = e);

            // Act
            sut.Map(source);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal("parent", updated.LogicalName);
            Assert.Equal(parent.Id, updated.Id);
            Assert.Equal("value", updated["b"]);
            Assert.Single(updated.Attributes);
        }

        /// <summary>
        /// Tests that a different configuration changes the behaviour without any code change.
        /// </summary>
        [Fact]
        public void Map_WithADifferentConfiguration_MapsDifferentAttributesAndLookup()
        {
            // Arrange
            var owner = new EntityReference("owner", Guid.NewGuid());
            var mappings = new Dictionary<string, string> { { "x", "y" }, { "p", "q" } };
            var sut = this.CreateSut("ownerid", mappings);
            var source = new Entity("child", Guid.NewGuid()) { ["ownerid"] = owner, ["x"] = 1, ["p"] = 2, ["a"] = "ignored" };
            Entity updated = null;
            this.orgSvcMock.Setup(o => o.Update(It.IsAny<Entity>())).Callback<Entity>(e => updated = e);

            // Act
            sut.Map(source);

            // Assert
            Assert.Equal("owner", updated.LogicalName);
            Assert.Equal(1, updated["y"]);
            Assert.Equal(2, updated["q"]);
            Assert.False(updated.Contains("a"));
        }

        /// <summary>
        /// Tests that nothing is written when none of the mapped attributes are on the source.
        /// </summary>
        [Fact]
        public void Map_WhenNoMappedAttributeIsPresent_WritesNothing()
        {
            // Arrange
            var parent = new EntityReference("parent", Guid.NewGuid());
            var sut = this.CreateSut("parentid", new Dictionary<string, string> { { "a", "b" } });
            var source = new Entity("child", Guid.NewGuid()) { ["parentid"] = parent };

            // Act
            sut.Map(source);

            // Assert
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
        }

        /// <summary>
        /// Tests that the source record is not modified.
        /// </summary>
        [Fact]
        public void Map_DoesNotModifyTheSource()
        {
            // Arrange
            var parent = new EntityReference("parent", Guid.NewGuid());
            var sut = this.CreateSut("parentid", new Dictionary<string, string> { { "a", "b" } });
            var source = new Entity("child", Guid.NewGuid()) { ["parentid"] = parent, ["a"] = "value" };

            // Act
            sut.Map(source);

            // Assert
            Assert.Equal(2, source.Attributes.Count);
            Assert.False(source.Contains("b"));
        }

        private MapFieldsToRelatedRecordBusinessLogic CreateSut(string lookup, IReadOnlyDictionary<string, string> mappings)
        {
            return new MapFieldsToRelatedRecordBusinessLogic(this.orgSvcMock.Object, this.logWriterMock.Object, lookup, mappings);
        }
    }
}
```

Code: `src/common/Defra.Imports.BusinessLogic/Mapping/MapFieldsToRelatedRecordBusinessLogic.cs`, added to the `.projitems`. It depends on abstractions only (`IOrganizationService`, `ILogWriter`) and receives its configuration already parsed.

```csharp
namespace Defra.Imports.BusinessLogic.Mapping
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.BusinessLogic.Logging;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Copies configured attributes from a source record onto the record that one of its lookups points at.
    /// The attribute and lookup names are configuration, so the class works for any pair of tables.
    /// </summary>
    public sealed class MapFieldsToRelatedRecordBusinessLogic
    {
        private readonly IOrganizationService orgSvc;
        private readonly ILogWriter logWriter;
        private readonly string lookupAttribute;
        private readonly IReadOnlyDictionary<string, string> fieldMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFieldsToRelatedRecordBusinessLogic"/> class.
        /// </summary>
        /// <param name="orgSvc">The organization service.</param>
        /// <param name="logWriter">The log writer.</param>
        /// <param name="lookupAttribute">The logical name of the lookup that points at the record to update.</param>
        /// <param name="fieldMappings">The attributes to copy, keyed by source attribute with the target attribute as the value.</param>
        public MapFieldsToRelatedRecordBusinessLogic(IOrganizationService orgSvc, ILogWriter logWriter, string lookupAttribute, IReadOnlyDictionary<string, string> fieldMappings)
        {
            this.orgSvc = orgSvc ?? throw new ArgumentNullException(nameof(orgSvc));
            this.logWriter = logWriter ?? throw new ArgumentNullException(nameof(logWriter));
            this.lookupAttribute = lookupAttribute ?? throw new ArgumentNullException(nameof(lookupAttribute));
            this.fieldMappings = fieldMappings ?? throw new ArgumentNullException(nameof(fieldMappings));
        }

        /// <summary>
        /// Copies the mapped attributes that the source holds onto the related record with one update.
        /// Writes nothing when the lookup is empty or none of the mapped attributes are present. The source is not modified.
        /// </summary>
        /// <param name="source">The source record.</param>
        public void Map(Entity source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var related = source.GetAttributeValue<EntityReference>(this.lookupAttribute);
            if (related == null)
            {
                this.logWriter.Log(Severity.Info, nameof(MapFieldsToRelatedRecordBusinessLogic), $"{source.LogicalName} {source.Id} has no value for {this.lookupAttribute}; nothing mapped.");
                return;
            }

            var update = new Entity(related.LogicalName, related.Id);
            foreach (var mapping in this.fieldMappings)
            {
                if (source.Contains(mapping.Key))
                {
                    update[mapping.Value] = source[mapping.Key];
                }
            }

            if (update.Attributes.Count == 0)
            {
                return;
            }

            this.orgSvc.Update(update);
        }
    }
}
```

## 3. Plug-in

Test: `tests/Defra.Imports.UnitTests/Plugins/Mapping/MapFieldsToRelatedRecordTests.cs`. It drives the plug-in through `IServiceProvider` and constructs it with step configuration, as the platform does.

```csharp
namespace Defra.Imports.UnitTests.Plugins.Mapping
{
    using System;
    using Defra.Imports.Plugins.Mapping;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="MapFieldsToRelatedRecord"/>.
    /// </summary>
    public sealed class MapFieldsToRelatedRecordTests
    {
        private const string Config =
            "<MapFieldsToRelatedRecord lookup='parentid'><FieldMappings>" +
            "<FieldMapping mapfrom='a' mapto='b' /></FieldMappings></MapFieldsToRelatedRecord>";

        private readonly Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
        private readonly Mock<IPluginExecutionContext> pluginContextMock = new Mock<IPluginExecutionContext>();
        private readonly Mock<IOrganizationService> orgSvcMock = new Mock<IOrganizationService>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFieldsToRelatedRecordTests"/> class.
        /// </summary>
        public MapFieldsToRelatedRecordTests()
        {
            var orgSvcFactoryMock = new Mock<IOrganizationServiceFactory>();
            orgSvcFactoryMock.Setup(f => f.CreateOrganizationService(It.IsAny<Guid?>())).Returns(this.orgSvcMock.Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(ITracingService))).Returns(new Mock<ITracingService>().Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(IPluginExecutionContext))).Returns(this.pluginContextMock.Object);
            this.serviceProviderMock.Setup(s => s.GetService(typeof(IOrganizationServiceFactory))).Returns(orgSvcFactoryMock.Object);
            this.pluginContextMock.Setup(c => c.PreEntityImages).Returns(new EntityImageCollection());
        }

        /// <summary>
        /// Tests that the mapping in the step configuration is applied to the related record.
        /// </summary>
        [Fact]
        public void Execute_WithStepConfiguration_UpdatesTheRelatedRecord()
        {
            // Arrange
            var parent = new EntityReference("parent", Guid.NewGuid());
            var target = new Entity("child", Guid.NewGuid()) { ["parentid"] = parent, ["a"] = "value" };
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection { { "Target", target } });
            Entity updated = null;
            this.orgSvcMock.Setup(o => o.Update(It.IsAny<Entity>())).Callback<Entity>(e => updated = e);

            // Act
            new MapFieldsToRelatedRecord(Config, null).Execute(this.serviceProviderMock.Object);

            // Assert
            Assert.Equal(parent.Id, updated.Id);
            Assert.Equal("value", updated["b"]);
        }

        /// <summary>
        /// Tests that values only on the pre-image are still mapped when the target omits them.
        /// </summary>
        [Fact]
        public void Execute_WhenTheTargetOmitsAMappedAttribute_UsesThePreImage()
        {
            // Arrange
            var parent = new EntityReference("parent", Guid.NewGuid());
            var target = new Entity("child", Guid.NewGuid()) { ["parentid"] = parent };
            var preImage = new Entity("child", target.Id) { ["a"] = "from image" };
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection { { "Target", target } });
            this.pluginContextMock.Setup(c => c.PreEntityImages).Returns(new EntityImageCollection { { "PreImage", preImage } });
            Entity updated = null;
            this.orgSvcMock.Setup(o => o.Update(It.IsAny<Entity>())).Callback<Entity>(e => updated = e);

            // Act
            new MapFieldsToRelatedRecord(Config, null).Execute(this.serviceProviderMock.Object);

            // Assert
            Assert.Equal("from image", updated["b"]);
        }

        /// <summary>
        /// Tests that a step with no configuration fails with a clear message and writes nothing.
        /// </summary>
        [Fact]
        public void Execute_WithoutStepConfiguration_ThrowsAndWritesNothing()
        {
            // Arrange
            var target = new Entity("child", Guid.NewGuid());
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection { { "Target", target } });

            // Act and assert
            Assert.Throws<InvalidPluginExecutionException>(() => new MapFieldsToRelatedRecord().Execute(this.serviceProviderMock.Object));
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
        }
    }
}
```

Code: `src/pluginassemblies/Defra.Imports.Plugins/Mapping/MapFieldsToRelatedRecord.cs`. The plug-in head includes new files automatically. The class needs the `(string, string)` constructor, because that is what the platform calls when a step has configuration.

```csharp
namespace Defra.Imports.Plugins.Mapping
{
    using Defra.Imports.BusinessLogic;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.Mapping;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Copies attributes from the triggering record onto a related record. The lookup and the attribute mappings
    /// come from the step's unsecure configuration, so each registered step defines its own mapping.
    /// </summary>
    public class MapFieldsToRelatedRecord : Plugin
    {
        private const string PreImageName = "PreImage";

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFieldsToRelatedRecord"/> class.
        /// </summary>
        public MapFieldsToRelatedRecord()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MapFieldsToRelatedRecord"/> class.
        /// </summary>
        /// <param name="unsecureConfig">The step's unsecure configuration.</param>
        /// <param name="secureConfig">The step's secure configuration.</param>
        public MapFieldsToRelatedRecord(string unsecureConfig, string secureConfig)
            : base(unsecureConfig, secureConfig)
        {
        }

        /// <inheritdoc/>
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            var settings = MapFieldsToRelatedRecordSettings.Parse(this.UnsecureConfig);
            var target = (Entity)context.InputParameters["Target"];

            // The target holds only the attributes that were supplied, so fill the rest from the pre-image.
            var source = new Entity(target.LogicalName, target.Id);
            if (context.PreEntityImages.Contains(PreImageName))
            {
                source.Attributes.AddRange(context.PreEntityImages[PreImageName].Attributes);
            }

            foreach (var attribute in target.Attributes)
            {
                source[attribute.Key] = attribute.Value;
            }

            new MapFieldsToRelatedRecordBusinessLogic(orgSvc, logWriter, settings.LookupAttribute, settings.FieldMappings).Map(source);
        }
    }
}
```

## Step registration for the hand-off

The step is registered in a development environment, not in code. For the first use, the user registers the step on the plug-in class with the message, table, stage, mode, filtering attributes and image that the design states, and pastes the configuration into the step's unsecure configuration. The names below are placeholders; use the real logical names from the model.

```xml
<MapFieldsToRelatedRecord lookup="<lookup logical name on the triggering table>">
  <FieldMappings>
    <FieldMapping mapfrom="<source column>" mapto="<column on the related record>" />
  </FieldMappings>
</MapFieldsToRelatedRecord>
```

A second requirement of the same kind is a second step with its own configuration. No new class is needed. A requirement that this class cannot express, such as resolving the related record by a query instead of a lookup, is a new strategy or a new generic class chosen in the design, not a use-case-named copy of this one.

Do not add a `[CrmPluginRegistration]` attribute that bakes in one use of a generic plug-in. If the team's convention requires one, add it per documented step.
