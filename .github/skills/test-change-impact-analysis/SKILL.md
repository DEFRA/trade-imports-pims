---
name: test-change-impact-analysis
description: 'Determine which BDD scenarios, step bindings, fixtures, integration tests, and test data assets are impacted by a branch/diff or work item, and produce the traceability chain from Work Item through Acceptance Criteria, Scenario, Implementation, Integration Tests, to Production Implementation. Use before implementing or updating tests for a change, to scope the work and avoid missed regressions. Reusable by any delivery agent needing change-scoped test impact.'
---

# Test Change Impact Analysis

## Intended Agent

This skill is primarily used by the Quality Engineer agent, but is reusable by any delivery-focused agent (e.g. a Developer agent scoping regression risk for a production change).

## Purpose

Given a work item, branch, or diff, determine the full scope of test-related artefacts affected by the change, and establish the traceability chain that connects the requirement to its implementation and verification. This skill produces the impact/traceability analysis only — see Boundaries for who acts on it.

## Boundaries

**Out of scope:**

- Implementing or updating the impacted step bindings, fixtures, or integration tests — hand off to `acceptance-test-automation` / `integration-test-automation`.
- Diagnosing root cause of a test failure — `test-failure-triage`.
- Reviewing test code for maintainability/duplication — `test-automation-quality-review`.
- Changing requirements, acceptance criteria, or scenario intent — Product Analyst.
- Changing architecture artefacts or resolving contract ambiguity — Solution Architect.

## Trigger Conditions

- A work item is being picked up for test implementation and its test scope needs establishing before work starts.
- A branch/diff exists against `main` (including uncommitted changes) and its downstream test impact needs to be understood.
- A pull request needs a testing impact summary for reviewers.

## Expected Inputs

- The work item and its acceptance criteria.
- The current branch diff against `main`, including uncommitted changes, where available. Note this cannot always be relied upon — e.g. when writing tests for pre-existing implementation with no diff to inspect.
- The existing test estate: [Defra.Imports.Specs](../../../tests/Defra.Imports.Specs/), [Defra.Imports.IntegrationTests](../../../tests/Defra.Imports.IntegrationTests/), [Defra.Imports.Scenarios](../../../tests/Defra.Imports.Scenarios/).
- Relevant architecture artefacts under [architecture](../../../architecture/), where the change touches a documented contract.

## Process

1. **Establish a baseline** — where a diff exists, diff the current branch against `main` (including uncommitted changes) to identify changed production code, architecture artefacts, and existing tests. Where no diff exists (e.g. a work item picked up before implementation starts), use its acceptance criteria plus the existing scenario/test estate as the baseline instead. Record which mode was used, and state explicitly if neither gives enough information to scope reliably.
2. **Map to test artefacts** — for the change, determine:
   - Which BDD scenarios require implementation updates.
   - Which step bindings require modification.
   - Which automation fixtures require modification.
   - Which integration tests require modification.
   - Which test data assets (`Defra.Imports.Scenarios`) require modification.
   - Which reusable test components should be extended rather than duplicated.
3. **Build the traceability chain** — Work Item → Acceptance Criteria → BDD Scenario → Scenario Implementation → Integration Tests → Production Implementation. Note any broken or missing link explicitly rather than omitting it.

## Output Standards

- **Impact Table**: Artefact type (Scenario / Step Binding / Fixture / Integration Test / Data Asset) / Item / Change required / Reuse vs. new.
- **Traceability Chain**: Work Item → Acceptance Criteria → BDD Scenario → Scenario Implementation → Integration Tests → Production Implementation, with gaps called out explicitly.
- State "No test impact identified" explicitly if the change/work item has none, rather than omitting the section.

## Escalation Guidance

- If a traceability link is missing (e.g. an acceptance criterion with no corresponding scenario), route it to the Product Analyst; if the gap is architectural coverage (e.g. no ADR/SAD defines the contract under test), route it to the Solution Architect. Do not fill the gap yourself.
- If the baseline could not be established reliably (no diff, insufficient work item detail), present the analysis as scoped/partial rather than complete, and state what would resolve it.
