---
name: integration-test-design
description: 'Design, implement, and maintain integration tests that validate Dataverse, Logic Apps, Service Bus, and external API behaviour against documented architectural contracts. Use when an implemented solution needs integration test coverage for component interactions, system boundaries, data transformations, security boundaries, or deployment assumptions. Owned by the Quality Engineer agent.'
---

# Integration Test Design

## Intended Agent

This skill may only be used by the Quality Engineer agent. If invoked by another agent: stop, explain that it is owned by the Quality Engineer, and recommend handing off.

## Purpose

Design, implement, and maintain integration tests that validate real system boundaries — Dataverse, Logic Apps, Service Bus, and external APIs — confirming the implementation honours its intended architectural contract, not just incidental implementation detail.

## Boundaries

**Out of scope:**

- Reqnroll step bindings, hooks, and UI-driven scenario automation — `acceptance-test-automation`.
- Defining or changing solution architecture, technical design, or architectural contracts — Solution Architect. This skill validates against contracts; it does not set them.
- Production implementation of business logic — Developer agents.
- Broad consolidation/duplication review across the automation estate — `test-automation-quality-review`.
- Authoring new scenario/data-builder classes — `scenario-builder-implementation`. Invoke it directly when `Defra.Imports.Scenarios` doesn't already have the setup you need.

## Trigger Conditions

- A newly implemented solution component (Dataverse table/plug-in/flow, Logic App, Service Bus integration, external API integration) needs integration test coverage.
- An existing integration test needs updating because an architectural contract or implementation has changed.
- `test-change-impact-analysis` has identified affected integration tests.

## Expected Inputs

- The implementation change or component under test.
- Relevant architecture artefacts — ADRs and SADs under [architecture](../../../architecture/) — as the definition of intended contract behaviour.
- The existing integration test estate:
  - [Defra.Imports.IntegrationTests](../../../tests/Defra.Imports.IntegrationTests/) — MSTest + FluentAssertions tests against Dataverse, Logic Apps, and Service Bus via `Microsoft.PowerPlatform.Dataverse.Client`. Governed by [integration-tests.instructions.md](../../instructions/integration-tests.instructions.md) — new test data setup must use `Defra.Imports.Scenarios`/`ScenarioBuilder`, never `Marktek.Fluent.Testing.Engine`.
  - [Defra.Imports.Scenarios](../../../tests/Defra.Imports.Scenarios/) — shared `ScenarioBuilder`/`Bogus` data builders, reused here and by acceptance tests.

## Process

1. **Identify the boundary under test** — the specific component interaction, system boundary, or contract being validated.
2. **Align to architecture artefacts** — read the relevant ADR/SAD to confirm intended behaviour before writing assertions; validate the *contract*, not incidental implementation detail.
3. **Check for reuse** — search existing integration tests and `Defra.Imports.Scenarios` for a builder that already covers the setup you need. If none exists, hand off to `scenario-builder-implementation` to extend `Defra.Imports.Scenarios` rather than hand-rolling test data in the test itself.
4. **Implement or update** the integration test using MSTest and FluentAssertions, covering whichever of these are relevant to the change: component interactions, system boundaries, Dataverse/Logic Apps/Service Bus integrations, external APIs, security boundaries, data transformations, deployment assumptions.
5. **Verify** — build and run the affected test(s) before considering the change complete.
6. **Flag maintainability concerns** (duplicated setup, brittle assertions) for `test-automation-quality-review` rather than performing an ad hoc broad refactor inline.

## Output Standards

- The implemented/updated integration test(s), committed against [Defra.Imports.IntegrationTests](../../../tests/Defra.Imports.IntegrationTests/), built and passing.
- A short summary of: which boundary/contract was validated, which ADR/SAD it was aligned to, and which existing (or newly added) data builders were used.

## Escalation Guidance

- If the implementation appears to violate a documented architectural contract, do not silently adjust the test to match — escalate to the Solution Architect for a decision on whether the implementation or the contract is wrong.
- If no ADR/SAD exists to confirm intended contract behaviour, flag the gap rather than assuming; recommend the Solution Architect document the decision.
- If a discovered issue is a production defect rather than a test design question, route it to the appropriate Developer agent rather than working around it in the test.
