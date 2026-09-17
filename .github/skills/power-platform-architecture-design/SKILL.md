---
name: power-platform-architecture-design
description: 'Apply Power Platform and Dataverse architecture best practice to a design — component placement (declarative vs. plug-in, sync vs. async), Dataverse entity/security/environment/integration/licensing guidance, and this repository''s specific ALM rules for deploying new configuration/reference data and for retiring components without breaking contracts. Use whenever a design touches Dataverse tables, workflows, actions, cloud flows, plug-ins, connectors, Copilot Studio, Power Pages, or introduces/changes/retires a component that must deploy through the ephemeral Dev/Test environment model. Reusable by any Power Platform architecture agent.'
---

# Power Platform Architecture Design

## Purpose

Provide the domain-specific Power Platform and Dataverse design guidance, and the repository-specific ALM rules that make a design safely deployable, so that the calling agent does not need to hold this detail itself. This skill answers "how should this be built on the platform, and how does it get into every environment without a manual step or a broken contract?" — it does not decide *whether* to build it (see `architecture-options-analysis`) or classify the change's review depth (see `architecture-review-classification`).

## Trigger Conditions

Use this skill whenever a design:

- Introduces or changes a Dataverse table, relationship, choice, workflow, action, cloud flow, plug-in, custom API, or connector.
- Requires synchronous, server-side logic (business rule, workflow, action, plug-in) or asynchronous automation (cloud flow).
- Introduces a new configuration/reference record (a rule, parameter, threshold, or lookup value read at runtime).
- Changes or retires an existing component that other solutions, flows, or integrations may depend on.
- Touches security model, environment/tenant strategy, Copilot Studio, Power Pages, or licensing.

## Responsibilities

### Component placement (Dataverse-first design)

- Prefer Dataverse-native, declarative, low-code capabilities (workflows, actions, business process flows, cloud flows) over plug-ins, custom code, external services, custom databases, or additional integration platforms; introduce these only when the platform cannot reasonably satisfy the requirement.
- Cloud flows have no synchronous equivalent. For synchronous, server-side logic, apply this hierarchy: table-scoped business rules → real-time workflows → actions/custom workflow activities → plug-ins. Use custom workflow activities where reusable input/output behaviour is required; use plug-ins only where earlier options cannot satisfy the requirement (e.g. to augment a managed action).
- Decompose high-level, end-to-end processes into their constituent rules/processes/actions rather than a single monolithic flow or workflow, so cross-cutting behaviours (e.g. validation) apply consistently wherever the underlying data or action is used.
- Review existing tables/flows/integrations/APIs/plug-ins/connectors for reuse or extension before proposing a new one, and justify why a new component is required when one is proposed.

### Power Platform architecture guidance

State which of the following are relevant to the requirement at hand, and apply them:

- **Dataverse design** — entity modelling, relationships, choices/option sets, calculated/rollup fields; avoid unnecessary customisation of shared/out-of-the-box entities.
- **Security model design** — business units, security roles, teams, column-level security, least-privilege access.
- **Environment and tenant strategy** — appropriate use of Dev/Test/UAT/Production (and ephemeral) environments; cross-tenant or multi-environment considerations.
- **Integration architecture** — connectors, custom connectors, virtual tables, API-based integration patterns, favouring platform-native options first.
- **Power Automate design** — cloud flow vs. plug-in choice, trigger scoping, error handling, connector licensing.
- **Copilot Studio** — where a conversational/AI-native approach may reduce effort or improve the outcome, and its governance/data implications.
- **Power Pages** — external-facing security, authentication, and content model impact where applicable.
- **Licensing impact analysis** — licence/entitlement consequences (per-user, per-app, premium connectors, add-on capacity).
- **Capacity and scale** — API limits, storage capacity, throughput implications.
- **Reusable design patterns** — encourage configuration tables over hard-coded behaviour, environment variables over environment-specific customisation, reusable cloud flows over duplicated logic, Custom APIs for reusable service boundaries. Discourage large plug-in frameworks, generic processing engines, excessive abstraction layers, bespoke orchestration platforms, and reusable infrastructure built solely for hypothetical future requirements.

### Reference and configuration data migration (repository-specific)

A design introducing a new configuration/reference record is **not complete** until its ALM path into every environment is defined. Because this repository's Dev/Test environments are ephemeral and routinely rebuilt (see [AGENTS.md](../../../AGENTS.md) "Environments"), a manual creation step is never truly "one-off" — it recurs on every rebuild and silently regresses.

- Define the ALM path alongside the schema, as a mandatory part of ALM Considerations in every SAD/solution design/ADR.
- Prefer the repository's existing mechanism: `deploy/Defra.Imports.Deployment` (Capgemini Package Deployer) ships a `data/core` Configuration Migration dataset (`schema.xml`, `export.json`, `import.json`, `extract/`) imported automatically via the `<dataimport>` step in `ImportConfig.xml`/`ImportConfig.Standalone.xml` — distinct from `data/seed` (optional test data, only imported when `ImportSeedData` is set; see `PackageImportExtension.cs`). Add any single/global/environment-independent configuration record to `data/core`.
- State the mechanism explicitly: (a) `data/core` Configuration Migration dataset; (b) an Environment Variable (noting it lacks the change-audit trail some rule tables require); or (c) an existing/extended cloud flow, workflow, or Package Deployer import-extension setup routine (e.g. `PackageImportExtension.AfterPrimaryImport`).
- Manual, undocumented, environment-specific record creation is a **blocking architecture gap**, not an acceptable assumption. Where none of (a)–(c) can satisfy the requirement, flag it as an open, blocking risk requiring redesign or escalation — do not accept manual creation as the resolution.

### Non-breaking change and deprecation management (repository-specific)

This repository builds and evolves **to contract**: never break an existing integration, contract, managed action, or dependent solution. Component deletion is not routine — removing one requires an upgrade step in every downstream environment.

- Prefer additive, backward-compatible change over renaming/removing a component consumed elsewhere; gate new behaviour with a feature flag (Environment Variable or configuration/setting-definition record) rather than a hard cutover.
- Confirm every consumer/dependency of a component (flows, plug-ins, views, forms, integrations, other tables/columns) has been removed or migrated before it is marked obsolete — obsolescence is not a substitute for resolving dependencies.
- Mark obsolete rather than delete: processes/plug-in steps → **Inactive** (`ImportConfig.xml`/`ImportConfig.Standalone.xml`); views/forms → deactivated or admin/support-scoped; tables/columns/choices → retained (potentially hidden/read-only) in the schema.
- Track every obsolete component by adding a row (component type and name) to [`architecture/deprecated-components.md`](../../../architecture/deprecated-components.md) when it is marked obsolete; remove the row once actually deleted.
- Batch actual removal into a discrete, scheduled clean-up activity (its own Feature/User Story) once tracked components are confirmed unused.

## Workflow

1. Identify which of the guidance areas above are relevant to the requirement; state explicitly which are not and why.
2. Apply reuse-before-create and Dataverse-first component placement.
3. Where a new configuration/reference record is introduced, define its ALM path per **Reference and configuration data migration**.
4. Where an existing component is changed or retired, apply **Non-breaking change and deprecation management** and update `architecture/deprecated-components.md`.
5. Summarise the applicable guidance, the placement decision, and the ALM/deprecation mechanism as an **ALM Considerations** block for inclusion in the calling agent's SAD/ADR/solution design.

## Output Standards

Produce an **ALM Considerations** block covering: solution boundaries, managed vs. unmanaged approach, deployment complexity, environment strategy, release/pipeline implications, solution layering, cross-solution dependencies, the explicit configuration-data migration mechanism (a/b/c above) where relevant, and the non-breaking/deprecation approach where a component is changed or retired.

## Escalation Guidance

- Treat any design that would rely on a manual, undocumented post-deployment step as a blocking gap — escalate for redesign rather than presenting the design as complete.
- Escalate proposed deletions that still have unresolved dependents rather than proceeding to mark them obsolete.
- Escalate requirements that conflict with Dataverse-first/reuse-first principles for a compliant alternative, rejection, deferral, or redesign decision.
