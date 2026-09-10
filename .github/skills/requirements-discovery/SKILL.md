---
name: requirements-discovery
description: 'Analyse stakeholder requests, business problem statements, meeting notes, workshop outputs, vision statements and epics to discover complete, testable, implementation-ready requirements. Use when eliciting requirements, refining a vague ask, identifying stakeholders/scope/assumptions/constraints/dependencies/risks, detecting ambiguity or conflicting requirements, generating clarifying questions, or separating business requirements from implementation ideas. Precedes backlog creation.'
---

# Requirements Discovery

## Purpose

Turn raw business input into a structured requirements discovery output that a delivery team can act on with minimal further discovery. This skill analyses; it does not design solutions, choose technology, write code, or define test strategy.

## When to Use

- A stakeholder request, problem statement, meeting/workshop note, vision statement, or epic needs to be turned into requirements.
- An existing requirement is vague, ambiguous, or has conflicting parts that need to be surfaced.
- Before creating or refining Azure DevOps Features/User Stories (see the Product Analyst agent), to ensure the requirement is analysed first.

## Expected Inputs

Stakeholder requests, business problem statements, meeting notes, workshop notes, existing requirements, vision statements, epics — in any combination, however informal.

## Working Principles

- Prefer clarification over assumptions; explicitly flag uncertainty rather than silently resolving it.
- Focus on business outcomes, not features requested.
- Distinguish facts (stated directly) from assumptions (inferred).
- Do not make architecture or implementation decisions — flag implementation ideas found in the input and move them out of the requirements.
- Challenge the proposed solution where the underlying business problem is unclear: ask "what problem does this solve?" before accepting the ask at face value.
- Write in concise British English.

## Procedure

1. **Read the input** in full before analysing. Note the literal business request separately from any solution ideas embedded in it.
2. **Identify the business objective** — the outcome being sought, not the feature requested.
3. **Identify stakeholders and actors** — who is asking, who is affected, who will use the outcome.
4. **Determine scope** — list what is explicitly in scope and what is explicitly or implicitly out of scope.
5. **Surface assumptions** — anything treated as true but not confirmed by the input.
6. **Surface constraints** — technical, regulatory, organisational, or time constraints mentioned or implied.
7. **Surface dependencies** — other teams, systems, data, or decisions this requirement depends on.
8. **Surface risks** — business risks if the requirement is misunderstood, delayed, or wrong.
9. **Detect ambiguity and conflicts** — flag statements that could be interpreted more than one way, and any contradictions between stated needs.
10. **Draft candidate requirements** — plain-language statements of what must be true when this is delivered, kept free of implementation detail.
11. **Generate clarifying questions** — one per unresolved ambiguity, gap, or assumption that materially affects scope or acceptance criteria.
12. **Recommend next steps** — what should happen before this is ready for backlog creation (e.g. stakeholder confirmation, a workshop, a spike).

## Output Format

Produce output using these headings, in order:

1. Business Objective
2. Stakeholders
3. In Scope
4. Out of Scope
5. Assumptions
6. Constraints
7. Dependencies
8. Risks
9. Open Questions
10. Candidate Requirements
11. Recommended Next Steps

Leave a section explicitly stating "None identified" rather than omitting it — an empty section is a signal in itself.

## Success Criteria

This skill succeeds when a delivery team, reading only the output, understands the business problem, the desired outcome, the scope, the risks, and the unresolved questions — without needing significant further discovery.

## Handoff

This skill produces analysis, not backlog items. To turn Candidate Requirements into Azure DevOps Epics/Features/User Stories, hand off to the Product Analyst agent, which owns backlog creation, INVEST quality checks, and Azure Boards CLI operations.
