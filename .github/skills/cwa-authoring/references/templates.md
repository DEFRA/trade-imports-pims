# Custom workflow activity templates

A configuration-driven activity, shown in the order you build it: tests first, then the smallest code that passes them. The code was compiled and run against this repository's analysers and test project with no added warnings.

The capability is "trim the named text fields of a record". It is named for that capability. The table and the fields are inputs, so a maker reuses one activity across tables and processes, and the next similar requirement needs no release. Compare `MapFieldsFromItahcToImportRecord`, which exposes its mapping web resource as an input for the same reason.

Adapt the shape, not the behaviour. Use the early-bound model wherever the table is known at compile time. The late-bound `Entity` here is deliberate, because the table and fields are inputs.

## Order of work

1. Business logic: the rule, with a second input set as proof that nothing is hard-coded.
2. Activity: the thin adapter that reads inputs and sets outputs.

Run each test and confirm that it fails for the right reason before writing the code for it. A missing type is a compile failure; add the smallest stub so that the test fails on behaviour.

## 1. Business logic

Test: `tests/Defra.Imports.UnitTests/BusinessLogic/Text/TrimTextFieldsBusinessLogicTests.cs`. Write them simplest first: the main case, a different field list, nothing to do, then invalid input.

```csharp
namespace Defra.Imports.UnitTests.BusinessLogic.Text
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.Text;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="TrimTextFieldsBusinessLogic"/>.
    /// </summary>
    public sealed class TrimTextFieldsBusinessLogicTests
    {
        private readonly Mock<IOrganizationService> orgSvcMock = new Mock<IOrganizationService>();
        private readonly TrimTextFieldsBusinessLogic sut;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrimTextFieldsBusinessLogicTests"/> class.
        /// </summary>
        public TrimTextFieldsBusinessLogicTests()
        {
            this.sut = new TrimTextFieldsBusinessLogic(this.orgSvcMock.Object, new Mock<ILogWriter>().Object);
        }

        /// <summary>
        /// Tests that only the changed fields are written, and the count is returned.
        /// </summary>
        [Fact]
        public void Trim_WithPaddedAndCleanFields_UpdatesOnlyThePaddedOnes()
        {
            // Arrange
            var record = new EntityReference("thing", Guid.NewGuid());
            this.SetupRecord(record, new Entity("thing", record.Id) { ["name"] = "  padded ", ["code"] = "clean" });
            Entity updated = null;
            this.orgSvcMock.Setup(o => o.Update(It.IsAny<Entity>())).Callback<Entity>(e => updated = e);

            // Act
            var count = this.sut.Trim(record, new[] { "name", "code" });

            // Assert
            Assert.Equal(1, count);
            Assert.Equal("thing", updated.LogicalName);
            Assert.Equal(record.Id, updated.Id);
            Assert.Equal("padded", updated["name"]);
            Assert.False(updated.Contains("code"));
        }

        /// <summary>
        /// Tests that a different list of fields changes the behaviour without any code change.
        /// </summary>
        [Fact]
        public void Trim_WithADifferentFieldList_TrimsThoseFields()
        {
            // Arrange
            var record = new EntityReference("other", Guid.NewGuid());
            this.SetupRecord(record, new Entity("other", record.Id) { ["title"] = " t ", ["notes"] = " n " });
            Entity updated = null;
            this.orgSvcMock.Setup(o => o.Update(It.IsAny<Entity>())).Callback<Entity>(e => updated = e);

            // Act
            var count = this.sut.Trim(record, new[] { "title", "notes" });

            // Assert
            Assert.Equal(2, count);
            Assert.Equal("t", updated["title"]);
            Assert.Equal("n", updated["notes"]);
        }

        /// <summary>
        /// Tests that nothing is written when nothing needs trimming.
        /// </summary>
        [Fact]
        public void Trim_WhenNothingNeedsTrimming_WritesNothing()
        {
            // Arrange
            var record = new EntityReference("thing", Guid.NewGuid());
            this.SetupRecord(record, new Entity("thing", record.Id) { ["name"] = "clean" });

            // Act
            var count = this.sut.Trim(record, new[] { "name" });

            // Assert
            Assert.Equal(0, count);
            this.orgSvcMock.Verify(o => o.Update(It.IsAny<Entity>()), Times.Never);
        }

        /// <summary>
        /// Tests that an empty field list is rejected before the service is called.
        /// </summary>
        [Fact]
        public void Trim_WithNoFields_ThrowsWithoutCallingTheService()
        {
            // Act and assert
            Assert.Throws<ArgumentException>(() => this.sut.Trim(new EntityReference("thing", Guid.NewGuid()), new string[0]));
            this.orgSvcMock.VerifyNoOtherCalls();
        }

        private void SetupRecord(EntityReference reference, Entity record)
        {
            this.orgSvcMock
                .Setup(o => o.Retrieve(reference.LogicalName, reference.Id, It.IsAny<ColumnSet>()))
                .Returns(record);
        }
    }
}
```

Code: `src/common/Defra.Imports.BusinessLogic/Text/TrimTextFieldsBusinessLogic.cs`, added to `Defra.Imports.BusinessLogic.projitems`. It depends on abstractions only and does not know about workflows.

```csharp
namespace Defra.Imports.BusinessLogic.Text
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Defra.Imports.BusinessLogic.Logging;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;

    /// <summary>
    /// Removes leading and trailing white space from the named text fields of a record.
    /// The table and the fields are inputs, so the class works for any table.
    /// </summary>
    public sealed class TrimTextFieldsBusinessLogic
    {
        private readonly IOrganizationService orgSvc;
        private readonly ILogWriter logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrimTextFieldsBusinessLogic"/> class.
        /// </summary>
        /// <param name="orgSvc">The organization service.</param>
        /// <param name="logWriter">The log writer.</param>
        public TrimTextFieldsBusinessLogic(IOrganizationService orgSvc, ILogWriter logWriter)
        {
            this.orgSvc = orgSvc ?? throw new ArgumentNullException(nameof(orgSvc));
            this.logWriter = logWriter ?? throw new ArgumentNullException(nameof(logWriter));
        }

        /// <summary>
        /// Trims the named fields of the record, writing one update that holds only the fields that changed.
        /// Writes nothing when no field needs trimming.
        /// </summary>
        /// <param name="record">The record to trim.</param>
        /// <param name="fieldNames">The logical names of the text fields.</param>
        /// <returns>The number of fields that were trimmed.</returns>
        public int Trim(EntityReference record, IReadOnlyCollection<string> fieldNames)
        {
            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            if (fieldNames == null || fieldNames.Count == 0)
            {
                throw new ArgumentException("At least one field name is required.", nameof(fieldNames));
            }

            var current = this.orgSvc.Retrieve(record.LogicalName, record.Id, new ColumnSet(fieldNames.ToArray()));
            var update = new Entity(record.LogicalName, record.Id);

            foreach (var fieldName in fieldNames)
            {
                var value = current.GetAttributeValue<string>(fieldName);
                if (value != null && value != value.Trim())
                {
                    update[fieldName] = value.Trim();
                }
            }

            if (update.Attributes.Count == 0)
            {
                this.logWriter.Log(Severity.Info, nameof(TrimTextFieldsBusinessLogic), $"{record.LogicalName} {record.Id} needs no trimming.");
                return 0;
            }

            this.orgSvc.Update(update);
            return update.Attributes.Count;
        }
    }
}
```

## 2. Activity

Test: `tests/Defra.Imports.UnitTests/Workflows/Text/TrimTextFieldsTests.cs`. `WorkflowActivityTests<T>` provides the invoker and the mocks.

```csharp
namespace Defra.Imports.UnitTests.Workflows.Text
{
    using System;
    using System.Collections.Generic;
    using Defra.Imports.Workflows.Text;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Query;
    using Moq;
    using Xunit;

    /// <summary>
    /// Unit tests for <see cref="TrimTextFields"/>.
    /// </summary>
    public sealed class TrimTextFieldsTests : WorkflowActivityTests<TrimTextFields>
    {
        /// <summary>
        /// Tests that the fields named in the input are trimmed and the count is returned as an output.
        /// </summary>
        [Fact]
        public void Execute_WithAFieldList_TrimsThoseFieldsAndReturnsTheCount()
        {
            // Arrange
            var record = new EntityReference("thing", Guid.NewGuid());
            this.OrgSvcMock
                .Setup(o => o.Retrieve("thing", record.Id, It.IsAny<ColumnSet>()))
                .Returns(new Entity("thing", record.Id) { ["name"] = " a ", ["code"] = " b ", ["other"] = " c " });
            Entity updated = null;
            this.OrgSvcMock.Setup(o => o.Update(It.IsAny<Entity>())).Callback<Entity>(e => updated = e);

            // Act
            var outputs = this.WorkflowInvoker.Invoke(GetInputs(record, "name, code"));

            // Assert
            Assert.Equal(2, outputs[nameof(TrimTextFields.TrimmedFieldCount)]);
            Assert.Equal("a", updated["name"]);
            Assert.Equal("b", updated["code"]);
            Assert.False(updated.Contains("other"));
        }

        /// <summary>
        /// Tests that a missing required input fails.
        /// </summary>
        [Fact]
        public void Execute_WithoutARecord_Throws()
        {
            // Act and assert
            Assert.ThrowsAny<Exception>(() => this.WorkflowInvoker.Invoke(GetInputs(null, "name")));
        }

        private static IDictionary<string, object> GetInputs(EntityReference record, string fields)
        {
            return new Dictionary<string, object>
            {
                { nameof(TrimTextFields.Record), record },
                { nameof(TrimTextFields.FieldNames), fields },
            };
        }
    }
}
```

Code: `src/pluginassemblies/Defra.Imports.Workflows/Text/TrimTextFields.cs`. The workflow head includes new files automatically.

The base class is written in full because the `using Defra.Imports.BusinessLogic;` that `[CrmPluginRegistration]` needs also brings in a different `WorkflowActivity`, and the build then fails with CS0507.

The record input has no `[ReferenceTarget]` because the activity is deliberately table-agnostic. For a table-specific activity, add `[ReferenceTarget(<Entity>.EntityLogicalName)]`.

```csharp
namespace Defra.Imports.Workflows.Text
{
    using System;
    using System.Activities;
    using System.Linq;
    using Defra.Imports.BusinessLogic;
    using Defra.Imports.BusinessLogic.Extensions;
    using Defra.Imports.BusinessLogic.Logging;
    using Defra.Imports.BusinessLogic.Text;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>
    /// Trims the named text fields of a record. The record and the fields are inputs, so a maker can use the
    /// activity for any table without a code change.
    /// </summary>
    [CrmPluginRegistration(
        nameof(TrimTextFields),
        "Trim text fields",
        "Removes leading and trailing white space from the named text fields of a record",
        "Defra.Imports.Workflows.Text",
        IsolationModeEnum.Sandbox)]
    public class TrimTextFields : Defra.Imports.Workflows.WorkflowActivity
    {
        /// <summary>
        /// Gets or sets the record to trim.
        /// </summary>
        [Input("Record")]
        [RequiredArgument]
        public InArgument<EntityReference> Record { get; set; }

        /// <summary>
        /// Gets or sets the logical names of the text fields to trim, separated by commas.
        /// </summary>
        [Input("Field names (comma separated)")]
        [RequiredArgument]
        public InArgument<string> FieldNames { get; set; }

        /// <summary>
        /// Gets or sets the number of fields that were trimmed.
        /// </summary>
        [Output("Trimmed field count")]
        public OutArgument<int> TrimmedFieldCount { get; set; }

        /// <inheritdoc/>
        internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)
        {
            var record = this.Record.GetRequired(context, nameof(this.Record));
            var fieldNames = this.FieldNames.GetRequired(context, nameof(this.FieldNames))
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(fieldName => fieldName.Trim())
                .ToList();

            var count = new TrimTextFieldsBusinessLogic(orgSvc, logWriter).Trim(record, fieldNames);

            this.TrimmedFieldCount.Set(context, count);
        }
    }
}
```

## Where configuration can come from

Choose the home for each varying value in the design, in this order of preference:

1. An activity input, when the maker of each process knows the value (as above).
2. A mapping or configuration web resource passed as an input, parsed by `FieldMappingConfigParser` and applied by `GenericEntityMapper`, as `MapFieldsFromItahcToImportRecord` does, when the value is a set of mappings that the business maintains.
3. A `defraexp_configurationparameter` record read through `IConfigurationParameterRepository`, when it is a single environment-level switch or threshold.
4. Reference or seed data under `data`, when it is a set of rows that the logic looks up.
