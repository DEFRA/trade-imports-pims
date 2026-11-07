---
name: cwa-authoring
description: Write, change or review a Dataverse custom workflow activity (CWA) in src/pluginassemblies/Defra.Imports.Workflows and the business logic and unit tests behind it. Covers deciding whether a CWA is justified, generalising the requirement to a capability and naming the activity for it, designing the inputs and outputs that makers see as its configuration, building test-first with SOLID classes, the WorkflowActivity base class, shared-project inclusion, verification and the registration hand-off. Use whenever a request adds or changes a custom workflow activity or the business logic class that one calls. Owned by the Developer agent.
---

# Custom workflow activity authoring

Code standards are in [csharp-source.instructions.md](../../instructions/csharp-source.instructions.md) and [unit-tests.instructions.md](../../instructions/unit-tests.instructions.md). This skill is the procedure; it does not repeat the rules. Code to adapt is in [templates.md](./references/templates.md).

## When to use a CWA

Business logic belongs in declarative components. Atomic processes are encapsulated as actions, and a CWA is a building block that those components call. A CWA is justified when:

- a workflow or action needs a capability that the platform does not offer, such as a calculation, a lookup that conditions cannot express or a call that needs code;
- it is generic and reusable, so that a maker can use it in many processes by changing its inputs.

Make a CWA granular and configurable. Do not write one that embeds a single business process; that process belongs in the workflow or action that calls generic activities. If the request describes a business process, say so in the design proposal.

## Design principles for this skill

The Developer agent's Design Principles apply in full (SOLID, test-driven, configuration- and data-driven, future-proof). For a CWA that means:

- **Name it for the capability, and expose what varies as inputs.** A CWA's inputs are its configuration, and makers change them without a release. `TrimTextFields` with a record and a list of fields, not `TrimItahcName`. See `MapFieldsFromItahcToImportRecord`, which takes its mapping as an input.
- **One activity, one job** (single responsibility). Prefer several small activities that makers combine to one large one with many modes. Add a new activity, or a new input with a safe default, rather than a flag that changes what an existing one does (open/closed).
- **The activity is a thin adapter.** It reads inputs, calls a business logic class that depends on abstractions, and sets outputs.
- **Tests come first.** Each behaviour starts as a failing test.

## Procedure

1. **Generalise first.** Restate the requirement as the general capability it is an instance of, and write down the proposed name. Search `Defra.Imports.Workflows` and `Defra.Imports.BusinessLogic` for an existing generic activity or utility to extend through its inputs before designing a new one. Ask: "If a second, similar requirement arrived next month, could a maker meet it with this activity and different inputs?"
2. **Capture the contract.**

   | Item | Decide |
   | --- | --- |
   | Capability and name | The general capability and the activity name that describes it |
   | Inputs | Name, type, whether required, and for a table-specific lookup the target table (`[ReferenceTarget]`). Everything that varies is an input |
   | Configuration beyond inputs | Anything that is not per-process (a mapping web resource, a configuration parameter, seed data), where it lives, its format and how a bad value fails |
   | Outputs | Name and type, and what a workflow or action reads from them |
   | Effects | Records read and written, and whether it writes at all |
   | Failure | Throw (which fails the workflow and surfaces a message) or log and continue |
   | Display text | Registration name, friendly name, description and group, which makers see. Describe the capability, not the first use |

3. **Find the neighbours.** Look in `src/pluginassemblies/Defra.Imports.Workflows/<Area>` and the matching business logic folder. `ImportApplication/MapFieldsFromItahcToImportRecord.cs` (mapping resource as an input), `ImportApplication/PopulateCountriesOfTransitFromNotification.cs` (lookups in, logic class) and `Utils/CountRecordsByFetchXml.cs` (output) are good examples of the shape.
4. **Check the model** for every table and column that the code names at compile time, as in `plugin-authoring`. Ask the user to regenerate the model if one is missing. Names that are inputs are late-bound by design.
5. **Propose the design and wait for approval.** State the capability and name, the contract, the classes with their single responsibilities and abstractions, the ordered test list (simplest first, with a second-inputs test) and the registration hand-off. Say how the design meets each design principle.
6. **Take the baseline** with `New-WarningBaseline.ps1` before the first C# edit.
7. **Build test-first, one behaviour at a time.**
   - Write the test, run it and confirm that it fails for the right reason (red). Write the smallest code that passes (green), then refactor.
   - Order: the business logic class in `Defra.Imports.BusinessLogic.<Area>` (with its `Compile` entry in the `.projitems`), tested directly; then the activity in `Defra.Imports.Workflows.<Area>`, deriving from `Defra.Imports.Workflows.WorkflowActivity`, tested with `WorkflowActivityTests<TCodeActivity>`.
8. **Verify** with the full `Test-Change.ps1` gate and `-PathFilter` for the new and changed files, and cover the checklist below.
9. **Report,** including how the design meets the principles, the inputs makers will supply, and the hand-off below.

## Registration hand-off

A custom workflow activity is registered with its plug-in assembly, and its arguments are read from the assembly. Workflows, actions and flows that use it are solution metadata. Tell the user, in the report:

- the activity's name, display name and group as given in the attribute;
- that the new or changed arguments appear in the process designer only after the assembly is updated in their development environment, and that any workflow or action that uses it is changed there and synchronised with `pipelines/sync-solution-metadata.yml`;
- that changing or removing an argument breaks processes that use it. Prefer adding an optional input, and flag a breaking change before making it;
- that code outside the solution metadata folder needs `+solutionVer(defra_Imports): <major|minor|patch>` in the commit body.

## Test checklist

- Tests were written first, and each was seen to fail before its code existed.
- A test supplies a second, different set of inputs and shows that the behaviour changes with no code change.
- Each required input missing produces the expected failure, as `GetRequired` throws `ArgumentNullException`.
- Each output is set to the expected value.
- The organisation service receives the expected calls, and no calls when there is nothing to do.
- Error handling matches the design.
- Test inputs use the property names of the arguments as dictionary keys.
- The new test files add no analyser warnings.

## Pitfalls

- Activities are cached and shared across executions. Keep them stateless; read arguments from the `CodeActivityContext` on each call.
- Read arguments with `context.GetValue(...)`, or `GetRequired` from `Defra.Imports.BusinessLogic.Extensions` for a required one. Do not read an `InArgument` directly, or from a field.
- A lookup argument has no table check without `[ReferenceTarget]`. Add it so that makers can only bind the right table.
- The workflow service is created for the workflow's user (`workflowContext.UserId`). Do not change the identity.
- `Defra.Imports.BusinessLogic` also contains a `WorkflowActivity` class, and the `using` that `[CrmPluginRegistration]` needs imports it. Derive from `Defra.Imports.Workflows.WorkflowActivity` written in full, or the build fails with CS0507. Do not use `WorkFlowActivityBase`.
- Changing an argument's name or type is a breaking change for processes that use it.
- Do not hard-code a table, column, key or threshold that a maker might want to change; make it an input.
- Do not add a boolean input that switches the activity into a different job. Split it into two activities.
