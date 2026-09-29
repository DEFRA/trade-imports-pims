---
name: test-failure-triage
description: 'Diagnose a failing or flaky automated test and classify its root cause as a requirement defect, BDD scenario defect, test implementation defect, production implementation defect, or environmental/configuration issue, then recommend the correct owner to route it to. Use whenever a test fails and the cause is not yet known. Reusable by any agent investigating test failures.'
---

# Test Failure Triage

## Intended Agent

This skill is primarily used by the Quality Engineer agent, but is reusable by any agent investigating a failing or flaky automated test.

## Purpose

Determine the true root cause of a failing or flaky test so the fix is routed to the correct owner, rather than assumed to be a production defect by default.

This skill classifies and routes only — it does not implement the fix. The fix is carried out by whichever skill/agent owns the classified cause (see table below).

## Boundaries

**Out of scope:**

- Implementing the fix for a test implementation defect — hand off to `acceptance-test-automation` / `integration-test-automation`.
- Changing production code — Developer agents.
- Changing business requirements or scenario intent — Product Analyst.
- Reviewing test code for general maintainability outside the specific failure — `test-automation-quality-review`.
- Scoping which tests a change impacts — `test-change-impact-analysis`.

## Trigger Conditions

- A test is failing and the cause is not yet established.
- A test is flaky (intermittent failure) and needs root-cause classification before being trusted again.
- A pull request or CI run reports test failures needing triage before merge.

## Expected Inputs

- The failing test's output/log/stack trace.
- The scenario or test case under investigation, and its acceptance criteria.
- Recent related changes (see `test-change-impact-analysis` output, if available).
- Environment/configuration context (which environment the test ran against).

## Classification Categories & Routing

| Category | Description | Owner |
| --- | --- | --- |
| Requirement defect | The requirement itself is wrong, ambiguous, or incomplete | Product Analyst |
| BDD scenario defect | The scenario is written incorrectly relative to a correct requirement | Product Analyst |
| Test implementation defect | The automation (step binding, fixture, integration test) doesn't correctly implement a correct scenario | `acceptance-test-automation` / `integration-test-automation` |
| Production implementation defect | The system under test doesn't behave as the correct requirement/scenario specifies | Developer agent |
| Environmental/configuration issue | The failure is caused by test data, environment state, or configuration rather than any of the above | Environment/configuration owner |

## Process

1. **Gather evidence** from the Expected Inputs above.
2. **Test each hypothesis against the table** — do not assume a production defect by default; systematically rule categories in or out using the evidence.
3. **Identify the most likely root cause**, citing the evidence that supports it and what would change the classification.
4. **Recommend routing** to the owner from the table.

## Output Standards

For each failing test:

- **Classification** — the category from the table above, with supporting evidence and reasoning.
- **Recommended owner** — from the table above.
- **Confidence** — how certain the classification is, and what additional evidence would confirm or change it.

When triaging multiple failures, present one row per test: Test / Classification / Evidence / Recommended Owner / Confidence.

## Escalation Guidance

- Never rewrite a test, step binding, or feature file to force a pass instead of routing the defect to its true owner.
- If the failure stems from an architectural contract disagreement (e.g. an integration test failing because the implementation and an ADR disagree), route to the Solution Architect.
