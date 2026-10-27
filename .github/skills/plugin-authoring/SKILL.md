---
name: plugin-authoring
description: Write, change or review a Dataverse plug-in in src/pluginassemblies/Defra.Imports.Plugins and the business logic and unit tests behind it. Covers deciding whether code is justified over a workflow, action or flow, defining the step contract (message, stage, mode, filtering attributes, images), the Plugin base class, the CrmPluginRegistration attribute, shared-project inclusion, unit tests, verification and the step registration hand-off. Use whenever a request adds or changes a plug-in or the business logic class a plug-in calls. Owned by the Developer agent.
---

# Plug-in authoring

Code standards are in [csharp-source.instructions.md](../../instructions/csharp-source.instructions.md) and [unit-tests.instructions.md](../../instructions/unit-tests.instructions.md). This skill is the procedure; it does not repeat the rules. Code to adapt is in [templates.md](./references/templates.md).

## When to use code

This repository prefers low-code. A plug-in needs a reason. Use one only when the requirement cannot be met by a workflow, action, business process flow or cloud flow, for example:

- it must change the behaviour of a managed action, or run on a message that workflows and flows do not support;
- it must run inside the transaction, in the pre-operation or post-operation stage, and a real-time workflow cannot do that;
- it is generic logic that is unlikely to change, such as a platform extension.

If the logic is a business process, say so in the design proposal and propose the declarative alternative. Do not write the plug-in until the user decides.

## Procedure

1. **Capture the step contract.** Agree each of these with the user, and ask for any that are missing.

   | Item | Decide |
   | --- | --- |
   | Table | Logical name, and whether the model already has it |
   | Message | `Create`, `Update`, `Delete`, a custom action name, and so on |
   | Stage and mode | Pre-validation, pre-operation or post-operation; synchronous or asynchronous |
   | Filtering attributes | The attributes that trigger an `Update` step. Do not leave them empty without a reason |
   | Images | Pre or post image, and only the attributes that the logic reads |
   | Behaviour | Inputs, outputs, writes, and what must not happen |
   | Failure | Throw (which rolls back the transaction) or log and continue, and the user-facing message |
   | Recursion | Whether the plug-in writes to its own table, and how it avoids a loop |

2. **Find the neighbours.** Look in `src/pluginassemblies/Defra.Imports.Plugins/<Table>` and `src/common/Defra.Imports.BusinessLogic/<Table or theme>` for similar plug-ins. `ParameterSetKeyDataPair/PopulateAnimalNumber.cs` with `AddSpeciesAnimalNumbers` and `PopulateAnimalNumberTests` is a recent example.
3. **Check the model.** Every table and column must exist in `Defra.Imports.Model`. Look up generated names with `Get-DataverseEntityPropertyClrType.ps1`. If one is missing, stop and ask the user to regenerate the model.
4. **Propose the design and wait for approval.** State the step contract, the classes to add or change, the tests, and what the user must register in a development environment.
5. **Take the baseline.** Run `New-WarningBaseline.ps1` from `dotnet-build-verification` before the first C# edit.
6. **Implement.**
   - Put the rules in a business logic class in `Defra.Imports.BusinessLogic.<Area>`. Add its `Compile` entry to `Defra.Imports.BusinessLogic.projitems`.
   - Add the thin plug-in class in `Defra.Imports.Plugins.<Table>`, deriving from `Plugin`, with one `[CrmPluginRegistration]` attribute per step.
   - Put shared names in a constants class rather than keeping private copies.
7. **Write the tests.** Test the business logic class directly, and the plug-in through `IServiceProvider`. Cover the checklist below.
8. **Verify.** Run the full `Test-Change.ps1` gate with `-PathFilter` covering the new and changed source and test files. Only a bare `RESULT PASS` with an empty `IN SCOPE` counts.
9. **Report.** Include the step registration hand-off below.

## Step registration hand-off

The plug-in step itself is solution metadata. Do not write `SdkMessageProcessingSteps` XML. Tell the user, in the report:

- the message, primary entity, stage, mode, filtering attributes, execution order and images to register, matching the `[CrmPluginRegistration]` attribute;
- to register the step in their development environment against the plug-in assembly, and to synchronise the solution metadata with `pipelines/sync-solution-metadata.yml`;
- that code outside the solution metadata folder needs `+solutionVer(defra_Imports): <major|minor|patch>` in the commit body.

## Test checklist

- Each message, stage or image variation that the plug-in handles has a test.
- The plug-in does nothing when the target lacks the triggering attribute or the condition is not met, and the organisation service receives no write.
- An `Update` target holds only the changed attributes, so logic that needs other values reads them from the pre-image. Test with a target that lacks them.
- A missing pre-image or lookup is handled as the design says.
- Error handling matches the design: an exception propagates, or it is logged and execution continues.
- Writes are asserted by the entity and attributes passed to `Create` or `Update`, and the input target is not mutated unless that is the purpose.
- The new test files add no analyser warnings.

## Pitfalls

- Pre-operation changes to the `Target` are saved with the operation; a post-operation step needs an explicit `Update`.
- An `Update` on the same table can trigger the plug-in again. Constrain the filtering attributes and check `context.Depth` where a loop is possible.
- `Target` on `Create` holds only the attributes that were supplied. Do not assume the rest hold defaults.
- A lookup set through `@odata.bind` has no `Name` on its `EntityReference`. Retrieve the record if you need the name.
- For a custom message, take the name from `CustomActionNames` and use `IsRootContextCustomMessage` rather than a string literal.
- Do not catch and swallow an exception unless the design says that the plug-in must not block the operation.
