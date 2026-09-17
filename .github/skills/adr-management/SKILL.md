---
name: adr-management
description: 'Create, update, and govern Architecture Decision Records (ADRs) under /architecture/adr. Reviews existing ADRs before drafting a new one, updates or supersedes rather than duplicates, applies the repository ADR template, links related requirements and related/superseded ADRs, and keeps the ADR set internally consistent over time. Use whenever a decision has lasting consequences (new/extended pattern, implementation pattern choice, cross-solution dependency, cost trade-off, security/integration approach) or when maintaining the accuracy of existing architecture artefacts. Reusable by any architecture-focused agent that produces governed decisions.'
---

# ADR Management

## Purpose

Ensure decisions with lasting architectural consequence are recorded once, consistently, and kept authoritative over time — so approved ADRs remain the reliable architectural source of truth (outranking existing code, documentation, requirements, and any proposed design when they conflict). This skill owns the ADR artefact and its lifecycle; it does not perform the underlying options analysis (see `architecture-options-analysis`) or debt classification content (see `risk-and-debt-analysis`), though it consumes their output when populating an ADR's Consequences section.

## Trigger Conditions

Use this skill whenever:

- A decision has lasting consequences: a new or extended pattern, an implementation pattern choice, a cross-solution dependency, a cost trade-off, or a security/integration approach.
- A design review or implementation check reveals that implementation has diverged from an approved ADR.
- An existing ADR needs to change status (e.g. Superseded, Deprecated) as a result of a new decision.

## Expected Inputs

- The decision to be recorded, including its rationale and alternatives (typically from `architecture-options-analysis`).
- The originating work item(s) (Epic/Feature/User Story) and the acceptance criteria the decision supports.
- Existing ADRs under `/architecture/adr` in the affected area.
- Debt classification content (technical debt/architecture debt, Accepted/Deferred/Mitigated/Eliminated/Unknown) from `risk-and-debt-analysis`, where relevant to Consequences.

## Responsibilities

1. **Review before writing.** Search `/architecture/adr` for existing decisions in the affected area before drafting a new ADR. Update or supersede an existing ADR where the decision has changed, rather than creating a duplicate.
2. **Use the repository template.** Use [0000-template.md](../../../architecture/adr/0000-template.md) as the starting point for every new ADR (copy it to the next numbered file, e.g. `architecture/adr/0001-title.md`).
3. **Populate every section:**
   - Title and status (Proposed / Accepted / Superseded / Deprecated).
   - Context — the problem and constraints that led to the decision.
   - Decision — what was decided.
   - Alternatives considered — and why they were rejected (draw from `architecture-options-analysis` output).
   - Consequences — positive, negative, and follow-on impacts (including cost, licensing, and both technical debt and architecture debt, each classified as Accepted/Deferred/Mitigated/Eliminated/Unknown — draw from `risk-and-debt-analysis`).
   - Related requirement(s) and related ADRs (including any ADR this one supersedes).
4. **Link superseded ADRs clearly** — when an ADR replaces another, update the superseded ADR's status and add an explicit link in both directions.
5. **Maintain artefact accuracy over time** (without taking on routine document administration): review SADs/diagrams/models when a major decision changes and update, supersede, archive, or retire artefacts no longer accurate.
6. **Surface, don't silently resolve, conflicts.** Where implementation, documentation, requirements, or a proposed design conflicts with an approved ADR, surface the conflict explicitly and recommend a resolution path (update the ADR, correct the artefact/implementation, or escalate for a stakeholder decision) rather than resolving it unilaterally.
7. **Minimise pattern proliferation.** Challenge variation across solutions not justified by a genuine difference in requirements or constraints; prefer consistency over novelty unless there is a measurable benefit to deviating from an existing, approved pattern.

## Workflow

1. Search `/architecture/adr` for ADRs governing the affected area.
2. Decide: new ADR, update to an existing ADR, or supersession.
3. Copy the template (for a new/superseding ADR) and populate every section listed above.
4. Cross-link related requirements and related/superseded ADRs.
5. Set the correct status, and update the status of any ADR being superseded.
6. Where a conflict with an approved ADR was found during any other skill's work, record it and recommend a resolution path rather than resolving it silently.

## Output Standards

Every ADR must contain, in the template structure: Title, Status, Context, Decision, Alternatives Considered, Consequences (with technical debt and architecture debt each classified), Related requirement(s), and Related ADRs (including any it supersedes).

## Escalation Guidance

- Where a proposed decision conflicts with an approved ADR, do not proceed to write a new ADR that silently overrides it — surface the conflict and recommend escalation to stakeholders or the calling agent for resolution.
- Where an ADR is Medium/Low confidence per `architecture-options-analysis`, state this in the ADR's Consequences/assumptions and recommend validation activities rather than presenting the decision as settled.
