# Plug-in templates

Adapt these to the approved design. Replace `Itahc`, `ExampleTitle` and `defraimp_name` with the real area, use case and column, and look up the generated names first. They are illustrative: copy the structure, not the behaviour.

## Business logic class

`src/common/Defra.Imports.BusinessLogic/Itahc/ExampleTitleBusinessLogic.cs`. Add it to `Defra.Imports.BusinessLogic.projitems`.

```csharp
namespace Defra.Imports.BusinessLogic.Itahc
{
    using System;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Describes the rule that the class applies.
    /// </summary>
    public sealed class ExampleTitleBusinessLogic
    {
        private readonly IOrganizationService orgSvc;
        private readonly ILogWriter logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleTitleBusinessLogic"/> class.
        /// </summary>
        /// <param name="orgSvc">The organization service.</param>
        /// <param name="logWriter">The log writer.</param>
        public ExampleTitleBusinessLogic(IOrganizationService orgSvc, ILogWriter logWriter)
        {
            this.orgSvc = orgSvc ?? throw new ArgumentNullException(nameof(orgSvc));
            this.logWriter = logWriter ?? throw new ArgumentNullException(nameof(logWriter));
        }

        /// <summary>
        /// Applies the rule and writes the result. State here whether the method writes.
        /// </summary>
        /// <param name="target">The record from the plug-in target.</param>
        /// <param name="preImage">The record as it was before the change, or null.</param>
        public void Apply(defraimp_itahc target, defraimp_itahc preImage)
        {
            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            var name = target.defraimp_name ?? preImage?.defraimp_name;
            if (string.IsNullOrWhiteSpace(name))
            {
                this.logWriter.Log(Severity.Info, nameof(ExampleTitleBusinessLogic), "No name to apply; nothing written.");
                return;
            }

            // Update only the attributes that change. Do not send the whole input entity.
            this.orgSvc.Update(new defraimp_itahc
            {
                Id = target.Id,
                defraimp_name = name.Trim(),
            });
        }
    }
}
```

## Plug-in class

`src/pluginassemblies/Defra.Imports.Plugins/Itahc/ExampleTitle.cs`. The plug-in head includes new files automatically.

```csharp
namespace Defra.Imports.Plugins.Itahc
{
    using Defra.Imports.BusinessLogic;
    using Defra.Imports.BusinessLogic.Itahc;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;

    /// <summary>
    /// Trims the name of an ITAHC when it changes.
    /// </summary>
    [CrmPluginRegistration(
        MessageNameEnum.Update,
        defraimp_itahc.EntityLogicalName,
        StageEnum.PostOperation,
        ExecutionModeEnum.Synchronous,
        "defraimp_name",
        "Update Step - ITAHC Example Title",
        0,
        IsolationModeEnum.Sandbox,
        Image1Attributes = "defraimp_name",
        Image1Name = "PreImage",
        Image1Type = ImageTypeEnum.PreImage)]
    public class ExampleTitle : Plugin
    {
        private const string PreImageName = "PreImage";

        /// <inheritdoc/>
        protected override void Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)
        {
            var target = ((Entity)context.InputParameters["Target"]).ToEntity<defraimp_itahc>();
            var preImage = this.GetPreImage<defraimp_itahc>(context, PreImageName);

            new ExampleTitleBusinessLogic(orgSvc, logWriter).Apply(target, preImage);
        }
    }
}
```

The attribute's first argument may be a `MessageNameEnum` (as above) or a custom message name string. Keep the image name and attributes consistent between the attribute, the `GetPreImage` call and the step registered in the development environment.

## Plug-in test

`tests/Defra.Imports.UnitTests/Plugins/Itahc/ExampleTitleTests.cs`. Test the plug-in through `IServiceProvider`, as `PopulateAnimalNumberTests` does.

```csharp
namespace Defra.Imports.UnitTests.Plugins.Itahc
{
    using System;
    using Defra.Imports.Model;
    using Defra.Imports.Plugins.Itahc;
    using Microsoft.Xrm.Sdk;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="ExampleTitle"/>.
    /// </summary>
    public sealed class ExampleTitleTests
    {
        private readonly Mock<IServiceProvider> serviceProviderMock;
        private readonly Mock<IPluginExecutionContext> pluginContextMock;
        private readonly Mock<IOrganizationService> orgSvcMock;
        private readonly ExampleTitle sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleTitleTests"/> class.
        /// </summary>
        public ExampleTitleTests()
        {
            var tracingServiceMock = new Mock<ITracingService>();
            var orgSvcFactoryMock = new Mock<IOrganizationServiceFactory>();

            this.pluginContextMock = new Mock<IPluginExecutionContext>();
            this.orgSvcMock = new Mock<IOrganizationService>();
            this.serviceProviderMock = new Mock<IServiceProvider>();

            orgSvcFactoryMock
                .Setup(f => f.CreateOrganizationService(It.IsAny<Guid?>()))
                .Returns(this.orgSvcMock.Object);
            this.serviceProviderMock
                .Setup(s => s.GetService(typeof(ITracingService)))
                .Returns(tracingServiceMock.Object);
            this.serviceProviderMock
                .Setup(s => s.GetService(typeof(IPluginExecutionContext)))
                .Returns(this.pluginContextMock.Object);
            this.serviceProviderMock
                .Setup(s => s.GetService(typeof(IOrganizationServiceFactory)))
                .Returns(orgSvcFactoryMock.Object);

            this.pluginContextMock.Setup(c => c.PreEntityImages).Returns(new EntityImageCollection());

            this.sut = new ExampleTitle();
        }

        /// <summary>
        /// Tests that a changed name is trimmed and written back.
        /// </summary>
        [Fact]
        public void Execute_WhenTheNameChanges_UpdatesTheTrimmedName()
        {
            // Arrange
            var target = new defraimp_itahc { Id = Guid.NewGuid(), defraimp_name = "  CHEDA.GB.2026.0000001  " };
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection { { "Target", target } });

            defraimp_itahc updated = null;
            this.orgSvcMock
                .Setup(o => o.Update(It.IsAny<Entity>()))
                .Callback<Entity>(e => updated = e.ToEntity<defraimp_itahc>());

            // Act
            this.sut.Execute(this.serviceProviderMock.Object);

            // Assert
            Assert.NotNull(updated);
            Assert.Equal(target.Id, updated.Id);
            Assert.Equal("CHEDA.GB.2026.0000001", updated.defraimp_name);
        }

        /// <summary>
        /// Tests that nothing is written when there is no name.
        /// </summary>
        [Fact]
        public void Execute_WhenThereIsNoName_WritesNothing()
        {
            // Arrange
            var target = new defraimp_itahc { Id = Guid.NewGuid() };
            this.pluginContextMock.Setup(c => c.InputParameters).Returns(new ParameterCollection { { "Target", target } });

            // Act
            this.sut.Execute(this.serviceProviderMock.Object);

            // Assert
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
        }
    }
}
```

Add a separate `ExampleTitleBusinessLogicTests` class that constructs the business logic class with `Mock<IOrganizationService>` and `Mock<ILogWriter>`, and covers the rules without the plug-in plumbing, as `AddSpeciesAnimalNumbersTests` does.
