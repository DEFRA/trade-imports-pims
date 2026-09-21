---
name: acceptance-test-automation
description: 'Implement and maintain Reqnroll BDD step bindings, hooks, and fixtures that automate Product-Analyst-approved Given/When/Then scenarios against the model-driven app via Power Playwright. Use when a user story''s BDD scenarios need automated implementation, an existing scenario implementation needs updating as production behaviour evolves, or a scenario needs a limited non-functional wording refinement. Owned by the Quality Engineer agent.'
---

# Acceptance Test Automation

## Intended Agent

This skill may only be used by the Quality Engineer agent.

If the current agent is not the Quality Engineer agent:

- Stop.
- Explain that this skill is owned by the Quality Engineer.
- Recommend handing off to the Quality Engineer agent.

## Purpose

Turn approved BDD Given/When/Then scenarios into passing, maintainable Reqnroll step bindings, hooks, and fixtures — without altering the business intent, scope, or acceptance-criteria coverage the Product Analyst authored.

This skill implements automation only. It does not author scenario business intent (`bdd-scenario-generation`), design integration tests (`integration-test-automation`), or decide whether existing automation needs a maintainability refactor (`test-automation-quality-review`) — though it applies that skill's reuse and anti-pattern principles while writing, and defers to it for broader review.

## Reference documentation

Use the project testing guidance alongside this skill when implementing or updating acceptance automation:

- [docs/testing/acceptance/acceptance-testing-standard.md](../../../docs/testing/acceptance/acceptance-testing-standard.md) — canonical acceptance test standards and conventions.
- [docs/testing/acceptance/power-playwright-reference.md](../../../docs/testing/acceptance/power-playwright-reference.md) — project-specific Power Playwright automation patterns.
- [docs/testing/acceptance/recipes.md](../../../docs/testing/acceptance/recipes.md) — implementable step and hook recipes.
- [docs/testing/acceptance/anti-patterns.md](../../../docs/testing/acceptance/anti-patterns.md) — forbidden patterns and common mistakes to avoid.
- [docs/testing/scenario-builder-reference.md](../../../docs/testing/scenario-builder-reference.md) — shared ScenarioBuilder setup used by acceptance tests.

## Boundaries

**Out of scope:**

- Eliciting requirements, defining acceptance criteria, or authoring scenario business intent — Product Analyst.
- Deciding *whether* an automated scenario is warranted at all — that judgement belongs to `bdd-scenario-generation`'s Applicability Assessment, already applied before this skill is invoked.
- Integration tests validating Dataverse/Logic Apps/Service Bus/external API boundaries directly — `integration-test-automation`.
- Broad consolidation/duplication review across the automation estate — `test-automation-quality-review` (this skill applies its principles locally but does not run the full review).
- Production implementation of business logic (workflows, actions, cloud flows, plug-ins, CWAs) — Developer agents.

## Trigger Conditions

- An approved user story has BDD scenarios (`.feature` files) with no automated implementation.
- An existing scenario implementation needs updating because production behaviour has changed.
- A scenario's wording is causing automation ambiguity and needs a permitted non-functional refinement.

## Expected Inputs

- The work item and its acceptance criteria (for traceability).
- The approved `.feature` file(s) — the source of truth for scenario intent.
- The existing automation estate under [tests](../../../tests/):
  - [Defra.Imports.Specs](../../../tests/Defra.Imports.Specs/) — `Features/`, `StepDefinitions/`, `Hooks/`, `Model/`, `Services/`, `Transformations/`, `environment.json`. Framework: MSTest via `Reqnroll.MSTest`.
  - [Defra.Imports.Scenarios](../../../tests/Defra.Imports.Scenarios/) — shared scenario/data-builder library (`ScenarioBuilder`, `Bogus`). Follow `scenario-builder-implementation` when creating or extending a builder here.

Treat existing patterns (step binding naming, fixture setup, persona/session handling, data builders) as the established convention — extend and reuse them before introducing new ones.

## Process

1. **Confirm source of truth** — treat the approved `.feature` file as authoritative for scenario intent; do not reinterpret it to make automation pass.
2. **Check for reuse** — search `StepDefinitions/`, `Hooks/`, `Model/`, `Services/`, `Transformations/`, and `Defra.Imports.Scenarios` for an existing binding, fixture, or data builder that already covers the required behaviour before writing a new one.
3. **Implement or update** step bindings, hooks, supporting fixtures/page/service abstractions, and data transformations required by the scenario, following the Automation Principles below.
4. **Apply Permitted Scenario Refinements only where needed** (see below) — never as a substitute for a Product Analyst amendment.
5. **Run the tests** to confirm the new/updated scenario(s) pass before reporting completion. If they can't be run in the current environment, say so explicitly rather than assuming success.
6. **Escalate business-facing issues** discovered during automation per Escalation Guidance, rather than silently reinterpreting the scenario.
7. **Flag maintainability concerns** (duplicated bindings/fixtures, naming drift, complexity) for `test-automation-quality-review` rather than performing an ad hoc broad refactor inline.

## Permitted Scenario Refinements

Limited, non-business-facing refinements are allowed to improve automation reliability, for example:

- Adding quotation marks around string parameters.
- Standardising parameter notation.
- Adjusting wording to align with existing step bindings.
- Removing unnecessary wording that creates binding ambiguity.
- Eliminating repetitive details inferable from scenario context.
- Improving consistency with established repository conventions.

Such refinements must preserve business intent, acceptance criteria coverage, stakeholder readability, and behavioural meaning. They are implementation-supporting changes only.

## Automation Principles

- Prefer reusable step definitions over one-off bindings; extend and parameterise existing bindings rather than duplicating them.
- Keep business language (feature files) clearly separated from implementation (step bindings, services, models).
- Favour deterministic execution — avoid timing-dependent or order-dependent tests; never sleep/wait instead of waiting for a condition.
- Never modify production code, hard-code environment-specific identifiers, embed business rules in automation, or edit a feature file to compensate for an implementation defect — see `test-automation-quality-review`'s anti-patterns for the full list this skill must avoid reproducing.

## Output Standards

- The implemented/updated step bindings, hooks, fixtures, or data transformations, committed against the correct test project location.
- Confirmation the tests were run and passed, or an explicit note if they couldn't be run.
- A short summary of: which scenario(s) were automated/updated, which existing bindings/fixtures were reused, any permitted scenario refinements applied and why, and any escalations raised.

## Escalation Guidance

Do not independently change business requirements, acceptance criteria, user intent, business rules, expected behaviour, or scope.

If automation reveals ambiguity, contradiction, missing information, or a requirement defect:

1. Explain the issue.
2. Recommend a change.
3. Return the scenario to the Product Analyst for approval and amendment.

If the automation is passing but the underlying architecture/contract behaviour is in question, escalate to the Solution Architect rather than guessing.
