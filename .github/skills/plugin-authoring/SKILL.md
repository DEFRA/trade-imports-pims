---
name: plugin-authoring
description: Write, change or review a Dataverse plug-in in src/pluginassemblies/Defra.Imports.Plugins and the business logic and unit tests behind it. Covers deciding whether code is justified over a workflow, action or flow, generalising the requirement to a capability and naming the plug-in for it, defining the step contract (message, stage, mode, filtering attributes, images) and its configuration, building test-first with SOLID, configuration-driven classes, the Plugin base class, shared-project inclusion, verification and the step registration hand-off. Use whenever a request adds or changes a plug-in or the business logic class a plug-in calls. Owned by the Developer agent.
---

# Plug-in authoring

Code standards are in [csharp-source.instructions.md](../../instructions/csharp-source.instructions.md) and [unit-tests.instructions.md](../../instructions/unit-tests.instructions.md). This skill is the procedure; it does not repeat the rules. Code to adapt is in [templates.md](./references/templates.md).

## When to use code

This repository prefers low-code. A plug-in needs a reason. Use one only when the requirement cannot be met by a workflow, action, business process flow or cloud flow, for example:

- it must change the behaviour of a managed action, or run on a message that workflows and flows do not support;
- it must run inside the transaction, in the pre-operation or post-operation stage, and a real-time workflow cannot do that;
- it is generic logic that is unlikely to change, such as a platform extension.

If the logic is a business process, say so in the design proposal and propose the declarative alternative. Do not write the plug-in until the user decides.

## Design principles for this skill

The Developer agent's Design Principles apply in full (SOLID, test-driven, configuration- and data-driven, future-proof). For a plug-in that means:

- **Name it for the capability.** `MapKeyDataPairToCommodityComplement`, not `PopulateAnimalNumber`. The first requirement is one instance of the capability, and it must read as one row of configuration. `PopulateAnimalNumber` is an example of what not to copy; copy its test style, not its name or its hard-coded column.
- **What varies is configuration.** Tables, lookups, columns, keys, thresholds and mappings go in the step's unsecure configuration or in data, never in constants in the class. [templates.md](./references/templates.md) shows a plug-in that is configured by step.
- **The plug-in stays a thin adapter** (single responsibility). The rules live in a business logic class that depends on abstractions and receives parsed, validated configuration.
- **Tests come first.** Each behaviour starts as a failing test.

## Procedure

1. **Generalise first.** Before the contract, restate the requirement as the general capability it is an instance of, and write down the proposed name. Search `Defra.Imports.BusinessLogic` and the plug-in head for an existing generic class, mapper or configuration mechanism to extend through data before designing a new one. Ask: "If a second, similar requirement arrived next month, would this plug-in serve it with only a new step and configuration?"
2. **Capture the step contract.** Agree each of these with the user, and ask for any that are missing.

   | Item | Decide |
   | --- | --- |
   | Capability and name | The general capability and the plug-in name that describes it |
   | Table | Logical name, and whether the model already has it |
   | Message | `Create`, `Update`, `Delete`, a custom action name, and so on |
   | Stage and mode | Pre-validation, pre-operation or post-operation; synchronous or asynchronous |
   | Filtering attributes | The attributes that trigger an `Update` step. Do not leave them empty without a reason |
   | Images | Pre or post image, and only the attributes that the logic reads |
   | Configuration | What varies, where it lives (step configuration, a configuration parameter, a mapping web resource or seed data), its format and how a bad value fails |
   | Behaviour | Inputs, outputs, writes, and what must not happen |
   | Failure | Throw (which rolls back the transaction) or log and continue, and the user-facing message |
   | Recursion | Whether the plug-in writes to its own table, and how it avoids a loop |

3. **Find the neighbours.** Look in `src/pluginassemblies/Defra.Imports.Plugins/<Table>` and `src/common/Defra.Imports.BusinessLogic/<Table or theme>` for similar plug-ins and for `Utils` such as `FieldMappingConfigParser` and `GenericEntityMapper`. `ParameterSetKeyDataPair/PopulateAnimalNumber.cs` with `AddSpeciesAnimalNumbers` and `PopulateAnimalNumberTests` shows the test style, but not the naming or the hard-coded column to copy.
4. **Check the model.** Every table and column that the code names at compile time must exist in `Defra.Imports.Model`. Look up generated names with `Get-DataverseEntityPropertyClrType.ps1`. If one is missing, stop and ask the user to regenerate the model. Names that are configuration are late-bound by design.
5. **Propose the design and wait for approval.** State the capability and name, the step contract, what is configuration and where it lives, the classes with their single responsibilities and abstractions, the ordered test list (simplest first, with a second-configuration test), and what the user must register in a development environment. Say how the design meets each design principle.
6. **Take the baseline.** Run `New-WarningBaseline.ps1` from `dotnet-build-verification` before the first C# edit.
7. **Build test-first, one behaviour at a time.**
   - Write the test, run it and confirm that it fails for the right reason (red).
   - Write the smallest code that passes it (green), then refactor while the tests stay green.
   - Order: settings and configuration parsing, then the business logic class in `Defra.Imports.BusinessLogic.<Area>` (add its `Compile` entry to `Defra.Imports.BusinessLogic.projitems`), then the thin plug-in class in `Defra.Imports.Plugins.<Table>`, deriving from `Plugin`, with a `(string, string)` constructor if it takes step configuration.
   - Put shared names in a constants class rather than keeping private copies.
8. **Verify.** Run the full `Test-Change.ps1` gate with `-PathFilter` covering the new and changed source and test files. Only a bare `RESULT PASS` with an empty `IN SCOPE` counts. Cover the checklist below.
9. **Report.** Include how the design meets the principles, the configuration to supply, and the step registration hand-off below.

## Step registration hand-off

The plug-in step itself is solution metadata. Do not write `SdkMessageProcessingSteps` XML. Tell the user, in the report:

- the message, primary entity, stage, mode, filtering attributes, execution order and images to register, matching the design;
- the exact unsecure configuration to paste into the step, and the format that it takes. A second use of the same capability is another step with its own configuration, not another plug-in;
- to register the step in their development environment against the plug-in assembly, and to synchronise the solution metadata with `pipelines/sync-solution-metadata.yml`;
- that code outside the solution metadata folder needs `+solutionVer(defra_Imports): <major|minor|patch>` in the commit body.

## Test checklist

- Tests were written first, and each was seen to fail before its code existed.
- A test supplies a second, different configuration and shows that the behaviour changes with no code change.
- Missing, malformed or incomplete configuration fails with a clear message and writes nothing.
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
- Do not bake one use case into a generic plug-in's name, constants or `[CrmPluginRegistration]` attribute.
- Do not add a flag, mode or `if` for each new requirement. When the logic cannot express it through configuration, add a new small strategy or class chosen by configuration (open/closed).
