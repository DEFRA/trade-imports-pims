---
description: "Use when writing, reviewing or refactoring C# under src: Dataverse plug-ins and custom workflow activities, the business logic and repository classes they call, and the shared projects they compile. Covers the plug-in and workflow base classes, step and activity registration attributes, the generated early-bound model, shared-project inclusion, behaviour preservation and analyser rules."
applyTo: "src/**/*.cs"
---

# C# Source Standards

These rules apply to the plug-in assemblies and the shared projects they compile. Follow the conventions of the neighbouring code where these rules are silent.

## Solution layout

| Project | Path | Holds |
| --- | --- | --- |
| Plug-in head | `src/pluginassemblies/Defra.Imports.Plugins` | Plug-in classes (`Plugin`), one folder per table |
| Workflow head | `src/pluginassemblies/Defra.Imports.Workflows` | Custom workflow activities (`WorkflowActivity`), one folder per table or theme |
| `Defra.Imports.BusinessLogic` | `src/common` | Business logic classes, the `Plugin` base class, logging, constants and utilities (shared project) |
| `Defra.Imports.Repositories` | `src/common` | `CrmRepository` classes and organisation service helpers (shared project) |
| `Defra.Imports.Model` | `src/common` | The generated early-bound model and `ImportsContext` (shared project) |
| `Defra.Imports.Container` | `src/common` | A class library that compiles the three shared projects for the `UnitTest` configuration |

- Both heads compile the shared projects (`.shproj` and `.projitems`) in the `Debug` and `Release` configurations. In the `UnitTest` configuration they reference `Defra.Imports.Container` instead.
- `src/solutions/defra_Imports` holds the solution metadata. It is synchronised from a development environment; do not edit it as part of C# work.

## Design principles

Apply these to new code and to code you change. They are part of the definition of done, not preferences.

- **Single responsibility.** An entry point adapts the platform and calls one business logic class. A business logic class has one reason to change. Split a class that you describe with "and".
- **Open/closed.** Add behaviour with configuration, data or a new small class, not by editing a working one. Replace `if` and `switch` chains on a key with a lookup that data chooses.
- **Liskov and interface segregation.** An implementation honours the contract of its abstraction. Depend on the narrowest abstraction you need: a repository or mapper interface, or a value, rather than a whole service or entity.
- **Dependency inversion.** Business logic receives its dependencies (`IOrganizationService`, `ILogWriter`, repository and mapper interfaces) through its constructor. Never create them inside the class with `new`, and never read them from a static, so that a test can substitute them. The entry point is the composition point.
- **Configuration- and data-driven.** Anything that varies by table, column, key, threshold, message or environment is configuration, not code. The existing mechanisms are:
  - step unsecure and secure configuration (`UnsecureConfig` and `SecureConfig` on `Plugin`, set by the `Plugin(string, string)` constructor that the entry point must pass through);
  - custom workflow activity inputs;
  - `defraexp_configurationparameter` records, through `IConfigurationParameterRepository`;
  - XML mapping web resources, parsed by `FieldMappingConfigParser` and applied by `GenericEntityMapper`, as `MapFieldsFromOneRecordToAnotherBusinessLogic` does;
  - seed and reference data under `data`.
  Parse and validate configuration in one place, fail with a clear message on bad configuration, and cover it with tests. Extend an existing generic class (the mapper, the fetch template service) before writing a new one. Hard-code only what is structural.
- **Capability naming.** Name every class, file, namespace, step, argument and web resource for the capability, not the first use case. Use `MapKeyDataPairToCommodityComplement`, not `PopulateAnimalNumber`; the animal number is one row of configuration. When you touch a use-case-named class that you must keep, because registered steps refer to it, add the generic class and make the old one a thin adapter that supplies its configuration.
- **No speculative generality.** Generalise to the capability that the requirement plainly implies and no further. Add no extension point that nothing uses.

## Scope

- Make the smallest change that delivers the request. Do not refactor unrelated legacy code as a side effect of another change.
- `src/common/Defra.Imports.Model` is generated. Never edit it, and never add hand-written types to the `Defra.Imports.Model` namespace.
- If a table or column you need is missing from the generated model, stop and ask for the model to be regenerated. The table is added to `Model/builderSettings.json` and the **Generate model** VS Code task is run. Do not work around the gap with late-bound queries, hand-written schema names or "regenerate later" comments. (Names that are configuration or inputs are a different case; see Business logic and data access.)
- Prefer configuration over code. Business processes belong in workflows, actions and flows. Code belongs here only when it is generic, reusable and unlikely to change, or when Dataverse offers no declarative alternative. Keep it configuration-driven (see Design principles).
- Work test-first. Write a failing test for each behaviour before the code that satisfies it (see [unit-tests.instructions.md](./unit-tests.instructions.md)).

## Platform constraints

- Do not use `async`, `Task`, parallel or threading code in plug-ins, activities or anything they call. Sandbox plug-ins run synchronously within a time limit.
- Plug-in and activity instances are cached and invoked concurrently. Keep them stateless, and never hold per-invocation objects in fields or statics.
- The heads and shared projects target net462 with the default language version, C# 7.3. Do not use `record` types, switch expressions, `using` declarations, target-typed `new` or nullable reference types.
- Do not change the identity that the organisation service is created for, or elevate privileges, without a reviewed design.
- Do not add retries, transaction wrappers or deferred writes. Transaction control stays with Dataverse.

## Behaviour

- When you change existing code, its behaviour is frozen unless a change is explicitly approved. This covers outputs, target mutations, database effects and write timing, exceptions and their messages, and catch-and-continue behaviour.
- Do not fix suspected bugs you meet on the way, and do not add validation, guards or failure modes that the request does not ask for. Flag them in your report instead.
- Handle and log errors in the way that the surrounding code does. Do not add an empty `catch`.

## Plug-ins

- A plug-in is a `public class X : Plugin` in `Defra.Imports.Plugins.<Table>` that overrides `Execute(IPluginExecutionContext context, IOrganizationService orgSvc, TracingServiceLogWriter logWriter, RepositoryFactory repositoryFactory)`. `Plugin` is in `Defra.Imports.BusinessLogic`. Do not derive from the `PluginBase` class in the plug-in head.
- Keep the class thin. Read the `Target` and images from the context, read configuration (`this.UnsecureConfig`), call a business logic class, and apply the result. Put the rules in `Defra.Imports.BusinessLogic.<Table or Theme>`.
- Name the plug-in for the capability (see Design principles), and put what varies, such as column mappings and keys, in the step's unsecure configuration or in data. To receive step configuration, give the plug-in a `(string unsecureConfig, string secureConfig)` constructor that calls `base(unsecureConfig, secureConfig)` and keep the parameterless constructor.
- Read the target with `((Entity)context.InputParameters["Target"]).ToEntity<T>()`. Read images with `GetPreImage<T>` and `GetPostImage<T>`.
- Decorate the class with one `[CrmPluginRegistration]` attribute per step, as neighbouring plug-ins do. The attribute documents the step. The registration itself lives in the solution metadata under `src/solutions/defra_Imports/src/SdkMessageProcessingSteps`, so tell the user which step to register in a development environment; do not write that XML.
- Keep step scope narrow: set filtering attributes, and use an image only for the attributes that the logic reads. Avoid organisation-wide steps on out-of-the-box messages and entities.
- Guard against recursion where the plug-in updates its own table (check `context.Depth` or the changed attributes).

## Custom workflow activities

- An activity is a `public class X : Defra.Imports.Workflows.WorkflowActivity` in `Defra.Imports.Workflows.<Area>` that overrides `internal override void ExecuteWorkflowActivity(CodeActivityContext context, IWorkflowContext workflowContext, IOrganizationService orgSvc, ILogWriter logWriter)`. Write the base class in full: `Defra.Imports.BusinessLogic` also holds a `WorkflowActivity`, and the `using Defra.Imports.BusinessLogic;` that `[CrmPluginRegistration]` needs would otherwise win. Do not derive from the legacy `WorkFlowActivityBase`.
- Declare inputs and outputs as `InArgument<T>` and `OutArgument<T>` properties with `[Input]`, `[Output]` and `[RequiredArgument]`. Give each a clear display name, because it is what makers see. Add `[ReferenceTarget(<Entity>.EntityLogicalName)]` to a lookup input.
- Read inputs with `GetRequired(context, nameof(this.Argument))` (from `Defra.Imports.BusinessLogic.Extensions`) for a required value, or `context.GetValue(...)`. Write outputs with `Set(context, value)`.
- Decorate the class with `[CrmPluginRegistration(name, displayName, description, groupName, IsolationModeEnum.Sandbox)]`.
- Make activities generic, granular and reusable, so that a maker can combine them in a workflow, action or flow without a code change. Expose what varies (tables, fields, keys, thresholds, mapping resources) as inputs, as `MapFieldsFromItahcToImportRecord` does with its mapping web resource, and name the activity for the capability.

## Business logic and data access

- Business logic classes are `public` and `sealed` where possible, take their dependencies (`IOrganizationService`, `ILogWriter`, repository and mapper interfaces) in the constructor, null-check them, and expose one clear method per use case. Take configuration as a parsed, validated object (a dictionary, a small settings class) rather than reading it from inside the rules. Do not depend on plug-in or workflow types such as `IPluginExecutionContext` or `CodeActivityContext`, so that the class can be unit tested.
- Never mutate an entity that you were given unless that is the documented purpose of the method. Build a new entity for an `Update` that holds only the attributes that change.
- Use the early-bound model. Name tables and columns with `<Entity>.EntityLogicalName` and `<Entity>.Fields.<Name>`, never string literals, and use the generated option set enums rather than numbers. The exception is a table, column or key that is supplied by configuration or an input: a generic, configuration-driven class handles it late-bound (`Entity`, `EntityReference.LogicalName`), validates it, and fails clearly if it is wrong. This does not permit working around a gap in the generated model for a name that the code fixes at compile time.
- Generated property casing varies between tables (`defraimp_Name`, `defraimp_name`), so look it up rather than guess:
  `pwsh -NoProfile -Command "& ./.github/skills/scenario-builder-implementation/scripts/Get-DataverseEntityPropertyClrType.ps1 -EntityLogicalName defraimp_itahc -AttributeName defraimp_name,defraimp_consigneename"`
- Retrieve only the columns you need (`ColumnSet` or a LINQ projection). Use `CrmRepository<ImportsContext, T>` or the organisation service directly, following the neighbouring code. Use FetchXML only when `QueryExpression` and LINQ cannot express the query, and say why in a comment.
- Log through `ILogWriter` (`logWriter.Log(Severity, nameof(Class), message)`). Never log secrets or personal data.
- Put shared names (custom message names, business keys) in a constants class in `Defra.Imports.BusinessLogic`, not as private copies in several classes.

## Shared projects

- `Defra.Imports.Model` compiles `**\*.cs`, so new files are included automatically.
- `Defra.Imports.BusinessLogic` and `Defra.Imports.Repositories` list their files in their `.projitems`. Add a `Compile` entry for each new file and remove the entry for each deleted one. A stale entry breaks the build.
- Shared code compiles into both heads. After you move a namespace, fix every `using` (keeping them in alphabetical order, SA1210) and build both `Defra.Imports.Plugins` and `Defra.Imports.Workflows`.
- Add no references to the plug-in assemblies from the shared projects.

## Style and analysers

- StyleCop and Creedengo analyse every project, and the existing code already carries many warnings. New and changed code must add none.
- Do not add suppressions (`#pragma warning disable`, `[SuppressMessage]`, `.editorconfig` severity changes or `NoWarn`) without explicit approval.
- Put `using` directives inside the namespace, prefix instance members with `this.`, and document public types and members. SA1633 (file header) is disabled.
- Write documentation and comments in British English, except where StyleCop mandates wording. For example, constructor summaries read `Initializes a new instance of the <see cref="X"/> class.`
- Comment only what needs clarifying.
