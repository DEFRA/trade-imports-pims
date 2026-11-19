# Plug-in and custom workflow activity pattern

This guide explains how code-based plug-ins and custom workflow activities (CWAs) are structured in this repository, and how to migrate an existing one. The decision and its rationale are in [ADR 0001](../../architecture/adr/0001-plugin-workflow-di-scaffold.md). What each completed migration changed is in the [migration log](../../architecture/plugin-workflow-migration-log.md).

The repository prefers low-code. This pattern governs the code-based components that already exist and any that are genuinely needed; it is not a reason to write new business logic in C#.

## Reference implementations

Read a migrated slice before writing a new one. The source is the reference; this guide does not duplicate it.

| Shape | Entry point | Follow it through |
| ----- | ----------- | ----------------- |
| Plug-in, one step, no images | `Defra.Imports.Plugins.Itahc.FlagRecordOnWatchList` | `FlagRecordOnWatchListHandler` → `IWatchFlagCreator` → `WatchFlagQueries` |
| Plug-in, several steps, images | `Defra.Imports.Plugins.ImportApplication.DetermineInspectionRequirement` | `DetermineInspectionRequirementHandler` → `IInspectionRequirementDeterminer` |
| CWA | `Defra.Imports.Workflows.ImportApplication.InspectionPlaceOfOriginChange` | `InspectionPlaceOfOriginChangeHandler` → `IInspectionPlaceOfOriginChanger` |

## Layers

| Project | Holds | Must not hold |
| ------- | ----- | ------------- |
| `Defra.Imports.Plugins`, `Defra.Imports.Workflows` (deployment heads) | Entry-point classes, their handlers, and the head's composition root. | Business rules or query construction. |
| `Defra.Imports.Application` | Use-case services, business rules, `Constants`, `ReferenceData`, and shared registration methods. | Handlers, or any dependency on `IPluginExecutionContext`, `CodeActivityContext` or a plug-in class. |
| `Defra.Imports.Dataverse` | Generated model (`Model`), query classes (`Queries`), narrow input types (`Projections`) and the extension methods that create them (`Extensions`). | Hand-written types in the generated `Model` namespace. |
| `Defra.Imports.Infrastructure` | The container, host adapters, handler contracts, identity provider, logging and plug-in constants. | Anything specific to a business capability. |

### Handlers

- A handler is a separate file beside its entry point, in the same namespace and head. Handlers are never shared between heads.
- A plug-in handler implements `IPluginHandler`. It checks the step it is running in, obtains the entity from the context, calls an application service and applies the result as the host requires. Use the constants in `Defra.Imports.Infrastructure.Plugins` (`PluginMessages`, `PluginStages`, `PluginModes`, `PluginInputParameters`) for the step check.
- A workflow handler implements `IWorkflowActivityHandler`. It takes the concrete activity and the `CodeActivityContext` by constructor injection, reads and writes the activity's arguments through the context, and calls an application service.
- The handler decides how a result is persisted (copying values onto a pre-operation `Target`, or calling a service method that issues an explicit `Update`), because that determines the observable write behaviour. Deciding what the result is belongs to the application service.
- A handler makes one or two calls. It does not sequence a read, decide and write flow, and it holds no business rules.
- Handlers and their dependencies are synchronous. Do not use `async`, tasks or threads.
- A handler that depends on `ILogger` can only run in a plug-in, and fails to resolve when Application Insights integration is disabled.

### Application services

- Name them for the use case (`IWatchFlagCreator`), not the entity.
- They may take query classes, `IOrganizationService` or `ImportsContext` directly. Do not add a per-entity reader or writer interface.
- Keep persistence explicit in the method contract and document it. A method that only computes must not write.
- Never change an entity that was passed in; the pipeline `Target` is a mutable reference. Either take a narrow projection (below) or clone the entity with `EntityExtensions.Clone`, change the clone and return it.
- Accept what the host already holds, so that the service does not need to read the record again.
- Never obtain the container or act as a service locator.

### Projections and queries

- Where a use case reads only a few attributes, pass a small immutable context type from `Defra.Imports.Dataverse.Projections`, created by an extension method on the typed entity in `Defra.Imports.Dataverse.Extensions`. The service's inputs are then explicit and it cannot change the `Target`. A projection does not check that attributes are present: an attribute missing from a `Create` `Target` reads as null.
- Extract a query class where a query is reused, hard to fake (paging, `ExecuteMultiple`, FetchXML) or returns values from several tables. A simple single-use lookup can stay in the service.
- Prefer early-bound LINQ, in query syntax where there are joins, over `QueryExpression` unless LINQ cannot express the query.
- Add schema types by regenerating the model (settings in `Model\builderSettings.json`), never by hand.

## Composition

- `PluginContainer` registers type pairs, not factories. Each implementation needs exactly one public constructor. Everything the container constructs is transient, and anything disposable is disposed with the scope in reverse order of construction.
- Each head has one static root (`PluginCompositionRoot`, `WorkflowCompositionRoot`) that only calls `Register`. Add new registrations to the existing root; do not create another.
- Services and queries that both heads need are registered by one extension method per capability in `Application` (for example `InspectionRequirementRegistrations.AddInspectionRequirements`). Handlers and host-specific choices, such as the log writer, stay in each head's root.
- The adapters lend the following to each execution scope. They are never disposed by the scope.

  | Plug-in scope | Workflow scope |
  | ------------- | -------------- |
  | `IPluginExecutionContext`, `IPluginExecution` | `IWorkflowContext`, `CodeActivityContext`, the concrete activity |
  | `IOrganizationService` (executing user) | `IOrganizationService` (executing user) |
  | `IOrganizationServiceFactory`, `ITracingService` | `IOrganizationServiceFactory`, `ITracingService` |
  | `IExecutionServiceProvider` | `IExecutionServiceProvider` |
  | `ILogger`, only if Dataverse supplied one | |

- `WorkflowActivityBase<THandler>` registers the activity under its concrete type, so a handler must request that exact type, not `CodeActivity`.
- Never store a handler, scope, context, service or execution value on a root, a plug-in or an activity. Plug-in and activity instances are cached and invoked concurrently.
- `Plugins.dll` and `Workflows.dll` each compile their own copy of the shared source. Nothing may pass between them, and the workflow head must not reference the plug-in head.
- Add a resolution test for every handler to `PluginCompositionRootTests` or `WorkflowCompositionRootTests`. A missing registration otherwise appears only at run time.

### Failure handling

Both adapters apply the same order of precedence:

1. If the handler fails, that failure is the one surfaced, including deliberate business exceptions. Cleanup is still attempted and cleanup failures are traced.
2. If the handler succeeds but cleanup fails, the cleanup failure is surfaced once all cleanup has been attempted. Several failures are combined in an `AggregateException`.
3. A failure while tracing never replaces either.

`PluginBase<THandler>` rethrows an `InvalidPluginExecutionException` unchanged and wraps any other exception in one, with a generic message and the original as the inner exception. `WorkflowActivityBase<THandler>` rethrows the failure unchanged. `System.Activities.WorkflowInvoker` reduces an `AggregateException` to its first inner exception, so do not rely on the aggregate from a CWA.

### Execution identity

- A plain `IOrganizationService` or injected `ImportsContext` always acts as the executing user (`IPluginExecutionContext.UserId` or `IWorkflowContext.UserId`). It is never rebound during an execution.
- A component that needs another identity injects `IExecutionServiceProvider` and asks for it by name: executing user, initiating user or SYSTEM. There is no arbitrary-user operation.
- The workflow host cannot supply the initiating user or SYSTEM. Those requests fail with `NotSupportedException`; they never return an executing-user service under another name.
- `IImportsContextFactory` creates a context for a chosen identity. The caller owns it and disposes it within the execution. Do not share a context or its tracked entities across identities.
- Never retry a failed call as SYSTEM. Any SYSTEM use needs a narrowly scoped, reviewed justification.
- No component uses these identity services yet. They are verified against mocks only, so validate the effective identity in a Dataverse environment before the first use.

## Adding or migrating a component

### New entry points

- Use a domain folder: services in `Application\<Domain>`, queries and projections in `Dataverse`, the entry point and handler in the head.
- A plug-in is `public sealed class <UtilityName> : PluginBase<THandler>` with a parameterless constructor that passes `PluginCompositionRoot.Root`. Name it for what it does, not for a message, stage or filter.
- A CWA is `public sealed class <UtilityName> : WorkflowActivityBase<THandler>` with a parameterless constructor that passes `WorkflowCompositionRoot.Root`. It declares its arguments and nothing else.
- Register steps and images with the Plug-in Registration Tool in a development environment and commit the exported solution metadata. Do not use registration attributes in code.

### Migrating an existing entry point

Migrate one use case at a time, moving its handler, application logic and Dataverse dependencies together. Do not migrate a legacy class only to complete the structure.

1. Characterise the existing behaviour with tests first: success and failure paths, changes to the `Target`, outputs, and the order and timing of reads and writes.
2. Keep the entry point's identity: class name, namespace, assembly, step registration (message, stage, mode, execution order, filtering attributes, images, impersonation) and activity argument names, types and defaults. No step is re-registered and no workflow is reconfigured.
3. Preserve observable behaviour:
   - what is written, when, and by which identity;
   - transaction behaviour: add no updates, retries, deferred writes or transaction wrappers;
   - error contracts, user-visible messages and any existing catch-and-continue behaviour.
4. Do not fix suspected bugs. Mark each with a code comment and list it in the migration log for a decision.
5. Replace test-only seams, such as the custom `IFetchService` workflow extension, with test-specific composition. Do not reproduce them.
6. Resolve the analyser warnings in the code you move or materially change, without suppressions. Where StyleCop requires American wording (constructor summaries beginning `Initializes a new instance of`), follow the analyser; use British English elsewhere.
7. Delete the legacy classes and tests that the slice replaces, once every active test case has been ported.
8. Build both heads in Release and run the unit tests under the `UnitTest` configuration.
9. Validate in an isolated Dataverse environment. Unit tests run against `Container.dll`, not the Release assemblies, and do not replace this.
10. Add an entry to the migration log.

### Versioning

Shared source and the plug-in assemblies sit outside the solution metadata folder, so the commit body must include `+solutionVer(defra_Imports): patch` (or higher). Link the Azure Boards item with `AB#` in the pull request title and body.

## Testing

Test support is in `tests\Defra.Imports.UnitTests\Support`.

- `PluginTestBase<TPlugin>` and `WorkflowActivityTestBase<TActivity>` simulate a host invocation and supply common assertions.
- `PluginRuntime` simulates the services Dataverse supplies to one plug-in execution. `ForStep(message, entity, stage, mode)` and `WithTarget(target)` describe the step that the handler checks.
- Test a workflow handler through its activity and `WorkflowInvoker`, because a `CodeActivityContext` cannot be constructed. `WorkflowInvoker` wraps failures, so search the inner exceptions for the expected type.
- A strict `IOrganizationService` mock is the simplest proof that an invalid invocation read and wrote nothing.

Behaviour of the early-bound LINQ provider, established by unit tests against the SDK's client-side provider and not yet confirmed in a deployed environment:

- Queries are sent as `Execute(RetrieveMultipleRequest)`, not `RetrieveMultiple`, so set up `Execute` on the service double.
- Projections into the early-bound type and into anonymous types both work, provided the service returns typed entities. Mocks must therefore return early-bound entities. In the sandbox this depends on the `ProxyTypesAssemblyAttribute` in `ImportsContext.cs`.
- `FirstOrDefault()` is evaluated on the client and does not limit the query. That is acceptable for a primary-key lookup; for any other filter add `.Take(1)`.
- The provider joins related tables only. Use separate queries for unrelated tables.

Behaviour of `EntityExtensions`, pinned by `EntityExtensionsTests`:

- `Clone` is shallow. The clone has its own attribute collection, but attribute values such as `EntityReference` and `OptionSetValue` are shared, so replace them; do not change them in place.
- `Clone` copies the logical name, and sets the identifier only when it differs, so that an early-bound entity without an identifier does not gain an empty primary key attribute.
- `GetChanges` compares `Money`, `OptionSetValue`, `EntityReference`, `DateTime` and `string` by value. Strings are case-sensitive. Other reference types, such as `EntityCollection`, should be assumed to compare by reference.
