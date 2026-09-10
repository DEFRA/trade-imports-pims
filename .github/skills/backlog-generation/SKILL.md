---
name: backlog-generation
description: 'Convert validated requirements into implementation-ready epics, features, user stories and acceptance criteria for Agile delivery teams. Use when decomposing requirements into a backlog structure, writing user stories in "As a / I want / So that" format, defining testable acceptance criteria, identifying dependencies and sequencing, or checking implementation readiness before backlog creation. Follows requirements-discovery; precedes Azure DevOps Boards creation by the Product Analyst agent.'
---

# Backlog Generation

## Purpose

Turn validated, discovered requirements into an implementation-ready backlog structure — epics, features, user stories, and acceptance criteria — that a delivery team can estimate and build with minimal further clarification. This skill decomposes and writes stories; it does not create or update Azure DevOps Boards work items and does not make architecture or technology decisions.

## When to Use

- Candidate requirements (e.g. from the requirements-discovery skill) are ready to be broken down into a backlog.
- A large requirement, epic, or feature needs to be decomposed into smaller, independently deliverable increments.
- User stories exist but lack acceptance criteria, dependency analysis, or sequencing guidance.
- Before handing off to the Product Analyst agent to create or update Azure Boards Features/User Stories.

## Expected Inputs

Validated or candidate requirements, business objectives, existing epics/features, requirements-discovery output — in any combination.

## Working Principles

- Focus on business outcomes, not implementation.
- Optimise for implementation readiness: a story should be estimable and buildable with minimal further clarification.
- Create vertically sliced stories wherever possible — each delivers a thin slice of end-to-end value, not a technical layer.
- Keep stories independently deliverable; avoid stories that depend on same-sprint completion of another unless explicitly sequenced.
- Avoid architecture and technology decisions — flag them as questions or assumptions instead.
- Write in concise British English.

## Traceability

Maintain an explicit chain from business objective to acceptance criteria:

```
Business Objective → Epic → Feature → User Story → Acceptance Criteria
```

Every user story must be traceable to a parent feature, then a parent epic, and ultimately to the business objective it serves. If a story cannot be traced, flag it as a gap rather than inventing a parent.

## Procedure

1. **Confirm the business objective(s)** the requirements serve. If unclear, treat this as a blocking open question rather than assuming.
2. **Draft the epic(s)** — one epic per distinct business outcome. Each epic states the outcome, not a solution.
3. **Draft features** for every epic — decompose each epic into one or more features grouping related stories. Every user story must sit under a feature; do not attach a story directly to an epic.
4. **Decompose into user stories**:
   - Prefer vertical slices (thin end-to-end value) over horizontal/technical slices.
   - Split by workflow step, business rule variation, user role, or data variation — not by technical layer.
   - Each story should be small enough to complete within a single iteration.
5. **Write each user story** using the format:
   ```
   As a <role>
   I want <capability>
   So that <benefit>
   ```
6. **Write acceptance criteria** for each story (see standards below).
7. **Identify dependencies** — between stories, on other teams, systems, data, or decisions. State the direction of the dependency (X must complete before Y).
8. **Recommend a delivery sequence** — order stories/features by dependency and value, and flag any that could run in parallel.
9. **Surface assumptions** made during decomposition.
10. **Identify missing information** that would block implementation — do not silently fill gaps with invented detail.

## Acceptance Criteria Standards

Acceptance criteria must:

- Be testable — a tester can verify pass/fail without asking the author for clarification.
- Be observable — describe a visible outcome or system state, not an internal mechanism.
- Be measurable where possible — use concrete values, thresholds, or examples rather than vague terms like "quickly" or "appropriately".
- Be written from a business perspective, in plain language.
- Avoid implementation detail unless the business has explicitly mandated it (e.g. a regulatory data format).

Prefer Given/When/Then or a plain testable statement — either is acceptable as long as the criteria above are met.

## Output Format

Produce output using these headings, in order:

1. Epic
2. Features
3. User Stories
4. Acceptance Criteria
5. Assumptions
6. Dependencies
7. Sequencing Recommendations
8. Remaining Questions

Leave a section explicitly stating "None identified" rather than omitting it. Present each user story immediately followed by its own acceptance criteria for readability, in addition to the consolidated headings above where useful for a larger backlog.

## Success Criteria

This skill succeeds when a delivery team can estimate and implement every story with minimal further clarification, full traceability back to the business objective is preserved, and any remaining gaps are explicit rather than silently assumed.

## Handoff

This skill produces backlog *content*, not Azure DevOps Boards work items. To create or update Epics/Features/User Stories in Azure Boards (including `AB#` linkage per [AGENTS.md](../../../AGENTS.md)), hand off to the Product Analyst agent, which owns Azure DevOps CLI operations and INVEST quality checks.
