# Custom workflow activity templates

Adapt these to the approved design. Replace `Itahc`, `NormaliseName` and the column with the real area, activity and column, and look up the generated names first. They are illustrative: copy the structure, not the behaviour.

## Business logic class

`src/common/Defra.Imports.BusinessLogic/Itahc/NormaliseNameBusinessLogic.cs`, added to `Defra.Imports.BusinessLogic.projitems`. It does not depend on any workflow type, so it can be unit tested directly.

```csharp
namespace Defra.Imports.BusinessLogic.Itahc
{
    using System;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>
    /// Returns the trimmed name of an ITAHC.
    /// </summary>
    public sealed class NormaliseNameBusinessLogic
    {
        private readonly IOrganizationService orgSvc;
        private readonly ILogWriter logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="NormaliseNameBusinessLogic"/> class.
        /// </summary>
        /// <param name="orgSvc">The organization service.</param>
        /// <param name="logWriter">The log writer.</param>
        public NormaliseNameBusinessLogic(IOrganizationService orgSvc, ILogWriter logWriter)
        {
            this.orgSvc = orgSvc ?? throw new ArgumentNullException(nameof(orgSvc));
            this.logWriter = logWriter ?? throw new ArgumentNullException(nameof(logWriter));
        }

        /// <summary>
        /// Gets the trimmed name of the ITAHC. This method reads and writes nothing else.
        /// </summary>
        /// <param name="itahc">The ITAHC to read.</param>
        /// <returns>The trimmed name, or an empty string when the ITAHC has no name.</returns>
        public string GetNormalisedName(EntityReference itahc)
        {
            if (itahc == null)
            {
                throw new ArgumentNullException(nameof(itahc));
            }

            var record = this.orgSvc
                .Retrieve(defraimp_itahc.EntityLogicalName, itahc.Id, new ColumnSet(defraimp_itahc.Fields.defraimp_name))
                .ToEntity<defraimp_itahc>();

            if (string.IsNullOrWhiteSpace(record.defraimp_name))
            {
                this.logWriter.Log(Severity.Info, nameof(NormaliseNameBusinessLogic), $"ITAHC {itahc.Id} has no name.");
                return string.Empty;
            }

            return record.defraimp_name.Trim();
        }
    }
}
```

## Custom workflow activity

`src/pluginassemblies/Defra.Imports.Workflows/Itahc/NormaliseName.cs`. The workflow head includes new files automatically.

```csharp
namespace Defra.Imports.Workflows.Itahc
{
    using System.Activities;
    using Defra.Imports.BusinessLogic;
    using Defra.Imports.BusinessLogic.Extensions;
    using Defra.Imports.BusinessLogic.Itahc;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.Model;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>
    /// Gets the trimmed name of an ITAHC for use in a process.
    /// </summary>
    [CrmPluginRegistration(
        nameof(NormaliseName),
        "Normalise the name of an ITAHC",
        "Returns the name of an ITAHC with leading and trailing white space removed",
        "Defra.Imports.Workflows.Itahc",
        IsolationModeEnum.Sandbox)]
    public class NormaliseName : Defra.Imports.Workflows.WorkflowActivity
    {
        /// <summary>
        /// Gets or sets the ITAHC to read.
        /// </summary>
        [Input("ITAHC")]
        [RequiredArgument]
        [ReferenceTarget(defraimp_itahc.EntityLogicalName)]
        public InArgument<EntityReference> Itahc { get; set; }

        /// <summary>
        /// Gets or sets the trimmed name.
        /// </summary>
        [Output("Normalised name")]
        public OutArgument<string> NormalisedName { get; set; }

        /// <inheritdoc/>
        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            var itahc = this.Itahc.GetRequired(context, nameof(this.Itahc));

            var name = new NormaliseNameBusinessLogic(orgSvc, logWriter).GetNormalisedName(itahc);

            this.NormalisedName.Set(context, name);
        }
    }
}
```

The base class is written in full because the `using Defra.Imports.BusinessLogic;` that `[CrmPluginRegistration]` needs also brings in a different `WorkflowActivity`, and a using directive inside the namespace wins over the enclosing `Defra.Imports.Workflows` namespace.

## Activity test

`tests/Defra.Imports.UnitTests/Workflows/Itahc/NormaliseNameTests.cs`. `WorkflowActivityTests<T>` provides the invoker and the mocks.

```csharp
namespace Defra.Imports.UnitTests.Workflows.Itahc
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.Model;
    using Defra.Imports.Workflows.Itahc;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="NormaliseName"/>.
    /// </summary>
    public sealed class NormaliseNameTests : WorkflowActivityTests<NormaliseName>
    {
        /// <summary>
        /// Tests that the output is the trimmed name.
        /// </summary>
        [Fact]
        public void Execute_WithAName_ReturnsTheTrimmedName()
        {
            // Arrange
            var itahcId = Guid.NewGuid();
            this.OrgSvcMock
                .Setup(o => o.Retrieve(defraimp_itahc.EntityLogicalName, itahcId, It.IsAny<ColumnSet>()))
                .Returns(new defraimp_itahc { Id = itahcId, defraimp_name = " name " });

            // Act
            var outputs = this.WorkflowInvoker.Invoke(GetInputs(new EntityReference(defraimp_itahc.EntityLogicalName, itahcId)));

            // Assert
            Assert.Equal("name", outputs[nameof(NormaliseName.NormalisedName)]);
        }

        /// <summary>
        /// Tests that a missing required input fails.
        /// </summary>
        [Fact]
        public void Execute_WithoutAnItahc_Throws()
        {
            // Act and assert
            Assert.ThrowsAny<Exception>(() => this.WorkflowInvoker.Invoke(GetInputs(null)));
        }

        private static IDictionary<string, object> GetInputs(EntityReference itahc)
        {
            return new Dictionary<string, object> { { nameof(NormaliseName.Itahc), itahc } };
        }
    }
}
```

Add a `NormaliseNameBusinessLogicTests` class for the rules, as `AddSpeciesAnimalNumbersTests` does for its logic class.
