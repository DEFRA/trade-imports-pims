---
name: bdd-scenario-generation
description: 'Generate Behaviour Driven Development (BDD) Given/When/Then scenarios from requirements, user stories and acceptance criteria. Use when writing Gherkin-style scenarios, generating happy-path/error/validation/edge-case scenarios, verifying acceptance criteria coverage, identifying missing behavioural requirements, or flagging ambiguities that impact testability. Follows requirements-discovery and backlog-generation; precedes automated test authoring.'
---

# BDD Scenario Generation

## Purpose

Convert requirements, user stories, and acceptance criteria into Given/When/Then scenarios that describe observable system behaviour. This skill generates scenarios and analyses coverage; it does not write automated test code or step definitions.

## When to Use

- A user story or acceptance criteria set needs to be expressed as testable BDD scenarios.
- Acceptance criteria exist but their behavioural coverage (happy path, error, validation, edge case) has not been checked.
- Stakeholders, developers and testers need a shared, business-readable description of expected behaviour before implementation or test automation begins.

## Expected Inputs

Requirements, user stories (As a/I want/So that), acceptance criteria, business rules — in any combination. If none of these are supplied, ask for at least one before generating scenarios.

## Working Principles

- Focus on behaviour rather than implementation — describe what the system does, not how.
- Prefer multiple simple scenarios over one complex scenario.
- Use concise British English and business language a non-technical stakeholder can follow.
- Keep scenarios understandable by business and technical stakeholders alike.

## Scenario Standards

- Focus on observable behaviour and outcomes, not internal mechanisms.
- Use business language; avoid UI element names, field IDs, API calls, or code references.
- Keep each scenario independent — it must not rely on the state left behind by another scenario.
- Express the outcome (`Then`) clearly and, where possible, in a way that is directly verifiable.
- One behaviour per scenario — split rather than combine conditions with "and" in the `When`/`Then` steps.

## Procedure

1. **Confirm the source material** — identify the requirement, user story or acceptance criteria being converted. If the business objective is unclear, treat it as a blocking question rather than assuming.
2. **Identify the feature** the scenarios belong to, and name it from the user's perspective.
3. **Generate happy-path scenarios** — the primary successful flow(s) described by the acceptance criteria.
4. **Generate validation scenarios** — required fields, format rules, boundary values, and business rule constraints.
5. **Generate error scenarios** — failure conditions, system errors, and rejected actions, including the expected user-facing outcome.
6. **Generate edge-case scenarios** where appropriate — unusual but plausible conditions (e.g. concurrency, empty states, limits) that are not covered by the categories above.
7. **Verify acceptance criteria coverage** — map every acceptance criterion to at least one scenario. Do not invent acceptance criteria that were not supplied or implied.
8. **Identify missing behavioural requirements** — behaviours implied by the feature but not covered by any supplied acceptance criteria.
9. **Highlight ambiguities** that impact testability — vague terms, undefined thresholds, or conflicting rules that prevent a scenario from being written unambiguously.

## Output Format

```
Feature: <feature name>

Scenario: <scenario name>
  Given <precondition>
  And <additional precondition>
  When <action or event>
  Then <expected outcome>
  And <additional outcome>
```

Group scenarios under their feature, and label each scenario's category (Happy Path, Validation, Error, Edge Case) either inline or as a preceding heading.

## Additional Analysis

After the scenarios, include:

1. Acceptance Criteria Coverage — which criteria are covered, by which scenario(s).
2. Missing Scenarios — behaviours not covered by supplied requirements or acceptance criteria.
3. Testability Concerns — anything that cannot be verified as written.
4. Ambiguities Detected — unclear or conflicting requirements found during generation.

State "None identified" explicitly for any section with nothing to report rather than omitting it.

## Success Criteria

This skill succeeds when stakeholders, developers and testers share a common, unambiguous understanding of expected behaviour, every supplied acceptance criterion is traceable to a scenario, and any gaps or ambiguities are surfaced explicitly rather than silently resolved.

## Handoff

This skill produces scenario *content*, not automated test code. Automated step definitions or test implementation should be handled by a dedicated test-authoring skill or agent for the target framework.
