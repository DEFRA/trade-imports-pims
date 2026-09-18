---
name: test-automation-quality-review
description: 'Review automated test code (acceptance step bindings/hooks/fixtures, integration tests, unit tests) for duplication, complexity, naming inconsistency, execution time, and diagnostic quality, and recommend consolidation or anti-pattern fixes. Use when maintaining the maintainability and architecture of an automated test estate, or before/after implementing new test automation to check reuse. Reusable by any agent responsible for automated test code, not just the Quality Engineer.'
---

# Test Automation Quality Review

## Intended Agent

Primarily used by the Quality Engineer agent for the acceptance (Reqnroll) and integration test estates it owns, but reusable by any agent maintaining automated test code (e.g. a Developer agent reviewing unit tests).

## Purpose

Assess the maintainability, architecture, and reuse discipline of an automated test estate — independent of testing layer (acceptance, integration, unit) — and produce concrete consolidation or anti-pattern-fix recommendations.

This skill reviews and recommends only; it does not author new tests or scenario intent (`acceptance-test-automation`, `integration-test-design`) and does not judge whether a failing test is a defect (`test-failure-triage`).

## Boundaries

**Out of scope:**

- Writing new step bindings, fixtures, or integration tests from scratch to cover new behaviour — `acceptance-test-automation`, `integration-test-design`.
- Diagnosing why a specific test is failing — `test-failure-triage`.
- Determining which tests are impacted by a change — `test-change-impact-analysis`.
- Changing business intent, scope, or acceptance criteria of a scenario.

## Trigger Conditions

- The test estate shows signs of duplicated bindings, fixtures, or helper methods.
- Naming, structure, or diagnostic output has become inconsistent across the suite.
- Execution time or flakiness has become a concern.
- `acceptance-test-automation` or `integration-test-design` flags a maintainability concern discovered while implementing a scenario, and asks for a broader review before refactoring.

## Expected Inputs

- The test project(s) or specific files under review, e.g. [Defra.Imports.Specs](../../../tests/Defra.Imports.Specs/), [Defra.Imports.IntegrationTests](../../../tests/Defra.Imports.IntegrationTests/), [Defra.Imports.Scenarios](../../../tests/Defra.Imports.Scenarios/), or a unit test project owned by another agent.
- Any known pain points (slow suite, flaky tests, recent duplication reports).

## Review Dimensions

For each dimension, look for the anti-pattern indicator and recommend the paired fix:

| Dimension | Anti-pattern indicator | Recommended fix |
| --- | --- | --- |
| Duplication | Near-identical step bindings, fixtures, or helper methods | Consolidate into one reusable, parameterised implementation |
| Determinism & execution time | Sleeping/waiting instead of waiting for a condition | Replace with a condition-based wait |
| Portability | Hard-coded environment-specific identifiers | Extract to configuration or fixture setup |
| Separation of concerns | Business rules embedded in automation code | Escalate to the Product Analyst; automation should assert behaviour, not encode rules |
| Scenario integrity | Feature files modified to compensate for an implementation defect | Escalate per Escalation Guidance; do not accept the workaround |
| Production boundary | Production code modified from within a test-authoring task | Escalate to the relevant Developer agent |
| Naming & structure | Inconsistent naming or structure across the suite | Align to established repository naming/structure conventions |
| Diagnostics | Unclear or unhelpful failure output | Improve assertion messages/logging so failures are actionable |

## Process

1. Scope the review to the file(s), project, or estate identified.
2. Scan against each Review Dimension above.
3. For each finding, capture: location, dimension, why it's a problem, and the concrete recommendation.
4. Prioritise duplication and diagnostic-quality findings highest — they compound across the largest number of future tests.
5. Hand findings back to the calling skill/agent — this skill does not itself decide whether business behaviour needs to change.

## Output Standards

A findings table: **Location** / **Finding** / **Dimension** / **Recommendation** / **Priority (High/Medium/Low)**.

State "None identified" explicitly if no issues are found rather than omitting the section.

## Escalation Guidance

- If a finding traces back to a feature file being modified to mask an implementation defect, escalate to the Product Analyst (scenario) or the relevant Developer agent (production defect) rather than accepting the workaround.
- If resolving a finding would require a broad, disruptive refactor, flag it for explicit sign-off rather than performing it unprompted.
