# 0001. Lightweight DI Scaffold for Dataverse Plug-ins and Custom Workflow Activities

- **Status**: Proposed. The pattern is implemented and unit-tested, and six plug-ins and two custom workflow activities (CWAs) have been migrated to it in source. Nothing has been validated in a live Dataverse environment. Normal review approval is still required.
- **Date**: 2026-10-22 (last updated 2026-11-19)
- **Related requirement(s)**: None. No Epic, Feature, User Story or `AB#` item was supplied, so no work-item traceability is recorded. One is needed before a pull request is raised.
- **Related ADR(s)**: None.
- **Decision confidence**: **Medium**. The composition and scoping mechanics, the two host adapters and every migrated slice are covered by unit tests, and both deployment heads build in Release. Confidence cannot be High until the migrated components have run in a Dataverse environment (see "Open Questions / Assumptions").

Related documents:

- [Plug-in and custom workflow activity pattern](../../docs/development/plugin-and-workflow-activity-pattern.md): the rules and steps for using the pattern.
- [Plug-in and workflow migration log](../plugin-workflow-migration-log.md): what each migration changed, preserved and left for investigation.

## Context

- Business logic and data access are compiled from Shared Projects into two deployed, sandbox-registered assemblies, `Defra.Imports.Plugins` and `Defra.Imports.Workflows`, and into a non-deployed `Defra.Imports.Container` assembly that only the unit tests reference. Each deployed assembly must stay a single self-contained DLL.
- Before this decision there was no dependency injection. The legacy `Plugin` base class built a `RepositoryFactory` and log writer on every call, each plug-in built its own business-logic classes, and the CWAs used several unrelated base classes. An earlier, unused `PluginBase` attempt at the same problem had been abandoned.
- Platform constraints, verified against Microsoft Learn guidance:
  - Plug-ins and CWAs must not use asynchronous or parallel execution.
  - `IPlugin` instances are cached and invoked concurrently, so any shared state, including a composition root, must be safe under concurrent calls.
  - `ILogger` is supplied to plug-ins only, and only when Application Insights integration is enabled. CWAs have no equivalent.
  - A synchronous step has a two-second practical budget and a two-minute hard limit, so per-execution overhead matters.

## Decision

Adopt a small, owned dependency injection scaffold and a three-layer shared-source structure for code-based plug-ins and CWAs, and migrate the existing ones to it one vertical slice at a time without changing their observable behaviour.

1. **Three Shared Projects** are compiled into both deployment heads and `Container`:

   | Project | Responsibility |
   | ------- | -------------- |
   | `Defra.Imports.Infrastructure` | The embedded `PluginContainer`, the host adapters `PluginBase<THandler>` and `WorkflowActivityBase<THandler>`, the handler contracts, the identity-aware service provider and shared logging. |
   | `Defra.Imports.Application` | Targeted, use-case-shaped services and business rules, with `Constants` and `ReferenceData`. No handlers. |
   | `Defra.Imports.Dataverse` | Generated early-bound types and `ImportsContext`, the context factory, query classes, and the projections that carry narrow inputs to application services. |

   The legacy `Defra.Imports.Model` project is retired.

2. **One registration root per deployment head.** `PluginCompositionRoot` and `WorkflowCompositionRoot` hold type registrations only and are frozen on first use. Registrations that both heads need are added by one shared method per capability (for example `AddInspectionRequirements`), so the roots cannot drift apart.

3. **One scope per execution.** Each adapter creates an execution scope, lends it the services Dataverse supplied for that call, resolves the handler graph from it and disposes it on every path. Nothing per-execution is ever held in a root, a plug-in or an activity.

4. **Handlers sit beside their host.** A plug-in or activity class is a declarative entry point. Its handler adapts the execution context or activity arguments and makes one or two calls to an application service. Handler contracts are a synchronous `void Execute()`.

5. **Existing entry points keep their identity.** Class names, namespaces, assembly identity, step registrations and activity arguments are unchanged, so no step is re-registered and no workflow is reconfigured.

### Scope of the decision

- **This applies to existing code-based logic.** The repository standard prefers low-code and treats plug-ins as the exception. This decision does not change that for new requirements. It governs how the existing plug-ins and CWAs are restructured, because moving them to declarative components would risk runtime differences that a refactoring must not introduce.
- **Behaviour is preserved, with two accepted exceptions:**
  - Each migrated plug-in handler checks the step it is running in (entity, message, stage, mode and required parameters or images) and fails if it does not match. This affects a misconfigured registration only.
  - `PluginBase<THandler>` wraps any exception that is not an `InvalidPluginExecutionException` in one, with a generic message, the original as the inner exception and the detail in the plug-in trace log. The legacy base let the raw exception escape. Deliberate `InvalidPluginExecutionException`s from business code pass through unchanged.
- **Suspected bugs are recorded, not fixed.** Each migration lists them in the migration log for a separate decision.
- **Some infrastructure has no consumer yet.** `IExecutionServiceProvider` (executing user, initiating user and SYSTEM services), `IImportsContextFactory` and the optional `ILogger` registration are available for future use. They are verified against mocks only and must be validated in a Dataverse environment before their first consumer relies on them.

## Alternatives Considered

| Option | Description | Why rejected |
| ------ | ----------- | ------------ |
| Move the logic to declarative components | Replace the plug-ins and CWAs with workflows, actions or flows, in line with the low-code preference. | This is a refactoring of working legacy logic. Re-platforming it would risk runtime differences in transactions, timing and error behaviour, and is a separate decision per capability. |
| Status quo | Keep manual construction in each plug-in. | Construction is duplicated across the plug-ins, nothing prevents per-execution state being captured in a static field, and the CWA base classes are inconsistent. |
| Third-party container | Bundle a lightweight NuGet container. | Each deployed assembly must stay self-contained in the sandbox; a bundled package adds licensing, update and assembly-binding concerns for very little saved code. |
| One composition root per plug-in | Each plug-in owns its registrations. | Duplicates registrations and invites drift without improving isolation, which the per-execution scope already provides. |
| Task-returning handlers | Asynchronous handler contract bridged synchronously. | No asynchronous requirement exists and the platform does not support it. |
| One handler abstraction for both hosts | CWAs reuse `PluginBase<THandler>`. | A CWA has no plug-in execution context or `ILogger`, so the adapter would have to fabricate them. |
| Migrate everything at once | Move all legacy logic and entry points in one change. | Unnecessarily broad and high-risk. |
| AI or agentic option | | Not relevant to internal composition plumbing. |

## Consequences

- **Positive**: dependencies are explicit and constructor-injected; scope, disposal and failure handling live in one place per host; application services are reusable from both hosts and testable without a host; migrated code clears its analyser warnings; resolution of every registered handler is checked by unit tests.
- **Negative**: each migration touches namespaces, registrations and tests and must be characterised carefully to preserve behaviour. A missing registration is a new kind of runtime failure. A small slice needs more files than the legacy equivalent.
- **Cost impact**: no new licence, connector, runtime dependency or infrastructure. One-off engineering and regression effort per migration.
- **Impact on other solutions/environments**: none intended. No schema, registration or solution metadata changes. Both heads remain single self-contained DLLs, and the ephemeral environment and Package Deployer model is unchanged.
- **Technical debt impact**: Reduces, for each migrated slice.
- **Architecture debt impact**: Reduces. Mixed `Model` ownership is eliminated. Two patterns coexist until the remaining 11 plug-ins and the unmigrated CWAs move.

### Non-functional considerations

| Category | Assessment |
| -------- | ---------- |
| Security | The default service is the executing user. Other identities must be requested explicitly, and a CWA request for an identity its host cannot supply fails. No new roles or credentials. |
| Performance | Scope creation and reflective resolution add per-execution overhead that has not been measured. |
| Supportability | Failures are traced with the handler name. Resolution errors (missing registration, missing host service) are new to support staff. |
| Maintainability | Improved: one composition and lifecycle path per host. |

### Risks

| Risk | Likelihood | Impact | Mitigation |
| ---- | ---------- | ------ | ---------- |
| Migrated components behave differently in Dataverse from the unit tests (sandbox type resolution, images, write timing, transactions). | Medium | High | Deploy to an isolated environment and run the integration, specification and scenario suites before merging. |
| Unit tests exercise `Container.dll`, not the code compiled into the Release assemblies. | Low | Medium | The same deployed-environment test run. |
| A migration changes persistence timing, error contracts or output values. | Medium | High | Characterise before migrating; the pattern guide lists what must be preserved; behaviour changes need approval. |
| The identity provider returns the wrong user, or a context is reused across identities. | Medium | High | No consumer exists yet; validate effective identity in Dataverse before the first one. |
| A handler starts asynchronous or parallel work. | Low | High | Synchronous contract and review. |
| A cached plug-in or activity retains per-execution state. | Low | High | Scope and services are local to the adapter's execution path; concurrent-use tests cover both adapters. |

## Open Questions / Assumptions

- **Approval** of this ADR is pending.
- **Live validation** has not been performed for any migrated component. It should cover step and activity execution, transactions and error behaviour, tracing, and anonymous LINQ projections in the sandbox.
- **Performance** of scope creation and resolution is unmeasured.
- **Callers that depend on error text**: no test or source in this repository matches on the generic message that `PluginBase<THandler>` now produces. Flows, integrations or tests outside the repository that match on the previous raw exception text have not been checked.
- **Work item**: an Azure Boards item is needed for traceability.
- **Verification performed on 2026-11-19**: `Defra.Imports.Plugins` and `Defra.Imports.Workflows` build in Release with no errors, and `Defra.Imports.UnitTests` passes under the `UnitTest` configuration (945 passed, none failed or skipped). No Debug or full-solution build, analyser baseline, performance measurement or Dataverse deployment was performed.
