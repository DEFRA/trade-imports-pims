---
name: cwa-authoring
description: Write, change or review a Dataverse custom workflow activity (CWA) in src/pluginassemblies/Defra.Imports.Workflows and the business logic and unit tests behind it. Covers deciding whether a CWA is justified, designing the inputs and outputs that makers see, the WorkflowActivity base class, the CrmPluginRegistration attribute, shared-project inclusion, unit tests with WorkflowActivityTests, verification and the registration hand-off. Use whenever a request adds or changes a custom workflow activity or the business logic class that one calls. Owned by the Developer agent.
---

# Custom workflow activity authoring

Code standards are in [csharp-source.instructions.md](../../instructions/csharp-source.instructions.md) and [unit-tests.instructions.md](../../instructions/unit-tests.instructions.md). This skill is the procedure; it does not repeat the rules. Code to adapt is in [templates.md](./references/templates.md).

## When to use a CWA

Business logic belongs in declarative components. Atomic processes are encapsulated as actions, and a CWA is a building block that those components call. A CWA is justified when:

- a workflow or action needs a capability that the platform does not offer, such as a calculation, a lookup that conditions cannot express or a call that needs code;
- it is generic and reusable, so that a maker can use it in many processes by changing its inputs.

Make a CWA granular and configurable. Do not write one that embeds a single business process; that process belongs in the workflow or action that calls generic activities. If the request describes a business process, say so in the design proposal.

## Procedure

1. **Capture the contract.**

   | Item | Decide |
   | --- | --- |
   | Purpose | One sentence, stated generically so that other processes can reuse it |
   | Inputs | Name, type, whether required, and for lookups the target table (`[ReferenceTarget]`) |
   | Outputs | Name and type, and what a workflow or action reads from them |
   | Effects | Records read and written, and whether it writes at all |
   | Failure | Throw (which fails the workflow and surfaces a message) or log and continue |
   | Display text | Registration name, friendly name, description and group, which makers see |

2. **Find the neighbours.** Look in `src/pluginassemblies/Defra.Imports.Workflows/<Area>` and the matching business logic folder. `ImportApplication/PopulateCountriesOfTransitFromNotification.cs` (lookups in, logic class) and `Utils/CountRecordsByFetchXml.cs` (output) are good examples.
3. **Check the model** for every table and column, as in `plugin-authoring`. Ask the user to regenerate the model if one is missing.
4. **Propose the design and wait for approval.** State the contract, the classes to add or change, the tests and the registration hand-off.
5. **Take the baseline** with `New-WarningBaseline.ps1` before the first C# edit.
6. **Implement.**
   - Put the rules in a business logic class in `Defra.Imports.BusinessLogic.<Area>`, with its `Compile` entry in the `.projitems`.
   - Add the activity in `Defra.Imports.Workflows.<Area>`, deriving from `WorkflowActivity`, with its `[CrmPluginRegistration]` attribute.
7. **Write the tests.** Derive from `WorkflowActivityTests<TCodeActivity>` and cover the checklist below. Test the business logic class on its own as well.
8. **Verify** with the full `Test-Change.ps1` gate and `-PathFilter` for the new and changed files.
9. **Report,** including the hand-off below.

## Registration hand-off

A custom workflow activity is registered with its plug-in assembly, and its arguments are read from the assembly. Workflows, actions and flows that use it are solution metadata. Tell the user, in the report:

- the activity's name, display name and group as given in the attribute;
- that the new or changed arguments appear in the process designer only after the assembly is updated in their development environment, and that any workflow or action that uses it is changed there and synchronised with `pipelines/sync-solution-metadata.yml`;
- that changing or removing an argument breaks processes that use it. Prefer adding an optional input, and flag a breaking change before making it;
- that code outside the solution metadata folder needs `+solutionVer(defra_Imports): <major|minor|patch>` in the commit body.

## Test checklist

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
