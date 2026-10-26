# 0001. Lightweight DI Scaffold for Dataverse Plug-ins and Custom Workflow Activities (Application / Dataverse / Infrastructure Shared Projects)

- **Status**: Proposed. The composition/scoping scaffold, `PluginBase<THandler>`, `WorkflowActivityBase<THandler>`, the identity-aware service provider and the watch list illustrative example are implemented and unit-tested. One existing plug-in, `PopulateAnimalNumber`, has been migrated in source, followed by `FlagRecordOnWatchList` and `FlagRecordOnWatchListNotification` (see "Adoption Record"), but nothing is registered with Dataverse against the new code, and deployed-host and effective-identity behaviour have not been validated in a live Dataverse environment. The example is compiled into both deployment heads (and therefore into the assemblies the `defra_Imports` solution builds), but no plug-in step, activity registration or other solution metadata refers to it: it is not registered, not included in the solution's registered components, and not a template. Seek normal review approval before any production adoption.
- **Date**: 2026-10-22
- **Related requirement(s)**: Explicit user requests to scaffold separated application, Dataverse and infrastructure Shared Projects with execution-scoped dependency injection (DI), retain the non-deployed `Container`, add a handler-first `WorkflowActivityBase<THandler>`, and define the migration rules for analyser warnings, model ownership, handler placement and application-service responsibilities. The scaffold migrates no existing business logic; the migration rules govern later capability migrations. No Epic/Feature/User Story or `AB#` was supplied, so no work-item traceability is recorded.
- **Related ADR(s)**: None. This is the first substantive ADR; `0000-template.md` is the template only.
- **Decision confidence**:
  - **High** for the synchronous `void Execute()` handler contract (Dataverse invokes plug-ins and CWAs synchronously and no asynchronous requirement exists) and for the composition/scoping mechanics (both deployment heads build in Release without errors;   the `Defra.Imports.UnitTests.Infrastructure` suite passes 80/80 and the full unit suite passes with no failures).
  - **Medium** for `WorkflowActivityBase<THandler>` (unit-tested through a real `System.Activities.WorkflowInvoker`, but no registered activity has validated host behaviour).
  - **Medium** for the identity-aware provider and context factory (unit-tested against requested factory arguments only, not effective Dataverse impersonation).
    - **Medium** for the layer boundaries and the watch list example: both heads compile the example and unit tests exercise it end to end through `Container.dll`, but nothing is verified in a live Dataverse host (sandbox type resolution, `ownerid` on a stage 20 `Create`, effective identity, write timing) and warning clean-up awaits a representative vertical migration.
    - **High** for the amended `EntityExtensions.Clone<T>` and the `GetChanges` value-equality behaviour for the types tested, because tests pin both. **Medium** for the LINQ provider notes, because only the client-side provider behaviour is tested.

## Context

### Current pattern

- Business logic and data access are compiled from **Shared Projects** (`.shproj`/`.projitems`), originally `Defra.Imports.Model`, `Defra.Imports.BusinessLogic` and `Defra.Imports.Repositories`. They are imported into two deployed, sandbox-registered head assemblies, `Defra.Imports.Plugins` (17 plug-in classes deriving from `Defra.Imports.BusinessLogic.Plugin`) and `Defra.Imports.Workflows` (CWAs using several base-class families), and into a third, **non-deployed** assembly, `Defra.Imports.Container`, referenced only by `Defra.Imports.UnitTests`.
- Both head `.csproj` files use a **configuration-conditional import**: the shared projects are compiled directly into `Plugins`/`Workflows` unless `$(Configuration) == 'UnitTest'`, when they take a `ProjectReference` to `Container.dll`. Deployed assemblies stay self-contained (no external DLL in the Dataverse sandbox) while unit tests get one directly referenceable copy.
- There is **no DI**: `Plugin.Execute` manually creates a `RepositoryFactory` and `TracingServiceLogWriter` per call, and concrete plug-ins manually create their business-logic classes.
- An unused non-generic `Defra.Imports.Plugins.PluginBase` (with `ILocalPluginContext`, `LocalPluginContext`, `LocalTracingService`) is derived from by none of the 17 plug-ins: an abandoned earlier attempt at the same problem.

### Platform constraints

Verified against Microsoft Learn guidance:

- **No asynchronous or parallel execution** in plug-ins or CWAs: the task-based asynchronous pattern, Task Parallel Library and managed threading are all unsupported ([guidance](https://learn.microsoft.com/power-apps/developer/data-platform/best-practices/business-logic/do-not-use-parallel-execution-in-plug-ins)).
- **`IPlugin` instances are cached and invoked concurrently** ([guidance](https://learn.microsoft.com/power-apps/developer/data-platform/best-practices/business-logic/develop-iplugin-implementations-stateless)), so shared/static state, including a composition root, must be safe under concurrent `Execute` calls.
- **`ILogger` (`Microsoft.Xrm.Sdk.PluginTelemetry.ILogger`) is plug-in-specific** and only populated when Application Insights integration is enabled; CWAs have no equivalent (`CodeActivityContext.GetExtension<T>()` supplies `IWorkflowContext`, `ITracingService` and `IOrganizationServiceFactory` only).
- A synchronous plug-in step has a **2-second practical budget and a 2-minute hard ceiling** ([guidance](https://learn.microsoft.com/power-apps/developer/data-platform/analyze-performance)), so scope construction/teardown cost is a real constraint.

## Decision

Add three Shared Projects, **`Defra.Imports.Application`**, **`Defra.Imports.Dataverse`** and **`Defra.Imports.Infrastructure`**, alongside `BusinessLogic`/`Repositories`, imported into `Plugins`, `Workflows` and `Container` with the **existing configuration-conditional pattern**. The scaffold changes no deployed registration and migrates no existing business logic. Capabilities are migrated later, one vertical slice at a time, applying the rules below. The legacy `Defra.Imports.Model` project is retired: generated early-bound types live in `Defra.Imports.Dataverse` (`Defra.Imports.Dataverse.Model`), and `Constants` and `ReferenceData` live in `Defra.Imports.Application`.

| Project                        | Responsibility |
| ------------------------------ | -------------- |
| `Defra.Imports.Infrastructure` | The bespoke embedded `PluginContainer` (lightweight DI, not a third-party package); `PluginBase<THandler>`, `IPluginHandler`, `IPluginExecution`; `WorkflowActivityBase<THandler>` and `IWorkflowActivityHandler`; and the identity-aware `IExecutionServiceProvider` (`PluginExecutionServiceProvider`, `WorkflowExecutionServiceProvider`). |
| `Defra.Imports.Application`    | Targeted, use-case-shaped application services and business rules, plus `Constants` and `ReferenceData`. No plug-in or workflow handlers. |
| `Defra.Imports.Dataverse`      | Generated early-bound types and `ImportsContext` (`Defra.Imports.Dataverse.Model`; Model Builder settings in `Model\builderSettings.json`), the context factory, hand-written helpers, extension methods that project typed entities onto narrow contexts (`Defra.Imports.Dataverse.Extensions`, with the contexts in `Defra.Imports.Dataverse.Projections`), and shared query classes and their result projections under `Defra.Imports.Dataverse.Queries`. |

### Implementation scope and analyser compliance

- Remove the unused non-generic `Defra.Imports.Plugins.PluginBase` and its `ILocalPluginContext`, `LocalPluginContext` and `LocalTracingService` as part of the scaffold, after rechecking references. This is an explicit, user-requested exception to retiring components via deprecation, limited to this unused source scaffold. Preserve `Defra.Imports.BusinessLogic.Plugin`, all live plug-in classes and their registrations.
- Where British English conflicts with a StyleCop-required wording, follow the analyser: constructor XML summaries use `Initializes a new instance of the ... class.` Never suppress an analyser to retain British spelling; use British English elsewhere.
- Each migration resolves existing analyser warnings in the implementation it moves or materially refactors, plus any it introduces, without suppressions. Unrelated repository-wide warnings are tracked separately.

### Confirmed migration constraints

Confirmed by the user; they govern later migrations.

- **Behaviour and bugs:** migrated code behaves identically even when restructured: workflow output names, types and values; target mutations; database effects; execution identity; transaction behaviour; error contracts; deliberate business exceptions and user-visible messages; and existing catch-and-continue behaviour. Raise suspected bugs and questionable practices for an explicit decision; never fix them silently.
- **Entry-point identity:** retain concrete plug-in and activity class names, namespaces and assembly identity, registration metadata (message, stage, mode, execution order, filtering attributes, image names/columns, impersonation) and activity argument metadata, types, names and defaults. No step re-registration or workflow reconfiguration. Utility-based naming applies to new entry points only; internal handlers/services may be renamed or replaced.
- **Persistence and transactions:** the handler decides whether a change is persisted through the pipeline target or an explicit update, preserving the existing observable behaviour and write timing. Business logic returns a cloned entity and never mutates its input (see Layer ownership). Do not add updates, persist a disconnected copy of the target, add `SaveChanges()` on disposal, transaction wrappers, retries or deferred writes. Transaction control stays with Dataverse.
- **Identity and context lifetime:** unqualified `IOrganizationService` and `ImportsContext` mean the executing user. No implicit elevation, identity change or fallback to another user; unsupported workflow identity requests fail explicitly; any privileged operation needs a separate reviewed design. Contexts stay independent unless shared tracking is demonstrably required, and then only within one invocation and identity.
- **Ownership:** a scope disposes container-created disposables, never host-provided SDK services, the activity or its execution context. Callers dispose factory-created contexts within the invocation. Deferred queries and context-backed results must not outlive their owner. Resolving a parent factory through DI does not make nested objects container-owned.
- **Workflow extension overrides:** the custom `IFetchService` extension overrides exist only to substitute dependencies in tests. Replace them with test-specific DI composition when migrating those activities; do not reproduce custom extension lookup or override precedence. Genuine SDK extensions continue to come from `CodeActivityContext.GetExtension<T>()`. Test composition must not mutate the frozen production root or introduce process-wide overrides.

### Execution and cleanup failure precedence

Both host adapters apply this policy to activation, handler execution and workflow argument mapping:

1. If execution fails, preserve that failure (and its stack) as primary, including deliberate business exceptions. Attempt all remaining cleanup and trace cleanup failures separately. This does not authorise a generic wrapper that changes an existing business error contract.
2. If execution succeeds but cleanup fails, surface the cleanup failure (multiple failures aggregate into an `AggregateException`) once all cleanup has been attempted.
3. A failure while tracing a cleanup error never replaces the primary error; do not add broad catches that hide execution failures.

For plug-ins the aggregate surfaces as `InvalidPluginExecutionException.InnerException`. For workflows the adapter throws the full aggregate and traces it, but `System.Activities.WorkflowInvoker` translates it so the caller of `Invoke` sees only the first inner cleanup exception; do not rely on the aggregate shape from a `CodeActivity` host. This policy does not change handler-level catch-and-continue contracts.

### Layer ownership

- **Model:** schema-bound types live in `Dataverse` under the generated `Defra.Imports.Dataverse.Model` namespace. Hand-written types must not be placed there (regeneration may overwrite them); extension methods and the contexts they project onto live beside it in `Defra.Imports.Dataverse.Extensions` and `Defra.Imports.Dataverse.Projections`. Add new schema types by regenerating, never in a parallel project, duplicate type or namespace. Query result projections (for example `WatchListInitialisationContext`) are hand-written beside the query returning them in `Defra.Imports.Dataverse.Queries`, may span several tables, and are not returned by application services. `Constants` and `ReferenceData` live in `Defra.Imports.Application.Constants` and `.ReferenceData`.
- **Handlers sit beside their host.** A plug-in handler and a CWA handler are separate files in the **same namespace and head project** as their `IPlugin`/`CodeActivity` class (`Defra.Imports.Plugins.<Area>`, `Defra.Imports.Workflows.<Area>`). There is no `Handlers` namespace and no handler in `Application`, because handlers depend on plug-in or workflow context concerns and are not shared.
- **Handlers adapt and orchestrate.** A plug-in handler validates the execution context against the registration contract (message, primary entity, stage, mode, images/parameters), obtains the entity from the context, calls a coarse use-case-shaped application method and applies the result as its host requires (for example copying returned values onto the pre-operation target, or choosing an application command that issues an explicit `Update`). Choosing *how the result is persisted* is a handler concern because it determines the observable write behaviour; deciding *what the result is* is not. A handler makes one or two such calls and does not sequence read → decide → write flow or absorb business rules or query construction. Workflow handlers likewise adapt activity arguments and preserve the activity contract.
- **Application services own reusable use-case logic, not host plumbing.**
  - They never depend on a plug-in class, `IPluginExecutionContext`, `CodeActivityContext` or a handler.
  - They are targeted and business-shaped (for example `IWatchListInitialiser`), not a catch-all `I<Entity>ApplicationService`.
  - They may take query classes, `IOrganizationService` or `ImportsContext` directly; a per-entity reader/writer interface would add indirection without hiding a dependency.
  - Extract a `Dataverse` query class where a query is reused, hard to fake (paging, `ExecuteMultiple`, FetchXML) or returns a multi-table projection. Prefer early-bound LINQ (query syntax where joins are involved) over `QueryExpression` unless LINQ cannot express the query.
  - Keep persistence explicit in method contracts. `InitialiseWatchList(Guid watchListId)` reads, initialises and updates, so it takes an ID. `GetInitialisedWatchList(defraimp_WatchList watchList)` returns a new entity and issues no create/update. A `Get...` method does not imply a database read: document the side-effect contract and test it.
  - **Business logic never mutates an entity it is given** (the pipeline `Target` is a mutable reference): it clones its input, changes the clone and returns it.
  - Methods accept the `Entity` the host already holds rather than an ID or `EntityReference` that would force a re-read. Where a use case reads only a few attributes, it may instead take a small immutable context type that an extension method on the typed entity projects (for example `CreateItahcWatchFlagsContext`, in `Defra.Imports.Dataverse.Projections`, projected by `ItahcExtensions` in `Defra.Imports.Dataverse.Extensions`, so that queries can reuse the projection); the projection holds the values, so it also forces no re-read. The handler's choice of method decides who persists: the pipeline (copy onto the `Target`) or an explicit `Update`. No write-abstraction port is introduced.
- **Migrate vertically.** Move a use case's handler, application logic and Dataverse dependencies together, updating namespaces and `.projitems` ownership atomically and removing old inclusions. Do not migrate legacy classes merely to complete the scaffold.

### The composition model

1. **`PluginContainer`** (`Defra.Imports.Infrastructure.DependencyInjection`) has two states of one type.
   - **Root:** `new PluginContainer()`. `Register<TInterface, TImplementation>()`/`Register<TImplementation>()` register type pairs (not factories); resolution is reflective, requires **exactly one public constructor** per implementation and detects cycles. Registrations are mutable until the first `CreateExecutionScope()`, then frozen (further `Register` throws `InvalidOperationException`).
   - **Execution scope:** `root.CreateExecutionScope()` returns an independent container holding a copy of the root's registrations. Only a scope accepts `Register<TService>(instance)`, a **borrowed** runtime instance (such as the current `IOrganizationService`) that is never disposed. Borrow-registering on the root, or creating a scope from a scope, throws.
   - Operations are lock-guarded, so concurrent `CreateExecutionScope()`/`Resolve<T>()` against one cached root is safe, which answers the concurrent-`IPlugin` constraint (30-iteration `Parallel.For` and 10-iteration concurrent-`Execute` tests show no cross-execution leakage).
   - Constructor-created services are transient and tracked. Scope `Dispose()` disposes them in reverse construction order, continues past individual failures (aggregating them), never disposes borrowed instances, and still disposes services constructed before a later dependency failed.
   - Production code resolves the handler graph from the scope, never the root, because only the scope holds the invocation's borrowed services; the root has no such registrations, so this fails structurally.
2. **`PluginBase<THandler>`** (`Defra.Imports.Infrastructure.Plugins`, `where THandler : IPluginHandler`) is the adapter plug-in classes derive from.
   - It takes the frozen root through `protected PluginBase(PluginContainer container)`.
   - `IPlugin.Execute` creates **one scope per call**, borrow-registers `IPluginExecutionContext`, `IOrganizationService` (executing user), `ITracingService` and the thin `IPluginExecution` (`Context`, `Service`, `Tracing` only), registers `ILogger` **only if Dataverse supplied one**, resolves `THandler` and calls its synchronous `void Execute()`.
   - A missing mandatory runtime service fails explicitly (`InvalidOperationException`, wrapped as `InvalidPluginExecutionException`), with no silent fallback. A handler with a hard `ILogger` dependency is therefore plug-in-only and fails resolution where none exists; handlers meant to be CWA-portable must not depend on `ILogger`.
   - Handlers needing more (such as a query class) take it as their own constructor dependency rather than through a broad context object.
   - **Composition-root ownership:** one registration-only `internal static PluginCompositionRoot` (a `Lazy<PluginContainer>` built by `BuildRoot()`, which only calls `Register`) serves the whole `Plugins` assembly; never capture a per-execution instance in it. Adding a plug-in adds registrations to that root, not a new root. `Workflows` is a separately deployed assembly with its own static root (`WorkflowCompositionRoot`), shared by its activities; common registration code may be factored into a shared helper. Neither root is yet referenced by a registered component.
   - **Naming and registration:** name new `IPlugin` classes for the utility they provide (for example `WatchListInitialisationPlugin`), not a message, stage, mode or filter; the name is independent of the handler type. Configure steps and images with the Plug-in Registration Tool, never code attributes. One class may have several steps.
3. **`WorkflowActivityBase<THandler> : CodeActivity`** (`Defra.Imports.Infrastructure.Workflows`, `where THandler : IWorkflowActivityHandler`, with a synchronous `void Execute()` contract) is the workflow equivalent. Existing workflow bases and activities are untouched and none has migrated.
   - It takes the root through a protected constructor; every concrete activity has a public parameterless constructor passing the single `WorkflowCompositionRoot.Root`. The root registers handler and service types only, never the host-created activity.
   - A sealed `Execute(CodeActivityContext)` validates the context and creates one scope per invocation. It gets `IWorkflowContext`, `IOrganizationServiceFactory` and `ITracingService` via `GetExtension<T>()`, creates the organisation service for `IWorkflowContext.UserId`, and borrow-registers those plus the `CodeActivityContext`. Missing services fail explicitly.
   - It calls `protected abstract void RegisterActivity(PluginContainer scope)` before resolving the handler. The override is only `scope.Register<ConcreteActivity>(this)`, binding the exact host-created activity (instance registrations resolve by exact type, so registering `CodeActivity` would not satisfy a handler needing the concrete type).
   - The handler receives the activity, `CodeActivityContext` and services by constructor injection, reads `InArgument<T>` and writes `OutArgument<T>` through the current context, and calls application services. Activities declare argument metadata and the binding override only, with no business logic.
   - Failures are traced when possible and rethrown without losing the cause; the scope is disposed on every path under the failure precedence above. Never cache a handler, scope, context or service on the activity or root, nor store execution values on the activity, because a cached activity is invoked concurrently.
   - It never creates `IPluginExecutionContext`/`IPluginExecution`, requires `IPluginHandler`, registers `ILogger` or introduces asynchronous work. Workflow handlers sit beside their activity in `Workflows`.
   - **Why handler-first:** the container builds the whole handler graph, activities stay declarative entry points, and application services stay reusable from plug-ins, for the cost of one small handler contract and a binding override.
4. **Shared-source type identity.** Shared Projects are compiled separately into each importing assembly, so `Plugins.dll` and `Workflows.dll` hold distinct CLR copies of types such as `PluginContainer`. This is safe because nothing passes between them. Under `UnitTest`, both heads reference `Container.dll`, preserving type identity for tests. Unit tests therefore exercise `Container.dll`'s IL, not the bytes shipped in Release (see Risk Register).

### Execution identity

**Default:** an unqualified `IOrganizationService` is the **executing user** (`IPluginExecutionContext.UserId`, `IWorkflowContext.UserId`), not necessarily the initiating user. This is host-provided impersonation, not a new connection. An injected `ImportsContext` is constructed over that default service, registered as a concrete type, container-owned and transient (not a shared tracking context).

**Host capabilities** (see [CreateOrganizationService](https://learn.microsoft.com/en-us/dotnet/api/microsoft.xrm.sdk.iorganizationservicefactory.createorganizationservice?view=dataverse-sdk-latest) and [impersonation](https://learn.microsoft.com/en-us/power-apps/developer/data-platform/impersonate-a-user)):

| Requested identity | Plug-in factory | Workflow factory |
| ------------------ | --------------- | ---------------- |
| Executing user | `context.UserId` (`Guid.Empty` also means `UserId`). | Calls use workflow `UserId`. |
| Initiating user | `context.InitiatingUserId`. | Not a documented switch; calls still use workflow `UserId`. |
| SYSTEM | `null`. | `null` is not SYSTEM; execution remains workflow `UserId`. |
| Specific user | A valid system-user ID, subject to permissions. | Passing another ID does not change identity. |

- `IExecutionServiceProvider` offers three named operations (executing user, initiating user, SYSTEM) and no arbitrary-user API. Separate plug-in and workflow implementations are bound as borrowed, invocation-local instances in each adapter and never cached in a root.
- Plain `IOrganizationService` stays executing-user-only; never rebind it mid-execution. A component needing another identity injects the provider and asks deliberately. SYSTEM use needs a narrowly scoped, reviewed justification; never retry an access-denied call as SYSTEM or fall back to another user.
- The workflow provider supplies only identities the host supports; others fail with `NotSupportedException` before business calls and are traced. Never return an executing-user service claiming to be SYSTEM or another user. No external connection, credentials or undocumented impersonation. A workflow needing another identity requires a separately validated, security-reviewed design with its ALM path recorded.
- `IImportsContextFactory`/`ImportsContextFactory` (Dataverse layer) creates contexts for the selected identity (`CreateForExecutingUser`, `CreateForInitiatingUser`, `CreateForSystem`). These contexts are **caller-owned** (dispose with `using` within the invocation), are not tracked by the container and never reuse a context or its tracked entities across identities, scopes or concurrent operations. Business services never obtain the container or a service locator.

### Illustrative example

The example is a watch list initialisation use case shown through two hosts sharing one business method. It uses types already in the generated model (`defraimp_WatchList`, `SystemUser`, `defraimp_Transporter`). It is **documentation-by-example**, not a reusable capability or real business rule: it **must not be registered with Dataverse, included in the solution as a registered component or step, extended or treated as a template**. Every example type carries an `ILLUSTRATIVE EXAMPLE` XML summary referencing this ADR. `EntityExtensions` is a generic, reusable helper and is deliberately not labelled as an example.

The example is implemented and unit-tested in `Defra.Imports.Application`, `Defra.Imports.Dataverse`, `Defra.Imports.Plugins` and `Defra.Imports.Workflows`, and is compiled into both Release heads. Unit tests exercise it through `Container.dll` under the `UnitTest` configuration. Nothing was validated in a live Dataverse environment. Implementation exposed two defects in the original snippet's `Clone<T>` (see the lessons learnt below); the snippets that follow match the verified source.

- `IWatchListInitialiser` (Application) has two methods.
  - `GetInitialisedWatchList(defraimp_WatchList)` returns a **new** watch list with a default `defraimp_Name` and `defraimp_Comments` where blank. It never mutates its input and persists nothing.
  - `InitialiseWatchList(Guid)` retrieves the watch list, calls `GetInitialisedWatchList` and issues one `Update` containing only the changed values.
- `GetInitialisedWatchList` calls `WatchListQueries.GetWatchListInitialisationContext` (Dataverse), which returns a `WatchListInitialisationContext`: a multi-table projection of the owning `SystemUser` and, if referenced, the `defraimp_Transporter`.
- **Plug-in host:** a pre-operation, synchronous `Create` step on `defraimp_watchlist`. The handler wraps the pipeline `Target`, calls `GetInitialisedWatchList` with that in-memory entity (no re-read) and copies only the changed values back onto the `Target`, which the pipeline persists.
- **CWA host:** the activity receives only an `EntityReference`, so its handler calls `InitialiseWatchList(reference.Id)`. The unavoidable retrieve and the delta `Update` (executing user) live in the application method.

**Shared helpers and logic** (Application and Dataverse):

```csharp
// Defra.Imports.Application.WatchLists\IWatchListInitialiser.cs
namespace Defra.Imports.Application.WatchLists
{
    using System;
    using Defra.Imports.Dataverse.Model;

    /// <summary>Example only: demonstrates a targeted application service. Not a real business rule.</summary>
    public interface IWatchListInitialiser
    {
        /// <summary>Returns a new watch list with initialisation values populated where blank. The input is not modified and nothing is persisted.</summary>
        defraimp_WatchList GetInitialisedWatchList(defraimp_WatchList watchList);

        /// <summary>Retrieves the watch list, initialises it and updates only the values that changed.</summary>
        void InitialiseWatchList(Guid watchListId);
    }
}

// Defra.Imports.Application.WatchLists\WatchListInitialiser.cs
namespace Defra.Imports.Application.WatchLists
{
    using System;
    using Defra.Imports.Dataverse;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Dataverse.Queries;
    using Microsoft.Xrm.Sdk;

    public sealed class WatchListInitialiser : IWatchListInitialiser
    {
        private readonly WatchListQueries queries;
        private readonly IOrganizationService service;

        public WatchListInitialiser(WatchListQueries queries, IOrganizationService service)
        {
            this.queries = queries ?? throw new ArgumentNullException(nameof(queries));
            this.service = service ?? throw new ArgumentNullException(nameof(service));
        }

        public void InitialiseWatchList(Guid watchListId)
        {
            var watchList = this.queries.GetWatchListForInitialisation(watchListId);
            var changes = this.GetInitialisedWatchList(watchList).GetChanges(watchList);

            // Nothing is sent if the watch list was already initialised.
            if (changes.Attributes.Count > 0)
            {
                this.service.Update(changes);
            }
        }

        public defraimp_WatchList GetInitialisedWatchList(defraimp_WatchList watchList)
        {
            if (watchList == null)
            {
                throw new ArgumentNullException(nameof(watchList));
            }

            var owner = watchList.OwnerId
                ?? throw new InvalidOperationException("The watch list must have an owner.");
            var context = this.queries.GetWatchListInitialisationContext(owner.Id, watchList.defraimp_TransporterId?.Id);

            var initialised = watchList.Clone();
            if (string.IsNullOrWhiteSpace(initialised.defraimp_Name))
            {
                initialised.defraimp_Name =
                    $"{context.TransporterName ?? "Unassigned transporter"} watch list ({context.OwnerName})";
            }

            if (string.IsNullOrWhiteSpace(initialised.defraimp_Comments))
            {
                initialised.defraimp_Comments = $"Owned by {context.OwnerName} ({context.OwnerEmail}).";
            }

            return initialised;
        }
    }
}

// Defra.Imports.Dataverse\EntityExtensions.cs (hand-written, outside the generated Model namespace)
namespace Defra.Imports.Dataverse
{
    using System;
    using Microsoft.Xrm.Sdk;

    public static class EntityExtensions
    {
        // Shallow copy: the attribute collection is new, so adding or replacing values never touches the source.
        public static T Clone<T>(this T entity)
            where T : Entity, new()
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var clone = new T();
            foreach (var attribute in entity.Attributes)
            {
                clone[attribute.Key] = attribute.Value;
            }

            // A parameterless Entity has no logical name, and setting an empty identifier on an early-bound
            // type would add a primary key attribute that the source does not have.
            clone.LogicalName = entity.LogicalName;
            if (clone.Id != entity.Id)
            {
                clone.Id = entity.Id;
            }

            return clone;
        }

        // An entity holding only the values that are new or different, suitable for Update.
        public static Entity GetChanges(this Entity updated, Entity original)
        {
            if (updated == null)
            {
                throw new ArgumentNullException(nameof(updated));
            }

            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            var changes = new Entity(updated.LogicalName, updated.Id);
            foreach (var attribute in updated.Attributes)
            {
                if (!original.Attributes.TryGetValue(attribute.Key, out var value)
                    || !Equals(value, attribute.Value))
                {
                    changes[attribute.Key] = attribute.Value;
                }
            }

            return changes;
        }
    }
}

// Defra.Imports.Dataverse.Queries\WatchListInitialisationContext.cs
namespace Defra.Imports.Dataverse.Queries
{
    using System;

    /// <summary>A multi-table projection (owning user and, optionally, the transporter); not an SDK entity.</summary>
    public sealed class WatchListInitialisationContext
    {
        public WatchListInitialisationContext(
            Guid ownerId, string ownerName, string ownerEmail, string transporterName)
        {
            this.OwnerId = ownerId;
            this.OwnerName = ownerName;
            this.OwnerEmail = ownerEmail;
            this.TransporterName = transporterName;
        }

        public Guid OwnerId { get; }

        public string OwnerName { get; }

        public string OwnerEmail { get; }

        public string TransporterName { get; }
    }
}

// Defra.Imports.Dataverse.Queries\WatchListQueries.cs
namespace Defra.Imports.Dataverse.Queries
{
    using System;
    using System.Linq;
    using Defra.Imports.Dataverse.Model;
    using Microsoft.Xrm.Sdk.Client;

    public class WatchListQueries
    {
        private readonly ImportsContext context;

        public WatchListQueries(ImportsContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));

            // These queries are read-only, so avoid change tracking on the invocation-local context.
            this.context.MergeOption = MergeOption.NoTracking;
        }

        // One bounded retrieve for callers that hold only an ID; projects exactly what the initialiser reads.
        public defraimp_WatchList GetWatchListForInitialisation(Guid watchListId)
        {
            var query =
                from watchList in this.context.defraimp_WatchListSet
                where watchList.defraimp_WatchListId == watchListId
                select new defraimp_WatchList
                {
                    defraimp_WatchListId = watchList.defraimp_WatchListId,
                    OwnerId = watchList.OwnerId,
                    defraimp_TransporterId = watchList.defraimp_TransporterId,
                    defraimp_Name = watchList.defraimp_Name,
                    defraimp_Comments = watchList.defraimp_Comments,
                };

            return query.FirstOrDefault()
                ?? throw new InvalidOperationException("The requested watch list was not found.");
        }

        // A projection spanning systemuser and defraimp_transporter. The tables are unrelated and the
        // transporter is optional, so these are two independent early-bound queries behind one method.
        public WatchListInitialisationContext GetWatchListInitialisationContext(Guid ownerId, Guid? transporterId)
        {
            var ownerQuery =
                from user in this.context.SystemUserSet
                where user.SystemUserId == ownerId
                select new { user.FullName, user.InternalEMailAddress };

            var owner = ownerQuery.FirstOrDefault()
                ?? throw new InvalidOperationException("The watch list owner was not found or is not a user.");

            string transporterName = null;
            if (transporterId.HasValue)
            {
                var id = transporterId.Value;
                var transporterQuery =
                    from transporter in this.context.defraimp_TransporterSet
                    where transporter.defraimp_TransporterId == id
                    select new { transporter.defraimp_Name };

                transporterName = transporterQuery.FirstOrDefault()?.defraimp_Name;
            }

            return new WatchListInitialisationContext(
                ownerId, owner.FullName, owner.InternalEMailAddress, transporterName);
        }
    }
}
```

Snippets abridge the XML documentation. The real source carries full documentation, with constructor summaries using the analyser-required wording and an `ILLUSTRATIVE EXAMPLE` summary on every example type. Notes on the example:

- Queries use the container-owned, executing-user `ImportsContext` (transient, so `NoTracking` does not leak to other consumers) and must be resolved from the invocation scope.
- The LINQ provider joins only related tables and its outer-join support is unclear, so the example uses two independent queries (the tables are unrelated and the transporter is optional) and keeps string composition in the initialiser. Where related tables are mandatory, prefer a single `join` in query syntax.
- The owner is assumed to be a user; a team owner needs a second query path.
- **LINQ provider behaviour** (verified by unit tests against the SDK's client-side provider; **unverified in a live environment**; confidence **Medium**):
  - Member-initialiser projections into the early-bound type and anonymous-type projections both work.
  - The provider sends `Execute(RetrieveMultipleRequest)`, not `RetrieveMultiple`, so test doubles must set up `Execute`.
  - `FirstOrDefault()` is evaluated client-side and does not set `TopCount`. That is acceptable for primary-key lookups, as here. If a non-unique filter is ever used, add `.Take(1)` explicitly.
  - Anonymous projections need typed entities from the service. This should hold in the sandbox because the shared Model carries `[assembly: ProxyTypesAssemblyAttribute]` (in `ImportsContext.cs`), which is compiled into each head, but it has not been confirmed in a deployed host.
- **`GetChanges` equality** (pinned by tests; confidence **High** for the types listed): `Money`, `OptionSetValue`, `EntityReference` (different instances for the same record), `DateTime` and `string` compare by value. Strings compare case-sensitively, so a case-only change is reported. Reference-typed values such as `EntityCollection` were not tested and should be assumed to compare by reference.
- **Lessons learnt from implementation** (the original snippet's `Clone<T>` failed two tests; later migrations must not copy it):
  - `new T()` on a plain `Entity` has no `LogicalName`, so `Clone<Entity>` lost it. `Clone` now copies `LogicalName`.
  - On an early-bound entity with `Id == Guid.Empty`, `clone.Id = entity.Id` calls the generated setter, which adds a `defraimp_watchlistid = Guid.Empty` attribute. `GetChanges` then reported it and the plug-in handler copied it onto the `Target`. `Id` is now set only when it differs.

**Plug-in host.** The handler is intended for a **pre-operation (stage 20), synchronous `Create`** step. `ToEntity<T>()` gives a typed view of the `Target`; the initialiser clones again, so the `Target` is changed only by the handler's explicit copy of the changed values.

```csharp
// Defra.Imports.Plugins\PluginCompositionRoot.cs
using System;
using Defra.Imports.Application.WatchLists;
using Defra.Imports.Dataverse.Contexts;
using Defra.Imports.Dataverse.Model;
using Defra.Imports.Dataverse.Queries;
using Defra.Imports.Infrastructure.DependencyInjection;
using Defra.Imports.Plugins.WatchLists;

internal static class PluginCompositionRoot
{
    private static readonly Lazy<PluginContainer> RootValue =
        new Lazy<PluginContainer>(BuildRoot);

    internal static PluginContainer Root => RootValue.Value;

    private static PluginContainer BuildRoot()
    {
        var root = new PluginContainer();

        // ILLUSTRATIVE EXAMPLE registrations: not a real business process; do not register with
        // Dataverse or copy as a template (comment abridged; see the ADR).
        root.Register<IWatchListInitialiser, WatchListInitialiser>();
        root.Register<WatchListQueries>();
        root.Register<WatchListInitialisationHandler>();

        root.Register<IImportsContextFactory, ImportsContextFactory>();
        root.Register<ImportsContext>();
        return root;
    }
}

// Defra.Imports.Plugins\WatchLists\WatchListInitialisationPlugin.cs
namespace Defra.Imports.Plugins.WatchLists
{
    using Defra.Imports.Infrastructure.Plugins;

    /// <summary>Example only: do not register this as a real business process.</summary>
    public sealed class WatchListInitialisationPlugin : PluginBase<WatchListInitialisationHandler>
    {
        public WatchListInitialisationPlugin()
            : base(PluginCompositionRoot.Root)
        {
        }
    }
}

// Defra.Imports.Plugins\WatchLists\WatchListInitialisationHandler.cs
namespace Defra.Imports.Plugins.WatchLists
{
    using System;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Plugins;
    using Microsoft.Xrm.Sdk;

    public sealed class WatchListInitialisationHandler : IPluginHandler
    {
        private const string CreateMessage = "Create";
        private const string TargetParameter = "Target";
        private const int PreOperationStage = 20;
        private const int SynchronousMode = 0;

        private readonly IWatchListInitialiser initialiser;
        private readonly IPluginExecution execution;

        public WatchListInitialisationHandler(IWatchListInitialiser initialiser, IPluginExecution execution)
        {
            this.initialiser = initialiser ?? throw new ArgumentNullException(nameof(initialiser));
            this.execution = execution ?? throw new ArgumentNullException(nameof(execution));
        }

        public void Execute()
        {
            var context = this.execution.Context;
            if (context.MessageName != CreateMessage
                || context.PrimaryEntityName != defraimp_WatchList.EntityLogicalName
                || context.Stage != PreOperationStage
                || context.Mode != SynchronousMode
                || !context.InputParameters.TryGetValue(TargetParameter, out var value)
                || !(value is Entity target))
            {
                throw new InvalidOperationException(
                    "This example requires a synchronous pre-operation watch list Create with a Target.");
            }

            var watchList = target.ToEntity<defraimp_WatchList>();
            var initialised = this.initialiser.GetInitialisedWatchList(watchList);

            // The pipeline persists these values; no Update is issued.
            var changes = initialised.GetChanges(watchList);
            foreach (var change in changes.Attributes)
            {
                target[change.Key] = change.Value;
            }

            this.execution.Tracing.Trace("Watch list initialised: {0} value(s) applied.", changes.Attributes.Count);
        }
    }
}
```

**CWA host.** The handler makes a single call to the persisting command. `WorkflowCompositionRoot` registers `InitialiseWatchListHandler` and services once, and does **not** register `InitialiseWatchListActivity` as a constructible type.

```csharp
// Defra.Imports.Workflows\WorkflowCompositionRoot.cs
using System;
using Defra.Imports.Application.WatchLists;
using Defra.Imports.Dataverse.Contexts;
using Defra.Imports.Dataverse.Model;
using Defra.Imports.Dataverse.Queries;
using Defra.Imports.Infrastructure.DependencyInjection;
using Defra.Imports.Workflows.WatchLists;

internal static class WorkflowCompositionRoot
{
    private static readonly Lazy<PluginContainer> RootValue =
        new Lazy<PluginContainer>(BuildRoot);

    internal static PluginContainer Root => RootValue.Value;

    private static PluginContainer BuildRoot()
    {
        var root = new PluginContainer();

        // ILLUSTRATIVE EXAMPLE registrations: not a real business process; do not register with
        // Dataverse or copy as a template (comment abridged; see the ADR).
        // The activity is host-created, so only its handler is registered here.
        root.Register<IWatchListInitialiser, WatchListInitialiser>();
        root.Register<WatchListQueries>();
        root.Register<InitialiseWatchListHandler>();

        root.Register<IImportsContextFactory, ImportsContextFactory>();
        root.Register<ImportsContext>();
        return root;
    }
}

// Defra.Imports.Workflows\WatchLists\InitialiseWatchListActivity.cs
namespace Defra.Imports.Workflows.WatchLists
{
    using System.Activities;
    using Defra.Imports.Infrastructure.DependencyInjection;
    using Defra.Imports.Infrastructure.Workflows;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Workflow;

    /// <summary>Example only: do not register this as a real business process.</summary>
    public sealed class InitialiseWatchListActivity : WorkflowActivityBase<InitialiseWatchListHandler>
    {
        public InitialiseWatchListActivity()
            : base(WorkflowCompositionRoot.Root)
        {
        }

        [Input("Watch list")]
        [ReferenceTarget("defraimp_watchlist")]
        [RequiredArgument]
        public InArgument<EntityReference> WatchList { get; set; }

        protected override void RegisterActivity(PluginContainer scope)
        {
            scope.Register<InitialiseWatchListActivity>(this);
        }
    }
}

// Defra.Imports.Workflows\WatchLists\InitialiseWatchListHandler.cs
namespace Defra.Imports.Workflows.WatchLists
{
    using System;
    using System.Activities;
    using Defra.Imports.Application.WatchLists;
    using Defra.Imports.Dataverse.Model;
    using Defra.Imports.Infrastructure.Workflows;

    public sealed class InitialiseWatchListHandler : IWorkflowActivityHandler
    {
        private readonly InitialiseWatchListActivity activity;
        private readonly CodeActivityContext context;
        private readonly IWatchListInitialiser initialiser;

        public InitialiseWatchListHandler(
            InitialiseWatchListActivity activity,
            CodeActivityContext context,
            IWatchListInitialiser initialiser)
        {
            this.activity = activity ?? throw new ArgumentNullException(nameof(activity));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.initialiser = initialiser ?? throw new ArgumentNullException(nameof(initialiser));
        }

        public void Execute()
        {
            var reference = this.activity.WatchList.Get(this.context);
            if (reference == null
                || reference.Id == Guid.Empty
                || reference.LogicalName != defraimp_WatchList.EntityLogicalName)
            {
                throw new ArgumentException("A valid watch list reference is required.");
            }

            // The retrieve, initialisation and delta update are one application command.
            this.initialiser.InitialiseWatchList(reference.Id);
        }
    }
}
```

**Shared path.** Both hosts resolve `IWatchListInitialiser` to the same shared-source `WatchListInitialiser`, which calls the same `WatchListQueries.GetWatchListInitialisationContext`.

- Plug-in: `WatchListInitialisationPlugin` → `WatchListInitialisationHandler` → `GetInitialisedWatchList`; the handler copies changed values onto the `Target`.
- Workflow: `InitialiseWatchListActivity` → `InitialiseWatchListHandler` → `InitialiseWatchList` → `GetWatchListForInitialisation`, then `GetInitialisedWatchList`, then `IOrganizationService.Update` of the changed values.

Only the boundary adapters differ: which application method they call and how the result is persisted. Each head compiles the shared source into its own assembly with its own root, scope and service instances. Unit tests should assert that the input entity is never mutated, for both hosts. Neither workflow nor plug-in handler reuses the other's host-specific types; neither `IPluginExecutionContext` nor `IPluginExecution` exists in a workflow scope.

### Testing the pattern

Reusable test support lives in `tests\Defra.Imports.UnitTests\Support` (confidence **High**: used by the example's passing tests):

- `PluginTestBase<TPlugin>` and `WorkflowActivityTestBase<TActivity>` supply a simulated host invocation and the common assertions (a failure that must not touch the strict `IOrganizationService` mock), so derived tests describe only the step or activity contract.
- `PluginRuntime` simulates the services Dataverse supplies to one plug-in invocation. `ForStep(message, entity, stage, mode)`, `ForPreOperationCreate(target)` and `WithTarget(target)` describe the registration the handler validates.

Constraints discovered while testing:

- Test a CWA handler through its activity and `WorkflowInvoker`, because `CodeActivityContext` cannot be constructed. `WorkflowInvoker` wraps failures, so search the inner exceptions for the expected type.
- LINQ queries execute `Execute(RetrieveMultipleRequest)`, so set up `Execute` on the service double rather than `RetrieveMultiple`.
- Mocks must return typed (early-bound) entities, as the sandbox does, or anonymous projections fail.
- A strict `IOrganizationService` mock is the simplest proof that an invalid invocation read and wrote nothing.

## Alternatives Considered

| Option | Why rejected |
| ------ | ------------ |
| **Third-party lightweight container (NuGet)** | Every deployed assembly must stay self-contained for sandbox registration; a bundled package adds licensing, update and assembly-binding surface for very little saved code. A small, owned, tested container is proportionate. |
| **Status quo (manual `new` composition per plug-in)** | Already duplicated across 17 plug-ins, no scoping discipline (nothing stops capturing a per-execution object in a static field), and hard to unit test without a real `IServiceProvider`. Viable for small one-off plug-ins but does not scale. |
| **One composition root per plug-in** | All plug-ins in `Plugins.dll` can share one immutable registration root; per-plug-in roots duplicate registrations and risk drift without improving isolation (a fresh scope per call provides that). `Workflows.dll` keeps its own root as a separate assembly. |
| **Task-returning handlers bridged synchronously** | No asynchronous requirement or SDK operation exists; the contract and blocking bridge add a failure mode and maintenance for nothing. Reconsider only if a supported requirement changes the execution model. |
| **Full layered rewrite migrating business logic now** | Out of scope (user mandate: no business logic migration); a large, high-risk change to 17 live steps contrary to incremental delivery and proportionate architecture. |
| **Single `PluginBase<THandler>` abstraction shared by CWAs** | `CodeActivityContext` has no `IPluginExecutionContext` equivalent and no `ILogger`; reusing plug-in-dependent handlers would fabricate context CWAs do not supply. |
| **`WorkflowActivityBase<TService>`** | Selects one service per callback and leaves argument mapping and orchestration on the activity; handler-first was requested and is preferred. |
| **Manual scope in each activity** | Lower initial change but duplicates lifecycle and error handling; acceptable only as a transitional technique. |
| **Keep the `Model` project and Application handlers; only add DI** | Least churn but preserves the layer ambiguity and shares plug-in-only handlers. Handlers may stay in `Application` only as a short-lived transition when a migration cannot safely move them. |
| **Move all legacy logic and entry points at once** | Unnecessarily broad and high-risk; migrate per use case. |
| **AI/agentic option** | Not relevant to internal composition/testability plumbing. |

**Reuse assessment:** `PluginContainer`, `PluginBase<THandler>`, `WorkflowActivityBase<THandler>` and `IPluginExecution` are reusable platform capabilities, with every future step an expected consumer. The watch list example is not.

The target layout is a proportionate structural improvement, not a rewrite of unrelated components.

## Consequences

- **Positive:** explicit constructor-injected dependencies; one place for scope plumbing; layer ownership that keeps plug-in and workflow host concerns out of `Application`; clearer persistence effects; analyser warnings in migrated code are addressed rather than carried forward; disposal failures are aggregated rather than swallowed.
- **Negative:** each migration incurs namespace, `.projitems`, registration and test updates, and must be behaviour-characterised to avoid changing target mutation, write timing, transactions, exceptions or workflow contracts. Fail-fast resolution errors are new failure modes for support staff.
- **Cost and licensing:** no new licence, connector, runtime dependency or infrastructure; one-off engineering and regression effort per migration.
- **Other solutions and environments:** no schema, registration or deployed behaviour change is authorised. Shared-source namespace changes require checking all consumers and rebuilding both heads and the UnitTest/Container path, preserving assembly and entry-point identities. The ephemeral Dev/Test and Package Deployer model is unchanged.
- **Technical debt impact:** **Reduces**, once a slice is migrated and its warnings cleared; not yet verified by a representative slice.
- **Architecture debt impact:** **Reduces**. Mixed `Model` ownership is **Eliminated**; handler placement is **Deferred** until slices migrate.

## ALM Considerations

- **Configuration/reference data:** none introduced (no table, choice, environment variable or migration record), so `data/core`/`data/seed`/Package Deployer import guidance does not apply.
- **Deployability:** no manual step. The new shared projects use the existing MSBuild import mechanism; nothing is created by hand in an ephemeral environment. Both heads remain single self-contained DLLs for sandbox registration, and the `UnitTest`-only `Container.dll` routing is unchanged.
- **Non-breaking change:** existing plug-in classes and `WorkFlowActivityBase` are untouched, and no registered step, image or managed action changes. The unused `PluginBase` scaffold is deleted outright (not a registered component, so no deprecation entry is needed).
- **Execution identity:** no credentials, environment-specific user IDs, security-role changes or elevated operations. Any later non-default workflow execution configuration or privileged operation needs a reviewed, solution/package-deployable path, never manual per-environment setup.
- **Registration:** author registrations with the Plug-in Registration Tool in the isolated development environment, include the assembly, steps and images in the owning solution, commit the exported metadata and deploy through the Package Deployer package, so rebuilt environments need no manual re-registration. Registration attributes in source are not the deployment mechanism.
- **Workflow adapter:** additive shared infrastructure only. Existing activity type names, argument contracts, metadata and workflow dependencies are preserved on later migration.

## NFR Assessment

| Category        | Materiality         | Assessment |
| --------------- | ------------------- | ---------- |
| Security        | Impacted            | Default bindings retain the executing user; non-default identities are explicit, host-specific and least-privilege; unsupported workflow identities fail rather than silently impersonate. Review any SYSTEM operation and validate effective access in Dataverse. No new roles or credentials. |
| Performance     | Impacted            | Root initialisation, scope creation/disposal and reflective resolution add overhead, not yet measured. Measure cold and warm paths before first adoption and validate total execution in Dataverse. The plug-in time budget is not a workflow threshold. |
| Scalability     | Low relevance       | No change to volume or throughput; concurrency isolation of the composition layer is tested. |
| Availability    | Low relevance       | A resolution failure fails the transaction as an unhandled plug-in exception does today. |
| Maintainability | Impacted (positive) | Centralised composition and scope plumbing, explicit dependencies, argument adaptation moved out of activities, at the cost of a small handler contract and per-activity binding override. |
| Supportability  | Impacted            | Fail-fast explicit exceptions (missing service, missing registration, cycle, duplicate registration) are easier to diagnose than silent manual construction but are new to operations staff. |
| Observability   | Impacted            | `ITracingService` is always borrowed and used; `ILogger`/Application Insights remains conditional and environment-dependent. |
| Auditability    | Low relevance       | No change to Dataverse audit behaviour. |
| Compliance      | Not applicable     | No data classification or regulatory impact. |
| Recoverability  | Low relevance       | Disposal failures are aggregated and surfaced, a minor improvement. |
| Data residency  | Not applicable     | No data movement. |
| Accessibility   | Not applicable     | No user-facing surface. |

## Risk Register

| Description | Category | Likelihood | Impact | Mitigation | Owner |
| ----------- | -------- | ---------- | ------ | ---------- | ----- |
| A handler uses `async void` or starts background/parallel work, letting work or exceptions escape the invocation. | Technical / Architecture debt | Low | High | Synchronous `void` contract, explicit prohibition and review; assess an analyser only if adoption shows the need. | Unassigned |
| Unit tests exercise `Container.dll`'s copy of the shared code, not the IL in Release `Plugins.dll`/`Workflows.dll`. | Technical debt | Low | Medium | Integration/E2E suites (`Defra.Imports.IntegrationTests`, `Specs`, `Scenarios`) exercise the deployed solution; reflection checks confirmed Release assemblies carry no `Container` reference. | Unassigned |
| No real plug-in or CWA uses the pattern, so it is unproven against a real message, entity, composition root and registered activity. | Architecture debt | Medium | Medium | Treat the first real adoption as the validation activity; raise confidence to High for that scenario only after it lands, is registered and reviewed. | Unassigned |
| The illustrative example is compiled into the shipped assemblies and is mistaken for a real capability, registered, or copied as a template. | Architecture debt / Operational | Low | Medium | Every example type carries an `ILLUSTRATIVE EXAMPLE` summary referencing this ADR; no step, activity registration or solution metadata refers to it; review any registration or extension of it as a new adoption. Consider removing or replacing it when the first real adoption lands. | Unassigned |
| `ILogger`-dependent handlers silently become plug-in-only. | Supportability | Low | Low | Documented convention; consider naming/interface conventions for CWA-portable handlers. | Unassigned |
| A cached activity retains a resolved service or invocation values, leaking state across executions. | Technical / Security | Low | High | Scope, services and values are local to the sealed execution path; tests cover a reused activity invoked concurrently, validation, output mapping and disposal. Unverified under real workflow-host concurrency. | Unassigned |
| An identity provider returns the wrong user, or a context is reused across identities. | Security / Technical | Medium | High | Executing-user defaults, separate plug-in/workflow implementations, `NotSupportedException` for unsupported identities, invocation-local contexts with caller-owned disposal. Mocks verify factory arguments only; validate effective identity and access-denied behaviour in Dataverse. | Unassigned |
| Refactoring changes registered type/argument identities, persistence timing or error/output contracts. | Technical / Operational | Medium | High | Preserve the confirmed migration contracts, characterise existing behaviour and require approval for bug fixes or behaviour changes. | Unassigned |
| A move changes shared-project inclusion or leaves host-specific dependencies in `Application`, causing duplicate types, build failures or coupling. | Architecture debt | Medium | Medium | One vertical slice at a time; update `.projitems` and namespaces atomically; handlers stay in their heads; build both heads and UnitTest after each move. | Unassigned |
| Existing analyser warnings are carried forward or hidden by suppressions. | Technical debt | Medium | Low | Baseline the migrated files, resolve warnings in touched code without blanket suppressions, review unrelated baseline warnings separately. | Unassigned |
| Disposal failure masks the primary exception. | Technical / Supportability | Low | High | Failure precedence applied and tested in both adapters; secondary failures traced. `WorkflowInvoker` reduces the aggregate to its first inner exception for external callers. | Unassigned |

## Technical and Architecture Debt Register

| Description | Classification | Review/retirement criteria |
| ----------- | -------------- | -------------------------- |
| Unused non-generic `PluginBase` and its context/tracing helpers. | **Eliminated**: deleted; a repository-wide source search found no remaining reference. | None. |
| Constructor summaries needing the analyser-required `Initializes` wording. | **Eliminated**: no SA1642/SA1615 warnings from the scaffold in either head's Release build (a large unrelated repository-wide warning set remains). | None. |
| Mixed `Model` ownership of schema types and constants/reference data. | **Eliminated**: schema types in `Dataverse.Model`; `Constants`/`ReferenceData` in `Application`; `Model` project retired. | Routine review only. |
| Plug-in and workflow handlers placed outside their host's namespace (including in `Application`). | **Deferred** until each use case migrates. | Complete with a real migration; update `.projitems` and namespaces, remove old inclusions, confirm both heads and UnitTest build, and check for duplicate types and analyser warnings in the migrated scope. |
| `WorkflowActivityBase<THandler>` has no registered adopter. | **Deferred adoption validation**. | Register a real activity, verify in an isolated Dataverse environment (injection, lifecycle, argument mapping, transactions), then raise confidence to High for that scenario. |
| Identity provider and non-default context factory verified with mocks only. | **Deferred adoption validation**. | Validate effective users and access boundaries in an isolated environment before any identity-sensitive adoption. |
| `IFetchService` extension-override test seams in existing activities. | **Deferred** until those activities migrate. | Replace with test-specific DI composition at migration. |

## Fitness Check

- **Classification:** Review Level **3 (new pattern)**, Complexity **Medium**: a new cross-cutting composition pattern for every future plug-in step, within one solution and without cross-solution or licensing impact. This ADR is the only artefact, with adoption guidance folded in. No separate SAD, Reusable Capability Assessment or Operational Support Model is proportionate, because nothing is registered or deployed differently. Revisit at Level 2 depth (a targeted update, not a new ADR) when a real step first adopts the pattern. The workflow adapter and migration-boundary rules are Level 2 / Medium extensions.
- **Traceability:** no originating work item exists. **ADR alignment:** no prior ADR exists.
- **Verification performed:**
  - Both heads build in Release without errors: `dotnet build src\pluginassemblies\Defra.Imports.Plugins\Defra.Imports.Plugins.csproj --configuration Release` and the `Defra.Imports.Workflows` equivalent. The illustrative example is compiled into both heads. The `UnitTest` configuration reports analyser/XML-documentation warnings (no baseline comparison performed), none failing the build or tests.
  - `dotnet test tests\Defra.Imports.UnitTests\Defra.Imports.UnitTests.csproj -p:Configuration=UnitTest` passed in full: 418 passed, 8 skipped (existing `DetermineInspection` tests), 0 failed. The example is exercised through `Container.dll` under `UnitTest`.
  - The `Defra.Imports.UnitTests.Infrastructure` filter (`--filter FullyQualifiedName~Defra.Imports.UnitTests.Infrastructure`) passed 80/80, including a case asserting the plug-in provider passes `Guid.Empty` through to `CreateOrganizationService` when the context `UserId` is `Guid.Empty`.
  - Tests pin the `Clone<T>` fixes, `GetChanges` value equality, the `Execute(RetrieveMultipleRequest)` query path, the absent-`ownerid` `InvalidOperationException` (surfaced as `InvalidPluginExecutionException`) and the shared initialiser's never-mutate-the-input rule.
  - Reflection over both Release DLLs found no `Defra.Imports.Container` reference (9 references for `Plugins.dll`, 10 for `Workflows.dll`).
- **Not performed:** a Debug build, a full-solution build, any analyser baseline, performance measurement, and any live Dataverse validation: sandbox type resolution for anonymous LINQ projections, `ownerid` on a stage 20 `Create` `Target`, effective identity, `InitialiseWatchList` write timing and error contract, workflow registration, transactions and deployed behaviour. Nothing was registered or run in a Dataverse environment.

## Adoption Guidance

To adopt the pattern for a **real** plug-in step or CWA:

1. Use a `<YourDomain>` folder convention: targeted application services in `Application`, queries, projections and models in `Dataverse`, handlers beside their host class in `Plugins` or `Workflows`.
2. Add the handler and its registrations to the single `PluginCompositionRoot` (or `WorkflowCompositionRoot`). Never capture a per-execution instance in a root, and never add a per-plug-in root or repeat common registrations.
3. A new plug-in is a `public sealed class <UtilityName>Plugin : PluginBase<YourHandler>` with a parameterless constructor calling `base(PluginCompositionRoot.Root)`. Configure steps and images with the Plug-in Registration Tool and capture them in the owning solution. For an existing entry point, refactor behind its existing type identity and registration.
4. A plug-in handler (`IPluginHandler`, `Plugins` assembly, constructor-injected) validates the step contract, obtains the entity from the context, calls the application method without passing anything that forces an extra read, and applies the result to the target. It implements synchronous `void Execute()`.
5. A new CWA uses `WorkflowActivityBase<THandler>`, not `PluginBase<THandler>`. Its parameterless constructor passes `WorkflowCompositionRoot.Root`, `RegisterActivity(scope)` binds `this` under its concrete type, and its handler validates inputs, calls the application method and persists the result or sets outputs. Preserve existing activity contracts on migration. SDK argument attributes define workflow inputs and outputs, not deployment registrations.
6. Keep the roots in their own heads; the workflow head must not reference the plug-in head or reuse its static container.
7. Start with one small use case. Characterise success and failure behaviour, target mutations, outputs and persistence timing before replacing composition. Apply the confirmed migration constraints, move handlers, query construction and rules into their owning layers (preferring business-oriented query methods to a generic repository), and update `.projitems` inclusions and namespaces atomically so UnitTest still compiles shared types once through `Container`.
8. Test the first adoption in an isolated Dataverse environment (transactions, exceptions, tracing, CWA reuse). Local tests and self-contained builds do not replace this. Write the unit tests first with the support described in "Testing the pattern".
9. Changes to shared source or the plug-in assemblies need a manual version increment, because they sit outside the solution metadata folder. The shipped solution is `defra_Imports` (`src\solutions\defra_Imports\defra_Imports.cdsproj` references both assemblies), so include `+solutionVer(defra_Imports): patch` (or higher) in the commit body, as the example's own commit does. Link the Azure Boards item with `AB#` in the PR title and body (none was supplied here).

## Adoption Record: `PopulateAnimalNumber` (first migration)

The first existing plug-in, `Defra.Imports.Plugins.ParameterSetKeyDataPair.PopulateAnimalNumber`, now follows the pattern. It is proven by unit tests only; it has not been validated in a live Dataverse environment.

- **Entry point identity:** the class name, namespace and assembly are unchanged, so the two registered steps (post-operation, synchronous `Create` and `Update` of `defraimp_parametersetkeydatapair`, with the `PreImage` on `Update`) need no re-registration. The `[CrmPluginRegistration]` attributes were removed in line with "Configure steps and images with the Plug-in Registration Tool, never code attributes". They were not read by any tool and had drifted from the solution metadata (execution order and the `Create` filtering attribute).
- **Layers:** the entry point stays `PopulateAnimalNumber` and keeps its handler name, `PopulateAnimalNumberHandler`, which sits beside it, but the work is generic. The handler projects the key data pair with the `ToApplyCommodityComplementKeyDataContext` extension method (`ParameterSetKeyDataPairExtensions`, in `Defra.Imports.Dataverse.Extensions`) onto a small immutable context (`ApplyCommodityComplementKeyDataContext`, in `Defra.Imports.Dataverse.Projections`) holding the key, the value and the importer notification and parameter set identifiers (with the parameter set name, if the reference carried one), and passes it to `ICommodityComplementKeyDataApplier.Apply` (`Defra.Imports.Application.CommodityComplements`). The applier owns the business rule, the explicit `Update` and the map of understood keys (`number_animal` to `defraimp_numberofanimals`). `CommodityComplementQueries` (`Defra.Imports.Dataverse.Queries`) holds the two early-bound LINQ queries that replaced the `QueryExpression` and `Retrieve` calls. The handler owns the `Update` pre-image merge, because it is a host concern. The key name is `KeyDataPairKeys.NumberOfAnimals` in `Application.Constants`.
- **Shared plug-in constants:** message names, stages, modes and the `Target` parameter name live in `Defra.Imports.Infrastructure.Plugins` (`PluginMessages`, `PluginStages`, `PluginModes`, `PluginInputParameters`) rather than as private constants in each handler. The watch list example still declares its own and was not changed.
- **Shared logging moved:** `ILogWriter`, `Severity` and `TracingServiceLogWriter` moved from `Defra.Imports.BusinessLogic.Logging` to `Defra.Imports.Infrastructure.Logging` and all unmigrated consumers were updated. `TimestampedTracingLogWriter` is a single-constructor subclass that the container can resolve and that timestamps messages as the legacy `Plugin` base class did. The unused `ConsoleLogWriter` and the legacy `AddSpeciesAnimalNumbers` were deleted.
- **Behaviour preserved:** the catch-and-continue contract for failures after the step contract is validated (each is traced as an error and swallowed), the warning messages, the unconditional `Update` and the use of the first pre-image whatever its name. Each is flagged with a code comment for investigation. The handler validates the registered step contract (entity, stage, mode, message, `Target`) before the catch and fails loudly with `InvalidOperationException`, surfaced as `InvalidPluginExecutionException`. This is an approved change in behaviour for a misconfigured registration only.
- **Items for later investigation (preserved, commented in code):** swallowing service faults inside a transaction, the unconditional `Update`, the first-match lookup with no `TopCount`, a pre-image overlay that nulls `defraimp_data` when the `Target` omits it, and use of the first pre-image whatever its name. The warning tag changed from `AddSpeciesAnimalNumbers` to `CommodityComplementKeyDataApplier` and the warning texts are now key-neutral (for example "Cannot apply key data pair 'number_animal' - …"); both are trace-only changes.

## Adoption Record: `FlagRecordOnWatchList` and `FlagRecordOnWatchListNotification` (second migration)

The two plug-ins that flag a new ITAHC or importer notification on the active watch lists now follow the pattern. They are proven by unit tests only; they have not been validated in a live Dataverse environment.

- **Entry point identity:** `Defra.Imports.Plugins.Itahc.FlagRecordOnWatchList` and `Defra.Imports.Plugins.ImporterNotification.FlagRecordOnWatchListNotification` keep their names, namespaces and assembly, so the two registered steps (post-operation, synchronous `Create` of `defraimp_itahc` and `defraimp_importernotification`, filtering on the entity's own ID, no images) need no re-registration. The `[CrmPluginRegistration]` attributes were removed; they agreed with the solution metadata.
- **Layers:** each plug-in has a handler beside it (`FlagRecordOnWatchListHandler`, `FlagRecordOnWatchListNotificationHandler`) that validates the step contract, copies the `Target` with `ToEntity` and projects it with the `ToCreateWatchFlagsContext` extension method (`ItahcExtensions` and `ImporterNotificationExtensions`, one class per typed entity in `Defra.Imports.Dataverse.Extensions`) onto a small immutable context (`CreateItahcWatchFlagsContext`, `CreateNotificationWatchFlagsContext`, in `Defra.Imports.Dataverse.Projections`), which it passes to `IWatchFlagCreator` (`Defra.Imports.Application.WatchLists`). The extensions and contexts are in `Dataverse` rather than `Application` so that any future query class can reuse the projections without `Dataverse` depending on `Application`. The contexts hold only what the use case reads: a reference to the record, its name and the economic operator names. `WatchFlagCreator` owns the rules: the map from watch type to watch list lookup, the check order and the explicit `Create` of each `defraimp_watchflag`. `WatchFlagQueries` (`Defra.Imports.Dataverse.Queries`) holds the two early-bound LINQ queries that replaced the repository `Find` and the `QueryExpression`; `FindEconomicOperatorId` takes the watch type and selects the matching operator table, so no table or attribute name is held outside the generated model.
- **Contexts instead of entities:** a use case that needs only a handful of attributes takes a context rather than the whole typed entity, so its inputs are explicit and it cannot read or depend on attributes the handler did not intend to supply. The projection copies values, so the service cannot mutate the `Target`, and nothing forces a re-read. A `Create` `Target` holds only the attributes that were supplied, so a projected attribute that was not supplied reads as null, exactly as the legacy code read it; the projections do not validate that attributes are present. This refines the "Methods accept the `Entity` the host already holds" rule in "Layer ownership" for narrow inputs.
- **Deleted:** `FlagRecordsOnWatchListBase`, `FlagRecordParams`, `FlagRecordOnWatchListBusinessLogic`, `FlagRecordsOnWatchListNotificationBusinessLogic`, their `.projitems` entries and their two test classes (all five cases were ported).
- **Behaviour preserved:** the date filter, the single watch list read (made even when no identifier is given), the check order, interleaved reads and creates, one flag at most per identifier, the flag name built from the Target's name, and no catch.
- **Step-contract validation:** each handler validates the entity, stage (post-operation), mode (synchronous), message (`Create`) and `Target` and fails loudly with `InvalidOperationException`, surfaced as `InvalidPluginExecutionException`. This changes behaviour for a misconfigured registration only. Separately, `PluginBase` wraps any other failure in an `InvalidPluginExecutionException` (the legacy base let the raw exception escape), and the unused legacy trace writer is no longer created.
- **Items for later investigation (preserved, commented in code):** `DateTime.Now` (server local time, not UTC); no paging of the watch list read; the economic operator lookup has no ordering, so only the first operator with a name is checked; only the first matching watch list is flagged; no guard against duplicate flags; the flag name is "`<watch list> - `" when the `Target` omits `defraimp_name`.

## Open Questions / Assumptions

- **Approval pending:** obtain normal review approval before migrating production behaviour or registering any component.
- **First adoption is the validation:** only the unregistered example and the unit suite exist; update this ADR (not a new one) when the first real adoption lands, and consider removing or replacing the example at that point.
- **Registered-environment validation:** `WorkflowActivityBase<THandler>` has not been verified in a live, registered activity (host injection, lifecycle, argument mapping, concurrency), and effective identity, impersonation and least-privilege access are unverified.
- **Migration conformance:** the confirmed constraints and layer boundaries must be verified in the first representative migration of an existing plug-in or activity, raising any bugs or questionable practices for approval. The analyser baseline per migrated file is captured at that point.
- **Performance** of root initialisation, scope creation/disposal and reflective resolution is unmeasured; measure before any performance-sensitive adoption.
- **Illustrative example** (implemented and unit-tested; not validated in a live Dataverse environment):
  - **Resolved:** the LINQ provider, `Clone<T>` and `GetChanges` equality questions are answered in the "Illustrative example" notes. Only live-sandbox confirmation of anonymous projections (typed entities via `ProxyTypesAssemblyAttribute`) remains.
  - **Open:** `ownerid` on a stage 20 `Create` `Target` is unverified; it needs an isolated environment. The example throws an explicit `InvalidOperationException` (surfaced as `InvalidPluginExecutionException`) when it is absent, and a unit test covers that case. A real initialiser needs an explicit rule, such as defaulting to the initiating user.
  - **Open:** effective-identity behaviour (see registered-environment validation above), including that the executing-user `IOrganizationService` the example's `Update` uses behaves as documented.
  - **Open:** write timing and error contract of `InitialiseWatchList`, which issues `IOrganizationService.Update` directly (executing user, no new transaction or retry); characterise both in an isolated environment.
  - **Open:** performance. Cloning and diffing add per-execution allocation, and the projection costs up to two requests; measure before optimising. Mandatory related tables can use a single `join`; unrelated tables could use `ExecuteMultiple` (sequential). Parallel calls are not permitted.
  - `Clone` is shallow: logic must replace mutable attribute values (`EntityReference`, `OptionSetValue`, `EntityCollection`) rather than mutate them. Sending only changed values avoids needless downstream plug-in, flow and audit triggers.
