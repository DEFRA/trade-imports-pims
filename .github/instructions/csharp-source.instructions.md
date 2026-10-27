---
description: "Use when writing, reviewing or refactoring C# under src: Dataverse plug-ins and custom workflow activities, their handlers, application services, Dataverse queries, projections and extensions, constants and composition roots. Covers the ADR 0001 layers, behaviour preservation, never mutating inputs, early-bound LINQ queries, narrow input contexts, shared-project inclusion and analyser rules."
applyTo: "src/**/*.cs"
---

# C# Source Standards

[ADR 0001](../../architecture/adr/0001-plugin-workflow-di-scaffold.md) is the source of truth for how plug-ins and custom workflow activities are structured. These rules apply it at code level. If they conflict with the ADR, follow the ADR and raise the conflict.

The ADR's "Adoption Record" sections describe the reference implementations: `PopulateAnimalNumber`, `FlagRecordOnWatchList` and `FlagRecordOnWatchListNotification`. The watch list initialisation example (`WatchListInitialiser` and related types) is illustrative only. Do not extend it or copy it.

## Scope

- Apply these rules to new code and to code you migrate. Do not refactor unrelated legacy code (`Defra.Imports.BusinessLogic`, `Defra.Imports.Repositories` and unmigrated plug-ins and activities) as a side effect of another change.
- `src/common/Defra.Imports.Dataverse/Model` is generated. Never edit it, and never add hand-written types to the `Defra.Imports.Dataverse.Model` namespace.
- If a table or column you need is missing from the generated model, stop and ask for the model to be regenerated. The table is added to `Model/builderSettings.json` and the **Generate model** VS Code task is run. Do not work around the gap with late-bound queries, hand-written schema names or "regenerate later" comments.

## Platform constraints

- Do not use `async`, `Task`, parallel or threading code in plug-ins, activities or anything they call. See [Platform constraints](../../architecture/adr/0001-plugin-workflow-di-scaffold.md#platform-constraints).
- Plug-in and activity instances are cached and invoked concurrently. Keep them stateless, and never hold per-invocation objects in fields, statics or a composition root.
- The heads and shared projects target net462 with the default language version, C# 7.3. Do not use `record` types, switch expressions, `using` declarations, target-typed `new` or nullable reference types.
- An unqualified `IOrganizationService` or `ImportsContext` acts as the executing user. Do not change identity or elevate privileges without a reviewed design. See [Execution identity](../../architecture/adr/0001-plugin-workflow-di-scaffold.md#execution-identity).

## Behaviour

- When you restructure existing code, its behaviour is frozen unless a change is explicitly approved. This covers outputs, target mutations, database effects and write timing, exceptions and messages, and catch-and-continue behaviour. See [Confirmed migration constraints](../../architecture/adr/0001-plugin-workflow-di-scaffold.md#confirmed-migration-constraints).
- Do not fix suspected bugs. Do not add validation, guards, null checks or failure modes that the existing code did not have, other than a handler's step-contract validation.
- Mark each suspected bug or bad practice that you preserve with a `TODO:` comment ending "should be investigated, but is preserved by the refactoring". For example: `// TODO: DateTime.Now is server local time, not UTC. This should be investigated, but is preserved by the refactoring.`
- Do not add retries, transaction wrappers, deferred writes or `SaveChanges()`. Transaction control stays with Dataverse.

## Layers

| Layer | Home | Holds |
| --- | --- | --- |
| Entry point | `Defra.Imports.Plugins.<Area>` or `Defra.Imports.Workflows.<Area>` | The host class only |
| Handler | Beside its entry point, in the same namespace and head | Host adaptation; no business rules |
| Application service | `Defra.Imports.Application.<Area>` | Reusable use-case logic, named for the use case |
| Queries | `Defra.Imports.Dataverse.Queries` | Read-only query classes and their multi-table result types |
| Input contexts | `Defra.Imports.Dataverse.Projections` and `Defra.Imports.Dataverse.Extensions` | Narrow immutable inputs and the projections onto them |
| Constants | `Defra.Imports.Application.Constants` and `.ReferenceData`; plug-in platform constants in `Defra.Imports.Infrastructure.Plugins` | Shared values |

See [Layer ownership](../../architecture/adr/0001-plugin-workflow-di-scaffold.md#layer-ownership) for the reasoning.

### Entry points

- An existing entry point keeps its class name, namespace and assembly, because the registered steps refer to them. Name a new one for the utility it provides (for example `WatchListInitialisationPlugin`), not for a message or stage.
- A plug-in class is `public sealed class X : PluginBase<XHandler>` with a public parameterless constructor that calls `base(PluginCompositionRoot.Root)`. A workflow activity derives from `WorkflowActivityBase<THandler>` instead. See [The composition model](../../architecture/adr/0001-plugin-workflow-di-scaffold.md#the-composition-model).
- The solution metadata in `src/solutions/defra_Imports/src/SdkMessageProcessingSteps` defines the step registrations, not code attributes. Remove `[CrmPluginRegistration]` attributes when you migrate a class, and do not edit the registration XML by hand.

### Handlers

- Validate the step contract and throw `InvalidOperationException` on a mismatch. The contract covers the primary entity, stage, mode, message (compared with `StringComparison.OrdinalIgnoreCase`) and the `Target` or images used.
- Copy the entity before use (`target.ToEntity<T>()` returns a copy) and project it if the service takes a context. Then make one or two application calls and apply the result as the host requires.
- Hold no business rules, queries or private platform constants.
- Take `ILogWriter` only if the handler logs. Catch only where the behaviour being preserved catches. Do not depend on `ILogger` in anything a workflow might use.
- Examples: `FlagRecordOnWatchListHandler` (no catch) and `PopulateAnimalNumberHandler` (preserved catch-and-continue).

### Application services

- Define an interface `I<UseCase>` and a sealed implementation `<UseCase>`, named for the generic use case rather than the plug-in or table (for example `IWatchFlagCreator`).
- Give each class exactly one public constructor, taking query classes, `IOrganizationService` or `ImportsContext` and, if it logs, `ILogWriter`. Null-check each argument with `?? throw new ArgumentNullException(nameof(argument))`.
- Do not depend on plug-in or workflow types such as `IPluginExecutionContext`, `IPluginExecution`, `CodeActivityContext` or handlers.
- Keep rule data, such as maps from keys to columns, private to the class.
- Make persistence explicit in the method contract and its XML documentation. A method that writes issues its own `Create` or `Update`. A `Get…` method returns a new entity and writes nothing.
- Never mutate an entity you are given. Clone it with `EntityExtensions.Clone`, change the clone and return it. `Clone` is shallow, so replace `EntityReference`, `OptionSetValue` and `EntityCollection` values rather than changing them.

### Input contexts

- When a service reads only a few attributes, pass it a narrow context rather than the whole typed entity.
- The context is a `public sealed class` in `Defra.Imports.Dataverse.Projections`. It has one constructor and get-only properties holding only what the service reads.
- Project onto it with an extension method on the typed entity, such as `ToCreateWatchFlagsContext()`. Put the method in that entity's own `<Entity>Extensions` class in `Defra.Imports.Dataverse.Extensions`, and never combine several entities in one extensions class.
- The projection copies values and does not check that they are present. A `Create` `Target` holds only the attributes that were supplied, so the rest must project as null.
- When a service creates several records from one context, give each record its own `EntityReference` instance.
- Examples: `CreateItahcWatchFlagsContext` and `ItahcExtensions`.

### Queries

- A query class takes `ImportsContext` and sets `MergeOption.NoTracking` in its constructor, because its queries are read-only.
- Use early-bound LINQ that selects only the columns needed (`select new T { … }`). Prefer one query per table. When a business key, such as the watch type, chooses the table, switch on it.
- Use `QueryExpression`, FetchXML or late-bound `CreateQuery(name)` only when LINQ cannot express the query, and say why in a comment.
- Name tables and columns with `<Entity>.EntityLogicalName` and `<Entity>.Fields.<Name>`, never string literals. Generated property casing varies between tables (`defraimp_Name`, `defraimp_name`), so look it up rather than guess:
  `pwsh -NoProfile -Command "& ./.github/skills/scenario-builder-implementation/scripts/Get-DataverseEntityPropertyClrType.ps1 -EntityLogicalName defraimp_itahc -AttributeName defraimp_name,defraimp_consigneename"`
- Document paging and ordering assumptions in the XML documentation, for example "only the first page is used".
- Example: `WatchFlagQueries`.

### Constants

- Take message, stage, mode and input-parameter names from `PluginMessages`, `PluginStages`, `PluginModes` and `PluginInputParameters` in `Defra.Imports.Infrastructure.Plugins`. Add a missing platform value there and pin it in `PluginConstantsTests`.
- Put business keys in a dedicated file in `Defra.Imports.Application.Constants` (for example `KeyDataPairKeys`), and reference-data identifiers in `Defra.Imports.Application.ReferenceData`.
- Do not keep private copies of these values in handlers or services.

## Composition and shared projects

- Register every new handler, service and query class in `PluginCompositionRoot.BuildRoot()` or `WorkflowCompositionRoot`. Each head has one root; never add another. Services resolve as transients from the execution scope, so every implementation needs exactly one public constructor.
- `Defra.Imports.Application` and `Defra.Imports.Dataverse` compile `**\*.cs`, so new files are included automatically. `Defra.Imports.Infrastructure` and `Defra.Imports.BusinessLogic` list their files in their `.projitems`. Add an entry for each new file and remove the entry for each deleted one.
- Shared code compiles into both heads. After you move a namespace, fix every `using` (keeping them in alphabetical order, SA1210) and build both `Defra.Imports.Plugins` and `Defra.Imports.Workflows`.

## Style and analysers

- StyleCop and Creedengo analyse every project. New and changed code must add no warnings.
- Do not add suppressions (`#pragma warning disable`, `[SuppressMessage]` or `.editorconfig` severity changes) without explicit approval.
- Put `using` directives inside the namespace, prefix instance members with `this.`, and document public types and members.
- Write documentation and comments in British English, except where StyleCop mandates wording. For example, constructor summaries read `Initializes a new instance of the <see cref="X"/> class.`
- Comment only what needs clarifying.
